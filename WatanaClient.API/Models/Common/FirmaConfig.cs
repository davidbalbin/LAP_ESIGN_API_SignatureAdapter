using System.Text.Json.Serialization;

namespace WatanaClient.API.Models.Common
{
    /// <summary>
    /// Configuración de firma para un archivo específico
    /// </summary>
    public class FirmaConfig
    {
        /// <summary>
        /// Nombre del archivo a firmar
        /// </summary>
        [JsonPropertyName("archivo")]
        public string Archivo { get; set; } = string.Empty;

        /// <summary>
        /// Ubicación de la firma en el eje X (desde el margen inferior izquierdo)
        /// </summary>
        [JsonPropertyName("ubicacion_x")]
        public int UbicacionX { get; set; }

        /// <summary>
        /// Ubicación de la firma en el eje Y (desde el margen inferior izquierdo)
        /// </summary>
        [JsonPropertyName("ubicacion_y")]
        public int UbicacionY { get; set; }

        /// <summary>
        /// Largo de la representación visual de la firma
        /// </summary>
        [JsonPropertyName("largo")]
        public int Largo { get; set; } = 160;

        /// <summary>
        /// Alto de la representación visual de la firma
        /// </summary>
        [JsonPropertyName("alto")]
        public int Alto { get; set; } = 40;

        /// <summary>
        /// Número de página donde se mostrará la firma
        /// </summary>
        [JsonPropertyName("pagina")]
        public int Pagina { get; set; } = 1;

        /// <summary>
        /// Texto que se mostrará en la firma
        /// </summary>
        /// <remarks>
        /// Las etiquetas entre "&lt;" y "&gt;" serán reemplazadas con valores del certificado:
        /// &lt;FIRMANTE&gt;, &lt;ORGANIZACION&gt;, &lt;TITULO&gt;, &lt;CORREO&gt;, &lt;DIRECCION&gt;, &lt;FECHA&gt;
        /// </remarks>
        [JsonPropertyName("texto")]
        public string Texto { get; set; } = "Firmado digitalmente por: <FIRMANTE>\r\n<ORGANIZACION>\r\n<TITULO>\r\n<CORREO>\r\n<DIRECCION>\r\n<FECHA>\r\n Firmado";

        /// <summary>
        /// Motivo de la firma
        /// </summary>
        [JsonPropertyName("motivo")]
        public string Motivo { get; set; } = "Firma digital";

        /// <summary>
        /// Imagen para la representación impresa (opcional), zipeada en base64
        /// </summary>
        [JsonPropertyName("image_zip_base64")]
        public string? ImageZipBase64 { get; set; }
    }
}