# KB Maintenance Tracker

Ongoing knowledgebase and onboarding maintenance after [PR #2](https://github.com/th3w1zard1/Andastra/pull/2) (**merged** 2026-05-24 → `master` @ `f48cdaad1`).

## PR #2 outcome

| Item | Status |
|------|--------|
| Merge | **Merged** to `master` 2026-05-24 `[REPO]` |
| CI on merge commit | All checks **pass** (Solution Build, Test Builds, dotnet-desktop, Lint, Snyk) `[REPO]` |
| Scope delivered | Evidence-first KB (33 docs), plans 001–048, CI/doc drift remediations 1–39, tool-chain build recovery |

## LFG plans landed (001–048)

| Plan | Topic |
|------|-------|
| 001–029 | KB initial, CI recovery, compile fixes, onboarding |
| 030–035 | Release docs, solution-build `-m:1`, NuGet toolchain |
| 036–043 | PR merge-readiness, product UX stub, agent workflow, definition-of-done |
| 044–048 | OdyPatch host model, plans index, operational-risk sync, topology/README |

Full index: [docs/plans/README.md](../../plans/README.md)

## Local validation (green path)

| Check | Command / artifact | Status |
|-------|-------------------|--------|
| BioWare + tests | `dotnet build/test` per [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md) | Green `[REPO]` |
| Full solution | `dotnet build Andastra.sln --framework net9.0 -c Release -m:1` | Green Linux net9.0 `[REPO]` |
| Tool chain | OdyTools, OdyPatch, OdyPatch.UI, KotorCLI, ConvertKotorGame | Green net9.0 `[REPO]` |
| OdyPatch NuGet pack | `./helper_scripts/build-nuget.sh` | Green net9.0 Linux (plan 035); **not in CI** `[REPO]` |

## CI expectations (post-merge)

Re-check `gh pr checks` on PRs touching build/CI. Baseline contract from PR #2:

| Job | Notes |
|-----|-------|
| CI — Test / Lint | BioWare + Andastra.Tests ladder |
| CI — Solution Build (net9.0) | `-m:1` serialization (plan 031) |
| Test Builds matrix | net48-win, net9.0 linux/osx/win |
| dotnet-desktop | Windows BioWare + full tool stack |

## Known remaining gaps (non-blocking)

- OdyPatch mod-install UX unverified without K1/TSL install — [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md) `[OPEN]`
- `30-product-ux/` deep content deferred — stub layer only (plan 038) `[REPO]`
- AGPLv3 vs OdyPatch LGPL packaging policy — [license-and-compliance.md](../40-operational-risk/license-and-compliance.md) caveat C12 `[OPEN]`
- NuGet pack not validated in CI `[REPO]`
- Vendor PyKotor wiki under `vendor/` — reference only `[REPO]`

## Suggested next slices (049+)

| Plan | Topic |
|------|-------|
| 049 | Post-merge meta + utility csproj topology fix |
| 050+ | OdyPatch UX validation, tier-4 docs audit, NuGet CI smoke (optional) |

## Repo implications

- Update this doc when landing plan 049+ slices or when CI contract changes.
- New doc PRs should reference this tracker for maintenance context.
