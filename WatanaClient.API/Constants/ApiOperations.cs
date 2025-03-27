namespace WatanaClient.API.Constants
{
    /// <summary>
    /// Constantes que representan las operaciones disponibles en la API Watana Firmador
    /// </summary>
    public static class ApiOperations
    {
        /// <summary>
        /// Operaci�n para enviar una carpeta con documentos para firma
        /// </summary>
        public const string EnviarCarpeta = "enviar_carpeta";

        /// <summary>
        /// Operaci�n para consultar el estado de una carpeta
        /// </summary>
        public const string ConsultarCarpeta = "consultar_carpeta";

        /// <summary>
        /// Operaci�n para descargar el contenido de una carpeta
        /// </summary>
        public const string DescargarCarpeta = "descargar_carpeta";

        /// <summary>
        /// Operaci�n para eliminar una carpeta
        /// </summary>
        public const string EliminarCarpeta = "eliminar_carpeta";

        /// <summary>
        /// Operaci�n para validar un documento PDF
        /// </summary>
        public const string ValidarPdf = "validar_pdf";

        /// <summary>
        /// Operaci�n para firmar un documento PDF con un certificado de agente automatizado
        /// </summary>
        public const string FirmarPdf = "firmar_pdf";

        /// <summary>
        /// Operaci�n para aplicar un sello de tiempo a un documento PDF
        /// </summary>
        public const string SellarPdf = "sellar_pdf";

        /// <summary>
        /// Operaci�n para preparar una solicitud de firma
        /// </summary>
        public const string PrepararSolicitud = "preparar_solicitud";

        /// <summary>
        /// Operaci�n para enviar una solicitud de firma a uno o m�s firmantes
        /// </summary>
        public const string EnviarSolicitud = "enviar_solicitud";

        /// <summary>
        /// Operaci�n para consultar el estado de una solicitud de firma
        /// </summary>
        public const string ConsultarSolicitud = "consultar_solicitud";
    }
}