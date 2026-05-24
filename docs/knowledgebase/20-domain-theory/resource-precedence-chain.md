# Resource Precedence Chain

How mod and game resources override each other — critical for modding fidelity.

## Documented Order (README)

Resource precedence for mod support: `[REPO]` (`README.md`)

```
override → module → save → chitin
```

(highest priority first)

## Implementation Evidence

KotorDiff documents resolution order for diff/resolution tooling: `[REPO]` (`src/Tools/KotorDiff/Resolution/ResourceResolver.cs`)

1. Override folder (highest)
2. Module archives (`.mod`, `.rim` ordering within module context)
3. Chitin BIFs (lowest)
4. Save-specific resources where applicable

Comment in resolver: "Resolution order (ONLY applies to Override/Modules/Chitin)". `[REPO]`

## Modding Implications

`[SYNTH]`

- Loose files in `Override/` win over module contents — standard KotOR modding expectation
- Module load order and `.mod` vs `.rim` priority affects which DLG/2DA/NSS wins
- Save-embedded resources can shadow base game state during load

## Related Systems

- BioWare installation/extract APIs enumerate chitin, capsules, override
- OdyPatch/TSLPatcher apply patches assuming original precedence semantics
- Runtime resource providers must mirror original lookup or mods behave incorrectly

## Repo Implications

- Resource "not found" or "wrong version" bugs often trace to precedence order, not parser bugs.
- Changes to lookup order require dual-game RE validation and mod compatibility testing.
- Diff tooling (`KotorDiff`) is the best `[REPO]`-documented precedence reference in code today; runtime path should stay consistent.
