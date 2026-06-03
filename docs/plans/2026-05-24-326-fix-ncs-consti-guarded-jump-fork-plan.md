---
title: "fix: NCS CONSTI guarded JZ/JNZ jump fork"
type: fix
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-324-feat-ncs-consti-jump-fork-strref-scan-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# fix: NCS CONSTI guarded JZ/JNZ jump fork (plan 326)

## Summary

Plan **324** forked forward scan at every `JZ`/`JNZ` target, fixing `if (0) return; ActionSpeakStringByStrRef(n);` but causing false positives for `if (1) return; ActionSpeakStringByStrRef(n);` where the consumer is unreachable.

## Requirements

- R1. `TryReadConstIntImmediatelyBeforeJump` reads the nearest preceding `CONSTI` int push before a conditional jump.
- R2. Fork `JZ` target only when the constant is **0** (jump taken); fork `JNZ` target only when constant is **non-zero**.
- R3. Unconditional `JMP` still always forks.
- R4. Unknown/dynamic conditions: do not fork conditional targets (linear fall-through only) to avoid false-positive StrRef indexing.
- R5. Tests: `WhileZeroReturn` pass, `DeadReturnLocalStrRef` stays `StackStored`, **40** NcsConsti tests pass.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Constant-folded `if (literal)` only; dynamic conditions deferred to full stack simulation backlog.
