---
title: "feat: Holocron port phase N — 2DA memory find-references"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-082-feat-holocron-phase-m-strref-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase N (plan 083)

## Summary

Expose BioWare `TwoDAMemoryReferenceCache` as installation-wide find-references for a 2DA row (GFF fields that index into that 2DA), wired into OdyTools `ComboBox2DA` and KotorCLI.

## Requirements

- R1. `ReferenceCacheHelpers.Find2DAMemoryReferences` + `ConvertTwoDAMemoryToReferenceSearchResults` using existing cache scan.
- R2. `TwoDAMemoryReferenceHelper` in OdyTools; `ComboBox2DA` context menu "Find References..." calls it for the selected row.
- R3. KotorCLI `find-2da-ref <twoda> <row>` with `--install-dir` / `--installation`.
- R4. Tests: `tests/BioWare.Tests/ReferenceCacheTwoDAMemoryTests.cs`, `tests/KotorCLI.Tests/Find2DARefCommandTests.cs`.

## Scope Boundaries

- Row label / StrRef column sweep (Holocron `find_field_value_references` path) deferred.
- Reference cache persistence deferred.

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter TwoDAMemory`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter Find2DARef`
