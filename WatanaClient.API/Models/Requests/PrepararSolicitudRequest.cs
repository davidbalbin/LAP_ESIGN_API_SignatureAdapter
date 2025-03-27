using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para preparar una solicitud con múltiples archivos
/// </summary>
public class PrepararSolicitudRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.PrepararSolicitud;

    /// <summary>
    /// Código asignado a la carpeta (mínimo 4, máximo 10 caracteres)
    /// </summary>
    [JsonPropertyName("carpeta_codigo")]
    public string CarpetaCodigo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de la carpeta
    /// </summary>
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Lista de archivos para la solicitud
    /// </summary>
    [JsonPropertyName("archivos")]
    public List<Archivo> Archivos { get; set; } = new List<Archivo>();
}
