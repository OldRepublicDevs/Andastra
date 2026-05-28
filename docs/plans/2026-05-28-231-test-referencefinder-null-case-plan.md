---
title: "test: referencefinder null guards and case sensitivity"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-230-test-referencefinder-module-scope-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder null guards and case sensitivity (plan 231)

## Summary

Extend OdyTools ReferenceFinder tests with null-installation guards for remaining installation APIs and bytes-level case-sensitivity behavior for Tag search.

## Requirements

- R1. `FindScriptReferences_NullInstallation_ThrowsArgumentNullException`
- R2. `FindTemplateResRefReferences_NullInstallation_ThrowsArgumentNullException`
- R3. `FindConversationResRefReferences_NullInstallation_ThrowsArgumentNullException`
- R4. `FindTagInGffBytes_CaseSensitive_RequiresExactCase`
- R5. ReferenceFinder filter **36** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
