# 0001. Use Pokémon Bootstrap Theme (Bootswatch Darkly + Pokémon Override)

## Status
Accepted

## Date
2026-03-12

## Context
PokeChat requires a visual theme that reflects the Pokémon universe while maintaining
a professional and modern look suitable for a demo project. The application uses
Bootstrap 5 as its CSS framework. A custom theme needs to be applied without
building one from scratch, balancing development speed with visual quality.

Three options were evaluated:
- **Option 1:** Bootswatch theme only (fast, generic).
- **Option 2:** Fully custom CSS on top of Bootstrap 5 (slow, full control).
- **Option 3:** Bootswatch Darkly as base + Pokémon color and style overrides (balanced).

## Decision
We will use **Option 3**: Bootswatch Darkly as the Bootstrap 5 base theme, overridden
with a custom Pokémon-inspired palette (red, yellow, black) and component styles.
PokeAPI sprites are used for imagery as they are copyright-free.

## Consequences
### Positive
- Fast implementation — no need to build a theme from scratch.
- Darkly provides a clean, dark, modern base that complements the Pokémon aesthetic.
- Pokémon type colors are defined as CSS custom properties for easy reuse.
- PokeAPI sprites avoid any copyright issues.

### Negative
- Dependency on Bootswatch CDN — if CDN is unavailable, theme falls back to unstyled.
- Limited customization compared to a fully custom theme.

### Risks
- Bootswatch Darkly updates may conflict with custom overrides — mitigated by pinning CDN version.
