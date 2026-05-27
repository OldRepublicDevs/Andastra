---
title: "docs: KotorCLI reference search command documentation"
type: docs
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-084-feat-holocron-phase-o-2da-row-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# docs: KotorCLI reference search commands (plan 085)

## Summary

Document all wired KotorCLI installation reference-search commands (`find-refs`, `find-strref`, `find-2da-ref`, `find-field-value`) in `src/Tools/KotorCLI/README.md` with examples and flags so agents and modders can discover the Holocron port surface.

## Requirements

- R1. README section listing all four commands with purpose and BioWare backing type.
- R2. Copy-paste `dotnet run` examples for each command.
- R3. Flag reference table shared across commands (`--install-dir`, `--installation`, scope/match flags where applicable).
- R4. Remove stale duplicate `assemble` bullet in Script Tools.

## Scope Boundaries

- No new CLI behavior; documentation only.
- README “stub” status for unrelated commands unchanged.

## Verification

- Manual: README renders correctly; examples match `FindRefsCommand`, `FindStrRefCommand`, `Find2DARefCommand`, `FindFieldValueCommand` option names.
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FindRefs|FindStrRef|Find2DARef|FindFieldValue"`
