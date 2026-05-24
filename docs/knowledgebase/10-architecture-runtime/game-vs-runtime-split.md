# Game vs Runtime Split

Documents the actual split between `Runtime/` and `Game/` trees — a known README drift area.

## The Drift

**README claims:** `[REPO]`

- `Andastra.Runtime.Games.Odyssey` (and Aurora, Eclipse, Infinity)
- `Andastra.Runtime.Scripting` as separate runtime project

**Repository reality:** `[REPO]`

| README name | Actual location |
|-------------|-----------------|
| Runtime.Games.Odyssey | `src/Andastra/Game/Games/Odyssey/` |
| Runtime.Games.Aurora | `src/Andastra/Game/Games/Aurora/` |
| Runtime.Games.Eclipse | `src/Andastra/Game/Games/Eclipse/` |
| Runtime.Scripting | `src/Andastra/Game/Scripting/` |
| Runtime.Games.Common | Split: `Runtime/Games/Common/` **and** `Game/Games/Common/` |

Additional: `src/Andastra/Runtime/Engines/Odyssey/` exists with Odyssey-specific runtime hooks. `[REPO]`

## Why Both `Runtime/Games/Common` and `Game/Games/Common`

`[SYNTH]` The split reflects historical layering:

- **Runtime/Games/Common** — interfaces and shared types consumed by the runtime library assembly
- **Game/Games/Common** — executable-side engine profiles, loaders, and game-loop integration

When adding shared abstractions, check which assembly already references the target namespace before choosing a folder.

## Engine Folder Contents (Odyssey example)

`src/Andastra/Game/Games/Odyssey/` typically includes: `[REPO]`

- Profiles and installation detection
- Combat, dialogue, save/load subsystems
- Engine-specific loaders and render glue

Cross-reference `docs/CROSS_BINARY_*` and `docs/ghidra_*` for RE-backed behavior maps.

## Infinity Engine

README lists `Andastra.Runtime.Games.Infinity` but no `Game/Games/Infinity/` directory was found. `[REPO]`/`[OPEN]` Treat as future scope only.

## Repo Implications

- Update KB and new docs with **actual paths**; do not copy README project names blindly.
- PR reviewers should reject new code under fictional `Runtime.Games.Odyssey` paths unless a refactor explicitly moves them.
- A future refactor to align README with layout would be a dedicated architectural change — not implied by this KB.
