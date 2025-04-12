namespace LapDrive.SignatureAdapter.Models.Constants
{
    public static class CommonConstants
    {
        public static class ContentTypes
        {
            public const string ApplicationJson = "application/json";
            public const string ApplicationZip = "application/x-zip-compressed";
            public const string ApplicationPdf = "application/pdf";
            public const string TextPlain = "text/plain";
        }

        public static class DocumentTypes
        {
            public const string File = "file";
            public const string Folder = "folder";
        }

        public static class ProcessStatuses
        {
            public const string InProgress = "InProgress";
            public const string Pending = "Pending";
            public const string Completed = "Completed";
            public const string Rejected = "Rejected";
            public const string Cancelled = "Cancelled";
        }

        public static class WatanaStatuses
        {
            public const string EnProceso = "en-proceso";
            public const string EnEspera = "en-espera";
            public const string Firmado = "firmado";
            public const string Rechazado = "rechazado-por-firmante";
        }
    }
}