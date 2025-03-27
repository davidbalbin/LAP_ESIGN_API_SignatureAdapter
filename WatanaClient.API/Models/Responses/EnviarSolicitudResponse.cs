using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de enviar una solicitud de firma
/// </summary>
public class EnviarSolicitudResponse
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
    /// Email del solicitante
    /// </summary>
    [JsonPropertyName("solicitante")]
    public string Solicitante { get; set; } = string.Empty;

    /// <summary>
    /// Código asignado a la solicitud de firma
    /// </summary>
    [JsonPropertyName("firma_codigo")]
    public string FirmaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Lista de solicitudes generadas para cada firmante
    /// </summary>
    [JsonPropertyName("solicitudes")]
    public List<SolicitudInfo> Solicitudes { get; set; } = new List<SolicitudInfo>();
}
