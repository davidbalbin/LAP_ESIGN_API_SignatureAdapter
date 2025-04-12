using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Recipient details in a signature process
    /// </summary>
    public class RecipientDetail
    {
        /// <summary>
        /// Gets or sets the recipient's display name
        /// </summary>
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient's email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}