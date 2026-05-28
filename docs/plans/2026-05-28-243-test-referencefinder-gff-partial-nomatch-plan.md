---
title: "test: referencefinder template conversation gff partial and field value nomatch"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-242-test-referencefinder-fieldvalue-case-partial-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder template/conversation GFF partial and field-value no-match (plan 243)

## Summary

Complete bytes-level partial-match coverage for template and conversation ResRef scans; add field-value GFF no-match guard. Brings ReferenceFinder test count from **71 → 74**.

## Requirements

- R1. `FindTemplateResRefInGffBytes_PartialMatch_FindsSubstring`
- R2. `FindConversationResRefInGffBytes_PartialMatch_FindsSubstring`
- R3. `FindFieldValueInGffBytes_NoMatchReturnsEmpty`
- R4. ReferenceFinder filter count is **74** tests, all passing on net9.0.

## Scope Boundaries

- **In:** Three tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.
- **Out:** Chitin-only scope (deferred).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
