---
title: "feat: 2DA NavigateToCell test coverage"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA NavigateToCell test coverage (plan 485)

Plan **465** Day 21 slice per `docs/twoda_editor_ux_and_feature_completion.md`: expose **NavigateToCell** for headless tests of keyboard navigation hooks (Ctrl+Home / Ctrl+End use this internally).

## Problem

`NavigateToCell(int rowIdx, int colIdx)` is private but documented; headless tests cannot assert first/last cell jumps or clamping without reflection. Go To Column numeric index resolution (`ResolveGoToColumnGridIndex`) lacks a dedicated numeric-index test.

## Scope

In scope:

- N1. Make `NavigateToCell(int rowIdx, int colIdx)` **public** (behavior unchanged: clamp row/col, update selection, scroll, status)
- N2. Four headless tests in `OdyTool2DATests.cs`:
  - `NavigateToCell(0, 1)` selects first data cell (row 0, grid col 1)
  - `NavigateToCell(lastRow, lastCol)` selects last data cell
  - Out-of-range indices clamp safely (negative row/col → 0; oversized → last)
  - `ResolveGoToColumnGridIndex("2")` returns grid index 3 (0-based data column 2 → "value")

Out of scope: `TryHandleWindowShortcut` (not needed — direct `NavigateToCell` covers behavior), dialog UI, engine RE.

## Requirements

- R1. Public `NavigateToCell` preserves clamping: `rowIdx ∈ [0, Count-1]`, `colIdx ∈ [0, GetEffectiveColumnCount()-1]`
- R2. After navigation, `SelectedItem` matches target row and `CurrentColumn` index matches target col
- R3. `ResolveGoToColumnGridIndex("2")` on default 4-column fixture returns **3** (grid index for header "value")
- R4. Named-column Go To test from Day 6 remains passing

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **149** `OdyTool2DA_*` tests (145 prior + 4 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| TBD | TBD | TBD |
