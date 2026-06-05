---
title: "feat: odyTools FieldValueReferenceHelper UTM/UTS/UTW wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-412-feat-odytools-fieldvalue-utp-utd-wiring-plan.md
branch: feat/plan-413-odytools-fieldvalue-utm-uts-utw-wiring
---

# feat: OdyTools FieldValueReferenceHelper UTM/UTS/UTW wiring (plan 413)

## Summary

Plan **412** / PR **#81** lands `FieldValueReferenceHelper` and wires UTP/UTD. PR **#78** (UTC) and **#72** (GFF) cover other editors. This slice stacks on **#81** and wires **Find Field Value References** into `OdyToolUTM`, `OdyToolUTS`, and `OdyToolUTW` Tag / TemplateResRef context menus (appended after existing tag/resref search items).

## Requirements

- R1. `OdyToolUTM`, `OdyToolUTS`, and `OdyToolUTW` Tag/ResRef menus include field-value search scoped to `Tag` / `TemplateResRef` via `AppendFieldValueFindReferencesMenuItem`.
- R2. Existing **10** `FieldValueReferenceHelperTests` remain green (Append API already covered; no new tests required for wiring-only slice).
- R3. Build-ladder Step 3d plan range updated to **406**–**413**.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
