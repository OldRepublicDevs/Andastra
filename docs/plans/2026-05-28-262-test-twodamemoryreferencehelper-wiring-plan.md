---
title: "test: twodamemoryreferencehelper findandshow wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-084-feat-holocron-phase-o-2da-row-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# test: TwoDAMemoryReferenceHelper FindAndShow wiring (plan 262)

## Summary

Continue Holocron reference-search wiring after plan 261. Add **4** tests for `TwoDAMemoryReferenceHelper` guard clauses and override wiring smoke.

## Requirements

- R1. `FindAndShowTwoDAMemoryReferences` no-ops on negative row, blank filename, or null installation (no throw).
- R2. `CollectTwoDARowReferences` returns empty for negative row index.
- R3. `FindAndShowTwoDAMemoryReferences` with override hit and `showOptionsDialog: false` completes without exception.
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter TwoDAMemoryReferenceHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter TwoDAMemoryReferenceHelper
```
