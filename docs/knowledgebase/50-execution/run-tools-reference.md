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

Listed in solution — K1↔TSL portability wizard. `[REPO]` Build/run unverified on current branch.

### Standalone OdyTool editors

25+ individual csprojs under `src/Tools/OdyTools/` — prefer building specific editor csproj over AIO OdyTools. Shared standalone props include `DialogHelper.cs` (2026-05-23). `[REPO]`

### OdyTools (AIO)

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
```

Green after delegate-wiring fix (2026-05-23). `[REPO]`

## Broken / Known Failures

| Tool | Issue |
|------|-------|
| **KotorCLI** | Crashes on startup — System.CommandLine API bug `[REPO]` |

## Script Tooling

```bash
# Agent-mandated NSS/NCS operations (PowerShell)
./helper_scripts/NcsTool.ps1 --help
```

## Repo Implications

- OdyPatch compile path is green; runtime UX validation still needs a K1/TSL install.
- Prefer NSSComp/NCSDecomp for script pipeline validation in CI/agent loops.
- Product UX layer (`30-product-ux/`) remains deferred for content scope, not compile blockers.
