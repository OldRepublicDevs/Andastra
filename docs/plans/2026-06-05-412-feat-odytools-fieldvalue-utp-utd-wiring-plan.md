---
title: "feat: odyTools FieldValueReferenceHelper UTP/UTD wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-409-feat-odytools-fieldvalue-utc-wiring-plan.md
branch: feat/plan-412-odytools-fieldvalue-utp-utd-wiring
---

# feat: OdyTools FieldValueReferenceHelper UTP/UTD wiring (plan 412)

## Summary

Open PR **#72** (GFF) and **#78** (UTC) cover other template editors. This slice lands `FieldValueReferenceHelper` on `master` and wires **Find Field Value References** into `OdyToolUTP` and `OdyToolUTD` Tag / TemplateResRef fields (appending to existing tag/resref context menus), with OdyTools.Tests coverage and build-ladder Step 3d.

## Requirements

- R1. `FieldValueReferenceHelper` with collect, FindAndShow, Attach, and Append menu APIs.
- R2. `OdyToolUTP` and `OdyToolUTD` Tag/ResRef context menus include field-value search scoped to `Tag` / `TemplateResRef`.
- R3. **10** unit tests; build-ladder Step 3d row.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
