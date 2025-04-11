namespace LapDrive.SignatureAdapter.Models.Enums;

/// <summary>
/// Status of a signer in a signature process
/// </summary>
public enum SignerStatus
{
    /// <summary>
    /// Signer has not taken any action yet
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Signer has signed the document
    /// </summary>
    Signed = 1,

    /// <summary>
    /// Signer has rejected the document
    /// </summary>
    Rejected = 2
}