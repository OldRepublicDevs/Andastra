---
title: "feat: 2DA Shift+Click rectangular range selection"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA Shift+Click rectangular range selection (plan 471)

Plan **465** Day 7 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 (High priority).

## Requirements

- R1. **Range anchor + Shift+Click** — fields `_cellRangeActive`, anchor/end row/col; normal click sets anchor; Shift+click extends rectangle
- R2. **Visual feedback** — `ApplyRangeHighlight()` / `ClearRangeHighlight()` with distinct brush
- R3. **Copy respects range** — TSV block copy when `_cellRangeActive`; row copy otherwise
- R4. **Status bar** — append range coords when >1 cell
- R5. **Tests** — rectangle select, block copy, column clear on range
- R6. **Docs** — plan 465 Day 7, README row 471

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_
```

Target: **103+** `OdyTool2DA_*` tests (100 current + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
