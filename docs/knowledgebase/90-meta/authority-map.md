# Authority Map

When sources disagree, use this hierarchy to decide which claim wins.

## Tier 1 — Agent and Contributor Rules

| Source | Scope | Notes |
|--------|-------|-------|
| `.cursorrules` | Engine fidelity, git discipline, C# 7.3, RE workflow | Highest authority for agent work |
| `.cursor/rules/odypatch-only.mdc` | Tool naming; HoloPatcher prohibition | Solution must reference OdyPatch only |
| `AGENTS.md` | Build/test/run paths, environment gotchas | Practical agent operations |

## Tier 2 — Knowledgebase

| Source | Scope |
|--------|-------|
| `docs/knowledgebase/00-intent/` | Mission, scope, definition of done |
| `docs/knowledgebase/10-architecture-runtime/` | Corrected architecture vs README drift |
| `docs/knowledgebase/20-domain-theory/` | Engine and format domain models |
| `docs/knowledgebase/40-operational-risk/` | Build health, drift, RE gaps |
| `docs/knowledgebase/50-execution/` | Setup, build ladder, run paths |

KB **corrects** stale README/diagram claims when backed by `[REPO]` evidence from `src/`.

## Tier 3 — Format Specifications

| Source | Scope |
|--------|-------|
| `wiki/` (repo root) | KotOR/BioWare file format byte layouts, struct IDs |
| `docs/NSS_GRAMMAR_REFERENCE.md` | NWScript grammar deep reference |

Prefer `wiki/` for on-disk format bytes; prefer KB for how Andastra implements parsers.

## Tier 4 — Investigation Archives

| Source | Scope | Caveat |
|--------|-------|--------|
| `docs/*.md` (61+ files) | RE reports, bug analyses, TSLPatcher studies | May reference obsolete paths (`CSharpKOTOR`, `OdysseyRuntime`) |
| `.cursor/plans/` | Implementation plans | Point-in-time; verify against git |

Always check file dates and cross-reference `src/` before treating as current truth.

## Tier 5 — Non-Authoritative

| Source | Why excluded |
|--------|--------------|
| `vendor/src/toolset/wiki/` | PyKotor/HoloPatcher mirror; duplicates root `wiki/` |
| `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` | Describes TypeScript/KotOR.js work not in this repo `[REPO]` |
| `wiki/Home.md` | Still titled "PyKotor Wiki"; links HoloPatcher `[REPO]` |

## Conflict Resolution

1. If `.cursorrules` and README disagree on RE format → follow `.cursorrules`.
2. If README architecture diagram and `src/Andastra/` tree disagree → follow `src/`; document drift in [documentation-drift-register.md](../40-operational-risk/documentation-drift-register.md).
3. If `docs/` investigation and current code disagree → code wins; file drift entry.
4. If wiki format spec and BioWare parser disagree → treat as `[OPEN]` until roundtrip test resolves.

## Repo Implications

- New contributors should start at [90-meta/README.md](README.md), not random `docs/` RE dumps.
- Engine behavior changes require dual-binary RE verification even when older single-game `docs/` exist.
- Do not resurrect HoloPatcher naming in any tier-1 or tier-2 document.
