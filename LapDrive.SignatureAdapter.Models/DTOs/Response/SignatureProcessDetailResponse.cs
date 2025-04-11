using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response;

/// <summary>
/// Response model with detailed information about a signature process
/// </summary>
public class SignatureProcessDetailResponse
{
    /// <summary>
    /// Gets or sets the process ID
    /// </summary>
    [JsonPropertyName("processId")]
    public string ProcessId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the signature process
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date of the process
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the subject of the signature process
    /// </summary>
    [JsonPropertyName("subject")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message of the signature process
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the document information
    /// </summary>
    [JsonPropertyName("document")]
    public DocumentDetail Document { get; set; } = new();

    /// <summary>
    /// Gets or sets the signers information
    /// </summary>
    [JsonPropertyName("signers")]
    public List<SignerDetail> Signers { get; set; } = new();

    /// <summary>
    /// Gets or sets the recipients information
    /// </summary>
    [JsonPropertyName("recipients")]
    public List<RecipientDetail>? Recipients { get; set; }

    /// <summary>
    /// Gets or sets the signing URL
    /// </summary>
    [JsonPropertyName("signingUrl")]
    public string? SigningUrl { get; set; }
}

/// <summary>
/// Document details in a signature process
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
    /// Gets or sets the library name
    /// </summary>
    [JsonPropertyName("libraryName")]
    public string LibraryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the web URL
    /// </summary>
    [JsonPropertyName("webUrl")]
    public string WebUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the document type
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Signer details in a signature process
/// </summary>
public class SignerDetail
{
    /// <summary>
    /// Gets or sets the signer's display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signer's email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signer's status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signer's signature date if signed
    /// </summary>
    [JsonPropertyName("signatureDate")]
    public DateTime? SignatureDate { get; set; }
}

/// <summary>
/// Recipient details in a signature process
/// </summary>
public class RecipientDetail
{
    /// <summary>
    /// Gets or sets the recipient's display name
    /// </summary>
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the recipient's email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class SignatureProcessStatusResponse
{
    public string Estado { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string EnlaceParaFirmar { get; set; } = string.Empty;
    public List<FirmanteInfo> Firmantes { get; set; } = new List<FirmanteInfo>();
}

public class FirmanteInfo
{
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaFirma { get; set; }
}