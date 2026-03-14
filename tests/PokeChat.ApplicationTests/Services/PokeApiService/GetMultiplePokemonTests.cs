using System.Net;
using System.Text;
using FluentAssertions;
using PokeChat.ApplicationTests.Helpers;

namespace PokeChat.ApplicationTests.Services.PokeApiService;

/// <summary>
/// Unit tests for <see cref="PokeChat.Application.Services.PokeApiService.GetMultiplePokemonAsync"/>.
/// </summary>
public sealed class GetMultiplePokemonTests : IDisposable
{
    private readonly PokeApiServiceFactory _factory = new();

    public void Dispose() => _factory.Dispose();

    [Fact]
    public async Task GetMultiplePokemonAsync_MultipleNames_ReturnsAllSuccessful()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildPokemonJson(id: 25, name: "pikachu"));

        // Act
        var results = await sut.GetMultiplePokemonAsync(
            ["pikachu", "bulbasaur", "charmander"],
            CancellationToken.None);

        // Assert
        results.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetMultiplePokemonAsync_EmptyNames_ReturnsEmptyList()
    {
        // Arrange
        var sut = _factory.Create(PokeApiJsonBuilder.BuildPokemonJson());

        // Act
        var results = await sut.GetMultiplePokemonAsync([], CancellationToken.None);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task GetMultiplePokemonAsync_SomeFailures_ReturnsOnlySuccessful()
    {
        // Arrange
        var responses = new Queue<HttpResponseMessage>(
        [
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    PokeApiJsonBuilder.BuildPokemonJson(25, "pikachu"),
                    Encoding.UTF8, "application/json")
            },
            new HttpResponseMessage(HttpStatusCode.NotFound),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    PokeApiJsonBuilder.BuildPokemonJson(1, "bulbasaur"),
                    Encoding.UTF8, "application/json")
            }
        ]);

        var sut = _factory.Create(responses);

        // Act
        var results = await sut.GetMultiplePokemonAsync(
            ["pikachu", "unknown", "bulbasaur"],
            CancellationToken.None);

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(p => p.Name == "pikachu");
        results.Should().Contain(p => p.Name == "bulbasaur");
    }
}
