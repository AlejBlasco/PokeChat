namespace PokeChat.Application.DTOs;

/// <summary>
/// Represents the core data of a Pokémon retrieved from /pokemon/{name}.
/// Designed to provide context to Semantic Kernel.
/// </summary>
public sealed record PokemonDto
{
    /// <summary>Gets the national Pokédex identifier.</summary>
    public int Id { get; init; }

    /// <summary>Gets the Pokémon name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the base experience gained when defeating this Pokémon.</summary>
    public int BaseExperience { get; init; }

    /// <summary>Gets the height in decimetres.</summary>
    public int Height { get; init; }

    /// <summary>Gets the weight in hectograms.</summary>
    public int Weight { get; init; }

    /// <summary>Gets the list of types (e.g. fire, water).</summary>
    public IReadOnlyList<string> Types { get; init; } = [];

    /// <summary>Gets the list of ability names.</summary>
    public IReadOnlyList<string> Abilities { get; init; } = [];

    /// <summary>Gets the base stats keyed by stat name (e.g. hp, attack).</summary>
    public IReadOnlyDictionary<string, int> Stats { get; init; } = new Dictionary<string, int>();

    /// <summary>Gets the default front sprite URL.</summary>
    public string? SpriteUrl { get; init; }
}
