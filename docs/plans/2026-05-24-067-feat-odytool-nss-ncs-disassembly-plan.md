---
title: "feat: odytool nss ncs disassembly tab (u5)"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U5)
branch: feat/holocron-fac-kotorcli
---

# feat: OdyToolNSS NCS disassembly tab (U5)

## Summary

Deliver holocron **U5** on `feat/holocron-fac-kotorcli`: add a read-only **Disassembly** tab to `OdyToolNSS` showing NCS bytecode with instruction offsets via BioWare `Scripts.DisassembleNcs`. Decompile failures still show disassembly when bytecode is valid. Revert unrelated WIP in `FileResultsDialog` (U6 scope).

## Requirements

- R1. BioWare `Scripts.DisassembleNcsBytes(byte[] data, bool pretty = true)` returns formatted disassembly text from in-memory NCS bytes.
- R2. `OdyToolNSS` exposes Source + Disassembly tabbed editor; Disassembly updates on Load (NCS), Compile success, and New/Revert.
- R3. Decompile failure on NCS load still populates Disassembly when bytes are valid.
- R4. Empty/null NCS bytes clear Disassembly tab without throwing.
- R5. Tests in `OdyTools.Tests` (net9.0 Linux path).

## Scope Boundaries

- **In:** BioWare byte[] disassembly API, OdyToolNSS tab UI, characterization tests.
- **Out:** Reference finder wiring, FileResultsDialog field paths, NCS bytecode reference search (U6 phase 2).

## Implementation Units

### U5a — BioWare DisassembleNcsBytes

**Files:** `src/BioWare/Tools/Scripts.cs`

**Approach:** Refactor shared formatting from `DisassembleNcs(string path)`; add `DisassembleNcsBytes` using `NCSAuto.ReadNcs(data)`.

### U5b — OdyToolNSS Disassembly tab

**Files:** `src/Tools/OdyTools/Editors/OdyToolNSS.axaml.cs`

**Approach:** Wrap existing source editor in tab control; add read-only monospace disassembly pane; implement `RefreshDisassembly(byte[] ncsBytes)`.

### U5c — Tests

**Files:** `tests/OdyTools.Tests/ScriptsDisassemblyTests.cs`

**Approach:** Compile minimal NSS via `NCSAuto.CompileNss`, disassemble bytes, assert non-empty output with hex offset prefix.

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Valid NCS bytes | Non-empty disassembly with offset lines |
| Empty bytes | Empty string |
| Invalid bytes | Error message in disassembly (no throw from API) |
| OdyToolNSS load NCS | Disassembly tab populated (headless smoke optional) |

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter Disassemble`
