using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Signer information from Watana
    /// </summary>
    public class SignerInfo
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [JsonPropertyName("firmante")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        [JsonPropertyName("nombreCompleto")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the state
        /// </summary>
        [JsonPropertyName("estado")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signature date
        /// </summary>
        [JsonPropertyName("fechaFirma")]
        public DateTime? SignatureDate { get; set; }
    }
}