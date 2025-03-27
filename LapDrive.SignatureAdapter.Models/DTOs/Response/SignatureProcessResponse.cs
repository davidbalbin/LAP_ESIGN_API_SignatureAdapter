using System.Text.Json.Serialization;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response;

/// <summary>
/// Response model for a signature process creation
/// </summary>
public class SignatureProcessResponse
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
    /// Gets or sets the message from the signature provider
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL for signing the document
    /// </summary>
    [JsonPropertyName("signingUrl")]
    public string SigningUrl { get; set; } = string.Empty;
}