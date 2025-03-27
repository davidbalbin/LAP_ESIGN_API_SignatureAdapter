using System.Threading.Tasks;
using WatanaClient.API.Models.Webhooks;

namespace WatanaClient.API.Services
{
    /// <summary>
    /// Interfaz para el procesador de webhooks de Watana Firmador
    /// </summary>
    public interface IWatanaWebhookProcessor
    {
        /// <summary>
        /// Procesa una notificación de webhook
        /// </summary>
        /// <param name="requestBody">Cuerpo de la solicitud de webhook en formato JSON</param>
        /// <returns>Resultado del procesamiento del webhook</returns>
        Task<WebhookResult> ProcessWebhookAsync(string requestBody);
    }
}