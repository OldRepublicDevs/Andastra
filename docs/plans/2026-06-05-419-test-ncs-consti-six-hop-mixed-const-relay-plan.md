---
title: "test: NCS CONSTI six-hop mixed CONST+CPTOPSP JSR relay"
type: test
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-411-test-ncs-consti-five-hop-mixed-const-relay-plan.md
branch: feat/plan-419-ncs-consti-six-hop-mixed-const-relay
---

# test: NCS CONSTI six-hop mixed CONST+CPTOPSP JSR relay (plan 419)

## Summary

PR **#80** (plan **411**) adds five-hop mixed CONST+CPTOPSP relay (`inner` calls `speak(0, s)`). PR **#87** (plan **418**) raises `MaxNestedJsrRelayDepth` to **6** for six-hop symmetric multi-arg relay. This slice stacks on **#87**, raises depth to **7** for the extra `mid2` mixed-relay layer, and adds six-hop mixed tests (`root→deepest→outer→relay→mid→mid2→inner→speak(0,s)`) with **+2** NcsConsti characterization tests.

## Problem Frame

The NCS CONSTI nested JSR relay arc characterizes how `StrRefReferenceCache` traces StrRef constants through multi-level subroutine calls. Mixed relay (CONST first arg + CPTOPSP second arg at the leaf callee) is a distinct push pattern from symmetric multi-arg relay. Five-hop mixed is covered by plan **411**; six-hop mixed completes the mixed-relay depth ladder at `MaxNestedJsrRelayDepth = 7`.

## Requirements

- R1. `MaxNestedJsrRelayDepth` **6 → 7** (six-hop mixed adds `mid2` vs six-hop multi-arg).
- R2. Six-hop mixed relay NSS: `root→deepest→outer→relay→mid→mid2→inner→speak(0,s)` → `StrRefConsumer`.
- R3. `GetConstiUsageContext_SixHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer` and matching `StrRefReferenceCache` indexing test.
- R4. **107** NcsConsti tests pass (105 from plan **418** + 2).
- R5. Index plan **419** in `docs/plans/README.md`; refresh tracker Step 3b count when applicable.

## Key Technical Decisions

- **Stack on plan 418 branch:** extend `MaxNestedJsrRelayDepth` to **7** — six-hop mixed adds a `mid2` relay layer vs six-hop symmetric multi-arg, so depth **6** is insufficient.
- **NSS pattern:** Mirror plan **418** six-hop multi-arg topology (`root→deepest→outer→relay→mid→inner→speak`) with leaf callee `speak(0, s)` instead of `speak(a, s)` — do not add `mid2` (that would be 7 JSR hops and exceed `MaxNestedJsrRelayDepth = 6`).
- **No scanner change unless tests fail:** Same posture as plans **370**, **411**, and **418** characterization slices.

## Implementation Units

### U1. Plan document and README index

**Goal:** Land plan **419** and register it in the plans index.

**Files:**
- `docs/plans/2026-06-05-419-test-ncs-consti-six-hop-mixed-const-relay-plan.md`
- `docs/plans/README.md`

**Test scenarios:** Test expectation: none — documentation only.

**Verification:** Plan file exists; README row for **419** present.

### U2. Six-hop mixed CONST relay characterization tests

**Goal:** Add two NcsConsti tests proving six-hop mixed relay resolves to `StrRefConsumer` and indexes in `StrRefReferenceCache`.

**Dependencies:** U1

**Files:**
- `tests/BioWare.Tests/NcsConstiScannerTests.cs`

**Approach:** Insert tests adjacent to existing five-hop mixed and six-hop multi-arg tests. NSS:

```text
void speak(int a, int s) { ActionSpeakStringByStrRef(s); }
void inner(int a, int s) { speak(0, s); }
void mid(int a, int s) { inner(a, s); }
void relay(int a, int s) { mid(a, s); }
void outer(int a, int s) { relay(a, s); }
void deepest(int a, int s) { outer(a, s); }
void root(int a, int s) { deepest(a, s); }
void main() { root(99, <targetStrRef>); }
```

**Test scenarios:**
- Happy path: `GetConstiUsageContext` returns `StrRefConsumer` for the main CONSTI at offset of `targetStrRef`.
- Integration: `StrRefReferenceCache.ScanResource` reports `HasReferences(targetStrRef) == true`.

**Verification:** Filter `FullyQualifiedName~SixHopMixedConstCptopspRelayStrRef` passes; full `FullyQualifiedName~NcsConsti` count is **107**.

### U3. Scanner depth adjustment (conditional)

**Goal:** Only if U2 tests fail — raise or fix relay tracing in `NcsConstiScanner`.

**Dependencies:** U2

**Files:**
- `src/BioWare/Tools/NcsConstiScanner.cs`

**Test scenarios:** Re-run U2 tests after any scanner change.

**Verification:** All NcsConsti tests green.

## Scope Boundaries

### In scope

- Two characterization tests and plan/README sync.
- Conditional scanner fix if probes fail.

### Deferred to Follow-Up Work

- Merge PR **#80** (plan **411**) and PR **#87** (plan **418**) before or after this PR lands — independent open PRs.
- Seven-hop relay (would require raising `MaxNestedJsrRelayDepth` beyond 6).
- Field-value arc (plans **412–417**, PRs **#81–#86**).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SixHopMixedConstCptopspRelayStrRef
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
