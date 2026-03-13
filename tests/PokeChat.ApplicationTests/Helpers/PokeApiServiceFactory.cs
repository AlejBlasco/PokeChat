using System.Net;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PokeChat.Application.Mappings;
using PokeChat.Application.Services;

namespace PokeChat.ApplicationTests.Helpers;

/// <summary>
/// Factory for building <see cref="PokeApiService"/> instances with fake dependencies
/// for use in unit tests.
/// </summary>
public sealed class PokeApiServiceFactory : IDisposable
{
    // Base URL used only to satisfy HttpClient requirements in tests.
    // Real URL is configured via appsettings.json in production.
    public const string TestBaseUrl = "https://pokeapi.co/api/v2/";

    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    /// <summary>Gets the last handler created by this factory.</summary>
    public FakeHttpMessageHandler? LastHandler { get; private set; }

    /// <summary>Initializes a new instance of <see cref="PokeApiServiceFactory"/>.</summary>
    public PokeApiServiceFactory()
    {
        _cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<PokeApiMappingProfile>())
            .CreateMapper();
    }

    /// <summary>Builds a <see cref="PokeApiService"/> that always returns the given JSON body.</summary>
    public PokeApiService Create(string jsonBody)
    {
        LastHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, jsonBody);
        return Build(LastHandler);
    }

    /// <summary>Builds a <see cref="PokeApiService"/> that always returns the given HTTP status.</summary>
    public PokeApiService Create(HttpStatusCode statusCode)
    {
        LastHandler = new FakeHttpMessageHandler(statusCode, string.Empty);
        return Build(LastHandler);
    }

    /// <summary>Builds a <see cref="PokeApiService"/> that returns responses from a queue.</summary>
    public PokeApiService Create(Queue<HttpResponseMessage> responses)
    {
        LastHandler = new FakeHttpMessageHandler(responses);
        return Build(LastHandler);
    }

    /// <summary>
    /// Creates and validates the AutoMapper configuration.
    /// Call in mapping profile tests to assert configuration is valid.
    /// </summary>
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<PokeApiMappingProfile>());
        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }

    private PokeApiService Build(FakeHttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri(TestBaseUrl) };
        return new PokeApiService(httpClient, _cache, _mapper);
    }

    /// <inheritdoc />
    public void Dispose() => _cache.Dispose();
}
