# Build and Test Ladder

Narrowest-first validation path — use before claiming work complete.

## Ladder (in order)

### Step 1 — BioWare library

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
```

### Step 2 — BioWare tests build

```bash
dotnet build tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0
```

### Step 3 — BioWare tests run

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0
```

### Step 4 — Andastra tests

```bash
dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0
```

### Step 5 — Targeted tool (if relevant)

```bash
dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0 -- --help
dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0 -- --help
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- --help
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- \
  --validate --game-dir tests/fixtures/odypatch-fake-game \
  --tslpatchdata tests/fixtures/odypatch-minimal-mod/tslpatchdata
dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0
```

OdyPatch `--help` and `--validate` (minimal fixture, no game install) mirror CI `nuget-pack-smoke` (plans 053/055). Build Release first or use `--no-build` after `build-nuget.sh`. `[REPO]`

OdyPatch installer GUI: `dotnet run` the **OdyPatch** host only (see [run-tools-reference.md](run-tools-reference.md)). `[REPO]`

### Step 6 — Runtime / Game (optional, needs game install)

```bash
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```

### Step 7 — Full solution

```bash
dotnet build Andastra.sln --framework net9.0
```

Succeeds on Linux net9.0 after standalone obj/bin isolation (2026-05-23). `[REPO]`

CI `solution-build` uses `-m:1` to avoid parallel `Andastra.Core.deps.json` locks (plan 031). Use locally only if you hit the same lock:

```bash
dotnet build Andastra.sln --framework net9.0 -c Release -m:1
```

Release + full test pass optional:

```bash
dotnet build Andastra.sln --configuration Release --framework net9.0
dotnet test Andastra.sln --framework net9.0 --verbosity normal
```

## Analyzers

```bash
dotnet build src/BioWare/BioWare.csproj --configuration Release -p:RunAnalyzersDuringBuild=true --framework net9.0
```

## Skip Unless Fixing

- Isolated Stride assembly processor failures on single game csproj Linux builds `[REPO]`
- Orphan test files not wired into test csprojs — see [build-health-matrix](../40-operational-risk/build-health-matrix.md) `[REPO]`

## Tool chain (when relevant)

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0
# GUI/CLI host (launches OdyPatch.UI) — do not dotnet run the UI library csproj
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help
dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0
```

### Optional — OdyPatch NuGet pack

```bash
./helper_scripts/build-nuget.sh
```

See [run-tools-reference.md](run-tools-reference.md) and [NUGET.md](../../NUGET.md). Product UX boundaries: [30-product-ux/](../30-product-ux/README.md). `[REPO]`

## Repo Implications

- Vertical-slice validation should pass Steps 1–4 for format/script changes minimum.
- Full solution net9.0 build succeeds on Linux after plan 020 standalone obj isolation.
- C# changes in net48-only code paths need explicit net48 build on Windows CI or local Windows.
