---
title: "test: referencefinder scope partial and field value"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-225-test-referencefinder-guard-tests-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder scope, partial match, field value (plan 226)

## Summary

Extend OdyTools `ReferenceFinderTests` with installation search behaviors mirrored from plan 068 scenarios and BioWare field-value tests.

## Requirements

- R1. `FindScriptReferences_NoOverride_SkipsOverrideUtc` — override UTC skipped when `SearchOverride = false`.
- R2. `FindTagReferences_PartialMatch_OverrideUtc` — `PartialMatch` finds substring in Tag field.
- R3. `FindFieldValueReferences_OverrideUtc_FindsTag` — field-value search on Tag (OdyTools parity with BioWare.Tests).
- R4. ReferenceFinder filter **22** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
