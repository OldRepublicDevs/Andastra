---
title: "feat: kotorcli init command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-131-feat-kotorcli-mixed-pipeline-integration-test-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI init command test closure (plan 132)

## Summary

Add tests for `init --default` scaffolding: config file, source tree, and gitignore without interactive prompts.

## Requirements

- R1. Expose `InitCommand.Execute` for direct tests.
- R2. Test: `init --default` in empty directory creates `kotorcli.cfg` with package name from directory.
- R3. Test: `init --default` creates `src/scripts` and `.gitignore`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InitCommand`
