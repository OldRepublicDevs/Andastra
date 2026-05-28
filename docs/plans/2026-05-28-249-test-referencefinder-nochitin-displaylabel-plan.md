---
title: "test: referencefinder template conversation nochitin and display label"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-248-test-referencefinder-fieldvalue-chitin-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder template/conversation no-chitin and DisplayLabel (plan 249)

## Summary

Complete no-chitin skip coverage for template and conversation searches; add `ReferenceSearchResult.DisplayLabel` formatting test. Brings ReferenceFinder test count from **89 → 92**.

## Requirements

- R1. `FindTemplateResRefReferences_NoChitin_SkipsChitinResource`
- R2. `FindConversationResRefReferences_NoChitin_SkipsChitinResource`
- R3. `ReferenceSearchResult_DisplayLabel_FormatsResourceFieldAndValue`
- R4. ReferenceFinder filter count is **92** tests, all passing on net9.0.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
