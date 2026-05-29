---
title: "feat: NCS CONSTI variable StrRef CPTOPSP forward trace"
type: feat
status: complete
date: 2026-05-28
completed: 2026-05-29
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-309-cptopsp-variable-strref
---

# feat: NCS CONSTI variable StrRef CPTOPSP→ACTION forward trace (plan 309)

## Summary

Index CONSTI literals that flow through stack locals into StrRef ACTION parameters — e.g. `int n = 424242; ActionSpeakStringByStrRef(n);` — without full stack simulation. Plan **307** excluded all `StackStored` CONSTI; this slice reclassifies store→reload→ACTION patterns as `StrRefConsumer`.

## Brainstorm — design alternatives

### A. Full stack simulation (defer)

Walk every instruction, maintain virtual SP/BP, resolve all stack slots at ACTION boundaries.

| Pros | Cons |
|------|------|
| Handles arbitrary control flow, structs, multi-slot spills | High complexity; overlaps NCS decompiler/interpreter; out of scope for reference-cache heuristics |
| Future-proof for exotic patterns | Risk of false StrRef positives on non-StrRef int locals |

**Verdict:** Deferred per plan **063** backlog; not chosen for this slice.

### B. Forward CPTOPSP trace after CPDOWNSP store (chosen)

When CONSTI is immediately followed by `CPDOWNSP`/`CPDOWNBP`, record stack offset + size. Scan forward (bounded window) for matching `CPTOPSP`/`CPTOPBP`, then walk the ACTION argument run from the load instruction, linking the reloaded slot to the original CONSTI offset.

| Pros | Cons |
|------|------|
| Minimal diff; reuses `StrRefParamIndicesByActionId` + slot matching from plan **305** | Misses patterns with intervening unmatched stack ops |
| Matches compiler output for `int n = X; Action(..., n)` | 128-byte window may miss distant reloads (acceptable) |
| Preserves plan **307** exclusion for unused locals | Multi-int-arg with only one variable StrRef needs slot alignment (handled via existing slot map) |

**Verdict:** Chosen — best cost/benefit for Holocron StrRef reference finder.

### C. Defer entirely (status quo)

Keep `StackStored` → never index; rely on slow-path exact StrRef queries only.

| Pros | Cons |
|------|------|
| Zero implementation risk | Cache misses common mod script pattern (local StrRef variable) |
| | Reference finder UX gap vs PyKotor expectations |

**Verdict:** Rejected — user-visible false negative for typical NSS.

## Requirements

- **R1.** When `GetConstiUsageContext` sees `StackStored` (`CPDOWNSP`/`CPDOWNBP` after CONSTI), call `TryFindStrRefConsumerViaStackReload` before returning `StackStored`.
- **R2.** Forward scan: bounded to **128 bytes** after store instruction; allow stepping through neutral opcodes (`MOVSP`, `NOP`, arithmetic with known sizes, jumps with known sizes).
- **R3.** Match `CPTOPSP`/`CPTOPBP` with same 4-byte copy size as the store; stack offset may differ after `MOVSP`/`DECxSP`/`INCxSP` — compare `loadOffset + stackPointerDelta == storeOffset`.
- **R4.** From matching load, `TryGetActionArgumentRunFrom` builds stack slots; reload slot links to original CONSTI `ValueByteOffset`; `IsConstiAtStrRefParameterSlot` confirms StrRef param index.
- **R5.** `ShouldIndexAsStrRefCandidate` returns true when reclassified as `StrRefConsumer` (including below `StrRefCandidateMinimum` when action signature confirms StrRef).
- **R6.** Unused local pattern (`int n = X; int m = n + 1`) remains `StackStored` / not indexed.
- **R7.** Tests: `ActionSpeakStringByStrRef(n)`, `BarkString(OBJECT_SELF, n)` cache-indexed; unused local unchanged; prior 20 NcsConsti tests still pass.
- **R8.** Update plan **063** deferred note when landed.

## NSS fixture examples

```nss
// R7 — should index 424242 in cache
void main() {
    int n = 424242;
    ActionSpeakStringByStrRef(n);
}

// R7 — second-arg StrRef via variable
void main() {
    int n = 50;
    BarkString(OBJECT_SELF, n);
}

// R6 — should NOT index (arithmetic consumer, not StrRef ACTION)
void main() {
    int n = 424242;
    int m = n + 1;
}
```

## Verification ladder

1. `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
2. `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti`
3. Expect **25** passed (20 baseline + 5 variable-StrRef cases)

## Scope boundaries

- Single-int StrRef ACTION args and second-arg StrRef (e.g. `BarkString`) via existing `ScriptDefs` map.
- No full stack simulation; no struct/multi-slot variable tracking beyond matching offset reload.
- No AgentDecompile / engine binary analysis (NCS scanner tooling only).
- CPDOWNBP/CPTOPBP supported symmetrically with CPDOWNSP/CPTOPSP.
