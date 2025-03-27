using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using WatanaClient.API.Constants;
using WatanaClient.API.Exceptions;

namespace WatanaClient.API.Utils
{
    /// <summary>
    /// Utilidades para validar datos
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Valida un código de carpeta
        /// </summary>
        /// <param name="carpetaCodigo">Código de carpeta a validar</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el código de carpeta no es válido</exception>
        public static void ValidateCarpetaCodigo(string carpetaCodigo)
        {
            if (string.IsNullOrEmpty(carpetaCodigo))
                throw new WatanaValidationException("El código de carpeta es obligatorio", nameof(carpetaCodigo));

            if (carpetaCodigo.Length < WatanaConstants.Limites.MinLongitudCodigoCarpeta ||
                carpetaCodigo.Length > WatanaConstants.Limites.MaxLongitudCodigoCarpeta)
                throw new WatanaValidationException(
                    $"El código de carpeta debe tener entre {WatanaConstants.Limites.MinLongitudCodigoCarpeta} y {WatanaConstants.Limites.MaxLongitudCodigoCarpeta} caracteres",
                    nameof(carpetaCodigo)
                );

            // Validar que solo contenga caracteres alfanuméricos
            if (!Regex.IsMatch(carpetaCodigo, @"^[a-zA-Z0-9_\-]+$"))
                throw new WatanaValidationException(
                    "El código de carpeta solo puede contener letras, números, guiones y guiones bajos",
                    nameof(carpetaCodigo)
                );
        }

        /// <summary>
        /// Valida un email
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el email no es válido</exception>
        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new WatanaValidationException("El email es obligatorio", nameof(email));

            try
            {
                var mailAddress = new MailAddress(email);
                if (mailAddress.Address != email)
                    throw new WatanaValidationException("El email no es válido", nameof(email));
            }
            catch (FormatException)
            {
                throw new WatanaValidationException("El email no es válido", nameof(email));
            }
        }

        /// <summary>
        /// Valida un número de horas de vigencia
        /// </summary>
        /// <param name="vigenciaHoras">Número de horas de vigencia a validar</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el número de horas no es válido</exception>
        public static void ValidateVigenciaHoras(int vigenciaHoras)
        {
            if (vigenciaHoras < WatanaConstants.Limites.MinVigenciaHoras || 
                vigenciaHoras > WatanaConstants.Limites.MaxVigenciaHoras)
                throw new WatanaValidationException(
                    $"La vigencia en horas debe estar entre {WatanaConstants.Limites.MinVigenciaHoras} y {WatanaConstants.Limites.MaxVigenciaHoras}",
                    nameof(vigenciaHoras)
                );
        }

        /// <summary>
        /// Valida una cadena Base64
        /// </summary>
        /// <param name="base64">Cadena Base64 a validar</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando la cadena Base64 no es válida</exception>
        public static void ValidateBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                throw new WatanaValidationException("La cadena Base64 es obligatoria", nameof(base64));

            try
            {
                Convert.FromBase64String(base64);
            }
            catch (FormatException)
            {
                throw new WatanaValidationException("La cadena Base64 no es válida", nameof(base64));
            }
        }

        /// <summary>
        /// Valida una posición en un documento PDF
        /// </summary>
        /// <param name="position">Posición a validar</param>
        /// <param name="parameterName">Nombre del parámetro</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando la posición no es válida</exception>
        public static void ValidatePosition(int? position, string parameterName)
        {
            if (!position.HasValue)
                throw new WatanaValidationException($"La posición '{parameterName}' es obligatoria", parameterName);

            if (position.Value < 0)
                throw new WatanaValidationException($"La posición '{parameterName}' no puede ser negativa", parameterName);
        }

        /// <summary>
        /// Valida un número de página
        /// </summary>
        /// <param name="pagina">Número de página a validar</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el número de página no es válido</exception>
        public static void ValidatePagina(int pagina)
        {
            if (pagina <= 0)
                throw new WatanaValidationException("El número de página debe ser mayor que cero", nameof(pagina));
        }

        /// <summary>
        /// Valida que un valor no sea nulo o vacío
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="parameterName">Nombre del parámetro</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el valor es nulo o vacío</exception>
        public static void ValidateNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new WatanaValidationException($"El valor '{parameterName}' es obligatorio", parameterName);
        }

        /// <summary>
        /// Valida que un valor no sea nulo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="parameterName">Nombre del parámetro</param>
        /// <exception cref="WatanaValidationException">Se lanza cuando el valor es nulo</exception>
        public static void ValidateNotNull(object value, string parameterName)
        {
            if (value == null)
                throw new WatanaValidationException($"El valor '{parameterName}' es obligatorio", parameterName);
        }
    }
}