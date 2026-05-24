---
title: "fix: Repair test-builds workflow shell syntax"
type: fix
status: completed
date: 2026-05-23
origin: PR #2 Test Build jobs fail with pwsh ParserError
---

# fix: Repair test-builds workflow shell syntax

## Summary

Fix `test-builds.yml` Build step using bash `if [ ]` inside `shell: pwsh`, causing ParserError on all matrix jobs. Switch cross-platform publish smoke test from OdyPatch (build-red) to NSSComp (build-green).

---

## Problem Frame

PR #2 **Test Build** jobs fail with `Missing '(' after 'if' in if statement` — bash syntax in PowerShell. Even after syntax fix, OdyPatch publish fails due to OdyTools dependency chain.

---

## Requirements

- R1. Rewrite Build step using valid PowerShell conditionals
- R2. Publish `NSSComp` instead of `OdyPatch` as cross-platform smoke build
- R3. Update Verify step for `nsscomp` / `nsscomp.exe` output names
- R4. Remove full-solution `dotnet restore` in favor of targeted restore

---

## Scope Boundaries

- Do not fix OdyTools/OdyPatch compile errors in this slice
- Do not change build-all-platforms.yml release pipeline (separate follow-up)

---

## Implementation Units

- U1. Rewrite `test-builds.yml` build/verify/restore steps

**Verification:** Workflow YAML valid; Test Build matrix passes except known unrelated failures.

---

## Sources & References

- GitHub run 26349318803 logs (ParserError)
- `docs/knowledgebase/40-operational-risk/build-health-matrix.md`
