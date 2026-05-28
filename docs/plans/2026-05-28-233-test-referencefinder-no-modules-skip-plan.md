---
title: "test: referencefinder no modules skip tag template conversation"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-232-test-referencefinder-module-tag-template-conv-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder NoModules skip — tag, template, conversation (plan 233)

## Summary

Complete module-scope negative matrix: tag, template ResRef, and conversation searches skip module UTC when `SearchModules = false`, mirroring `FindScriptReferences_NoModules_SkipsModuleUtc`.

## Requirements

- R1. `FindTagReferences_NoModules_SkipsModuleUtc`
- R2. `FindTemplateResRefReferences_NoModules_SkipsModuleUtc`
- R3. `FindConversationResRefReferences_NoModules_SkipsModuleUtc`
- R4. ReferenceFinder filter **42** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
