using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Common
{
    /// <summary>
    /// Representa un archivo para las operaciones con la API Watana Firmador
    /// </summary>
    public class Archivo
    {
        /// <summary>
        /// Nombre del archivo PDF
        /// </summary>
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Archivo PDF zipeado en base64
        /// </summary>
        [JsonPropertyName("zip_base64")]
        public string ZipBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Configuración de la representación visual de la firma
        /// </summary>
        /// <remarks>
        /// - Si es null: No habrá representación visual de la firma
        /// - Si es true: El firmante podrá seleccionar manualmente la posición
        /// - Si es un objeto con propiedades: Se usará la configuración especificada
        /// </remarks>
        [JsonPropertyName("firma_visual")]
        public object? FirmaVisual { get; set; }
    }
}