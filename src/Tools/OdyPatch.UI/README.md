# OdyPatch.UI

Avalonia UI library for the OdyPatch mod installer. **This project is not directly runnable** (`OutputType=Library`); the desktop host and CLI entry point live in **`OdyPatch`** (`src/Tools/OdyPatch/Program.cs`).

## Run the GUI

From repository root (Linux — always pass `--framework net9.0`):

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

Default mode launches the Avalonia GUI. Use explicit CLI flags for headless install/validate/uninstall (see OdyPatch host README).

## Build

```bash
dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
```

Desktop CI also builds this project on Windows (`dotnet-desktop.yml`). `[REPO]`

## Architecture

| Piece | Role |
|-------|------|
| **OdyPatch** | Runnable host (GUI + CLI) — references this library |
| **OdyPatch.UI** | Views, view models, RTE editor, autoupdate wiring |
| **BioWare.TSLPatcher** | Patch semantics (`src/BioWare/TSLPatcher/`) |
| **OdyTools** | Shared editor/tooling dependencies |

net9.0 references **Andastra**; net48 references **BioWare** directly (dual-target split). `[REPO]` (`OdyPatch.UI.csproj`)

## Key surfaces

- `App.axaml` / `MainWindow.axaml` — primary installer window
- `Core.cs` — mod load, install, game-path validation
- `UpdateManager.cs` / NetSparkle — autoupdate (see [AUTOUPDATE.md](../../../docs/AUTOUPDATE.md))
- `Rte/` — rich-text editor for mod READMEs (net9 vs net48 split views)

## NuGet

Packable (`IsPackable=true`, `PackageId=OdyPatch.UI`, `BSL-1.1`). The published **`OdyPatch`** package (see [NUGET.md](../../../docs/NUGET.md)) is the primary consumer-facing artifact; UI packaging is separate from `helper_scripts/build-nuget.sh` today. `[REPO]`

## Documentation

| Topic | Location |
|-------|----------|
| Host run/CLI | [OdyPatch README](../OdyPatch/README.md) |
| Installer UX (stub) | [odypatch-installer-ux.md](../../../docs/knowledgebase/30-product-ux/odypatch-installer-ux.md) |
| Tool chain | [run-tools-reference.md](../../../docs/knowledgebase/50-execution/run-tools-reference.md) |
| Patch domain | [tslpatcher-domain.md](../../../docs/knowledgebase/20-domain-theory/tslpatcher-domain.md) |

## License

NuGet metadata uses `BSL-1.1` in `OdyPatch.UI.csproj`. The Andastra repository root is **AGPLv3** — see [LICENSE](../../../LICENSE) and [license-and-compliance.md](../../../docs/knowledgebase/40-operational-risk/license-and-compliance.md).
