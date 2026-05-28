---
title: "test: referencefinder bytes nomatch and template conversation partial"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-237-test-referencefinder-gff-case-fieldvalue-empty-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder bytes NoMatch and template/conversation partial match (plan 238)

## Summary

Add GFF bytes negative-match guards for tag, template, and conversation; extend installation partial-match coverage to template ResRef and conversation ResRef.

## Requirements

- R1. `FindTagInGffBytes_NoMatchReturnsEmpty`
- R2. `FindTemplateResRefInGffBytes_NoMatchReturnsEmpty`
- R3. `FindConversationResRefInGffBytes_NoMatchReturnsEmpty`
- R4. `FindTemplateResRefReferences_PartialMatch_OverrideUtc`
- R5. `FindConversationResRefReferences_PartialMatch_OverrideUtc`
- R6. ReferenceFinder filter **59** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
