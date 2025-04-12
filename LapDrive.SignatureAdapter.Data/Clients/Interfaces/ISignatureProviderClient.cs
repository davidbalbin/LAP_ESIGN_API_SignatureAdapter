using LapDrive.SignatureAdapter.Models.DTOs.Response;
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

    /// <summary>
    /// Gets the status of a signature process
    /// </summary>
    /// <param name="processId">The ID of the process to get</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The status of the signature process</returns>
    Task<SignatureProcessStatusResponse> GetSignatureProcessStatusAsync(string processId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a signature process
    /// </summary>
    /// <param name="processId">The ID of the process to cancel</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>True if the process was canceled successfully, false otherwise</returns>
    Task<bool> CancelSignatureProcessAsync(string processId, CancellationToken cancellationToken = default);
}