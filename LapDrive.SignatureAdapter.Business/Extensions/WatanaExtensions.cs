using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Business.Extensions
{
    /// <summary>
    /// Extension methods for Watana specific mappings
    /// </summary>
    public static class WatanaExtensions
    {
        /// <summary>
        /// Maps Watana state to standard SignerStatus
        /// </summary>
        /// <param name="watanaState">The state from Watana</param>
        /// <returns>The standardized signer status</returns>
        public static SignerStatus ToSignerStatus(this string watanaState)
        {
            return watanaState switch
            {
                var state when state == CommonConstants.WatanaStatuses.Firmado => SignerStatus.Signed,
                var state when state == CommonConstants.WatanaStatuses.Rechazado => SignerStatus.Rejected,
                var state when state == CommonConstants.WatanaStatuses.EnProceso => SignerStatus.InProgress,
                var state when state == CommonConstants.WatanaStatuses.EnEspera => SignerStatus.Pending,
                _ => SignerStatus.Pending
            };
        }

        /// <summary>
        /// Maps Watana state to standard ProcessStatus
        /// </summary>
        /// <param name="watanaState">The state from Watana</param>
        /// <returns>The standardized process status</returns>
        public static ProcessStatus ToProcessStatus(this string watanaState)
        {
            return watanaState switch
            {
                var state when state == CommonConstants.WatanaStatuses.Firmado => ProcessStatus.Completed,
                var state when state == CommonConstants.WatanaStatuses.Rechazado => ProcessStatus.Rejected,
                var state when state == CommonConstants.WatanaStatuses.EnProceso => ProcessStatus.InProgress,
                var state when state == CommonConstants.WatanaStatuses.EnEspera => ProcessStatus.WaitingForSignatures,
                _ => ProcessStatus.Pending
            };
        }
    }
}