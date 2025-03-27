using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Models.Requests;
using WatanaClient.API.Models.Responses;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Operaciones relacionadas con solicitudes de firma en la API Watana Firmador
    /// </summary>
    public interface ISolicitudOperations
    {
        /// <summary>
        /// Prepara una solicitud con una lista de archivos para su posterior configuración y envío a firmantes
        /// </summary>
        /// <param name="request">Solicitud con los datos de la carpeta y los archivos</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con la información de los archivos cargados</returns>
        Task<PrepararSolicitudResponse> PrepararSolicitudAsync(PrepararSolicitudRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Envía una solicitud de firma a uno o más firmantes configurando qué PDF firmará cada uno
        /// </summary>
        /// <param name="request">Solicitud con los datos de los firmantes y la configuración de las firmas</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con los enlaces de firma para cada firmante</returns>
        Task<EnviarSolicitudResponse> EnviarSolicitudAsync(EnviarSolicitudRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Consulta el estado de una solicitud de firma compuesta (con múltiples firmantes)
        /// </summary>
        /// <param name="firmaCodigo">Código de la solicitud de firma</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el estado de cada solicitud</returns>
        Task<ConsultarSolicitudResponse> ConsultarSolicitudAsync(string firmaCodigo, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Consulta el estado de una solicitud de firma por su número de solicitud
        /// </summary>
        /// <param name="solicitudNumero">Número de la solicitud generado por la API (Ej: W24090020)</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el estado de la solicitud</returns>
        Task<ConsultarSolicitudResponse> ConsultarSolicitudPorNumeroAsync(string solicitudNumero, CancellationToken cancellationToken = default);
    }
}