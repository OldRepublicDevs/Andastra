---
title: "test: kotorcli find-field-value cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-207-test-kotorcli-find-strref-2da-ref-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: find-field-value CLI subprocess (plan 208)

## Summary

Complete KotorCLI installation reference-search CLI subprocess coverage with **`find-field-value`** (plans 206–207 covered `find-refs`, `find-strref`, `find-2da-ref`).

## Requirements

- R1. CLI hit test on override UTC tag fixture.
- R2. CLI no-match exits non-zero.
- R3. Extend `InstallationRefSearchCommandCliTests.cs`.
- R4. README test count **306**; note ref-search CLI suite complete.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~InstallationRefSearchCommandCli"
```
