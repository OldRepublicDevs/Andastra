# Tools Ecosystem

Development tools under `src/Tools/` and their relationships.

## Policy

- **OdyPatch + OdyPatch.UI only** — HoloPatcher must not be added to solution or `src/Tools/`. `[REPO]` (`.cursor/rules/odypatch-only.mdc`)
- NSS/NCS script operations: use `helper_scripts/NcsTool.ps1` per agent rules. `[REPO]`

## Tool Matrix

| Tool | Path | Role | Build status |
|------|------|------|--------------|
| **OdyPatch** | `src/Tools/OdyPatch/` | TSLPatcher-compatible mod installer core | Green (net9.0) `[REPO]` |
| **OdyPatch.UI** | `src/Tools/OdyPatch.UI/` | Avalonia GUI for patching | Green (net9.0) `[REPO]` |
| **OdyTools** | `src/Tools/OdyTools/` | Holocron-style AIO content editor | Green (net9.0) `[REPO]` |
| **Standalone OdyTool editors** | `src/Tools/OdyTools/*/` (25+ csprojs) | Per-format editors (GFF, DLG, etc.) | Green (shared props) `[REPO]` |
| **NSSComp** | `src/Tools/NSSComp/` | NWScript compiler CLI | Green `[REPO]` |
| **NCSDecomp.CLI** | `src/Tools/NCSDecomp.CLI/` | NWScript decompiler CLI | Green `[REPO]` |
| **KotorDiff** | `src/Tools/KotorDiff/` | Installation/file diff | Green `[REPO]` |
| **KotorCLI** | `src/Tools/KotorCLI/` | Module/build CLI (PyKotorCLI port) | `--help` works on net9.0 |
| **ConvertKotorGame** | `src/Tools/ConvertKotorGame/` | K1↔TSL format portability GUI | Builds on net9.0 |

## Dependencies

```
OdyPatch.UI → OdyPatch → OdyTools + BioWare + Andastra (net9)
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
- Tooling tasks may use OdyPatch/OdyTools when the slice requires installer or editor UX; script-only work can stay on NSSComp/NCSDecomp.
- Runtime mod-install validation requires a local K1/TSL install — compile green ≠ UX verified.
