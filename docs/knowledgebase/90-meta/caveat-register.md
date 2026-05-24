# Caveat Register

Living register of auth walls, runtime uncertainty, and stale-evidence risks. Update when new gaps are discovered.

| ID | Category | Caveat | Label | Last verified |
|----|----------|--------|-------|---------------|
| C1 | Stale evidence | `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` described KotOR.js/TypeScript as primary deliverable | [REPO] | **Resolved 2026-05-23** — reframed as historical Phase 1 report |
| C2 | Stale evidence | `docs/MONOGAME_RUNNING.md` referenced obsolete `src/OdysseyRuntime/Odyssey.Game/` paths | [REPO] | **Resolved 2026-05-23** — updated to Andastra.Game |
| C3 | Stale evidence | ~~README links to missing `docs/QUICKSTART.md` and `docs/engine_roadmap.md`~~ **Resolved 2026-05-23** | [REPO] | 2026-05-23 |
| C4 | Source/runtime drift | ~~CI (`ci.yml`) references missing `src/CSharpKOTOR.Tests/`~~ **Resolved 2026-05-23** | [REPO] | 2026-05-23 |
| C5 | Build uncertainty | `OdyTools.csproj` and `OdyPatch` fail to build (method-group → `System.Action`); tool UX unverified | [REPO] | 2026-05-23 |
| C6 | Runtime uncertainty | `Andastra.Game` requires local K1/TSL installation; no CI game-run | [REPO] | 2026-05-23 |
| C7 | RE partial coverage | Some source comments use K1-only addresses without TSL pairs | [REPO] | 2026-05-23 |
| C8 | Auth wall | AgentDecompile requires running Ghidra MCP server and local game binaries (`/K1_swkotor`, `/TSL_swkotor2`) | [REPO] | 2026-05-23 |
| C9 | Architecture drift | README lists `Runtime.Games.{Odyssey,Aurora,...}`; engine code lives under `src/Andastra/Game/Games/` | [REPO] | **Resolved 2026-05-23** — README architecture updated |
| C10 | Vendor noise | `vendor/src/toolset/wiki/` duplicate PyKotor corpus; HoloPatcher pages not authoritative for Andastra | [REPO] | **Partially resolved 2026-05-23** — `wiki/Home.md` rebranded; vendor tree remains reference-only |
| C11 | Path drift | `.cursorrules` cited `scripts/NcsTool.ps1`; file is at `helper_scripts/NcsTool.ps1` | [REPO] | **Resolved 2026-05-23** — agent rules updated |
| C12 | License | AGPLv3 obligations for combined distribution of engine + tools | [REPO]/[OPEN] | 2026-05-23 |

## Template for New Entries

```markdown
| Cxx | Category | Description | [LABEL] | YYYY-MM-DD |
```

Categories: `Stale evidence`, `Source/runtime drift`, `Build uncertainty`, `Runtime uncertainty`, `RE partial coverage`, `Auth wall`, `License`.

## Repo Implications

- Before citing any `docs/` investigation in KB or PRs, check this register.
- Closing a caveat requires updating the register row and the linked drift/build doc.
- `[OPEN]` license items need legal review before commercial distribution packaging.
