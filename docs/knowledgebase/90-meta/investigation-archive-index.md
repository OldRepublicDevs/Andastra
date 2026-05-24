# Investigation Archive Index

Tier-4 investigation and RE reports under `docs/` (excluding `docs/knowledgebase/` and `docs/plans/`). **Not authoritative** without cross-checking `src/` and the knowledgebase. `[REPO]`

See [authority-map.md](authority-map.md) tier 4.

## When to use

| Use case | Prefer instead |
|----------|----------------|
| Onboarding / agent defaults | [90-meta/README.md](README.md) → KB layers |
| Architecture truth | `docs/knowledgebase/10-architecture-runtime/` |
| Build/run commands | [50-execution/](../50-execution/) |
| Format byte layouts | `wiki/` |
| Deep RE bug analysis | Tier-4 docs below — verify paths and dates |

## Scale

~62 markdown files at `docs/*.md` plus nested topic folders (TSLPatcher, NCS, main menu, etc.). `[REPO]`

## Stale path patterns (grep before citing)

| Obsolete | Current |
|----------|---------|
| `src/CSharpKOTOR/` | `src/BioWare/` |
| `src/OdysseyRuntime/`, `Odyssey.Game/` | `src/Andastra/Game/Andastra.Game.csproj` |
| `src/OdyPatch/` (no Tools) | `src/Tools/OdyPatch/` |
| `TSLPatcher.Core` NuGet | `BioWare.TSLPatcher` in-repo |
| HoloPatcher as Andastra tool | OdyPatch / OdyPatch.UI only |

Resolved examples: [documentation-drift-register.md](../40-operational-risk/documentation-drift-register.md).

## High-value archives (verify before trusting)

| Doc | Topic | Caveat |
|-----|-------|--------|
| `dialogue_fast_skip_bug_analysis.md` | TSL dialogue skip bug RE | Updated Andastra path (plan 050) |
| `MONOGAME_RUNNING.md` | MonoGame runtime setup | Obsolete paths explicitly marked |
| `REVERSE_ENGINEERING_PROJECT_SUMMARY.md` | Historical Phase 1 report | Not current product truth — see caveat C1 |
| `TSLPatcher_*` cluster | Patch semantics RE | Cross-check `src/BioWare/TSLPatcher/` |
| `main_menu_implementation_*.md` | Menu flow investigations | May predate current Game layout |

## Agent workflow

1. Read KB tier 1–2 first.
2. If citing tier-4 doc, grep for obsolete paths above.
3. File drift in [documentation-drift-register.md](../40-operational-risk/documentation-drift-register.md) when stale claims affect onboarding.

## Repo implications

- Do not duplicate investigation content into KB without evidence labels.
- Vendor `docs/` under `vendor/` are reference-only (PyKotor mirror).
