namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Represents a document location in SharePoint
/// </summary>
public class SharePointLocation
{
    /// <summary>
    /// Gets or sets the library name containing the document
    /// </summary>
    public string LibraryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the web URL containing the document
    /// </summary>
    public string WebUrl { get; set; } = string.Empty;
}