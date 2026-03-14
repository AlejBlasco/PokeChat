# PokeChat 🔴⚡

[![CI](https://github.com/AlejBlasco/PokeChat/actions/workflows/ci.yml/badge.svg)](https://github.com/AlejBlasco/PokeChat/actions/workflows/ci.yml)
[![CodeQL](https://github.com/AlejBlasco/PokeChat/actions/workflows/codeql.yml/badge.svg)](https://github.com/AlejBlasco/PokeChat/actions/workflows/codeql.yml)
[![Dependency Review](https://github.com/AlejBlasco/PokeChat/actions/workflows/dependency-review.yml/badge.svg)](https://github.com/AlejBlasco/PokeChat/actions/workflows/dependency-review.yml)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green)

An AI-powered Pokémon chat assistant built with Blazor and Microsoft Semantic Kernel.

## Overview

PokeChat is a modern .NET demo application showcasing Clean Architecture, CQRS, and AI integration. Users can chat with an AI assistant specialized in Pokémon knowledge, powered by Microsoft Semantic Kernel and the [PokeAPI](https://pokeapi.co).

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor Server (.NET 8) |
| AI / Chat | Microsoft Semantic Kernel |
| External API | PokeAPI |
| Validation | FluentValidation |
| CQRS | MediatR |
| Testing | xUnit, bUnit, FluentAssertions, NSubstitute |
| Test Reports | Allure Reports |
| CI/CD | GitHub Actions |

## Architecture

Clean Architecture with CQRS — no database persistence.

```
src/
├── PokeChat.Domain/        # Entities, Value Objects, Interfaces
├── PokeChat.Application/   # Use Cases, CQRS Handlers, SK Plugins, PokeAPI client
└── PokeChat.Web/           # Blazor Server UI
tests/
├── PokeChat.DomainTests/
├── PokeChat.ApplicationTests/
└── PokeChat.WebTests/
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An [OpenAI](https://platform.openai.com/) API key

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/AlejBlasco/PokeChat.git
   cd PokeChat
   ```

2. Configure secrets:
   ```bash
   dotnet user-secrets init --project src/PokeChat.Web
   dotnet user-secrets set "SemanticKernel:ApiKey" "<your-api-key>" --project src/PokeChat.Web
   dotnet user-secrets set "SemanticKernel:ModelId" "gpt-4o-mini" --project src/PokeChat.Web
   ```

3. Run the application:
   ```bash
   dotnet run --project src/PokeChat.Web
   ```

4. Open your browser at `http://localhost:5000`

### Running Tests

```bash
dotnet test
```

## Security

This project enforces security best practices across 4 levels:

- **Level 1** — Dependency control via Dependabot and Central Package Management
- **Level 2** — Secrets managed via `dotnet user-secrets` and GitHub Secrets
- **Level 3** — Static analysis via CodeQL and SonarCloud
- **Level 4** — CI/CD hardening with pinned action versions and branch protection rules

## License

MIT
