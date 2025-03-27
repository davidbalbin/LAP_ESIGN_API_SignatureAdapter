namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing a document recipient
/// </summary>
public class Recipient
{
    /// <summary>
    /// Gets or sets the recipient's display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the recipient's email
    /// </summary>
    public string Email { get; set; } = string.Empty;
}