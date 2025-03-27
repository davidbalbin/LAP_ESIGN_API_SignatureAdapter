using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Constants;
using WatanaClient.API.Exceptions;
using WatanaClient.API.Http;
using WatanaClient.API.Models.Requests;
using WatanaClient.API.Models.Responses;
using WatanaClient.API.Utils;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Implementación de las operaciones relacionadas con validación de documentos
    /// </summary>
    public class ValidacionOperations : IValidacionOperations
    {
        private readonly IWatanaHttpClient _httpClient;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ValidacionOperations"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API</param>
        public ValidacionOperations(IWatanaHttpClient httpClient)
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
    }
}