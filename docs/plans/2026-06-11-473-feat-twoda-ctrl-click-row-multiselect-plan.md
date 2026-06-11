---
title: "feat: 2DA Ctrl+Click row multi-select"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA Ctrl+Click row multi-select (plan 473)

Plan **465** Day 9 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 High: Ctrl+Click adds disjoint rows to selection.

## Requirements

- R1. **Ctrl+Click `#` column** — toggle row in `SelectedItems` without clearing others
- R2. **Normal `#` click** — single-row `SelectRowByIndex` unchanged
- R3. **Clear column/range** — `ToggleRowSelection` clears column and cell-range modes
- R4. **Keyboard shortcuts dialog** — document Ctrl+Click (#) and Shift+Click
- R5. **Tests** — toggle multi-select, clears range/column, single-select regression

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **109** `OdyTool2DA_*` tests (106 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
