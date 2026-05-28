---
title: "feat: enable NCS CONSTI StrRef scanning in reference cache"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-082-feat-holocron-phase-m-strref-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: NCS StrRef reference cache (plan 086)

## Summary

Enable `StrRefReferenceCache` NCS scanning by extracting CONSTI instructions from compiled scripts, shared with installation StrRef search.

## Requirements

- R1. `NcsConstiScanner` in BioWare with `ExtractConstiInstructions` and `ExtractConstiOffsetsForValue`.
- R2. `StrRefReferenceCache.ScanNCS` wired from `ScanResource`; slow-path `FindStrRefReferences` scans NCS for target StrRef.
- R3. Tests in `tests/BioWare.Tests/NcsConstiScannerTests.cs` using `NCSAuto.CompileNss` fixture.

## Scope Boundaries

- KotorDiff `ReferenceAnalyzers` refactor to call BioWare scanner deferred.
- Distinguishing StrRef vs 2DA-memory CONSTI **partially landed** (2026-05-28, plan **292**): `StrRefCandidateMinimum` threshold skips low CONSTI in cache scans; slow path still matches any CONSTI. Opcode-context disambiguation remains deferred.

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter "NcsConsti|NcsStrRef"`
