using System;
using System.Net;

namespace WatanaClient.API.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando ocurre un error en la API Watana Firmador
    /// </summary>
    public class WatanaApiException : WatanaException
    {
        /// <summary>
        /// Código de estado HTTP devuelto por la API
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Contenido de la respuesta de error
        /// </summary>
        public string ResponseContent { get; }

        /// <summary>
        /// Código de error específico devuelto por la API (si está disponible)
        /// </summary>
        public string? ErrorCode { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaApiException"/>
        /// </summary>
        /// <param name="statusCode">Código de estado HTTP</param>
        /// <param name="message">Mensaje de error</param>
        /// <param name="responseContent">Contenido de la respuesta</param>
        /// <param name="errorCode">Código de error específico (opcional)</param>
        public WatanaApiException(HttpStatusCode statusCode, string message, string responseContent, string? errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaApiException"/>
        /// </summary>
        /// <param name="statusCode">Código de estado HTTP</param>
        /// <param name="message">Mensaje de error</param>
        /// <param name="responseContent">Contenido de la respuesta</param>
        /// <param name="innerException">Excepción interna</param>
        /// <param name="errorCode">Código de error específico (opcional)</param>
        public WatanaApiException(HttpStatusCode statusCode, string message, string responseContent, Exception innerException, string? errorCode = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Obtiene una representación en cadena de esta excepción
        /// </summary>
        /// <returns>Representación en cadena de la excepción</returns>
        public override string ToString()
        {
            return $"Status Code: {StatusCode}, Error: {Message}, Content: {ResponseContent}";
        }
    }
}