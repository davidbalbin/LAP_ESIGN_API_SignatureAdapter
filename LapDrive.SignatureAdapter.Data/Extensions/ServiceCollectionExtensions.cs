using LapDrive.SignatureAdapter.Data.Repositories.Interfaces;
using LapDrive.SignatureAdapter.Data.Repositories.Implementation;
using LapDrive.SignatureAdapter.Data.Clients.Interfaces;
using LapDrive.SignatureAdapter.Data.Clients.Implementation;
using LapDrive.SignatureAdapter.Data.Configuration;
using LapDrive.SignatureAdapter.Data.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WatanaClient.API.Extensions;

namespace LapDrive.SignatureAdapter.Data.Extensions;

/// <summary>
/// Extension methods for setting up data access services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds data access layer services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration" /> to read settings from.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration options
        services.Configure<SharePointOptions>(configuration.GetSection("SharePoint"));
        services.Configure<SignatureProviderOptions>(configuration.GetSection("SignatureProvider"));
        
        // Register factories
        services.AddSingleton<SharePointContextFactory>();
        
        // Register clients
        services.AddScoped<ISharePointClient, SharePointClient>();
        services.AddScoped<ISignatureProviderClient, WatanaSignatureProviderClient>();
        
        // Register repositories
        services.AddScoped<IDocumentRepository, SharePointDocumentRepository>();
        services.AddScoped<ISignatureProcessRepository, WatanaSignatureProcessRepository>();
        services.AddScoped<ISignatureTrackingRepository, SharePointSignatureTrackingRepository>();

        // Register Watana Client
        services.AddWatanaClient(configuration, "SignatureProvider");
        
        return services;
    }
}