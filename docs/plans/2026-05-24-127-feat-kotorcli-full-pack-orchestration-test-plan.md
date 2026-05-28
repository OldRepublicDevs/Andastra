---
title: "feat: kotorcli full pack orchestration integration test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-126-feat-kotorcli-build-pipeline-integration-test-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI full pack orchestration integration test (plan 127)

## Summary

Verify `pack` with default orchestration (`convert` + `compile` inline, no skip flags) produces a MOD from JSON sources alone.

## Requirements

- R1. Integration test: JSON source only → `pack` (no `--noConvert` / `--noCompile`) exits zero.
- R2. Test: `test.mod` contains the UTC resref without a separate manual `convert` step.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~BuildPipelineIntegration`
