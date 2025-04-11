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
            
            // Always use preparar_solicitud and enviar_solicitud approach for both single files and folders
            var processId = await PrepareAndSendFolderAsync(signatureProcess, cancellationToken);
            return processId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating signature process with Watana");
            throw new DataException($"Error creating signature process with Watana: {ex.Message}", ex);
        }
    }

    private async Task<string> PrepareAndSendFolderAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken)
    {
        // Step 1: Prepare the request (upload documents)
        var archivos = new List<Archivo>();
        
        ArgumentNullException.ThrowIfNull(signatureProcess.Document?.Content, nameof(signatureProcess.Document.Content));
        
        if (signatureProcess.Document.Type == Models.Enums.DocumentType.Folder)
        {
            // Process ZIP file containing multiple PDFs
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
        }
        else
        {
            // Process single PDF file
            ArgumentException.ThrowIfNullOrEmpty(signatureProcess.Document.Name);
            archivos.Add(new Archivo
            {
                Nombre = signatureProcess.Document.Name,
                ZipBase64 = Convert.ToBase64String(ZipDocument(signatureProcess.Document.Content, signatureProcess.Document.Name))
            });
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
                    Largo = 160,
                    Alto = 40,
                    Pagina = signer.SignatureInfo.PageNumber,
                    Texto = "Firmado por: <FIRMANTE>\r\n<ORGANIZACION>\r\n<TITULO>\r\n<CORREO>\r\nMotivo: Firma Digital\r\nFecha: <FECHA>",
                    Motivo = "Firma Digital",
                    ImageZipBase64 = GetDefaultLogoBase64()
                });
            }
            
            firmantes.Add(new FirmanteConfig
            {
                Email = signer.Email,
                NombreCompleto = signer.DisplayName,
                SelloTiempo = false,
                Firmas = firmas
            });
        }
        
        var enviarRequest = new EnviarSolicitudRequest
        {
            CarpetaCodigo = signatureProcess.RequestId,
            FirmaCodigo = $"F{DateTime.Now:yyyyMMddHHmmss}",
            VigenciaHoras = 168,
            Firmantes = firmantes
        };
        
        var enviarResponse = await _watanaClient.Solicitudes.EnviarSolicitudAsync(enviarRequest, cancellationToken);
        
        if (!enviarResponse.Success)
        {
            throw new DataException($"Error sending folder with Watana: {enviarResponse.Mensaje}");
        }
        
        return enviarResponse.FirmaCodigo;
    }

    private string GetDefaultLogoBase64()
    {
        var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "logo.jpeg");
        if (!File.Exists(logoPath))
        {
            _logger.LogWarning("Logo file not found at {Path}", logoPath);
            return string.Empty;
        }

        try
        {
            var imageBytes = File.ReadAllBytes(logoPath);
            return Convert.ToBase64String(ZipDocument(imageBytes, "logo.jpeg"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading logo file");
            return string.Empty;
        }
    }

    private FirmaVisual GetFirmaVisual(SignatureInfo signatureInfo)
    {
        var firmaVisual = new FirmaVisual
        {
            Largo = 160,
            Alto = 40,
            Pagina = signatureInfo.PageNumber,
            Texto = "Firmado por: <FIRMANTE>\r\n<ORGANIZACION>\r\n<TITULO>\r\n<CORREO>\r\nMotivo: Firma Digital\r\nFecha: <FECHA>",
            ImageZipBase64 = GetDefaultLogoBase64()
        };

        // Si hay una posición predefinida, permite al firmante posicionar manualmente
        if (!string.IsNullOrEmpty(signatureInfo.Position))
        {
            firmaVisual.UbicacionX = 0;
            firmaVisual.UbicacionY = 0;
        }
        // Si se proporcionan coordenadas X e Y específicas
        else if (signatureInfo.X.HasValue && signatureInfo.Y.HasValue)
        {
            firmaVisual.UbicacionX = signatureInfo.X.Value;
            firmaVisual.UbicacionY = signatureInfo.Y.Value;
        }
        // Posicionamiento automático por defecto
        else
        {
            firmaVisual.UbicacionX = 50;
            firmaVisual.UbicacionY = 50;
        }

        return firmaVisual;
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