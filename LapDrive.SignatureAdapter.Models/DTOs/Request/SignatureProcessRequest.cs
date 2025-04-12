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