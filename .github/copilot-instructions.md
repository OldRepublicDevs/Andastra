# Project Guidelines

## Code Style
- Follow each project file's configured language version. Most projects in this repo pin `LangVersion` to `7.3` (for example `src/Andastra/Andastra.csproj`), while some utility/tooling projects use newer versions.
- Keep `Nullable` and `ImplicitUsings` disabled unless the specific project already enables them.
- Preserve existing text/encoding behavior in script tooling paths (for example Windows-1252 handling in `src/Andastra/Compilers.cs` and `src/Andastra/NCSCompiler.cs`).
- Do not broadly change warning policies; this repo intentionally does not treat warnings as errors.

## Architecture
- Respect the layered runtime structure documented in `README.md`: `Andastra.Game` -> runtime layers -> scripting/core -> format parsing.
- Keep engine-specific behavior in `src/Andastra/Runtime/Games/<Engine>` and shared abstractions in common/core runtime layers.
- Keep file-format parsing/manipulation logic in `src/BioWare` and avoid duplicating parser logic in game/runtime projects.
- Treat `vendor/` as third-party code: avoid edits there unless the task explicitly requires it.

## Build and Test
- Prefer VS Code tasks in `.vscode/tasks.json` for repeatable local workflows.
- Common commands:
  - `dotnet restore Andastra.sln`
  - `dotnet build Andastra.sln`
  - `dotnet test Andastra.sln --verbosity normal`
  - `dotnet build src/Andastra/Andastra.csproj /property:Configuration=Debug`
- For runtime execution, use `dotnet run --project src/Andastra/Game/Andastra.Game.csproj -- --game-path "<path-to-game>"`.

## Conventions
- Validate paths against the current tree before scripting changes. Some docs and older scripts may reference historical locations.
- Keep tool-specific changes under `src/Tools/` and runtime/game changes under `src/Andastra/`.
- Be mindful that Debug and Release builds are configured differently (analyzers/perf toggles in `src/Andastra/Andastra.csproj`).
- Do not change package version pinning/suppression in `Directory.Build.props` and `Directory.Build.targets` unless the task explicitly requires dependency work.
