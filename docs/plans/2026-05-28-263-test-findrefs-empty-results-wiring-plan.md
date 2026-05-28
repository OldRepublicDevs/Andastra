---
title: "test: find-references empty results wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-262-test-twodamemoryreferencehelper-wiring-plan.md
branch: feat/holocron-port-phase-b
---

# test: Find-references empty results wiring (plan 263)

## Summary

Add **2** tests verifying empty-result preconditions for StrRef and 2DA row search (the branch that would show an info dialog in UI).

## Requirements

- R1. `CollectStrRefReferences` returns empty when StrRef has no override hits.
- R2. `CollectTwoDARowReferences` returns empty on an empty override install.
- R3. Filtered OdyTools tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~NoMatch_ReturnsEmpty|FullyQualifiedName~EmptyInstall_ReturnsEmpty"
```
