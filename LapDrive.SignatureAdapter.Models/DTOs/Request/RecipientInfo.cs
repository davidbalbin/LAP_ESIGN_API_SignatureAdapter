using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request
{
    /// <summary>
    /// Recipient information for the signature process
    /// </summary>
    public class RecipientInfo
    {
        /// <summary>
        /// Gets or sets the recipient's display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient's email address
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}