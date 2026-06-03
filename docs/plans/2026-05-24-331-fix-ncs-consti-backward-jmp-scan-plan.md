---
title: "fix: NCS CONSTI backward JMP scan termination"
type: fix
status: complete
date: 2026-06-03
completed: 2026-06-03
origin: docs/plans/2026-05-24-330-test-ncs-consti-while-zero-if-live-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# fix: NCS CONSTI backward JMP scan termination (plan 331)

## Summary

Probe of `while (1) { if (0) return; } ActionSpeakStringByStrRef(n);` hangs `GetConstiUsageContext` because `TryFindStrRefConsumerViaStackReloadFromScan` recursively follows backward unconditional `JMP` loop edges without a visited guard. Fix by forking only **forward** `JMP` targets; treat backward `JMP` as a scan terminator on that path. Add regression tests for four control-flow probes (one fix, three confirmations).

## Problem Frame

Plans **324–330** hardened conditional jump fork/continue heuristics. A live `while (1)` with a dead inner `if (0) return` still emits a backward `JMP` to the loop head. Recursing into that target re-enters the same edge indefinitely.

## Requirements

- R1. In `TryFindStrRefConsumerViaStackReloadFromScan`, follow `JMP` (0x1D) fork only when `jumpTarget > scanOffset` (forward edge).
- R2. Backward `JMP` must not recurse; linear scan may still advance past the instruction when fall-through layout permits.
- R3. `GetConstiUsageContext_WhileOneDeadIfReturnLocalStrRef_ReturnsStrRefConsumer` — completes without hang; linear scan finds bytecode-order consumer after loop (known unreachable-code limitation).
- R4. Regression probes (no scanner change expected beyond R1–R2):
  - `GetConstiUsageContext_DoWhileBreakLocalStrRefViaCptopsp_ReturnsStrRefConsumer`
  - `GetConstiUsageContext_DeadForBodyLocalStrRef_RemainsStackStored`
  - `GetConstiUsageContext_NestedDeadIfReturnLocalStrRefViaCptopsp_ReturnsStrRefConsumer`
- R5. **52** NcsConsti tests pass (48 baseline + 4 new).

## Key Technical Decisions

- **Forward-only JMP fork:** Matches plan **324** intent (explore alternate paths) while avoiding loop back-edge cycles without a full CFG.
- **Classification:** Post-loop consumer may still classify as `StrRefConsumer` via linear fall-through (same layout as `while`+`break`); hang fix is the primary deliverable.

## Implementation Units

### U1. Probe tests (test-first)

**Goal:** Lock expected behavior for four NSS probes before/after scanner fix.

**Files:** `tests/BioWare.Tests/NcsConstiScannerTests.cs`

**Test scenarios:**

- Happy: do-while break then consumer → `StrRefConsumer`.
- Happy: nested `if (0) { if (0) return; }` then consumer → `StrRefConsumer`.
- Edge: dead `for` body consumer → `StackStored`.
- Edge: infinite `while (1)` with dead inner return; consumer after loop → completes without hang; `StrRefConsumer` via linear scan.

**Verification:** Filtered NcsConsti test run; while-one-dead-if must finish within test timeout.

### U2. Scanner backward-JMP guard

**Goal:** Stop infinite recursion on loop back-edges.

**Files:** `src/BioWare/Tools/NcsConstiScanner.cs`

**Approach:** In the `0x1D` branch of `TryFindStrRefConsumerViaStackReloadFromScan`, require `jumpTarget > scanOffset` before recursive fork.

**Patterns to follow:** Plan **329** `ShouldContinueLinearAfterConditionalJump` / plan **324** forward fork.

**Verification:** All NcsConsti tests green; probe program for while-one-dead-if returns promptly.

## Scope Boundaries

### In scope

- SP forward scan `JMP` fork guard and four regression tests.

### Deferred to Follow-Up Work

- Full CFG or visited-set simulation for complex loop nests.
- PR body refresh (only if all probes pass without scanner changes — not applicable; scanner fix required).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
