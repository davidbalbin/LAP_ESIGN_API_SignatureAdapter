using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de eliminar una carpeta
/// </summary>
public class EliminarCarpetaResponse
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
}
