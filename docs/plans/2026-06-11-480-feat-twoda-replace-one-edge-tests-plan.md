---
title: "feat: 2DA replace one edge-case test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA replace one edge-case test coverage (plan 480)

Plan **465** Day 16 slice per `docs/twoda_editor_ux_and_feature_completion.md` **§6** (Replace one); extends plan **479** with invalid/stale cursor coverage and Day 15 review hygiene.

## Scope

In scope:

- R1. Test `TryReplaceOne` with **invalid cursor** (`ConfigureReplace` only, no `TryFindNextMatch`) → no-op
- R2. Test **stale cursor** (find hit, mutate cell to remove match, `TryReplaceOne` → data unchanged, find advances)
- R3. Add `GetLastFindRowIndex` / `GetLastFindColumnIndex` assertions to existing `TryReplaceOne_ReplacesCurrentFindMatchOnly` (code review follow-up from PR **#150**)
- R4. Plan **479** doc hygiene: Scope section, deferred UX (§6), stale-match R2 note, agent recipe (reviewer follow-ups that did not land in **#150**)
- R5. Optional: `ConfigureReplace` XML summary noting cursor reset (if not already on `master`)

Out of scope: dialog UX, find-in-column, regex, `FindReplaceWidget` extraction, new production APIs beyond existing hooks.

## Requirements

- R1. `ConfigureReplace("PMBTest", "Replaced")` without prior find → `TryReplaceOne()` leaves all cells unchanged
- R2. After successful find, mutate matched cell so find text no longer present → `TryReplaceOne()` leaves data unchanged and advances cursor via internal `FindNextMatch()`
- R3. `TryReplaceOne_ReplacesCurrentFindMatchOnly` asserts cursor at row 0 col 2 after find, before replace
- R4. Plan **479** updated with Scope, deferred §6 UX, stale-match behavior note, agent test recipe
- R5. `ConfigureReplace` doc comment mentions find cursor reset (mirrors `ConfigureFind`)

## Implementation

| File | Change |
|------|--------|
| `tests/OdyTools.Tests/OdyTool2DATests.cs` | 2 new tests + cursor assertions in existing replace-one test |
| `src/Tools/OdyTools/Editors/OdyTool2DA.axaml.cs` | Optional XML doc on `ConfigureReplace` |
| `docs/plans/2026-06-11-479-feat-twoda-replace-one-tests-plan.md` | Hygiene (R4) |
| `docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md` | Day 16 section |
| `docs/plans/README.md` | Row 480 |

## Test scenarios

### `OdyTool2DA_TryReplaceOne_InvalidCursor_LeavesDataUnchanged`

1. Load test 2DA
2. `ConfigureReplace("PMBTest", "Replaced")` — no `TryFindNextMatch`
3. `TryReplaceOne()`
4. Assert `GetLastFindRowIndex()` / `GetLastFindColumnIndex()` are `-1`
5. Assert cell values unchanged (BuildAndParse or source snapshot)

### `OdyTool2DA_TryReplaceOne_StaleCursor_AdvancesFindWithoutMutating`

1. Load test 2DA
2. `ConfigureReplace("PMBTest", "Replaced")`
3. `TryFindNextMatch()` → true; record cursor (0, 2)
4. Mutate `source[0][2]` to value without "PMBTest"
5. `TryReplaceOne()`
6. Assert mutated cell unchanged
7. Assert cursor advanced to next match (0, 4)

### `OdyTool2DA_TryReplaceOne_ReplacesCurrentFindMatchOnly` (extend)

After `TryFindNextMatch()`, assert `GetLastFindRowIndex() == 0` and `GetLastFindColumnIndex() == 2`.

## Agent recipe (review follow-ups from PR #150 not landed there)

1. Always call `TryFindNextMatch()` after `ConfigureReplace()` before replace-one tests — `ConfigureReplace` resets `_lastFindRow` / `_lastFindCol` to `-1`.
2. Assert find cursor via `GetLastFindRowIndex()` / `GetLastFindColumnIndex()`, not grid visual focus (headless-safe).
3. Stale match: when cell no longer contains find text, `ReplaceOne()` calls `FindNextMatch()` and returns without `PushState()` — data unchanged, cursor moves.

## Deferred UX (§6 — not this slice)

Per `docs/twoda_editor_ux_and_feature_completion.md` §6:

- Find/Replace dialog UX (Ctrl+F / Ctrl+H)
- Search scope: current column, all columns, row labels
- Whole-cell match, highlight all matches
- Replace in selection vs whole sheet
- Regex replace (advanced)
- Row filter extensions

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **130** `OdyTool2DA_*` tests (128 prior + 2 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#151](https://github.com/th3w1zard1/Andastra/pull/151) | `258752666` | **130** `OdyTool2DA_*` |
