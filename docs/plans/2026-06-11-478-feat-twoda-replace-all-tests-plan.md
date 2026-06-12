---
title: "feat: 2DA replace all test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA replace all test coverage (plan 478)

Plan **465** Day 14 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: Find/Replace — Replace All core loop.

## Requirements

- R1. `ConfigureReplace()` sets find/replace text and match-case flag
- R2. `TryReplaceAll()` public wrapper for `ReplaceAll()`
- R3. Replaces all occurrences across grid cells
- R4. Case-sensitive and case-insensitive behavior
- R5. Empty find text is a no-op

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **125** `OdyTool2DA_*` tests (122 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#149](https://github.com/th3w1zard1/Andastra/pull/149) | `88dc572e4` | **125** `OdyTool2DA_*` |
