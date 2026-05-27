---
title: "feat: find-refs --json and --count-only"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-079-feat-holocron-phase-j-kotorcli-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find-refs JSON and count-only output (plan 096)

## Summary

Add machine-readable output modes to KotorCLI `find-refs` for CI and agent pipelines (deferred from plan 079).

## Requirements

- R1. `--json` emits a single JSON object on stdout with `needle`, `type`, `count`, and `references[]` (`resource`, `filepath`, `fieldPath`, `matchedValue`, `displayLabel`).
- R2. `--count-only` prints only the hit count (integer); no per-hit lines or summary footer.
- R3. Empty results: JSON `count: 0` and empty array; count-only prints `0`; exit 1 unchanged.
- R4. Human text output unchanged when neither flag is set.
- R5. Tests cover JSON shape, count-only, and empty-result behavior.

## Scope Boundaries

- No changes to `find-strref` / other find commands yet; no module filter globs.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindRefs`
