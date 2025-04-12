namespace LapDrive.SignatureAdapter.Data.Configuration;

/// <summary>
/// Configuration options for the signature provider
/// </summary>
public class SignatureProviderOptions
{
    /// <summary>
    /// Gets or sets the API URL
    /// </summary>
    public string ApiUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the authentication token
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
    
    /// <summary>
    /// Gets or sets the maximum number of retries for failed requests
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the signature templates configuration
    /// </summary>
    public SignatureProviderTemplates Templates { get; set; } = new SignatureProviderTemplates();

    /// <summary>
    /// Gets or sets the signature process status configuration
    /// </summary>
    public SignatureProviderStatus Status { get; set; } = new SignatureProviderStatus();
}