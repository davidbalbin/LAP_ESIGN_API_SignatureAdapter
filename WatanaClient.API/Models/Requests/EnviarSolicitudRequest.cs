using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para enviar una solicitud de firma a uno o más firmantes
/// </summary>
public class EnviarSolicitudRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.EnviarSolicitud;

    /// <summary>
    /// Código de la carpeta previamente preparada
    /// </summary>
    [JsonPropertyName("carpeta_codigo")]
    public string CarpetaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Código asignado a la solicitud de firma
    /// </summary>
    [JsonPropertyName("firma_codigo")]
    public string FirmaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Vigencia en horas de la solicitud (mínimo 1, máximo 168)
    /// </summary>
    [JsonPropertyName("vigencia_horas")]
    public int VigenciaHoras { get; set; } = 24;

    /// <summary>
    /// Indica si se debe reemplazar una solicitud existente con el mismo código
    /// </summary>
    [JsonPropertyName("reemplazar")]
    public bool Reemplazar { get; set; }

    /// <summary>
    /// Indica si la firma debe ser secuencial (cada firmante firma después del anterior)
    /// </summary>
    [JsonPropertyName("firma_secuencial")]
    public bool FirmaSecuencial { get; set; }

    /// <summary>
    /// Lista de firmantes con la configuración de sus firmas
    /// </summary>
    [JsonPropertyName("firmantes")]
    public List<FirmanteConfig> Firmantes { get; set; } = new List<FirmanteConfig>();
}
