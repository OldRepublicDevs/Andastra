---
title: "docs: close kotorcli find-2da-ref --full-row plan 107"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-107-feat-kotorcli-find-2da-ref-full-row-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close KotorCLI find-2da-ref --full-row (plan 276)

## Completion (2026-05-28)

- Plan **107** marked `status: complete` with R1–R5 evidence table.
- Parent plans **063** and **068** updated with KotorCLI `--full-row` closure note.
- Tests: **13** `Find2DARef`, **2** `TwoDARow` — all passed.

## Summary

Close plan **107** — implementation already landed on `feat/holocron-port-phase-b`. This slice verifies requirements R1–R5, runs targeted tests, and updates parent plan docs (**063**, **068**) plus marks plan **107** complete.

## Requirements (from plan 107 — verify only)

- R1. `ReferenceCacheHelpers.CollectTwoDARowReferences` in BioWare mirrors OdyTools row-sweep (memory, label field-value, StrRef columns).
- R2. `TwoDAMemoryReferenceHelper.CollectTwoDARowReferences` delegates to BioWare.
- R3. KotorCLI `find-2da-ref --full-row` uses `CollectTwoDARowReferences`; without flag, behavior unchanged.
- R4. BioWare unit tests for row sweep; KotorCLI integration tests including `--full-row`.
- R5. KotorCLI README documents `--full-row`.

## Verification Evidence

| Req | File | Status |
|-----|------|--------|
| R1 | `src/BioWare/Tools/ReferenceCache.cs` — `CollectTwoDARowReferences` | Landed |
| R2 | `src/Tools/OdyTools/Utils/TwoDAMemoryReferenceHelper.cs` — delegates to BioWare | Landed |
| R3 | `src/Tools/KotorCLI/Commands/Find2DARefCommand.cs` — `--full-row` flag | Landed |
| R4 | `tests/BioWare.Tests/ReferenceCacheHelpersTwoDARowReferencesTests.cs` (**2**); `tests/KotorCLI.Tests/Find2DARefCommandTests.cs` (**13** filter) | Landed |
| R5 | `src/Tools/KotorCLI/README.md` — `--full-row` documented | Landed |

## Test Commands

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef --verbosity quiet
# Passed: 13

dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDARow --verbosity quiet
# Passed: 2
```

## Scope Boundaries

- Doc-only closure; no production code changes.
- No AgentDecompile (tooling-only).
- Browser tests N/A.
