namespace LapDrive.SignatureAdapter.Models.Enums
{
    public enum ProcessStatus
    {
        /// <summary>
        /// Process is waiting to be started
        /// </summary>
        Pending,

        /// <summary>
        /// Process is in progress
        /// </summary>
        InProgress,

        /// <summary>
        /// Process is waiting for signatures
        /// </summary>
        WaitingForSignatures,

        /// <summary>
        /// Process has been completed successfully
        /// </summary>
        Completed,

        /// <summary>
        /// Process has been rejected
        /// </summary>
        Rejected,

        /// <summary>
        /// Process has been cancelled
        /// </summary>
        Cancelled
    }
}