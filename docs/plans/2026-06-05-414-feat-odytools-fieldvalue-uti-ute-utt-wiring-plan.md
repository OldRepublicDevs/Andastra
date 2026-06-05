---
title: "feat: odyTools FieldValueReferenceHelper UTI/UTE/UTT wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-413-feat-odytools-fieldvalue-utm-uts-utw-wiring-plan.md
branch: feat/plan-414-odytools-fieldvalue-uti-ute-utt-wiring
---

# feat: OdyTools FieldValueReferenceHelper UTI/UTE/UTT wiring (plan 414)

## Summary

Plans **412**–**413** / PRs **#81**–**#82** wire UTP/UTD/UTM/UTS/UTW. PR **#78** (UTC) and **#72** (GFF) cover other editors. This slice stacks on **#82** and wires **Find Field Value References** into `OdyToolUTI`, `OdyToolUTE`, and `OdyToolUTT` Tag / TemplateResRef context menus — completing the remaining UT* template editors on this arc.

## Requirements

- R1. `OdyToolUTI`, `OdyToolUTE`, and `OdyToolUTT` Tag/ResRef menus include field-value search scoped to `Tag` / `TemplateResRef` via `AppendFieldValueFindReferencesMenuItem`.
- R2. Existing **10** `FieldValueReferenceHelperTests` remain green (wiring-only; Append API already covered).
- R3. Build-ladder Step 3d plan range updated to **406**–**414**.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
