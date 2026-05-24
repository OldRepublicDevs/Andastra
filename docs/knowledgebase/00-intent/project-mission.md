# Project Mission

## What Andastra Is

Andastra is a unified .NET game engine runtime and development tooling suite for BioWare engine families, built with MonoGame/Stride graphics backends. `[REPO]` (`README.md`)

Primary focus for agents and active development: **reverse-engineering and modding Knights of the Old Republic I (K1) and II: The Sith Lords (TSL)** with engine fidelity as the highest priority. `[REPO]` (`.cursorrules`)

The project aims for faithful reimplementations comparable in scope to Xoreos, differentiated by modern .NET, cross-platform tooling, and a shared multi-engine abstraction layer. `[REPO]` (`README.md`)

## Supported Engine Families

| Family | Games (examples) | Maturity |
|--------|------------------|----------|
| Odyssey | KOTOR, TSL, Jade Empire | Most mature — K1/TSL functional targets |
| Aurora | NWN, NWN2 | Foundational |
| Eclipse | Dragon Age, Mass Effect | Foundational |
| Infinity | BG, IWD, PST | README target; minimal implementation `[OPEN]` |

See [engine-family-scope.md](engine-family-scope.md) for detail.

## Core Components

1. **Unified engine runtime** — layered architecture from domain core through graphics and per-engine game rules. `[REPO]`
2. **BioWare library** — file format parsers, resource extraction, TSLPatcher logic. `[REPO]`
3. **Development tools** — OdyPatch / OdyPatch.UI (mod installer), OdyTools editors, NSSComp, NCSDecomp, KotorDiff, KotorCLI, ConvertKotorGame. `[REPO]`

## Name and Heritage

The name combines logical AND (system integration), Astraea (mythological link to Xoreos/EOS naming), and Andraste (BioWare Dragon Age lore). `[REPO]` (`README.md`)

## Non-Goals

- Shipping commercial games or replacing official executables for end users `[SYNTH]`
- Reviving HoloPatcher — only **OdyPatch** and **OdyPatch.UI** exist in solution. `[REPO]` (`.cursor/rules/odypatch-only.mdc`)
- K1-only or TSL-only documentation of shared engine behavior without inline comparison. `[REPO]` (`.cursorrules`)
- Single-game binary reference format for shared engine functions. `[REPO]`

## License

AGPLv3. See [license-and-compliance.md](../40-operational-risk/license-and-compliance.md). `[REPO]` (`LICENSE`)

## Repo Implications

- Feature work defaults to Odyssey/K1+TSL unless explicitly scoped to another engine family.
- Fidelity beats feature velocity for engine-behavior changes.
- Public-facing narrative stays in root `README.md`; KB carries corrected architecture and operational truth.
