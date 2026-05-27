---
title: "feat: kotorcli init with file unpack integration test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-133-feat-kotorcli-config-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI init with file unpack integration test (plan 134)

## Summary

Verify `init --default` with an initial MOD path unpacks resources into the scaffolded source tree.

## Requirements

- R1. Integration test: create MOD with UTC → `InitCommand.Execute(..., initFile, ...)` exits zero.
- R2. Test: unpacked UTC appears as JSON under `src/blueprints/creatures/` per default init rules.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InitCommand`
