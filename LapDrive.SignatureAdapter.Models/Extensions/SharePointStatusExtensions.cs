using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.Extensions
{
    /// <summary>
    /// Extension methods for converting SignatureProcessStatus to SharePoint status strings
    /// </summary>
    public static class SharePointStatusExtensions
    {
        /// <summary>
        /// Converts SignatureProcessStatus to SharePoint status string
        /// </summary>
        public static string ToSharePointStatus(this SignatureProcessStatus status)
        {
            return status switch
            {
                SignatureProcessStatus.Pending => "Pending",
                SignatureProcessStatus.InProgress => "InProgress",
                SignatureProcessStatus.WaitingForSignatures => "InProgress",
                SignatureProcessStatus.Completed => "Completed",
                SignatureProcessStatus.Rejected => "Rejected",
                SignatureProcessStatus.Expired => "Rejected",
                SignatureProcessStatus.Canceled => "Cancelled",
                _ => "Pending"
            };
        }
    }
}