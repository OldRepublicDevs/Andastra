---
title: "feat: Build Andastra evidence-first knowledgebase"
type: feat
status: completed
date: 2026-05-23
origin: build-knowledgebase skill + kb-orchestrator orchestration
---

# feat: Build Andastra evidence-first knowledgebase

## Summary

Create a layered, evidence-labeled knowledgebase under `docs/knowledgebase/` for the Andastra .NET game engine project. The KB establishes authority hierarchy over stale README/wiki/docs drift, documents corrected runtime architecture, and provides execution runbooks for agents and contributors.

---

## Problem Frame

Andastra has 60+ investigation docs, a 58-file wiki, and agent rules in `.cursorrules`, but no unified knowledgebase. README architecture diagrams diverge from actual `src/` layout; CI and several docs reference obsolete paths (`CSharpKOTOR`, `OdysseyRuntime`, HoloPatcher). Agents and contributors lack a single evidence-first entry point.

---

## Requirements

- R1. Create layered KB taxonomy: `00-intent`, `10-architecture-runtime`, `20-domain-theory`, `40-operational-risk`, `50-execution`, `90-meta`
- R2. Every factual claim uses evidence labels (`[REPO]`, `[SYNTH]`, `[OPEN]`, etc.)
- R3. Every doc includes a **Repo implications** section
- R4. Correct README drift (Runtime.Games vs Game/Games) with cited evidence
- R5. Seed caveat register with known stale docs and build failures
- R6. Provide build/test ladder aligned with `AGENTS.md`
- R7. No HoloPatcher references in new KB docs
- R8. Cross-link index in `90-meta/README.md`; validate with `git diff --check`

---

## Scope Boundaries

- No `30-product-ux` layer this pass (OdyTools/OdyPatch UX requires build fix + product research)
- No Aurora/Eclipse/Infinity deep domain theory beyond maturity tiers
- No wholesale migration of existing `docs/` or `wiki/` content
- No CI workflow fixes (document drift only)
- No code changes outside KB markdown

### Deferred to Follow-Up Work

- `30-product-ux/` after OdyTools build recovery and `kb-product-ux-researcher` pass
- Create missing `docs/QUICKSTART.md` and `docs/engine_roadmap.md` (README links)
- Repair CI path drift in `.github/workflows/`

---

## Context & Research

### Relevant Code and Patterns

- `README.md`, `AGENTS.md`, `.cursorrules`, `.cursor/rules/odypatch-only.mdc`
- `Andastra.sln`, `src/Andastra/`, `src/BioWare/`, `src/Tools/`
- `docs/WORKFLOWS.md`, `docs/REVA_PROJECT.md`, `docs/CROSS_BINARY_ANALYSIS_PHASE1.md`
- `wiki/` format specs (authoritative for byte-level format details)

### Institutional Learnings

- `docs/solutions/` does not exist yet

### External References

- build-knowledgebase skill taxonomy and evidence contract

---

## Key Technical Decisions

- **Authority hierarchy**: `.cursorrules` > `AGENTS.md` > KB > `wiki/` (formats) > `docs/` (investigations, may be stale)
- **18 focused docs** across 6 layers rather than mega-files
- **Odyssey-first domain scope** matching repo maturity and agent focus

---

## Implementation Units

- U1. **Meta scaffold** — `90-meta/README.md`, `evidence-contract.md`
- U2. **Authority and caveats** — `authority-map.md`, `caveat-register.md`
- U3. **Intent: mission** — `00-intent/project-mission.md`
- U4. **Intent: scope** — `00-intent/engine-family-scope.md`
- U5. **Intent: DoD** — `00-intent/definition-of-done.md`
- U6. **Architecture: topology** — `10-architecture-runtime/solution-topology.md`
- U7. **Architecture: layering** — `runtime-layering.md`, `game-vs-runtime-split.md`
- U8. **Architecture: BioWare + tools** — `bioware-library-boundary.md`, `tools-ecosystem.md`
- U9. **Domain: RE methodology** — `20-domain-theory/reverse-engineering-methodology.md`
- U10. **Domain: Odyssey + resources** — `odyssey-engine-overview.md`, `resource-precedence-chain.md`
- U11. **Domain: formats + scripting** — `file-format-catalog.md`, `ncs-nwscript-vm.md`, `tslpatcher-domain.md`
- U12. **Risk: build + drift** — `40-operational-risk/build-health-matrix.md`, `documentation-drift-register.md`
- U13. **Risk: CI + RE + license** — `ci-release-risks.md`, `re-fidelity-gaps.md`, `license-and-compliance.md`
- U14. **Execution runbooks** — all six `50-execution/` docs
- U15. **Cross-link and validate** — wire index, run validation ladder

**Verification:** All 18 docs exist; `git diff --check` clean; no HoloPatcher in KB; relative links resolve.

---

## System-Wide Impact

- **Documentation only** — no runtime or build behavior changes
- **Agent workflows** — KB becomes preferred onboarding path over stale docs
- **Unchanged invariants** — `.cursorrules` remains highest authority

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| KB content goes stale quickly | Caveat register + drift register with dated entries |
| Over-claiming runtime parity | Label runtime uncertainty; require game install for validation |
| Copying stale docs | Cross-check against `src/` before citing investigation docs |

---

## Sources & References

- kb-orchestrator orchestration plan (2026-05-23 session)
- ce-repo-research-analyst findings
- `README.md`, `AGENTS.md`, `.cursorrules`
