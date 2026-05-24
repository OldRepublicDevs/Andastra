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
7. ~~Sync KB tool docs after C5 build recovery~~ **Done 2026-05-23**
8. ~~Sync onboarding docs (QUICKSTART, engine_roadmap)~~ **Done 2026-05-23**
9. ~~Fix KotorCLI System.CommandLine 2.0 Option API~~ **Done 2026-05-23**
10. ~~Sync post-018 onboarding/AGENTS + KotorCLI desktop CI~~ **Done 2026-05-23**
11. ~~Fix OdyTools standalone obj/bin collision for parallel solution builds~~ **Done 2026-05-23**
12. ~~Add CI solution-build job + sync post-020 KB claims~~ **Done 2026-05-23**
13. ~~Remove duplicate NAudio/compile items in WAV/DLG/SSF standalones~~ **Done 2026-05-23**
14. ~~Finalize build-health matrix + PR CI validation notes~~ **Done 2026-05-23**
15. ~~Sync engine_roadmap and build ladder post-023~~ **Done 2026-05-23**
16. ~~Fix wiki format-page HoloPatcher links + AGENTS tool coverage~~ **Done 2026-05-23**
17. ~~Fix authority-map wiki/Home drift + standalone editor contributing guidance~~ **Done 2026-05-23**
18. ~~Add ConvertKotorGame to dotnet-desktop CI + sync NCS/dev-setup docs~~ **Done 2026-05-23**
19. ~~Sync QUICKSTART and build ladder tool chain post-027~~ **Done 2026-05-23**
20. ~~Add OdyPatch.UI to dotnet-desktop CI + fix README build commands~~ **Done 2026-05-23**
21. ~~Fix release doc OdyPatch paths + README run commands~~ **Done 2026-05-23**
22. ~~Serialize solution-build CI to fix Andastra.Core deps lock~~ **Done 2026-05-23**
23. ~~Fix NUGET.md OdyPatch paths + sync CI build notes~~ **Done 2026-05-23**
24. ~~Reframe NUGET.md for BioWare.TSLPatcher + OdyPatch~~ **Done 2026-05-23**
25. ~~Fix NuGet publish scripts + MANUAL_PUSH_INSTRUCTIONS drift~~ **Done 2026-05-23**
26. ~~Fix OdyPatch NuGet pack SPDX license expression~~ **Done 2026-05-23**
27. ~~Sync PR merge-readiness doc + PR #2 body for plans 033–035~~ **Done 2026-05-23**
28. ~~Sync AGENTS/QUICKSTART/dev-setup with NuGet pack toolchain~~ **Done 2026-05-23**
29. ~~Seed `30-product-ux/` KB stub layer~~ **Done 2026-05-23**
30. ~~Fix OdyPatch README TSLPatcher.Core / path drift~~ **Done 2026-05-23**
31. ~~Refresh PR #2 body + sync engine_roadmap through plan 039~~ **Done 2026-05-23**
32. ~~Sync build-and-test-ladder + tools-ecosystem post-035~~ **Done 2026-05-23**
33. ~~Sync agent-workflow + authority-map taxonomy~~ **Done 2026-05-23**
34. ~~Sync definition-of-done + contributing-paths~~ **Done 2026-05-23**
35. ~~Add OdyPatch.UI README + fix GUI entry-point drift~~ **Done 2026-05-23**
36. ~~Sync OdyPatch host vs UI library architecture docs~~ **Done 2026-05-23**
37. ~~Add plans index + agent onboarding OdyPatch sync~~ **Done 2026-05-23**
38. ~~Sync operational-risk + tslpatcher-domain OdyPatch host~~ **Done 2026-05-23**
39. ~~Sync solution-topology + README OdyPatch roles~~ **Done 2026-05-23**
40. ~~Post-merge meta refresh + Andastra.Utility csproj path~~ **Done 2026-05-24**
41. ~~Investigation archive index + dialogue doc path fix~~ **Done 2026-05-24**
42. ~~OdyPatch NuGet pack CI smoke job~~ **Done 2026-05-24**
43. ~~Evidence label pass on thin execution/domain KB docs~~ **Done 2026-05-24**
44. ~~OdyPatch --help routed to CLI + CI smoke~~ **Done 2026-05-24**
45. ~~PR #3 merge conflicts with master squash~~ **Done 2026-05-24**
46. ~~OdyPatch validate CLI fixture + CI smoke~~ **Done 2026-05-24**
47. ~~Post-PR #3 tracker still lists open PR~~ **Done 2026-05-24**
48. ~~Validate fixture missing from build-and-test-ladder~~ **Done 2026-05-24**
49. ~~No canonical OdyPatch E2E manual runbook~~ **Done 2026-05-24**
50. ~~Runbook missing from KB onboarding index~~ **Done 2026-05-24**
51. ~~Dev setup omits OdyPatch validate fixture path~~ **Done 2026-05-24**

