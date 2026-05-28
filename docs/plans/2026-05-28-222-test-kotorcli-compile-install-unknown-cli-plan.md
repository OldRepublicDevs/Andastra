---
title: "test: kotorcli compile install unknown target cli"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-221-test-kotorcli-config-launch-edge-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: compile/install unknown-target CLI + PR sync (plan 222)

## Summary

Mirror remaining per-command failure paths for `compile` and `install` through CLI subprocess; refresh PR #11 body to reflect plans 211–221 and **358** tests.

## Requirements

- R1. `CliCompile_UnknownTarget_ExitsNonZero` — mirrors `CompileCommandTests.Execute_UnknownTarget_ExitsNonZero`.
- R2. `CliInstall_UnknownTarget_ExitsNonZero` — mirrors `InstallCommandTests.Execute_UnknownTarget_ExitsNonZero`.
- R3. Extend `BuildPipelineCommandCliTests.cs`.
- R4. Update PR #11 description with plans 214–221 summary and current test count.
- R5. README **358** tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
