---
title: "feat: Holocron port phase O — 2DA row find-references parity"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-083-feat-holocron-phase-n-2da-memory-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase O (plan 084)

## Summary

Complete Holocron `ComboBox2DA` “Find References” by merging 2DA memory refs (083), GFF field-value search for the row label, and StrRef column scans; expose field-value search on KotorCLI.

## Requirements

- R1. `ReferenceFinder.FindFieldValueReferences` with optional field-name filter and `ReferenceSearchOptions`.
- R2. `TwoDAMemoryReferenceHelper.FindAndShowTwoDARowReferences` merges memory + label + row StrRefs; `ComboBox2DA` calls it with `_this2DA`.
- R3. KotorCLI `find-field-value <value>` with `--partial`, `--case-sensitive`, `--install-dir` / `--installation`.
- R4. Tests: `tests/BioWare.Tests/ReferenceFinderFieldValueTests.cs`, `tests/KotorCLI.Tests/FindFieldValueCommandTests.cs`.

## Scope Boundaries

- ReferenceSearchOptions dialog in ComboBox2DA deferred (use installation-wide defaults).
- Reference cache persistence deferred.

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FieldValue`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindFieldValue`
