---
title: "test: kotorcli key-pack end-to-end"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-187-docs-kotorcli-readme-archive-closure-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI key-pack end-to-end (plan 188)

## Summary

Add integration tests for `key-pack` creating a KEY from a directory of BIF files, with public `KeyPackCommand.Execute` for test access (matching `CreateArchiveCommand`).

## Requirements

- R1. `key-pack` on a directory containing one BIF exits 0 and writes a KEY file.
- R2. Generated KEY enables `list-archive` on the source BIF (sibling KEY stem match).
- R3. Missing input directory exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **255** and remove `key-pack` from archive out-of-scope note.
