---
title: "test: kotorcli launch dry-run resolve game binary cli"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-218-test-kotorcli-pack-unpack-roundtrip-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: launch --dry-run game binary resolution CLI (plan 219)

## Summary

Mirror key `LaunchCommand.ResolveGameBinary` unit scenarios through CLI subprocess (`launch --dry-run`) so path resolution is proven end-to-end, not only via direct API calls.

## Requirements

- R1. `--installDir` with `swkotor.exe` exits **0** and logs resolved full path.
- R2. Install dir without game exe exits **1** (env vars cleared).
- R3. `--gameBin` wins over install-dir `swkotor.exe`.
- R4. Extend `LaunchCommandCliTests.cs`; clear `KOTOR_PATH` / `K1_PATH` / `K2_PATH` in tests.
- R5. README **347** tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommandCli"
```
