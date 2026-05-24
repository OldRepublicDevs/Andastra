# Engine Roadmap

Implementation status across BioWare engine families. Synchronized with [engine-family-scope](knowledgebase/00-intent/engine-family-scope.md) in the knowledgebase.

## Overview

Andastra targets four engine families through a shared abstraction layer. **Odyssey (K1/TSL)** is the primary implementation focus for agents and runtime work.

## Maturity Tiers

| Engine | Example games | Status | Code location |
|--------|---------------|--------|---------------|
| **Odyssey** | KOTOR, TSL | **Active — most mature** | `src/Andastra/Game/Games/Odyssey/` |
| **Odyssey** | Jade Empire | Planned | Not observed as dedicated folder |
| **Aurora** | NWN, NWN2 | Foundational | `src/Andastra/Game/Games/Aurora/` |
| **Eclipse** | Dragon Age, Mass Effect | Foundational | `src/Andastra/Game/Games/Eclipse/` |
| **Infinity** | BG, IWD, PST | README target only | No `Game/Games/Infinity/` yet |

## Odyssey (K1/TSL) — Functional Areas

| Subsystem | Status | Notes |
|-----------|--------|-------|
| Area loading (LYT/VIS/rooms) | Implemented | See [odyssey-engine-overview](knowledgebase/20-domain-theory/odyssey-engine-overview.md) |
| Navigation / walkmesh | Implemented | BWM pathfinding |
| Entity / component system | Implemented | Creatures, doors, triggers, etc. |
| NCS / NWScript VM | Implemented | `Game/Scripting/` |
| Dialogue (DLG) | Implemented | Ongoing RE-backed refinements |
| Combat (d20 rounds) | Implemented | K1/TSL differences inline |
| Save / load | Implemented | BioWare SAV + game serializers |
| Mod / resource precedence | Implemented | override → module → save → chitin |
| Main menu / character creation | Partial | See `docs/main_menu_implementation_*.md` investigations |

Runtime parity with original executables is validated via reverse engineering (AgentDecompile) and manual in-game testing — not fully automated in CI.

## Aurora / Eclipse / Infinity

Foundational project structure exists for Aurora and Eclipse under `Game/Games/`. Infinity is listed in README but lacks a corresponding implementation folder.

**Default contribution scope:** Odyssey unless explicitly expanding another engine family.

## BioWare Library (cross-cutting)

Format parsers and resource extraction in `src/BioWare/` support all engine families. Maturity is highest for Odyssey-relevant formats (GFF, 2DA, TLK, NCS, MDL, BWM, etc.). See [file-format-catalog](knowledgebase/20-domain-theory/file-format-catalog.md).

## Tools Roadmap

| Tool | Status |
|------|--------|
| NSSComp, NCSDecomp.CLI, KotorDiff | Build and run |
| OdyTools, OdyPatch, standalone OdyTool editors | Build on net9.0 (2026-05-23) |
| KotorCLI, ConvertKotorGame | Build / `--help` on net9.0 (2026-05-23) |
| **Andastra.sln** (full) | Green on Linux net9.0; CI `solution-build` job (2026-05-23) |

Details: [tools-ecosystem](knowledgebase/10-architecture-runtime/tools-ecosystem.md).

## Known Gaps

- Some RE reference comments lack TSL addresses ([re-fidelity-gaps](knowledgebase/40-operational-risk/re-fidelity-gaps.md))
- Infinity engine not started in source tree

## Repo Implications

Update this roadmap when engine-family maturity changes materially. Deep RE progress belongs in `docs/` investigation files; this doc stays a scannable status summary with KB links.
