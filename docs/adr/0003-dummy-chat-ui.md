# 0003. Dummy Chat UI Component

## Status
Accepted

## Date
2026-03-13

## Context
PokeChat requires a chat interface that will eventually integrate with Microsoft Semantic Kernel.
Before implementing the AI backend, a functional dummy frontend is needed to:
- Validate the UI/UX design and Pokémon theme.
- Provide a foundation for future Semantic Kernel integration.
- Allow designer and developer agents to work independently on UI and backend.

The chat interface must follow modern chatbot UI standards (header, message history,
input area, send button) while maintaining the Pokémon theme established in ADR-0001.

## Decision
We will implement a self-contained Blazor component `ChatBot.razor` in `PokeChat.Web`
with the following characteristics:
- Dummy responses — no real AI calls, simulated PokéBot replies.
- Full chat UI: header with bot identity, scrollable message history, input field, send button.
- Pokémon-themed design consistent with Bootswatch Darkly + Pokémon overrides (ADR-0001).
- Component hosted on a dedicated `/chat` page.
- Ready for Semantic Kernel integration in a future feature — no breaking changes required.
- No copyrighted images or assets — all imagery must be copyright-free (PokeAPI sprites) or CSS-only.

## Consequences
### Positive
- UI can be validated and iterated independently of AI backend.
- Clean separation between UI and business logic from day one.
- Designer can work on the chat UI without waiting for Semantic Kernel integration.
- Copyright-free approach ensures the project can be published publicly without legal risk.

### Negative
- Dummy responses must be replaced in a future feature — temporary technical debt.
- CSS-only icons and avatars require more implementation effort than using icon libraries.

### Risks
- UI assumptions may not match Semantic Kernel response format — mitigated by keeping
  the message model generic (role + content + timestamp).
