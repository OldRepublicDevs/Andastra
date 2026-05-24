# Manual NuGet Package Push Instructions

Manual steps to verify and push the **OdyPatch** NuGet package. For overview and programmatic usage, see [NUGET.md](NUGET.md).

Package project: `src/Tools/OdyPatch/OdyPatch.csproj`

## Step 1: Build the package

**Linux / macOS:**

```bash
dotnet build src/Tools/OdyPatch/OdyPatch.csproj --configuration Release -f net9.0
dotnet pack src/Tools/OdyPatch/OdyPatch.csproj --configuration Release --no-build -p:TargetFrameworks=net9.0
```

**Windows (PowerShell):**

```powershell
dotnet pack src/Tools/OdyPatch/OdyPatch.csproj --configuration Release
```

Or use the helper script from repo root:

```bash
./helper_scripts/build-nuget.sh
```

```powershell
.\helper_scripts\build-nuget.ps1
```

## Step 2: Verify package exists

**PowerShell:**

```powershell
Get-ChildItem "src/Tools/OdyPatch/bin/Release" -Recurse -Filter "*.nupkg"
```

**Bash:**

```bash
find src/Tools/OdyPatch/bin/Release -name "*.nupkg"
```

## Step 3: Check package metadata version

**PowerShell:**

```powershell
$pkg = Get-ChildItem "src/Tools/OdyPatch/bin/Release" -Recurse -Filter "OdyPatch.*.nupkg" | Select-Object -First 1
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::OpenRead($pkg.FullName)
$nuspec = $zip.Entries | Where-Object { $_.FullName -like "*.nuspec" } | Select-Object -First 1
$stream = $nuspec.Open()
$reader = New-Object System.IO.StreamReader($stream)
$xml = $reader.ReadToEnd()
$reader.Close()
$stream.Close()
$zip.Dispose()
if ($xml -match '<version>([^<]+)</version>') { Write-Host "Package version: $($matches[1])" }
```

## Step 4: Push package

**PowerShell:**

```powershell
$apiKey = "YOUR_NUGET_API_KEY_HERE"
$pkg = (Get-ChildItem "src/Tools/OdyPatch/bin/Release" -Recurse -Filter "OdyPatch.*.nupkg" | Select-Object -First 1).FullName
dotnet nuget push $pkg --api-key $apiKey --source https://api.nuget.org/v3/index.json --skip-duplicate
```

**Bash:**

```bash
./helper_scripts/build-nuget.sh --publish --api-key YOUR_NUGET_API_KEY_HERE
```

See [NUGET_SETUP.md](NUGET_SETUP.md) for API key configuration.

## Step 5: Verify on NuGet.org

After pushing, check: <https://www.nuget.org/packages/OdyPatch/>

**Note:** The filename may include a pre-release suffix; the `<version>` in the `.nuspec` inside the package is what NuGet.org publishes.

## If push fails

Common errors:

- **403 Forbidden**: API key is invalid or expired
- **409 Conflict**: Package version already exists
- **400 Bad Request**: Package metadata is invalid

If you get a 409, the package already exists — check NuGet.org for the published version.

## Historical note

Older docs referenced `CSharpKOTOR` or `TSLPatcher.Core` packages. TSLPatcher logic now lives in `BioWare.TSLPatcher` (in-repo library); only **OdyPatch** is packable for NuGet distribution.
