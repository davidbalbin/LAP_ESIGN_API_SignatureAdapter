using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Configuration;
using LapDrive.SignatureAdapter.Models.Entities;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WatanaClient.API;
using WatanaClient.API.Models.Common;
using WatanaClient.API.Models.Requests;
using System.Text;
using WatanaClient.API.Services;
using System.IO.Compression;

namespace LapDrive.SignatureAdapter.Data.Clients.Implementation;

/// <summary>
/// Watana signature provider client implementation
/// </summary>
public class WatanaSignatureProviderClient : ISignatureProviderClient
{
    private readonly IWatanaClient _watanaClient;
    private readonly SignatureProviderOptions _options;
    private readonly ILogger<WatanaSignatureProviderClient> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WatanaSignatureProviderClient"/> class.
    /// </summary>
    /// <param name="watanaClient">The Watana client</param>
    /// <param name="options">The signature provider options</param>
    /// <param name="logger">The logger</param>
    public WatanaSignatureProviderClient(
        IWatanaClient watanaClient,
        IOptions<SignatureProviderOptions> options,
        ILogger<WatanaSignatureProviderClient> logger)
    {
        _watanaClient = watanaClient ?? throw new ArgumentNullException(nameof(watanaClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<string> CreateSignatureProcessAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating signature process with Watana");
            
            // Map document to Watana model
            var isFolder = signatureProcess.Document.Type == Models.Enums.DocumentType.Folder;
            
            if (isFolder)
            {
                // For folders, use the preparar_solicitud and enviar_solicitud approach
                var processId = await PrepareAndSendFolderAsync(signatureProcess, cancellationToken);
                return processId;
            }
            else
            {
                // For single files, use the enviar_carpeta approach
                var request = new EnviarCarpetaRequest
                {
                    CarpetaCodigo = signatureProcess.RequestId,
                    Titulo = signatureProcess.Subject,
                    Descripcion = signatureProcess.Message,
                    VigenciaHoras = 48, // Default 48 hours
                    Firmante = new Firmante
                    {
                        Email = signatureProcess.Signers.First().Email,
                        NombreCompleto = signatureProcess.Signers.First().DisplayName
                    },
                    Archivos = new List<Archivo>
                    {
                        new Archivo
                        {
                            Nombre = signatureProcess.Document.Name ?? throw new ArgumentNullException(nameof(signatureProcess.Document.Name)),
                            ZipBase64 = Convert.ToBase64String(ZipDocument(
                                signatureProcess.Document.Content ?? throw new ArgumentNullException(nameof(signatureProcess.Document.Content)),
                                signatureProcess.Document.Name ?? throw new ArgumentNullException(nameof(signatureProcess.Document.Name)))),
                            FirmaVisual = GetFirmaVisual(signatureProcess.Signers.First().SignatureInfo)
                        }
                    }
                };
                
                var response = await _watanaClient.Carpetas.EnviarCarpetaAsync(request, cancellationToken);
                
                if (!response.Success)
                {
                    throw new DataException($"Error from Watana API: {response.Mensaje}");
                }
                
                return response.SolicitudNumero;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating signature process with Watana");
            throw new DataException($"Error creating signature process with Watana: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<string> GetSigningUrlAsync(string processId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(processId, nameof(processId));

        try
        {
            _logger.LogInformation("Getting signing URL for process {ProcessId}", processId);

            // Get carpeta response
            var response = await _watanaClient.Carpetas.ConsultarCarpetaAsync(processId, cancellationToken);

            if (!response.Success)
            {
                throw new DataException($"Error consultando carpeta en Watana: {response.Mensaje}");
            }

            // Use the same request to get the signing URL
            var carpetaRequest = new EnviarCarpetaRequest
            {
                CarpetaCodigo = response.SolicitudNumero
            };

            var enlaceResponse = await _watanaClient.Carpetas.EnviarCarpetaAsync(carpetaRequest, cancellationToken);

            if (!enlaceResponse.Success)
            {
                throw new DataException($"Error obteniendo enlace de firma en Watana: {enlaceResponse.Mensaje}");
            }

            return enlaceResponse.EnlaceParaFirmar ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting signing URL for process {ProcessId}", processId);
            throw new DataException($"Error getting signing URL: {ex.Message}", ex);
        }
    }

    private async Task<string> PrepareAndSendFolderAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken)
    {
        // Step 1: Prepare the request (upload documents)
        var archivos = new List<Archivo>();
        
        ArgumentNullException.ThrowIfNull(signatureProcess.Document?.Content, nameof(signatureProcess.Document.Content));
        using (var zipArchive = new ZipArchive(new MemoryStream(signatureProcess.Document.Content), ZipArchiveMode.Read))
        {
            foreach (var entry in zipArchive.Entries)
            {
                if (entry.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    using var entryStream = entry.Open();
                    using var memoryStream = new MemoryStream();
                    await entryStream.CopyToAsync(memoryStream, cancellationToken);
                    var fileBytes = memoryStream.ToArray();
                    
                    archivos.Add(new Archivo
                    {
                        Nombre = entry.Name,
                        ZipBase64 = Convert.ToBase64String(ZipDocument(fileBytes, entry.Name))
                    });
                }
            }
        }
        
        var prepararRequest = new PrepararSolicitudRequest
        {
            CarpetaCodigo = signatureProcess.RequestId,
            Nombre = signatureProcess.Subject,
            Archivos = archivos
        };
        
        var prepararResponse = await _watanaClient.Solicitudes.PrepararSolicitudAsync(prepararRequest, cancellationToken);
        
        if (!prepararResponse.Success)
        {
            throw new DataException($"Error preparing folder with Watana: {prepararResponse.Mensaje}");
        }
        
        // Step 2: Send the signing request
        var firmantes = new List<FirmanteConfig>();
        
        foreach (var signer in signatureProcess.Signers)
        {
            var firmas = new List<FirmaConfig>();
            
            foreach (var archivo in prepararResponse.Archivos)
            {
                firmas.Add(new FirmaConfig
                {
                    Archivo = archivo.Nombre,
                    UbicacionX = signer.SignatureInfo.X ?? 50,
                    UbicacionY = signer.SignatureInfo.Y ?? 50,
                    Largo = 300,
                    Alto = 40,
                    Pagina = signer.SignatureInfo.PageNumber,
                    Texto = "Firmado digitalmente por: <FIRMANTE>\r\n<FECHA>",
                    Motivo = "Firma Digital"
                });
            }
            
            firmantes.Add(new FirmanteConfig
            {
                Email = signer.Email,
                NombreCompleto = signer.DisplayName,
                SelloTiempo = true,
                Firmas = firmas
            });
        }
        
        var enviarRequest = new EnviarSolicitudRequest
        {
            CarpetaCodigo = signatureProcess.RequestId,
            FirmaCodigo = $"F{DateTime.Now:yyyyMMddHHmmss}",
            VigenciaHoras = 48,
            Firmantes = firmantes
        };
        
        var enviarResponse = await _watanaClient.Solicitudes.EnviarSolicitudAsync(enviarRequest, cancellationToken);
        
        if (!enviarResponse.Success)
        {
            throw new DataException($"Error sending folder with Watana: {enviarResponse.Mensaje}");
        }
        
        return enviarResponse.FirmaCodigo;
    }

    private FirmaVisual GetFirmaVisual(SignatureInfo signatureInfo)
    {
        // Si hay una posición predefinida, permite al firmante posicionar manualmente
        if (!string.IsNullOrEmpty(signatureInfo.Position))
        {
            return new FirmaVisual
            {
                UbicacionX = 0,
                UbicacionY = 0,
                Largo = 300,
                Alto = 40,
                Pagina = signatureInfo.PageNumber,
                Texto = "Firmado digitalmente por: <FIRMANTE>\r\n<FECHA>"
            };
        }
        
        // Si se proporcionan coordenadas X e Y específicas
        if (signatureInfo.X.HasValue && signatureInfo.Y.HasValue)
        {
            return new FirmaVisual
            {
                UbicacionX = signatureInfo.X.Value,
                UbicacionY = signatureInfo.Y.Value,
                Largo = 300,
                Alto = 40,
                Pagina = signatureInfo.PageNumber,
                Texto = "Firmado digitalmente por: <FIRMANTE>\r\n<FECHA>"
            };
        }
        
        // Posicionamiento automático por defecto
        return new FirmaVisual
        {
            UbicacionX = 50,
            UbicacionY = 50,
            Largo = 300,
            Alto = 40,
            Pagina = signatureInfo.PageNumber,
            Texto = "Firmado digitalmente por: <FIRMANTE>\r\n<FECHA>"
        };
    }

    private byte[] ZipDocument(byte[] documentContent, string documentName)
    {
        ArgumentNullException.ThrowIfNull(documentContent);
        ArgumentException.ThrowIfNullOrEmpty(documentName);

        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry(documentName, System.IO.Compression.CompressionLevel.Optimal);
            using var entryStream = entry.Open();
            entryStream.Write(documentContent, 0, documentContent.Length);
        }
        
        return memoryStream.ToArray();
    }
}