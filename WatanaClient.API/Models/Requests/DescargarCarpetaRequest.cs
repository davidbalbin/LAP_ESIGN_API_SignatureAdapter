using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para descargar los archivos de una carpeta
/// </summary>
public class DescargarCarpetaRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.DescargarCarpeta;

    /// <summary>
    /// Código de la carpeta a descargar
    /// </summary>
    [JsonPropertyName("carpeta_codigo")]
    public string CarpetaCodigo { get; set; } = string.Empty;
}
