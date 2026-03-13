using System.Text.Json;

namespace PokeChat.ApplicationTests.Helpers;

/// <summary>
/// Builds JSON payloads that match PokeAPI response shapes for use in unit tests.
/// </summary>
public static class PokeApiJsonBuilder
{
    /// <summary>Builds a minimal /pokemon/{name} JSON response.</summary>
    public static string BuildPokemonJson(
        int id = 25,
        string name = "pikachu",
        string type = "electric",
        string ability = "static",
        int hp = 35,
        string spriteUrl = "https://example.com/pikachu.png") =>
        JsonSerializer.Serialize(new
        {
            id,
            name,
            base_experience = 112,
            height = 4,
            weight = 60,
            types = new[] { new { type = new { name = type, url = "" } } },
            abilities = new[] { new { ability = new { name = ability, url = "" } } },
            stats = new[] { new { base_stat = hp, stat = new { name = "hp", url = "" } } },
            sprites = new { front_default = spriteUrl }
        });

    /// <summary>Builds a minimal /pokemon-species/{name} JSON response.</summary>
    public static string BuildSpeciesJson(
        int id = 25,
        string name = "pikachu",
        string flavorText = "A mouse pokemon.",
        string habitat = "forest",
        string generation = "generation-i",
        bool isLegendary = false,
        bool isMythical = false,
        int captureRate = 190,
        int baseHappiness = 70) =>
        JsonSerializer.Serialize(new
        {
            id,
            name,
            flavor_text_entries = new[]
            {
                new { flavor_text = flavorText, language = new { name = "en", url = "" } }
            },
            habitat = new { name = habitat, url = "" },
            generation = new { name = generation, url = "" },
            is_legendary = isLegendary,
            is_mythical = isMythical,
            capture_rate = captureRate,
            base_happiness = baseHappiness
        });
}
