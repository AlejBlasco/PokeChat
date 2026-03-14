using System.Net;
using FluentAssertions;
using PokeChat.ApplicationTests.Helpers;

namespace PokeChat.ApplicationTests.Services.PokeApiService;

/// <summary>
/// Unit tests for <see cref="PokeChat.Application.Services.PokeApiService.GetPokemonSpeciesAsync"/>.
/// </summary>
public sealed class GetPokemonSpeciesTests : IDisposable
{
    private readonly PokeApiServiceFactory _factory = new();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task GetPokemonSpeciesAsync_ValidName_ReturnsMappedDto()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildSpeciesJson(id: 25, name: "pikachu"));

        // Act
        var result = await sut.GetPokemonSpeciesAsync("pikachu", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(25);
        result.Name.Should().Be("pikachu");
        result.FlavorText.Should().Be("A mouse pokemon.");
        result.Habitat.Should().Be("forest");
        result.Generation.Should().Be("generation-i");
        result.IsLegendary.Should().BeFalse();
        result.IsMythical.Should().BeFalse();
        result.CaptureRate.Should().Be(190);
        result.BaseHappiness.Should().Be(70);
    }

    [Fact]
    public async Task GetPokemonSpeciesAsync_HttpRequestException_ReturnsNull()
    {
        // Arrange
        var sut = _factory.Create(HttpStatusCode.NotFound);

        // Act
        var result = await sut.GetPokemonSpeciesAsync("unknown", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPokemonSpeciesAsync_SameName_ReturnsCachedResult()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildSpeciesJson(id: 25, name: "pikachu"));

        // Act
        await sut.GetPokemonSpeciesAsync("pikachu", CancellationToken.None);
        await sut.GetPokemonSpeciesAsync("pikachu", CancellationToken.None);

        // Assert
        _factory.LastHandler!.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetPokemonSpeciesAsync_FlavorText_NormalisesWhitespace()
    {
        // Arrange
        var sut = _factory.Create(
            PokeApiJsonBuilder.BuildSpeciesJson(flavorText: "A mouse\npokemon.\fRare."));

        // Act
        var result = await sut.GetPokemonSpeciesAsync("pikachu", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.FlavorText.Should().Be("A mouse pokemon. Rare.");
    }
}
