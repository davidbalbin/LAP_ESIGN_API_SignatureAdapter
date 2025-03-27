using FluentValidation;
using LapDrive.SignatureAdapter.Business.Services.Interfaces;
using LapDrive.SignatureAdapter.Models.DTOs.Request;
using LapDrive.SignatureAdapter.Models.DTOs.Response;
using LapDrive.SignatureAdapter.Models.Entities;
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
        ILogger<SignatureProcessService> logger)
    {
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        _signatureProcessRepository = signatureProcessRepository ?? throw new ArgumentNullException(nameof(signatureProcessRepository));
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
            throw new ValidationException(errors);
        }

        try
        {
            // Get document from SharePoint
            var documentContent = await _documentRepository.GetDocumentContentAsync(
                request.Document.WebUrl,
                request.Document.LibraryName,
                request.Document.Id,
                request.Document.Type == "folder",
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
                    Type = request.Document.Type == "folder" ? Models.Enums.DocumentType.Folder : Models.Enums.DocumentType.File,
                    Content = documentContent
                },
                Signers = request.Signers.Select(s => new Signer
                {
                    DisplayName = s.DisplayName,
                    Email = s.Email,
                    SignatureInfo = new SignatureInfo
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
                "InProgress",
                processId,
                cancellationToken);

            // Return response
            return new SignatureProcessResponse
            {
                ProcessId = processId,
                Status = "InProgress",
                Message = "Signature process created successfully",
                SigningUrl = await _signatureProcessRepository.GetSigningUrlAsync(processId, cancellationToken)
            };
        }
        catch (Exception ex) when (ex is not ValidationException && ex is not BusinessException)
        {
            _logger.LogError(ex, "Error creating signature process");
            throw new DataException("An error occurred while creating the signature process", ex);
        }
    }
}