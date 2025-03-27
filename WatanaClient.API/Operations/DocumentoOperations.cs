using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Constants;
using WatanaClient.API.Exceptions;
using WatanaClient.API.Http;
using WatanaClient.API.Models.Common;
using WatanaClient.API.Models.Requests;
using WatanaClient.API.Models.Responses;
using WatanaClient.API.Utils;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Implementación de las operaciones relacionadas con documentos individuales
    /// </summary>
    public class DocumentoOperations : IDocumentoOperations
    {
        private readonly IWatanaHttpClient _httpClient;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DocumentoOperations"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API</param>
        public DocumentoOperations(IWatanaHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task<ValidarPdfResponse> ValidarPdfAsync(string pdfZipBase64, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pdfZipBase64))
                throw new ArgumentNullException(nameof(pdfZipBase64));

            // Crear la solicitud
            var request = new ValidarPdfRequest
            {
                ZipBase64 = pdfZipBase64
            };

            try
            {
                // Enviar la solicitud
                return await _httpClient.SendAsync<ValidarPdfResponse>(
                    HttpMethod.Post,
                    ApiOperations.ValidarPdf,
                    request,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            catch (WatanaException ex)
            {
                throw new WatanaException("Error al validar el documento PDF. Este método solo está disponible en entorno de PRODUCCIÓN.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<ValidarPdfResponse> ValidarPdfAsync(byte[] pdfBytes, CancellationToken cancellationToken = default)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentNullException(nameof(pdfBytes), "Los bytes del PDF no pueden ser nulos o vacíos");

            try
            {
                // Comprimir el PDF y convertirlo a Base64
                string zipBase64 = ZipUtils.CompressPdfToZipBase64(pdfBytes);

                // Validar el PDF
                return await ValidarPdfAsync(zipBase64, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (!(ex is WatanaException))
            {
                throw new WatanaException("Error al comprimir y codificar el PDF", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<FirmarPdfResponse> FirmarPdfAsync(FirmarPdfRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(request.ZipBase64))
                throw new WatanaValidationException("El contenido del PDF (ZipBase64) es obligatorio", nameof(request.ZipBase64));

            try
            {
                // Enviar la solicitud
                return await _httpClient.SendAsync<FirmarPdfResponse>(
                    HttpMethod.Post,
                    ApiOperations.FirmarPdf,
                    request,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            catch (WatanaException ex)
            {
                throw new WatanaException("Error al firmar el documento PDF. Este método solo está disponible en entorno de PRODUCCIÓN.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> FirmarPdfAsync(byte[] pdfBytes, FirmaVisualRequest? firmaVisual = null, bool selloTiempo = true, CancellationToken cancellationToken = default)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentNullException(nameof(pdfBytes), "Los bytes del PDF no pueden ser nulos o vacíos");

            try
            {
                // Comprimir el PDF y convertirlo a Base64
                string zipBase64 = ZipUtils.CompressPdfToZipBase64(pdfBytes);

                // Crear la solicitud
                var request = new FirmarPdfRequest
                {
                    ZipBase64 = zipBase64,
                    SelloTiempo = selloTiempo,
                    FirmaVisual = firmaVisual
                };

                // Firmar el PDF
                var response = await FirmarPdfAsync(request, cancellationToken).ConfigureAwait(false);

                // Descomprimir el PDF firmado
                return ZipUtils.DecompressZipBase64ToPdf(response.ZipBase64);
            }
            catch (Exception ex) when (!(ex is WatanaException))
            {
                throw new WatanaException("Error al comprimir, firmar o descomprimir el PDF", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<SellarPdfResponse> SellarPdfAsync(string pdfZipBase64, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pdfZipBase64))
                throw new ArgumentNullException(nameof(pdfZipBase64));

            // Crear la solicitud
            var request = new SellarPdfRequest
            {
                ZipBase64 = pdfZipBase64
            };

            try
            {
                // Enviar la solicitud
                return await _httpClient.SendAsync<SellarPdfResponse>(
                    HttpMethod.Post,
                    ApiOperations.SellarPdf,
                    request,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            catch (WatanaException ex)
            {
                throw new WatanaException("Error al sellar el documento PDF. Este método solo está disponible en entorno de PRODUCCIÓN.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<byte[]> SellarPdfAsync(byte[] pdfBytes, CancellationToken cancellationToken = default)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                throw new ArgumentNullException(nameof(pdfBytes), "Los bytes del PDF no pueden ser nulos o vacíos");

            try
            {
                // Comprimir el PDF y convertirlo a Base64
                string zipBase64 = ZipUtils.CompressPdfToZipBase64(pdfBytes);

                // Sellar el PDF
                var response = await SellarPdfAsync(zipBase64, cancellationToken).ConfigureAwait(false);

                // Descomprimir el PDF sellado
                return ZipUtils.DecompressZipBase64ToPdf(response.ZipBase64);
            }
            catch (Exception ex) when (!(ex is WatanaException))
            {
                throw new WatanaException("Error al comprimir, sellar o descomprimir el PDF", ex);
            }
        }
    }
}