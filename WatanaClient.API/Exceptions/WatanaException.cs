using System;

namespace WatanaClient.API.Exceptions
{
    /// <summary>
    /// Excepción base para todos los errores relacionados con la API Watana Firmador
    /// </summary>
    public class WatanaException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaException"/> con un mensaje de error específico
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        public WatanaException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaException"/> con un mensaje de error
        /// específico y una excepción interna que es la causa de esta excepción
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        /// <param name="innerException">Excepción que es la causa de la excepción actual</param>
        public WatanaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}