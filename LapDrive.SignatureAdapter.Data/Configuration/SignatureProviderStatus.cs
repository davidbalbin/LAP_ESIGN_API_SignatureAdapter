namespace LapDrive.SignatureAdapter.Data.Configuration
{
    public class SignatureProviderStatus
    {
        public string Signed { get; set; } = string.Empty;
        public string Rejected { get; set; } = string.Empty;
        public string InProcess { get; set; } = string.Empty;
        public string Waiting { get; set; } = string.Empty;
    }
}