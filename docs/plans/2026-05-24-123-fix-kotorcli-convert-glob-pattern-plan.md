---
title: "fix: kotorcli convert glob pattern for src/**/*.json"
type: fix
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-122-feat-kotorcli-install-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# fix: KotorCLI convert glob pattern closure (plan 123)

## Summary

Fix `convert` file discovery so init-template patterns like `src/**/*.json` resolve correctly, matching pack/compile/list behavior.

## Requirements

- R1. Replace broken `Directory.GetFiles` glob hack with `FindFilesMatchingPattern` (same approach as `CompileCommand`/`PackCommand`).
- R2. Resolve include patterns from `GetTargetSources` (patterns are already extracted from package/target `sources` tables).
- R3. Test: JSON under `src/nested/` is converted when config uses `include = "src/**/*.json"`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ConvertCommand`
