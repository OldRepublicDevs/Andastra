---
title: "test: referencefinder chitin template conversation and no-chitin script"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-246-test-referencefinder-chitin-scope-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder chitin template/conversation and no-chitin script (plan 247)

## Summary

Extend chitin scope coverage to template and conversation searches; add script no-chitin skip test mirroring plan 246 tag coverage. Brings ReferenceFinder test count from **83 → 86**.

## Requirements

- R1. `FindTemplateResRefReferences_ChitinOnly_ReturnsFieldPath`
- R2. `FindConversationResRefReferences_ChitinOnly_ReturnsFieldPath`
- R3. `FindScriptReferences_NoChitin_SkipsChitinResource`
- R4. ReferenceFinder filter count is **86** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests reusing `WriteChitinWithUtc` in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Production changes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
