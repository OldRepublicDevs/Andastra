---
title: "feat: 2DA replace one test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA replace one test coverage (plan 479)

Plan **465** Day 15 slice per `docs/twoda_editor_ux_and_feature_completion.md` **§6** (Replace one); tracked under §14 Medium Find/Replace (after Find Next / Replace All).

## Requirements

- R1. `TryReplaceOne()` public wrapper for `ReplaceOne()`
- R1.5. Depends on `ConfigureReplace()` / `ConfigureFind()` (plans **478** / **477**) and a successful prior `TryFindNextMatch()`; `ConfigureReplace()` resets the find cursor — re-find before replace in tests
- R2. Given a valid find cursor, replaces the first `_findText` occurrence in that cell only; invalid cursor or empty find → no-op
- R3. Other matching cells remain unchanged (single-cell replace)
- R4. Case-sensitive and case-insensitive behavior
- R5. Empty find text is a no-op

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **128** `OdyTool2DA_*` tests (125 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#150](https://github.com/th3w1zard1/Andastra/pull/150) | `f294aab9b` | **128** `OdyTool2DA_*` |
