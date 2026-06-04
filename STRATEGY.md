---
name: Andastra
last_updated: 2026-05-24
---

# Andastra Strategy

## Target problem

KOTOR and related BioWare mod authors depend on fragmented, partially maintained Python tooling (PyKotor, HolocronToolset) and opaque game binaries to inspect, edit, and validate content. They lose hours to format gaps, broken CLI stubs, and editors that do not match in-game behavior—while a faithful .NET runtime and tool suite could unify parsing, editing, and engine parity on modern platforms.

## Our approach

Build **BioWare-first libraries** with **Holocron 1:1 tool ports** in vertical slices: each slice lands working code, tests, and build-ladder validation before the next. Prefer engine-fidelity and repository integrity over breadth; defer speculative subsystems until a slice proves user value.

## Who it's for

**Primary:** KOTOR/TSL mod author — They're hiring Andastra to open, edit, diff, patch, and validate game resources with tools that behave like Holocron/PyKotor but run reliably on Linux/modern .NET and feed a future faithful runtime.

**Secondary:** Tooling/CI agent — They're hiring Andastra for scriptable KotorCLI/OdyPatch workflows with correct exit codes and test coverage gates.

## Key metrics

- **Solution build green (net9.0)** — `dotnet build Andastra.sln --framework net9.0` on main; measured in CI.
- **Holocron port slice merge rate** — Feature PRs from plan backlog (063+) merged with tests; tracked in `docs/plans/README.md`.
- **BioWare test pass count** — Regression safety for format/parser work; `dotnet test tests/BioWare.Tests` and OdyTools filters.

## Tracks

### Odyssey runtime fidelity

Faithful K1/TSL behavior from reverse-engineered binaries, shared where possible with inline K1/TSL diffs.

_Why it serves the approach:_ Runtime is the north star; tools and parsers exist to support playable parity.

### Holocron → OdyTools port

Editor and workflow parity (LIP, FAC, reference finder, indoor builder, etc.) in testable vertical slices.

_Why it serves the approach:_ Mod authors get immediate value without waiting for full runtime completion.

### CLI and automation (KotorCLI, OdyPatch)

Non-interactive commands with real BioWare implementations, NuGet-packaged patcher, CI smoke tests.

_Why it serves the approach:_ Enables agents and power users to validate at scale.

## Not working on

- HoloPatcher reintroduction (OdyPatch only).
- Full Module Designer 3D viewport before GL strategy is settled.
- Avalonia browser E2E in CI (local/manual for editor UX).

## Marketing

**One-liner:** Andastra is a cross-platform .NET engine and modding toolkit for BioWare's classic games—faithful runtime plus Holocron-grade tools.

**Key message:** One codebase for parsing, editing, patching, and (eventually) playing KOTOR and related titles on modern systems, with tests and engine references at every layer.
