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
        /// Maps Watana process status to application SignatureProcessStatus
        /// </summary>
        /// <param name="watanaStatus">The Watana status string</param>
        /// <returns>The mapped SignatureProcessStatus</returns>
        public static SignatureProcessStatus ToProcessStatus(this string watanaStatus) => watanaStatus switch
        {
            var state when state == WatanaStatuses.Firmado => SignatureProcessStatus.Completed,
            var state when state == WatanaStatuses.Rechazado => SignatureProcessStatus.Rejected,
            var state when state == WatanaStatuses.EnProceso => SignatureProcessStatus.WaitingForSignatures,
            var state when state == WatanaStatuses.EnEspera => SignatureProcessStatus.Pending,
            _ => SignatureProcessStatus.Pending
        };
    }
}