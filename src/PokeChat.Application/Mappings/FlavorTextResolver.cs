using AutoMapper;
using PokeChat.Application.HttpModels;

namespace PokeChat.Application.Mappings;

/// <summary>
/// AutoMapper value resolver that extracts the first English flavor text
/// from a PokeAPI species response, normalising whitespace characters.
/// </summary>
internal sealed class FlavorTextResolver
    : IValueResolver<PokeApiSpeciesResponse, object, string>
{
    /// <inheritdoc />
    public string Resolve(
        PokeApiSpeciesResponse source,
        object destination,
        string destMember,
        ResolutionContext context)
    {
        return source.FlavorTextEntries
            .Where(f => f.Language.Name == "en")
            .Select(f => f.FlavorText
                .Replace("\n", " ")
                .Replace("\f", " "))
            .FirstOrDefault() ?? string.Empty;
    }
}
