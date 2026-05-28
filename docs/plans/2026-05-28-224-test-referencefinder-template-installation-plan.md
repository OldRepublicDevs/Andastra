---
title: "test: referencefinder template resref installation search"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-068-feat-reference-finder-installation-utc-plan.md
branch: feat/holocron-port-phase-b
---

# test: FindTemplateResRefReferences installation search (plan 224)

## Summary

Pivot from KotorCLI CLI subprocess (substantially complete at 364 tests) to OdyTools/BioWare reference-finder parity: add missing installation-level test for `FindTemplateResRefReferences`, mirroring existing tag and conversation override tests.

## Requirements

- R1. `FindTemplateResRefReferences_OverrideUtc_ReturnsFieldPath` in `ReferenceFinderTests.cs`.
- R2. Stub K1 install with override UTC containing `TemplateResRef`; assert field path and matched value.
- R3. Run `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`.
- R4. Update PR #11 body with plan 223 + OdyTools slice note.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
dotnet build Andastra.sln --framework net9.0 -m:1
```
