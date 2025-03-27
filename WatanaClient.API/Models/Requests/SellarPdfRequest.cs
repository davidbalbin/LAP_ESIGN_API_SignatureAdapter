using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para aplicar un sello de tiempo a un documento PDF
/// </summary>
public class SellarPdfRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.SellarPdf;

    /// <summary>
    /// Documento PDF zipeado en base64
    /// </summary>
    [JsonPropertyName("zip_base64")]
    public string ZipBase64 { get; set; } = string.Empty;
}
