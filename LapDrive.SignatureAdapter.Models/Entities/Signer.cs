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
    /// Gets or sets the signature information
    /// </summary>
    public SignatureInfo SignatureInfo { get; set; } = new();
}

/// <summary>
/// Entity representing signature information
/// </summary>
public class SignatureInfo
{
    /// <summary>
    /// Gets or sets the X coordinate for the signature
    /// </summary>
    public int? X { get; set; }
    
    /// <summary>
    /// Gets or sets the Y coordinate for the signature
    /// </summary>
    public int? Y { get; set; }
    
    /// <summary>
    /// Gets or sets the page number for the signature
    /// </summary>
    public int PageNumber { get; set; } = 1;
    
    /// <summary>
    /// Gets or sets the predefined position for the signature
    /// </summary>
    public string? Position { get; set; }
}