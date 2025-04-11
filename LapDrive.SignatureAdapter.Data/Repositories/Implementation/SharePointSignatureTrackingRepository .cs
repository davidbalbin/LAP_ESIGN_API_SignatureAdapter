using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace LapDrive.SignatureAdapter.Data.Repositories.Implementation;

/// <summary>
/// SharePoint implementation of the signature tracking repository
/// </summary>
public class SharePointSignatureTrackingRepository : ISignatureTrackingRepository
{
    private readonly ISharePointClient _sharePointClient;
    private readonly ILogger<SharePointSignatureTrackingRepository> _logger;
    private const string TrackingListName = "EnviosFirmas";

    /// <summary>
    /// Initializes a new instance of the <see cref="SharePointSignatureTrackingRepository"/> class.
    /// </summary>
    /// <param name="sharePointClient">The SharePoint client</param>
    /// <param name="logger">The logger</param>
    public SharePointSignatureTrackingRepository(
        ISharePointClient sharePointClient,
        ILogger<SharePointSignatureTrackingRepository> logger)
    {
        _sharePointClient = sharePointClient ?? throw new ArgumentNullException(nameof(sharePointClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task RegisterTrackingAsync(
        string processId,
        string subject,
        string message,
        string documentId,
        string siteUrl,
        string libraryName,
        IEnumerable<string> signers,
        IEnumerable<string> recipients,
        string signingUrl,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering tracking information for signature process {ProcessId}", processId);

            var trackingSiteUrl = "https://lapperu.sharepoint.com/sites/dkmt365/firmas";

            var metadata = new Dictionary<string, object>
            {
                { "Title", $"Process {processId}" },
                { "Remitente", recipients.FirstOrDefault() ?? string.Empty },
                { "Destinatarios", string.Join(",", recipients) },
                { "Firmantes", string.Join(",", signers) },
                { "Asunto", subject },
                { "Mensaje", message },
                { "Estado", "Pendiente" },
                { "CircuitURL", signingUrl },
                { "CircuitID", processId },
                { "DocId", documentId },
                { "SiteUrl", siteUrl },
                { "List", libraryName }
            };

            // Create a new item in the tracking list
            await _sharePointClient.CreateListItemAsync(
                trackingSiteUrl,
                TrackingListName,
                metadata,
                cancellationToken);

            _logger.LogInformation("Tracking information registered successfully for process {ProcessId}", processId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering tracking information for process {ProcessId}", processId);
            throw new DataException($"Error registering tracking information: {ex.Message}", ex);
        }
    }
}