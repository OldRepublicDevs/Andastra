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

Adds `FieldValueReferenceHelper` (deferred from plan **406**) and wires **Find Field Value References** on `OdyToolUTC` Tag and TemplateResRef fields via append-only context menus. **8** tests; build-ladder Step **3d**.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
