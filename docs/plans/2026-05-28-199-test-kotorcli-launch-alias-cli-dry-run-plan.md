---
title: "test: kotorcli launch alias cli dry-run"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-198-test-kotorcli-validate-installation-no-essential-plan.md
branch: feat/holocron-port-phase-b
---

# test: launch alias CLI dry-run (plan 199)

## Summary

Add subprocess CLI tests proving **`launch`**, **`serve`**, **`play`**, and **`test`** route to the same dry-run path resolution (PyKotor/Holocron alias parity).

## PyKotor / Holocron parity

PyKotor KotorCLI registers `launch`, `serve`, `play`, and `test` as aliases for the same entry point. C# registers four `Command` instances sharing `LaunchCommand.Execute`.

## Requirements

- R1. Parameterized CLI test: `{alias} --dry-run --gameBin <fake.exe> <target>` → exit 0 for `launch`, `serve`, `play`, `test`.
- R2. Assert stdout/stderr contains resolved executable path.
- R3. README test count **282** (278 + 4 `[TestCase]` rows counted by NUnit).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```

## Out of scope

- Spawning the real game process.
- Full install+launch pipeline.
