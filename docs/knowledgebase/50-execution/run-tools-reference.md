# Run Tools Reference

CLI entry points and known working vs broken tools.

## Working CLIs

### NSSComp (NWScript compiler)

```bash
dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0 -- --help
```

### NCSDecomp.CLI (NWScript decompiler)

```bash
dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0 -- --help
```

### KotorDiff

Build and run per project README/tool docs. Uses BioWare installation model. `[REPO]`

## GUI Tools

### OdyPatch.UI

```bash
dotnet run --project src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
```

**Caveat:** Compiles on net9.0 (2026-05-23); end-to-end mod-install UX is unverified without a game install. `[REPO]`

### OdyPatch (host)

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

### ConvertKotorGame

```bash
dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0
```

K1↔TSL portability wizard — builds on net9.0 (2026-05-23). `[REPO]`

### Standalone OdyTool editors

25+ individual csprojs under `src/Tools/OdyTools/` — prefer building specific editor csproj over AIO OdyTools. Shared standalone props include `DialogHelper.cs` (2026-05-23). `[REPO]`

### OdyTools (AIO)

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
```

Green after delegate-wiring fix (2026-05-23). `[REPO]`

### KotorCLI

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help
```

System.CommandLine 2.0 Option API fixed 2026-05-23. `[REPO]`

## NuGet pack (OdyPatch)

```bash
./helper_scripts/build-nuget.sh
```

Produces `OdyPatch.*.nupkg` under `src/Tools/OdyPatch/bin/Release/` (Linux uses net9.0). Requires valid SPDX `PackageLicenseExpression` in `OdyPatch.csproj` (`LGPL-3.0-only` as of plan 035). See [NUGET.md](../../NUGET.md). `[REPO]`

## Broken / Known Failures

| Tool | Issue |
|------|-------|
| *(none in tool chain)* | Isolated Stride assembly processor failures on Linux game-only builds `[REPO]` |

## Script Tooling

```bash
# Agent-mandated NSS/NCS operations (PowerShell)
./helper_scripts/NcsTool.ps1 --help
```

## Repo Implications

- OdyPatch compile path is green; runtime UX validation still needs a K1/TSL install.
- Prefer NSSComp/NCSDecomp for script pipeline validation in CI/agent loops.
- Product UX layer (`30-product-ux/`) remains deferred for content scope, not compile blockers.
