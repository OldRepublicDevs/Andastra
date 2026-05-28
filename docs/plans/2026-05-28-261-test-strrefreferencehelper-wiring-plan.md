---
title: "test: strrefreferencehelper findandshow wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-082-feat-holocron-phase-m-strref-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# test: StrRefReferenceHelper FindAndShow wiring (plan 261)

## Summary

Pivot after reference-finder closure (plan 260). Add **4** tests for `StrRefReferenceHelper.FindAndShowStrRefReferences` guard clauses and override wiring smoke.

## Requirements

- R1. `FindAndShowStrRefReferences` no-ops on negative StrRef or null installation (no throw).
- R2. `CollectStrRefReferences` returns empty for negative StrRef.
- R3. `FindAndShowStrRefReferences` with override SSF hit and `showOptionsDialog: false` completes without exception.
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter StrRefReferenceHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter StrRefReferenceHelper
```
