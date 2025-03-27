namespace LapDrive.SignatureAdapter.Models.Enums;

/// <summary>
/// Signature types
/// </summary>
public enum SignatureType
{
    /// <summary>
    /// Standard digital signature
    /// </summary>
    Standard = 0,
    
    /// <summary>
    /// Advanced digital signature
    /// </summary>
    Advanced = 1,
    
    /// <summary>
    /// Qualified digital signature
    /// </summary>
    Qualified = 2,
    
    /// <summary>
    /// Electronic signature (not digital)
    /// </summary>
    Electronic = 3
}