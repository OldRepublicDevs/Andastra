---
title: "feat: 2DA paste over current cell (anchor paste)"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA paste over current cell (plan 472)

Plan **465** Day 8 slice per `docs/twoda_editor_ux_and_feature_completion.md` §3 (Paste over selection).

## Requirements

- R1. **Anchor overwrite** — when a data cell is focused, paste TSV block starting at `(row, col)` without inserting rows
- R2. **Range anchor** — when `_cellRangeActive`, paste from range min corner
- R3. **Legacy insert** — full-width row paste and `#` column focus keep row-insert semantics
- R4. **Tests** — anchor overwrite, range anchor, full-row insert regression
- R5. **Docs** — plan 465 Day 8, README row 472

## Implementation

| Area | Change |
|------|--------|
| `PasteSelection` | `ShouldUseAnchorPaste` / `PasteAnchorOverwrite` / `ParseClipboardGrid` |
| Anchor rules | Range active; partial-width clipboard at data column; skip when col 0 or full-row width |
| `OdyTool2DATests.cs` | Anchor overwrite, range anchor, full-row + row-label insert regressions |

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **106+** `OdyTool2DA_*` tests (103 prior + 3 anchor + 1 row-label regression).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
