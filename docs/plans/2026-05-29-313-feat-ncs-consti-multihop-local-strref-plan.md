---
title: "feat: NCS CONSTI multi-hop local StrRef trace"
type: feat
status: in_progress
date: 2026-05-29
origin: docs/plans/2026-05-29-312-chore-merge-pr25-post-merge-tracker-plan.md
branch: feat/plan-313-multihop-local-strref
---

# feat: NCS CONSTI multi-hop local StrRef trace (plan 313)

## Summary

Extend plan **309** forward stack reload so CONSTI flowing through chained locals reaches StrRef ACTION parameters — e.g. `int n = 424242; int m = n; ActionSpeakStringByStrRef(m);`. Plan **309** handles single-hop `n→ACTION`; this slice adds one relay hop `n→m→ACTION` on the stack (`CPDOWNSP`/`CPTOPSP` chain).

## Brainstorm

| Approach | Pros | Cons | Decision |
|----------|------|------|----------|
| **Chained forward hop (chosen)** | Minimal diff; preserves 128-byte window | Max 1 relay hop in v1 | **Land** |
| Full stack simulation | Complete | Deferred plan **063** | **Defer** |
| BFS over all stack slots | Multi-hop general | Over-engineered for v1 | **Defer** |

## Requirements

- R1. When `CPTOPSP` reload matches store offset but is not consumed by StrRef ACTION, detect immediate `CPDOWNSP` relay and continue forward scan from new stack offset.
- R2. Preserve plan **309** R6: `int m = n + 1` (arithmetic) remains `StackStored`.
- R3. `CPDOWNBP`/`CPTOPBP` multi-hop deferred (stack locals use SP path).
- R4. Tests: `n→m→ActionSpeakStringByStrRef(m)` context + cache; arithmetic relay still excluded.
- R5. **31** NcsConsti tests pass (29 baseline + 2 multi-hop).
- R6. Update plan **063** deferred note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Stack-local (`CPDOWNSP`/`CPTOPSP`) relay only; no BP global multi-hop.
- No full stack simulation.
