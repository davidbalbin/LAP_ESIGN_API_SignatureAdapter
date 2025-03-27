using WatanaClient.API.Operations;

namespace WatanaClient.API.Services
{
    /// <summary>
    /// Interfaz principal para el cliente de la API Watana Firmador
    /// </summary>
    public interface IWatanaClient
    {
        /// <summary>
        /// Operaciones relacionadas con carpetas de documentos
        /// </summary>
        ICarpetaOperations Carpetas { get; }

        /// <summary>
        /// Operaciones relacionadas con documentos individuales
        /// </summary>
        IDocumentoOperations Documentos { get; }

        /// <summary>
        /// Operaciones relacionadas con solicitudes de firma
        /// </summary>
        ISolicitudOperations Solicitudes { get; }

        /// <summary>
        /// Operaciones relacionadas con validación de documentos
        /// </summary>
        IValidacionOperations Validaciones { get; }
    }
}