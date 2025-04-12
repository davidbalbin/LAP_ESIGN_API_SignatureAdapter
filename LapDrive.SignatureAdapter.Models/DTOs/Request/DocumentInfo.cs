using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request
{
    /// <summary>
    /// Document information for the signature process
    /// </summary>
    public class DocumentInfo
    {
        /// <summary>
        /// Gets or sets the document ID in the library
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the library name containing the document
        /// </summary>
        [JsonPropertyName("libraryName")]
        public string LibraryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the web URL containing the document
        /// </summary>
        [JsonPropertyName("webUrl")]
        public string WebUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the document type (file or folder)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "file";
    }
}