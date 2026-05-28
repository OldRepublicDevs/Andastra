---
title: "docs: consti strref vs 2da-memory disambiguation partial landing"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
---

# docs: CONSTI StrRef vs 2DA-memory disambiguation partial landing (plan 292)

## Summary

Document the **partially landed** generic NCS CONSTI disambiguation from plan **063** deferred backlog: `NcsConstiScanner.StrRefCandidateMinimum` filters low CONSTI values from cache indexing while explicit StrRef queries still match via slow path. Opcode-context 2DA-memory detection remains deferred.

## Requirements

- R1. Add KB verification row in `docs/knowledgebase/30-product-ux/odytools-editor-ux.md` for CONSTI threshold disambiguation (plans **086**, **095**, **099**, existing tests).
- R2. Update plan **063** deferred section: mark threshold-based disambiguation **partially landed**; opcode-context 2DA-memory still deferred.
- R3. Update plan **068** slice note for plan **292**.
- R4. No production code changes.

## Verification

- Read updated KB and parent plan files.
- Existing tests: `StrRefReferenceCache_SmallConsti_IsNotIndexed`, `FindStrRefReferences_SmallConstiSlowPath_StillFindsLiteral`, `IsPlausibleStrRefCandidate_*`.

## Scope Boundaries

- Doc-only; does not implement opcode-context CONSTI classification.
