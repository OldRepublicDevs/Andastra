---
title: "docs: evidence label pass on thin execution and domain docs"
type: docs
status: completed
date: 2026-05-24
origin: pr-merge-readiness suggested slice 052+; evidence-contract requires labeled factual claims
---

# docs: evidence label pass on thin execution and domain docs

## Summary

Add missing `[REPO]` / `[OPEN]` labels to KB docs with low label density so factual claims comply with [evidence-contract.md](../knowledgebase/90-meta/evidence-contract.md). Sync maintenance tracker CI table for plan 051 `nuget-pack-smoke` job.

---

## Requirements

- R1. Label unmarked factual claims in `run-game-runtime.md` (build commands, graphics deps, manual validation).
- R2. Label toolchain table and test references in `ncs-nwscript-vm.md`.
- R3. Label parser table and wiki authority claims in `file-format-catalog.md`.
- R4. Label layer path table rows in `runtime-layering.md`.
- R5. Label investigation doc references in `reverse-engineering-methodology.md`.
- R6. Add `nuget-pack-smoke` to `pr-merge-readiness.md` CI expectations table.
- R7. Drift remediation **#43**; plans index **052**; maintenance tracker update.

---

## Scope Boundaries

- No content rewrites beyond evidence labels and CI table sync.
- OdyPatch UX validation remains deferred (needs game install).
- No bulk pass on all 34 KB files — target five thin docs only.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Grep unlabeled paragraphs | Target docs have labels on all factual table rows and requirement bullets |

---

## Repo Implications

- Improves agent onboarding compliance with evidence contract.
- Future label passes can target `30-product-ux/` when UX validation unblocks.
