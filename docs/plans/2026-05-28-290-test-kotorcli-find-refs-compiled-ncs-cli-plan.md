---
title: "test: kotorcli find-refs compiled ncs cli subprocess"
type: test
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-289-test-find-script-references-compiled-ncs-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI find-refs compiled NCS CLI (plan 290)

## Summary

Subprocess integration test for `kotorcli find-refs --type script` hitting a compiled NCS with `ExecuteScript` CONSTS literal (plan **287** path via CLI).

## Requirements

- R1. `Cli_FindRefs_Script_CompiledNcsInOverride_ExitsZero` in `FindRefsCommandCliTests.cs`.
- R2. Fixture uses `NCSAuto.CompileNss` + override `.ncs` file.
- R3. CLI output contains needle ResRef.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Cli_FindRefs_Script_CompiledNcs
```

## Scope Boundaries

- Test-only slice; complements plan **289** BioWare installation test.
