---
title: "test: fileresultsdialog flat filepath display"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-254-test-fileresultsdialog-reference-edges-plan.md
branch: feat/holocron-port-phase-b
---

# test: FileResultsDialog flat filepath display (plan 258)

## Summary

Extend `FileResultsDialogReferenceSearchTests` with **2** display-format edge cases: flat filepaths (no parent folder) and empty-string field paths.

## Requirements

- R1. Results whose resource path has no directory show bare filename (no `parent/` prefix).
- R2. Empty-string `FieldPath` omits the `::` suffix (same as null).
- R3. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FileResultsDialogReferenceSearchTests` passes (**8** total).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FileResultsDialogReferenceSearchTests
```
