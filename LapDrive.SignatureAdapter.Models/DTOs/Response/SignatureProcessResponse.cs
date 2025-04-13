using System.Text.Json.Serialization;
using LapDrive.SignatureAdapter.Models.Enums;
using LapDrive.SignatureAdapter.Models.Extensions;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Response model for signature process creation
    /// </summary>
    public class SignatureProcessResponse
    {
        /// <summary>
        /// Gets or sets the process ID
        /// </summary>
        [JsonPropertyName("processId")]
        public string ProcessId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status enum internally
        /// </summary>
        [JsonIgnore]
        public SignatureProcessStatus StatusEnum { get; set; }

        /// <summary>
        /// Gets the status text
        /// </summary>
        [JsonPropertyName("status")]
        public string Status => SignatureStatusExtensions.ToDisplayString(StatusEnum);

        /// <summary>
        /// Gets or sets the response message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}