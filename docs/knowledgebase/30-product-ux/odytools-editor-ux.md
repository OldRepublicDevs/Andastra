# OdyTools Editor UX

Content authoring and inspection workflows for OdyTools and standalone editors.

## Surfaces

| Surface | Entry | Notes |
|---------|-------|-------|
| **OdyTools AIO** | `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0` | Holocron-style combined editor `[REPO]` |
| **Standalone editors** | Per-csproj under `src/Tools/OdyTools/` (GFF, DLG, NSS, etc.) | Prefer individual csproj over AIO for narrow tasks `[REPO]` |
| **ConvertKotorGame** | `dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0` | K1↔TSL portability wizard `[REPO]` |

## Expected workflow (typical mod author)

1. Open or create a game resource (GFF, DLG, 2DA, etc.) via the relevant editor. `[SYNTH]`
2. Edit with format-aware UI; save back to module/installation layout. `[SYNTH]`
3. Optional: compile NSS via NSSComp; diff installs via KotorDiff. `[REPO]` ([run-tools-reference.md](../50-execution/run-tools-reference.md))

## Verification status

| Claim | Status |
|-------|--------|
| OdyTools + standalones compile on net9.0 | Green `[REPO]` (2026-05-23) |
| Editor roundtrip fidelity vs original Holocron/PyKotor | **Unverified** `[OPEN]` |
| Full AIO launch UX on Linux | **Partial** — compile green; GUI runtime not CI-tested `[OPEN]` |

## Test coverage

- `tests/OdyTools.Tests/` covers selected editor behaviors (DLG, GFF, MDL, etc.). `[REPO]`
- No browser/automation suite for Avalonia UX in CI. `[REPO]`

## Repo implications

- Format correctness bugs → BioWare parsers + editor code under `OdyTools/`.
- Prefer standalone editor csproj when debugging a single format surface.
