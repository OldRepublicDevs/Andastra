# CI and Release Risks

GitHub Actions, multi-platform builds, and release automation caveats.

## Workflow Inventory

14 workflows under `.github/workflows/` including CI, multi-platform builds, release-please, NetSparkle autoupdate. `[REPO]`

Key docs: `docs/WORKFLOWS.md`, `docs/GITHUB_ACTIONS_SETUP.md`, `docs/AUTOUPDATE.md`

## CI Reliability Caveats

| Risk | Detail | Label |
|------|--------|-------|
| Stale test path | ~~`ci.yml` references `src/CSharpKOTOR.Tests/`~~ Fixed — BioWare.Tests ladder | [REPO] |
| OdyPatch path mismatch | ~~Some workflows use `src/OdyPatch/`~~ Fixed — `src/Tools/OdyPatch/` | [REPO] |
| OdyPatch/OdyTools build | Desktop CI builds OdyTools + OdyPatch + OdyPatch.UI + KotorCLI + ConvertKotorGame on Windows (2026-05-23) | [REPO] |
| No game runtime in CI | K1/TSL install not exercised in standard CI | [REPO] |
| Full solution build | `ci.yml` `solution-build` job — `Andastra.sln` net9.0 on ubuntu with `-m:1` to avoid parallel deps.json locks on shared `Andastra.Core` outputs (2026-05-23) | [REPO] |
| Windows desktop CI | `dotnet-desktop.yml` — BioWare tests + tool builds (Debug/Release matrix); may queue on busy Windows runners | [REPO] |

**`[SYNTH]`** CI green on main does not imply agent "green path" (BioWare + tests) without log inspection.

## Release / Autoupdate

- Multi-platform publish documented in `docs/WORKFLOWS.md` `[REPO]`
- NetSparkle used by OdyPatch.UI for updates `[REPO]`
- Secrets/keys for signing and autoupdate — see `docs/AUTOUPDATE.md`; treat as `[OPEN]` for contributors without org access

## NuGet

Internal package publishing docs: `docs/NUGET.md`, `docs/NUGET_SETUP.md`, `docs/MANUAL_PUSH_INSTRUCTIONS.md` `[REPO]`

OdyPatch pack/push via `helper_scripts/build-nuget.{sh,ps1}` — green on Linux net9.0 after SPDX fix (plan 035). TSLPatcher engine is in-repo (`BioWare.TSLPatcher`), not a separate NuGet package. `[REPO]`

## Repo Implications

- Do not block local KB/tool work on CI fix — document drift instead.
- Release engineering changes need workflow path audit against actual `src/Tools/` layout.
- Runtime parity claims require manual game testing outside CI.
