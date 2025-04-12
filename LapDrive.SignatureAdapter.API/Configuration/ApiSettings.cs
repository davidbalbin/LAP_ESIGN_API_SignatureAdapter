namespace LapDrive.SignatureAdapter.API.Configuration
{
    public class ApiSettings
    {
        public string Title { get; set; } = "LapDrive Signature Adapter API";
        public string Version { get; set; } = "v1";
        public string Description { get; set; } = "API for digital signature processes";
        public ContactInfo Contact { get; set; } = new();
    }

    public class ContactInfo
    {
        public string Name { get; set; } = "Development Team";
        public string Email { get; set; } = string.Empty;
    }
}