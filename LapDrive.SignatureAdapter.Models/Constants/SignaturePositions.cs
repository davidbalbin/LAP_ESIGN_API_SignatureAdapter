namespace LapDrive.SignatureAdapter.Models.Constants;

/// <summary>
/// Predefined signature positions
/// </summary>
public static class SignaturePositions
{
    /// <summary>
    /// Top left corner position
    /// </summary>
    public const string TopLeft = "topLeft";
    
    /// <summary>
    /// Top center position
    /// </summary>
    public const string TopCenter = "topCenter";
    
    /// <summary>
    /// Top right corner position
    /// </summary>
    public const string TopRight = "topRight";
    
    /// <summary>
    /// Bottom left corner position
    /// </summary>
    public const string BottomLeft = "bottomLeft";
    
    /// <summary>
    /// Bottom center position
    /// </summary>
    public const string BottomCenter = "bottomCenter";
    
    /// <summary>
    /// Bottom right corner position
    /// </summary>
    public const string BottomRight = "bottomRight";
    
    /// <summary>
    /// Gets the X coordinate for a predefined position
    /// </summary>
    /// <param name="position">The predefined position</param>
    /// <param name="pageWidth">The page width</param>
    /// <param name="signatureWidth">The signature width</param>
    /// <returns>The X coordinate</returns>
    public static int GetXCoordinate(string position, int pageWidth, int signatureWidth)
    {
        return position switch
        {
            TopLeft or BottomLeft => 50,
            TopCenter or BottomCenter => (pageWidth - signatureWidth) / 2,
            TopRight or BottomRight => pageWidth - signatureWidth - 50,
            _ => 50
        };
    }
    
    /// <summary>
    /// Gets the Y coordinate for a predefined position
    /// </summary>
    /// <param name="position">The predefined position</param>
    /// <param name="pageHeight">The page height</param>
    /// <param name="signatureHeight">The signature height</param>
    /// <returns>The Y coordinate</returns>
    public static int GetYCoordinate(string position, int pageHeight, int signatureHeight)
    {
        return position switch
        {
            TopLeft or TopCenter or TopRight => 50,
            BottomLeft or BottomCenter or BottomRight => pageHeight - signatureHeight - 50,
            _ => 50
        };
    }
}