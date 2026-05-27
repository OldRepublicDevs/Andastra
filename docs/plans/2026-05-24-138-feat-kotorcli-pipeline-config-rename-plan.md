---
title: "refactor: kotorcli shared pack-unpack pipeline config"
type: refactor
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-137-feat-kotorcli-pack-unpack-roundtrip-plan.md
branch: feat/holocron-port-phase-b
---

# refactor: KotorCLI shared pack-unpack pipeline config (plan 138)

## Summary

Rename `UnpackRemoveDeletedConfig` to `PackUnpackPipelineConfig` in `BuildPipelineIntegrationTests` — the constant is shared by pack→unpack roundtrip and removeDeleted tests.

## Requirements

- R1. Rename constant and all references in `BuildPipelineIntegrationTests.cs`.
- R2. Remove stray duplicate untracked plan file `2026-05-24-135-feat-kotorcli-unpack-remove-deleted-integration-plan.md` if present.
- R3. Full `KotorCLI.Tests` suite passes as merge-readiness gate.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
