---
title: "test: fileresultsdialog reference search population"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-250-test-referencefinder-displaylabel-edges-plan.md
branch: feat/holocron-port-phase-b
---

# test: FileResultsDialog reference search population (plan 251)

## Summary

Pivot from ReferenceFinder byte/install tests (95 tests, substantially complete) to OdyTools UI: verify `FileResultsDialog.FromReferenceSearch` populates list items with field-path suffixes. Adds **3** Avalonia headless tests.

## Requirements

- R1. `FromReferenceSearch_PopulatesFieldPathSuffix`
- R2. `FromReferenceSearch_SkipsNullResourceResults`
- R3. `FromReferenceSearch_SortsDisplayTextAlphabetically`
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FileResultsDialogReferenceSearch` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FileResultsDialogReferenceSearch
```
