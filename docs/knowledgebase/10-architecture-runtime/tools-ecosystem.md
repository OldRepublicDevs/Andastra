# Tools Ecosystem

Development tools under `src/Tools/` and their relationships.

## Policy

- **OdyPatch + OdyPatch.UI only** — HoloPatcher must not be added to solution or `src/Tools/`. `[REPO]` (`.cursor/rules/odypatch-only.mdc`)
- NSS/NCS script operations: use `helper_scripts/NcsTool.ps1` per agent rules (note path drift from `scripts/`). `[REPO]`

## Tool Matrix

| Tool | Path | Role | Build status |
|------|------|------|--------------|
| **OdyPatch** | `src/Tools/OdyPatch/` | TSLPatcher-compatible mod installer core | Red (depends on OdyTools) `[REPO]` |
| **OdyPatch.UI** | `src/Tools/OdyPatch.UI/` | Avalonia GUI for patching | Red (transitive) `[REPO]` |
| **OdyTools** | `src/Tools/OdyTools/` | Holocron-style AIO content editor | Red `[REPO]` |
| **Standalone OdyTool editors** | `src/Tools/OdyTools/*/` (25+ csprojs) | Per-format editors (GFF, DLG, etc.) | Mostly green `[REPO]` |
| **NSSComp** | `src/Tools/NSSComp/` | NWScript compiler CLI | Green `[REPO]` |
| **NCSDecomp.CLI** | `src/Tools/NCSDecomp.CLI/` | NWScript decompiler CLI | Green `[REPO]` |
| **KotorDiff** | `src/Tools/KotorDiff/` | Installation/file diff | Green `[REPO]` |
| **KotorCLI** | `src/Tools/KotorCLI/` | Module/build CLI (PyKotorCLI port) | Crashes on startup `[REPO]` |
| **ConvertKotorGame** | `src/Tools/ConvertKotorGame/` | K1↔TSL format portability GUI | `[OPEN]` build unverified |

## Dependencies

```
OdyPatch.UI → OdyPatch → OdyTools (broken) + BioWare + Andastra (net9)
NSSComp / NCSDecomp.CLI → BioWare
KotorDiff → BioWare + installation model
Standalone editors → BioWare (typical)
```

## Test Coverage

- `tests/OdyTools.Tests/` — editor tests (DLG, GFF, MDL, etc.). `[REPO]`
- No dedicated OdyPatch test project in solution. `[REPO]`

## AgentDecompile / RE Tooling

Not under `src/Tools/` but required for engine work:

- AgentDecompile MCP servers (`user-agdec-mcp-local`, `user-agdec-http`)
- Ghidra programs: `/K1_swkotor`, `/TSL_swkotor2`

See [reverse-engineering-methodology.md](../20-domain-theory/reverse-engineering-methodology.md).

## Repo Implications

- Mod installer documentation should say **OdyPatch**, never HoloPatcher.
- Tooling tasks should pick build-green CLIs (NSSComp, NCSDecomp) when OdyPatch is blocked.
- Fixing OdyTools `System.Action` method-group errors unblocks OdyPatch chain.
