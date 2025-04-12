using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
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
}