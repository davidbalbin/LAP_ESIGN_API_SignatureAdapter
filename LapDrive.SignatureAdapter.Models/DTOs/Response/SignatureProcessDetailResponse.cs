using System.Text.Json.Serialization;
using LapDrive.SignatureAdapter.Models.Enums;
using LapDrive.SignatureAdapter.Models.Extensions;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Response model with detailed information about a signature process
    /// </summary>
    public class SignatureProcessDetailResponse
    {
        /// <summary>
        /// Gets or sets the process ID
        /// </summary>
        [JsonPropertyName("processId")]
        public string ProcessId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status enum
        /// </summary>
        [JsonIgnore]
        public ProcessStatus StatusEnum { get; set; }

        /// <summary>
        /// Gets the status text
        /// </summary>
        [JsonPropertyName("status")]
        public string Status => StatusEnum.ToDisplayString();

        /// <summary>
        /// Gets or sets the creation date of the process
        /// </summary>
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the subject of the signature process
        /// </summary>
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message of the signature process
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document information
        /// </summary>
        [JsonPropertyName("document")]
        public DocumentDetail Document { get; set; } = new();

        /// <summary>
        /// Gets or sets the signers information
        /// </summary>
        [JsonPropertyName("signers")]
        public List<SignerDetail> Signers { get; set; } = new();

        /// <summary>
        /// Gets or sets the recipients information
        /// </summary>
        [JsonPropertyName("recipients")]
        public List<RecipientDetail>? Recipients { get; set; }
    }

    /// <summary>
    /// Document details in a signature process
    /// </summary>
    public class DocumentDetail
    {
        /// <summary>
        /// Gets or sets the document ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the library name
        /// </summary>
        [JsonPropertyName("libraryName")]
        public string LibraryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the web URL
        /// </summary>
        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document type
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

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