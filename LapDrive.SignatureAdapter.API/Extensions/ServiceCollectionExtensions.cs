namespace LapDrive.SignatureAdapter.API.Extensions;

/// <summary>
/// Extension methods for setting up API services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds API layer services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Register any API-specific services here
        
        // Register FluentValidation
        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}