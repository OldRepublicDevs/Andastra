---
title: "test: referencefinder template conversation glob and gff script case"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-235-test-referencefinder-filetypes-module-glob-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder template/conversation module glob and GFF script case (plan 236)

## Summary

Complete module-glob matrix for template and conversation searches; add GFF script ResRef case-sensitivity bytes test.

## Requirements

- R1. `FindTemplateResRefReferences_ModuleGlob_FiltersNonMatchingModule`
- R2. `FindConversationResRefReferences_ModuleGlob_FiltersNonMatchingModule`
- R3. `FindScriptResRefInGffBytes_CaseSensitive_RequiresExactCase`
- R4. ReferenceFinder filter **51** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
