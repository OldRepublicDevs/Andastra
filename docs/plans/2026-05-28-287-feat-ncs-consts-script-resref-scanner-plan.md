---
title: "feat: NCS CONSTS scanner for script reference finder"
type: feat
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
---

# feat: NCS CONSTS scanner for script reference finder (plan 287)

## Summary

Add `NcsConstStringScanner` for NWScript string literals in NCS V1.0 bytecode and wire `ReferenceFinder.FindScriptResRefInNcsBytes` to prefer CONSTS hits with `(NCS bytecode) offset_<n>` field paths (Holocron / StrRef cache parity). Substring fallback retained for non-NCS payloads.

## Requirements

- R1. `NcsConstStringScanner.ExtractConstStringInstructions` parses opcode `0x04` / qualifier `0x05`.
- R2. `FindScriptResRefInNcsBytes` prefers CONSTS matches; honors `CaseSensitive`.
- R3. NCS hit paths use `(NCS bytecode) offset_<byteOffset>`.
- R4. Substring fallback when CONSTS finds no match.
- R5. BioWare + ReferenceFinder tests; update 063/068 deferred notes.

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConstString
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindScriptResRefInNcsBytes
```
