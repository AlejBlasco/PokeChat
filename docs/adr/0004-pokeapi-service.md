# 0004. PokeAPI Service

## Status
Amended — 2026-03-14

## Date
2026-03-14

## Context
PokeChat requires a service to retrieve Pokémon data from the public PokeAPI (https://pokeapi.co).
This service will act as the data provider for the future Semantic Kernel integration, supplying
contextual Pokémon information to the AI chat assistant.

Two endpoints are required:
- `GET /pokemon/{name}` — base stats, types, abilities, sprites.
- `GET /pokemon-species/{name}` — flavor text, habitat, evolution chain info.

The PokeAPI does not support batch requests natively. To mitigate repeated calls and reduce
latency, two strategies are applied:
1. **Parallel requests** via `Task.WhenAll` for multiple Pokémon in a single call.
2. **In-memory cache** via `IMemoryCache` to avoid redundant HTTP calls within the same session.

## Decision
Implement `IPokeApiService` in `PokeChat.Application` with:
- A typed `HttpClient` registered via `IHttpClientFactory`.
- `IMemoryCache` for response caching (TTL: 1 hour).
- `Task.WhenAll` for parallel multi-pokemon fetching.
- Two DTOs: `PokemonDto` and `PokemonSpeciesDto` with fields relevant to Semantic Kernel context.
- Full DI registration via `ApplicationServiceExtensions`.
- Base URL and timeout configured via `appsettings.json` — never hardcoded.

### Amendment: AutoMapper for all DTO mappings
All mappings from PokeAPI response models to DTOs **must use AutoMapper**.
Manual `Map*` static methods in `PokeApiService` are prohibited.
A dedicated `MappingProfile` class must be created in `PokeChat.Application/Mappings/`.
AutoMapper must be registered in DI via `ApplicationServiceExtensions`.
Custom value resolvers must be used for non-trivial mappings (e.g. flavor text extraction).

### Amendment: PokeAPI response models in dedicated folder
PokeAPI raw response records must **not** live in the same file as `PokeApiService`.
They must be extracted to `PokeChat.Application/HttpModels/` in a single file `PokeApiResponses.cs`.
Namespace: `PokeChat.Application.HttpModels`.
These types remain `internal` to the Application layer — never exposed to Web or Domain.

### Amendment: Test class organisation by folder, not by filename prefix
Test classes must be organised into subfolders by the class under test.
Underscore separators in filenames (e.g. `PokeApiService_GetPokemonTests.cs`) are forbidden.

| Folder | File | Responsibility |
|---|---|---|
| `Services/PokeApiService/` | `GetPokemonTests.cs` | Tests for `GetPokemonAsync` |
| `Services/PokeApiService/` | `GetPokemonSpeciesTests.cs` | Tests for `GetPokemonSpeciesAsync` |
| `Services/PokeApiService/` | `GetMultiplePokemonTests.cs` | Tests for `GetMultiplePokemonAsync` |
| `Helpers/` | `FakeHttpMessageHandler.cs` | Shared fake HTTP handler |
| `Helpers/` | `PokeApiJsonBuilder.cs` | JSON payload builders |
| `Helpers/` | `PokeApiServiceFactory.cs` | SUT factory |

Namespaces must reflect folder structure:
- `PokeChat.ApplicationTests.Services.PokeApiService`
- `PokeChat.ApplicationTests.Helpers`

## Consequences
### Positive
- AutoMapper centralises all mapping logic — easier to maintain and extend for new fields.
- MappingProfile is testable in isolation.
- Response models in `HttpModels/` are clearly separated from service logic.
- Test folder structure mirrors source folder structure — easy to navigate.
- No underscore naming — consistent with .NET naming conventions.

### Negative
- AutoMapper adds a NuGet dependency (`AutoMapper`).
- Complex mappings (flavor text) require custom resolvers, adding some boilerplate.

### Risks
- PokeAPI is a public API with no SLA — mitigated by graceful null returns on failure.
- AutoMapper misconfiguration is a runtime risk — mitigated by `AssertConfigurationIsValid()` in tests.
