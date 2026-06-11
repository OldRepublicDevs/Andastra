---
title: "feat: 2DA Shift+Click rectangular range selection"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA Shift+Click rectangular range selection (plan 471)

Plan **465** Day 7 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 (High priority).

## Requirements

- R1. **Range anchor + Shift+Click** — fields `_cellRangeActive`, anchor/end row/col; normal click sets anchor via `SetCellRangeAnchor`; Shift+click extends rectangle via `SelectCellRange`
- R2. **Visual feedback** — `ApplyRangeHighlight()` / `ClearRangeHighlight()` with `RangeHighlightBrush` (`#FFF9C4`); non-range cells cleared to transparent
- R3. **Copy respects range** — TSV block copy when `_cellRangeActive`; full-row copy otherwise
- R4. **Status bar** — append `Range: R{min}–R{max}, C{min}–C{max}` when multi-cell range active
- R5. **Tests** — rectangle select, block copy, column clear on range
- R6. **Docs** — plan 465 Day 7, README row 471

## Implementation

| Area | Change |
|------|--------|
| `OdyTool2DA.axaml.cs` | Canonical `SelectCellRange`, `ClearCellRangeSelection`, `ApplyRangeHighlight`; Shift+click wired through `SelectCellRange` |
| `CopySelection` | Range-aware TSV block when `IsCellRangeActive` |
| `UpdateStatusBar` | Range coords when `cellCount > 1` |
| `OdyTool2DATests.cs` | `OdyTool2DA_ShiftClickRange_SelectsRectangle`, `OdyTool2DA_CopySelection_WithActiveRange_CopiesBlockOnly`, `OdyTool2DA_SelectCellRange_ClearsColumnHighlight` |

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_
```

Target: **103+** `OdyTool2DA_*` tests (100 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#142](https://github.com/th3w1zard1/Andastra/pull/142) | `b8131328b` | **103** `OdyTool2DA_*` |
