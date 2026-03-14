using PokeChat.Application.DTOs;

namespace PokeChat.Application.Services;

/// <summary>
/// Contract for the PokeAPI HTTP service.
/// Provides Pokémon data for consumption by Semantic Kernel and other application features.
/// </summary>
public interface IPokeApiService
{
    /// <summary>
    /// Retrieves core Pokémon data by name from /pokemon/{name}.
    /// Returns <see langword="null"/> if the Pokémon is not found or the request fails.
    /// </summary>
    /// <param name="name">The Pokémon name (case-insensitive).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<PokemonDto?> GetPokemonAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves species data by name from /pokemon-species/{name}.
    /// Returns <see langword="null"/> if the species is not found or the request fails.
    /// </summary>
    /// <param name="name">The Pokémon name (case-insensitive).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<PokemonSpeciesDto?> GetPokemonSpeciesAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves core data for multiple Pokémon in parallel.
    /// Individual failures are silently omitted from the result.
    /// </summary>
    /// <param name="names">The Pokémon names to fetch.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<PokemonDto>> GetMultiplePokemonAsync(
        IEnumerable<string> names,
        CancellationToken cancellationToken = default);
}
