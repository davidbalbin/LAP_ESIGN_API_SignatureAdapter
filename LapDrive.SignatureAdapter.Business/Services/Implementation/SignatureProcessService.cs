using FluentValidation;
using LapDrive.SignatureAdapter.Business.Extensions;
using LapDrive.SignatureAdapter.Business.Services.Interfaces;
using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.DTOs.Request;
using LapDrive.SignatureAdapter.Models.DTOs.Response;
using LapDrive.SignatureAdapter.Models.Entities;
using LapDrive.SignatureAdapter.Models.Enums;
using LapDrive.SignatureAdapter.Models.Exceptions;
using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace LapDrive.SignatureAdapter.Business.Services.Implementation;

/// <summary>
/// Service for managing signature processes
/// </summary>
public class SignatureProcessService : ISignatureProcessService
{
    private readonly IValidator<SignatureProcessRequest> _validator;
    private readonly IDocumentRepository _documentRepository;
    private readonly ISignatureProcessRepository _signatureProcessRepository;
    private readonly ISignatureTrackingRepository _trackingRepository; 
    private readonly ILogger<SignatureProcessService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureProcessService"/> class.
    /// </summary>
    /// <param name="validator">The validator for signature process requests</param>
    /// <param name="documentRepository">The document repository</param>
    /// <param name="signatureProcessRepository">The signature process repository</param>
    /// <param name="logger">The logger</param>
    public SignatureProcessService(
        IValidator<SignatureProcessRequest> validator,
        IDocumentRepository documentRepository,
        ISignatureProcessRepository signatureProcessRepository,
        ISignatureTrackingRepository trackingRepository,
        ILogger<SignatureProcessService> logger)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        _signatureProcessRepository = signatureProcessRepository ?? throw new ArgumentNullException(nameof(signatureProcessRepository));
        _trackingRepository = trackingRepository ?? throw new ArgumentNullException(nameof(trackingRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<SignatureProcessResponse> CreateSignatureProcessAsync(SignatureProcessRequest request, CancellationToken cancellationToken = default)
    {
        // Validate request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new Models.Exceptions.ValidationException(errors);
        }

        try
        {
            // Get document from SharePoint
            var documentContent = await _documentRepository.GetDocumentContentAsync(
                request.Document.WebUrl,
                request.Document.LibraryName,
                request.Document.Id,
                request.Document.Type == DocumentTypes.Folder,
                cancellationToken);

            if (documentContent == null)
            {
                throw new BusinessException($"Document with ID {request.Document.Id} not found in library {request.Document.LibraryName}");
            }

            // Create signature process entity
            var signatureProcess = new SignatureProcess
            {
                RequestId = request.RequestId ?? Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                Subject = request.Metadata.Subject,
                Message = request.Metadata.Message,
                Document = new Document
                {
                    Id = request.Document.Id,
                    Name = request.Document.Name,
                    LibraryName = request.Document.LibraryName,
                    WebUrl = request.Document.WebUrl,
                    Type = request.Document.Type == DocumentTypes.Folder ? DocumentType.Folder : DocumentType.File,
                    Content = documentContent
                },
                Signers = request.Signers.Select(s => new Signer
                {
                    DisplayName = s.DisplayName,
                    Email = s.Email,
                    SignatureInfo = new Models.Entities.SignatureInfo
                    {
                        PageNumber = s.Signature.PageNumber,
                        X = s.Signature.X,
                        Y = s.Signature.Y,
                        Position = s.Signature.Position
                    }
                }).ToList(),
                Recipients = request.Recipients?.Select(r => new Recipient
                {
                    DisplayName = r.DisplayName,
                    Email = r.Email
                }).ToList() ?? new List<Recipient>()
            };

            // Send to signature provider
            var processId = await _signatureProcessRepository.CreateSignatureProcessAsync(signatureProcess, cancellationToken);

            // Update document status in SharePoint
            await _documentRepository.UpdateDocumentStatusAsync(
                request.Document.WebUrl,
                request.Document.LibraryName,
                request.Document.Id,
                ProcessStatuses.InProgress,
                processId,
                cancellationToken);

            await _trackingRepository.RegisterTrackingAsync(
               processId,
               request.Metadata.Subject,
               request.Metadata.Message,
               request.Document.Id,
               request.Document.WebUrl,
               request.Document.LibraryName,
               request.Signers.Select(s => s.Email),
               request.Recipients?.Select(r => r.Email) ?? new List<string>(),
               "", 
               cancellationToken);

            // Return response
            var response = new SignatureProcessResponse
            {
                ProcessId = processId,
                StatusEnum = ProcessStatus.InProgress,
                Message = "Proceso de firma creado exitosamente"
            };

            return response;
        }
        catch (Exception ex) when (ex is not Models.Exceptions.ValidationException && ex is not BusinessException)
        {
            _logger.LogError(ex, "Error creating signature process");
            throw new DataException("An error occurred while creating the signature process", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<SignatureProcessDetailResponse?> GetSignatureProcessAsync(string processId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(processId))
        {
            throw new ArgumentNullException(nameof(processId));
        }

        try
        {
            // Get process status from Watana
            var signatureProcessStatus = await _signatureProcessRepository.GetSignatureProcessStatusAsync(processId, cancellationToken);

            if (signatureProcessStatus == null)
            {
                return null;
            }

            // Get tracking information from SharePoint
            var trackingInfo = await _trackingRepository.GetTrackingAsync(processId, cancellationToken);

            if (trackingInfo == null)
            {
                // If no tracking info is found, we can still return basic process info
                return new SignatureProcessDetailResponse
                {
                    ProcessId = processId,
                    StatusEnum = signatureProcessStatus?.Status?.ToProcessStatus() ?? ProcessStatus.Pending,
                    CreatedAt = DateTime.UtcNow, // No way to know exact creation time without tracking
                    Subject = signatureProcessStatus?.Title ?? string.Empty,
                    Message = string.Empty,
                    Signers = signatureProcessStatus?.Signers?.Select(f => new SignerDetail
                    {
                        DisplayName = f.FullName,
                        Email = f.Email,
                        StatusEnum = f.Status.ToSignerStatus(),
                        SignatureDate = f.SignatureDate
                    })?.ToList() ?? new List<SignerDetail>()
                };
            }

            // Map tracking info and status to detailed response
            return new SignatureProcessDetailResponse
            {
                ProcessId = processId,
                StatusEnum = signatureProcessStatus?.Status?.ToProcessStatus() ?? ProcessStatus.Pending,
                CreatedAt = trackingInfo.CreatedAt,
                Subject = trackingInfo.Subject,
                Message = trackingInfo.Message,
                Document = new DocumentDetail
                {
                    Id = trackingInfo.DocumentId,
                    Name = Path.GetFileName(trackingInfo.DocumentId),
                    LibraryName = trackingInfo.LibraryName,
                    WebUrl = trackingInfo.WebUrl,
                    Type = DocumentTypes.File // Default to file
                },
                Signers = signatureProcessStatus?.Signers?.Select(f => new SignerDetail
                {
                    DisplayName = f.FullName,
                    Email = f.Email,
                    StatusEnum = f.Status.ToSignerStatus(),
                    SignatureDate = f.SignatureDate
                })?.ToList() ?? new List<SignerDetail>(),
                Recipients = trackingInfo.Recipients.Select(r => new RecipientDetail
                {
                    DisplayName = r,
                    Email = r
                }).ToList()
            };
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            _logger.LogError(ex, "Error getting signature process with ID {ProcessId}", processId);
            throw new DataException($"Error getting signature process: {ex.Message}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CancelSignatureProcessAsync(string processId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(processId))
        {
            throw new ArgumentNullException(nameof(processId));
        }

        try
        {
            // Check if process exists and get its current status
            var processStatus = await _signatureProcessRepository.GetSignatureProcessStatusAsync(processId, cancellationToken);

            if (processStatus == null)
            {
                _logger.LogWarning("Signature process with ID {ProcessId} not found for cancellation", processId);
                return false;
            }

            // Check if process can be canceled (only if it's in progress)
            var status = processStatus?.Status ?? string.Empty;
            if (status != WatanaStatuses.EnProceso && status != WatanaStatuses.EnEspera)
            {
                throw new BusinessException($"Cannot cancel process with status '{status}'. Only in-progress processes can be canceled.");
            }

            // Cancel the process in the signature provider
            var canceled = await _signatureProcessRepository.CancelSignatureProcessAsync(processId, cancellationToken);

            if (!canceled)
            {
                _logger.LogWarning("Failed to cancel signature process with ID {ProcessId}", processId);
                return false;
            }

            // Get tracking information to update document status in SharePoint
            var trackingInfo = await _trackingRepository.GetTrackingAsync(processId, cancellationToken);

            if (trackingInfo != null)
            {
                // Update document status in SharePoint
                await _documentRepository.UpdateDocumentStatusAsync(
                    trackingInfo.WebUrl,
                    trackingInfo.LibraryName,
                    trackingInfo.DocumentId,
                    ProcessStatuses.Cancelled,
                    processId,
                    cancellationToken);

                // Update tracking record
                await _trackingRepository.UpdateTrackingStatusAsync(
                    processId,
                    ProcessStatuses.Cancelled,
                    cancellationToken);
            }

            return true;
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            _logger.LogError(ex, "Error canceling signature process with ID {ProcessId}", processId);
            throw new DataException($"Error canceling signature process: {ex.Message}", ex);
        }
    }
}