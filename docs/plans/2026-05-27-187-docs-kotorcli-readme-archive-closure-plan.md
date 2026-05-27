---
title: "docs: kotorcli readme archive test closure summary"
type: docs
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-186-test-kotorcli-search-archive-bif-without-key-plan.md
branch: feat/holocron-port-phase-b
---

# docs: KotorCLI README archive test closure summary (plan 187)

## Summary

Update `src/Tools/KotorCLI/README.md` to record that archive command integration tests (plans 140–186) are substantially complete, and narrow Known Issues / Next Steps to non-archive gaps (`launch`, utilities partial coverage).

## Requirements

- R1. Add an **Archive test coverage** subsection summarizing list/search/extract/create coverage (RIM, MOD, ERF, BIF+KEY, standalone KEY, BIF without KEY).
- R2. Replace the long Known Issues #2 archive enumeration with a concise closure statement plus remaining non-archive gaps.
- R3. Adjust Next Steps to de-emphasize archive work.

## Verification

- README renders cleanly; test count remains **252** (no new tests this slice).
