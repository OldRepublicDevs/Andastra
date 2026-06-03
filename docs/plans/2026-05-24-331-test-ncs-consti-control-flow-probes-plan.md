---
title: "test: NCS CONSTI remaining control-flow probes"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-330-test-ncs-consti-while-zero-if-live-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI remaining control-flow probes (plan 331)

## Summary

Add four CONSTI control-flow regression probes not yet covered after plan **330**. Each probe compiles NSS to NCS, locates the local StrRef CONSTI, and asserts `GetConstiUsageContext` classifies it as `StrRefConsumer` (live code after the control-flow construct). Fix `NcsConstiScanner` only if a probe fails.

## Requirements

- R1. `GetConstiUsageContext_DoWhileBreakLocalStrRefViaCptopsp_ReturnsStrRefConsumer` — `do { break; } while (1); ActionSpeakStringByStrRef(n);`
- R2. `GetConstiUsageContext_WhileInfiniteDeadReturnLocalStrRef_ReturnsStrRefConsumer` — `while (1) { if (0) return; } ActionSpeakStringByStrRef(n);`
- R3. `GetConstiUsageContext_DeadForBodyLocalStrRef_RemainsStackStored` — `for (i = 0; i < 0; i++) { ActionSpeakStringByStrRef(n); }` (never-entered body stays stack-stored)
- R4. `GetConstiUsageContext_NestedDeadIfReturnLocalStrRef_ReturnsStrRefConsumer` — `if (0) { if (0) return; } ActionSpeakStringByStrRef(n);`
- R5. **52** NcsConsti tests pass (48 existing + 4 new)

## Key Technical Decisions

- Test-first: add probes before any scanner change; scanner edits only on failure.
- Mirror existing `GetConstiUsageContext_*` patterns in `NcsConstiScannerTests.cs` (compile NSS, extract CONSTI, assert context).
- Fallback path (all probes pass without scanner edits): update PR #36 body and tracker for plans 324–331.

## Implementation Units

### U1. Add four control-flow probe tests

**Goal:** Lock in behavior for do-while break, infinite while with dead inner return, never-entered for body, and nested dead-if return.

**Files:** `tests/BioWare.Tests/NcsConstiScannerTests.cs`

**Approach:** Insert four `[Test]` methods after plan 330 tests, using `targetStrRef = 424242` and the NSS snippets from R1–R4.

**Test scenarios:**

- Happy path (R1, R2, R4): live consumer after control flow → `StrRefConsumer`.
- Edge case (R3): never-entered for body → `StackStored`.

**Verification:** `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` — expect 52 passed.

### U2. Fix scanner if probes fail (conditional)

**Goal:** Restore correct forward-scan / dead-branch handling for failing probe bytecode shapes.

**Files:** `src/BioWare/Tools/NcsConstiScanner.cs` (only if U1 tests fail)

**Dependencies:** U1

**Verification:** Same test filter — 52 passed.

### U3. Sync PR #36 tracker (conditional fallback or completion)

**Goal:** Document plans 324–331 on open PR #36 and `pr-merge-readiness.md`.

**Files:** `docs/knowledgebase/90-meta/pr-merge-readiness.md`

**Dependencies:** U1, U2 (if any)

**Verification:** Tracker lists plan 331; test count reflects 52 NcsConsti tests.

## Scope Boundaries

- No AgentDecompile work (NCS scanner slice).
- No HoloPatcher references.
- Deferred: additional control-flow shapes beyond the four listed probes.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
