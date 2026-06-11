---
title: "fix: Resolve CodeQL advanced vs default setup conflict"
type: fix
status: active
date: 2026-06-05
origin: docs/knowledgebase/90-meta/pr-merge-readiness.md (Suggested next slices 375+)
---

# fix: Resolve CodeQL advanced vs default setup conflict

## Summary

Stop failing **CodeQL Advanced** workflow runs caused by SARIF upload rejection when GitHub **default CodeQL setup** is also enabled. Remove the redundant advanced workflow so the repo relies on the already-enabled default setup, and document the manual UI path if maintainers later prefer advanced configuration.

---

## Problem Frame

PR CI runs `.github/workflows/codeql.yml` (**CodeQL Advanced**) in parallel with GitHub's repository **default CodeQL setup**. Upload step fails with:

> CodeQL analyses from advanced configurations cannot be processed when the default setup is enabled

This has been documented as duplicate/non-blocking in merge-readiness since PR #36, but it still produces red **CodeQL Advanced** checks on open PRs. The workflow matrix also includes **c-cpp** with `build-mode: autobuild` despite no CI C++ build on ubuntu — wasteful and fragile for incidental `.cpp` files under `helper_scripts/` and `src/KotORSaveLoadCpp/`.

---

## Requirements

- R1. Eliminate SARIF upload failure from advanced vs default setup conflict
- R2. Avoid duplicate CodeQL analysis (advanced workflow + default setup)
- R3. Document manual GitHub UI steps if advanced setup is preferred later
- R4. Update merge-readiness tracker with outcome and verification notes
- R5. Update plans index row for plan **407**

---

## Key Technical Decisions

| Decision | Rationale |
|----------|-----------|
| **Remove** `.github/workflows/codeql.yml` | Default setup is already enabled at repo level; GitHub rejects advanced SARIF while default is active. Removal is the only fully repo-automatable fix. |
| Do not add manual C++ build to CodeQL | Repo primary surface is .NET; C++ is ancillary. Default setup covers relevant languages without autobuild fragility. |
| Document advanced-setup revival path | If maintainers disable default setup in **Settings → Code security → Code scanning**, they may restore an advanced workflow with `c-cpp` at `build-mode: none` and csharp/python/actions at `none`. |

---

## Scope Boundaries

### In scope

- Delete conflicting advanced workflow file
- KB/plans index sync

### Deferred to Follow-Up Work

- Re-introducing advanced CodeQL with custom query packs after default setup is disabled in GitHub UI
- Fixing `code/snyk` quota failures (separate non-blocking issue)

### Non-goals

- Changing GitHub repository default CodeQL settings via API (requires admin UI)
- Modifying application C# source for CodeQL alerts

---

## Implementation Units

### U1. Remove redundant CodeQL Advanced workflow

**Goal:** Stop failing workflow and duplicate scans.

**Requirements:** R1, R2

**Files:**

- `.github/workflows/codeql.yml` (delete)

**Approach:** Delete the workflow file. Default CodeQL setup continues to scan on push/PR to `master` without SARIF conflict.

**Test scenarios:**

- Test expectation: none — workflow deletion; verification is CI observation on PR.

**Verification:** Open PR no longer schedules **CodeQL Advanced** workflow; default CodeQL (if visible) remains unchanged.

---

### U2. Document manual advanced-setup path and sync tracker

**Goal:** Preserve maintainer guidance and close plan **407** in docs.

**Requirements:** R3, R4, R5

**Dependencies:** U1

**Files:**

- `docs/knowledgebase/90-meta/pr-merge-readiness.md`
- `docs/plans/README.md`

**Approach:**

- Add **Known remaining gaps** or CI notes: default setup is authoritative; to use advanced workflow again, disable default in GitHub UI first, then restore workflow with all matrix languages at `build-mode: none` (no c-cpp autobuild on ubuntu).
- Add plan **407** row to plans index.

**Test scenarios:**

- Test expectation: none — documentation only.

**Verification:** Docs reference plan **407** and explain default-vs-advanced conflict resolution.

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Default setup disabled accidentally later | Document that advanced workflow was removed intentionally; revival steps in merge-readiness |
| Loss of custom CodeQL matrix (actions/python explicit jobs) | Default setup already covers standard languages for this repo |

---

## Manual GitHub UI (only if reverting to advanced setup)

1. **Settings → Code security → Code scanning → CodeQL analysis**
2. Disable **Default setup**
3. Restore `.github/workflows/codeql.yml` with matrix entries using `build-mode: none` (including c-cpp)
4. Confirm PR checks show single CodeQL source without SARIF upload errors

---

## Sources & Research

- Failed run `27012352665`: SARIF upload error on csharp/actions/c-cpp/python matrix jobs
- `[REPO]` `.github/workflows/codeql.yml` — c-cpp autobuild, advanced upload
- `[REPO]` `docs/knowledgebase/90-meta/pr-merge-readiness.md` — duplicate CodeQL noted since PR #36
