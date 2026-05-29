---
title: "feat: NCS CONSTI deep multi-hop local StrRef trace"
type: feat
status: in_progress
date: 2026-05-29
origin: docs/plans/2026-05-29-313-feat-ncs-consti-multihop-local-strref-plan.md
branch: feat/plan-317-deep-multihop-local-strref
---

# feat: NCS CONSTI deep multi-hop local StrRef trace (plan 317)

## Summary

Extend plan **313** relay tracing to support **three+ hop** local chains (`n→m→k→ACTION`) and non-adjacent `CPDOWNSP` after `CPTOPSP` by instruction-aligned relay discovery instead of requiring `CPTOPSP` immediately followed by `CPDOWNSP`.

## Requirements

- R1. After matching `CPTOPSP` reload without StrRef ACTION consumer, scan forward (bounded) for next `CPDOWNSP` at instruction boundaries and recurse.
- R2. Preserve plan **309** R6: arithmetic relay (`n+1`) remains `StackStored`.
- R3. Tests: `n→m→k→ActionSpeakStringByStrRef(k)` context + cache.
- R4. **33** NcsConsti tests pass (31 baseline + 2 three-hop).
- R5. Update plan **063** note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Stack-local `CPDOWNSP`/`CPTOPSP` only; no BP multi-hop.
- Bounded relay window; no full stack simulation.
