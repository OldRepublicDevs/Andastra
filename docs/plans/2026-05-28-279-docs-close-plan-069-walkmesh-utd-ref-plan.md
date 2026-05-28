---
title: "docs: close plan 069 walkmesh and utd-utp-utt ref finder"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-069-feat-holocron-u3-walkmesh-u6-utd-utp-utt-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close plan 069 — U3 walkmesh tests + UTD/UTP/UTT ref finder (plan 279)

## Completion (2026-05-28)

- Plan **069** marked `status: complete` with R1–R4 evidence.
- Plan **063** U3 and U6 rows note plan **069** closure.
- Tests: **2** IndoorMapBuildWalkmesh, **8** ScriptReferenceHelper — all passed.

## Summary

Close plan **069** — R1–R4 appear landed on `feat/holocron-port-phase-b` (extended by plans **224–278** for ReferenceSearchOptionsDialog and tag/template search). Verify, mark plan **069** complete, and sync plan **063**.

## Requirements

- R1. `IndoorMapBuildWalkmeshTests` asserts `BuildWalkmeshForRoom` → `BWMType.AreaModel`.
- R2. `ScriptReferenceHelper` centralizes script reference search + results dialog.
- R3. UTD/UTP/UTT script combo menus wire **Find References** via `ScriptReferenceHelper`.
- R4. Walkmesh tests use stub install (no real game).
- R5. Mark plan **069** `status: complete` with verification transcript.
- R6. Update plan **063** progress table with plan **069** closure note.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~IndoorMapBuildWalkmesh
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests
```

## Scope Boundaries

- Doc/plan closure only; no production code changes expected.
