using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de preparar una solicitud con múltiples archivos
/// </summary>
public class PrepararSolicitudResponse
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
    /// Código de la carpeta
    /// </summary>
    [JsonPropertyName("carpeta_codigo")]
    public string CarpetaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Lista de archivos preparados
    /// </summary>
    [JsonPropertyName("archivos")]
    public List<ArchivoPrepared> Archivos { get; set; } = new List<ArchivoPrepared>();
}
