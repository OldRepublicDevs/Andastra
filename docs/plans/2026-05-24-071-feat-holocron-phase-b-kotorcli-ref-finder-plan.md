---
title: "feat: Holocron port phase B — KotorCLI scripts/diff + reference finder phase 2"
type: feat
status: complete
completed: 2026-05-24
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase B (plan 071)

## Summary

After plan 063/069/070 landed on master, continue PyKotor/Holocron merge with KotorCLI script utility wiring, BioWare `Utilities.DiffFiles` integration, reference finder tag/template ResRef search, and a minimal search-options dialog for OdyTools editors.

## Requirements

- R1. KotorCLI `disassemble` delegates to `BioWare.Tools.Scripts.DisassembleNcs`; failures exit non-zero.
- R2. KotorCLI `assemble` delegates to `BioWare.Resource.Formats.NCS.NCSAuto.CompileNss`; failures exit non-zero.
- R3. KotorCLI `diff` delegates to `BioWare.Tools.Utilities.DiffFiles`; identical files exit 0, differences exit 1.
- R4. `ReferenceFinder.FindTagReferences` and `FindTemplateResRefReferences` with case/partial options.
- R5. `ReferenceSearchOptionsDialog` for scope toggles; UTC/UTD/UTP/UTT tag and template ResRef context menus.
- R6. Unit tests for new BioWare and KotorCLI surfaces.

## Deferred

- KotorCLI `merge` (no BioWare merge helper yet).
- NCS bytecode reference scanning (`ReferenceCache`).
- Module Designer 3D, Lip Syncer, PLT parser, texture/model convert STUBs.

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
