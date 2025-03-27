using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de firmar un documento PDF
/// </summary>
public class FirmarPdfResponse
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Documento PDF firmado zipeado en base64
    /// </summary>
    [JsonPropertyName("zip_base64")]
    public string ZipBase64 { get; set; } = string.Empty;
}
