---
title: "feat: kotorcli gff xml convert closure"
type: feat
status: completed
date: 2026-05-24
origin: docs/plans/2026-05-24-112-feat-kotorcli-tlk-xml-convert-closure-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI GFF XML convert closure (plan 113)

## Summary

Add integration tests for wired `gff2xml` / `xml2gff` commands (follow-on to plans 106/109/111/112 format convert test coverage). Fix round-trip bugs if tests expose them.

## Requirements

- R1. Integration test: `gff2xml` writes non-empty XML from a minimal GFF fixture.
- R2. Integration test: `xml2gff` after `gff2xml` round-trips a root string field (e.g. `Label`).

## Scope Boundaries

- GFF JSON convert tests already exist; this slice covers XML only.
- No AgentDecompile — tooling-only.

## Implementation Units

- U1. **GFF XML integration tests** — `tests/KotorCLI.Tests/FormatConvertIntegrationTests.cs`: add `Gff2Xml_MinimalGff_WritesXmlFile` and `Xml2Gff_AfterGff2Xml_PreservesLabelField` mirroring SSF/TLK patterns (temp dir, `RunKotorCli`, BioWare round-trip assert via `GFFAuto.ReadGff`).
- U2. **Round-trip fixes** — if U1 fails: inspect `GFFXmlWriter` / `GFFXmlReader` / `Conversions.ConvertGffToXml` / `ConvertXmlToGff` (same class of bugs as SSF `IndentXml` or TLK BOM).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
