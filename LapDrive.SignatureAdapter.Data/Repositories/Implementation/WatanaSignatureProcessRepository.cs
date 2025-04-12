using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Models.DTOs.Response;
using LapDrive.SignatureAdapter.Models.Entities;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;
using WatanaClient.API.Models.Requests;

namespace LapDrive.SignatureAdapter.Data.Repositories.Implementation;

/// <summary>
/// Watana implementation of the signature process repository
/// </summary>
public class WatanaSignatureProcessRepository : ISignatureProcessRepository
{
    private readonly ISignatureProviderClient _signatureProviderClient;
    private readonly ILogger<WatanaSignatureProcessRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WatanaSignatureProcessRepository"/> class.
    /// </summary>
    /// <param name="signatureProviderClient">The signature provider client</param>
    /// <param name="logger">The logger</param>
    public WatanaSignatureProcessRepository(
        ISignatureProviderClient signatureProviderClient,
        ILogger<WatanaSignatureProcessRepository> logger)
    {
        _signatureProviderClient = signatureProviderClient ?? throw new ArgumentNullException(nameof(signatureProviderClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<string> CreateSignatureProcessAsync(SignatureProcess signatureProcess, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating signature process with subject '{Subject}'", signatureProcess.Subject);
            
            var processId = await _signatureProviderClient.CreateSignatureProcessAsync(signatureProcess, cancellationToken);
            
            _logger.LogInformation("Signature process created with ID {ProcessId}", processId);
            
            return processId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating signature process with subject '{Subject}'", signatureProcess.Subject);
            throw new DataException($"Error creating signature process: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    /// <inheritdoc/>
    public async Task<SignatureProcessStatusResponse?> GetSignatureProcessStatusAsync(string processId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting signature process status for ID {ProcessId}", processId);

            var status = await _signatureProviderClient.GetSignatureProcessStatusAsync(processId, cancellationToken);

            // Si no se encontró el proceso, retornamos null
            if (status == null)
            {
                _logger.LogWarning("Signature process with ID {ProcessId} was not found", processId);
                return null;
            }

            return status;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting signature process status for ID {ProcessId}", processId);
            throw new DataException($"Error getting signature process status: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CancelSignatureProcessAsync(string processId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Canceling signature process {ProcessId} in Watana", processId);

            var response = await _signatureProviderClient.CancelSignatureProcessAsync(processId, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling signature process {ProcessId} in Watana", processId);
            throw new DataException($"Error canceling signature process in Watana: {ex.Message}", ex);
        }
    }
}