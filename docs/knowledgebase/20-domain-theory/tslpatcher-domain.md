# TSLPatcher Domain

Mod patch semantics as implemented in BioWare and exposed via OdyPatch.

## Implementation Location

TSLPatcher logic modules live under **`src/BioWare/`** (TSLPatcher namespace/folders). `[REPO]`

OdyPatch (`src/Tools/OdyPatch/`) provides installer UX compatible with TSLPatcher mod packages. `[REPO]`

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

OdyPatch currently fails to build due to OdyTools dependency chain. `[REPO]` TSLPatcher **library** code in BioWare may still build via BioWare project independently.

## Repo Implications

- Patch semantic bugs are BioWare-layer fixes; UI issues are OdyPatch.UI.
- Do not reference HoloPatcher in new docs — OdyPatch is the supported installer name.
- Adding patch features requires RE against original TSLPatcher behavior where fidelity matters.
