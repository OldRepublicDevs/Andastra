---
title: "feat: NCS CONSTI BP multi-hop local StrRef relay"
type: feat
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-319-refactor-ncs-instruction-size-hardening-plan.md
branch: feat/plan-321-ncs-consti-bp-multihop
---

# feat: NCS CONSTI BP multi-hop local StrRef relay (plan 321)

## Summary

Extend plan **317** relay tracing to **CPTOPBP** loads: when a global `CPDOWNBP` CONSTI is copied through `CPTOPBP` → `CPDOWNSP`/`CPDOWNBP` relay chains into a StrRef ACTION, classify as `StrRefConsumer`. Plan **317** only recursed relay hops after `CPTOPSP` (0x03), not `CPTOPBP` (0x27).

## Requirements

- R1. After matching `CPTOPBP` reload without StrRef ACTION consumer, scan forward (bounded) for next `CPDOWNSP` or `CPDOWNBP` and recurse `TryFindStrRefConsumerViaStackReload`.
- R2. Add `TryFindNextCpdownbpAfterLoad` mirroring `TryFindNextCpdownspAfterLoad` for BP relay stores.
- R3. Tests: global `g = StrRef; int m = g; ActionSpeakStringByStrRef(m)` context + cache.
- R4. **35** NcsConsti tests pass (33 baseline + 2 BP multi-hop).
- R5. Update plan **063** CONSTI note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Relay discovery only; no full stack simulation.
- Preserve plan **309** R6 arithmetic relay exclusion.
