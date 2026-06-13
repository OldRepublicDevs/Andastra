---
title: "feat: 2DA regex find test coverage"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA regex find test coverage (plan 484)

Plan **465** Day 20 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14 Medium: **regex option** for Find Next — headless tests without dialog changes.

## Problem

Find Next uses literal `IndexOf` only. UX spec lists regex find; tests need `ConfigureFind(..., useRegex: true)` without dialog UI.

## Scope

In scope:

- R1. `_findUseRegex` field; extend `ConfigureFind(..., bool useRegex = false)`
- R2. `CellMatchesFind` helper: regex via `Regex.IsMatch` with case flag; invalid pattern → no match (no throw)
- R3. Find Next / find-in-column paths use helper; literal default unchanged
- R4. `ConfigureReplace` resets `_findUseRegex` to false
- R5. Four headless tests

Out of scope: regex replace-one/replace-all, dialog checkbox, `.cursor/` files.

## Requirements

- R1. `ConfigureFind("P\\w+Test", useRegex: true)` finds `PMBTest`
- R2. Invalid pattern `[unclosed` → `TryFindNextMatch()` false
- R3. Regex + match case respected
- R4. Literal find without `useRegex` unchanged

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_ -m:1
```

Target: **145** `OdyTool2DA_*` tests (141 prior + 4 new).

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#156](https://github.com/th3w1zard1/Andastra/pull/156) | `7196863e9` | **145** `OdyTool2DA_*` |
