# TSLPatcher Domain

Mod patch semantics as implemented in BioWare and exposed via OdyPatch.

## Implementation Location

TSLPatcher logic modules live under **`src/BioWare/`** (TSLPatcher namespace/folders). `[REPO]`

OdyPatch (`src/Tools/OdyPatch/`) is the **runnable host** (GUI + CLI via `Program.cs`). Avalonia UI lives in **OdyPatch.UI** (`src/Tools/OdyPatch.UI/`, library csproj). Patch semantics remain in BioWare. `[REPO]` (plans 044–045)

## Capabilities (README / tool docs)

OdyPatch supports modifications including: 2DA, GFF, TLK, NSS/NCS, SSF. `[REPO]` (`README.md`)

## Investigation Archive

Extensive RE and assembly analysis under `docs/TSLPatcher_*`:

- `TSLPatcher_Reverse_Engineering_Progress.md`
- `TSLPatcher_Assembly_Analysis.md`
- `TSLPatcher_Complete_Implementation_Plan.md`
- `TSLPatcher_Ghidra_Limitations.md`

**Caveat:** These are investigation-era documents. Cross-check against current BioWare TSLPatcher code before treating as complete truth. `[REPO]`/`[OPEN]`

## Wiki / Vendor Syntax Docs

TSLPatcher install list syntax appears in vendor toolset wiki copies. Prefer verifying behavior against BioWare implementation and OdyPatch tests when available.

## Build Status

OdyPatch compiles on net9.0 (2026-05-23). Runtime mod-install UX still requires a local K1/TSL install for validation. TSLPatcher **library** code in BioWare builds via BioWare project independently. `[REPO]`

## Repo Implications

- Patch semantic bugs are BioWare-layer fixes; UI flow bugs are OdyPatch.UI; host/CLI wiring is OdyPatch (`Program.cs`).
- Do not reference HoloPatcher in new docs — OdyPatch is the supported installer name.
- Adding patch features requires RE against original TSLPatcher behavior where fidelity matters.
