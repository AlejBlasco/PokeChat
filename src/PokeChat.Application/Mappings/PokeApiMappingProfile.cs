using AutoMapper;
using PokeChat.Application.DTOs;
using PokeChat.Application.HttpModels;

namespace PokeChat.Application.Mappings;

/// <summary>
/// AutoMapper profile defining all mappings from PokeAPI response models to application DTOs.
/// </summary>
public sealed class PokeApiMappingProfile : Profile
{
    /// <summary>Initializes a new instance of <see cref="PokeApiMappingProfile"/>.</summary>
    public PokeApiMappingProfile()
    {
        // PokeApiPokemonResponse → PokemonDto
        CreateMap<PokeApiPokemonResponse, PokemonDto>()
            .ForMember(dest => dest.Types,
                opt => opt.MapFrom(src => src.Types.Select(t => t.Type.Name).ToList()))
            .ForMember(dest => dest.Abilities,
                opt => opt.MapFrom(src => src.Abilities.Select(a => a.Ability.Name).ToList()))
            .ForMember(dest => dest.Stats,
                opt => opt.MapFrom(src => src.Stats.ToDictionary(s => s.Stat.Name, s => s.BaseStat)))
            .ForMember(dest => dest.SpriteUrl,
                opt => opt.MapFrom(src => src.Sprites.FrontDefault))
            .ForMember(dest => dest.BaseExperience,
                opt => opt.MapFrom(src => src.BaseExperience));

        // PokeApiSpeciesResponse → PokemonSpeciesDto
        CreateMap<PokeApiSpeciesResponse, PokemonSpeciesDto>()
            .ForMember(dest => dest.FlavorText,
                opt => opt.MapFrom<FlavorTextResolver>())
            .ForMember(dest => dest.Habitat,
                opt => opt.MapFrom(src => src.Habitat != null ? src.Habitat.Name : null))
            .ForMember(dest => dest.Generation,
                opt => opt.MapFrom(src => src.Generation.Name));
    }
}
