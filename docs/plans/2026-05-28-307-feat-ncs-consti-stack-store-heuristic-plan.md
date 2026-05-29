---
title: "feat: NCS CONSTI stack-store and CPTOPSP run-break heuristics"
type: feat
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-307-consti-stack-store-heuristic
---

# feat: NCS CONSTI stack-store heuristics (plan 307)

## Summary

Reduce StrRef cache false positives when CONSTI literals are stored to stack locals (`CPDOWNSP`/`CPDOWNBP`) or when `CPTOPSP`/`CPDOWNBP` breaks a direct ACTION push run. Slow-path StrRef queries remain exact-match.

## Requirements

- R1. `ConstiUsageContext.StackStored` when CONSTI is immediately followed by `CPDOWNSP` or `CPDOWNBP`.
- R2. `ShouldIndexAsStrRefCandidate` excludes `StackStored`.
- R3. `TryGetActionArgumentRun` aborts when stack copy/load opcodes appear before ACTION.
- R4. Tests: local `int` literal above threshold not cache-indexed; slow-path still finds it; direct StrRef ACTION unchanged.
- R5. Update plan **063** / KB deferred note.

## Scope Boundaries

- No full stack simulation; immediate-next-opcode and run-walk abort only.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
