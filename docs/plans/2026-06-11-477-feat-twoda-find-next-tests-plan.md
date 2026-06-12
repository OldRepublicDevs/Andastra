---
title: "feat: 2DA find next match test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA find next match test coverage (plan 477)

Plan **465** Day 13 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: Find/Replace — test F3 / Find Next core loop.

## Requirements

- R1. `ConfigureFind()` / `TryFindNextMatch()` public test hooks
- R2. F3 delegates to `TryFindNextMatch()`
- R3. Case-sensitive and case-insensitive search
- R4. Sequential find advances; empty query returns false

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **122** `OdyTool2DA_*` tests (118 prior + 4 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#148](https://github.com/th3w1zard1/Andastra/pull/148) | `dcd707e45` | **122** `OdyTool2DA_*` |
