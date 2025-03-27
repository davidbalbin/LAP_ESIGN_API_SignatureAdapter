using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing a document for signature
/// </summary>
public class Document
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
    /// Gets or sets the library name containing the document
    /// </summary>
    public string LibraryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the web URL containing the document
    /// </summary>
    public string WebUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the document type
    /// </summary>
    public DocumentType Type { get; set; } = DocumentType.File;
    
    /// <summary>
    /// Gets or sets the document content
    /// </summary>
    public byte[]? Content { get; set; }
}