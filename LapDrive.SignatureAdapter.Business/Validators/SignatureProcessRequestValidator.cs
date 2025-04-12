using FluentValidation;
using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.DTOs.Request;
using System.Linq;

namespace LapDrive.SignatureAdapter.Business.Validators;

/// <summary>
/// Validator for SignatureProcessRequest
/// </summary>
public class SignatureProcessRequestValidator : AbstractValidator<SignatureProcessRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureProcessRequestValidator"/> class.
    /// </summary>
    public SignatureProcessRequestValidator()
    {
        RuleFor(x => x.Metadata)
            .NotNull()
            .WithMessage("Metadata is required");

        RuleFor(x => x.Metadata.Subject)
            .NotEmpty()
            .WithMessage("Subject is required")
            .MaximumLength(200)
            .WithMessage("Subject cannot exceed 200 characters");

        RuleFor(x => x.Document)
            .NotNull()
            .WithMessage("Document is required");

        RuleFor(x => x.Document.Id)
            .NotEmpty()
            .WithMessage("Document ID is required");

        RuleFor(x => x.Document.Name)
            .NotEmpty()
            .WithMessage("Document name is required");

        RuleFor(x => x.Document.LibraryName)
            .NotEmpty()
            .WithMessage("Library name is required");

        RuleFor(x => x.Document.WebUrl)
            .NotEmpty()
            .WithMessage("Web URL is required")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Web URL must be a valid URL");

        RuleFor(x => x.Document.Type)
            .NotEmpty()
            .WithMessage("Document type is required")
            .Must(type => type == CommonConstants.DocumentTypes.File || type == CommonConstants.DocumentTypes.Folder)
            .WithMessage($"Document type must be '{CommonConstants.DocumentTypes.File}' or '{CommonConstants.DocumentTypes.Folder}'");

        RuleFor(x => x.Signers)
            .NotEmpty()
            .WithMessage("At least one signer is required");

        RuleForEach(x => x.Signers)
            .ChildRules(signer =>
            {
                signer.RuleFor(s => s.DisplayName)
                    .NotEmpty()
                    .WithMessage("Signer display name is required");

                signer.RuleFor(s => s.Email)
                    .NotEmpty()
                    .WithMessage("Signer email is required")
                    .EmailAddress()
                    .WithMessage("Invalid email format");

                signer.RuleFor(s => s.Signature)
                    .NotNull()
                    .WithMessage("Signature information is required");

                signer.RuleFor(s => s.Signature.PageNumber)
                    .GreaterThan(0)
                    .WithMessage("Page number must be greater than 0");

                // Conditional validation for position vs coordinates
                signer.When(s => string.IsNullOrEmpty(s.Signature.Position), () =>
                {
                    signer.RuleFor(s => s.Signature.X)
                        .NotNull()
                        .WithMessage("X coordinate is required when position is not specified");

                    signer.RuleFor(s => s.Signature.Y)
                        .NotNull()
                        .WithMessage("Y coordinate is required when position is not specified");
                });

                signer.When(s => !string.IsNullOrEmpty(s.Signature.Position), () =>
                {
                    signer.RuleFor(s => s.Signature.Position)
                        .Must(position => new[] {
                            SignaturePositions.TopLeft,
                            SignaturePositions.TopCenter,
                            SignaturePositions.TopRight,
                            SignaturePositions.BottomLeft,
                            SignaturePositions.BottomCenter,
                            SignaturePositions.BottomRight
                        }.Contains(position))
                        .WithMessage($"Position must be one of: {SignaturePositions.TopLeft}, {SignaturePositions.TopCenter}, {SignaturePositions.TopRight}, {SignaturePositions.BottomLeft}, {SignaturePositions.BottomCenter}, {SignaturePositions.BottomRight}");
                });
            });

        // Validate recipients if present
        When(x => x.Recipients != null && x.Recipients.Any(), () =>
        {
            RuleForEach(x => x.Recipients)
                .ChildRules(recipient =>
                {
                    recipient.RuleFor(r => r.DisplayName)
                        .NotEmpty()
                        .WithMessage("Recipient display name is required");

                    recipient.RuleFor(r => r.Email)
                        .NotEmpty()
                        .WithMessage("Recipient email is required")
                        .EmailAddress()
                        .WithMessage("Invalid email format");
                });
        });
    }
}