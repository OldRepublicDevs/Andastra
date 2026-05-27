---
title: "refactor: kotorcli shared glob pattern matcher"
type: refactor
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-127-feat-kotorcli-full-pack-orchestration-test-plan.md
branch: feat/holocron-port-phase-b
---

# refactor: KotorCLI shared glob pattern matcher (plan 128)

## Summary

Extract duplicated `FindFilesMatchingPattern` from convert/pack/compile/list commands into one helper using the improved `src/**/*.ext` handling from plan 123.

## Requirements

- R1. Add `GlobPatternMatcher.FindFilesMatchingPattern(rootDir, pattern)` in `src/Tools/KotorCLI/`.
- R2. Replace private copies in `ConvertCommand`, `PackCommand`, `CompileCommand`, and `ListCommand`.
- R3. Existing KotorCLI command/integration tests continue to pass.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ConvertCommand|FullyQualifiedName~PackCommand|FullyQualifiedName~CompileCommand|FullyQualifiedName~ListCommand|FullyQualifiedName~BuildPipelineIntegration`
