---
title: "feat: kotorcli nss compile pack integration test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-128-refactor-kotorcli-shared-glob-matcher-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI NSS compile → pack integration test (plan 129)

## Summary

Fix compile include-pattern discovery for package/target `sources` tables, then verify NSS compile output lands in cache and packs into a MOD.

## Requirements

- R1. `CompileCommand` uses include/exclude pattern strings from `GetTargetSources` directly (not `ResolveTargetValue(..., "sources", ...)`).
- R2. Integration test: minimal NSS under `src/` → `compile` → `pack` (`--noConvert --noCompile`) produces MOD with NCS resref.
- R3. Integration test: `pack` with inline compile (no skip flags) produces MOD from NSS-only fixture.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~BuildPipelineIntegration|FullyQualifiedName~CompileCommand`
