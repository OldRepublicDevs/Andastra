---
title: "test: referencefinder field value partial tag case script gff partial"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-239-test-referencefinder-script-partial-fv-glob-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value partial, tag case, GFF script partial (plan 240)

## Summary

Final partial-match and case-sensitivity gaps for field-value and script GFF scans; installation-level tag case sensitivity.

## Requirements

- R1. `FindFieldValueReferences_PartialMatch_OverrideUtc`
- R2. `FindTagReferences_CaseSensitive_OverrideUtc`
- R3. `FindScriptResRefInGffBytes_PartialMatch_FindsSubstring`
- R4. ReferenceFinder filter **65** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
