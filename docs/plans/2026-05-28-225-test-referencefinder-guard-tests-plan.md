---
title: "test: referencefinder empty needle and null guards"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-224-test-referencefinder-template-installation-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder empty-needle and null-install guards (plan 225)

## Summary

Extend OdyTools `ReferenceFinderTests` with installation-level guard tests for tag, template, and conversation search (script already covered).

## Requirements

- R1. `FindTagReferences_EmptyNeedleReturnsEmpty`
- R2. `FindTemplateResRefReferences_EmptyNeedleReturnsEmpty`
- R3. `FindConversationResRefReferences_EmptyNeedleReturnsEmpty`
- R4. `FindTagReferences_NullInstallation_ThrowsArgumentNullException`
- R5. OdyTools ReferenceFinder filter **19** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
