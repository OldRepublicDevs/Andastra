---
title: "feat: 2DA column width persist test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA column width persist test coverage (plan 483)

Plan **465** Day 19 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: **column resize persist** — in-session width memory across grid rebuilds.

## Problem

`RebuildGridColumns()` always applies `DefaultColumnWidth` / `RowLabelColumnWidth`. User-resized or auto-fit widths are lost on hide/show column, text-wrap toggle, and similar rebuild paths.

## Scope

In scope:

- R1. `_persistedColumnWidths` dictionary keyed by column header name
- R2. `TrySetColumnWidth(int gridColumnIndex, double width)` public test hook; persists and applies
- R3. `GetColumnPixelWidth(int gridColumnIndex)` public read hook
- R4. `RebuildGridColumns()` restores persisted widths; `RenameColumnByIndex` migrates key
- R5. `AutoFitAllColumns()` persists fitted widths
- R6. Three to four headless tests

Out of scope: cross-session settings file, dialog UX, `.cursor/` files.

## Requirements

- R1. Width clamped to `[MinColumnWidth, 500]`
- R2. Default width when header has no persisted entry
- R3. Rebuild after `TrySetColumnWidth` restores same pixel width
- R4. Rename migrates persisted width to new header name

## Implementation

| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/OdyTool2DA.axaml.cs` | Persist dict, hooks, rebuild/autofit/rename integration |
| `tests/OdyTools.Tests/OdyTool2DATests.cs` | 3–4 new tests |
| `docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md` | Day 19 section |
| `docs/plans/README.md` | Row 483 |

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **140–141** `OdyTool2DA_*` tests (137 prior + 3–4 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| TBD | TBD | TBD |
