---
title: "test: kotorcli find-refs cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-079-feat-holocron-phase-j-kotorcli-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI find-refs CLI subprocess (plan 206)

## Summary

Add `dotnet exec KotorCLI.dll` integration tests for **`find-refs`** across all four `--type` values (plan 079 unit tests call `FindRefsCommand.Execute` directly only).

## Requirements

- R1. CLI subprocess tests: `script`, `tag`, `template`, `conversation` each exit 0 on override fixture hit.
- R2. CLI subprocess test: no match exits non-zero.
- R3. README test count **300** (295 + 5 CLI tests).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~FindRefsCommandCli"
```
