# Evidence Contract

Labels used across the Andastra knowledgebase. Every factual claim in KB docs should carry at least one label.

## Source Priority

1. User instructions, preferences, and memory
2. This knowledgebase (`docs/knowledgebase/`)
3. Repo-local evidence (`README.md`, `AGENTS.md`, `.cursorrules`, source, tests, CI)
4. Current official docs for named libraries/platforms
5. Direct product-surface observations
6. External research and comparisons
7. Community anecdotes

Lower-priority evidence must not overrule higher-priority evidence without explicit caveat.

## Label Definitions

| Label | Meaning | Example |
|-------|---------|---------|
| `[REPO]` | Observed fact in this repository | `Andastra.sln` contains 57 projects |
| `[UI]{public}` | Observed product/UI behavior without auth | CLI `--help` output from NSSComp |
| `[UI]{auth}` | Observed behavior requiring login or local build | OdyPatch GUI via `dotnet run` on OdyPatch host csproj |
| `[OFFICIAL]` | Vendor or official documentation claim | MonoGame 3.8 content pipeline docs |
| `[SYNTH]` | Synthesis or implication from multiple sources | "Odyssey is the default agent work target" |
| `[OPEN]` | Unresolved question, caveat, or unknown | Infinity engine implementation status |

## Prohibited Patterns

- Unlabeled speculation presented as fact
- K1-only or TSL-only engine sections without inline comparison (unless user explicitly requested)
- Single-game binary reference format in engine-behavior docs
- Treating `vendor/` PyKotor wiki or HoloPatcher docs as Andastra truth
- Copying investigation-era `docs/*.md` without cross-checking `src/`

## Engine Reference Format

When citing reverse-engineered functions:

```
FunctionName @ (/K1_swkotor @ 0xADDRESS, /TSL_swkotor2 @ 0xADDRESS)
```

If an address is unknown: `TODO: <task>`

Malformed legacy comments should be flagged with:

```
// TODO: Fix agentdecompile reference format - <details>
```

## Repo Implications

- KB authors must label claims at write time; reviewers grep for unlabeled factual paragraphs.
- When `[REPO]` and older `docs/` conflict, update the drift register and prefer verified `src/` evidence.
- Agent responses for engine tasks still require the AgentDecompile status suffix per `.cursorrules`.
