using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Models.Entities;
using LapDrive.SignatureAdapter.Models.Exceptions;
using Microsoft.Extensions.Logging;

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
    public async Task<string> GetSigningUrlAsync(string processId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Getting signing URL for process {ProcessId}", processId);
            
            var signingUrl = await _signatureProviderClient.GetSigningUrlAsync(processId, cancellationToken);
            
            return signingUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting signing URL for process {ProcessId}", processId);
            throw new DataException($"Error getting signing URL: {ex.Message}", ex);
        }
    }
}