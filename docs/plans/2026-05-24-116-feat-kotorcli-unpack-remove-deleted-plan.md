---
title: "feat: kotorcli unpack removeDeleted"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-115-feat-kotorcli-launch-path-resolution-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI unpack --removeDeleted (plan 116)

## Summary

Implement `--removeDeleted` on `unpack`: after extracting the archive, delete stale source files under rule destination trees that were not written during the current unpack.

## Requirements

- R1. When `--removeDeleted` is set, remove files under package rule roots (and default `src/`) that are absent from the current unpack output.
- R2. Never delete under `.kotorcli/`.
- R3. Integration test: stale file in `src/blueprints/creatures/` is removed after unpack with `--removeDeleted`; current archive resources remain.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~UnpackCommand`
