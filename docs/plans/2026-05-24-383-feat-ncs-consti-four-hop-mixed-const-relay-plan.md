---
title: "feat: ncs consti four-hop mixed const cptopsp jsr relay"
type: feat
status: active
date: 2026-06-05
origin: docs/knowledgebase/90-meta/pr-merge-readiness.md
branch: feat/plan-383-four-hop-mixed-const-relay
---

# feat: NCS CONSTI four-hop mixed CONST/CPTOPSP nested JSR relay (plan 383)

## Summary

Plan **370** covers three-hop mixed CONST+CPTOPSP relay (`relay → mid → inner → speak(0,s)`). Plan **374** covers four-hop symmetric multi-arg relay (`outer → relay → mid → inner → speak(a,s)`). Fill the gap with **four-hop mixed** at relay depth 4 (`outer → relay → mid → inner → speak(0,s)`).

## Problem Frame

The bounded nested JSR relay arc (plan **362**, `MaxNestedJsrRelayDepth = 4`) has characterization coverage for two- and three-hop mixed CONST/CPTOPSP push runs, and for four-hop multi-arg relays, but not for four-hop mixed CONST+CPTOPSP chains. This slice closes that gap with test-only coverage.

## Requirements

- R1. Four-hop relay where the innermost nested call uses CONST for first arg and CPTOPSP for StrRef param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef.
- R3. Existing NcsConsti tests pass; **+2** new tests.

## Key Technical Decisions

- Mirror the three-hop mixed NSS fixture extended with an `outer` hop; no scanner changes unless tests fail.
- Use `targetStrRef = 424242` and `BioWareGame.K1` compile path consistent with plans **370** and **374**.

## Implementation Units

### U1. Add four-hop mixed CONST/CPTOPSP relay characterization tests

**Goal:** Characterize scanner and cache behavior for `outer → relay → mid → inner → speak(0,s)`.

**Requirements:** R1, R2, R3

**Files:**
- `tests/BioWare.Tests/NcsConstiScannerTests.cs`

**Approach:** Add paired tests after the three-hop mixed block:
- `GetConstiUsageContext_FourHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer`
- `StrRefReferenceCache_FourHopMixedConstCptopspRelayStrRef_IsIndexed`

**Patterns to follow:** `GetConstiUsageContext_ThreeHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer` and `StrRefReferenceCache_ThreeHopMixedConstCptopspRelayStrRef_IsIndexed`.

**Test scenarios:**
- Happy path: four-hop mixed chain resolves to `ConstiUsageContext.StrRefConsumer`.
- Integration: `StrRefReferenceCache.ScanResource` indexes the target StrRef from compiled NCS bytes.

**Verification:**
```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only slice unless scanner bug discovered.
- No AgentDecompile work — characterization only.

### Deferred to Follow-Up Work

- Full CONSTI stack simulation for exotic control-flow (plan **063** backlog).
- Module Designer / OdyPatch E2E slices per pr-merge-readiness tracker.
