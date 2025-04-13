using System.Text.Json.Serialization;
using LapDrive.SignatureAdapter.Models.DTOs.Common;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response;

/// <summary>
/// Document details in a signature process response
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
    /// Gets or sets the document type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SharePoint location information
    /// </summary>
    [JsonIgnore]
    public SharePointLocationInfo Location { get; set; } = new();

    /// <summary>
    /// Gets or sets the library name (for backward compatibility)
    /// </summary>
    [JsonPropertyName("libraryName")]
    public string LibraryName
    {
        get => Location?.LibraryName ?? string.Empty;
        set => Location.LibraryName = value;
    }

    /// <summary>
    /// Gets or sets the web URL (for backward compatibility)
    /// </summary>
    [JsonPropertyName("webUrl")]
    public string WebUrl
    {
        get => Location?.WebUrl ?? string.Empty;
        set => Location.WebUrl = value;
    }
}