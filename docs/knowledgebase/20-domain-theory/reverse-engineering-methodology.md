# Reverse Engineering Methodology

Mandatory workflow for engine behavior involving K1 and TSL binaries.

## Programs

| Game | Ghidra program path |
|------|---------------------|
| KOTOR I | `/K1_swkotor` |
| TSL (K2) | `/TSL_swkotor2` |

`[REPO]` (`.cursorrules`, `AGENTS.md`)

Reva project binaries documented in `docs/REVA_PROJECT.md`. `[REPO]`

## Mandatory Workflow

For any engine behavior, mechanics, file formats, resources, or RE logic: `[REPO]` (`.cursorrules`)

1. Open and confirm **both** binaries in AgentDecompile
2. Locate and decompile in K1 — capture address and core logic
3. Locate and decompile in TSL — capture address and core logic
4. Compare; document differences **inline** in one unified description
5. Use required address formatting or explicit `TODO:` when unknown
6. Add source reference comments in C#

## Reference Formats

**Function references:**

```
FunctionName @ (/K1_swkotor @ 0xADDRESS, /TSL_swkotor2 @ 0xADDRESS)
```

**Source comments:**

```csharp
// Reference: /K1_swkotor @ 0xADDRESS, /TSL_swkotor2 @ 0xADDRESS
```

**Malformed legacy comments** — add grep-friendly marker:

```csharp
// TODO: Fix agentdecompile reference format - <details>
```

## AgentDecompile MCP

Configured via `.vscode/mcp.json` / workspace MCP settings. HTTP server proxies Ghidra tools including: `[REPO]` (`AGENTS.md`)

- `decompile_function`, `list_functions`, `search_everything`
- `match_function`, `get_call_graph`, `analyze_data_flow`

If MCP bridge unavailable: continue against local Ghidra programs and verify host/port config. `[REPO]`

## Agent Response Suffix

Engine-related agent responses must end with exactly one line: `[REPO]`

- `AgentDecompile status: Completed - Analyzed both K1 and TSL :)`
- `AgentDecompile status: Partially completed - Missing TSL address for <function>, TODO find it :(`
- `AgentDecompile status: Skipped - <exact reason> :(`

## Supporting Investigation Docs

| Doc | Use |
|-----|-----|
| `docs/CROSS_BINARY_ANALYSIS_PHASE1.md` | Cross-binary mapping phase 1 `[REPO]` |
| `docs/CROSS_BINARY_INTERNAL_STARTUP_MAPPING.md` | Startup sequence mapping `[REPO]` |
| `docs/ghidra_*` | Function-level analyses `[REPO]` |

Label as investigation-era; verify against current `src/` before citing as implementation truth. `[SYNTH]`

## Repo Implications

- Engine PRs without dual addresses are incomplete per project rules.
- Grep `TODO: Fix agentdecompile` to track reference-format debt.
- Wiki updates warranted when discoveries affect KotOR I/II modding or format understanding.
