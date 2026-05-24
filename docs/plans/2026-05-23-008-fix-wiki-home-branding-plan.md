---
title: "fix: wiki home andastra/odypatch branding"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md remediation #4
---

# fix: wiki home andastra/odypatch branding

## Summary

Rebrand `wiki/Home.md` from PyKotor/HoloPatcher to Andastra/OdyPatch, replace broken HoloPatcher wiki links with authoritative repo paths (KB + OdyPatch README), and update `wiki/README.txt` plus drift registers. Does not copy the vendor wiki corpus.

---

## Problem Frame

`wiki/Home.md` titles itself "PyKotor Wiki" and links to HoloPatcher pages that do not exist under `wiki/` (only in `vendor/`). This misleads contributors and conflicts with OdyPatch-only policy.

---

## Requirements

- R1. Retitle and reintroduce Home as Andastra wiki with PyKotor heritage note
- R2. Replace HoloPatcher end-user/mod-dev links with OdyPatch + KB paths
- R3. Fix or replace broken TSLPatcher syntax links (pages absent from `wiki/`) with vendor doc paths or KB pointers
- R4. Update Internal Documentation and tool references (HoloPatcher → OdyPatch; note Andastra in vendor section)
- R5. Update `wiki/README.txt` and mark drift item #4 partially/fully resolved in KB

---

## Scope Boundaries

- Copying 450+ vendor wiki files into `wiki/` — deferred
- Rewriting all format pages' PyKotor references — deferred
- OdyTools/OdyPatch build fixes — out of scope

---

## Implementation Units

- U1. **Rebrand wiki/Home.md**

**Goal:** Correct title, mod-tool links, internal docs, and broken syntax link targets.

**Requirements:** R1–R4

**Files:**
- Modify: `wiki/Home.md`

**Approach:**
- New header: Andastra Wiki; note format docs inherited from PyKotor wiki lineage
- End users: link to `docs/knowledgebase/50-execution/run-tools-reference.md` and `src/Tools/OdyPatch/README.md`
- Mod developers: link to `docs/knowledgebase/20-domain-theory/tslpatcher-domain.md` and vendor TSLPatcher syntax paths where no `wiki/` page exists
- Internal docs: OdyPatch README + KB, not HoloPatcher wiki stubs
- Vendor section: add Andastra as primary .NET runtime/tools stack

**Test scenarios:**
- Test expectation: none — documentation-only

**Verification:**
- No "HoloPatcher" in Home.md navigation sections (body historical mentions may note legacy)
- No "PyKotor Wiki" title

- U2. **Update wiki README and KB registers**

**Goal:** Align wiki README and close drift tracking.

**Requirements:** R5

**Dependencies:** U1

**Files:**
- Modify: `wiki/README.txt`
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- Modify: `docs/knowledgebase/90-meta/caveat-register.md` (C10 partial note)

**Verification:**
- Drift item #4 marked done for Home.md; vendor corpus remains noted as deferred

---

## Sources & References

- `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- `docs/knowledgebase/20-domain-theory/tslpatcher-domain.md`
