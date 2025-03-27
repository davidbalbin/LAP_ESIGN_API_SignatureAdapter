using LapDrive.SignatureAdapter.Data.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using PnP.Framework;

namespace LapDrive.SignatureAdapter.Data.Factories;

/// <summary>
/// Factory for creating SharePoint context objects
/// </summary>
public class SharePointContextFactory
{
    private readonly SharePointOptions _options;
    private readonly ILogger<SharePointContextFactory> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SharePointContextFactory"/> class.
    /// </summary>
    /// <param name="options">The SharePoint options</param>
    /// <param name="logger">The logger</param>
    public SharePointContextFactory(
        IOptions<SharePointOptions> options,
        ILogger<SharePointContextFactory> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a ClientContext for the specified web URL
    /// </summary>
    /// <param name="webUrl">The web URL</param>
    /// <returns>A SharePoint ClientContext</returns>
    public ClientContext CreateContext(string webUrl)
    {
        try
        {
            _logger.LogDebug("Creating SharePoint context for {WebUrl}", webUrl);
            
            // Use PnP.Framework's authentication manager to get the context
            var authManager = new AuthenticationManager();
            var clientContext = authManager.GetACSAppOnlyContext(
                webUrl,
                _options.ClientId,
                _options.ClientSecret);
            
            clientContext.RequestTimeout = -1; // Unlimited timeout
            
            return clientContext;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SharePoint context for {WebUrl}", webUrl);
            throw;
        }
    }

    /// <summary>
    /// Creates a ClientContext for the tenant site URL configured in options
    /// </summary>
    /// <returns>A SharePoint ClientContext</returns>
    public ClientContext CreateDefaultContext()
    {
        return CreateContext(_options.SiteUrl);
    }
}