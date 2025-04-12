using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request
{
    /// <summary>
    /// Signer information for the signature process
    /// </summary>
    public class SignerInfo
    {
        /// <summary>
        /// Gets or sets the signer's display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signer's email address
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signature placement information
        /// </summary>
        [JsonPropertyName("signature")]
        public SignatureInfo Signature { get; set; } = new();
    }
}