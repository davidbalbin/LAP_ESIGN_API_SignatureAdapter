using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para consultar el estado de una solicitud de firma
/// </summary>
public class ConsultarSolicitudRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.ConsultarSolicitud;

    /// <summary>
    /// Código de la solicitud de firma o número de solicitud generado por la API
    /// </summary>
    [JsonPropertyName("firma_codigo")]
    public string FirmaCodigo { get; set; } = string.Empty;
}