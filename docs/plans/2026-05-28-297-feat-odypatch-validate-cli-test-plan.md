---
title: "feat: odypatch validate cli subprocess test"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-23-055-odypatch-validate-fixture-plan.md
branch: feat/holocron-port-phase-b
---

# feat: OdyPatch `--validate` CLI subprocess test (plan 297)

## Summary

Add `tests/OdyPatch.Tests/` with an automated subprocess integration test for OdyPatch `--validate` using existing fixtures (`odypatch-fake-game`, `odypatch-minimal-mod`). Closes the deferred automated-test gap from plan 055 (CI smoke only) and plan 063 Holocron tracker.

## Requirements

- R1. Create `tests/OdyPatch.Tests/OdyPatch.Tests.csproj` (net9.0, NUnit, LangVersion 7.3; mirror `KotorCLI.Tests`).
- R2. Add `ValidateCommandCliTests.cs` with `Cli_Validate_MinimalMod_ExitsZero` asserting exit 0 and output contains `Validation completed successfully`.
- R3. Use `RunOdyPatch` helper: build OdyPatch DLL if missing (120s+ timeout); `dotnet exec` from `RepoRoot`; fixture paths relative to repo root.
- R4. Wire `OdyPatch.Tests` into CI `test` job (restore/build/test); do **not** add to `Andastra.sln` (matches `KotorCLI.Tests` pattern).
- R5. Sync KB `tools-ecosystem.md` Test Coverage section.
- R6. Update `docs/plans/README.md` and plan 063 deferred OdyPatch validation note.

## Verification

```bash
dotnet test tests/OdyPatch.Tests/OdyPatch.Tests.csproj --framework net9.0
dotnet build Andastra.sln --framework net9.0
```

## Scope Boundaries

- No engine/AgentDecompile work.
- No new fixtures; reuse plan 055 assets.
- Manual E2E install remains in [odypatch-e2e-runbook.md](../knowledgebase/50-execution/odypatch-e2e-runbook.md).
