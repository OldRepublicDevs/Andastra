---
title: "feat: holocron U3 walkmesh tests and U6 UTD/UTP/UTT ref finder"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U3 phase B + U6 phase 3)
branch: feat/holocron-fac-kotorcli
closure: docs/plans/2026-05-28-279-docs-close-plan-069-walkmesh-utd-ref-plan.md
---

# feat: Holocron U3 walkmesh tests and U6 UTD/UTP/UTT ref finder

## Completion (2026-05-28)

All requirements R1–R4 landed; follow-on scope (ReferenceSearchOptionsDialog, tag/template search) delivered in plans **224–278**. Closed doc-only via plan **279**.

| Req | Status | Evidence |
|-----|--------|----------|
| R1 | **Landed** | **2** `IndoorMapBuildWalkmeshTests` assert `BWMType.AreaModel` |
| R2 | **Landed** | `ScriptReferenceHelper.FindAndShowScriptReferences` + **8** tests |
| R3 | **Landed** | UTD/UTP/UTT/UTC script combo **Find References** via helper |
| R4 | **Landed** | Walkmesh tests use stub fixture (no real install) |

**Verification (2026-05-28):**

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~IndoorMapBuildWalkmesh
# Passed: 2

dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests
# Passed: 8
```

## Summary

Close remaining plan **063** gaps on `feat/holocron-fac-kotorcli`: headless indoor build walkmesh characterization (WOK `AreaModel` + face count), shared `ScriptReferenceHelper` for script combo **Find References** on UTD/UTP/UTT/UTC. Defers `ReferenceSearchOptionsDialog`, tag/template_resref search, KotorCLI utility STUBs, and U7 KB sync.

## Requirements

- R1. `IndoorMapBuildWalkmeshTests` assert `BuildWalkmeshForRoom` emits `BWMType.AreaModel` (full MOD build deferred — headless BuildMap hangs on stub install).
- R2. `ScriptReferenceHelper` centralizes installation-wide script reference search + `FileResultsDialog`.
- R3. UTD, UTP, UTT script combo context menus expose **Find References** (UTC refactored to helper).
- R4. Stub K1 installation suffices for build test (no real game install).

## Scope Boundaries

- **In:** Walkmesh build tests, shared helper, UTD/UTP/UTT/UTC wiring.
- **Out:** ReferenceSearchOptionsDialog UI, tag search, NCS bytecode scan, KotorCLI grep/merge/diff STUBs, U7 KB drift register.

## Implementation Units

### U3d — Walkmesh build tests

**Files:** `tests/OdyTools.Tests/IndoorMapBuildWalkmeshTests.cs`

### U6g — Shared helper + template editors

**Files:**
- `src/Tools/OdyTools/Utils/ScriptReferenceHelper.cs`
- `src/Tools/OdyTools/Editors/OdyToolUTD.axaml.cs`
- `src/Tools/OdyTools/Editors/OdyToolUTP.axaml.cs`
- `src/Tools/OdyTools/Editors/OdyToolUTT.axaml.cs`
- `src/Tools/OdyTools/Editors/OdyToolUTC.axaml.cs`

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| BuildWalkmeshForRoom | `AreaModel` preserved; PWK source upgraded |
| UTD script combo context menu | Find References enabled when install + script set |

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet build src/Tools/OdyTools/OdyTools.Standalone.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "ReferenceFinder|IndoorMap|ScriptsDisassembly"`

## Deferred (plan 070+)

- U7 KB sync (`odytools-editor-ux.md`, drift register)
- KotorCLI utility STUBs (`grep`, `merge`, `diff`, `disassemble`)
- `ReferenceSearchOptionsDialog` scope toggles in UI
- Tag / template_resref reference search
