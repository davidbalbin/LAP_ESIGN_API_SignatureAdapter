using LapDrive.SignatureAdapter.Models.DTOs.Request;
using LapDrive.SignatureAdapter.Models.DTOs.Response;

namespace LapDrive.SignatureAdapter.Business.Services.Interfaces;

/// <summary>
/// Service for managing signature processes
/// </summary>
public interface ISignatureProcessService
{
    /// <summary>
    /// Creates a new signature process from the specified request
    /// </summary>
    /// <param name="request">The signature process request data</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The created signature process information</returns>
    Task<SignatureProcessResponse> CreateSignatureProcessAsync(SignatureProcessRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a signature process by its ID
    /// </summary>
    /// <param name="processId">The ID of the signature process</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The signature process details or null if not found</returns>
    Task<SignatureProcessDetailResponse?> GetSignatureProcessAsync(string processId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a signature process by its ID
    /// </summary>
    /// <param name="processId">The ID of the process to cancel</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>True if the process was canceled, false if not found</returns>
    Task<bool> CancelSignatureProcessAsync(string processId, CancellationToken cancellationToken = default);
}