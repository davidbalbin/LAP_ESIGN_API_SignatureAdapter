using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Common
{
    /// <summary>
    /// Representa un firmante para las operaciones con la API Watana Firmador
    /// </summary>
    public class Firmante
    {
        /// <summary>
        /// Correo electrónico del firmante
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Nombre completo del firmante (opcional)
        /// </summary>
        [JsonPropertyName("nombre_completo")]
        public string? NombreCompleto { get; set; }
    }
}