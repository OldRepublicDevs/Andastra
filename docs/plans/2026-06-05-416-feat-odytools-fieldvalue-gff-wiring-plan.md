---
title: "feat: odyTools FieldValueReferenceHelper GFF wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-415-feat-odytools-fieldvalue-utc-wiring-plan.md
branch: feat/plan-416-odytools-fieldvalue-gff-wiring
---

# feat: OdyTools FieldValueReferenceHelper GFF wiring (plan 416)

## Summary

Plans **412**–**415** / PRs **#81**–**#84** complete UT* template editor field-value wiring. Open PR **#72** wires GFF on a separate stack. This slice stacks on **#84** and adds `AttachFieldValueFindReferencesMenu` to `OdyToolGFF` string/ResRef value editors with field-name filter from the selected tree node — completing the FieldValueReferenceHelper arc on one stack.

## Requirements

- R1. `OdyToolGFF` `_textEdit` and `_lineEdit` get field-value reference context menus scoped to the selected GFF field label via `GetSelectedFieldNameForReferenceSearch`.
- R2. Existing **10** `FieldValueReferenceHelperTests` remain green.
- R3. Build-ladder Step 3d plan range updated to **406**–**416**; note full template+GFF arc on this stack.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
