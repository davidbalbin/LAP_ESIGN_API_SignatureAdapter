using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de validar un documento PDF
/// </summary>
public class ValidarPdfResponse
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Lista de firmas en el documento
    /// </summary>
    [JsonPropertyName("firmas")]
    public List<FirmaInfo> Firmas { get; set; } = new List<FirmaInfo>();
}
