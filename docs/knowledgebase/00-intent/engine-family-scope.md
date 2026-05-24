# Engine Family Scope

Maturity tiers and evidence levels for each BioWare engine family in Andastra.

## Maturity Matrix

| Engine | Code location | Evidence level | Notes |
|--------|---------------|----------------|-------|
| **Odyssey (K1/TSL)** | `src/Andastra/Game/Games/Odyssey/` | High `[REPO]` | Area, nav, entities, scripting, dialogue, combat, save/load documented in README |
| **Odyssey (Jade Empire)** | README target | Low `[OPEN]` | Named in README; no dedicated `Games/JadeEmpire` folder observed |
| **Aurora** | `src/Andastra/Game/Games/Aurora/` | Medium-low `[REPO]` | Foundational scaffolding present |
| **Eclipse** | `src/Andastra/Game/Games/Eclipse/` | Medium-low `[REPO]` | Foundational scaffolding present |
| **Infinity** | Not under `Game/Games/` | Minimal `[OPEN]` | README lists Infinity; no top-level implementation folder |

Shared abstractions: `src/Andastra/Game/Games/Common/`, `src/Andastra/Runtime/Games/Common/`. `[REPO]`

## Agent Default Scope

`.cursorrules` mandates K1 + TSL dual-binary analysis for engine behavior. `[REPO]`

`[SYNTH]` Unless the user explicitly requests another engine, agents should:

1. Treat Odyssey K1/TSL as the implementation target
2. Document Aurora/Eclipse/Infinity only at README-stated intent level
3. Avoid inventing behavior for unimplemented engine families

## Per-Family Targets (from README)

| Family | Example titles |
|--------|----------------|
| Odyssey | KOTOR, TSL, Jade Empire |
| Aurora | Neverwinter Nights, NWN2 |
| Eclipse | Dragon Age, Mass Effect |
| Infinity | Baldur's Gate, Icewind Dale, Planescape: Torment |

`[REPO]` (`README.md`)

## Frontier Context (Tier 3 — not current product truth)

Adjacent open-source efforts (Xoreos, reone, KotOR.js) inform positioning but do not define Andastra behavior. `[SYNTH]` See `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` only as historical context — it describes TypeScript work not in this repo. `[REPO]`

## Explicit Non-Goals (this repo)

- HoloPatcher/HoloPatcher.UI restoration `[REPO]`
- Full Aurora/Eclipse/Infinity parity in v1 KB scope `[SYNTH]`
- PyKotor vendor tree as implementation authority `[REPO]`

## Repo Implications

- New engine-specific code belongs under `src/Andastra/Game/Games/{Engine}/` following Odyssey patterns.
- README claims about `Runtime.Games.Infinity` are aspirational until code exists — see [game-vs-runtime-split.md](../10-architecture-runtime/game-vs-runtime-split.md).
- Wiki format docs cover Aurora structs useful across families; runtime integration depth varies by engine.
