---
title: "feat: kotorcli pack unpack roundtrip (no removeDeleted)"
type: feat
status: active
date: 2026-05-27
origin: docs/plans/2026-05-24-136-feat-kotorcli-pack-unpack-remove-deleted-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI pack → unpack roundtrip (plan 137)

## Summary

Integration test proving JSON UTC sources packed into a MOD round-trip through `unpack` (without `--removeDeleted`): file restored under `[package.rules]` path and GFF field values preserved.

## Requirements

- R1. Config with `[package.sources] include = "src/**/*.json"` and `[package.rules] "*.utc" = "src/blueprints/creatures"` (reuse `UnpackRemoveDeletedConfig` pattern from plan 136).
- R2. Write UTC JSON with a distinctive `Label` field via `GFFAuto`.
- R3. `PackCommand.Execute` produces `test.mod` containing the UTC (resref ≤ 16 chars, e.g. `rt_creature`).
- R4. Delete source `.utc.json` after pack so unpack must recreate it.
- R5. `UnpackCommand.Execute("default", modPath, removeDeleted: false, logger)` exits 0.
- R6. Assert JSON restored at `src/blueprints/creatures/<resref>.utc.json` and `Label` field round-trips.

## Decisions

- Call `PackCommand` / `UnpackCommand.Execute` directly (matches existing pipeline tests).
- Use `GFF` + `SetString("Label", …)` + `GFFAuto.WriteGff` for source JSON (matches convert/pack pipeline tests).
- Read restored JSON with `GFFAuto.ReadGff(..., ResourceType.GFF_JSON)` and assert `Label`.

## Test scenarios

| ID | Scenario | Expected |
|----|----------|----------|
| T1 | Pack UTC JSON → MOD, delete source, unpack (no removeDeleted) | JSON reappears under rules path; `Label` unchanged |

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~BuildPipeline"
```

## Out of scope

- `--removeDeleted` behavior (plan 136)
- Production changes unless test exposes a bug
- CLI subprocess / `Program.Main` tests
