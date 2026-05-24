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

**Caveat:** Build fails until OdyTools chain is fixed. `[REPO]`

### ConvertKotorGame

Listed in solution — K1↔TSL portability wizard. `[REPO]` Build/run unverified on current branch.

### Standalone OdyTool editors

25+ individual csprojs under `src/Tools/OdyTools/` — prefer building specific editor csproj over AIO OdyTools. `[REPO]`

## Broken / Known Failures

| Tool | Issue |
|------|-------|
| **KotorCLI** | Crashes on startup — System.CommandLine API bug `[REPO]` |
| **OdyTools (AIO)** | Compile errors — blocks OdyPatch `[REPO]` |

## Script Tooling

```bash
# Agent-mandated NSS/NCS operations (PowerShell)
./helper_scripts/NcsTool.ps1 --help
```

## Repo Implications

- Mod installer UX testing blocked on OdyTools fix — document status, don't claim OdyPatch works end-to-end.
- Prefer NSSComp/NCSDecomp for script pipeline validation in CI/agent loops.
- Product UX layer (`30-product-ux`) deferred until tools build green.
