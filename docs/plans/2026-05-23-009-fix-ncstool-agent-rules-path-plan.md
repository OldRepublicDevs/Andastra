---
title: "fix: ncstool path in agent rules"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/90-meta/caveat-register.md C11
---

# fix: ncstool path in agent rules

## Summary

Correct agent rule files to cite `helper_scripts/NcsTool.ps1` (the actual path) and resolve caveat C11 plus the Agent Rules Path Drift register entry.

---

## Problem Frame

`.cursorrules` and `.github/copilot-instructions.md` instruct agents to use `scripts/NcsTool.ps1`, but the script lives at `helper_scripts/NcsTool.ps1`. KB docs already use the correct path and document the drift as C11.

---

## Requirements

- R1. Update `.cursorrules` NSS/NCS rule to `helper_scripts/NcsTool.ps1`
- R2. Update `.github/copilot-instructions.md` to match
- R3. Remove stale path-drift notes from KB docs that reference C11 as open
- R4. Resolve C11 in caveat register and Agent Rules Path Drift in documentation drift register

---

## Scope Boundaries

- Moving or renaming `helper_scripts/NcsTool.ps1` — out of scope
- OdyTools/OdyPatch build fixes — out of scope

---

## Implementation Units

- U1. **Fix agent rule paths**

**Goal:** Align authoritative agent instructions with repo layout.

**Requirements:** R1, R2

**Files:**
- Modify: `.cursorrules`
- Modify: `.github/copilot-instructions.md`

**Test scenarios:**
- Test expectation: none — config/doc path correction

**Verification:**
- No remaining `scripts/NcsTool.ps1` in rule files

- U2. **Close KB drift tracking**

**Goal:** Mark C11 resolved and remove redundant path-drift callouts.

**Requirements:** R3, R4

**Dependencies:** U1

**Files:**
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- Modify: `docs/knowledgebase/90-meta/caveat-register.md`
- Modify: `docs/knowledgebase/20-domain-theory/ncs-nwscript-vm.md`
- Modify: `docs/knowledgebase/10-architecture-runtime/tools-ecosystem.md`
- Modify: `docs/knowledgebase/50-execution/run-tools-reference.md`

**Verification:**
- C11 marked resolved; drift register Agent Rules section marked resolved

---

## Sources & References

- `helper_scripts/NcsTool.ps1` (sole NcsTool location)
- Caveat C11, drift register Agent Rules Path Drift
