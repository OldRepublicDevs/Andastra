---
title: "test: referencefinder field value case sensitivity and partial match"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-241-test-referencefinder-installation-case-sensitivity-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value case and partial match (plan 242)

## Summary

Close the field-value search gap: byte-level case/partial coverage and installation-level case sensitivity. Brings ReferenceFinder test count from **68 → 71**.

## Requirements

- R1. `FindFieldValueInGffBytes_CaseSensitive_RequiresExactCase`
- R2. `FindFieldValueInGffBytes_PartialMatch_FindsSubstring`
- R3. `FindFieldValueReferences_CaseSensitive_OverrideUtc`
- R4. ReferenceFinder filter count is **71** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Production changes; chitin-only scope (deferred from plan 241).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
