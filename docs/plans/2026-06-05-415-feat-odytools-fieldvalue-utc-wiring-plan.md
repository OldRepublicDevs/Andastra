---
title: "feat: odyTools FieldValueReferenceHelper UTC wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-414-feat-odytools-fieldvalue-uti-ute-utt-wiring-plan.md
branch: feat/plan-415-odytools-fieldvalue-utc-wiring
---

# feat: OdyTools FieldValueReferenceHelper UTC wiring (plan 415)

## Summary

Plans **412**–**414** / PRs **#81**–**#83** wire UTP/UTD/UTM/UTS/UTW/UTI/UTE/UTT. Open PR **#78** wires UTC separately on an older stack. This slice stacks on **#83** and adds UTC Tag/TemplateResRef field-value menus on the unified helper stack — completing UT* template editor coverage (GFF remains on **#72**).

## Requirements

- R1. `OdyToolUTC` Tag/ResRef context menus include field-value search scoped to `Tag` / `TemplateResRef` via `AppendFieldValueFindReferencesMenuItem`.
- R2. Existing **10** `FieldValueReferenceHelperTests` remain green.
- R3. Build-ladder Step 3d plan range updated to **406**–**415**; note UT* arc complete on this stack.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
