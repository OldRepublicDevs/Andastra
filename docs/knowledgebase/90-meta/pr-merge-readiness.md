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

## Local validation (green path)

| Check | Command / artifact | Status |
|-------|-------------------|--------|
| BioWare + tests | `dotnet build/test` per [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md) | Green `[REPO]` |
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

## Suggested next slices (302+)

| Plan | Topic |
|------|-------|
| 302+ | Opcode-context CONSTI disambiguation (StrRef vs 2DA-memory) per plan **063** |
| 302+ | Module Designer depth, 2DA spreadsheet UX, OdyPatch E2E install runbook execution |

## Repo implications

- Update this doc when landing plan 049+ slices or when CI contract changes.
- New doc PRs should reference this tracker for maintenance context.
