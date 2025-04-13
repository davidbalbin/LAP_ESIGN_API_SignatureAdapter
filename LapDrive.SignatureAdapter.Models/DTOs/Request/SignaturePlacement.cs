using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request
{
    /// <summary>
    /// Defines the placement and visual configuration of a signature within a document
    /// </summary>
    public class SignaturePlacement
    {
        /// <summary>
        /// Gets or sets the X coordinate for the signature placement
        /// </summary>
        [JsonPropertyName("x")]
        public int? X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate for the signature placement
        /// </summary>
        [JsonPropertyName("y")]
        public int? Y { get; set; }

        /// <summary>
        /// Gets or sets the page number where the signature will be placed
        /// </summary>
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the predefined position for the signature
        /// </summary>
        /// <remarks>
        /// Valid values: "topLeft", "topCenter", "topRight", "bottomLeft", "bottomCenter", "bottomRight"
        /// </remarks>
        [JsonPropertyName("position")]
        public string? Position { get; set; }
    }
}