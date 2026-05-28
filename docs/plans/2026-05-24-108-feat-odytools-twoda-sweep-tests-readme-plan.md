---
title: "feat: odytools twoda row sweep tests and kotorcli readme accuracy"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-083-feat-holocron-phase-n-2da-memory-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: OdyTools 2DA row sweep tests + KotorCLI README accuracy (plan 108)

## Summary

Close deferred Holocron port gaps: test OdyTools `CollectTwoDARowReferences` label and StrRef sweeps when `twoDA` is supplied, and align KotorCLI README with wired vs stub command reality.

## Requirements

- R1. `CollectTwoDARowReferences_WithTwoDA_FindsLabelFieldValueRef` in `tests/OdyTools.Tests/TwoDAMemoryReferenceHelperTests.cs`.
- R2. `CollectTwoDARowReferences_WithTwoDA_FindsRowStrRefColumnRef` in the same file.
- R3. README Status / Known Issues / Next Steps no longer claim all commands are stubs.
- R4. README command list marks wired surfaces consistently (reference search, disassemble/assemble, utilities already wired).

## Scope Boundaries

- No new CLI flags or BioWare API changes.
- No full README audit of every stub command implementation depth.

## Verification

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReferenceHelper`
