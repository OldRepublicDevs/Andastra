---
title: "test: kotorcli stats and validate commands"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-188-test-kotorcli-key-pack-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate commands (plan 189)

## Summary

Add integration tests for `stats` and `validate` on supported BioWare formats, exposing `UtilityCommands.ExecuteStats` and `UtilityCommands.ExecuteValidate` for test access.

## Requirements

- R1. `stats` on a valid GFF (`.utc`) exits 0.
- R2. `validate` on the same GFF exits 0.
- R3. `validate` on a missing file exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **258**.
