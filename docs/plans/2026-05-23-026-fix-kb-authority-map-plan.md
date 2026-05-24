---
title: "docs: Fix KB authority-map drift and standalone editor guidance"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 post-025 meta doc audit
---

# docs: Fix KB authority-map drift and standalone editor guidance

## Summary

`authority-map.md` still lists rebranded `wiki/Home.md` as non-authoritative PyKotor/HoloPatcher content (fixed in plan 008). Standalone OdyTool editor contributors lack documented obj/bin isolation patterns from plan 020.

---

## Requirements

- R1. Update `authority-map.md` Tier 5 — remove or correct stale `wiki/Home.md` claim; note vendor wiki remains non-authoritative.
- R2. Add standalone editor contribution notes to `contributing-paths.md` (Directory.Build.props, shared props, Link metadata pattern).
- R3. Drift register remediation **#17**.

---

## Scope Boundaries

- No code or workflow changes.

## Test Scenarios

- `authority-map.md` does not claim Home.md is still "PyKotor Wiki".
- `contributing-paths.md` references `src/Tools/OdyTools/Editors/Directory.Build.props`.
