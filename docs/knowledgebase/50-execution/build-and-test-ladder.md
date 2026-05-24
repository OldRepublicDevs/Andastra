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
```

### Step 6 — Runtime / Game (optional, needs game install)

```bash
dotnet build src/Andastra/Game/Andastra.Game.csproj --framework net9.0
```

### Step 7 — Full solution (may fail on Stride assembly processor on Linux)

```bash
dotnet build Andastra.sln --configuration Release
dotnet test Andastra.sln --verbosity normal
```

## Analyzers

```bash
dotnet build src/BioWare/BioWare.csproj --configuration Release -p:RunAnalyzersDuringBuild=true --framework net9.0
```

## Skip Unless Fixing

- Full solution Stride assembly processor on Linux `[REPO]`
- Missing solution stubs (`MonoGameFPS`, `StrideGameFPS`) — not in solution; restore succeeds `[REPO]`

## Tool chain (when relevant)

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

## Repo Implications

- Vertical-slice validation should pass Steps 1–4 for format/script changes minimum.
- Full solution may still fail on orphan projects — do not misreport as environment issue.
- C# changes in net48-only code paths need explicit net48 build on Windows CI or local Windows.
