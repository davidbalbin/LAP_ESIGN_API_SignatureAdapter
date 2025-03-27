using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para validar un documento PDF
/// </summary>
public class ValidarPdfRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.ValidarPdf;

    /// <summary>
    /// Documento PDF zipeado en base64
    /// </summary>
    [JsonPropertyName("zip_base64")]
    public string ZipBase64 { get; set; } = string.Empty;
}
