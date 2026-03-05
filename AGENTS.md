# Andastra.NET

## Cursor Cloud specific instructions

### Environment

- **.NET 9.0 SDK** is installed at `$HOME/.dotnet`. The `DOTNET_ROOT` and `PATH` env vars are configured in `~/.bashrc`.
- **MonoGame Content Builder** (`dotnet-mgcb`) is installed as a global dotnet tool.
- **Arial font** (`ttf-mscorefonts-installer`) must be present for the MonoGame content pipeline to process `.spritefont` files. A copy of `Arial.ttf` is placed at `src/Andastra/Game/Fonts/Arial.ttf` because MonoGame on Linux cannot resolve system fonts by name alone.

### Build, Test, Lint

Standard commands per `README.md`. Key notes:

- **Restore**: `dotnet restore Andastra.sln` — the solution references two missing projects (`src/MonoGameFPS/`, `src/StrideGameFPS/`). If restore fails on these, create minimal stub `.csproj` files or restore individual projects instead.
- **Build core libraries**: `dotnet build src/BioWare/BioWare.csproj` builds cleanly for both `net9.0` and `net48`.
- **Build tests**: `dotnet build tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0` and `dotnet build tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0`.
- **Run tests**: `dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0` and `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0`.
- **Lint**: `dotnet build src/BioWare/BioWare.csproj --configuration Release -p:RunAnalyzersDuringBuild=true --framework net9.0`.
- **OdyTools.csproj** has pre-existing compilation errors (method group to `System.Action` conversion) on this branch. **OdyPatch** depends on OdyTools and also fails to build. These are not environment issues.
- The `powershell: not found` warnings during BioWare builds (`Remove-DuplicateUsings.ps1` pre-build target) are harmless on Linux.

### Running tools

- **NSSComp** (NWScript compiler/decompiler): `dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0 -- --help`
- **NCSDecomp.CLI**: `dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0 -- --help`
- **KotorCLI**: Has a pre-existing System.CommandLine API bug; crashes on startup.

### Gotchas

- Always specify `--framework net9.0` when building or running projects that multi-target `net9.0;net48` to avoid net48-specific issues on Linux.
- The BioWare.csproj pre-build step tries to invoke PowerShell; this fails silently on Linux and does not affect the build.
- The game runtime (`Andastra.Game`) requires a KOTOR/TSL game installation at runtime to function.
