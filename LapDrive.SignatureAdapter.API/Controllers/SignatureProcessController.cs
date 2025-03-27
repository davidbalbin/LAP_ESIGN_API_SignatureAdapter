using LapDrive.SignatureAdapter.Business.Services.Interfaces;
using LapDrive.SignatureAdapter.Models.DTOs.Request;
using LapDrive.SignatureAdapter.Models.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace LapDrive.SignatureAdapter.API.Controllers;

/// <summary>
/// Controller for signature process operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/signature-processes")]
[Produces("application/json")]
public class SignatureProcessController : ControllerBase
{
    private readonly ISignatureProcessService _signatureProcessService;
    private readonly ILogger<SignatureProcessController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureProcessController"/> class.
    /// </summary>
    /// <param name="signatureProcessService">The signature process service</param>
    /// <param name="logger">The logger</param>
    public SignatureProcessController(
        ISignatureProcessService signatureProcessService,
        ILogger<SignatureProcessController> logger)
    {
        _signatureProcessService = signatureProcessService ?? throw new ArgumentNullException(nameof(signatureProcessService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new signature process
    /// </summary>
    /// <param name="request">The signature process request data</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests</param>
    /// <returns>The created signature process information</returns>
    /// <response code="201">Returns the newly created signature process</response>
    /// <response code="400">If the request data is invalid</response>
    /// <response code="500">If an error occurs during processing</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a new signature process",
        Description = "Creates a new digital signature process with the specified documents and signers",
        OperationId = "CreateSignatureProcess",
        Tags = new[] { "Signature Processes" }
    )]
    [SwaggerResponse((int)HttpStatusCode.Created, "Signature process created successfully", typeof(SignatureProcessResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Invalid request data")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Internal server error")]
    [ProducesResponseType(typeof(SignatureProcessResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> CreateSignatureProcess(
        [FromBody] SignatureProcessRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating signature process with {DocumentId} from {WebUrl}/{LibraryName}", 
            request.Document.Id, request.Document.WebUrl, request.Document.LibraryName);

        var result = await _signatureProcessService.CreateSignatureProcessAsync(request, cancellationToken);
        
        _logger.LogInformation("Signature process created with ID {ProcessId}", result.ProcessId);
        
        return CreatedAtAction(
            nameof(CreateSignatureProcess),
            new { id = result.ProcessId },
            result);
    }
}