---
title: "test: find script references compiled ncs installation path"
type: test
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-287-feat-ncs-consts-script-resref-scanner-plan.md
branch: feat/holocron-port-phase-b
---

# test: FindScriptReferences compiled NCS installation path (plan 289)

## Summary

Add end-to-end installation search test proving plan **287** CONSTS scanner works through `ReferenceFinder.FindScriptReferences` (not only `FindScriptResRefInNcsBytes` unit helper).

## Requirements

- R1. `FindScriptReferences_OverrideCompiledNcs_ReturnsNcsBytecodePath` in `ReferenceFinderTests.cs`.
- R2. Fixture uses `NCSAuto.CompileNss` with `ExecuteScript("k_target_hb", ...)`.
- R3. Assert `(NCS bytecode) offset_<n>` field path on override `.ncs` hit.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindScriptReferences_OverrideCompiledNcs
```

## Scope Boundaries

- Test-only; no production code changes unless test reveals bug.