## KB Doc Drift (post-C5)

| Document | Issue | Status |
|----------|-------|--------|
| `run-tools-reference.md`, `tools-ecosystem.md`, `solution-topology.md` | ~~OdyTools/OdyPatch listed as Red~~ | **Resolved 2026-05-23** |
| `docs/QUICKSTART.md`, `docs/engine_roadmap.md` | ~~OdyTools blocks OdyPatch onboarding claims~~ | **Resolved 2026-05-23** |

## KB Meta Drift

| Document | Issue | Status |
|----------|-------|--------|
| `90-meta/authority-map.md` | ~~`wiki/Home.md` listed as stale PyKotor/HoloPatcher~~ | **Resolved 2026-05-23** |

## CI Workflow Drift

| Workflow | Issue | Status |
|----------|-------|--------|
| `.github/workflows/dotnet-desktop.yml` | ~~Microsoft template placeholders / broken MSIX steps~~ | **Resolved 2026-05-23** |
| `.github/workflows/dotnet-desktop.yml` | ~~Missing ConvertKotorGame build smoke~~ | **Resolved 2026-05-23** — plan 027 |
| `.github/workflows/dotnet-desktop.yml` | ~~Missing OdyPatch.UI build~~ | **Resolved 2026-05-23** — plan 029 |
| `README.md` Building section | ~~`dotnet build src/Tools/` fails (no csproj)~~ | **Resolved 2026-05-23** — plan 029 |
| `docs/WORKFLOWS.md`, `docs/GITHUB_ACTIONS_SETUP.md` | ~~`src/OdyPatch/` paths~~ | **Resolved 2026-05-23** — plan 030 |
| `docs/NUGET.md` | ~~`src/OdyPatch/` pack/push paths~~ | **Resolved 2026-05-23** — plan 032 |
| `docs/NUGET.md` | ~~`TSLPatcher.Core` NuGet project references~~ | **Resolved 2026-05-23** — plan 033 |
| `docs/MANUAL_PUSH_INSTRUCTIONS.md`, `helper_scripts/build-nuget.*` | ~~`CSharpKOTOR` / `src/OdyPatch/` publish paths~~ | **Resolved 2026-05-23** — plan 034 |
| `OdyPatch.csproj` | ~~Deprecated SPDX `LGPL-3.0` blocks `dotnet pack` (NU5032)~~ | **Resolved 2026-05-23** — plan 035 |
| PR #2 body | ~~Stale plans table (001–032) and NUGET/TSLPatcher.Core gap claims~~ | **Resolved 2026-05-23** — plan 036 |
| `AGENTS.md`, `QUICKSTART.md`, `dev-environment-setup.md` | ~~Missing NuGet pack / helper_scripts onboarding~~ | **Resolved 2026-05-23** — plan 037 |
| `30-product-ux/` KB layer | ~~Deferred entirely (plan 001)~~ | **Stub seeded 2026-05-23** — plan 038 |
| `src/Tools/OdyPatch/README.md` | ~~Stale Andastra/TSLPatcher.Core template~~ | **Resolved 2026-05-23** — plan 039 |
| PR #2 body / `engine_roadmap.md` | ~~Stale at plan 036; missing 037–039 outcomes~~ | **Resolved 2026-05-23** — plan 040 |
| `build-and-test-ladder.md`, `tools-ecosystem.md` | ~~Missing NuGet pack + product UX links~~ | **Resolved 2026-05-23** — plan 041 |
| `agent-workflow.md`, `authority-map.md` | ~~Missing 30-product-ux / 90-meta taxonomy~~ | **Resolved 2026-05-23** — plan 042 |
| `definition-of-done.md`, `contributing-paths.md` | ~~Missing CI m1 / NuGet / 30-product-ux paths~~ | **Resolved 2026-05-23** — plan 043 |
| `OdyPatch.UI` README + run docs | ~~Missing UI README; `dotnet run` on Library csproj~~ | **Resolved 2026-05-23** — plan 044 |
| `NUGET.md`, `bioware-library-boundary.md`, `tools-ecosystem.md` | ~~OdyPatch described as host library / installer core~~ | **Resolved 2026-05-23** — plan 045 |
| `90-meta/README.md` CI table | ~~Assumed CI green without HEAD re-check~~ | **Resolved 2026-05-23** — plan 046 |
| `docs/plans/` | ~~No index for 45 LFG plan files~~ | **Resolved 2026-05-23** — plan 046 |
| `build-health-matrix.md`, `tslpatcher-domain.md` | ~~OdyPatch/UI roles merged~~ | **Resolved 2026-05-23** — plan 047 |
| `README.md`, `solution-topology.md` | ~~OdyPatch.UI listed as mod tool~~ | **Resolved 2026-05-23** — plan 048 |
| `solution-topology.md` | ~~`BioWare.Utility.csproj` path~~ | **Resolved 2026-05-24** — plan 049 (`Andastra.Utility.csproj`) |
| `90-meta/` merge tracker | ~~Pre-merge PR #2 framing~~ | **Resolved 2026-05-24** — plan 049 |
| Tier-4 `docs/*.md` | ~~No archive index; stale OdysseyRuntime in dialogue doc~~ | **Resolved 2026-05-24** — plan 050 |
| NuGet pack CI | ~~Local-only validation~~ | **Resolved 2026-05-24** — plan 051 `nuget-pack-smoke` |
| KB evidence labels | ~~Thin docs missing `[REPO]` on factual claims~~ | **Resolved 2026-05-24** — plan 052 (5 docs) |
| OdyPatch CLI --help | ~~--help launched GUI; hung headless~~ | **Resolved 2026-05-24** — plan 053 |
| PR #3 merge | ~~CONFLICTING with master after PR #2 squash~~ | **Resolved 2026-05-24** — plan 054 |
| OdyPatch validate CI | ~~No config validation without game install~~ | **Resolved 2026-05-24** — plan 055 minimal fixture |
| KB tracker post-PR #3 | ~~PR #3 listed as open after merge~~ | **Resolved 2026-05-24** — plan 056 |
| Build ladder validate | ~~OdyPatch validate only in run-tools-reference~~ | **Resolved 2026-05-24** — plan 057 |
| OdyPatch E2E runbook | ~~Install path undocumented for manual verification~~ | **Resolved 2026-05-24** — plan 058 |
| KB onboarding runbook | ~~Runbook omitted from 90-meta index and agent-workflow~~ | **Resolved 2026-05-24** — plan 059 |
| Dev setup validate | ~~dev-environment-setup omitted validate fixture~~ | **Resolved 2026-05-24** — plan 060 |
| `.github/workflows/ci.yml` | ~~No full solution build smoke test~~ | **Resolved 2026-05-23** — `solution-build` job |
| `.github/workflows/ci.yml` | ~~Parallel solution build locks `Andastra.Core.deps.json`~~ | **Resolved 2026-05-23** — plan 031 `-m:1` |

## Build Health (C5)

| Item | Issue | Status |
|------|-------|--------|
| `OdyTools.csproj` | ~~Method-group → `System.Action` in `BindLostFocus` / `BindClick`~~ | **Resolved 2026-05-23** |
| `OdyPatch` / `OdyPatch.UI` | ~~Depends on broken OdyTools; missing project reference~~ | **Resolved 2026-05-23** |

## Repo Implications

- KB is authoritative for onboarding and architecture; check this register before citing investigation docs.
- New drift entries go here and in [caveat-register.md](../90-meta/caveat-register.md).
