---
title: "test: kotorcli find-strref negative ncs-strref-min cli validation"
type: test
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-293-test-kotorcli-find-strref-ncs-min-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI find-strref negative --ncs-strref-min CLI (plan 294)

## Summary

Subprocess test that `kotorcli find-strref` rejects `--ncs-strref-min -1` with non-zero exit — complements plan **293** slow-path CLI test and existing unit test `Execute_NcsStrRefMin_Negative_ReturnsError`. Includes post-PR-#11-merge closure doc sync for plans **291**–**294** in parent plans **063** / **068**.

## Requirements

- R1. `Cli_FindStrRef_NegativeNcsStrRefMin_ExitsNonZero` in `InstallationRefSearchCommandCliTests.cs`.
- R2. KotorCLI README test count **369**.
- R3. Update plans **063** / **068** with plan **293** CLI test + plan **294** closure note.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Cli_FindStrRef_NegativeNcsStrRefMin
```

## Scope Boundaries

- Test + doc sync; no production code changes.
