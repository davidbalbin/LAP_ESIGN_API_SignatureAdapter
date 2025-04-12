using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Models.Entities;
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

    /// <inheritdoc/>
    public async Task<SignatureProcessTracking?> GetTrackingAsync(string processId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting tracking information for signature process {ProcessId}", processId);

            var trackingSiteUrl = "https://lapperu.sharepoint.com/sites/dkmt365/firmas";

            // Get tracking item from the list by its CircuitID field
            var trackingItem = await _sharePointClient.GetListItemByFieldValueAsync(
                trackingSiteUrl,
                TrackingListName,
                "CircuitID",
                processId,
                cancellationToken);

            if (trackingItem == null)
            {
                _logger.LogWarning("No tracking information found for process {ProcessId}", processId);
                return null;
            }

            // Map SharePoint list item to tracking object
            return new SignatureProcessTracking
            {
                ProcessId = processId,
                CreatedAt = trackingItem.GetDateTimeValue("Created") ?? DateTime.UtcNow,
                Subject = trackingItem.GetStringValue("Asunto") ?? string.Empty,
                Message = trackingItem.GetStringValue("Mensaje") ?? string.Empty,
                DocumentId = trackingItem.GetStringValue("DocId") ?? string.Empty,
                WebUrl = trackingItem.GetStringValue("SiteUrl") ?? string.Empty,
                LibraryName = trackingItem.GetStringValue("List") ?? string.Empty,
                Signers = trackingItem.GetStringValue("Firmantes")?.Split(',').ToList() ?? new List<string>(),
                Recipients = trackingItem.GetStringValue("Destinatarios")?.Split(',').ToList() ?? new List<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tracking information for process {ProcessId}", processId);
            throw new DataException($"Error getting tracking information: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateTrackingStatusAsync(string processId, string status, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating tracking status for process {ProcessId} to {Status}", processId, status);

            var trackingSiteUrl = "https://lapperu.sharepoint.com/sites/dkmt365/firmas";

            // Get the tracking item from SharePoint
            var trackingItem = await _sharePointClient.GetListItemByFieldValueAsync(
                trackingSiteUrl,
                TrackingListName,
                "CircuitID",
                processId,
                cancellationToken);

            if (trackingItem == null)
            {
                _logger.LogWarning("No tracking record found for process {ProcessId}", processId);
                return;
            }

            // Update the status in SharePoint
            var metadata = new Dictionary<string, object>
        {
            { "Estado", status }
        };

            await _sharePointClient.UpdateListItemAsync(
                trackingSiteUrl,
                TrackingListName,
                trackingItem.Id,
                metadata,
                cancellationToken);

            _logger.LogInformation("Updated tracking status for process {ProcessId} to {Status}", processId, status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tracking status for process {ProcessId}", processId);
            throw new DataException($"Error updating tracking status: {ex.Message}", ex);
        }
    }
}