---
title: "test: referencefinder gff template conversation case and field value empty"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-236-test-referencefinder-glob-gff-script-case-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder GFF template/conversation case and field-value empty needle (plan 237)

## Summary

Complete bytes-level case-sensitivity coverage for template and conversation ResRef scans; add field-value GFF empty-needle guard.

## Requirements

- R1. `FindTemplateResRefInGffBytes_CaseSensitive_RequiresExactCase`
- R2. `FindConversationResRefInGffBytes_CaseSensitive_RequiresExactCase`
- R3. `FindFieldValueInGffBytes_EmptyNeedleReturnsEmpty`
- R4. ReferenceFinder filter **54** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
