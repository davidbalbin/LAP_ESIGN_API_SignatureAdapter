using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests
{
    /// <summary>
    /// Solicitud para enviar una carpeta con documentos para firma
    /// </summary>
    public class EnviarCarpetaRequest
    {
        /// <summary>
        /// Tipo de operación
        /// </summary>
        [JsonPropertyName("operacion")]
        public string Operacion { get; } = ApiOperations.EnviarCarpeta;

        /// <summary>
        /// Código asignado a la carpeta (mínimo 4, máximo 10 caracteres)
        /// </summary>
        [JsonPropertyName("carpeta_codigo")]
        public string CarpetaCodigo { get; set; } = string.Empty;

        /// <summary>
        /// Título de la carpeta
        /// </summary>
        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la carpeta
        /// </summary>
        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si se aplica sello de tiempo de LLAMA.PE a todos los archivos
        /// </summary>
        [JsonPropertyName("sello_de_tiempo")]
        public bool SelloTiempo { get; set; } = true;

        /// <summary>
        /// Observaciones adicionales para el firmante
        /// </summary>
        [JsonPropertyName("observaciones")]
        public string Observaciones { get; set; } = string.Empty;

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
        /// Indica si el firmante puede firmar sin necesidad de usar Watana App
        /// </summary>
        /// <remarks>
        /// Si es true, el firmante podrá completar la firma digital desde la web
        /// usando un certificado de Agente Automatizado de LLAMA.PE.
        /// </remarks>
        [JsonPropertyName("solo_firma_electronica_web")]
        public bool SoloFirmaElectronicaWeb { get; set; }

        /// <summary>
        /// Datos del firmante
        /// </summary>
        [JsonPropertyName("firmante")]
        public Firmante Firmante { get; set; } = new Firmante();

        /// <summary>
        /// Lista de archivos para firmar
        /// </summary>
        [JsonPropertyName("archivos")]
        public List<Archivo> Archivos { get; set; } = new List<Archivo>();
    }
}