# Odyssey Engine Overview

Unified map of Odyssey engine subsystems for K1 and TSL.

## Subsystems (README + code)

| Subsystem | Description | Primary code area |
|-----------|-------------|-------------------|
| **Areas** | LYT layout, VIS culling, room meshes | `Game/Games/Odyssey/` loaders, graphics |
| **Navigation** | Walkmesh pathfinding, surface materials | BWM + runtime nav |
| **Entities** | Creatures, doors, placeables, triggers, waypoints | Component/entity architecture |
| **Scripting** | NCS VM, NWScript engine API | `Game/Scripting/` |
| **Dialogue** | DLG conversations, VO, lip-sync | Odyssey dialogue managers |
| **Combat** | Round-based d20 combat, effects | Odyssey combat modules |
| **Save/Load** | Save serialization compatible with originals | BioWare SAV + game serializers |
| **Mod support** | Resource precedence chain | BioWare extract + runtime providers |

`[REPO]` (`README.md`, `src/Andastra/Game/Games/Odyssey/`)

## K1 vs TSL Treatment

Shared engine with minor differences. All behavior docs and implementations must: `[REPO]` (`.cursorrules`)

- Use one unified description
- Note K1/TSL differences inline
- Cite both binary addresses when RE-backed

Example entry-point references already in `Program.cs` comments (WinMain, mutex names, config loaders). `[REPO]`

## Investigation Cross-References

| Topic | Doc |
|-------|-----|
| Cross-binary analysis | `docs/CROSS_BINARY_ANALYSIS_PHASE1.md` |
| Startup files | `docs/swkotor_exe_startup_file_requirements.md`, `docs/swkotor2_exe_startup_file_requirements.md` |
| Main menu | `docs/ghidra_main_menu_reverse_engineering.md`, `docs/main_menu_implementation_*.md` |
| Dialogue timing | `docs/dialogue_timing_bug_analysis.md` |
| Walkability | `docs/walkability_bug_investigation.md` |

Verify against current code — several are point-in-time investigations.

## Maturity Caveat

Runtime features require a local game install for end-to-end validation. `[REPO]` CI does not run the game loop.

## Repo Implications

- Default engine work targets `Game/Games/Odyssey/` unless refactoring shared `Common` code.
- Subsystem bugs may need RE pass on both binaries before fix lands.
- README feature list is aspirational for some subsystems — confirm against tests and runtime before claiming parity.
