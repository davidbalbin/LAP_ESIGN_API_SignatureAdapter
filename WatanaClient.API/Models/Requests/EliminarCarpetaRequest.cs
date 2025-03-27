using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

 /// <summary>
/// Solicitud para eliminar una carpeta
/// </summary>
public class EliminarCarpetaRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.EliminarCarpeta;

    /// <summary>
    /// Código de la carpeta a eliminar
    /// </summary>
    [JsonPropertyName("carpeta_codigo")]
    public string CarpetaCodigo { get; set; } = string.Empty;
}
