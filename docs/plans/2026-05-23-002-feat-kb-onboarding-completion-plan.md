---
title: "feat: Complete KB onboarding entry points"
type: feat
status: completed
date: 2026-05-23
origin: PR #2 test plan + documentation-drift-register
---

# feat: Complete KB onboarding entry points

## Summary

Repair broken README documentation links by adding `docs/QUICKSTART.md` and `docs/engine_roadmap.md`, and add a prominent link to the new knowledgebase. Update KB drift register entries when links are restored.

---

## Problem Frame

PR #2 delivered `docs/knowledgebase/` but README still links to missing `docs/QUICKSTART.md` and `docs/engine_roadmap.md` (C3 in caveat register). New contributors and agents hit 404-equivalent dead links from the primary entry point.

---

## Requirements

- R1. Create `docs/QUICKSTART.md` pointing at KB execution ladder (not duplicating full runbooks)
- R2. Create `docs/engine_roadmap.md` with engine-family maturity aligned to KB intent layer
- R3. Add README link to `docs/knowledgebase/90-meta/README.md`
- R4. Update `documentation-drift-register.md` and `caveat-register.md` for resolved C3 items
- R5. Validate with `git diff --check` and link checker

---

## Scope Boundaries

- No README architecture diagram rewrite (large drift — KB documents corrections)
- No CI workflow fixes
- No code changes

---

## Implementation Units

- U1. **QUICKSTART** — Create `docs/QUICKSTART.md`
- U2. **Engine roadmap** — Create `docs/engine_roadmap.md`
- U3. **README + drift updates** — Add KB link; fix drift register; update caveat C3

**Verification:** README links resolve; drift register reflects fixes; `git diff --check` clean.

---

## Sources & References

- `docs/knowledgebase/50-execution/build-and-test-ladder.md`
- `docs/knowledgebase/00-intent/engine-family-scope.md`
- `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
