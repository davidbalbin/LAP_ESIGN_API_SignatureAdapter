using System.Text.Json.Serialization;
using LapDrive.SignatureAdapter.Models.Enums;
using LapDrive.SignatureAdapter.Models.Extensions;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Signer details in a signature process
    /// </summary>
    public class SignerDetail
    {
        /// <summary>
        /// Gets or sets the signer's display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signer's email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the signer's status
        /// </summary>
        [JsonIgnore]
        public SignerStatus StatusEnum { get; set; }

        /// <summary>
        /// Gets the status text
        /// </summary>
        [JsonPropertyName("status")]
        public string Status => StatusEnum.ToDisplayString();

        /// <summary>
        /// Gets or sets the signer's signature date if signed
        /// </summary>
        [JsonPropertyName("signatureDate")]
        public DateTime? SignatureDate { get; set; }
    }
}