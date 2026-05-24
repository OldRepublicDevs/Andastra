# BioWare Library Boundary

`src/BioWare/` is the shared format and resource library consumed by runtime, game, and tools.

## Responsibilities

| Area | Path (typical) | Purpose |
|------|----------------|---------|
| **Format parsers** | `Resource/Formats/{GFF,TwoDA,TLK,MDL,NCS,...}/` | Read/write game file formats |
| **Resource types** | `Common/ResourceType.cs`, related | ResRef, type IDs, extensions |
| **Extract** | Extract modules for chitin, capsules, saves | Installation resource enumeration |
| **TSLPatcher** | TSLPatcher modules under BioWare | Mod patch application logic |
| **Common** | `BioWare/Common/` | Shared enums (e.g. `BioWareGame`) |

Format folders observed under `Resource/Formats/`: BIF, BWM, DDS, ERF, GFF, KEY, LIP, LTR, LYT, MDL, NCS, PCC, RIM, SAV, SET, SSF, TEX, TLK, TPC, TwoDA, and others. `[REPO]`

## Target Framework

- `BioWare.csproj` multi-targets `net9.0;net48`. `[REPO]`
- Pre-build runs `scripts/Remove-DuplicateUsings.ps1` — fails silently on Linux (harmless). `[REPO]` (`AGENTS.md`)

## Consumption Pattern

`[SYNTH]`

1. **Tools** (NSSComp, NCSDecomp.CLI, KotorDiff, OdyPatch) reference BioWare directly for I/O.
2. **Runtime/Game** load resources through BioWare extract + runtime providers.
3. **Tests** — `BioWare.Tests` validates format roundtrips; primary safety net for parser changes.

## Tests

`tests/BioWare.Tests/` includes format roundtrips for GFF, 2DA, LYT, SSF, TXI, streams, modules, saves, and more. `[REPO]`

`tests/Andastra.Tests/` adds NCS compile roundtrip (K1+TSL). `[REPO]`

## Boundary Rules

- **Format byte layout changes** start in BioWare parsers, with wiki spec cross-check.
- **Engine gameplay rules** (combat timing, dialogue flow) belong in `Game/Games/`, not BioWare.
- **TSLPatcher semantics** live in BioWare; **OdyPatch** is the runnable installer host (exe); **OdyPatch.UI** is the Avalonia UI library. `[REPO]`

## Repo Implications

- A parser bug fix typically touches `src/BioWare/` + `tests/BioWare.Tests/Formats/`.
- Do not duplicate format logic in runtime — extend BioWare.
- Legacy docs referencing `src/CSharpKOTOR/` mean `src/BioWare/` today. `[REPO]`
