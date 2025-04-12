namespace LapDrive.SignatureAdapter.Models.Extensions
{
    using LapDrive.SignatureAdapter.Models.Enums;

    /// <summary>
    /// Extension methods for ProcessStatus enum
    /// </summary>
    public static class ProcessStatusExtensions
    {
        /// <summary>
        /// Converts ProcessStatus to a descriptive string
        /// </summary>
        /// <param name="status">The process status</param>
        /// <returns>A descriptive string representation of the status</returns>
        public static string ToDisplayString(this ProcessStatus status)
        {
            return status switch
            {
                ProcessStatus.Pending => "Pendiente",
                ProcessStatus.InProgress => "En proceso",
                ProcessStatus.WaitingForSignatures => "Esperando firmas",
                ProcessStatus.Completed => "Completado",
                ProcessStatus.Rejected => "Rechazado",
                ProcessStatus.Cancelled => "Cancelado",
                _ => "Estado desconocido"
            };
        }
    }
}