---
title: "test: kotorcli check-txi and check-2da cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-209-test-kotorcli-validate-installation-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: check-txi and check-2da CLI subprocess (plan 210)

## Summary

Add CLI subprocess tests for **`check-txi`** and **`check-2da`** in `ValidationCommandsTests`, mirroring existing unit tests (plan 209 closed `validate-installation` CLI gaps).

## Requirements

- R1. `check-txi` CLI: missing texture exits **1**.
- R2. `check-txi` CLI: TXI in Override exits **0**.
- R3. `check-2da` CLI: missing 2DA exits **1**.
- R4. `check-2da` CLI: 2DA in Override exits **0** (include minimal `chitin.key` like unit test).
- R5. README test count **313**; note validation CLI subprocess suite (`validate-installation`, `check-txi`, `check-2da`).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~CliCheck"
```
