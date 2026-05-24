---
title: "feat: indoor map builder build/save/open + embed tests"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U3)
branch: feat/holocron-fac-kotorcli
---

# feat: Indoor Map Builder — build/save/open + embed tests (U3)

## Summary

Deliver holocron plan **U3** phase A on `feat/holocron-fac-kotorcli`: embed `.indoor` JSON into built `.mod` files via `IndoorMapIo`, add headless characterization tests (Io + Write/Load + mod extract), and wire minimal Build/Save/Open file operations on `IndoorBuilderWindow`.

## Requirements

- R1. `IndoorMap.FinalizeModuleData` embeds `Write()` output as `indoormap.txt` before writing MOD (U3).
- R2. `IndoorMapIoTests` cover embed/extract roundtrip on ERF and file-path extract.
- R3. `IndoorMapWriteLoadTests` cover JSON Write/Load roundtrip with synthetic kit fixture.
- R4. `IndoorBuilderWindow` exposes Save/Open/Build/OpenMod methods wired to `Ui` actions; settings dialog wired.
- R5. Build without installation returns actionable failure (no silent success).

## Scope Boundaries

- **In:** Headless tests + embed wiring + programmatic file ops on window.
- **Out:** Full renderer polish, in-game walkability verification, K1 install roundtrip tests, KotorDiff (U4).

## Implementation Units

### U3a — Embed on build

**Files:** `src/Tools/OdyTools/Data/IndoorMap.cs`

### U3b — Headless tests

**Files:**
- `tests/BioWare.Tests/IndoorMapIoTests.cs`
- `tests/OdyTools.Tests/IndoorMapWriteLoadTests.cs`

### U3c — Window file ops

**Files:** `src/Tools/OdyTools/Windows/IndoorBuilderWindow.cs`

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Embed/extract ERF | JSON bytes roundtrip via `IndoorMapIo` |
| Write/Load JSON | Room count and module_id preserved |
| Open mod with embed | `OpenModFromPath` loads rooms when kits match |
| Build without install | Error message, no MOD written |
| Save to path | `.indoor` file written, reloadable |

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter IndoorMapIo`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter IndoorMapWriteLoad`
