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
}