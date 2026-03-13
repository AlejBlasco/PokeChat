using System.Net.Http.Json;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using PokeChat.Application.DTOs;
using PokeChat.Application.HttpModels;

namespace PokeChat.Application.Services;

/// <summary>
/// HTTP client implementation for the PokeAPI.
/// Responses are cached in memory (TTL: 1 hour) to reduce redundant API calls.
/// Multiple Pokémon can be fetched in parallel via <see cref="GetMultiplePokemonAsync"/>.
/// DTO mappings are delegated to AutoMapper.
/// </summary>
public sealed class PokeApiService : IPokeApiService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);

    /// <summary>Initializes a new instance of <see cref="PokeApiService"/>.</summary>
    public PokeApiService(HttpClient httpClient, IMemoryCache cache, IMapper mapper)
    {
        _httpClient = httpClient;
        _cache = cache;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PokemonDto?> GetPokemonAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"pokemon:{name.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out PokemonDto? cached))
            return cached;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PokeApiPokemonResponse>(
                $"pokemon/{name.ToLowerInvariant()}",
                cancellationToken);

            if (response is null)
                return null;

            var dto = _mapper.Map<PokemonDto>(response);
            _cache.Set(cacheKey, dto, CacheTtl);
            return dto;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<PokemonSpeciesDto?> GetPokemonSpeciesAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"pokemon-species:{name.ToLowerInvariant()}";

        if (_cache.TryGetValue(cacheKey, out PokemonSpeciesDto? cached))
            return cached;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<PokeApiSpeciesResponse>(
                $"pokemon-species/{name.ToLowerInvariant()}",
                cancellationToken);

            if (response is null)
                return null;

            var dto = _mapper.Map<PokemonSpeciesDto>(response);
            _cache.Set(cacheKey, dto, CacheTtl);
            return dto;
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PokemonDto>> GetMultiplePokemonAsync(
        IEnumerable<string> names,
        CancellationToken cancellationToken = default)
    {
        var tasks = names.Select(name => GetPokemonAsync(name, cancellationToken));
        var results = await Task.WhenAll(tasks);

        return results
            .Where(p => p is not null)
            .Select(p => p!)
            .ToList()
            .AsReadOnly();
    }
}
