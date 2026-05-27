---
title: "feat: kotorcli ssf xml convert closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-109-feat-kotorcli-2da-csv-convert-closure-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI SSF XML convert closure (plan 111)

## Summary

Add integration tests for wired `ssf2xml` / `xml2ssf` commands (follow-on to plans 106/109 format convert test coverage).

## Requirements

- R1. Integration test: `ssf2xml` writes non-empty XML from a minimal SSF fixture.
- R2. Integration test: `xml2ssf` after `ssf2xml` round-trips a sound slot StrRef value.

## Scope Boundaries

- R3. Fix `SSFXMLWriter.IndentXml` if round-trip tests expose empty XML output (LINQ to XML `Value` assignment drops child nodes).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
