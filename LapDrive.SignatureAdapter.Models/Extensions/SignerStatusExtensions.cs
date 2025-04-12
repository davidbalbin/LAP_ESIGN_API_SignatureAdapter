namespace LapDrive.SignatureAdapter.Models.Extensions
{
    using LapDrive.SignatureAdapter.Models.Enums;

    /// <summary>
    /// Extension methods for SignerStatus enum
    /// </summary>
    public static class SignerStatusExtensions
    {
        /// <summary>
        /// Converts SignerStatus to a descriptive string
        /// </summary>
        /// <param name="status">The signer status</param>
        /// <returns>A descriptive string representation of the status</returns>
        public static string ToDisplayString(this SignerStatus status)
        {
            return status switch
            {
                SignerStatus.Pending => "Pendiente de firma",
                SignerStatus.Signed => "Firmado",
                SignerStatus.Rejected => "Rechazado",
                SignerStatus.Cancelled => "Cancelado",
                SignerStatus.InProgress => "En proceso de firma",
                _ => "Estado desconocido"
            };
        }
    }
}