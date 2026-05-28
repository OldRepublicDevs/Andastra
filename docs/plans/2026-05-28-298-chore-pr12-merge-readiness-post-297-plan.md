---
title: "chore: pr12 merge readiness post-plan-297"
type: chore
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-297-feat-odypatch-validate-cli-test-plan.md
branch: feat/holocron-port-phase-b
---

# chore: PR #12 merge readiness post-plan 297 (plan 298)

## Summary

Prepare PR **#12** for merge after post-PR-#11 arc **291**–**297**: sync KB maintenance tracker, AGENTS.md test commands, plans index, and PR body merge gate after OdyPatch.Tests CI wiring (plan **297**).

## Requirements

- R1. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — note PR **#12** includes plans **291**–**297**, `tests/OdyPatch.Tests/` in CI `test` job.
- R2. Update `AGENTS.md` — add OdyPatch.Tests run command under Run tests section if missing.
- R3. Add plan **298** row to `docs/plans/README.md`.
- R4. Refresh PR **#12** body: merge readiness checklist post-297, milestone text updated (291–297 arc complete).
- R5. Optional: sync plan **063**/**068** slice history through **297** if not already done.
- R6. Verify: `dotnet test tests/OdyPatch.Tests/OdyPatch.Tests.csproj --framework net9.0 -c Release`.

## Verification

```bash
dotnet test tests/OdyPatch.Tests/OdyPatch.Tests.csproj --framework net9.0 -c Release
```

## Scope Boundaries

- Doc/chore sync only; no production code changes.
- Do not commit `.cursor/hooks/`.
