---
title: "fix: Restore CI Test and Lint job failures"
type: fix
status: completed
date: 2026-05-23
origin: PR #2 CI failures (restore --framework, validate-workflows js-yaml)
---

# fix: Restore CI Test and Lint job failures

## Summary

Fix CI regressions introduced in plan 003: `dotnet restore` does not accept `--framework`, and `validate-workflows.yml` requires unavailable `js-yaml` in github-script.

---

## Problem Frame

PR #2 CI jobs **Test**, **Lint**, and **Validate Workflow Syntax** fail. Test/Lint error: `MSB1001: Unknown switch --framework` on restore. Validate workflow fails: `Cannot find module 'js-yaml'`.

---

## Requirements

- R1. Remove `--framework` from all `dotnet restore` commands in CI workflows
- R2. Keep `--framework net9.0` on build/test commands only
- R3. Replace js-yaml github-script validation with Python PyYAML shell step
- R4. Verify locally: restore + build + test ladder passes (except known pre-existing test failure — investigate)

---

## Scope Boundaries

- Do not fix OdyPatch publish failures (OdyTools dependency)
- Do not change validate-workflows secrets check step beyond syntax fix

---

## Implementation Units

- U1. Fix `ci.yml` restore commands
- U2. Fix `test-builds.yml` restore in test step
- U3. Fix `validate-workflows.yml` YAML validation
- U4. Investigate/fix BioWare.Tests single failure if blocking CI

**Verification:** Local restore/build/test; push and confirm CI Test/Lint/Validate pass.

---

## Sources & References

- GitHub Actions run 26349244299 logs
- `docs/plans/2026-05-23-003-fix-ci-workflow-path-drift-plan.md`
