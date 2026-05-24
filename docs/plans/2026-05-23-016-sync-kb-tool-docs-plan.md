---
title: "docs: Sync KB tool docs after C5 build recovery"
type: docs
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md
---

# docs: Sync KB tool docs after C5 build recovery

## Summary

Eliminate stale "OdyTools/OdyPatch Red" claims across KB execution and architecture layers so agents reading runbooks after plans 013–015 get accurate build status.

---

## Problem Frame

Plans 013–015 fixed OdyTools compile, standalone DialogHelper, and dotnet-desktop CI. `build-health-matrix.md`, `AGENTS.md`, and drift register are updated, but six KB files still describe OdyTools as broken and block OdyPatch UX work. `[REPO]`

---

## Requirements

- R1. No KB file under `docs/knowledgebase/` claims OdyTools or OdyPatch fail to compile (except historical resolved entries in caveat register).
- R2. Tool matrix and dependency diagrams show green compile status with runtime-UX-unverified caveat where appropriate.
- R3. Drift register adds remediation item 7 done.

---

## Scope Boundaries

- Do not author `30-product-ux/` layer content in this slice.
- Do not fix KotorCLI.

---

## Implementation Units

- U1. **Update execution runbooks**

**Requirements:** R1, R2

**Files:**
- Modify: `docs/knowledgebase/50-execution/run-tools-reference.md`
- Modify: `docs/knowledgebase/00-intent/definition-of-done.md`

---

- U2. **Update architecture and domain docs**

**Requirements:** R1, R2

**Files:**
- Modify: `docs/knowledgebase/10-architecture-runtime/tools-ecosystem.md`
- Modify: `docs/knowledgebase/10-architecture-runtime/solution-topology.md`
- Modify: `docs/knowledgebase/20-domain-theory/tslpatcher-domain.md`
- Modify: `docs/knowledgebase/90-meta/README.md`

---

- U3. **Record remediation in drift register**

**Requirements:** R3

**Files:**
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`

**Verification:**
- `rg "blocks OdyPatch|Build fails until OdyTools|OdyTools.*Red"` under `docs/knowledgebase/` returns only resolved/historical lines.

---

## Sources & References

- Plans 013–015
- `docs/knowledgebase/40-operational-risk/build-health-matrix.md`
