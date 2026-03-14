using AutoMapper;
using FluentAssertions;
using PokeChat.Application.HttpModels;
using PokeChat.Application.DTOs;
using PokeChat.ApplicationTests.Helpers;

namespace PokeChat.ApplicationTests.Mappings.PokeApiMappingProfile;

/// <summary>
/// Tests for <see cref="PokeChat.Application.Mappings.PokeApiMappingProfile"/>.
/// Validates AutoMapper configuration and mapping correctness.
/// </summary>
public sealed class ConfigurationTests
{
    private readonly IMapper _mapper = PokeApiServiceFactory.CreateMapper();

    // ── Configuration validity ────────────────────────────────────────────────

    [Fact]
    public void Configuration_IsValid()
    {
        // Arrange + Act + Assert
        // AssertConfigurationIsValid is called inside CreateMapper().
        // If this test passes, all mappings are correctly defined.
        var act = () => PokeApiServiceFactory.CreateMapper();

        act.Should().NotThrow();
    }

    // ── PokeApiPokemonResponse → PokemonDto ───────────────────────────────────

    [Fact]
    public void Map_PokemonResponse_MapsScalarProperties()
    {
        // Arrange
        var source = new PokeApiPokemonResponse(
            Id: 25,
            Name: "pikachu",
            BaseExperience: 112,
            Height: 4,
            Weight: 60,
            Types: [],
            Abilities: [],
            Stats: [],
            Sprites: new PokeApiSprites(FrontDefault: "https://example.com/pikachu.png"));

        // Act
        var result = _mapper.Map<PokemonDto>(source);

        // Assert
        result.Id.Should().Be(25);
        result.Name.Should().Be("pikachu");
        result.BaseExperience.Should().Be(112);
        result.Height.Should().Be(4);
        result.Weight.Should().Be(60);
        result.SpriteUrl.Should().Be("https://example.com/pikachu.png");
    }

    [Fact]
    public void Map_PokemonResponse_MapsTypes()
    {
        // Arrange
        var source = BuildMinimalPokemonResponse() with
        {
            Types =
            [
                new PokeApiTypeSlot(new PokeApiNamedResource("electric", "")),
                new PokeApiTypeSlot(new PokeApiNamedResource("steel", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonDto>(source);

        // Assert
        result.Types.Should().HaveCount(2);
        result.Types.Should().ContainInOrder("electric", "steel");
    }

    [Fact]
    public void Map_PokemonResponse_MapsAbilities()
    {
        // Arrange
        var source = BuildMinimalPokemonResponse() with
        {
            Abilities =
            [
                new PokeApiAbilitySlot(new PokeApiNamedResource("static", "")),
                new PokeApiAbilitySlot(new PokeApiNamedResource("lightning-rod", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonDto>(source);

        // Assert
        result.Abilities.Should().HaveCount(2);
        result.Abilities.Should().ContainInOrder("static", "lightning-rod");
    }

    [Fact]
    public void Map_PokemonResponse_MapsStats()
    {
        // Arrange
        var source = BuildMinimalPokemonResponse() with
        {
            Stats =
            [
                new PokeApiStatSlot(BaseStat: 35, Stat: new PokeApiNamedResource("hp", "")),
                new PokeApiStatSlot(BaseStat: 55, Stat: new PokeApiNamedResource("attack", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonDto>(source);

        // Assert
        result.Stats.Should().HaveCount(2);
        result.Stats["hp"].Should().Be(35);
        result.Stats["attack"].Should().Be(55);
    }

    [Fact]
    public void Map_PokemonResponse_NullSprite_MapsSpriteUrlAsNull()
    {
        // Arrange
        var source = BuildMinimalPokemonResponse() with
        {
            Sprites = new PokeApiSprites(FrontDefault: null)
        };

        // Act
        var result = _mapper.Map<PokemonDto>(source);

        // Assert
        result.SpriteUrl.Should().BeNull();
    }

    // ── PokeApiSpeciesResponse → PokemonSpeciesDto ────────────────────────────

    [Fact]
    public void Map_SpeciesResponse_MapsScalarProperties()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse();

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.Id.Should().Be(25);
        result.Name.Should().Be("pikachu");
        result.IsLegendary.Should().BeFalse();
        result.IsMythical.Should().BeFalse();
        result.CaptureRate.Should().Be(190);
        result.BaseHappiness.Should().Be(70);
        result.Generation.Should().Be("generation-i");
    }

    [Fact]
    public void Map_SpeciesResponse_MapsEnglishFlavorText()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse() with
        {
            FlavorTextEntries =
            [
                new PokeApiFlavorTextEntry("Un pokemon de agua.", new PokeApiNamedResource("es", "")),
                new PokeApiFlavorTextEntry("A mouse pokemon.", new PokeApiNamedResource("en", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.FlavorText.Should().Be("A mouse pokemon.");
    }

    [Fact]
    public void Map_SpeciesResponse_FlavorText_NormalisesNewlines()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse() with
        {
            FlavorTextEntries =
            [
                new PokeApiFlavorTextEntry("A mouse\npokemon.\fRare.", new PokeApiNamedResource("en", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.FlavorText.Should().Be("A mouse pokemon. Rare.");
    }

    [Fact]
    public void Map_SpeciesResponse_NoEnglishFlavorText_ReturnsEmptyString()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse() with
        {
            FlavorTextEntries =
            [
                new PokeApiFlavorTextEntry("Un pokemon.", new PokeApiNamedResource("es", ""))
            ]
        };

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.FlavorText.Should().BeEmpty();
    }

    [Fact]
    public void Map_SpeciesResponse_NullHabitat_MapsHabitatAsNull()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse() with { Habitat = null };

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.Habitat.Should().BeNull();
    }

    [Fact]
    public void Map_SpeciesResponse_WithHabitat_MapsHabitatName()
    {
        // Arrange
        var source = BuildMinimalSpeciesResponse() with
        {
            Habitat = new PokeApiNamedResource("forest", "")
        };

        // Act
        var result = _mapper.Map<PokemonSpeciesDto>(source);

        // Assert
        result.Habitat.Should().Be("forest");
    }

    // ── Builders ─────────────────────────────────────────────────────────────

    private static PokeApiPokemonResponse BuildMinimalPokemonResponse() =>
        new(
            Id: 25,
            Name: "pikachu",
            BaseExperience: 112,
            Height: 4,
            Weight: 60,
            Types: [],
            Abilities: [],
            Stats: [],
            Sprites: new PokeApiSprites(FrontDefault: null));

    private static PokeApiSpeciesResponse BuildMinimalSpeciesResponse() =>
        new(
            Id: 25,
            Name: "pikachu",
            FlavorTextEntries: [new PokeApiFlavorTextEntry("A mouse pokemon.", new PokeApiNamedResource("en", ""))],
            Habitat: new PokeApiNamedResource("forest", ""),
            Generation: new PokeApiNamedResource("generation-i", ""),
            IsLegendary: false,
            IsMythical: false,
            CaptureRate: 190,
            BaseHappiness: 70);
}
