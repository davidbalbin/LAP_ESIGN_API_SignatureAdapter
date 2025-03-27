using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WatanaClient.API.Models.Common;

namespace WatanaClient.API.Models.Responses
{
    /// <summary>
    /// Respuesta a la solicitud de consultar el estado de una carpeta
    /// </summary>
    public class ConsultarCarpetaResponse
    {
        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje descriptivo del resultado de la operación
        /// </summary>
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>
        /// Número de solicitud asignado por Watana
        /// </summary>
        [JsonPropertyName("solicitud_numero")]
        public string SolicitudNumero { get; set; } = string.Empty;

        /// <summary>
        /// Estado de la carpeta (en-espera, firmado, rechazado-por-firmante, eliminado-por-firmante, eliminado-por-solicitante)
        /// </summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;
    }

    /// <summary>
    /// Información de un archivo en una carpeta
    /// </summary>
    public class ArchivoInfo
    {
        /// <summary>
        /// Nombre del archivo
        /// </summary>
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Contenido del archivo comprimido en ZIP y codificado en Base64
        /// </summary>
        [JsonPropertyName("zip_base64")]
        public string ZipBase64 { get; set; } = string.Empty;
    }

    
    
    /// <summary>
    /// Información de una firma en un documento PDF
    /// </summary>
    public class FirmaInfo
    {
        /// <summary>
        /// Tipo de firma (firma_digital o sello_tiempo)
        /// </summary>
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Detalle de quién firmó el documento
        /// </summary>
        [JsonPropertyName("detalle")]
        public string Detalle { get; set; } = string.Empty;

        /// <summary>
        /// Titular del certificado digital usado para la firma
        /// </summary>
        [JsonPropertyName("titular")]
        public string Titular { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el documento mantiene su integridad desde la firma
        /// </summary>
        [JsonPropertyName("integridad")]
        public bool Integridad { get; set; }

        /// <summary>
        /// Mensaje de error si hay problemas de integridad
        /// </summary>
        [JsonPropertyName("integridad_error")]
        public string IntegridadError { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de la firma
        /// </summary>
        [JsonPropertyName("fecha_hora")]
        public string FechaHora { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de inicio de vigencia del certificado
        /// </summary>
        [JsonPropertyName("valido_desde")]
        public string ValidoDesde { get; set; } = string.Empty;

        /// <summary>
        /// Número de serie del certificado
        /// </summary>
        [JsonPropertyName("numero_serie")]
        public string NumeroSerie { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de fin de vigencia del certificado
        /// </summary>
        [JsonPropertyName("valido_hasta")]
        public string ValidoHasta { get; set; } = string.Empty;

        /// <summary>
        /// Algoritmo usado para generar el hash del documento
        /// </summary>
        [JsonPropertyName("algoritmo_resumen")]
        public string AlgoritmoResumen { get; set; } = string.Empty;

        /// <summary>
        /// Algoritmo usado para la firma
        /// </summary>
        [JsonPropertyName("algoritmo_firma")]
        public string AlgoritmoFirma { get; set; } = string.Empty;

        /// <summary>
        /// Indica si la firma cubre todo el documento
        /// </summary>
        [JsonPropertyName("cubre_todo_documento")]
        public bool CubreTodoDocumento { get; set; }

        /// <summary>
        /// Indica si el certificado no está revocado
        /// </summary>
        [JsonPropertyName("valido_revocacion")]
        public bool ValidoRevocacion { get; set; }

        /// <summary>
        /// Mensaje de error si hay problemas de revocación
        /// </summary>
        [JsonPropertyName("valido_revocacion_error")]
        public string ValidoRevocacionError { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el certificado está en la lista de confianza de INDECOPI
        /// </summary>
        [JsonPropertyName("valido_indecopi_tsl")]
        public bool ValidoIndecopiTsl { get; set; }

        /// <summary>
        /// Mensaje de error si hay problemas con la lista de confianza
        /// </summary>
        [JsonPropertyName("valido_indecopi_tsl_error")]
        public string ValidoIndecopiTslError { get; set; } = string.Empty;

        /// <summary>
        /// Autoridad de certificación que emitió el certificado
        /// </summary>
        [JsonPropertyName("autoridad_certificacion")]
        public string AutoridadCertificacion { get; set; } = string.Empty;

        /// <summary>
        /// Formato de la firma (PAdES-BASELINE-LTA, TIMESTAMP, etc.)
        /// </summary>
        [JsonPropertyName("formato")]
        public string Formato { get; set; } = string.Empty;

        /// <summary>
        /// Clave pública del certificado
        /// </summary>
        [JsonPropertyName("clave_publica")]
        public ClavePublica ClavePublica { get; set; } = new ClavePublica();

        /// <summary>
        /// Cadena de certificados usados para la firma
        /// </summary>
        [JsonPropertyName("ruta_certificacion")]
        public List<string> RutaCertificacion { get; set; } = new List<string>();
    }

    /// <summary>
    /// Información de la clave pública de un certificado
    /// </summary>
    public class ClavePublica
    {
        /// <summary>
        /// Clave pública zipeada en base64
        /// </summary>
        [JsonPropertyName("zip_base64")]
        public string ZipBase64 { get; set; } = string.Empty;
    }

    /// <summary>
    /// Información de un archivo preparado para firma
    /// </summary>
    public class ArchivoPrepared
    {
        /// <summary>
        /// Token único asignado al archivo
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Tamaño del archivo
        /// </summary>
        [JsonPropertyName("size")]
        public string Size { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Información de una solicitud de firma
    /// </summary>
    public class SolicitudInfo
    {
        /// <summary>
        /// Número de solicitud asignado por Watana
        /// </summary>
        [JsonPropertyName("numero")]
        public string Numero { get; set; } = string.Empty;

        /// <summary>
        /// Email del firmante asignado a esta solicitud
        /// </summary>
        [JsonPropertyName("firmante")]
        public string Firmante { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de firmas que debe realizar el firmante
        /// </summary>
        [JsonPropertyName("cantidad_de_firmas")]
        public int CantidadDeFirmas { get; set; }

        /// <summary>
        /// Enlace para compartir con el firmante
        /// </summary>
        [JsonPropertyName("enlace_para_firmar")]
        public string EnlaceParaFirmar { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// Información detallada de una solicitud de firma
    /// </summary>
    public class SolicitudDetail
    {
        /// <summary>
        /// Número de solicitud asignado por Watana
        /// </summary>
        [JsonPropertyName("numero")]
        public string Numero { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de vencimiento de la solicitud
        /// </summary>
        [JsonPropertyName("vencimiento")]
        public string Vencimiento { get; set; } = string.Empty;

        /// <summary>
        /// Email del solicitante
        /// </summary>
        [JsonPropertyName("solicitante")]
        public string Solicitante { get; set; } = string.Empty;

        /// <summary>
        /// Email del firmante
        /// </summary>
        [JsonPropertyName("firmante")]
        public string Firmante { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de firmas que debe realizar el firmante
        /// </summary>
        [JsonPropertyName("cantidad_de_firmas")]
        public int CantidadDeFirmas { get; set; }

        /// <summary>
        /// Estado de la solicitud
        /// </summary>
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;
    }

}