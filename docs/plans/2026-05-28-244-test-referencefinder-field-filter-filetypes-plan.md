---
title: "test: referencefinder field filter wrong name and tag filetypes"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-243-test-referencefinder-gff-partial-nomatch-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-name filter and tag FileTypes (plan 244)

## Summary

Prove field-name filtering excludes non-listed GFF fields at byte and installation scope; prove `FileTypes` restricts tag search to UTC over UTP. Brings ReferenceFinder test count from **74 → 77**.

## Requirements

- R1. `FindFieldValueInGffBytes_WrongFieldName_SkipsNonListedFields`
- R2. `FindFieldValueReferences_WrongFieldName_SkipsNonListedFields`
- R3. `FindTagReferences_FileTypesUtcOnly_FindsUtcNotUtp`
- R4. ReferenceFinder filter count is **77** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Chitin-only scope (deferred).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
