---
title: "test: kotorcli no-config pipeline cli failures"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-222-test-kotorcli-compile-install-unknown-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: no-config build-pipeline CLI failures (plan 223)

## Summary

Mirror `Execute_NoConfigDirectory_ExitsNonZero` unit tests for remaining build-pipeline commands through CLI subprocess; mark KotorCLI CLI subprocess coverage substantially complete in README.

## Requirements

- R1. Empty project dir (no `kotorcli init`) → `convert`/`compile`/`pack`/`install`/`unpack` exit **1**.
- R2. `launch --install-only` in empty project dir exits **1**.
- R3. Extend `BuildPipelineCommandCliTests.cs` and `LaunchCommandCliTests.cs`.
- R4. README **364** tests; update Known Issues to note CLI subprocess substantially complete.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
