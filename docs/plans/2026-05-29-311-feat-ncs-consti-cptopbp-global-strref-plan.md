---
title: "feat: NCS CONSTI global StrRef CPTOPBP cross-subroutine trace"
type: feat
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-311-cptopbp-global-strref
---

# feat: NCS CONSTI global StrRef CPTOPBP cross-subroutine trace (plan 311)

## Summary

Extend plan **309** variable StrRef tracing for **global** `CPDOWNBP` → `CPTOPBP` flows where the compiler places the load subroutine **before** the store in bytecode (e.g. `g = 424242; sub1();` with `ActionSpeakStringByStrRef(g)` in `sub1`). Same-subroutine BP reload already worked; this slice adds full-file BP offset matching.

## Brainstorm

| Approach | Pros | Cons | Decision |
|----------|------|------|----------|
| **Full-file CPTOPBP match (chosen)** | Handles out-of-order subs; minimal diff | Theoretical false positive if same BP offset reused | **Land v1** |
| Forward-only scan | Safer for stack locals | Misses cross-subroutine globals | **Keep for CPDOWNSP** |
| Full stack/BP simulation | Complete | High complexity | **Deferred** |

## Requirements

- R1. After forward `TryFindStrRefConsumerViaStackReload` fails on `CPDOWNBP`, scan entire NCS for `CPTOPBP` with matching offset/size.
- R2. Reuse `TryGetActionArgumentRunFrom` + `IsConstiAtStrRefParameterSlot` for ACTION validation.
- R3. `CPDOWNSP` path unchanged (forward scan only).
- R4. Tests: same-sub global (`g` in `main`), cross-sub (`sub1` consumes `g` assigned in `main`).
- R5. **27** NcsConsti tests pass; update plan **063**.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
