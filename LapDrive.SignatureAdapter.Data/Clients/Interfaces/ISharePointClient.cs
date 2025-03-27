namespace LapDrive.SignatureAdapter.Data.Clients.Interfaces;

/// <summary>
/// Client for SharePoint operations
/// </summary>
public interface ISharePointClient
{
    /// <summary>
    /// Gets the contents of a file from SharePoint
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="itemId">The item ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The file contents as a byte array</returns>
    Task<byte[]> GetFileContentsAsync(string webUrl, string libraryName, string itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the contents of a folder as a zip file
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="folderId">The folder ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The folder contents as a zip file byte array</returns>
    Task<byte[]> GetFolderContentsAsZipAsync(string webUrl, string libraryName, string folderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates metadata for a SharePoint item
    /// </summary>
    /// <param name="webUrl">The SharePoint web URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="itemId">The item ID</param>
    /// <param name="metadata">The metadata to update</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    Task UpdateItemMetadataAsync(string webUrl, string libraryName, string itemId, Dictionary<string, object> metadata, CancellationToken cancellationToken = default);
}