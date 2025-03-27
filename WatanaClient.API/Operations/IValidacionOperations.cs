using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Models.Responses;

namespace WatanaClient.API.Operations
{
    /// <summary>
    /// Operaciones relacionadas con validación de documentos en la API Watana Firmador
    /// </summary>
    public interface IValidacionOperations
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
    }
}