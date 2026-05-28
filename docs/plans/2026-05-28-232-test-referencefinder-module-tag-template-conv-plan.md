---
title: "test: referencefinder module scope tag template conversation"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-230-test-referencefinder-module-scope-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder module scope — tag, template, conversation (plan 232)

## Summary

Extend plan 230 module-scope coverage to tag, template ResRef, and conversation ResRef installation searches.

## Requirements

- R1. `FindTagReferences_ModuleMod_ReturnsFieldPath`
- R2. `FindTemplateResRefReferences_ModuleMod_ReturnsFieldPath`
- R3. `FindConversationResRefReferences_ModuleMod_ReturnsFieldPath`
- R4. Refactor module UTC writer helper for reuse.
- R5. ReferenceFinder filter **39** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
