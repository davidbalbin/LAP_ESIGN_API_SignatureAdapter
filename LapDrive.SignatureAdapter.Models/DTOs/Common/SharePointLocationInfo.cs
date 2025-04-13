using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Common;

/// <summary>
/// SharePoint location information for documents
/// </summary>
public class SharePointLocationInfo
{
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
}