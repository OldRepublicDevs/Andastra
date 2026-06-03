---
title: "feat: NCS CONSTI jump-fork forward scan for local StrRef"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/knowledgebase/90-meta/pr-merge-readiness.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# feat: NCS CONSTI jump-fork forward scan (plan 324)

## Summary

After `CPDOWNSP` stores a CONSTI to a local, `TryFindStrRefConsumerViaStackReload` linearly scans forward and accumulates `MOVSP` deltas. For `if (0) return; ActionSpeakStringByStrRef(n);`, the compiler emits `JZ` over a dead branch containing an extra `MOVSP`. The linear scan applies that unreachable `MOVSP`, so `CPTOPSP` offset matching fails and the CONSTI is misclassified as `StackStored`.

## Requirements

- R1. Resolve NWScript jump targets using the same formula as `NCSBinaryReader` (`relative + opcodeOffset`).
- R2. In `TryFindStrRefConsumerViaStackReload`, when encountering `JMP`/`JZ`/`JNZ`, also scan from the jump target with the current `stackPointerDelta` (fork, do not reset store metadata).
- R3. Retain linear fall-through for `JZ`/`JNZ` so both paths remain explored.
- R4. Add regression test `GetConstiUsageContext_EarlyReturnLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R5. All **38** NcsConsti tests pass (37 baseline + 1 early-return).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Jump-fork in SP forward scan only; no full CFG/stack simulation.
- Does not change BP full-file relay logic from plans 321–322.
