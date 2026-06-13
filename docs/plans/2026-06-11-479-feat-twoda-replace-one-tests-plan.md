---
title: "feat: 2DA replace one test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA replace one test coverage (plan 479)

Plan **465** Day 15 slice per `docs/twoda_editor_ux_and_feature_completion.md` **§6** (Replace one); tracked under §14 Medium Find/Replace (after Find Next / Replace All).

## Scope

In scope:

- R1. `TryReplaceOne()` public wrapper for `ReplaceOne()`
- R2. Replace at valid find cursor; case flag; empty find no-op
- R3. Other matching cells unchanged (single-cell replace)

Out of scope (deferred §6 UX — see plan **480**): dialog UX, find-in-column, regex, `FindReplaceWidget` extraction.

## Requirements

- R1. `TryReplaceOne()` public wrapper for `ReplaceOne()`
- R1.5. Depends on `ConfigureReplace()` / `ConfigureFind()` (plans **478** / **477**) and a successful prior `TryFindNextMatch()`; `ConfigureReplace()` resets the find cursor — re-find before replace in tests
- R2. Given a valid find cursor, replaces the first `_findText` occurrence in that cell only; invalid cursor or empty find → no-op
- R2.5. **Stale match:** if the cell at the find cursor no longer contains find text, `ReplaceOne()` calls `FindNextMatch()` and returns without mutating data (plan **480** covers test)
- R3. Other matching cells remain unchanged (single-cell replace)
- R4. Case-sensitive and case-insensitive behavior
- R5. Empty find text is a no-op

## Agent recipe (test authoring)

1. Always call `TryFindNextMatch()` after `ConfigureReplace()` before replace-one tests — `ConfigureReplace` resets `_lastFindRow` / `_lastFindCol` to `-1`.
2. Assert find cursor via `GetLastFindRowIndex()` / `GetLastFindColumnIndex()`, not grid visual focus (headless-safe).
3. Stale match: mutate cell after find, then `TryReplaceOne()` — data unchanged, cursor advances.

## Deferred UX (§6)

Per `docs/twoda_editor_ux_and_feature_completion.md` §6: Find/Replace dialog, search scope, whole-cell match, highlight all, replace in selection, regex — not landed in PR **#150**; follow plan **480**+ for incremental test coverage before dialog work.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **128** `OdyTool2DA_*` tests (125 prior + 3 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#150](https://github.com/th3w1zard1/Andastra/pull/150) | `f294aab9b` | **128** `OdyTool2DA_*` |
