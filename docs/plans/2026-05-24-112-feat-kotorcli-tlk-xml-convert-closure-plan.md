---
title: "feat: kotorcli tlk xml convert closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-111-feat-kotorcli-ssf-xml-convert-closure-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI TLK XML convert closure (plan 112)

## Summary

Add integration tests for wired `tlk2xml` / `xml2tlk` commands (follow-on to plan 111 SSF XML closure).

## Requirements

- R1. Integration test: `tlk2xml` writes non-empty XML from a minimal TLK fixture.
- R2. Integration test: `xml2tlk` after `tlk2xml` round-trips entry text at stringref 0.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
