# Documentation Drift Register

Stale or broken documentation tracked for remediation.

## Broken README Links

| Link in README | Status |
|----------------|--------|
| `docs/QUICKSTART.md` | **Resolved** — added 2026-05-23 |
| `docs/engine_roadmap.md` | **Resolved** — added 2026-05-23 |

## Obsolete Paths in Docs

| Document | Issue | Prefer instead |
|----------|-------|----------------|
| `docs/MONOGAME_RUNNING.md` | `src/OdysseyRuntime/Odyssey.Game/` | `src/Andastra/Game/Andastra.Game.csproj` |
| `docs/NCSDecomp_*.md` (multiple) | `src/CSharpKOTOR/` | `src/BioWare/` |
| `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` | KotOR.js/TypeScript deliverables | This KB + current .NET `src/` |
| `tests/BioWare.Tests/Performance/Performance.csproj` | References `CSharpKOTOR.Tests.csproj` | Update or remove |

`[REPO]`

## README vs Source

| README claim | Actual |
|--------------|--------|
| `Runtime.Games.Odyssey` project | `Game/Games/Odyssey/` | **Resolved in README 2026-05-23** |
| `Runtime.Scripting` project | `Game/Scripting/` | **Resolved in README 2026-05-23** |
| `BioWare.NET` naming | `BioWare` csproj | **Resolved in README 2026-05-23** |
| `NSSComp.NET.csproj` | `NSSComp.csproj` | **Resolved in README 2026-05-23** |
| `NCSDecomp` project | `NCSDecomp.CLI` | **Resolved in README 2026-05-23** |

Documented in [game-vs-runtime-split.md](../10-architecture-runtime/game-vs-runtime-split.md). `[REPO]`

## Wiki Drift

| Item | Issue |
|------|-------|
| `wiki/Home.md` | ~~Titled "PyKotor Wiki"; HoloPatcher links~~ **Resolved 2026-05-23** — Andastra/OdyPatch branding and KB links |
| `vendor/src/toolset/wiki/` | Duplicate PyKotor corpus (~450+ files) — reference only |

KB uses OdyPatch naming; wiki cleanup deferred. `[REPO]`

## CI Drift

| Workflow | Issue |
|----------|-------|
| `.github/workflows/ci.yml` | ~~References `src/CSharpKOTOR.Tests/`~~ **Resolved 2026-05-23** — uses BioWare.Tests green path |
| Build workflows | ~~Some reference `src/OdyPatch/`~~ **Resolved 2026-05-23** — updated to `src/Tools/OdyPatch/` |

See [ci-release-risks.md](ci-release-risks.md). `[REPO]`

## Agent Rules Path Drift

| Rule says | Actual |
|-----------|--------|
| ~~`scripts/NcsTool.ps1`~~ | `helper_scripts/NcsTool.ps1` | **Resolved 2026-05-23** |

## Remediation Priority

1. ~~Create `docs/QUICKSTART.md` pointing at KB execution ladder~~ **Done 2026-05-23**
2. ~~Fix CI project paths before trusting CI green~~ **Done 2026-05-23** (OdyPatch build may still fail on OdyTools)
3. ~~Add README errata or update README architecture section~~ **Done 2026-05-23**
4. ~~Refresh wiki Home for Andastra/OdyPatch branding~~ **Done 2026-05-23** (vendor corpus cleanup deferred)

## Repo Implications

- KB is authoritative for onboarding until README/CI are repaired.
- Citing investigation docs requires checking this register first.
- New drift entries go here and in [caveat-register.md](../90-meta/caveat-register.md).
