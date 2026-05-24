# Quick Start Guide

Fast path into Andastra development. For full runbooks, use the [knowledgebase](knowledgebase/90-meta/README.md).

## Prerequisites

- **.NET 9.0 SDK** or later
- **MonoGame Content Builder** (`dotnet tool install -g dotnet-mgcb`) for runtime content pipeline
- **Arial font** on Linux: copy at `src/Andastra/Game/Fonts/Arial.ttf` (see [dev-environment-setup](knowledgebase/50-execution/dev-environment-setup.md))

Optional for runtime testing: a local KOTOR I or II installation.

## Clone and Build (recommended path)

Always pass `--framework net9.0` on Linux for multi-target projects.

```bash
git clone https://github.com/th3w1zard1/Andastra.git
cd Andastra

# Narrowest green path — BioWare library
dotnet build src/BioWare/BioWare.csproj --framework net9.0

# Tests
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0
dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0
```

Full solution build (`dotnet build Andastra.sln --framework net9.0`) succeeds on Linux. Tool chain (OdyTools, OdyPatch, KotorCLI, ConvertKotorGame) compiles on net9.0. See [build-health-matrix](knowledgebase/40-operational-risk/build-health-matrix.md).

## Run the Game

Requires a game installation:

```bash
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --help
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --game-path "/path/to/KOTOR"
```

Use `--no-launcher` or `--game` to skip the Avalonia launcher. Details: [run-game-runtime](knowledgebase/50-execution/run-game-runtime.md).

## Run Tools

```bash
dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0 -- --help
dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0 -- --help
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help
dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0
```

OdyPatch is the runnable host (launches OdyPatch.UI). Do not `dotnet run` the UI library csproj. `[REPO]`

Optional — pack OdyPatch NuGet package (Linux):

```bash
./helper_scripts/build-nuget.sh
```

See [NUGET.md](NUGET.md) and [run-tools-reference](knowledgebase/50-execution/run-tools-reference.md).

Runtime mod-install UX requires a local K1/TSL install. Details: [run-tools-reference](knowledgebase/50-execution/run-tools-reference.md).

## Agents and Contributors

- **Agents:** Start at [agent-workflow](knowledgebase/50-execution/agent-workflow.md) and [evidence-contract](knowledgebase/90-meta/evidence-contract.md)
- **Engine RE work:** [reverse-engineering-methodology](knowledgebase/20-domain-theory/reverse-engineering-methodology.md) (K1 + TSL dual-binary required)
- **Architecture:** Use [game-vs-runtime-split](knowledgebase/10-architecture-runtime/game-vs-runtime-split.md) for current layout

## Next Steps

| Goal | Document |
|------|----------|
| Engine family status | [engine_roadmap.md](engine_roadmap.md) |
| Full build ladder | [build-and-test-ladder](knowledgebase/50-execution/build-and-test-ladder.md) |
| NuGet pack (OdyPatch) | [NUGET.md](NUGET.md) |
| File formats | [file-format-catalog](knowledgebase/20-domain-theory/file-format-catalog.md) + `wiki/` |
| Cursor Cloud setup | [AGENTS.md](../AGENTS.md) |

## Repo Implications

This file is a thin entry point; authoritative operational detail lives in `docs/knowledgebase/`. Update KB first when build paths change, then refresh this guide's links.
