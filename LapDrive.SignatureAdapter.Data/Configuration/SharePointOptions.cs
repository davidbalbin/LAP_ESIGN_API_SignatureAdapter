namespace LapDrive.SignatureAdapter.Data.Configuration;

/// <summary>
/// Configuration options for SharePoint
/// </summary>
public class SharePointOptions
{
    /// <summary>
    /// Gets or sets the application client ID
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the application client secret
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the tenant ID
    /// </summary>
    public string TenantId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the site URL
    /// </summary>
    public string SiteUrl { get; set; } = string.Empty;
}