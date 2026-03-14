using System.Text.Json.Serialization;

namespace PokeChat.Application.HttpModels;

/// <summary>Raw PokeAPI /pokemon response.</summary>
internal sealed record PokeApiPokemonResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("base_experience")] int BaseExperience,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("weight")] int Weight,
    [property: JsonPropertyName("types")] List<PokeApiTypeSlot> Types,
    [property: JsonPropertyName("abilities")] List<PokeApiAbilitySlot> Abilities,
    [property: JsonPropertyName("stats")] List<PokeApiStatSlot> Stats,
    [property: JsonPropertyName("sprites")] PokeApiSprites Sprites);

/// <summary>Type slot in a pokemon response.</summary>
internal sealed record PokeApiTypeSlot(
    [property: JsonPropertyName("type")] PokeApiNamedResource Type);

/// <summary>Ability slot in a pokemon response.</summary>
internal sealed record PokeApiAbilitySlot(
    [property: JsonPropertyName("ability")] PokeApiNamedResource Ability);

/// <summary>Stat slot in a pokemon response.</summary>
internal sealed record PokeApiStatSlot(
    [property: JsonPropertyName("base_stat")] int BaseStat,
    [property: JsonPropertyName("stat")] PokeApiNamedResource Stat);

/// <summary>Sprites block in a pokemon response.</summary>
internal sealed record PokeApiSprites(
    [property: JsonPropertyName("front_default")] string? FrontDefault);

/// <summary>Raw PokeAPI /pokemon-species response.</summary>
internal sealed record PokeApiSpeciesResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("flavor_text_entries")] List<PokeApiFlavorTextEntry> FlavorTextEntries,
    [property: JsonPropertyName("habitat")] PokeApiNamedResource? Habitat,
    [property: JsonPropertyName("generation")] PokeApiNamedResource Generation,
    [property: JsonPropertyName("is_legendary")] bool IsLegendary,
    [property: JsonPropertyName("is_mythical")] bool IsMythical,
    [property: JsonPropertyName("capture_rate")] int CaptureRate,
    [property: JsonPropertyName("base_happiness")] int BaseHappiness);

/// <summary>Flavor text entry in a species response.</summary>
internal sealed record PokeApiFlavorTextEntry(
    [property: JsonPropertyName("flavor_text")] string FlavorText,
    [property: JsonPropertyName("language")] PokeApiNamedResource Language);

/// <summary>Generic named API resource used across multiple PokeAPI responses.</summary>
internal sealed record PokeApiNamedResource(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("url")] string Url);
