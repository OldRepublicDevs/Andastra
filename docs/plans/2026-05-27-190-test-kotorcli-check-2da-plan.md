---
title: "test: kotorcli check-2da command"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-189-test-kotorcli-stats-validate-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI check-2da command (plan 190)

## Summary

Add integration tests for `check-2da`, expose `ValidationCommands.ExecuteCheck2da` for test access, and align the CLI with `BioWare.Tools.Validation.Check2daFile` (CHITIN + OVERRIDE search order per PyKotor).

## PyKotor / Holocron parity

| Surface | PyKotor (upstream) | C# mirror | KotorCLI |
|---------|-------------------|-----------|----------|
| Core API | `check_2da_file()` in [validation.py](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/tools/validation.py) | `Validation.Check2daFile()` in `src/BioWare/Tools/Validation.cs` | `check-2da` in `ValidationCommands.cs` |
| Default search | `CHITIN`, `OVERRIDE` | `SearchLocation.CHITIN`, `SearchLocation.OVERRIDE` | Must delegate to `Validation.Check2daFile` (not `Installation.Resource`, which also scans Modules) |
| Return shape | `(found: bool, paths: list[Path])` | `(bool found, List<string> paths)` | Exit 0 when found, 1 when missing; log paths on success |

### Behavior deltas

- **PyKotor / BioWare**: existence check only; returns file paths.
- **KotorCLI extension**: after finding a path, optionally parses 2DA bytes and logs column × row dimensions and header preview. Parsing failure emits a warning but still exits 0 if the file was found (Holocron-friendly diagnostics).
- **Prior drift (fixed)**: `check-2da` used `Installation.Resource()`, searching Override → Modules → Chitin. Tests assert CHITIN + OVERRIDE parity via override-only fixtures (no chitin BIF content required).

### How tests assert parity

- `ExecuteCheck2da_FoundInOverride_ExitsZero` — override `.2da` fixture; mirrors `ExecuteCheckTxi_FoundInOverride_ExitsZero`.
- `ExecuteCheck2da_MissingTwoDA_ExitsNonZero` — empty install; mirrors missing TXI test.
- Optional structure logging via `LogTwoDAStructureIfPossible` when a loose override path is returned (Holocron-friendly diagnostics; not required for exit-code parity).

## Requirements

- R1. Public `ValidationCommands.ExecuteCheck2da(installPath, twodaName, logger)` callable from tests.
- R2. Command uses `Validation.Check2daFile` with default CHITIN + OVERRIDE locations.
- R3. Missing 2DA exits non-zero; found override 2DA exits zero.
- R4. Valid 2DA structure is logged when parse succeeds.

## Acceptance criteria

- [x] `ExecuteCheck2da` is public and used by the CLI handler.
- [x] Two new integration tests in `ValidationAndCatCommandsTests.cs` (missing + override found).
- [x] All KotorCLI.Tests pass on net9.0 (**261** total after this plan).
- [x] README test count updated to **261**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
