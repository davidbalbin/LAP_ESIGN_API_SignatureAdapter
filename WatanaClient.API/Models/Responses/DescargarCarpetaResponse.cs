using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de descargar los archivos de una carpeta
/// </summary>
public class DescargarCarpetaResponse
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
    /// Número de solicitud asignado por Watana
    /// </summary>
    [JsonPropertyName("solicitud_numero")]
    public string SolicitudNumero { get; set; } = string.Empty;

    /// <summary>
    /// Lista de archivos de la carpeta
    /// </summary>
    [JsonPropertyName("archivos")]
    public List<ArchivoInfo> Archivos { get; set; } = new List<ArchivoInfo>();
}
