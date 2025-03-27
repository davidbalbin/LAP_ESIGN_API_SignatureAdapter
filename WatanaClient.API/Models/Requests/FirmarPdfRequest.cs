using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Constants;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Requests;

/// <summary>
/// Solicitud para firmar un documento PDF con un certificado de agente automatizado
/// </summary>
public class FirmarPdfRequest
{
    /// <summary>
    /// Tipo de operación
    /// </summary>
    [JsonPropertyName("operacion")]
    public string Operacion { get; } = ApiOperations.FirmarPdf;

    /// <summary>
    /// Indica si se aplica sello de tiempo de LLAMA.PE al documento
    /// </summary>
    [JsonPropertyName("sello_de_tiempo")]
    public bool SelloTiempo { get; set; } = true;

    /// <summary>
    /// Configuración de la representación visual de la firma
    /// </summary>
    [JsonPropertyName("firma_visual")]
    public FirmaVisualRequest? FirmaVisual { get; set; }

    /// <summary>
    /// Documento PDF zipeado en base64
    /// </summary>
    [JsonPropertyName("zip_base64")]
    public string ZipBase64 { get; set; } = string.Empty;
}
