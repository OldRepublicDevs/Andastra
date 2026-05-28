---
title: "docs: close plan 107 find-2da-ref --full-row"
type: docs
status: complete
completed: 2026-05-28
date: 2026-05-28
origin: docs/plans/2026-05-24-107-feat-kotorcli-find-2da-ref-full-row-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close plan 107 — KotorCLI find-2da-ref --full-row (plan 276)

## Completion (2026-05-28)

- Plan **107** marked `status: complete` with R1–R5 evidence table and verification transcript.
- Parent plans **063** / **068**: U6 progress row and KotorCLI `--full-row` parity note added.
- Tests: **13** `Find2DARef`, **2** `TwoDARow` — all passed.

## Summary

Plan **107** implementation is landed on `feat/holocron-port-phase-b`. This slice verifies requirements R1–R5, flips plan **107** to `status: complete`, and records verification commands. No production code changes expected.

## Requirements

- R1. Confirm `ReferenceCacheHelpers.CollectTwoDARowReferences` in `src/BioWare/Tools/ReferenceCache.cs`.
- R2. Confirm `TwoDAMemoryReferenceHelper.CollectTwoDARowReferences` delegates to BioWare.
- R3. Confirm KotorCLI `find-2da-ref --full-row` in `src/Tools/KotorCLI/Commands/Find2DARefCommand.cs`.
- R4. Run BioWare TwoDARow + KotorCLI Find2DARef filtered tests (expect pass).
- R5. Confirm `src/Tools/KotorCLI/README.md` documents `--full-row`.
- R6. Mark plan **107** `status: complete` with completion notes and verification transcript.
- R7. Update parent plans **063** / **068** with plan **107** closure note and test counts.

## Scope Boundaries

- Doc/plan status only; no AgentDecompile; do not commit `.cursor/hooks/`.

## Implementation Units

### U1 — Verify R1–R5

**Verification:**

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDARow
```

Expected: **13** KotorCLI Find2DARef tests, **2** BioWare TwoDARow tests, all passed.

### U2 — Close plan 107

**Files:**

- Modify: `docs/plans/2026-05-24-107-feat-kotorcli-find-2da-ref-full-row-plan.md`

**Changes:** `status: complete`, completion date, verification summary.

### U3 — Mark this plan complete

**Files:**

- Modify: `docs/plans/2026-05-28-276-docs-kotorcli-find-2da-ref-full-row-closure-plan.md`

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Stale `active` on plan 107 confuses agents | This closure plan + status flip |

## Documentation / Operational Notes

- AgentDecompile: skipped (tooling/doc closure only).
