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
    /// Implementación de las operaciones relacionadas con solicitudes de firma
    /// </summary>
    public class SolicitudOperations : ISolicitudOperations
    {
        private readonly IWatanaHttpClient _httpClient;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SolicitudOperations"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para la comunicación con la API</param>
        public SolicitudOperations(IWatanaHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task<PrepararSolicitudResponse> PrepararSolicitudAsync(PrepararSolicitudRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Validar la solicitud
            ValidarPrepararSolicitudRequest(request);

            // Enviar la solicitud
            return await _httpClient.SendAsync<PrepararSolicitudResponse>(
                HttpMethod.Post,
                ApiOperations.PrepararSolicitud,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<EnviarSolicitudResponse> EnviarSolicitudAsync(EnviarSolicitudRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Validar la solicitud
            ValidarEnviarSolicitudRequest(request);

            // Enviar la solicitud
            return await _httpClient.SendAsync<EnviarSolicitudResponse>(
                HttpMethod.Post,
                ApiOperations.EnviarSolicitud,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ConsultarSolicitudResponse> ConsultarSolicitudAsync(string firmaCodigo, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(firmaCodigo))
                throw new ArgumentNullException(nameof(firmaCodigo));

            // Crear la solicitud
            var request = new ConsultarSolicitudRequest
            {
                FirmaCodigo = firmaCodigo
            };

            // Enviar la solicitud
            return await _httpClient.SendAsync<ConsultarSolicitudResponse>(
                HttpMethod.Post,
                ApiOperations.ConsultarSolicitud,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<ConsultarSolicitudResponse> ConsultarSolicitudPorNumeroAsync(string solicitudNumero, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(solicitudNumero))
                throw new ArgumentNullException(nameof(solicitudNumero));

            // Crear la solicitud con el número como código
            var request = new ConsultarSolicitudRequest
            {
                FirmaCodigo = solicitudNumero
            };

            // Enviar la solicitud
            return await _httpClient.SendAsync<ConsultarSolicitudResponse>(
                HttpMethod.Post,
                ApiOperations.ConsultarSolicitud,
                request,
                cancellationToken
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Valida que la solicitud de preparar solicitud sea correcta
        /// </summary>
        private void ValidarPrepararSolicitudRequest(PrepararSolicitudRequest request)
        {
            Validators.ValidateCarpetaCodigo(request.CarpetaCodigo);

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new WatanaValidationException("El nombre de la carpeta es obligatorio", nameof(request.Nombre));

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

        /// <summary>
        /// Valida que la solicitud de enviar solicitud sea correcta
        /// </summary>
        private void ValidarEnviarSolicitudRequest(EnviarSolicitudRequest request)
        {
            Validators.ValidateCarpetaCodigo(request.CarpetaCodigo);

            if (string.IsNullOrWhiteSpace(request.FirmaCodigo))
                throw new WatanaValidationException("El código de firma es obligatorio", nameof(request.FirmaCodigo));

            if (request.VigenciaHoras < WatanaConstants.Limites.MinVigenciaHoras || 
                request.VigenciaHoras > WatanaConstants.Limites.MaxVigenciaHoras)
                throw new WatanaValidationException(
                    $"La vigencia en horas debe estar entre {WatanaConstants.Limites.MinVigenciaHoras} y {WatanaConstants.Limites.MaxVigenciaHoras}",
                    nameof(request.VigenciaHoras)
                );

            if (request.Firmantes == null || request.Firmantes.Count == 0)
                throw new WatanaValidationException("Debe incluir al menos un firmante", nameof(request.Firmantes));

            foreach (var firmante in request.Firmantes)
            {
                if (string.IsNullOrWhiteSpace(firmante.Email))
                    throw new WatanaValidationException("El email del firmante es obligatorio", "Firmante.Email");

                if (firmante.Firmas == null || firmante.Firmas.Count == 0)
                    throw new WatanaValidationException("Debe incluir al menos una firma para el firmante", "Firmante.Firmas");

                foreach (var firma in firmante.Firmas)
                {
                    if (string.IsNullOrWhiteSpace(firma.Archivo))
                        throw new WatanaValidationException("El nombre del archivo a firmar es obligatorio", "Firma.Archivo");
                }
            }
        }
    }
}