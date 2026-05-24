---
title: "fix: re project summary andastra framing"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md C1
---

# fix: re project summary andastra framing

## Summary

Reframe `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` as a historical Phase 1 cross-binary investigation report and point readers to the Andastra .NET knowledgebase for current implementation status. Resolve caveat C1.

---

## Problem Frame

The RE summary presents KotOR.js/TypeScript as the primary engine deliverable. Andastra's active runtime is .NET under `src/Andastra/` and `src/BioWare/`. Binary findings remain valuable; framing must not mislead agents.

---

## Requirements

- R1. Add investigation-era banner with KB authority links
- R2. Reframe executive summary and deliverables — KotOR.js as vendor reference, not primary stack
- R3. Update implementation status / conclusion to reference Andastra C# paths and KB
- R4. Resolve C1 and drift register row; refresh stale repo-implications line in drift register

---

## Scope Boundaries

- Rewriting all binary analysis tables — keep as historical evidence
- OdyTools build fix — out of scope

---

## Implementation Units

- U1. **Reframe REVERSE_ENGINEERING_PROJECT_SUMMARY.md**

**Files:** `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md`

**Verification:** No claim that TypeScript/KotOR.js is the primary Andastra deliverable without historical qualifier

- U2. **Update KB registers**

**Files:** `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`, `docs/knowledgebase/90-meta/caveat-register.md`

---

## Sources & References

- `docs/knowledgebase/20-domain-theory/reverse-engineering-methodology.md`
- `docs/knowledgebase/90-meta/authority-map.md`
- Caveat C1
