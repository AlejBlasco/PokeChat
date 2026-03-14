namespace PokeChat.Application.Options;

/// <summary>
/// Configuration options for the PokeAPI HTTP client.
/// Bound from the "PokeApi" section in appsettings.json.
/// </summary>
public sealed class PokeApiOptions
{
    /// <summary>The configuration section key.</summary>
    public const string SectionKey = "PokeApi";

    /// <summary>Gets or sets the PokeAPI base URL.</summary>
    public string BaseUrl { get; set; } = "https://pokeapi.co/api/v2/";

    /// <summary>Gets or sets the HTTP request timeout in seconds.</summary>
    public int TimeoutSeconds { get; set; } = 10;
}
