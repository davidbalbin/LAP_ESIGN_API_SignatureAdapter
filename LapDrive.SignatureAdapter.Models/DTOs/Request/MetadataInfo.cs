using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request
{
    /// <summary>
    /// Metadata information for the signature process
    /// </summary>
    public class MetadataInfo
    {
        /// <summary>
        /// Gets or sets the subject of the signature process
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the additional message for the signers
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the request
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}