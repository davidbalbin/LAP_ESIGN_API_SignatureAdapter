using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.Extensions
{
    /// <summary>
    /// Extension methods for SignatureProcessStatus enum
    /// </summary>
    public static class SignatureStatusExtensions
    {
        /// <summary>
        /// Converts the status enum to a display string
        /// </summary>
        public static string ToDisplayString(this SignatureProcessStatus status)
        {
            return status switch
            {
                SignatureProcessStatus.Pending => "Pendiente",
                SignatureProcessStatus.InProgress => "En Proceso",
                SignatureProcessStatus.Completed => "Completado",
                SignatureProcessStatus.Rejected => "Rechazado",
                SignatureProcessStatus.Expired => "Expirado",
                SignatureProcessStatus.WaitingForSignatures => "Esperando Firmas",
                SignatureProcessStatus.Canceled => "Cancelado",
                _ => "Desconocido"
            };
        }
    }
}