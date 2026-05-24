# Run Game Runtime

Launching `Andastra.Game` with current CLI surface. `[REPO]`

## Build

```bash
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```

`[REPO]` (project path and TFM)

## Run

```bash
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- [args]
```

`[REPO]`

## CLI Arguments (from Program.cs)

| Flag | Purpose |
|------|---------|
| `--help`, `-?` | Print help via `GameSettingsExtensions.PrintHelp()` |
| `--game` | Select game (skips launcher when specified) |
| `--path` | Game installation path |
| `--no-launcher`, `-n` | Skip Avalonia launcher dialog |

`[REPO]` (`src/Andastra/Game/Program.cs`)

Additional settings parsed via `GameSettingsExtensions.ParseCliArgs`. `[REPO]`

## Requirements

- Valid KOTOR I and/or II installation on disk for gameplay `[REPO]`
- Graphics backend dependencies (MonoGame/Stride) per platform `[REPO]` (`README.md`, `Game/Graphics/`)

See also [MONOGAME_RUNNING.md](../../MONOGAME_RUNNING.md) for MonoGame-oriented quick start (aligned with this doc as of 2026-05-23). `[REPO]`

## Launcher Flow

Default: Avalonia `GameLauncher` GUI unless `--no-launcher` or `--game` forces CLI path. `[REPO]`

## Repo Implications

- Runtime validation is manual — no CI game-run. `[REPO]`
- RE-backed comments in `Program.cs` (WinMain, mutex `swkotor2`, config loaders) are reference examples for dual-address format. `[REPO]`
- Missing game path should error clearly — verify UX when changing launcher. `[OPEN]`
