namespace PokeChat.Application.DTOs;

/// <summary>
/// Represents the species data of a Pokémon retrieved from /pokemon-species/{name}.
/// Designed to provide narrative context to Semantic Kernel.
/// </summary>
public sealed record PokemonSpeciesDto
{
    /// <summary>Gets the national Pokédex identifier.</summary>
    public int Id { get; init; }

    /// <summary>Gets the Pokémon species name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the Pokédex flavor text in English.</summary>
    public string FlavorText { get; init; } = string.Empty;

    /// <summary>Gets the habitat name (e.g. forest, cave). Null if unknown.</summary>
    public string? Habitat { get; init; }

    /// <summary>Gets the generation in which the Pokémon was introduced (e.g. generation-i).</summary>
    public string Generation { get; init; } = string.Empty;

    /// <summary>Gets whether this Pokémon is legendary.</summary>
    public bool IsLegendary { get; init; }

    /// <summary>Gets whether this Pokémon is mythical.</summary>
    public bool IsMythical { get; init; }

    /// <summary>Gets the capture rate (0–255).</summary>
    public int CaptureRate { get; init; }

    /// <summary>Gets the base happiness.</summary>
    public int BaseHappiness { get; init; }
}
