using System;

namespace WatanaClient.API.Configuration
{
    /// <summary>
    /// Opciones de configuración para el cliente de la API Watana Firmador
    /// </summary>
    public class WatanaClientOptions
    {
        /// <summary>
        /// URL base de la API Watana Firmador
        /// </summary>
        /// <example>https://api.watana.pe/api/v1/xxxxxxxxx</example>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// Token de autenticación para la API
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiJ9...</example>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Tiempo máximo de espera para las solicitudes HTTP
        /// </summary>
        /// <remarks>Valor predeterminado: 30 segundos</remarks>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Número máximo de reintentos para solicitudes fallidas
        /// </summary>
        /// <remarks>Valor predeterminado: 3</remarks>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Indica si se debe validar el certificado SSL del servidor
        /// </summary>
        /// <remarks>
        /// Se recomienda mantener este valor en true en entornos de producción.
        /// Solo establezca en false para entornos de desarrollo o pruebas.
        /// </remarks>
        public bool ValidateServerCertificate { get; set; } = true;

        /// <summary>
        /// Tiempo de espera entre reintentos (en segundos)
        /// </summary>
        /// <remarks>Valor predeterminado: 2 segundos</remarks>
        public int RetryDelaySeconds { get; set; } = 2;

        /// <summary>
        /// Valida que las opciones de configuración sean correctas
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando las opciones de configuración no son válidas</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ApiUrl))
                throw new ArgumentException("La URL de la API es obligatoria", nameof(ApiUrl));

            if (string.IsNullOrWhiteSpace(Token))
                throw new ArgumentException("El token de autenticación es obligatorio", nameof(Token));

            if (!Uri.TryCreate(ApiUrl, UriKind.Absolute, out _))
                throw new ArgumentException("La URL de la API no es válida", nameof(ApiUrl));

            if (Timeout <= TimeSpan.Zero)
                throw new ArgumentException("El timeout debe ser mayor que cero", nameof(Timeout));

            if (MaxRetries < 0)
                throw new ArgumentException("El número máximo de reintentos no puede ser negativo", nameof(MaxRetries));

            if (RetryDelaySeconds <= 0)
                throw new ArgumentException("El tiempo de espera entre reintentos debe ser mayor que cero", nameof(RetryDelaySeconds));
        }
    }
}