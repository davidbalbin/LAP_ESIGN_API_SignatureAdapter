using LapDrive.SignatureAdapter.Models.Entities;

namespace LapDrive.SignatureAdapter.Data.Clients.Interfaces;

/// <summary>
/// Client for signature provider operations
/// </summary>
public interface ISignatureProviderClient
{
    /// <summary>
    /// Creates a new signature process
    /// </summary>
    /// <param name="signatureProcess">The signature process details</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The ID of the created signature process</returns>
    Task<string> CreateSignatureProcessAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken = default);
    
}