# Development Environment Setup

Local environment for building Andastra on Linux (Cursor Cloud aligned).

## Required

| Component | Setup |
|-----------|-------|
| **.NET 9 SDK** | `$HOME/.dotnet`; `DOTNET_ROOT` and `PATH` in `~/.bashrc` `[REPO]` (`AGENTS.md`) |
| **MonoGame Content Builder** | Global tool: `dotnet tool install -g dotnet-mgcb` `[REPO]` |
| **Arial font** | `ttf-mscorefonts-installer`; copy at `src/Andastra/Game/Fonts/Arial.ttf` for Linux content pipeline `[REPO]` |

## Optional / Engine Work

| Component | Setup |
|-----------|-------|
| **AgentDecompile MCP** | HTTP server + Ghidra; config in `.vscode/mcp.json` / workspace MCP `[REPO]` |
| **Ghidra programs** | `/K1_swkotor`, `/TSL_swkotor2` `[REPO]` |
| **Game installs** | KOTOR I and/or II paths for runtime testing `[REPO]` |

## Linux Gotchas

- Always pass `--framework net9.0` for multi-target projects `[REPO]`
- BioWare pre-build PowerShell script fails silently on Linux — harmless `[REPO]`
- `powershell: not found` warnings during BioWare build — ignore `[REPO]`

## Restore

```bash
dotnet restore Andastra.sln
```

Full solution restore succeeds on current branch (2026-05-23). For narrow agent loops, restore individual csprojs (BioWare, Game, tests). `[REPO]`

## NuGet pack (optional)

OdyPatch is the packable NuGet project. On Linux:

```bash
./helper_scripts/build-nuget.sh
```

Docs: `docs/NUGET.md`, `docs/NUGET_SETUP.md`, `docs/MANUAL_PUSH_INSTRUCTIONS.md`. Requires valid SPDX in `OdyPatch.csproj` (`LGPL-3.0-only`, plan 035). `[REPO]`

## OdyPatch (installer)

GUI and CLI share the **OdyPatch** executable host; **OdyPatch.UI** is the Avalonia library:

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
```

See [run-tools-reference.md](run-tools-reference.md) and tool READMEs under `src/Tools/OdyPatch/`. `[REPO]`

## Repo Implications

- Agents without game installs can still build/test BioWare and NCS roundtrips.
- RE work requires AgentDecompile bridge — report immediately if unavailable per `.cursorrules`.
- Font/content pipeline failures on Linux usually mean missing Arial.ttf copy.
