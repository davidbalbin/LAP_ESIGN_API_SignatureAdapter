using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Represents the metadata of a document
/// </summary>
public class DocumentMetadata
{
    /// <summary>
    /// Gets or sets the document ID
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the document name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the document type
    /// </summary>
    public DocumentType Type { get; set; } = DocumentType.File;
    
    /// <summary>
    /// Gets or sets the SharePoint location of the document
    /// </summary>
    public SharePointLocation Location { get; set; } = new();
}