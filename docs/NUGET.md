# NuGet Package Distribution

**OdyPatch** (`src/Tools/OdyPatch/`) is configured as a packable NuGet package (`IsPackable=true`). The TSLPatcher engine lives in **`src/BioWare/TSLPatcher/`** as the `BioWare.TSLPatcher` assembly — use a **project reference** to BioWare (or the TSLPatcher csproj) in-repo, not a separate `TSLPatcher.Core` package (that project does not exist). See [tslpatcher-domain](knowledgebase/20-domain-theory/tslpatcher-domain.md).

## Building NuGet Packages

### OdyPatch (packable)

```bash
dotnet pack src/Tools/OdyPatch/OdyPatch.csproj --configuration Release --framework net9.0
```

Output (version from csproj):

```text
src/Tools/OdyPatch/bin/Release/OdyPatch.<version>.nupkg
```

### TSLPatcher engine (in-repo reference)

For programmatic patching inside this solution, reference BioWare projects directly:

```bash
dotnet build src/BioWare/TSLPatcher/BioWare.NET.TSLPatcher.csproj --framework net9.0
```

BioWare.NET.TSLPatcher is **not** currently published as its own NuGet package.

## Installing OdyPatch

### From a local package feed

```bash
mkdir -p nuget-packages
cp src/Tools/OdyPatch/bin/Release/OdyPatch.*.nupkg nuget-packages/
dotnet add package OdyPatch --source ./nuget-packages
```

### From NuGet.org (after publishing)

```bash
dotnet add package OdyPatch
```

Or in `.csproj`:

```xml
<PackageReference Include="OdyPatch" Version="1.0.0-alpha1" />
```

## Using the libraries

### Programmatic mod install (BioWare.TSLPatcher)

```csharp
using BioWare.TSLPatcher;
using BioWare.TSLPatcher.Logger;

var logger = new PatchLogger();
var installer = new ModInstaller(
    modPath: @"C:\Mods\MyMod",
    gamePath: @"C:\Games\KOTOR2",
    changesIniPath: @"C:\Mods\MyMod\changes.ini",
    logger: logger
);

installer.Install();
```

Requires project references to BioWare TSLPatcher modules (see `OdyPatch.csproj`).

### OdyPatch package

The OdyPatch NuGet packages the patcher host library; UI lives in **OdyPatch.UI** (separate csproj, not necessarily published).

## Publishing to NuGet.org

1. Create an API key at [nuget.org](https://www.nuget.org) → Account Settings → API Keys.
2. Publish OdyPatch:

```bash
dotnet nuget push src/Tools/OdyPatch/bin/Release/OdyPatch.*.nupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

Symbol packages (if generated):

```bash
dotnet nuget push src/Tools/OdyPatch/bin/Release/OdyPatch.*.snupkg \
  --api-key YOUR_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

See also [NUGET_SETUP.md](NUGET_SETUP.md) for API key configuration.

## Package dependencies

- **BioWare.TSLPatcher**: In-repo library; depends on BioWare Core, Resource, Common, etc.
- **OdyPatch**: NuGet-packable host; depends on OdyTools + BioWare stack (see `OdyPatch.csproj`).

## Version management

Update version in `src/Tools/OdyPatch/OdyPatch.csproj`:

```xml
<Version>1.0.0-alpha1</Version>
```

Follow [Semantic Versioning](https://semver.org/) for releases.

## Notes

- OdyPatch targets `net9.0` and `net48` (see csproj). On Linux, pass `--framework net9.0` for pack/build.
- Historical docs referring to `TSLPatcher.Core` describe a pre-Andastra layout; use `BioWare.TSLPatcher` namespaces instead.
