namespace LapDrive.SignatureAdapter.Data.Repositories.Interfaces;

/// <summary>
/// Repository for document operations
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Gets the content of a document from a document library
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="documentId">The document ID</param>
    /// <param name="isFolder">Indicates whether the document is a folder</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The document content as a byte array</returns>
    Task<byte[]> GetDocumentContentAsync(string webUrl, string libraryName, string documentId, bool isFolder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of a document in a document library
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="documentId">The document ID</param>
    /// <param name="status">The new status</param>
    /// <param name="processId">The signature process ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    Task UpdateDocumentStatusAsync(string webUrl, string libraryName, string documentId, string status, string processId, CancellationToken cancellationToken = default);
}