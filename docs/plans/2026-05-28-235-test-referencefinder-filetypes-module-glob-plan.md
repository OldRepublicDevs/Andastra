---
title: "test: referencefinder filetypes filter and tag module glob"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-234-test-referencefinder-fieldvalue-module-ncs-case-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder FileTypes filter and tag module glob (plan 235)

## Summary

Cover `ReferenceSearchOptions.FileTypes` resource-type filtering and extend module glob behavior to tag search.

## Requirements

- R1. `FindScriptReferences_FileTypesUtcOnly_FindsUtcNotNcs` — UTC-only filter skips NCS byte hits.
- R2. `FindScriptReferences_FileTypesNcsOnly_FindsNcsNotUtc` — NCS-only filter skips GFF UTC hits.
- R3. `FindTagReferences_ModuleGlob_FiltersNonMatchingModule`
- R4. ReferenceFinder filter **48** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
