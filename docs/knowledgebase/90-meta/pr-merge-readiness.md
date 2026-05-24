# PR Merge Readiness (PR #2)

Merge-readiness tracker for branch `docs/feat-knowledgebase-initial` and [PR #2](https://github.com/th3w1zard1/Andastra/pull/2).

## Scope

Evidence-first knowledgebase, onboarding docs, CI/doc drift remediation (plans 001–044), and full tool-chain + solution build recovery.

## Plans landed (001–044)

| Plan | Topic |
|------|-------|
| 001–029 | KB initial, CI recovery, compile fixes, onboarding |
| 030 | Release doc OdyPatch paths + README run commands |
| 031 | Serialize solution-build CI (`-m:1`) |
| 032 | NUGET OdyPatch paths + CI build notes sync |
| 033 | Reframe NUGET.md for BioWare.TSLPatcher + OdyPatch |
| 034 | Fix NuGet publish scripts + MANUAL_PUSH_INSTRUCTIONS |
| 035 | OdyPatch NuGet pack SPDX (`LGPL-3.0-only`) |
| 036 | PR merge-readiness doc + PR #2 body refresh |
| 037 | Sync AGENTS/QUICKSTART/dev-setup with NuGet toolchain |
| 038 | Seed `30-product-ux/` stub layer |
| 039 | Fix OdyPatch README naming and layout drift |
| 040 | Refresh PR #2 body + sync engine_roadmap |
| 041 | Sync build-and-test-ladder + tools-ecosystem |
| 042 | Sync agent-workflow + authority-map taxonomy |
| 043 | Sync definition-of-done + contributing-paths |
| 044 | OdyPatch.UI README + GUI entry-point correction |

Full plan files: `docs/plans/2026-05-23-NNN-*.md`

## Local validation (green path)

| Check | Command / artifact | Status |
|-------|-------------------|--------|
| BioWare + tests | `dotnet build/test` per [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md) | Green `[REPO]` |
| Full solution | `dotnet build Andastra.sln --framework net9.0 -c Release -m:1` | Green Linux net9.0 `[REPO]` |
| Tool chain | OdyTools, OdyPatch, OdyPatch.UI, KotorCLI, ConvertKotorGame | Green net9.0 `[REPO]` |
| OdyPatch NuGet pack | `./helper_scripts/build-nuget.sh` | Green net9.0 Linux (plan 035) `[REPO]` |

## CI expectations

| Job | Notes |
|-----|-------|
| CI — Test / Lint | BioWare + Andastra.Tests ladder |
| CI — Solution Build (net9.0) | `-m:1` serialization (plan 031) |
| Test Builds matrix | net48-win, net9.0 linux/osx/win |
| dotnet-desktop | Windows BioWare + full tool stack |

Re-check `gh pr checks 2` on latest HEAD before merge. `[REPO]`

## Known remaining gaps (non-blocking)

- OdyPatch mod-install UX unverified without K1/TSL install — stub doc: [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md) `[OPEN]`
- `30-product-ux/` deep content deferred — stub layer only (plan 038) `[REPO]`
- AGPLv3 vs OdyPatch LGPL packaging policy — see [license-and-compliance.md](../40-operational-risk/license-and-compliance.md) caveat C12 `[OPEN]`
- Vendor PyKotor wiki corpus under `vendor/` — reference only `[REPO]`

## Repo implications

- Update this doc when adding plan 036+ slices or when CI contract changes.
- PR body should stay aligned with this checklist on merge-readiness passes.
