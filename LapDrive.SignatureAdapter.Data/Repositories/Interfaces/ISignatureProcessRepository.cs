using LapDrive.SignatureAdapter.Models.Entities;

namespace LapDrive.SignatureAdapter.Data.Repositories.Interfaces;

/// <summary>
/// Repository for signature process operations
/// </summary>
public interface ISignatureProcessRepository
{
    /// <summary>
    /// Creates a new signature process
    /// </summary>
    /// <param name="signatureProcess">The signature process to create</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The ID of the created signature process</returns>
    Task<string> CreateSignatureProcessAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the signing URL for a signature process
    /// </summary>
    /// <param name="processId">The signature process ID</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The signing URL</returns>
    Task<string> GetSigningUrlAsync(string processId, CancellationToken cancellationToken = default);
}