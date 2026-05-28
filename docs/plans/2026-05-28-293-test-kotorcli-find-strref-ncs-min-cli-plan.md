---
title: "test: kotorcli find-strref ncs-strref-min cli subprocess"
type: test
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-292-docs-consti-disambiguation-partial-landing-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI find-strref --ncs-strref-min CLI (plan 293)

## Summary

Subprocess integration test verifying `kotorcli find-strref` with `--ncs-strref-min 100` still finds a small CONSTI literal (50) in override NCS via slow path — complements plan **292** CONSTI disambiguation docs and existing unit test `Execute_SlowPathSmallConsti_FoundWithHighMinThreshold`.

## Requirements

- R1. `Cli_FindStrRef_SmallNcsConsti_WithHighMinThreshold_ExitsZero` in `InstallationRefSearchCommandCliTests.cs`.
- R2. Fixture uses `NCSAuto.CompileNss` with small literal + `--ncs-strref-min 100`.
- R3. KotorCLI README test count **368**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Cli_FindStrRef_SmallNcsConsti
```

## Scope Boundaries

- Test-only slice; no production code changes.
