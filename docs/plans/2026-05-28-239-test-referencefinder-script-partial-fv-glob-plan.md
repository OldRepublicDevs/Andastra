---
title: "test: referencefinder script partial field value glob ncs empty"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-238-test-referencefinder-nomatch-partial-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder script partial, field-value module glob, NCS empty needle (plan 239)

## Summary

Close remaining partial-match and module-glob gaps for script and field-value search; add NCS bytes empty-needle guard.

## Requirements

- R1. `FindScriptReferences_PartialMatch_OverrideUtc`
- R2. `FindFieldValueReferences_ModuleGlob_FiltersNonMatchingModule`
- R3. `FindScriptResRefInNcsBytes_EmptyNeedleReturnsEmpty`
- R4. ReferenceFinder filter **62** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
