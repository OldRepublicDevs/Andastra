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

### Ref-search targeted filters (Steps 3b–3d)

Use after reference-search or NCS CONSTI changes — not on every build:

- **Step 3b** — NCS CONSTI / StrRef cache-path / KotorCLI find-strref / OdyTools StrRef helper
- **Step 3c** — GFF script ResRef / ReferenceFinder / installation find-refs wiring
- **Step 3d** — GFF field-value / `FieldValueReferenceHelper` template + GFF editor wiring

### Step 3b — NCS CONSTI + StrRef ref-search (when relevant)

After NCS CONSTI / find-strref / OdyTools StrRef helper changes (plans **324**–**356**):

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRefCommandTests
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InstallationRefSearchCommandCliTests
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceHelperTests
```

Expected on `master` post plan **465** Day 2 five-hop relay (plan **411**/**466**): **167** NcsConsti + **18** FindStrRefCommand + **12** CLI ref-search + **10** StrRefReferenceHelper tests pass. Post Day 1 land ([PR #135](https://github.com/th3w1zard1/Andastra/pull/135)): **165** NcsConsti (**163** plan **463** + **2** four-hop mixed relay). Prior baseline post [PR #63](https://github.com/th3w1zard1/Andastra/pull/63) (plan **374**): **98** NcsConsti. `[REPO]`

### Step 3c — ReferenceFinder + script ResRef ref-search (when relevant)

After GFF script ResRef / installation reference-search / OdyTools helper wiring changes (plan **068** arc):

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceFinderTests
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindRefsCommand
```

Expected on `master`: **97** ReferenceFinder + **36** ReferenceSearchHelper + **8** ScriptReferenceHelper + **21** FindRefsCommand tests pass. `[REPO]`

### Step 3d — Field-value ref-search (when relevant)

After OdyTools `FieldValueReferenceHelper` / template editor field-value wiring changes (plans **412**–**416**, open [PR #85](https://github.com/th3w1zard1/Andastra/pull/85)):

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelperTests
```

Expected post **#85** merge: **10** FieldValueReferenceHelper tests pass; UT* + GFF editors wired on unified stack. Prior open stack: **#81**–**#85** (plans **412**–**416**). `[REPO]`

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

OdyPatch mod install against real K1/TSL: see [odypatch-e2e-runbook.md](odypatch-e2e-runbook.md) (manual E2E; not in CI). `[REPO]`

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
