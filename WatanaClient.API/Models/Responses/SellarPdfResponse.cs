using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de aplicar un sello de tiempo a un documento PDF
/// </summary>
public class SellarPdfResponse
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje descriptivo del resultado de la operación
    /// </summary>
    [JsonPropertyName("mensaje")]
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>
    /// Documento PDF sellado zipeado en base64
    /// </summary>
    [JsonPropertyName("zip_base64")]
    public string ZipBase64 { get; set; } = string.Empty;
}
