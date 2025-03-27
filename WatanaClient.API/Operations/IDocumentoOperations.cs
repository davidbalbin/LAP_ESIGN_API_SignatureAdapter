using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Models.Common;
using WatanaClient.API.Models.Requests;
using WatanaClient.API.Models.Responses;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Operaciones relacionadas con documentos PDF en la API Watana Firmador
    /// </summary>
    public interface IDocumentoOperations
    {
        /// <summary>
        /// Valida un documento PDF. 
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="pdfZipBase64">Documento PDF comprimido en ZIP y codificado en Base64</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con la información de las firmas del documento</returns>
        Task<ValidarPdfResponse> ValidarPdfAsync(string pdfZipBase64, CancellationToken cancellationToken = default);

        /// <summary>
        /// Valida un documento PDF a partir de sus bytes. 
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="pdfBytes">Bytes del documento PDF</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con la información de las firmas del documento</returns>
        Task<ValidarPdfResponse> ValidarPdfAsync(byte[] pdfBytes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Firma un documento PDF con un certificado de agente automatizado de LLAMA.PE.
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="request">Solicitud con los datos para la firma</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el documento firmado</returns>
        Task<FirmarPdfResponse> FirmarPdfAsync(FirmarPdfRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Firma un documento PDF con un certificado de agente automatizado de LLAMA.PE.
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="pdfBytes">Bytes del documento PDF</param>
        /// <param name="firmaVisual">Configuración de la representación visual de la firma (opcional)</param>
        /// <param name="selloTiempo">Indica si se debe aplicar sello de tiempo</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Bytes del documento PDF firmado</returns>
        Task<byte[]> FirmarPdfAsync(byte[] pdfBytes, FirmaVisualRequest? firmaVisual = null, bool selloTiempo = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aplica un sello de tiempo a un documento PDF que ya contiene al menos una firma digital.
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="pdfZipBase64">Documento PDF comprimido en ZIP y codificado en Base64</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Respuesta con el documento sellado</returns>
        Task<SellarPdfResponse> SellarPdfAsync(string pdfZipBase64, CancellationToken cancellationToken = default);

        /// <summary>
        /// Aplica un sello de tiempo a un documento PDF que ya contiene al menos una firma digital.
        /// NOTA: Solo disponible en entorno de PRODUCCIÓN
        /// </summary>
        /// <param name="pdfBytes">Bytes del documento PDF</param>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Bytes del documento PDF sellado</returns>
        Task<byte[]> SellarPdfAsync(byte[] pdfBytes, CancellationToken cancellationToken = default);
    }
}