# Caveat Register

Living register of auth walls, runtime uncertainty, and stale-evidence risks. Update when new gaps are discovered.

| ID | Category | Caveat | Label | Last verified |
|----|----------|--------|-------|---------------|
| C1 | Stale evidence | `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` describes KotOR.js/TypeScript deliverables not present in this repo | [REPO] | 2026-05-23 |
| C2 | Stale evidence | `docs/MONOGAME_RUNNING.md` references obsolete `src/OdysseyRuntime/Odyssey.Game/` paths | [REPO] | 2026-05-23 |
| C3 | Stale evidence | ~~README links to missing `docs/QUICKSTART.md` and `docs/engine_roadmap.md`~~ **Resolved 2026-05-23** | [REPO] | 2026-05-23 |
| C4 | Source/runtime drift | ~~CI (`ci.yml`) references missing `src/CSharpKOTOR.Tests/`~~ **Resolved 2026-05-23** | [REPO] | 2026-05-23 |
| C5 | Build uncertainty | `OdyTools.csproj` and `OdyPatch` fail to build (method-group → `System.Action`); tool UX unverified | [REPO] | 2026-05-23 |
| C6 | Runtime uncertainty | `Andastra.Game` requires local K1/TSL installation; no CI game-run | [REPO] | 2026-05-23 |
| C7 | RE partial coverage | Some source comments use K1-only addresses without TSL pairs | [REPO] | 2026-05-23 |
| C8 | Auth wall | AgentDecompile requires running Ghidra MCP server and local game binaries (`/K1_swkotor`, `/TSL_swkotor2`) | [REPO] | 2026-05-23 |
| C9 | Architecture drift | README lists `Runtime.Games.{Odyssey,Aurora,...}`; engine code lives under `src/Andastra/Game/Games/` | [REPO] | **Resolved 2026-05-23** — README architecture updated |
| C10 | Vendor noise | `vendor/src/toolset/wiki/` and HoloPatcher references are not authoritative for Andastra | [REPO] | 2026-05-23 |
| C11 | Path drift | `.cursorrules` cites `scripts/NcsTool.ps1`; file is at `helper_scripts/NcsTool.ps1` | [REPO] | 2026-05-23 |
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
