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
}