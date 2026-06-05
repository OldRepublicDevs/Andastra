---
title: "feat: odyTools FieldValueReferenceHelper and UTC wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-006-feat-odytools-fieldvalue-reference-helper-plan.md
branch: feat/plan-409-odytools-fieldvalue-utc-wiring
---

# feat: OdyTools FieldValueReferenceHelper and UTC wiring (plan 409)

## Summary

Plan **406** (open PR **#72**) lands `FieldValueReferenceHelper` on `OdyToolGFF` only. This slice adds the helper on `master`, wires **Find Field Value References** into `OdyToolUTC` Tag and TemplateResRef fields (appending to existing context menus), and adds OdyTools.Tests coverage plus build-ladder Step 3d.

## Requirements

- R1. `FieldValueReferenceHelper` with collect, FindAndShow, Attach, and Append menu APIs.
- R2. `OdyToolUTC` Tag/ResRef context menus include field-value search scoped to `Tag` / `TemplateResRef`.
- R3. **8** unit tests; build-ladder filter row.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
