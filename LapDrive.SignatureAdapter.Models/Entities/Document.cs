namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Represents a document for signature
/// </summary>
public class Document
{
    /// <summary>
    /// Gets or sets the document metadata
    /// </summary>
    public DocumentMetadata Metadata { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the document content
    /// </summary>
    public byte[]? Content { get; set; }
}