using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Responses
{
    /// <summary>
    /// Respuesta a la solicitud de enviar una carpeta con documentos para firma
    /// </summary>
    public class EnviarCarpetaResponse
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado de la operación
        /// </summary>
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>
        /// Número de solicitud asignado por Watana
        /// </summary>
        [JsonPropertyName("solicitud_numero")]
        public string SolicitudNumero { get; set; } = string.Empty;

        /// <summary>
        /// Enlace para compartir con el firmante
        /// </summary>
        [JsonPropertyName("enlace_para_firmar")]
        public string EnlaceParaFirmar { get; set; } = string.Empty;
    }
}