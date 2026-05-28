---
title: "test: referencefinder chitin scope harness and skip tests"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-245-test-referencefinder-filetypes-null-fields-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder chitin scope harness (plan 246)

## Summary

Add reusable chitin KEY/BIF fixture helper and three installation-scoped tests proving `SearchChitin` includes or excludes chitin resources. Brings ReferenceFinder test count from **80 → 83**.

## Requirements

- R1. `WriteChitinWithUtc` helper writes `chitin.key` + `data.bif` with dismantled UTC payload.
- R2. `FindTagReferences_ChitinOnly_ReturnsFieldPath`
- R3. `FindScriptReferences_ChitinOnly_ReturnsFieldPath`
- R4. `FindTagReferences_NoChitin_SkipsChitinResource`
- R5. ReferenceFinder filter count is **83** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Helper + three tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Production changes to `ReferenceFinder.cs`.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
