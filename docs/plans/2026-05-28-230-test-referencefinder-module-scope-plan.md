---
title: "test: referencefinder module scope and glob filter"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-227-test-referencefinder-nooverride-scope-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder module scope and glob filter (plan 230)

## Summary

Land deferred plan 227 module-scope coverage in OdyTools: installation search finds script refs inside `.mod` capsules, skips modules when disabled, and respects `ModuleGlobFilters`.

## Requirements

- R1. `FindScriptReferences_ModuleMod_ReturnsFieldPath` — UTC in `modules/*.mod` found when modules enabled, override/chitin off.
- R2. `FindScriptReferences_NoModules_SkipsModuleUtc` — same fixture empty when `SearchModules = false`.
- R3. `FindScriptReferences_ModuleGlob_FiltersNonMatchingModule` — glob limits scan to matching module filename.
- R4. ReferenceFinder filter **32** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
