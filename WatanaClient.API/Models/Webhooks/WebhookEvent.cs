using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Webhooks
{
    /// <summary>
    /// Evento base para las notificaciones de webhook
    /// </summary>
    public class WebhookEvent
    {
        /// <summary>
        /// Tipo de evento (firmado, rechazado-por-firmante, error)
        /// </summary>
        [JsonPropertyName("evento")]
        public string Evento { get; set; } = string.Empty;

        /// <summary>
        /// Código de la carpeta asociada al evento
        /// </summary>
        [JsonPropertyName("carpeta_codigo")]
        public string CarpetaCodigo { get; set; } = string.Empty;

        /// <summary>
        /// Número de la solicitud asociada al evento
        /// </summary>
        [JsonPropertyName("solicitud_numero")]
        public string SolicitudNumero { get; set; } = string.Empty;
    }
}