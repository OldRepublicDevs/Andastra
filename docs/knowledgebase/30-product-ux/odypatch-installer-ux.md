# OdyPatch Installer UX

Mod installation workflow for OdyPatch and OdyPatch.UI.

## Surfaces

| Surface | Entry | Notes |
|---------|-------|-------|
| **Tool README** | [src/Tools/OdyPatch/README.md](../../../src/Tools/OdyPatch/README.md) | Build/run entry points `[REPO]` |
| **OdyPatch.UI** | `dotnet run --project src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0` | Avalonia GUI — primary end-user path `[REPO]` |
| **OdyPatch host** | `dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0` | Host library / CLI-capable entry `[REPO]` |
| **NuGet consumer** | `OdyPatch` package — see [NUGET.md](../../NUGET.md) | Packable library; UI in separate csproj `[REPO]` |

## Expected workflow (TSLPatcher-compatible mods)

1. User selects a mod folder containing `changes.ini` (or equivalent TSLPatcher layout). `[REPO]`/`[SYNTH]`
2. User selects KOTOR I or II installation directory. `[REPO]`/`[SYNTH]`
3. Installer applies 2DA, GFF, TLK, NSS/NCS, SSF patches per mod config. `[REPO]` ([tslpatcher-domain.md](../20-domain-theory/tslpatcher-domain.md))

## Verification status

| Claim | Status |
|-------|--------|
| Projects compile on net9.0 | Green `[REPO]` (2026-05-23) |
| End-to-end mod install against real K1/TSL install | **Unverified** `[OPEN]` |
| CLI `--install` parity with legacy TSLPatcher | **Unverified** `[OPEN]` |
| Autoupdate (NetSparkle) UX | Documented in `docs/AUTOUPDATE.md`; not exercised in CI `[OPEN]` |

## Repo implications

- UX bugs split: patch semantics → BioWare TSLPatcher; UI flow → OdyPatch.UI.
- Validation requires a local game install; not available in standard CI.
