using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses;

/// <summary>
/// Respuesta a la solicitud de consultar el estado de una solicitud de firma
/// </summary>
public class ConsultarSolicitudResponse
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
    /// Código de la solicitud de firma consultada
    /// </summary>
    [JsonPropertyName("firma_codigo")]
    public string FirmaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Lista de solicitudes asociadas al código consultado
    /// </summary>
    [JsonPropertyName("solicitudes")]
    public List<SolicitudDetail> Solicitudes { get; set; } = new List<SolicitudDetail>();
}
