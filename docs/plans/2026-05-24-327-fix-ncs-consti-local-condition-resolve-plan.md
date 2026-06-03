---
title: "fix: NCS CONSTI local condition resolve and scan cutoff"
type: fix
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-326-fix-ncs-consti-guarded-jump-fork-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# fix: NCS CONSTI local condition resolve and scan cutoff (plan 327)

## Summary

Plan **326** guarded `JZ`/`JNZ` forks on literal `CONSTI` conditions but still false-positived `int x = 1; if (x) return; ActionSpeakStringByStrRef(n);` because linear scan followed `JMP` merge paths to the dead consumer. Variable `if (x)` uses `CPTOPSP` before `JZ`, not a literal push.

## Requirements

- R1. Resolve jump conditions from `CPTOPSP` → preceding `CPDOWNSP` → `CONSTI` when the compare loads a locally stored int.
- R2. After `JZ`/`JNZ` (whether forked or not), stop linear forward scan to avoid merge-path false positives.
- R3. Tests: variable `x=0` consumer path, variable `x=1` dead consumer stays `StackStored`; **42** NcsConsti tests pass.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Local literal initialization only; dynamic runtime values still conservative.
