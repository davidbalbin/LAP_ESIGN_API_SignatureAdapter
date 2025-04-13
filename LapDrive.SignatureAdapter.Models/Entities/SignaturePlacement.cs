namespace LapDrive.SignatureAdapter.Models.Entities;

/// <summary>
/// Entity representing signature placement configuration
/// </summary>
public class SignaturePlacement
{
    /// <summary>
    /// Gets or sets the X coordinate for the signature placement
    /// </summary>
    public int? X { get; set; }
    
    /// <summary>
    /// Gets or sets the Y coordinate for the signature placement
    /// </summary>
    public int? Y { get; set; }
    
    /// <summary>
    /// Gets or sets the page number where the signature will be placed
    /// </summary>
    public int PageNumber { get; set; } = 1;
    
    /// <summary>
    /// Gets or sets the predefined position for the signature
    /// </summary>
    public string? Position { get; set; }
}