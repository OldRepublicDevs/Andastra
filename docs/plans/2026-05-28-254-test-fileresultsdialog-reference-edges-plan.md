---
title: "test: fileresultsdialog reference search edges"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-251-test-fileresultsdialog-reference-search-plan.md
branch: feat/holocron-port-phase-b
---

# test: FileResultsDialog reference search edges (plan 254)

## Summary

Extend plan 251 FileResultsDialog coverage with **3** edge-case tests for empty/null input and field-path-less results.

## Requirements

- R1. `FromReferenceSearch` with empty list leaves result list empty.
- R2. `FromReferenceSearch` with null enumerable leaves result list empty.
- R3. Results without `FieldPath` display base `parent/filename` only (no `::` suffix).
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FileResultsDialogReferenceSearch` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FileResultsDialogReferenceSearch
```
