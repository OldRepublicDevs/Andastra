---
title: "feat: PyKotor port residuals — FAC removal tests, KotorCLI grep"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (deferred follow-up)
branch: feat/holocron-fac-kotorcli
closure: docs/plans/2026-05-28-283-docs-close-plan-070-pykotor-residuals-plan.md
---

# feat: PyKotor port residuals (plan 070)

## Completion (2026-05-28)

All requirements R1–R4 landed. UTD/UTP/UTT ref finder and walkmesh tests in plan **069** (closed **279**). Closed doc-only via plan **283**.

| Req | Status | Evidence |
|-----|--------|----------|
| R1 | **Landed** | `FACEditor_RemoveFaction_ReindexesReputations` in `OdyToolFACTests.cs` |
| R2 | **Landed** | KotorCLI `grep` — **9** tests matching `Grep` filter |
| R3 | **Landed** | KotorCLI `diff` in `GrepDiffCatCommandCliTests` + `UtilityCommandsTests` |
| R4 | **Landed** | **21** `FormatConvertIntegrationTests` |

**Verification (2026-05-28):**

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolFAC
# Passed: 3

dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Grep
# Passed: 9

dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvert
# Passed: 21
```

## Summary

Land deferred slices from plan 063 after U1–U7: **FAC removal** characterization tests, **KotorCLI grep** utility wiring with fail-fast STUB exit codes, and **KotorCLI integration tests** for format convert commands. UTD/UTP/UTT reference finder and indoor walkmesh tests landed in plan 069.

## Requirements

- R1. OdyToolFAC tests cover faction removal and reputation reindexing roundtrip.
- R2. KotorCLI `grep` searches file content; missing file / no match exits non-zero.
- R3. KotorCLI `diff`/`merge` STUBs exit non-zero (no false success).
- R4. KotorCLI integration tests assert `json2gff` / `gff2json` produce output files.

## Implementation Units

### U10 — FAC removal tests

**Files:** `OdyToolFACTests.cs`

### U11 — KotorCLI grep + integration tests

**Files:** `UtilityCommands.cs`, `tests/KotorCLI.Tests/`

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "IndoorMapBuild|OdyToolFAC|ReferenceFinder"`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
