using System;

namespace WatanaClient.API.Exceptions
{
    /// <summary>
    /// Excepción que se lanza cuando ocurre un error de validación en los datos
    /// </summary>
    public class WatanaValidationException : WatanaException
    {
        /// <summary>
        /// Nombre del parámetro que no pasó la validación
        /// </summary>
        public string? ParameterName { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaValidationException"/> con un mensaje de error específico
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        public WatanaValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaValidationException"/> con un mensaje de error
        /// específico y el nombre del parámetro que no pasó la validación
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        /// <param name="parameterName">Nombre del parámetro que no pasó la validación</param>
        public WatanaValidationException(string message, string parameterName) : base(message)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaValidationException"/> con un mensaje de error
        /// específico y una excepción interna que es la causa de esta excepción
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        /// <param name="innerException">Excepción que es la causa de la excepción actual</param>
        public WatanaValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaValidationException"/> con un mensaje de error
        /// específico, el nombre del parámetro que no pasó la validación y una excepción interna
        /// </summary>
        /// <param name="message">Mensaje de error que explica la razón de la excepción</param>
        /// <param name="parameterName">Nombre del parámetro que no pasó la validación</param>
        /// <param name="innerException">Excepción que es la causa de la excepción actual</param>
        public WatanaValidationException(string message, string parameterName, Exception innerException) : base(message, innerException)
        {
            ParameterName = parameterName;
        }
    }
}