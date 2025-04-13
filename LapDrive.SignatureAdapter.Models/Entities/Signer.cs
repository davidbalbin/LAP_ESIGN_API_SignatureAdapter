namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing a document signer
/// </summary>
public class Signer
{
    /// <summary>
    /// Gets or sets the signer's display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the signer's email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the signature placement configuration
    /// </summary>
    public SignaturePlacement SignaturePlacement { get; set; } = new();
}