using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using WatanaClient.API.Configuration;
using WatanaClient.API.Http;
using WatanaClient.API.Operations;
using WatanaClient.API.Services;

namespace WatanaClient.API.Extensions
{
    /// <summary>
    /// Extensiones para configurar el cliente de Watana en el contenedor de inyección de dependencias
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configura y registra el cliente de Watana con las opciones proporcionadas
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <param name="configureOptions">Acción para configurar las opciones</param>
        /// <returns>La colección de servicios para permitir encadenamiento</returns>
        public static IServiceCollection AddWatanaClient(this IServiceCollection services, Action<WatanaClientOptions> configureOptions)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            // Registrar opciones
            services.Configure(configureOptions);

            // Registrar el cliente HTTP
            services.AddHttpClient<IWatanaHttpClient, WatanaHttpClient>()
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<WatanaClientOptions>>().Value;
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = options.ValidateServerCertificate 
                            ? null 
                            : (message, cert, chain, errors) => true
                    };
                })
                .AddPolicyHandler((serviceProvider, _) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<WatanaClientOptions>>().Value;
                    return GetRetryPolicy(options);
                })
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetTimeoutPolicy());

            // Registrar servicios
            services.AddSingleton<IWatanaClient, Services.WatanaClient>();
            services.AddSingleton<IWatanaWebhookProcessor, WatanaWebhookProcessor>();

            return services;
        }

        /// <summary>
        /// Configura y registra el cliente de Watana con opciones desde la configuración
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <param name="configuration">Configuración de la aplicación</param>
        /// <param name="sectionName">Nombre de la sección de configuración (por defecto "Watana")</param>
        /// <returns>La colección de servicios para permitir encadenamiento</returns>
        public static IServiceCollection AddWatanaClient(this IServiceCollection services, IConfiguration configuration, string sectionName = "Watana")
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.Configure<WatanaClientOptions>(configuration.GetSection(sectionName));
            return services.AddWatanaClient();
        }

        /// <summary>
        /// Configura y registra el cliente de Watana con las opciones ya registradas
        /// </summary>
        /// <param name="services">Colección de servicios</param>
        /// <returns>La colección de servicios para permitir encadenamiento</returns>
        public static IServiceCollection AddWatanaClient(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Registrar el cliente HTTP
            services.AddHttpClient<IWatanaHttpClient, WatanaHttpClient>()
                .ConfigurePrimaryHttpMessageHandler(serviceProvider =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<WatanaClientOptions>>().Value;
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = options.ValidateServerCertificate 
                            ? null 
                            : (message, cert, chain, errors) => true
                    };
                })
                .AddPolicyHandler((serviceProvider, _) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<WatanaClientOptions>>().Value;
                    return GetRetryPolicy(options);
                })
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetTimeoutPolicy());

            // Registrar servicios
            services.AddSingleton<IWatanaClient, Services.WatanaClient>();
            services.AddSingleton<IWatanaWebhookProcessor, WatanaWebhookProcessor>();

            return services;
        }

        /// <summary>
        /// Obtiene la política de reintentos para solicitudes fallidas
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(WatanaClientOptions options)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() // HttpRequestException, 5xx and 408
                .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests) // 429
                .Or<TimeoutRejectedException>() // Timeout from Polly's TimeoutPolicy
                .WaitAndRetryAsync(
                    options.MaxRetries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.RetryDelaySeconds, retryAttempt))
                );
        }

        /// <summary>
        /// Obtiene la política de circuit breaker para evitar sobrecarga
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );
        }

        /// <summary>
        /// Obtiene la política de timeout para solicitudes que toman demasiado tiempo
        /// </summary>
        private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(30),
                timeoutStrategy: TimeoutStrategy.Optimistic
            );
        }
    }
}