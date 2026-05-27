---
title: "feat: kotorcli unpack removeDeleted integration test"
type: feat
status: active
date: 2026-05-27
origin: docs/plans/2026-05-24-134-feat-kotorcli-init-file-unpack-test-plan.md
branch: feat/holocron-port-phase-b
note: "Implementation commits also reference plan 136 (same slice)."
---

# feat: KotorCLI unpack --removeDeleted integration test (plan 135)

## Summary

Add pipeline-level coverage for `unpack --removeDeleted`: after `pack` produces a MOD, unpack with the flag deletes stale JSON under rule roots, preserves archive-backed sources, and never deletes under `.kotorcli/`.

## Requirements

- R1. Integration test in `BuildPipelineIntegrationTests`: pack UTC JSON → MOD, seed stale JSON, unpack with `removeDeleted: true` removes stale only.
- R2. Unit test in `UnpackCommandTests`: stale file under `.kotorcli/cache` survives unpack with `--removeDeleted`.
- R3. Optional roundtrip: delete source JSON, unpack without flag restores JSON under rules path.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~Unpack|FullyQualifiedName~BuildPipeline"`
