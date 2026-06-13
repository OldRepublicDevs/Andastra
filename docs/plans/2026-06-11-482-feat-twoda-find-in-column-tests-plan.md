---
title: "feat: 2DA find in column test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA find in column test coverage (plan 482)

Plan **465** Day 18 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: **Find in column** — column-scoped Find Next with headless tests.

## Problem

`FindNextMatch()` scans all columns left-to-right, top-to-bottom. UX spec calls for search in current column or all columns. Tests need a public hook to scope find to one grid column without dialog changes.

## Scope

In scope:

- R1. Extend `ConfigureFind(string text, bool matchCase = false, int columnIndex = -1)` where `-1` = all columns (preserve default)
- R2. When `columnIndex >= 1`, search only that column index across rows (top-to-bottom)
- R3. Column-scoped find advances row-by-row within the target column; returns false when no more hits
- R4. `columnIndex == -1` behavior unchanged (existing Find Next tests remain valid)
- R5. Four `OdyTool2DA_*` headless tests for column scope + default regression

Out of scope: Find dialog UI column picker, regex find, replace-in-column, `.cursor/` files.

## Requirements

- R1. `_findColumnIndex` field (default `-1`); reset on `ConfigureFind` / `ConfigureReplace`
- R2. `FindNextMatch()` branches: all-columns loop vs single-column row loop
- R3. Hit navigation via existing `SelectAndScrollToCell`
- R4. Four tests: single-column first hit, column-only advance (no cross-column jump), multi-row same column, all-columns default preserved

## Implementation

| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/OdyTool2DA.axaml.cs` | `_findColumnIndex`; extend `ConfigureFind`; column branch in `FindNextMatch` |
| `tests/OdyTools.Tests/OdyTool2DATests.cs` | 4 new tests |
| `docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md` | Day 18 section + exec row |
| `docs/plans/README.md` | Row 482 |

## Test scenarios

### `OdyTool2DA_TryFindNextMatch_ColumnScoped_FindsInTargetColumn`

1. Load test 2DA (4 rows)
2. `ConfigureFind("PMBTest", columnIndex: 4)` (race column)
3. First hit row 0 col 4; second `TryFindNextMatch()` false

### `OdyTool2DA_TryFindNextMatch_ColumnScoped_SkipsOtherColumns`

1. `ConfigureFind("PMBTest", columnIndex: 2)` (name column)
2. Hit row 0 col 2 only; second find false

### `OdyTool2DA_TryFindNextMatch_ColumnScoped_AdvancesDownRows`

1. `ConfigureFind("P", columnIndex: 2, matchCase: false)` — matches PMBTest (row 0) and P_HK47 (row 1)
2. First hit row 0 col 2; second hit row 1 col 2; third false

### `OdyTool2DA_TryFindNextMatch_AllColumns_DefaultUnchanged`

1. `ConfigureFind("PMBTest")` (implicit `-1`)
2. First hit col 2; second hit col 4 on same row (regression guard)

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **137** `OdyTool2DA_*` tests (133 prior + 4 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#154](https://github.com/th3w1zard1/Andastra/pull/154) | `bdb030edc` | **137** `OdyTool2DA_*` |
