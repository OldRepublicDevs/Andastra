---
title: "test: referencefinder template conversation filetypes and null field filter"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-244-test-referencefinder-field-filter-filetypes-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder template/conversation FileTypes and null field filter (plan 245)

## Summary

Extend FileTypes coverage to template and conversation installation searches; prove null/empty field-name filter searches all GFF string/ResRef fields. Brings ReferenceFinder test count from **77 → 80**.

## Requirements

- R1. `FindTemplateResRefReferences_FileTypesUtcOnly_FindsUtcNotUtp`
- R2. `FindConversationResRefReferences_FileTypesUtcOnly_FindsUtcNotUtp`
- R3. `FindFieldValueReferences_NullFieldNames_SearchesAllFields`
- R4. ReferenceFinder filter count is **80** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Chitin-only scope (deferred).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
