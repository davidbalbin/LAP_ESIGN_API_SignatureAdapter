using Microsoft.Extensions.Options;
using WatanaClient.API.Configuration;
using WatanaClient.API.Http;
using WatanaClient.API.Operations;

namespace WatanaClient.API.Services
{
    /// <summary>
    /// Implementación principal del cliente para la API Watana Firmador
    /// </summary>
    public class WatanaClient : IWatanaClient
    {
        private readonly IWatanaHttpClient _httpClient;
        private readonly WatanaClientOptions _options;

        /// <summary>
        /// Operaciones relacionadas con carpetas de documentos
        /// </summary>
        public ICarpetaOperations Carpetas { get; }

        /// <summary>
        /// Operaciones relacionadas con documentos individuales
        /// </summary>
        public IDocumentoOperations Documentos { get; }

        /// <summary>
        /// Operaciones relacionadas con solicitudes de firma
        /// </summary>
        public ISolicitudOperations Solicitudes { get; }

        /// <summary>
        /// Operaciones relacionadas con validación de documentos
        /// </summary>
        public IValidacionOperations Validaciones { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaClient"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API</param>
        /// <param name="options">Opciones de configuración</param>
        public WatanaClient(IWatanaHttpClient httpClient, IOptions<WatanaClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            // Validar opciones de configuración
            _options.Validate();

            // Inicializar operaciones
            Carpetas = new CarpetaOperations(_httpClient);
            Documentos = new DocumentoOperations(_httpClient);
            Solicitudes = new SolicitudOperations(_httpClient);
            Validaciones = new ValidacionOperations(_httpClient);
        }
    }
}