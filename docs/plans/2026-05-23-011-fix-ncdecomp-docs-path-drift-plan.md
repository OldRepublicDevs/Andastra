---
title: "fix: ncdecomp docs bioware path drift"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md NCSDecomp paths
---

# fix: ncdecomp docs bioware path drift

## Summary

Replace obsolete `src/CSharpKOTOR/Formats/NCS/NCSDecomp` references in seven `docs/NCSDecomp_*.md` investigation docs with the current BioWare layout (`src/BioWare/Resource/Formats/NCS/Decomp`) and update the drift register.

---

## Problem Frame

NCSDecomp porting/verification docs still cite the pre-Andastra `CSharpKOTOR` tree. Decompiler code lives under BioWare since the library consolidation.

---

## Requirements

- R1. Replace path strings in all `docs/NCSDecomp_*.md` files
- R2. Replace namespace references `CSharpKOTOR.Formats.NCS.NCSDecomp` → `BioWare.Resource.Formats.NCS.Decomp`
- R3. Correct directory tree in `NCSDecomp_Systematic_Verification.md` to match repo layout
- R4. Mark NCSDecomp drift entry resolved in documentation drift register

---

## Scope Boundaries

- `REVERSE_ENGINEERING_PROJECT_SUMMARY.md` — deferred (C1)
- Rewriting verification claims — out of scope; paths only

---

## Implementation Units

- U1. **Bulk path replacement in NCSDecomp docs**

**Files:** seven `docs/NCSDecomp_*.md`

**Verification:** `rg CSharpKOTOR docs/NCSDecomp` returns no matches

- U2. **Update drift register**

**Files:** `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`

---

## Sources & References

- `src/BioWare/Resource/Formats/NCS/Decomp/`
- Drift register Obsolete Paths row
