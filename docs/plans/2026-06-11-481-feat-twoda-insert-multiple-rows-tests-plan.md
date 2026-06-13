---
title: "feat: 2DA insert multiple rows test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA insert multiple rows test coverage (plan 481)

Plan **465** Day 17 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: **Insert N rows** — headless test coverage for `InsertMultipleRows()` core logic.

## Problem

`InsertMultipleRows()` opens a modal dialog (`ShowDialog`) to collect row count — not headless-testable. Follow Days 477–480 pattern: extract a public test hook that runs insert logic without the dialog; `InsertMultipleRows()` dialog OK handler calls the shared core with parsed count.

## Scope

In scope:

- R1. Public `TryInsertMultipleRows(int count)` (or equivalent) that runs insert logic without dialog; refactor `InsertMultipleRows()` to call shared core after dialog
- R2. Insert N>0 rows at end when no row selected (same as existing append behavior when no selection)
- R3. Insert N rows after highest selected row index (existing logic uses `selectedIndices.Max() + 1`)
- R4. `count <= 0` → no-op (no `PushState`, row count unchanged)
- R5. Three tests: append N, insert after selection, zero/negative no-op

Out of scope: dialog UX changes, column resize, find-in-column, virtualization, `.cursor/` files.

## Requirements

- R1. `TryInsertMultipleRows(int count)` public; `InsertMultipleRows()` dialog OK parses count and delegates to core
- R2. No selection → insert at `_sourceData.Count` (append)
- R3. With selection → `insertIndex = selectedIndices.Max() + 1`; N blank rows inserted at that index
- R4. `count <= 0` → return without `PushState`; `GetSourceData` count unchanged
- R5. Three `OdyTool2DA_*` tests covering R2–R4

## Implementation

| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/OdyTool2DA.axaml.cs` | Extract `TryInsertMultipleRows(int count)`; `InsertMultipleRows()` calls it after dialog |
| `tests/OdyTools.Tests/OdyTool2DATests.cs` | 3 new tests |
| `docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md` | Day 17 section + exec row |
| `docs/plans/README.md` | Row 481 |

## Test scenarios

### `OdyTool2DA_TryInsertMultipleRows_NoSelection_AppendsNRows`

1. Load test 2DA (4 rows)
2. Clear selection (default)
3. `TryInsertMultipleRows(3)`
4. Assert height 7; new rows at indices 4–6 are blank; existing rows unchanged

### `OdyTool2DA_TryInsertMultipleRows_WithSelection_InsertsAfterHighest`

1. Load test 2DA (4 rows)
2. `SetSelection(editor, 1)` (or multi-select 0,2 — use highest+1)
3. `TryInsertMultipleRows(2)`
4. Assert height 6; blank rows at indices 2–3; row 1 data (`P_HK47`) still at index 1; former row 2 shifted to index 4

### `OdyTool2DA_TryInsertMultipleRows_ZeroOrNegative_NoOp`

1. Load test 2DA
2. Record height
3. `TryInsertMultipleRows(0)` and `TryInsertMultipleRows(-1)`
4. Assert height unchanged; no undo stack growth (optional: compare source count)

## Agent recipe

1. Move lines 2404–2428 from `InsertMultipleRows()` into `TryInsertMultipleRows(int count)` with early return when `count <= 0`.
2. Dialog path: parse count into `_insertRowsResult`; on OK call `TryInsertMultipleRows(_insertRowsResult)`.
3. Use `SetSelection(editor, rowIndex)` and `BuildAndParse` / `GetSourceData` like `InsertRowBelow_InsertsAfterSelection`.
4. Multi-select highest-index case: `SetSelection(editor, 0, 2)` → insert after index 2.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **133** `OdyTool2DA_*` tests (130 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#152](https://github.com/th3w1zard1/Andastra/pull/152) | `0622bf2eb` | **133** `OdyTool2DA_*` |
