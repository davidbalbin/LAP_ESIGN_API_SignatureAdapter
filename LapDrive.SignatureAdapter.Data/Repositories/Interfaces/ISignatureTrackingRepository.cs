using LapDrive.SignatureAdapter.Models.Entities;

namespace LapDrive.SignatureAdapter.Data.Repositories.Interfaces;

/// <summary>
/// Repository for tracking signature processes
/// </summary>
public interface ISignatureTrackingRepository
{
    /// <summary>
    /// Registers a signature process tracking record in SharePoint
    /// </summary>
    /// <param name="processId">The signature process ID</param>
    /// <param name="subject">The subject of the process</param>
    /// <param name="message">The message of the process</param>
    /// <param name="documentId">The document ID</param>
    /// <param name="siteUrl">The SharePoint site URL</param>
    /// <param name="libraryName">The document library name</param>
    /// <param name="signers">The list of signer emails</param>
    /// <param name="recipients">The list of recipient emails</param>
    /// <param name="signingUrl">The URL for signing</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RegisterTrackingAsync(
        string processId,
        string subject,
        string message,
        string documentId,
        string siteUrl,
        string libraryName,
        IEnumerable<string> signers,
        IEnumerable<string> recipients,
        string signingUrl,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets tracking information for a signature process
    /// </summary>
    /// <param name="processId">The ID of the signature process</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The tracking information</returns>
    Task<SignatureProcessTracking?> GetTrackingAsync(string processId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the status of a signature process tracking record
    /// </summary>
    /// <param name="processId">The ID of the signature process</param>
    /// <param name="status">The new status</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task UpdateTrackingStatusAsync(string processId, string status, CancellationToken cancellationToken = default);
}