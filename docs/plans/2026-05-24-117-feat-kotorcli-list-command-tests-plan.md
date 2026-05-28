---
title: "feat: kotorcli list command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-116-feat-kotorcli-unpack-remove-deleted-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI list command test closure (plan 117)

## Summary

Add unit tests for the wired `list` command (build-pipeline target listing), following the `UnpackCommandTests` kotorcli.cfg fixture pattern.

## Requirements

- R1. Expose `ListCommand.Execute` for direct test invocation (matches `LaunchCommand` / `UnpackCommand`).
- R2. Test: minimal `kotorcli.cfg` project lists default target successfully (exit 0).
- R3. Test: filtering to unknown target name exits non-zero.
- R4. Test: working directory without `kotorcli.cfg` exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ListCommand`
