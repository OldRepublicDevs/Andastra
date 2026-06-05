---
title: "test: ncs consti five-hop mixed const cptopsp jsr relay"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-05-24-383-feat-ncs-consti-four-hop-mixed-const-relay-plan.md
branch: feat/plan-411-five-hop-mixed-const-relay
---

# test: NCS CONSTI five-hop mixed CONST+CPTOPSP JSR relay (plan 411)

## Summary

PR **#70** (plan **383**) adds four-hop mixed CONST+CPTOPSP relay (`speak(0, s)` callee). Extend to **five-hop mixed** (`main → outer → relay → mid → mid2 → inner → speak(0, s)`) by raising `MaxNestedJsrRelayDepth` to **5** (if not already on `master` via PR **#79**) and adding **2** characterization tests.

## Requirements

- R1. `MaxNestedJsrRelayDepth = 5` when five-hop mixed tests require it.
- R2. Five-hop mixed relay StrRef on second param → `StrRefConsumer`.
- R3. `StrRefReferenceCache` indexes the StrRef for the same NSS pattern.
- R4. Index plan **411** in README; refresh tracker Step 3b count.

## Scope Boundaries

- Does not duplicate PR **#70** (four-hop mixed), PR **#79** (five-hop pure multi-arg), or PR **#77** (arithmetic relay).
- Test + depth-ceiling slice only unless tests fail without scanner change.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopMixedConstCptopspRelayStrRef
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
