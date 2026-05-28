---
title: "feat: kotorcli gff json convert closure (plan 070 U11)"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-070-feat-pykotor-port-residuals-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI format convert closure (plan 106)

## Summary

Close plan 070 U11 residual: integration-test `json2gff` output and align KotorCLI README with wired convert/utility commands.

## Requirements

- R1. Integration test: `gff2json` then `json2gff` produces a non-empty GFF file.
- R2. Integration test: round-tripped GFF preserves a known string field (`Label`).
- R3. README marks `gff2json`/`json2gff` and `grep`/`diff`/`merge`/`cat` as implemented (not stub).

## Scope Boundaries

- No new convert commands; FAC tests (U10) already landed.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
