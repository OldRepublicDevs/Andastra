---
title: "feat: 2DA Fill Down within active cell range"
status: active
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA Fill Down within active cell range (plan 474)

Plan **465** Day 10 slice — extend Fill Down for rectangular range selection (plan **471**).

## Requirements

- R1. When `_cellRangeActive`, copy each column's top-row cell down through the range
- R2. Legacy Fill Down unchanged for row/column selection without active range
- R3. Single-row range is a no-op
- R4. Tests for multi-column fill, single-column fill, single-cell no-op

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **112** `OdyTool2DA_*` tests (109 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| _pending_ | _pending_ | _pending_ |
