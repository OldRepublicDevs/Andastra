---
title: "feat: 2DA column/row header selection tests"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA column/row header selection tests (plan 476)

Plan **465** Day 12 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 High: column/row header click selects entire column/row.

## Requirements

- R1. `SelectColumnByIndex()` public — column header click path (all rows + column mode + highlight)
- R2. Column select clears active cell range
- R3. Row select (`SelectRowByIndex`) clears column selection mode (# column click path)
- R4. Tests for column select, range clear, row clears column mode

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **118** `OdyTool2DA_*` tests (115 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
