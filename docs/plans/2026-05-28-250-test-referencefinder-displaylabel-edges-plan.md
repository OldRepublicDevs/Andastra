---
title: "test: referencefinder displaylabel edge cases and milestone"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-249-test-referencefinder-nochitin-displaylabel-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder DisplayLabel edge cases (plan 250)

## Summary

Complete `ReferenceSearchResult.DisplayLabel` branch coverage for FileResultsDialog formatting. Marks ReferenceFinder installation test suite **substantially complete** at **95** tests.

## Requirements

- R1. `ReferenceSearchResult_DisplayLabel_WithoutMatchedValue_OmitsEqualsClause`
- R2. `ReferenceSearchResult_DisplayLabel_WithoutResource_ReturnsFieldPathOnly`
- R3. `ReferenceSearchResult_DisplayLabel_WithResourceOnly_ReturnsFileName`
- R4. ReferenceFinder filter count is **95** tests, all passing on net9.0.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
