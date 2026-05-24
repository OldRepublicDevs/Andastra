# Running Andastra Game (MonoGame)

> **Authority:** For the current CLI surface and launcher flow, see [run-game-runtime.md](knowledgebase/50-execution/run-game-runtime.md) in the knowledgebase.

## Quick Start

```bash
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --k1 --path "/path/to/KOTOR" --no-launcher
```

On Windows PowerShell, use backslashes in paths if preferred:

```powershell
dotnet run --project src\Andastra\Game\Andastra.Game.csproj --framework net9.0 -- --k1 --path "C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic"
```

## Important Notes

### Correct Project File

- **Use:** `src/Andastra/Game/Andastra.Game.csproj` — main game executable and launcher
- **Do not use:** obsolete `src/OdysseyRuntime/Odyssey.Game/` paths from pre-Andastra layouts

MonoGame and Stride rendering backends live under `src/Andastra/Game/Graphics/` inside the `Andastra.Game` project.

### Command-Line Arguments

See `GameSettingsExtensions` / `Program.cs` for the full set. Common flags:

#### Game Selection

- `--k1`, `-k1` — Run KOTOR 1 (Odyssey default)
- `--k2`, `-k2`, `--tsl` — Run KOTOR 2 (TSL)

#### Paths and Launcher

- `--path <path>`, `-p <path>` — Path to KOTOR installation directory
- `--game` — Select game (skips launcher when specified)
- `--no-launcher`, `-n` — Skip Avalonia launcher dialog

#### Display and Debug

- `--width`, `--height`, `--fullscreen`, `--debug` — when supported by current `GameSettings` parsing

#### Help

- `--help`, `-?` — Show help message

### Examples

```bash
# Run with launcher GUI (default)
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0

# Run KOTOR 1 with explicit path, no launcher
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --k1 --path "/games/KOTOR" --no-launcher

# Show help
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --help
```

### Path Detection

If you do not specify `--path`, the launcher or game settings logic may attempt to auto-detect installation paths. A valid local K1/TSL installation is required for gameplay.

### Building First

```bash
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```

Or build the recommended green path:

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```
