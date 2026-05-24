# OdyPatch

TSLPatcher-compatible mod installer for Knights of the Old Republic I and II. OdyPatch is the **host library**; the Avalonia GUI lives in **OdyPatch.UI** (`src/Tools/OdyPatch.UI/`).

Patch semantics are implemented in **`BioWare.TSLPatcher`** (`src/BioWare/TSLPatcher/`) — not a separate `TSLPatcher.Core` project.

## Quick start

From repository root (Linux — always pass `--framework net9.0`):

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet run --project src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
```

Build:

```bash
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

## NuGet

OdyPatch is packable (`IsPackable=true`). See [docs/NUGET.md](../../../docs/NUGET.md) and run `./helper_scripts/build-nuget.sh` from repo root.

## Documentation

| Topic | Location |
|-------|----------|
| Installer UX (stub) | [odypatch-installer-ux.md](../../../docs/knowledgebase/30-product-ux/odypatch-installer-ux.md) |
| Patch domain | [tslpatcher-domain.md](../../../docs/knowledgebase/20-domain-theory/tslpatcher-domain.md) |
| Tool chain | [run-tools-reference.md](../../../docs/knowledgebase/50-execution/run-tools-reference.md) |
| CI / release | [docs/WORKFLOWS.md](../../../docs/WORKFLOWS.md) |

## Supported mod changes

2DA, GFF, TLK, NSS/NCS, SSF — see BioWare TSLPatcher modules and [tslpatcher-domain.md](../../../docs/knowledgebase/20-domain-theory/tslpatcher-domain.md).

## Port note

OdyPatch descends from a Python/Tkinter HoloPatcher lineage under `vendor/`. Andastra uses **OdyPatch** naming only (no HoloPatcher in solution). Functional parity claims require verification against BioWare implementation and original TSLPatcher behavior.

## License

NuGet metadata uses `LGPL-3.0-only` in `OdyPatch.csproj`. The Andastra repository root is **AGPLv3** — see [LICENSE](../../../LICENSE) and [license-and-compliance.md](../../../docs/knowledgebase/40-operational-risk/license-and-compliance.md) for combined distribution questions.
