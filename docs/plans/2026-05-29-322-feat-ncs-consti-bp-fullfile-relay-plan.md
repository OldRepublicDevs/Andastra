---
title: "feat: NCS CONSTI BP full-file relay trace"
type: feat
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-321-feat-ncs-consti-bp-multihop-local-strref-plan.md
branch: feat/plan-321-ncs-consti-bp-multihop
---

# feat: NCS CONSTI BP full-file relay trace (plan 322)

## Summary

Plan **321** added CPTOPBP relay hops in the forward scan only. Cross-subroutine patterns like `g = StrRef; sub1() { int m = g; int k = m; ActionSpeakStringByStrRef(k); }` fall through to `TryFindStrRefConsumerViaBpReload`, which matched CPTOPBP but did not recurse when the load was not consumed directly by ACTION.

## Requirements

- R1. In `TryFindStrRefConsumerViaBpReload`, after matching `CPTOPBP` without StrRef ACTION consumer, scan forward for next stack store and recurse `TryFindStrRefConsumerViaStackReload`.
- R2. Use `GetInstructionStepSizeAt` for BP full-file walks (align plan **319** unknown-opcode 2-byte step).
- R3. Tests: cross-sub `g→m→k→ACTION` context + cache.
- R4. **37** NcsConsti tests pass (35 baseline + 2 cross-sub BP relay).
- R5. Update plan **063** note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- BP full-file relay only; no full stack simulation.
