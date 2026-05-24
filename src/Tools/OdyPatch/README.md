# OdyPatch

TSLPatcher-compatible mod installer for Knights of the Old Republic I and II. **OdyPatch** is the runnable host (GUI + CLI); the Avalonia UI library lives in **OdyPatch.UI** (`src/Tools/OdyPatch.UI/`).

Patch semantics are implemented in **`BioWare.TSLPatcher`** (`src/BioWare/TSLPatcher/`) — not a separate `TSLPatcher.Core` project.

## Quick start

From repository root (Linux — always pass `--framework net9.0`):

```bash
# GUI (default) or CLI with --install / --validate / --uninstall
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

`OdyPatch.UI` is a library project — build it via `dotnet build`, do not `dotnet run` the UI csproj directly. See [OdyPatch.UI README](../OdyPatch.UI/README.md).

Build:

```bash
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

Headless config validation (no game install) — mirrors CI `nuget-pack-smoke`:

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- --help
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- \
  --validate --game-dir tests/fixtures/odypatch-fake-game \
  --tslpatchdata tests/fixtures/odypatch-minimal-mod/tslpatchdata
```

Manual mod install against a real K1/TSL install: [odypatch-e2e-runbook.md](../../../docs/knowledgebase/50-execution/odypatch-e2e-runbook.md).

## NuGet

OdyPatch is packable (`IsPackable=true`). See [docs/NUGET.md](../../../docs/NUGET.md) and run `./helper_scripts/build-nuget.sh` from repo root.

## Documentation

| Topic | Location |
|-------|----------|
| Installer UX (stub) | [odypatch-installer-ux.md](../../../docs/knowledgebase/30-product-ux/odypatch-installer-ux.md) |
| Patch domain | [tslpatcher-domain.md](../../../docs/knowledgebase/20-domain-theory/tslpatcher-domain.md) |
| Tool chain | [run-tools-reference.md](../../../docs/knowledgebase/50-execution/run-tools-reference.md) |
| E2E runbook | [odypatch-e2e-runbook.md](../../../docs/knowledgebase/50-execution/odypatch-e2e-runbook.md) |
| CI / release | [docs/WORKFLOWS.md](../../../docs/WORKFLOWS.md) |

## Supported mod changes

2DA, GFF, TLK, NSS/NCS, SSF — see BioWare TSLPatcher modules and [tslpatcher-domain.md](../../../docs/knowledgebase/20-domain-theory/tslpatcher-domain.md).

## Port note

OdyPatch descends from a Python/Tkinter HoloPatcher lineage under `vendor/`. Andastra uses **OdyPatch** naming only (no HoloPatcher in solution). Functional parity claims require verification against BioWare implementation and original TSLPatcher behavior.

## License

NuGet metadata uses `LGPL-3.0-only` in `OdyPatch.csproj`. The Andastra repository root is **AGPLv3** — see [LICENSE](../../../LICENSE) and [license-and-compliance.md](../../../docs/knowledgebase/40-operational-risk/license-and-compliance.md) for combined distribution questions.
