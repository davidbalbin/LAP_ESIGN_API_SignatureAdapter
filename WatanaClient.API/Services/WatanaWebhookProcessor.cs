using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WatanaClient.API.Constants;
using WatanaClient.API.Exceptions;
using WatanaClient.API.Models.Webhooks;
using WatanaClient.API.Serialization;

namespace WatanaClient.API.Services
{
    /// <summary>
    /// Implementación del procesador de webhooks de Watana Firmador
    /// </summary>
    public class WatanaWebhookProcessor : IWatanaWebhookProcessor
    {
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<WatanaWebhookProcessor>? _logger;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaWebhookProcessor"/>
        /// </summary>
        /// <param name="logger">Logger para registrar eventos (opcional)</param>
        public WatanaWebhookProcessor(ILogger<WatanaWebhookProcessor>? logger = null)
        {
            _logger = logger;
            _serializerOptions = JsonSerializationOptions.Default;
        }

        /// <inheritdoc/>
        public Task<WebhookResult> ProcessWebhookAsync(string requestBody)
        {
            if (string.IsNullOrEmpty(requestBody))
                throw new ArgumentNullException(nameof(requestBody));

            try
            {
                // Deserializar el evento genérico para determinar su tipo
                var webhookEvent = JsonSerializer.Deserialize<WebhookEvent>(requestBody, _serializerOptions);
                
                if (webhookEvent == null)
                    throw new WatanaException("No se pudo deserializar el evento de webhook");

                _logger?.LogInformation("Procesando evento de webhook: {Evento}", webhookEvent.Evento);

                WebhookResult result;

                // Procesar según el tipo de evento
                switch (webhookEvent.Evento)
                {
                    case WatanaConstants.EventosWebhook.Firmado:
                        var firmadoEvent = JsonSerializer.Deserialize<FirmadoWebhookEvent>(requestBody, _serializerOptions);
                        result = ProcessSignedDocument(firmadoEvent!);
                        break;

                    case WatanaConstants.EventosWebhook.RechazadoPorFirmante:
                        var rechazadoEvent = JsonSerializer.Deserialize<RechazadoWebhookEvent>(requestBody, _serializerOptions);
                        result = ProcessRejectedDocument(rechazadoEvent!);
                        break;

                    case WatanaConstants.EventosWebhook.Error:
                        var errorEvent = JsonSerializer.Deserialize<ErrorWebhookEvent>(requestBody, _serializerOptions);
                        result = ProcessErrorDocument(errorEvent!);
                        break;

                    default:
                        throw new WatanaException($"Tipo de evento de webhook desconocido: {webhookEvent.Evento}");
                }

                return Task.FromResult(result);
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Error al deserializar el evento de webhook");
                throw new WatanaException("Error al deserializar el evento de webhook", ex);
            }
        }

        /// <summary>
        /// Procesa un evento de documento firmado
        /// </summary>
        /// <param name="webhookEvent">Evento de webhook</param>
        /// <returns>Resultado del procesamiento</returns>
        private WebhookResult ProcessSignedDocument(FirmadoWebhookEvent webhookEvent)
        {
            _logger?.LogInformation("Documento firmado: Carpeta {CarpetaCodigo}, Solicitud {SolicitudNumero}",
                webhookEvent.CarpetaCodigo, webhookEvent.SolicitudNumero);

            return new WebhookResult
            {
                Evento = WatanaConstants.EventosWebhook.Firmado,
                CarpetaCodigo = webhookEvent.CarpetaCodigo,
                SolicitudNumero = webhookEvent.SolicitudNumero,
                Processed = true,
                Message = $"Documento firmado correctamente: {webhookEvent.SolicitudNumero}"
            };
        }

        /// <summary>
        /// Procesa un evento de documento rechazado
        /// </summary>
        /// <param name="webhookEvent">Evento de webhook</param>
        /// <returns>Resultado del procesamiento</returns>
        private WebhookResult ProcessRejectedDocument(RechazadoWebhookEvent webhookEvent)
        {
            _logger?.LogWarning("Documento rechazado: Carpeta {CarpetaCodigo}, Solicitud {SolicitudNumero}, Motivo: {Motivo}",
                webhookEvent.CarpetaCodigo, webhookEvent.SolicitudNumero, webhookEvent.Motivo);

            return new WebhookResult
            {
                Evento = WatanaConstants.EventosWebhook.RechazadoPorFirmante,
                CarpetaCodigo = webhookEvent.CarpetaCodigo,
                SolicitudNumero = webhookEvent.SolicitudNumero,
                Processed = true,
                Message = $"Documento rechazado: {webhookEvent.SolicitudNumero}, Motivo: {webhookEvent.Motivo}"
            };
        }

        /// <summary>
        /// Procesa un evento de error en el documento
        /// </summary>
        /// <param name="webhookEvent">Evento de webhook</param>
        /// <returns>Resultado del procesamiento</returns>
        private WebhookResult ProcessErrorDocument(ErrorWebhookEvent webhookEvent)
        {
            _logger?.LogError("Error en el documento: Carpeta {CarpetaCodigo}, Solicitud {SolicitudNumero}, Error: {Error}, Archivo: {Archivo}",
                webhookEvent.CarpetaCodigo, webhookEvent.SolicitudNumero, webhookEvent.Error, webhookEvent.Archivo);

            return new WebhookResult
            {
                Evento = WatanaConstants.EventosWebhook.Error,
                CarpetaCodigo = webhookEvent.CarpetaCodigo,
                SolicitudNumero = webhookEvent.SolicitudNumero,
                Processed = true,
                Message = $"Error en el documento: {webhookEvent.SolicitudNumero}, {webhookEvent.Error}"
            };
        }
    }
}