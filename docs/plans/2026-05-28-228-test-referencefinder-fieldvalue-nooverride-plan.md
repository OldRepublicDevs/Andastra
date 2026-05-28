---
title: "test: referencefinder field value nooverride scope"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-227-test-referencefinder-nooverride-scope-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value NoOverride scope (plan 228)

## Summary

Close the deferred plan 227 gap: OdyTools installation test for `FindFieldValueReferences` when override is disabled, mirroring KotorCLI `Execute_NoOverride_SkipsOverrideTag`.

## Requirements

- R1. `FindFieldValueReferences_NoOverride_SkipsOverrideUtc` — override UTC Tag not returned when `SearchOverride = false`.
- R2. ReferenceFinder filter **26** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
