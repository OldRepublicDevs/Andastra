# Runtime Layering

Layered architecture for the Andastra runtime, corrected to match repository layout.

## Intended Dependency Direction

Higher layers depend on lower layers; domain core avoids graphics/game-engine bindings. `[REPO]` (`README.md`)

```
┌─────────────────────────────────────────────────────────────┐
│              Andastra.Game (executable + launcher)           │
├─────────────────────────────────────────────────────────────┤
│  Game/Games/{Odyssey,Aurora,Eclipse}  │  Game/Graphics/*   │
│  Game/Scripting (NCS VM)              │  UI (Avalonia)     │
├─────────────────────────────────────────────────────────────┤
│  Runtime: Core │ Content │ Graphics │ Games/Common         │
├─────────────────────────────────────────────────────────────┤
│  BioWare (parsers, extract, TSLPatcher)                     │
└─────────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

| Layer | Path | Role |
|-------|------|------|
| **Executable** | `src/Andastra/Game/` | Entry point, launcher GUI, engine profiles, game loop |
| **Runtime domain** | `src/Andastra/Runtime/Core/` | Pure domain logic |
| **Content pipeline** | `src/Andastra/Runtime/Content/` | Asset conversion and caching |
| **Graphics abstraction** | `src/Andastra/Runtime/Graphics/`, `src/Andastra/Graphics/`, `Game/Graphics/{MonoGame,Stride,Common}/` | Rendering backends |
| **Shared game rules** | `Runtime/Games/Common/`, `Game/Games/Common/` | Cross-engine abstractions |
| **Engine-specific** | `Game/Games/{Odyssey,Aurora,Eclipse}/` | K1/TSL rules, loaders, combat, dialogue |
| **Scripting** | `src/Andastra/Game/Scripting/` | NCS VM — **not** a separate `Runtime.Scripting` csproj `[REPO]` |
| **Formats** | `src/BioWare/` | GFF, 2DA, TLK, MDL, NCS parsers, chitin/capsule extract |

## README vs Actual

README diagram lists `Runtime.Scripting` and `Runtime.Games.Odyssey` as separate projects. `[REPO]`

Actual layout:

- Scripting lives under `Game/Scripting/`
- Engine implementations live under `Game/Games/{Engine}/`
- `Runtime/Games/` contains only `Common/` plus interfaces

See [game-vs-runtime-split.md](game-vs-runtime-split.md).

## Dependency Rules

- `Runtime.Core` should remain free of MonoGame/Stride direct coupling where possible. `[REPO]`
- BioWare is foundational; referenced by Runtime, Game, and Tools. `[REPO]`
- `Andastra.Runtime.csproj` excludes some Game-coupled files via `<Compile Remove>`. `[REPO]`
- Prefer conditional branching (`GameType.K1` / `K2`) over new inheritance hierarchies for game differences. `[REPO]` (`.cursorrules`)

## Repo Implications

- New domain types start in `Runtime/Core` if graphics-agnostic.
- Engine-specific mechanics go to `Game/Games/Odyssey/` (or target engine folder).
- Format parsing changes belong in BioWare, not runtime.
