---
title: "feat: NCS CONSTI action-signature StrRef slot matching"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-303-feat-ncs-consti-opcode-context-disambiguation-plan.md
branch: feat/plan-305-consti-action-signature
---

# feat: NCS CONSTI action-signature StrRef slot matching (plan 305)

## Summary

Deepen plan **303** opcode-context heuristics by deriving StrRef-consumer ACTION metadata from `ScriptDefs.KOTOR_FUNCTIONS` and matching CONSTI operands to the correct StrRef parameter push slot in multi-argument ACTION runs.

## Requirements

- R1. Build `actionId -> StrRef param indices` map from `ScriptDefs` (`nStrRef` / `strRef` int params).
- R2. Walk backward/forward to collect the CONSTI run before an ACTION; map CONSTI index to NWScript push slot.
- R3. Only classify CONSTI as `StrRefConsumer` when it occupies a StrRef parameter slot (not sibling CONSTIs like `TALKVOLUME_*`).
- R4. Keep slow-path `ExtractConstiOffsetsForValue` unchanged.
- R5. Tests: `BarkString(OBJECT_SELF, smallStrRef)` indexed; `ActionSpeakStringByStrRef(strRef, TALKVOLUME_SHOUT)` indexes strRef only.
- R6. Update plan **063** / KB for action-signature landing.

## Scope Boundaries

- CONSTI-only run indexing; non-CONSTI args (objects, stack loads) inferred via param count only.
- No full stack simulation.

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
