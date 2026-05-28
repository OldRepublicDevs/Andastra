---
title: "test: referencesearchhelper ncs override findandshow smoke"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-273-docs-reference-search-count-sync-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper NCS override FindAndShow smoke (plan 274)

## Summary

Add **1** test wiring `ReferenceSearchHelper.FindAndShowScriptReferences` through the UI path to BioWare NCS byte-scan hits in override (complements `ReferenceFinderTests.FindScriptReferences_OverrideNcs`).

## Requirements

- R1. Stub install with override `.ncs` containing embedded ResRef needle.
- R2. `FindAndShowScriptReferences` with `showOptionsDialog: false` completes without throw.
- R3. `ReferenceSearchHelperTests` filter passes (**36** tests).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OverrideNcs_CompletesWithoutException
```
