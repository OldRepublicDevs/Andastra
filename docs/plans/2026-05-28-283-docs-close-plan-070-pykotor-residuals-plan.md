---
title: "docs: close plan 070 pykotor port residuals"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-070-feat-pykotor-port-residuals-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close plan 070 — PyKotor port residuals (plan 283)

## Completion (2026-05-28)

- Plan **070** marked `status: complete` with R1–R4 evidence.
- Plan **063** progress note added for deferred follow-up closure.
- Tests: **3** OdyToolFAC (incl. removal), **9** grep filter, **21** FormatConvert — all passed.

## Summary

Close plan **070** (FAC removal tests, KotorCLI grep/diff utilities, format-convert integration). UTD/UTP/UTT ref finder and walkmesh tests noted as landed in plan **069** (closed **279**).

## Requirements

- R1. `FACEditor_RemoveFaction_ReindexesReputations` in `OdyToolFACTests.cs`.
- R2. KotorCLI `grep` with match/no-match/missing-file coverage.
- R3. KotorCLI `diff` CLI tests (identical/different files).
- R4. Format convert integration (`json2gff` / `gff2json`) — **21** `FormatConvert` tests.
- R5. Mark plan **070** complete; update plan **063**.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolFAC
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Grep
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvert
```

## Scope Boundaries

- Doc/plan closure only; U4 KotorDiff integration tracked separately in plan **063** U4 row.
