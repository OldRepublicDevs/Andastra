---
title: "feat: ncs consti jsr call-literal strref consumer"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-356-jsr-call-literal-strref
---

# feat: NCS CONSTI JSR call-literal StrRef consumer (plan 356)

## Summary

`GetConstiUsageContext` returns `Unknown` for `speak(424242)` patterns where a CONSTI literal is passed as a subroutine argument and consumed by a StrRef ACTION in the callee. Classify as `StrRefConsumer` via bounded JSR forward trace.

## Requirements

- R1. `TryFindStrRefConsumerViaJsrCall`: CONSTI push run → JSR → callee CPTOPSP/CPTOPBP → StrRef ACTION within window.
- R2. Hook in `GetConstiUsageContext` before `Unknown`.
- R3. Tests: context + cache indexed for call-literal StrRef; negative for non-StrRef callee.
- R4. All **74+** NcsConsti tests pass.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- No full stack simulation; JSR + callee-entry heuristic only.
