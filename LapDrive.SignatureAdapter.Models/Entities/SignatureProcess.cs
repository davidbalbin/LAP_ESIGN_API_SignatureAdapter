using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing a signature process
/// </summary>
public class SignatureProcess
{
    /// <summary>
    /// Gets or sets the request ID
    /// </summary>
    public string RequestId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the subject
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the document
    /// </summary>
    public Document Document { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the signers
    /// </summary>
    public List<Signer> Signers { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the recipients
    /// </summary>
    public List<Recipient> Recipients { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the status
    /// </summary>
    public SignatureProcessStatus Status { get; set; } = SignatureProcessStatus.Pending;
}