# RE Fidelity Gaps

Known gaps in K1/TSL reverse-engineering coverage and reference quality.

## Policy vs Practice

**Policy:** Every shared engine function needs dual-binary addresses in unified format. `[REPO]` (`.cursorrules`)

**Practice gaps:**

- Some source files contain K1-only addresses without TSL pairs `[REPO]`
- Legacy comments use non-standard AgentDecompile formats — grep: `TODO: Fix agentdecompile`
- Phase 2 items may remain open in `docs/CROSS_BINARY_ANALYSIS_PHASE1.md` `[OPEN]`

## Example Areas Needing RE Debt Pass

Investigation docs exist but implementation parity varies:

| Area | Investigation docs |
|------|-------------------|
| Main menu / character creation | `docs/main_menu_implementation_*.md`, `docs/ghidra_main_menu_reverse_engineering.md` |
| Dialogue timing | `docs/dialogue_timing_bug_analysis.md` |
| Walkability / collision | `docs/walkability_bug_investigation.md`, `docs/collision_detector_analysis.md` |
| Startup file requirements | `docs/swkotor*_exe_startup_file_requirements.md` |

Verify each against current `src/Andastra/Game/` before assuming complete.

## Test Gap

Tests prove format roundtrips and NCS compile — **not** binary behavioral parity with originals. `[REPO]`

`[SYNTH]` Engine fidelity relies on RE workflow + manual in-game validation.

## MCP Availability

AgentDecompile requires running Ghidra MCP bridge. Without it, RE tasks must be marked:

`AgentDecompile status: Skipped - <reason> :(`

## Repo Implications

- Prioritize dual-address cleanup when touching files with malformed refs.
- New engine behavior without RE citations should not merge per project rules.
- Track open RE items in PR descriptions when addresses are TODO.
