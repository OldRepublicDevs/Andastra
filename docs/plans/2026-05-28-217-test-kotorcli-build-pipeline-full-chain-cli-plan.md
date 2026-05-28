---
title: "test: kotorcli build pipeline full-chain cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-216-test-kotorcli-convert-compile-install-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: build pipeline full-chain CLI subprocess (plan 217)

## Summary

Mirror the highest-value `BuildPipelineIntegrationTests` orchestration scenarios through **CLI subprocess** in `BuildPipelineCommandCliTests`. Plans 214–216 covered per-command CLI coverage; integration tests still invoke `*Command.Execute` directly for inline convert/compile and convert→pack→install chains.

## Problem Frame

Build-pipeline CLI subprocess suite is complete per-command but lacks end-to-end orchestration proofs that `pack` and `install` invoke convert/compile inline when flags are omitted.

## Requirements

- R1. CLI: JSON-only project → `pack default` (no `--noConvert`) exits **0** and `test.mod` contains UTC resref.
- R2. CLI: NSS-only project → `pack default` (no `--noCompile`) exits **0** and `test.mod` contains NCS resref.
- R3. CLI: JSON project → `convert default` → `pack default --noConvert --noCompile` → `install default --installDir <fake>` exits **0** and installed MOD contains UTC.
- R4. Extend `BuildPipelineCommandCliTests.cs`; reuse `RunKotorCli` and temp project patterns from plan 216.
- R5. Update `src/Tools/KotorCLI/README.md` test count and note full-chain CLI coverage.

## Scope Boundaries

- No changes to KotorCLI command implementations unless tests expose a real bug.
- No duplicate of plan 216 per-command tests.
- No `launch` or archive work.

## Context & Research

### Relevant Code and Patterns

- `tests/KotorCLI.Tests/BuildPipelineIntegrationTests.cs` — `Pack_WithInlineConvert_ProducesModFromJsonSource`, `Pack_WithInlineCompile_ProducesModFromNssSource`, `Convert_Pack_Install_Pipeline_ProducesInstalledMod`
- `tests/KotorCLI.Tests/BuildPipelineCommandCliTests.cs` — CLI harness, configs, `WriteModWithUtc`
- ResRefs ≤16 chars: `pk_creature`, `pk_main`, `plc_creature`

## Key Technical Decisions

- **Separate convert then pack with skip flags for R3:** Matches integration test sequencing; proves CLI convert staging + pack from cache + install copy without re-running convert during pack.
- **Inline orchestration for R1/R2:** Omit `--noConvert` / `--noCompile` on `pack` to mirror integration defaults.

## Implementation Units

- U1. **CLI inline convert pack**

**Goal:** R1 — pack orchestrates convert from JSON via subprocess.

**Files:**
- Modify: `tests/KotorCLI.Tests/BuildPipelineCommandCliTests.cs`

**Test scenarios:**
- Happy path: `pack default` after writing `*.utc.json` under `src/`; assert `test.mod` contains UTC.

**Verification:** Filter `FullyQualifiedName~CliPack_WithInlineConvert`

- U2. **CLI inline compile pack**

**Goal:** R2 — pack orchestrates compile from NSS via subprocess.

**Test scenarios:**
- Happy path: minimal `void main(){}` NSS; `pack default`; assert NCS in MOD.

**Verification:** Filter `FullyQualifiedName~CliPack_WithInlineCompile`

- U3. **CLI convert pack install chain**

**Goal:** R3 — full install chain via subprocess.

**Test scenarios:**
- Happy path: fake install with `chitin.key`; convert → pack (skip flags) → install; assert `modules/test.mod` resource.

**Verification:** Filter `FullyQualifiedName~CliConvert_Pack_Install`

- U4. **README inventory**

**Goal:** R5 — document closure.

**Files:**
- Modify: `src/Tools/KotorCLI/README.md`

**Test expectation:** none — doc sync only.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~BuildPipelineCommandCli"
```
