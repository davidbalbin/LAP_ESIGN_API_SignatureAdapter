using LapDrive.SignatureAdapter.Models.Constants;
using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Data.Mappers;

/// <summary>
/// Maps between Watana status strings and SignatureProcessStatus enum
/// </summary>
public static class WatanaStatusMapper
{
    /// <summary>
    /// Maps a Watana status string to SignatureProcessStatus
    /// </summary>
    public static SignatureProcessStatus ToSignatureProcessStatus(string? watanaStatus)
    {
        if (string.IsNullOrEmpty(watanaStatus))
            return SignatureProcessStatus.Pending;

        return watanaStatus switch
        {
            WatanaStatuses.EnProceso => SignatureProcessStatus.WaitingForSignatures,
            WatanaStatuses.EnEspera => SignatureProcessStatus.Pending,
            WatanaStatuses.Firmado => SignatureProcessStatus.Completed,
            WatanaStatuses.Rechazado => SignatureProcessStatus.Rejected,
            _ => SignatureProcessStatus.Pending
        };
    }

    /// <summary>
    /// Maps a SignatureProcessStatus to Watana status string
    /// </summary>
    public static string ToWatanaStatus(SignatureProcessStatus status)
    {
        return status switch
        {
            SignatureProcessStatus.WaitingForSignatures => WatanaStatuses.EnProceso,
            SignatureProcessStatus.Pending => WatanaStatuses.EnEspera,
            SignatureProcessStatus.Completed => WatanaStatuses.Firmado,
            SignatureProcessStatus.Rejected => WatanaStatuses.Rechazado,
            SignatureProcessStatus.InProgress => WatanaStatuses.EnProceso,
            _ => WatanaStatuses.EnEspera
        };
    }
}