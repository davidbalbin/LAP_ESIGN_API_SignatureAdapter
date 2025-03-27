namespace LapDrive.SignatureAdapter.Models.Enums;

/// <summary>
/// Signature process statuses
/// </summary>
public enum SignatureProcessStatus
{
    /// <summary>
    /// Process is pending
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Process is in progress
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Process is completed
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Process is rejected
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// Process is expired
    /// </summary>
    Expired = 4,

    /// <summary>
    /// Process is canceled
    /// </summary>
    Canceled = 5
}