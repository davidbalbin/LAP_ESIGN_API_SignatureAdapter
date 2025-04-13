using System.Text.Json.Serialization;
using LapDrive.SignatureAdapter.Models.Enums;

namespace LapDrive.SignatureAdapter.Models.DTOs.Response
{
    /// <summary>
    /// Response model for signature process status
    /// </summary>
    public class SignatureProcessStatusResponse
    {
        /// <summary>
        /// Gets or sets the current state
        /// </summary>
        /// <summary>
        /// Gets or sets the raw status string (from Watana)
        /// </summary>
        [JsonPropertyName("estado")]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status as enum
        /// </summary>
        [JsonPropertyName("statusEnum")]
        public SignatureProcessStatus StatusEnum { get; set; } = SignatureProcessStatus.Pending;

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [JsonPropertyName("titulo")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of signers
        /// </summary>
        [JsonPropertyName("firmantes")]
        public List<SignerInfo> Signers { get; set; } = new List<SignerInfo>();
    }
}