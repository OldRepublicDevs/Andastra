---
title: "test: referencefinder field value module and ncs case"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-233-test-referencefinder-no-modules-skip-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value module scope and NCS case sensitivity (plan 234)

## Summary

Extend module-scope matrix to field-value search and add NCS bytes case-sensitivity coverage. Tighten script empty-needle guard to include whitespace.

## Requirements

- R1. `FindFieldValueReferences_ModuleMod_FindsTag`
- R2. `FindFieldValueReferences_NoModules_SkipsModuleUtc`
- R3. `FindScriptResRefInNcsBytes_CaseSensitive_RequiresExactCase`
- R4. `FindScriptReferences_EmptyNeedleReturnsEmpty` also asserts whitespace needle.
- R5. ReferenceFinder filter **45** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
