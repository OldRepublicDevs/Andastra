---
title: "feat: Holocron port phase C — KotorCLI resource convert + GFF merge"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-071-feat-holocron-phase-b-kotorcli-ref-finder-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase C (plan 072)

## Summary

Continue PyKotor CLI parity: wire `texture-convert` and `model-convert` to existing `BioWare.Tools.ResourceConversions`, implement GFF overlay merge for `kotorcli merge`, and extend reference-finder context menus to `OdyToolUTI`.

## Requirements

- R1. `texture-convert` handles TPC↔TGA via `ResourceConversions`; invalid input exits non-zero.
- R2. `model-convert` handles MDL↔ASCII via `ResourceConversions`; `--to-ascii` flag respected.
- R3. `merge` overlays source GFF fields onto target via new `Utilities.MergeGffFiles`; writes `--output` or overwrites target.
- R4. `OdyToolUTI` tag and template ResRef fields get find-references context menus.
- R5. Unit tests for merge and resource convert executors.

## Deferred

- NCS bytecode reference scanning.
- Module Designer 3D, Lip Syncer, PLT parser.
- Deep struct-aware GFF merge conflict UI (Holocron toolset level).

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
