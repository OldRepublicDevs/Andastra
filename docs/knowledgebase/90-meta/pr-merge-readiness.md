# KB Maintenance Tracker

Ongoing knowledgebase and onboarding maintenance. Initial KB landed via [PR #2](https://github.com/th3w1zard1/Andastra/pull/2); follow-up plans 049–055 via [PR #3](https://github.com/th3w1zard1/Andastra/pull/3) (both merged 2026-05-24).

## PR #2 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-24 `[REPO]` |
| CI on merge commit | All checks **pass** (Solution Build, Test Builds, dotnet-desktop, Lint, Snyk) `[REPO]` |
| Scope delivered | Evidence-first KB (33 docs), plans 001–048, CI/doc drift remediations 1–39, tool-chain build recovery |

## PR #3 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-24 @ `bd06cca62` `[REPO]` |
| CI on PR | All checks **pass** including new `NuGet Pack Smoke (OdyPatch)` (help + validate smoke) `[REPO]` |
| Scope delivered | Plans 049–055: post-merge meta, investigation index, NuGet CI, evidence labels, OdyPatch CLI help fix, validate fixture |

## LFG plans landed (001–063)

| Plan | Topic |
|------|-------|
| 001–048 | See [docs/plans/README.md](../../plans/README.md) |
| 049 | Post-merge meta + utility csproj topology fix |
| 050 | Investigation archive index + stale path fixes |
| 051 | OdyPatch NuGet pack CI smoke job |
| 052 | Evidence label pass on thin execution/domain docs |
| 053 | OdyPatch CLI --help fix + CI smoke |
| 054 | PR #3 merge conflict resolution with master |
| 055 | OdyPatch validate fixture + CI smoke |
| 056 | Post-PR #3 maintenance tracker refresh |
| 057 | Build ladder OdyPatch validate sync (PR #4) |
| 058 | OdyPatch E2E manual runbook (PR #4) |
| 059 | Onboarding docs runbook sync (PR #4) |
| 060 | Dev setup and tools ecosystem validate sync (PR #4) |
| 061 | AGENTS.md and build health validate sync (PR #4) |
| 062 | OdyPatch README validate sync + local verify (PR #4) |
| 063 | PR #4 merge readiness gate (PR #4) |

Full index: [docs/plans/README.md](../../plans/README.md)

## PR #4 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` (plans 056–063) `[REPO]` |
| Scope | Post-PR #3 tracker refresh; OdyPatch validate fixture + E2E runbook documentation arc |

## PR #12 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `9860ada2a` via [PR #12](https://github.com/th3w1zard1/Andastra/pull/12) `[REPO]` |
| Scope | Post-PR-#11 KB/CLI verification (**291**–**296**), OdyPatch `--validate` subprocess test (**297**), merge-readiness closure (**298**) |
| Plans | **291**–**298** (merge-readiness **296**, **298**) |
| Test coverage | KotorCLI **369/369**; OdyPatch validate CLI **1/1** via `tests/OdyPatch.Tests/` in CI `test` job (plan **297**) `[REPO]` |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #13 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `360e48647` via [PR #13](https://github.com/th3w1zard1/Andastra/pull/13) `[REPO]` |
| Scope | Post-PR-#12 tracker sync (plan **299**): `pr-merge-readiness.md` PR #12 outcome, plan index, plan **063** milestone |
| Plans | **299** (tracker sync) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #14 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `64a3308d2` via [PR #14](https://github.com/th3w1zard1/Andastra/pull/14) `[REPO]` |
| Scope | Post-PR-#13 tracker sync (plan **300**): PR #13 outcome, plan index, plan **063** milestone |
| Plans | **300** (tracker sync) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #15 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `8b3c6b244` via [PR #15](https://github.com/th3w1zard1/Andastra/pull/15) `[REPO]` |
| Scope | Post-PR-#14 tracker sync (plan **301**): PR #14 outcome, plan index, plan **063** milestone |
| Plans | **301**–**302** (merge + tracker sync **302**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #16 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `75fc00744` via [PR #16](https://github.com/th3w1zard1/Andastra/pull/16) `[REPO]` |
| Scope | Post-PR-#15 tracker sync (plan **302**): PR #15 outcome, plan index, plan **063** milestone |
| Plans | **302** |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #17 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `0f7c5bf1e` via [PR #17](https://github.com/th3w1zard1/Andastra/pull/17) `[REPO]` |
| Scope | NCS CONSTI opcode-context StrRef disambiguation v1 (plan **303**): `GetConstiUsageContext`, `ShouldIndexAsStrRefCandidate`, **14** NcsConsti tests |
| Plans | **303**–**304** (merge + tracker sync **304**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #19 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `4e3ad2f6a` via [PR #19](https://github.com/th3w1zard1/Andastra/pull/19) `[REPO]` |
| Scope | NCS CONSTI action-signature StrRef slot matching (plan **305**): `ScriptDefs` StrRef param map, multi-arg ACTION stack-slot alignment, **17** NcsConsti tests |
| Plans | **305**–**306** (merge + tracker sync **306**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #21 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-28 @ `d17ff8cde` via [PR #21](https://github.com/th3w1zard1/Andastra/pull/21) `[REPO]` |
| Scope | NCS CONSTI stack-store cache exclusion (plan **307**): `StackStored` context, CPTOPSP run-break, **20** NcsConsti tests |
| Plans | **307**–**308** (merge + tracker sync **308**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #23 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-29 @ `ced72418b` via [PR #23](https://github.com/th3w1zard1/Andastra/pull/23) `[REPO]` |
| Scope | NCS CONSTI variable StrRef CPTOPSP forward trace (plan **309**): `MOVSP`-adjusted reload + StrRef ACTION slot match, **25** NcsConsti tests |
| Plans | **309**–**310** (merge + tracker sync **310**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #25 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-29 @ `08bd4a3a3` via [PR #25](https://github.com/th3w1zard1/Andastra/pull/25) `[REPO]` |
| Scope | NCS CONSTI global StrRef CPTOPBP cross-subroutine trace (plan **311**): full-file BP offset match for `CPDOWNBP`→`CPTOPBP` flows, **29** NcsConsti tests |
| Plans | **311**–**312** (merge + tracker sync **312**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #27 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-29 @ `76855e679` via [PR #27](https://github.com/th3w1zard1/Andastra/pull/27) `[REPO]` |
| Scope | NCS CONSTI multi-hop local StrRef trace (plan **313**): chained `CPTOPSP`→`CPDOWNSP` relay for `n→m→ACTION`, **31** NcsConsti tests |
| Plans | **313**–**314** (merge + tracker sync **314**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #29 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-29 @ `58a2697fe` via [PR #29](https://github.com/th3w1zard1/Andastra/pull/29) `[REPO]` |
| Scope | NCS CONSTI instruction-aligned CPTOPBP scan (plan **315**): `GetInstructionSizeAt` walk replaces byte scan; extended opcode sizes for cross-subroutine files, **31** NcsConsti tests |
| Plans | **315**–**316** (merge + tracker sync **316**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #31 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-29 @ `326b812a1` via [PR #31](https://github.com/th3w1zard1/Andastra/pull/31) `[REPO]` |
| Scope | NCS CONSTI deep multi-hop local StrRef trace (plan **317**): instruction-aligned `CPDOWNSP` relay discovery + `n→m→k→ACTION`, **33** NcsConsti tests |
| Plans | **317**–**318** (merge + tracker sync **318**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #34 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-30 @ `4514f2b05` via [PR #34](https://github.com/th3w1zard1/Andastra/pull/34) `[REPO]` |
| Scope | NCS CONSTI BP multi-hop + full-file cross-sub relay (plans **321**–**322**): CPTOPBP relay hop in forward scan; `TryFindStrRefConsumerViaBpReload` relay recursion + `GetInstructionStepSizeAt`; **37** NcsConsti tests at merge |
| Plans | **321**–**323** (feature **321**/**322** + tracker sync **323**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #33 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-30 @ `8bdf07844` via [PR #33](https://github.com/th3w1zard1/Andastra/pull/33) `[REPO]` |
| Scope | NCS CONSTI `GetInstructionSizeAt` walk hardening (plan **319**): scoped `GetInstructionStepSizeAt` for BP full-file walks; `GetInstructionSizeAt` returns 0 for unknown opcodes in forward/relay scans; **37** NcsConsti tests at merge |
| Plans | **319**–**320** (feature + tracker sync **320**) |
| CI on merge | All meaningful checks **pass**; `code/snyk` quota failure documented as non-blocking `[REPO]` |

## PR #36 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-06-03 @ `f49c2a028` via [PR #36](https://github.com/th3w1zard1/Andastra/pull/36) `[REPO]` |
| Scope | NCS CONSTI control-flow arc (plans **324**–**343**): scanner + cache/subroutine tests + KotorCLI gating + BioWare slow/cache tests + README/KB docs + plan 063 sync; **74** NcsConsti + **18** FindStrRef + **12** CLI ref-search tests |
| Plans | **324**–**344** (feature **324**–**335**, **337**–**339**; docs **340**–**344**; tracker sync **325**, **336**, **344**) |
| CI on merge | Solution Build, Test, Lint **pass**; duplicate CodeQL + `code/snyk` quota documented as non-blocking `[REPO]` |

## PR #37 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-06-03 @ `211a82204` via [PR #37](https://github.com/th3w1zard1/Andastra/pull/37) `[REPO]` |
| Scope | OdyTools `StrRefReferenceHelper` cache-path NCS gating (plan **346**): builds `StrRefReferenceCache` when `IncludeNcsStrRefScan`; **10** StrRefReferenceHelper tests |
| Plans | **346**–**347** (feature **346** + tracker sync **347**) |
| CI on merge | Solution Build, Test, Lint **pass**; duplicate CodeQL + `code/snyk` quota documented as non-blocking `[REPO]` |

## PR #38 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-06-03 @ `922e1f1a0` via [PR #38](https://github.com/th3w1zard1/Andastra/pull/38) `[REPO]` |
| Scope | build-and-test-ladder **Step 3b** StrRef ref-search filter commands (plan **348**): NcsConsti (**74**), FindStrRefCommand (**18**), InstallationRefSearch CLI (**12**), StrRefReferenceHelper (**10**) |
| Plans | **348**–**349** (docs **348** + tracker sync **349**) |
| CI on merge | Solution Build, Test, Lint **pass**; duplicate CodeQL + `code/snyk` quota documented as non-blocking `[REPO]` |

## PR #40 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-06-03 @ `a517c0fd4` via [PR #40](https://github.com/th3w1zard1/Andastra/pull/40) `[REPO]` |
| Scope | build-and-test-ladder **Step 3c** ReferenceFinder filter commands (plan **350**): ReferenceFinderTests (**97**), ReferenceSearchHelper (**36**), ScriptReferenceHelper (**8**), FindRefsCommand (**21**); stale plan **286** NcsConsti count fix in `odytools-editor-ux.md` |
| Plans | **350**–**351** (docs **350** + tracker sync **351**) |
| CI on merge | Solution Build, Test, Lint **pass**; duplicate CodeQL + `code/snyk` quota documented as non-blocking `[REPO]` |

## PR #42 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-06-03 @ `3cc980ddc` via [PR #42](https://github.com/th3w1zard1/Andastra/pull/42) `[REPO]` |
| Scope | plan **063** + KB sync with build-ladder **Step 3b**/**3c** ref-search filters (plan **352**): cross-links PR **#37**–**#41** arc; verification row in `odytools-editor-ux.md` |
| Plans | **352**–**353** (docs **352** + tracker sync **353**) |
| CI on merge | Solution Build, Test, Lint **pass**; duplicate CodeQL + `code/snyk` quota documented as non-blocking `[REPO]` |

## Local validation (green path)

| Check | Command / artifact | Status |
|-------|-------------------|--------|
| BioWare + tests | `dotnet build/test` per [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md) | Green `[REPO]` |
| Ref-search Step 3b (NCS CONSTI / StrRef) | `--filter` NcsConsti (**74**), FindStrRefCommand (**18**), InstallationRefSearch CLI (**12**), StrRefReferenceHelper (**10**) | Green `[REPO]` (plans **348**–**353**, 2026-06-03) |
| Ref-search Step 3c (ReferenceFinder) | `--filter` ReferenceFinderTests (**97**), ReferenceSearchHelper (**36**), ScriptReferenceHelper (**8**), FindRefsCommand (**21**) | Green `[REPO]` (plans **348**–**353**, 2026-06-03) |
| Full solution | `dotnet build Andastra.sln --framework net9.0 -c Release -m:1` | Green Linux net9.0 `[REPO]` |
| Tool chain | OdyTools, OdyPatch, OdyPatch.UI, KotorCLI, ConvertKotorGame | Green net9.0 `[REPO]` |
| OdyPatch NuGet pack | `./helper_scripts/build-nuget.sh` | Green net9.0 Linux; CI `nuget-pack-smoke` (help + validate smoke, plans 051/053/055) `[REPO]` |

## CI expectations (post-merge)

Re-check `gh pr checks` on PRs touching build/CI. Baseline contract from PR #2:

| Job | Notes |
|-----|-------|
| CI — Test / Lint | BioWare + Andastra.Tests + OdyPatch.Tests ladder (plan 297) |
| CI — Solution Build (net9.0) | `-m:1` serialization (plan 031) |
| CI — NuGet Pack Smoke | `nuget-pack-smoke` — pack + CLI `--help` + `--validate` fixture (plans 051/053/055) |
| Test Builds matrix | net48-win, net9.0 linux/osx/win |
| dotnet-desktop | Windows BioWare + full tool stack |

## Known remaining gaps (non-blocking)

- OdyPatch mod-install UX unverified without K1/TSL install — [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md) `[OPEN]`
- `30-product-ux/` deep content deferred — stub layer only (plan 038) `[REPO]`
- AGPLv3 vs OdyPatch LGPL packaging policy — [license-and-compliance.md](../40-operational-risk/license-and-compliance.md) caveat C12 `[OPEN]`
- NuGet pack validated in CI via `nuget-pack-smoke` (plan 051); publish still manual `[REPO]`
- Vendor PyKotor wiki under `vendor/` — reference only `[REPO]`

## Suggested next slices (353+)

| Plan | Topic |
|------|-------|
| 353+ | Full CONSTI stack simulation for exotic control-flow per plan **063** |
| 353+ | Module Designer depth, 2DA spreadsheet UX, OdyPatch E2E install runbook execution |
| 353+ | ReferenceFinder / OdyTools vertical slices per plan **063** backlog |

## Repo implications

- Update this doc when landing plan 049+ slices or when CI contract changes.
- New doc PRs should reference this tracker for maintenance context.
