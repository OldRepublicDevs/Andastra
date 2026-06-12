---
title: "feat: 2DA in-cell editing test coverage"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA in-cell editing test coverage (plan 475)

Plan **465** Day 11 slice — testable in-cell edit API and headless coverage for F2 / edit-mode guards.

## Requirements

- R1. `BeginCellEdit()` public wrapper for DataGrid `BeginEdit()` (F2 / double-click path)
- R2. `IsCellEditing()` public wrapper for edit-mode detection
- R3. F2 key handler calls `BeginCellEdit()`
- R4. `TryHandleSelectionShortcut` skips when cell editing
- R5. Headless tests with graceful fallback when DataGrid edit TextBox is not observable

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **115** `OdyTool2DA_*` tests (112 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
