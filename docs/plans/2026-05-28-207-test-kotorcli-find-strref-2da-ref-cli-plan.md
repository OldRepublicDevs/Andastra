---
title: "test: kotorcli find-strref and find-2da-ref cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-206-test-kotorcli-find-refs-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: find-strref and find-2da-ref CLI subprocess (plan 207)

## Summary

Mirror plan 206 for installation StrRef and 2DA row reference commands: subprocess tests through `dotnet exec KotorCLI.dll`.

## Requirements

- R1. `find-strref` CLI hit + no-match on override SSF fixture.
- R2. `find-2da-ref` CLI hit + no-match on override UTC appearance row fixture.
- R3. Scope flags: `--override-only --no-chitin --no-modules`; strref adds `--no-ncs` for speed.
- R4. README test count **304** (300 + 4 tests).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~InstallationRefSearchCommandCli"
```
