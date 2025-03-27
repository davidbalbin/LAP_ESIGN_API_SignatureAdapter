namespace WatanaClient.API.Models.Webhooks
{
    /// <summary>
    /// Resultado del procesamiento de un webhook
    /// </summary>
    public class WebhookResult
    {
        /// <summary>
        /// Tipo de evento procesado
        /// </summary>
        public string Evento { get; set; } = string.Empty;

        /// <summary>
        /// Código de la carpeta asociada al evento
        /// </summary>
        public string CarpetaCodigo { get; set; } = string.Empty;

        /// <summary>
        /// Número de la solicitud asociada al evento
        /// </summary>
        public string SolicitudNumero { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el webhook fue procesado correctamente
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado del procesamiento
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Datos adicionales opcionales (puede ser usado para información específica)
        /// </summary>
        public object? Data { get; set; }
    }
}