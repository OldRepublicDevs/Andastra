---
title: "feat: kotorcli pack unpack removeDeleted integration"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-135-feat-kotorcli-glob-pattern-matcher-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI pack → unpack --removeDeleted integration (plan 136)

## Summary

Pipeline-level coverage: after `pack` produces a MOD from JSON sources, `unpack --removeDeleted` removes stale JSON under rule roots while preserving archive-backed sources and `.kotorcli/cache`.

## Requirements

- R1. Integration test in `BuildPipelineIntegrationTests`: pack UTC JSON → MOD, seed stale JSON, unpack with `removeDeleted: true` removes stale only.
- R2. Unit test in `UnpackCommandTests`: stale file under `.kotorcli/cache` survives unpack with `--removeDeleted`.
- R3. No production code changes unless tests reveal a bug.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~Unpack|FullyQualifiedName~BuildPipeline"`
