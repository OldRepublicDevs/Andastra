# Andastra.NET

## Cursor Cloud specific instructions

### Environment

- **.NET 9.0 SDK** is installed at `$HOME/.dotnet`. The `DOTNET_ROOT` and `PATH` env vars are configured in `~/.bashrc`.
- **MonoGame Content Builder** (`dotnet-mgcb`) is installed as a global dotnet tool.
- **Arial font** (`ttf-mscorefonts-installer`) must be present for the MonoGame content pipeline to process `.spritefont` files. A copy of `Arial.ttf` is placed at `src/Andastra/Game/Fonts/Arial.ttf` because MonoGame on Linux cannot resolve system fonts by name alone.

### Build, Test, Lint

Standard commands per `README.md`. Key notes:

- **Restore**: `dotnet restore Andastra.sln` succeeds on current branch. Full solution **build** succeeds on Linux net9.0 (2026-05-23, plan 020): `dotnet build Andastra.sln --framework net9.0`.
- **Build core libraries**: `dotnet build src/BioWare/BioWare.csproj` builds cleanly for both `net9.0` and `net48`.
- **Build tests**: `dotnet build tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0` and `dotnet build tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0`.
- **Run tests**: `dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0` and `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0`.
- **Lint**: `dotnet build src/BioWare/BioWare.csproj --configuration Release -p:RunAnalyzersDuringBuild=true --framework net9.0`.
- **OdyTools** and **OdyPatch** build on net9.0 after delegate-wiring fixes (2026-05-23). Standalone editors use shared props including `DialogHelper.cs`.
- The `powershell: not found` warnings during BioWare builds (`Remove-DuplicateUsings.ps1` pre-build target) are harmless on Linux.

### Running tools

- **NSSComp** (NWScript compiler/decompiler): `dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0 -- --help`
- **NCSDecomp.CLI**: `dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0 -- --help`
- **KotorCLI**: `dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help`
- **ConvertKotorGame**: `dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0`
- **OdyPatch**: `dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0`

### AgentDecompile Integration

The workspace is configured to talk to an external **Agent Decompile** HTTP server via the
`.vscode/mcp.json` settings. This server proxies requests to a Ghidra installation and
exposes a rich set of reverse‑engineering tools such as:

- `mcp_agentdecompil_get_current_program`
- `mcp_agentdecompil_list_functions`
- `mcp_agentdecompil_search_everything`
- `mcp_agentdecompil_decompile_function`
- `mcp_agentdecompil_get_call_graph`
- `mcp_agentdecompil_match_function`
- `mcp_agentdecompil_execute_script`
- `mcp_agentdecompil_analyze_data_flow` and many others (see the MCP docs or run `mcp_agentdecompil_* --help`).

These tools allow us to compare the K1 and TSL local Ghidra programs
(`/K1_swkotor` and `/TSL_swkotor2`) and
automatically back‑fill source comments, labels, and structures in the decompiler
workspace.

> **Note:** The Agent Decompile HTTP server must be running and reachable from the
> environment when you intend to use the MCP bridge. If the bridge is unavailable,
> continue the analysis against the local Ghidra programs (`/K1_swkotor` and
> `/TSL_swkotor2`) and verify the host/port in `.vscode/mcp.json` before retrying.

### Gotchas

- Always specify `--framework net9.0` when building or running projects that multi-target `net9.0;net48` to avoid net48-specific issues on Linux.
- The BioWare.csproj pre-build step tries to invoke PowerShell; this fails silently on Linux and does not affect the build.
- The game runtime (`Andastra.Game`) requires a KOTOR/TSL game installation at runtime to function.
