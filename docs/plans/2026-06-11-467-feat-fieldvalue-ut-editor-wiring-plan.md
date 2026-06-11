---
title: "feat: field-value find-refs wiring on UT editors (plans 412-416)"
type: feat
status: completed
date: 2026-06-11
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
branch: feat/plan-465-day3-fieldvalue-arc
---

# feat: field-value find-refs wiring on UT editors (plan 467 / 465 Day 3)

## Summary

Rebases open PR stack **#81**–**#85** onto post-D2 `master` without merging stale branches (those branches regress LIP/KotorDiff). Adds `AppendFieldValueFindReferencesMenuItem` and wires Tag/TemplateResRef context menus on UTC, UTP, UTD, UTM, UTS, UTW, UTI, UTE, UTT. GFF editor already uses `AttachFieldValueFindReferencesMenu`.

## Requirements

- R1. `AppendFieldValueFindReferencesMenuItem` composes with existing ReferenceSearchHelper menus.
- R2. **10** `FieldValueReferenceHelperTests` pass.
- R3. Close superseded **#78**, **#81**–**#85**.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```

## Suggested next slices

- Plan **465** Day 4+: KotorDiff installation ref search stub, Holocron deferred items.
