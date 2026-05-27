---
title: "feat: kotorcli config command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-132-feat-kotorcli-init-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI config command test closure (plan 133)

## Summary

Add tests for `config --local` get/set/unset/list using package-scoped `.kotorcli/user.cfg`.

## Requirements

- R1. Expose `ConfigCommand.Execute` for direct tests.
- R2. Test: `--local` set writes key/value to `.kotorcli/user.cfg`.
- R3. Test: `--local` unset removes key; `--list` on empty config exits zero.
- R4. Test: no operation specified exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ConfigCommand`
