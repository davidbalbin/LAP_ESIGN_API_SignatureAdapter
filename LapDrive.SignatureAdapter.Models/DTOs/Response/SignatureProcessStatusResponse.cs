using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Response model for signature process status
    /// </summary>
    public class SignatureProcessStatusResponse
    {
        /// <summary>
        /// Gets or sets the current state
        /// </summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signing URL
        /// </summary>
        [JsonPropertyName("enlaceParaFirmar")]
        public string EnlaceParaFirmar { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of signers
        /// </summary>
        [JsonPropertyName("firmantes")]
        public List<FirmanteInfo> Firmantes { get; set; } = new List<FirmanteInfo>();
    }

    /// <summary>
    /// Signer information from Watana
    /// </summary>
    public class FirmanteInfo
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        [JsonPropertyName("nombreCompleto")]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signature date
        /// </summary>
        [JsonPropertyName("fechaFirma")]
        public DateTime? FechaFirma { get; set; }
    }
}