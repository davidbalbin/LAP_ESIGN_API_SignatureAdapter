using LapDrive.SignatureAdapter.Business.Services.Interfaces;
using LapDrive.SignatureAdapter.Business.Services.Implementation;
using LapDrive.SignatureAdapter.Business.Validators;
using FluentValidation;
using LapDrive.SignatureAdapter.Models.DTOs.Request;
using Microsoft.Extensions.DependencyInjection;

namespace LapDrive.SignatureAdapter.Business.Extensions;

/// <summary>
/// Extension methods for setting up business services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds business layer services to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<ISignatureProcessService, SignatureProcessService>();
        
        // Register validators
        services.AddScoped<IValidator<SignatureProcessRequest>, SignatureProcessRequestValidator>();
        
        return services;
    }
}