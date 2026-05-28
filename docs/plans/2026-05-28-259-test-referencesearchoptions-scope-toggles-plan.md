---
title: "test: referencesearchoptionsdialog scope toggles"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-101-test-referencesearchoptionsdialog-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchOptionsDialog scope toggles (plan 259)

## Summary

Extend `ReferenceSearchOptionsDialogTests` with scope and matching toggle round-trip coverage. Adds **2** Avalonia headless tests.

## Requirements

- R1. `ToSearchOptions` round-trips override/modules/chitin scope toggles from `SetDefaults`.
- R2. `ToSearchOptions` round-trips case-sensitive and partial-match toggles from `SetDefaults`.
- R3. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchOptionsDialog` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchOptionsDialog
```
