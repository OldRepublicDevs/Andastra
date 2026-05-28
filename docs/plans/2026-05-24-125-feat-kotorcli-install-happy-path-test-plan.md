---
title: "feat: kotorcli install command happy path test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-124-feat-kotorcli-pack-happy-path-test-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI install command happy path test (plan 125)

## Summary

Add a happy-path test for `install` that copies a packed mod into a fake KOTOR install directory (with `chitin.key`).

## Requirements

- R1. Test: valid config + packed `test.mod` + fake install dir with `chitin.key` exits zero.
- R2. Test: `modules/test.mod` exists in the fake install directory after install.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InstallCommand`
