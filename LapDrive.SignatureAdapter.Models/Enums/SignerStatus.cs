namespace LapDrive.SignatureAdapter.Models.Enums
{
    /// <summary>
    /// Standardized status for signature process signers
    /// </summary>
    public enum SignerStatus
    {
        /// <summary>
        /// Waiting for the signer's action
        /// </summary>
        Pending,

        /// <summary>
        /// Document has been signed
        /// </summary>
        Signed,

        /// <summary>
        /// Document has been rejected by the signer
        /// </summary>
        Rejected,

        /// <summary>
        /// Process has been cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// Signature in progress
        /// </summary>
        InProgress
    }
}