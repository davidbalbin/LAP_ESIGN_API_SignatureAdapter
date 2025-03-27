using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests
{
    /// <summary>
    /// Solicitud para consultar el estado de una carpeta
    /// </summary>
    public class ConsultarCarpetaRequest
    {
        /// <summary>
        /// Tipo de operación
        /// </summary>
        [JsonPropertyName("operacion")]
        public string Operacion { get; } = ApiOperations.ConsultarCarpeta;

        /// <summary>
        /// Código de la carpeta a consultar
        /// </summary>
        [JsonPropertyName("carpeta_codigo")]
        public string CarpetaCodigo { get; set; } = string.Empty;
    }
}