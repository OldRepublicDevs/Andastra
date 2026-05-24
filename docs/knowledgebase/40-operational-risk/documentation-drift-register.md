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
| `docs/MONOGAME_RUNNING.md` | ~~`src/OdysseyRuntime/Odyssey.Game/`~~ | `src/Andastra/Game/Andastra.Game.csproj` | **Resolved 2026-05-23** |
| `docs/NCSDecomp_*.md` (multiple) | ~~`src/CSharpKOTOR/`~~ | `src/BioWare/Resource/Formats/NCS/Decomp/` | **Resolved 2026-05-23** |
| `docs/REVERSE_ENGINEERING_PROJECT_SUMMARY.md` | ~~KotOR.js/TypeScript deliverables~~ | KB + Andastra .NET `src/` | **Resolved 2026-05-23** |
| `tests/BioWare.Tests/Performance/Performance.csproj` | ~~References `CSharpKOTOR.Tests.csproj`~~ | Removed stale reference | **Resolved 2026-05-23** |

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
2. ~~Fix CI project paths before trusting CI green~~ **Done 2026-05-23**
3. ~~Add README errata or update README architecture section~~ **Done 2026-05-23**
4. ~~Refresh wiki Home for Andastra/OdyPatch branding~~ **Done 2026-05-23** (vendor corpus cleanup deferred)
5. ~~Fix OdyTools compile (method-group → delegate wiring)~~ **Done 2026-05-23** — unblocks OdyPatch chain
6. ~~Replace placeholder `dotnet-desktop.yml` workflow~~ **Done 2026-05-23** — Windows BioWare + OdyTools + OdyPatch build
7. ~~Sync KB tool docs after C5 build recovery~~ **Done 2026-05-23** — run-tools-reference, tools-ecosystem, topology

## KB Doc Drift (post-C5)

| Document | Issue | Status |
|----------|-------|--------|
| `run-tools-reference.md`, `tools-ecosystem.md`, `solution-topology.md` | ~~OdyTools/OdyPatch listed as Red~~ | **Resolved 2026-05-23** |

## CI Workflow Drift

| Workflow | Issue | Status |
|----------|-------|--------|
| `.github/workflows/dotnet-desktop.yml` | ~~Microsoft template placeholders / broken MSIX steps~~ | **Resolved 2026-05-23** |

## Build Health (C5)

| Item | Issue | Status |
|------|-------|--------|
| `OdyTools.csproj` | ~~Method-group → `System.Action` in `BindLostFocus` / `BindClick`~~ | **Resolved 2026-05-23** |
| `OdyPatch` / `OdyPatch.UI` | ~~Depends on broken OdyTools; missing project reference~~ | **Resolved 2026-05-23** |

## Repo Implications

- KB is authoritative for onboarding and architecture; check this register before citing investigation docs.
- New drift entries go here and in [caveat-register.md](../90-meta/caveat-register.md).
