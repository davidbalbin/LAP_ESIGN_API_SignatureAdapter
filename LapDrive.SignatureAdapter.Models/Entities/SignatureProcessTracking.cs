namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing tracking information for a signature process
/// </summary>
public class SignatureProcessTracking
{
    /// <summary>
    /// Gets or sets the process ID
    /// </summary>
    public string ProcessId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the document ID
    /// </summary>
    public string DocumentId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the web URL
    /// </summary>
    public string WebUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the library name
    /// </summary>
    public string LibraryName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signers
    /// </summary>
    public List<string> Signers { get; set; } = new();

    /// <summary>
    /// Gets or sets the recipients
    /// </summary>
    public List<string> Recipients { get; set; } = new();
}