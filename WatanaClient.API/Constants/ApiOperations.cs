namespace WatanaClient.API.Constants
{
    /// <summary>
    /// Constantes que representan las operaciones disponibles en la API Watana Firmador
    /// </summary>
    public static class ApiOperations
    {
        /// <summary>
        /// Operación para enviar una carpeta con documentos para firma
        /// </summary>
        public const string EnviarCarpeta = "enviar_carpeta";

        /// <summary>
        /// Operación para consultar el estado de una carpeta
        /// </summary>
        public const string ConsultarCarpeta = "consultar_carpeta";

        /// <summary>
        /// Operación para descargar el contenido de una carpeta
        /// </summary>
        public const string DescargarCarpeta = "descargar_carpeta";

        /// <summary>
        /// Operación para eliminar una carpeta
        /// </summary>
        public const string EliminarCarpeta = "eliminar_carpeta";

        /// <summary>
        /// Operación para validar un documento PDF
        /// </summary>
        public const string ValidarPdf = "validar_pdf";

        /// <summary>
        /// Operación para firmar un documento PDF con un certificado de agente automatizado
        /// </summary>
        public const string FirmarPdf = "firmar_pdf";

        /// <summary>
        /// Operación para aplicar un sello de tiempo a un documento PDF
        /// </summary>
        public const string SellarPdf = "sellar_pdf";

        /// <summary>
        /// Operación para preparar una solicitud de firma
        /// </summary>
        public const string PrepararSolicitud = "preparar_solicitud";

        /// <summary>
        /// Operación para enviar una solicitud de firma a uno o más firmantes
        /// </summary>
        public const string EnviarSolicitud = "enviar_solicitud";

        /// <summary>
        /// Operación para consultar el estado de una solicitud de firma
        /// </summary>
        public const string ConsultarSolicitud = "consultar_solicitud";
    }
}