using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Common
{
    /// <summary>
    /// Configuración de un firmante para solicitudes compuestas
    /// </summary>
    public class FirmanteConfig
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

        /// <summary>
        /// Indica si se debe aplicar sello de tiempo de LLAMA.PE a los documentos firmados por este firmante
        /// </summary>
        [JsonPropertyName("sello_de_tiempo")]
        public bool SelloTiempo { get; set; } = true;

        /// <summary>
        /// Indica si el firmante puede firmar sin necesidad de usar Watana App
        /// </summary>
        /// <remarks>
        /// Si es true, el firmante podrá completar la firma digital desde la web
        /// usando un certificado de Agente Automatizado de LLAMA.PE.
        /// </remarks>
        [JsonPropertyName("solo_firma_electronica_web")]
        public bool SoloFirmaElectronicaWeb { get; set; }

        /// <summary>
        /// Configuración de cada firma que debe realizar este firmante
        /// </summary>
        [JsonPropertyName("firmas")]
        public List<FirmaConfig> Firmas { get; set; } = new List<FirmaConfig>();
    }
}