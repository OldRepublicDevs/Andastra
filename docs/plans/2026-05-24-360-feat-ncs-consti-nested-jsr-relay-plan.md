---
title: "feat: ncs consti nested jsr relay strref"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-358-feat-ncs-consti-jsr-multi-arg-slot-plan.md
branch: feat/plan-360-nested-jsr-relay
---

# feat: NCS CONSTI nested JSR relay StrRef (plan 360)

## Summary

Plan **358** classifies direct JSR call-literals when caller push slot aligns with callee StrRef ACTION param. Extend to **one-hop nested JSR relays** — e.g. `main → relay(424242) → speak(s) → ActionSpeakStringByStrRef(s)` — by following CPTOPSP param load into a nested JSR before StrRef ACTION.

## Requirements

- R1. When callee loads a matching param via CPTOPSP and forwards via JSR (not ACTION), recurse into nested subroutine (depth cap **4**).
- R2. Single-arg relay pattern: `void relay(int s) { speak(s); }` with StrRef consumer at leaf.
- R3. Negative: nested relay to noop → `Unknown`.
- R4. Existing **80** NcsConsti tests pass; **+3** new tests (**83** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- One-hop (bounded-depth) nested JSR relay only; no full stack simulation.
