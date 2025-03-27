using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Models.Requests;
using WatanaClient.API.Models.Responses;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Operaciones relacionadas con carpetas de documentos en la API Watana Firmador
    /// </summary>
    public interface ICarpetaOperations
    {
        /// <summary>
        /// Envía una carpeta con documentos para firma digital
        /// </summary>
        /// <param name="request">Solicitud con los datos de la carpeta y los archivos</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        Task<EnviarCarpetaResponse> EnviarCarpetaAsync(EnviarCarpetaRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Consulta el estado de una carpeta previamente enviada
        /// </summary>
        /// <param name="carpetaCodigo">Código de la carpeta</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el estado de la carpeta</returns>
        Task<ConsultarCarpetaResponse> ConsultarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Descarga los archivos de una carpeta
        /// </summary>
        /// <param name="carpetaCodigo">Código de la carpeta</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con los archivos de la carpeta o un stream con el archivo ZIP</returns>
        Task<DescargarCarpetaResponse> DescargarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Descarga los archivos de una carpeta como un stream (para carpetas grandes)
        /// </summary>
        /// <param name="carpetaCodigo">Código de la carpeta</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Stream con el archivo ZIP</returns>
        Task<Stream> DescargarCarpetaComoStreamAsync(string carpetaCodigo, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina una carpeta
        /// </summary>
        /// <param name="carpetaCodigo">Código de la carpeta</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        Task<EliminarCarpetaResponse> EliminarCarpetaAsync(string carpetaCodigo, CancellationToken cancellationToken = default);
    }
}