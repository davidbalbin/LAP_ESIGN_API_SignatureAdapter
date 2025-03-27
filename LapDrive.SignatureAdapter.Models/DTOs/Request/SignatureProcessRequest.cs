using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Request;

/// <summary>
/// Request model for creating a signature process
/// </summary>
public class SignatureProcessRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the request (optional, generated if not provided)
    /// </summary>
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets or sets the metadata for the signature process
    /// </summary>
    [JsonPropertyName("metadata")]
    public MetadataInfo Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the document to be signed
    /// </summary>
    [JsonPropertyName("document")]
    public DocumentInfo Document { get; set; } = new();

    /// <summary>
    /// Gets or sets the signers for the document
    /// </summary>
    [JsonPropertyName("signers")]
    public List<SignerInfo> Signers { get; set; } = new();

    /// <summary>
    /// Gets or sets the recipients who will receive the signed document
    /// </summary>
    [JsonPropertyName("recipients")]
    public List<RecipientInfo>? Recipients { get; set; }
}

/// <summary>
/// Metadata information for the signature process
/// </summary>
public class MetadataInfo
{
    /// <summary>
    /// Gets or sets the subject of the signature process
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the additional message for the signers
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date of the request
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

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

/// <summary>
/// Signer information for the signature process
/// </summary>
public class SignerInfo
{
    /// <summary>
    /// Gets or sets the signer's display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signer's email address
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signature placement information
    /// </summary>
    [JsonPropertyName("signature")]
    public SignatureInfo Signature { get; set; } = new();
}

/// <summary>
/// Signature placement information
/// </summary>
public class SignatureInfo
{
    /// <summary>
    /// Gets or sets the X coordinate for the signature
    /// </summary>
    [JsonPropertyName("x")]
    public int? X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate for the signature
    /// </summary>
    [JsonPropertyName("y")]
    public int? Y { get; set; }

    /// <summary>
    /// Gets or sets the page number for the signature
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

/// <summary>
/// Recipient information for the signature process
/// </summary>
public class RecipientInfo
{
    /// <summary>
    /// Gets or sets the recipient's display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recipient's email address
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}