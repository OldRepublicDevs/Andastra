---
title: "chore: pr12 merge readiness and kotorcli readme sync"
type: chore
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-28-295-docs-holocron-post-merge-closure-291-294-plan.md
branch: feat/holocron-port-phase-b
---

# chore: PR #12 merge readiness and KotorCLI README sync (plan 296)

## Summary

Prepare PR **#12** for merge after post-PR-#11 arc **291**–**295**: fix stale KotorCLI README test count in Known Issues (**364** → **369**), note ref-search CLI subprocess coverage (plans **289**–**294**), and refresh PR body merge gate.

## Requirements

- R1. Update `src/Tools/KotorCLI/README.md` Known Issues test count and ref-search CLI note.
- R2. Create plan **296** (this file).
- R3. Refresh PR **#12** body with merge readiness checklist.
- R4. Run `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```

## Scope Boundaries

- Doc/README sync only; no production code changes.
