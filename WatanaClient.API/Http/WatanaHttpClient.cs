using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WatanaClient.API.Configuration;
using WatanaClient.API.Constants;
using WatanaClient.API.Exceptions;
using WatanaClient.API.Serialization;

namespace WatanaClient.API.Http
{
    /// <summary>
    /// Implementación del cliente HTTP para comunicarse con la API Watana Firmador
    /// </summary>
    public class WatanaHttpClient : IWatanaHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly WatanaClientOptions _options;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="WatanaHttpClient"/>
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para realizar solicitudes</param>
        /// <param name="options">Opciones de configuración</param>
        public WatanaHttpClient(HttpClient httpClient, IOptions<WatanaClientOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            // Configurar cliente HTTP
            _httpClient.BaseAddress = new Uri(_options.ApiUrl);
            _httpClient.DefaultRequestHeaders.Add(WatanaConstants.HttpHeaders.Authorization, _options.Token);
            _httpClient.Timeout = _options.Timeout;

            // Configurar opciones de serialización
            _serializerOptions = JsonSerializationOptions.Default;
        }

        /// <inheritdoc/>
        public async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string operacion, object requestBody, CancellationToken cancellationToken = default)
        {
            // Preparar la solicitud
            var request = CreateRequest(method, operacion, requestBody);

            // Enviar la solicitud
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new WatanaException("Error de comunicación con la API", ex);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                throw new WatanaException("La solicitud ha excedido el tiempo de espera", ex);
            }

            // Procesar la respuesta
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                HandleErrorResponse(response.StatusCode, responseContent);
            }

            try
            {
                return JsonSerializer.Deserialize<TResponse>(responseContent, _serializerOptions)!;
            }
            catch (JsonException ex)
            {
                throw new WatanaException($"Error al deserializar la respuesta: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<Stream> SendAndGetStreamAsync(HttpMethod method, string operacion, object requestBody, CancellationToken cancellationToken = default)
        {
            // Preparar la solicitud
            var request = CreateRequest(method, operacion, requestBody);

            // Enviar la solicitud
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new WatanaException("Error de comunicación con la API", ex);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                throw new WatanaException("La solicitud ha excedido el tiempo de espera", ex);
            }

            // Procesar la respuesta
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleErrorResponse(response.StatusCode, errorContent);
            }

            // Determinar si la respuesta es un archivo o un JSON
            var contentType = response.Content.Headers.ContentType?.MediaType;
            
            if (contentType == WatanaConstants.HttpHeaders.ApplicationZip)
            {
                // Es un archivo ZIP, devolver el stream directamente
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
            else
            {
                // Es JSON, procesarlo según corresponda
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent));
                return memoryStream;
            }
        }

        /// <summary>
        /// Crea una solicitud HTTP para la API
        /// </summary>
        private HttpRequestMessage CreateRequest(HttpMethod method, string operacion, object requestBody)
        {
            // En el cuerpo de la solicitud, asegurarnos de que se incluya la operación
            var jsonBody = JsonSerializer.Serialize(requestBody, _serializerOptions);
            var request = new HttpRequestMessage(method, string.Empty)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, WatanaConstants.HttpHeaders.ApplicationJson)
            };

            return request;
        }

        /// <summary>
        /// Maneja una respuesta de error de la API
        /// </summary>
        private void HandleErrorResponse(System.Net.HttpStatusCode statusCode, string responseContent)
        {
            string errorMessage = "Error en la API Watana";
            string? errorCode = null;

            try
            {
                // Intentar deserializar la respuesta para obtener el mensaje de error
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _serializerOptions);
                if (errorResponse != null && !string.IsNullOrEmpty(errorResponse.Error))
                {
                    errorMessage = errorResponse.Error;
                }
            }
            catch (JsonException)
            {
                // Si no se puede deserializar, usar el contenido de la respuesta como está
                errorMessage = $"Error en la API Watana: {responseContent}";
            }

            throw new WatanaApiException(statusCode, errorMessage, responseContent, errorCode);
        }

        /// <summary>
        /// Clase interna para deserializar respuestas de error
        /// </summary>
        private class ErrorResponse
        {
            /// <summary>
            /// Mensaje de error
            /// </summary>
            public string? Error { get; set; }
        }
    }
}