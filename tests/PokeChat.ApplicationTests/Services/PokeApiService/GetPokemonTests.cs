using System.Net;
using FluentAssertions;
using PokeChat.ApplicationTests.Helpers;

namespace PokeChat.ApplicationTests.Services.PokeApiService;

/// <summary>
/// Unit tests for <see cref="PokeChat.Application.Services.PokeApiService.GetPokemonAsync"/>.
/// </summary>
public sealed class GetPokemonTests : IDisposable
{
    private readonly PokeApiServiceFactory _factory = new();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task GetPokemonAsync_ValidName_ReturnsMappedDto()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildPokemonJson(id: 25, name: "pikachu"));

        // Act
        var result = await sut.GetPokemonAsync("pikachu", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(25);
        result.Name.Should().Be("pikachu");
        result.Types.Should().ContainSingle(t => t == "electric");
        result.Abilities.Should().ContainSingle(a => a == "static");
        result.Stats.Should().ContainKey("hp").WhoseValue.Should().Be(35);
        result.SpriteUrl.Should().Be("https://example.com/pikachu.png");
    }

    [Fact]
    public async Task GetPokemonAsync_HttpRequestException_ReturnsNull()
    {
        // Arrange
        var sut = _factory.Create(HttpStatusCode.NotFound);

        // Act
        var result = await sut.GetPokemonAsync("unknown", CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPokemonAsync_SameName_ReturnsCachedResult()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildPokemonJson(id: 25, name: "pikachu"));

        // Act
        await sut.GetPokemonAsync("pikachu", CancellationToken.None);
        await sut.GetPokemonAsync("pikachu", CancellationToken.None);

        // Assert
        _factory.LastHandler!.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetPokemonAsync_NameNormalisedToLower_HitsSameCache()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildPokemonJson(id: 25, name: "pikachu"));

        // Act
        await sut.GetPokemonAsync("Pikachu", CancellationToken.None);
        await sut.GetPokemonAsync("PIKACHU", CancellationToken.None);

        // Assert
        _factory.LastHandler!.CallCount.Should().Be(1);
    }
}
