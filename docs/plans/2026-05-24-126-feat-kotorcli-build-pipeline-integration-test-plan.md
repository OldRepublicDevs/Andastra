---
title: "feat: kotorcli build pipeline integration test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-125-feat-kotorcli-install-happy-path-test-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI build pipeline integration test (plan 126)

## Summary

Stage converted GFF binaries into `.kotorcli/cache` so `pack` can consume them, then add an integration test covering convert → pack → install.

## Requirements

- R1. `ConvertCommand` copies each converted (or up-to-date) binary GFF into `.kotorcli/cache/<target>/`.
- R2. Integration test: JSON source → `convert` → `pack` (`--noConvert --noCompile`) → `install` succeeds.
- R3. Integration test: installed `modules/test.mod` contains the converted UTC resref.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~BuildPipelineIntegration`
