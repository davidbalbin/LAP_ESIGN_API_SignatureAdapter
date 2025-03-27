using System;
using System.IO;
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
    /// Implementación de las operaciones relacionadas con carpetas
    /// </summary>
    public class CarpetaOperations : ICarpetaOperations
    {
        private readonly IWatanaHttpClient _httpClient;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CarpetaOperations"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para la comunicación con la API</param>
        public CarpetaOperations(IWatanaHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task<EnviarCarpetaResponse> EnviarCarpetaAsync(EnviarCarpetaRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Validar la solicitud
            ValidarEnviarCarpetaRequest(request);

            // Enviar la solicitud
            return await _httpClient.SendAsync<EnviarCarpetaResponse>(
                HttpMethod.Post,
                ApiOperations.EnviarCarpeta,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ConsultarCarpetaResponse> ConsultarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(carpetaCodigo))
                throw new ArgumentNullException(nameof(carpetaCodigo));

            // Validar el código de carpeta
            Validators.ValidateCarpetaCodigo(carpetaCodigo);

            // Crear la solicitud
            var request = new ConsultarCarpetaRequest
            {
                CarpetaCodigo = carpetaCodigo
            };

            // Enviar la solicitud
            return await _httpClient.SendAsync<ConsultarCarpetaResponse>(
                HttpMethod.Post,
                ApiOperations.ConsultarCarpeta,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<DescargarCarpetaResponse> DescargarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(carpetaCodigo))
                throw new ArgumentNullException(nameof(carpetaCodigo));

            // Validar el código de carpeta
            Validators.ValidateCarpetaCodigo(carpetaCodigo);

            // Crear la solicitud
            var request = new DescargarCarpetaRequest
            {
                CarpetaCodigo = carpetaCodigo
            };

            try
            {
                // Intentar descargar la carpeta como JSON primero
                return await _httpClient.SendAsync<DescargarCarpetaResponse>(
                    HttpMethod.Post,
                    ApiOperations.DescargarCarpeta,
                    request,
                    cancellationToken
                ).ConfigureAwait(false);
            }
            catch (WatanaApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Si el contenido de la respuesta es un ZIP, se habrá lanzado una WatanaApiException
                // al intentar deserializar el ZIP como JSON. En este caso, recomendamos usar el método DescargarCarpetaComoStreamAsync
                throw new WatanaException(
                    "La carpeta excede el tamaño máximo para ser descargada como JSON. " +
                    "Utilice el método DescargarCarpetaComoStreamAsync para descargar la carpeta como un stream.",
                    ex
                );
            }
        }

        /// <inheritdoc/>
        public async Task<Stream> DescargarCarpetaComoStreamAsync(string carpetaCodigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(carpetaCodigo))
                throw new ArgumentNullException(nameof(carpetaCodigo));

            // Validar el código de carpeta
            Validators.ValidateCarpetaCodigo(carpetaCodigo);

            // Crear la solicitud
            var request = new DescargarCarpetaRequest
            {
                CarpetaCodigo = carpetaCodigo
            };

            // Enviar la solicitud
            return await _httpClient.SendAndGetStreamAsync(
                HttpMethod.Post,
                ApiOperations.DescargarCarpeta,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<EliminarCarpetaResponse> EliminarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(carpetaCodigo))
                throw new ArgumentNullException(nameof(carpetaCodigo));

            // Validar el código de carpeta
            Validators.ValidateCarpetaCodigo(carpetaCodigo);

            // Crear la solicitud
            var request = new EliminarCarpetaRequest
            {
                CarpetaCodigo = carpetaCodigo
            };

            // Enviar la solicitud
            return await _httpClient.SendAsync<EliminarCarpetaResponse>(
                HttpMethod.Post,
                ApiOperations.EliminarCarpeta,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Valida que la solicitud de enviar carpeta sea correcta
        /// </summary>
        private void ValidarEnviarCarpetaRequest(EnviarCarpetaRequest request)
        {
            Validators.ValidateCarpetaCodigo(request.CarpetaCodigo);

            if (string.IsNullOrWhiteSpace(request.Titulo))
                throw new WatanaValidationException("El título de la carpeta es obligatorio", nameof(request.Titulo));

            if (request.VigenciaHoras < WatanaConstants.Limites.MinVigenciaHoras || 
                request.VigenciaHoras > WatanaConstants.Limites.MaxVigenciaHoras)
                throw new WatanaValidationException(
                    $"La vigencia en horas debe estar entre {WatanaConstants.Limites.MinVigenciaHoras} y {WatanaConstants.Limites.MaxVigenciaHoras}",
                    nameof(request.VigenciaHoras)
                );

            if (request.Firmante == null)
                throw new WatanaValidationException("El firmante es obligatorio", nameof(request.Firmante));

            if (string.IsNullOrWhiteSpace(request.Firmante.Email))
                throw new WatanaValidationException("El email del firmante es obligatorio", "Firmante.Email");

            if (request.Archivos == null || request.Archivos.Count == 0)
                throw new WatanaValidationException("Debe incluir al menos un archivo", nameof(request.Archivos));

            foreach (var archivo in request.Archivos)
            {
                if (string.IsNullOrWhiteSpace(archivo.Nombre))
                    throw new WatanaValidationException("El nombre del archivo es obligatorio", "Archivo.Nombre");

                if (string.IsNullOrWhiteSpace(archivo.ZipBase64))
                    throw new WatanaValidationException("El contenido del archivo (ZipBase64) es obligatorio", "Archivo.ZipBase64");
            }
        }
    }
}