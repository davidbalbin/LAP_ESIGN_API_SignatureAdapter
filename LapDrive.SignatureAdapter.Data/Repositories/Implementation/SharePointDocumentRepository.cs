using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;

namespace LapDrive.SignatureAdapter.Data.Repositories.Implementation;

/// <summary>
/// SharePoint implementation of the document repository
/// </summary>
public class SharePointDocumentRepository : IDocumentRepository
{
    private readonly ISharePointClient _sharePointClient;
    private readonly ILogger<SharePointDocumentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SharePointDocumentRepository"/> class.
    /// </summary>
    /// <param name="sharePointClient">The SharePoint client</param>
    /// <param name="logger">The logger</param>
    public SharePointDocumentRepository(
        ISharePointClient sharePointClient,
        ILogger<SharePointDocumentRepository> logger)
    {
        _sharePointClient = sharePointClient ?? throw new ArgumentNullException(nameof(sharePointClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<byte[]> GetDocumentContentAsync(string webUrl, string libraryName, string documentId, bool isFolder, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting content for document {DocumentId} from {WebUrl}/{LibraryName}", documentId, webUrl, libraryName);
            
            if (isFolder)
            {
                return await _sharePointClient.GetFolderContentsAsZipAsync(webUrl, libraryName, documentId, cancellationToken);
            }
            else
            {
                return await _sharePointClient.GetFileContentsAsync(webUrl, libraryName, documentId, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document content from SharePoint for {DocumentId} in {WebUrl}/{LibraryName}", documentId, webUrl, libraryName);
            throw new DataException($"Error retrieving document content: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task UpdateDocumentStatusAsync(string webUrl, string libraryName, string documentId, string status, string processId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating status to {Status} for document {DocumentId} in {WebUrl}/{LibraryName}", status, documentId, webUrl, libraryName);
            
            var metadata = new Dictionary<string, object>
            {
                { "FlujoFirmas", $"{status},{processId}" }
            };
            
            await _sharePointClient.UpdateItemMetadataAsync(webUrl, libraryName, documentId, metadata, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating document status in SharePoint for {DocumentId} in {WebUrl}/{LibraryName}", documentId, webUrl, libraryName);
            throw new DataException($"Error updating document status: {ex.Message}", ex);
        }
    }
}