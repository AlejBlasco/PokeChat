using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeChat.Application.Mappings;
using PokeChat.Application.Options;
using PokeChat.Application.Services;

namespace PokeChat.Application.Extensions;

/// <summary>
/// Extension methods for registering Application layer services in the DI container.
/// </summary>
public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registers all Application layer services including the PokeAPI HTTP client,
    /// memory cache, and AutoMapper profiles.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PokeApiOptions>(configuration.GetSection(PokeApiOptions.SectionKey));

        var options = configuration
            .GetSection(PokeApiOptions.SectionKey)
            .Get<PokeApiOptions>() ?? new PokeApiOptions();

        services.AddMemoryCache();

        services.AddAutoMapper(typeof(PokeApiMappingProfile).Assembly);

        services.AddHttpClient<IPokeApiService, PokeApiService>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
        });

        return services;
    }
}
