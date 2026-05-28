---
title: "test: kotorcli launch alias cli install and spawn"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-204-feat-kotorcli-launch-spawn-plan.md
branch: feat/holocron-port-phase-b
---

# test: launch alias CLI install + spawn (plan 205)

## Summary

Close the launch CLI integration gap from plan 204: subprocess tests for **`launch`/`serve`/`play`/`test`** with **`--wait`** through `dotnet exec KotorCLI.dll`, using a Linux shell stub as `swkotor.exe`.

## Requirements

- R1. Parameterized CLI test: `{alias} default --installDir <fake> --wait` from temp kotorcli project → exit 0.
- R2. Assert `modules/test.mod` and launch marker file exist after run.
- R3. Linux-only (shell stub); `Assert.Ignore` elsewhere.
- R4. README test count **295** (291 + 4 aliases).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommandCli"
```
