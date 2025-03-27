using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WatanaClient.API.Http
{
    /// <summary>
    /// Interfaz para el cliente HTTP que se comunica con la API Watana Firmador
    /// </summary>
    public interface IWatanaHttpClient
    {
        /// <summary>
        /// Envía una solicitud HTTP a la API y deserializa la respuesta
        /// </summary>
        /// <typeparam name="TResponse">Tipo de la respuesta esperada</typeparam>
        /// <param name="method">Método HTTP (siempre POST para Watana API)</param>
        /// <param name="operacion">Operación de la API a ejecutar</param>
        /// <param name="requestBody">Cuerpo de la solicitud</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta deserializada</returns>
        Task<TResponse> SendAsync<TResponse>(HttpMethod method, string operacion, object requestBody, CancellationToken cancellationToken = default);

        /// <summary>
        /// Envía una solicitud HTTP a la API y devuelve la respuesta como un stream
        /// (útil para descargar archivos grandes)
        /// </summary>
        /// <param name="method">Método HTTP (siempre POST para Watana API)</param>
        /// <param name="operacion">Operación de la API a ejecutar</param>
        /// <param name="requestBody">Cuerpo de la solicitud</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Stream con el contenido de la respuesta</returns>
        Task<Stream> SendAndGetStreamAsync(HttpMethod method, string operacion, object requestBody, CancellationToken cancellationToken = default);
    }
}