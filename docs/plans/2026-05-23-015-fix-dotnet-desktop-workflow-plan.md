---
title: "fix: Replace placeholder dotnet-desktop CI workflow"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/ci-release-risks.md
---

# fix: Replace placeholder dotnet-desktop CI workflow

## Summary

Replace the stock Microsoft `dotnet-desktop.yml` template (placeholder solution names, MSIX signing, `dotnet test` at repo root) with a Windows desktop build job aligned to the BioWare + OdyTools + OdyPatch green path, and sync agent/KB docs that still claim OdyTools is broken.

---

## Problem Frame

`.NET Core Desktop / build (Release)` runs on every PR but the workflow file still contains `your-solution-name` placeholders, broken PFX decode steps, and unscoped `dotnet test`. Failures surface as OdyPatch/OdyTools compile errors or MSBuild/secret errors unrelated to PR doc changes. `[REPO]`

---

## Requirements

- R1. `.github/workflows/dotnet-desktop.yml` builds BioWare.Tests (net9.0 + net48) and OdyTools/OdyPatch on `windows-latest` for Debug and Release matrix.
- R2. No placeholder env vars or MSIX packaging steps remain.
- R3. Triggers match `ci.yml` (`main`, `master`).
- R4. `AGENTS.md`, `build-and-test-ladder.md`, and `ci-release-risks.md` reflect OdyTools/OdyPatch green status.
- R5. Drift register records remediation item 6.

---

## Scope Boundaries

- Do not add OdyPatch publish/MSIX packaging in this slice.
- Do not fix KotorCLI or full-solution restore gaps.

---

## Implementation Units

- U1. **Replace dotnet-desktop.yml**

**Goal:** Deterministic Windows desktop CI aligned with green tool chain.

**Requirements:** R1, R2, R3

**Files:**
- Modify: `.github/workflows/dotnet-desktop.yml`

**Approach:**
- `windows-latest`, .NET 9.0.x, setup-msbuild for net48 tests.
- Restore/build/test BioWare.Tests net9.0 and net48.
- Restore/build OdyTools and OdyPatch net9.0.
- Remove template MSIX/PFX/upload steps.

**Verification:**
- Valid YAML; local `dotnet build` commands match workflow steps.

---

- U2. **Sync agent and KB docs**

**Goal:** Remove stale OdyTools-red claims.

**Requirements:** R4, R5

**Dependencies:** U1

**Files:**
- Modify: `AGENTS.md`
- Modify: `docs/knowledgebase/50-execution/build-and-test-ladder.md`
- Modify: `docs/knowledgebase/40-operational-risk/ci-release-risks.md`
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`

**Verification:**
- No remaining "OdyTools fails to build" claims in these four files.

---

## Sources & References

- `docs/knowledgebase/40-operational-risk/ci-release-risks.md`
- `.github/workflows/ci.yml`, `.github/workflows/test-builds.yml`
- Plans 013–014 (OdyTools/OdyPatch recovery)
