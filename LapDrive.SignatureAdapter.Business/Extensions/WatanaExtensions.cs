using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Business.Extensions
{
    /// <summary>
    /// Extension methods for Watana status mapping
    /// </summary>
    public static class WatanaExtensions
    {
        /// <summary>
        /// Maps Watana signer status to application SignerStatus
        /// </summary>
        /// <param name="watanaStatus">The Watana status string</param>
        /// <returns>The mapped SignerStatus</returns>
        public static SignerStatus ToSignerStatus(this string watanaStatus) => watanaStatus switch
        {
            var state when state == WatanaStatuses.Firmado => SignerStatus.Signed,
            var state when state == WatanaStatuses.Rechazado => SignerStatus.Rejected,
            var state when state == WatanaStatuses.EnProceso => SignerStatus.InProgress,
            var state when state == WatanaStatuses.EnEspera => SignerStatus.Pending,
            _ => SignerStatus.Pending
        };

        /// <summary>
        /// Maps Watana process status to application ProcessStatus
        /// </summary>
        /// <param name="watanaStatus">The Watana status string</param>
        /// <returns>The mapped ProcessStatus</returns>
        public static ProcessStatus ToProcessStatus(this string watanaStatus) => watanaStatus switch
        {
            var state when state == WatanaStatuses.Firmado => ProcessStatus.Completed,
            var state when state == WatanaStatuses.Rechazado => ProcessStatus.Rejected,
            var state when state == WatanaStatuses.EnProceso => ProcessStatus.InProgress,
            var state when state == WatanaStatuses.EnEspera => ProcessStatus.WaitingForSignatures,
            _ => ProcessStatus.Pending
        };
    }
}