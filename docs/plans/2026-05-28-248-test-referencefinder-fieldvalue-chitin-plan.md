---
title: "test: referencefinder field value chitin scope completion"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-247-test-referencefinder-chitin-template-conversation-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value chitin scope completion (plan 248)

## Summary

Complete chitin scope coverage for field-value search and byte-level null field-name filter. Brings ReferenceFinder test count from **86 → 89**.

## Requirements

- R1. `FindFieldValueReferences_ChitinOnly_ReturnsFieldPath`
- R2. `FindFieldValueReferences_NoChitin_SkipsChitinResource`
- R3. `FindFieldValueInGffBytes_NullFieldNames_SearchesAllFields`
- R4. ReferenceFinder filter count is **89** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests reusing `WriteChitinWithUtc` and existing field-value patterns.
- **Out:** Production changes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
