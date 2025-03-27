namespace WatanaClient.API.Constants
{
    /// <summary>
    /// Constantes generales utilizadas en la API Watana Firmador
    /// </summary>
    public static class WatanaConstants
    {
        /// <summary>
        /// Estados posibles para las carpetas y solicitudes
        /// </summary>
        public static class Estados
        {
            /// <summary>
            /// La solicitud está pendiente de firma
            /// </summary>
            public const string EnEspera = "en-espera";

            /// <summary>
            /// La solicitud está en proceso de firma
            /// </summary>
            public const string EnProceso = "en-proceso";

            /// <summary>
            /// La solicitud ha sido firmada completamente
            /// </summary>
            public const string Firmado = "firmado";

            /// <summary>
            /// La solicitud ha sido rechazada por el firmante
            /// </summary>
            public const string RechazadoPorFirmante = "rechazado-por-firmante";

            /// <summary>
            /// La solicitud ha sido eliminada por el firmante
            /// </summary>
            public const string EliminadoPorFirmante = "eliminado-por-firmante";

            /// <summary>
            /// La solicitud ha sido eliminada por el solicitante
            /// </summary>
            public const string EliminadoPorSolicitante = "eliminado-por-solicitante";
        }

        /// <summary>
        /// Tipos de eventos para las notificaciones de webhook
        /// </summary>
        public static class EventosWebhook
        {
            /// <summary>
            /// Evento cuando un documento es firmado
            /// </summary>
            public const string Firmado = "firmado";

            /// <summary>
            /// Evento cuando un documento es rechazado por el firmante
            /// </summary>
            public const string RechazadoPorFirmante = "rechazado-por-firmante";

            /// <summary>
            /// Evento cuando ocurre un error durante el proceso de firma
            /// </summary>
            public const string Error = "error";
        }

        /// <summary>
        /// Tipos de firmas en los documentos
        /// </summary>
        public static class TiposFirma
        {
            /// <summary>
            /// Firma digital
            /// </summary>
            public const string FirmaDigital = "firma_digital";

            /// <summary>
            /// Sello de tiempo
            /// </summary>
            public const string SelloTiempo = "sello_tiempo";
        }

        /// <summary>
        /// Límites para diversos valores en la API
        /// </summary>
        public static class Limites
        {
            /// <summary>
            /// Longitud mínima para el código de carpeta
            /// </summary>
            public const int MinLongitudCodigoCarpeta = 4;

            /// <summary>
            /// Longitud máxima para el código de carpeta
            /// </summary>
            public const int MaxLongitudCodigoCarpeta = 10;

            /// <summary>
            /// Valor mínimo para la vigencia en horas
            /// </summary>
            public const int MinVigenciaHoras = 1;

            /// <summary>
            /// Valor máximo para la vigencia en horas
            /// </summary>
            public const int MaxVigenciaHoras = 168;

            /// <summary>
            /// Tamaño máximo (en MB) para la respuesta en JSON
            /// </summary>
            public const int MaxTamanoRespuestaJsonMB = 100;
        }

        /// <summary>
        /// Cabeceras HTTP utilizadas en las solicitudes
        /// </summary>
        public static class HttpHeaders
        {
            /// <summary>
            /// Cabecera para el token de autenticación
            /// </summary>
            public const string Authorization = "Authorization";

            /// <summary>
            /// Cabecera para el tipo de contenido
            /// </summary>
            public const string ContentType = "Content-Type";

            /// <summary>
            /// Valor para el tipo de contenido JSON
            /// </summary>
            public const string ApplicationJson = "application/json";

            /// <summary>
            /// Valor para el tipo de contenido ZIP
            /// </summary>
            public const string ApplicationZip = "application/x-zip-compressed";
        }
    }
}