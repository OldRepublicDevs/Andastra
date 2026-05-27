---
title: "test: kotorcli extract error paths"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-153-test-kotorcli-search-archive-case-sensitive-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract error paths (plan 154)

## Summary

Mirror archive command error-path coverage for `extract`: missing input file and unsupported archive extension.

## Requirements

- R1. Missing `--file` archive path exits non-zero.
- R2. Unsupported file extension exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
