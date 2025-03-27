using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Webhooks
{
    /// <summary>
    /// Evento de documento firmado correctamente
    /// </summary>
    public class FirmadoWebhookEvent : WebhookEvent
    {
        // Hereda todas las propiedades de WebhookEvent
        // No tiene propiedades adicionales
    }

    /// <summary>
    /// Evento de documento rechazado por el firmante
    /// </summary>
    public class RechazadoWebhookEvent : WebhookEvent
    {
        /// <summary>
        /// Motivo del rechazo indicado por el firmante
        /// </summary>
        [JsonPropertyName("motivo")]
        public string Motivo { get; set; } = string.Empty;
    }

    /// <summary>
    /// Evento de error durante el proceso de firma
    /// </summary>
    public class ErrorWebhookEvent : WebhookEvent
    {
        /// <summary>
        /// Descripción del error ocurrido
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del archivo que causó el error
        /// </summary>
        [JsonPropertyName("archivo")]
        public string Archivo { get; set; } = string.Empty;
    }
}