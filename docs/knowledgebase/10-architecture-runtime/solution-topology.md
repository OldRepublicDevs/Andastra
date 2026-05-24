# Solution Topology

Overview of `Andastra.sln` project clusters and build expectations.

## Scale

- **57 projects** in `Andastra.sln`. `[REPO]`
- Monorepo: runtime libraries, game executable, BioWare formats, 25+ tool projects, test projects. `[REPO]`

## Project Clusters

```
Andastra.sln
├── Runtime & Game
│   ├── src/Andastra/Andastra.csproj          (aggregator / startup shell)
│   ├── src/Andastra/Runtime/Andastra.Runtime.csproj
│   ├── src/Andastra/Game/Andastra.Game.csproj   (main executable)
│   ├── src/Andastra/Graphics/
│   └── src/Andastra/UI/
├── Formats & Resources
│   ├── src/BioWare/BioWare.csproj            (net9.0;net48)
│   └── src/BioWare/Utility/BioWare.Utility.csproj
├── Tools (src/Tools/)
│   ├── OdyPatch / OdyPatch.UI
│   ├── OdyTools (+ 25 standalone editor csprojs)
│   ├── NSSComp, NCSDecomp.CLI, KotorDiff, KotorCLI, ConvertKotorGame
│   └── ...
└── Tests
    ├── tests/Andastra.Tests/
    ├── tests/BioWare.Tests/
    └── tests/OdyTools.Tests/
```

## Target Frameworks

| Project class | TFM | Notes |
|---------------|-----|-------|
| BioWare, many tools, tests | `net9.0;net48` | Always pass `--framework net9.0` on Linux. `[REPO]` |
| Runtime / Game | `net9.0` | Primary runtime target. `[REPO]` |
| OdyPatch | `net9.0` + `net48` | net9 path references Andastra. `[REPO]` |

## Build Health Summary

| Status | Projects |
|--------|----------|
| **Green (baseline)** | BioWare, BioWare.Tests, Andastra.Tests, NSSComp, NCSDecomp.CLI, OdyTools, OdyPatch, standalone OdyTool editors, KotorCLI, ConvertKotorGame | `[REPO]` |
| **Green (full solution)** | `Andastra.sln` net9.0 on Linux + CI `solution-build` job (2026-05-23) | `[REPO]` |
| **Not in solution** | `src/StrideGameFPS/` on disk; `MonoGameFPS` gitignored/absent | `[REPO]` |

Detail: [build-health-matrix.md](../40-operational-risk/build-health-matrix.md)

## Key Dependencies

- **MonoGame** 3.8.4.1 — primary graphics backend. `[REPO]`
- **Stride** 4.2.x — secondary graphics/audio backend on Game project. `[REPO]`
- **Avalonia** 11.x — tool UIs. `[REPO]`
- **NUnit** 4.x — test framework. `[REPO]`

## Restore Gotcha

`MonoGameFPS` / `StrideGameFPS` are not in `Andastra.sln`; `dotnet restore Andastra.sln` succeeds. Full solution net9.0 build succeeds on Linux (2026-05-23). `[REPO]`

## Repo Implications

- Agents should default to building **BioWare + targeted tests** for narrow slices; full solution build is green on net9.0 Linux when validating broad changes.
- Tool work may target OdyTools AIO, OdyPatch, or standalone editors — all compile on net9.0 as of 2026-05-23.
- Multi-target builds need explicit `--framework net9.0` on Linux.
