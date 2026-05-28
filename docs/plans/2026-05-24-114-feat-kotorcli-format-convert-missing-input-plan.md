---
title: "feat: kotorcli format convert missing input exit codes"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-113-feat-kotorcli-gff-xml-convert-closure-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI format convert missing-input exit codes (plan 114)

## Summary

Close the format-convert error contract: every wired KotorCLI convert command must exit non-zero when the input file is missing. Only `json2gff` had coverage today.

## Requirements

- R1. Parameterized integration test covering all ten wired convert commands with a non-existent input path.
- R2. Replace standalone `Json2Gff_MissingInput_ExitsNonZero` with the shared coverage (no duplicate assertion).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
