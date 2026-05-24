# Run Game Runtime

Launching `Andastra.Game` with current CLI surface.

## Build

```bash
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```

## Run

```bash
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- [args]
```

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
- Graphics backend dependencies (MonoGame/Stride) per platform

## Obsolete Documentation Warning

**Do not follow** `docs/MONOGAME_RUNNING.md` verbatim — references obsolete `OdysseyRuntime` paths. `[REPO]`

Use this KB doc + current `Program.cs` instead.

## Launcher Flow

Default: Avalonia `GameLauncher` GUI unless `--no-launcher` or `--game` forces CLI path. `[REPO]`

## Repo Implications

- Runtime validation is manual — no CI game-run.
- RE-backed comments in `Program.cs` (WinMain, mutex `swkotor2`, config loaders) are reference examples for dual-address format.
- Missing game path should error clearly — verify UX when changing launcher.
