---
title: "feat: holocron port phase A — KotorCLI converts + OdyToolFAC"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U1, U2)
branch: feat/holocron-fac-kotorcli
closure: docs/plans/2026-05-28-281-docs-close-plan-064-kotorcli-fac-plan.md
---

# feat: Holocron port phase A — KotorCLI converts + OdyToolFAC

## Completion (2026-05-28)

All requirements R1–R5 landed on `feat/holocron-fac-kotorcli`. Closed doc-only via plan **281**.

| Req | Status | Evidence |
|-----|--------|----------|
| R1 | **Landed** | `FormatConvertCommands.cs` delegates to BioWare `Conversions` (no STUB-only paths) |
| R2 | **Landed** | `OdyToolFAC.axaml` + `FACHelpers` load/save |
| R3 | **Landed** | `WindowUtils` routes `ResourceType.FAC` → `OdyToolFAC` |
| R4 | **Landed** | `OdyToolFAC.Standalone.csproj` in solution |
| R5 | **Landed** | **3** `OdyToolFACTests` pass |

**Verification (2026-05-28):**

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvert
# Passed: 21

dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolFAC
# Passed: 3
```

## Summary

Deliver implementation units **U1** and **U2** from the PyKotor/Holocron port continuation plan on a **feature branch** (`feat/holocron-fac-kotorcli`), separate from docs-only PR #4. Wire KotorCLI format-convert STUBs to BioWare `Conversions`, add Holocron-parity `OdyToolFAC` editor with standalone shell, route `ResourceType.FAC` in `WindowUtils`, and add headless characterization tests.

## Requirements

- R1. KotorCLI `tlk2xml`, `xml2tlk`, `ssf2xml`, `xml2ssf`, `json2gff` call BioWare conversions with non-zero exit on missing input or failure (U1).
- R2. `OdyToolFAC` loads/saves `repute.fac` via `FACHelpers`; faction list + reputation matrix editing (U2).
- R3. `WindowUtils` opens `OdyToolFAC` for `.fac` before generic GFF (AIO + standalone).
- R4. `OdyToolFAC.Standalone.csproj` builds with `FAC_STANDALONE` define; solution entry added.
- R5. `OdyToolFACTests` pass under headless Avalonia.

## Scope Boundaries

- **In:** U1 + U2 only; fixes required for build/test green.
- **Out:** PR #4 docs batch; U3–U7 (Indoor Builder, KotorDiff, NCS disasm, reference finder); save-game nested FAC.
- **Branch:** Do not commit feature code to `docs/post-pr3-tracker-sync`.

## Implementation Units

### U1 — KotorCLI format convert wiring

**Files:**
- Modify: `src/Tools/KotorCLI/Commands/FormatConvertCommands.cs`

**Verification:**
- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- No `TODO: STUB` log-only paths remain for TLK/SSF/JSON→GFF converts

### U2 — OdyToolFAC editor

**Files:**
- Create: `src/Tools/OdyTools/Editors/OdyToolFAC.axaml`, `OdyToolFAC.axaml.cs`
- Create: `src/Tools/OdyTools/Editors/OdyToolFAC.Standalone.csproj`
- Modify: `src/Tools/OdyTools/Editors/WindowUtils.cs`
- Modify: `src/Tools/OdyTools/Editors/Standalone/EditorStandaloneHost/EditorStandaloneApp.axaml.cs` (`FAC_STANDALONE` branches)
- Modify: `Andastra.sln`
- Test: `tests/OdyTools.Tests/OdyToolFACTests.cs`

**Patterns:** `OdyToolSSF` standalone + editor; `vendor/.../fac.py` behavior reference.

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| KotorCLI json2gff | Missing input → exit 1; valid JSON → GFF file written |
| KotorCLI tlk2xml / ssf2xml | Same error/success contract as gff2json |
| FAC load/build | Minimal FAC roundtrips factions + reputations |
| FAC rename faction | Modified name persists after Build |
| WindowUtils FAC route | `ResourceType.FAC` → `OdyToolFAC`, not `OdyToolGFF` |
| Standalone build | `OdyToolFAC.Standalone.csproj` net9.0 Release succeeds |

## Risks

- Standalone `#if` chains must include `FAC_STANDALONE` alongside existing editor defines.
- OdyTools main project may have pre-existing build errors unrelated to this slice; validate via OdyTools.Tests filter.

## Repo Implications

- Opens PR #5 (feature) for Holocron parity phase A; PR #4 remains docs-only merge candidate.
