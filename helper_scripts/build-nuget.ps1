# Build and optionally publish the OdyPatch NuGet package.
# Usage: .\helper_scripts\build-nuget.ps1 [--publish] [--source <feed-url>] [--api-key <key>]
#
# API Key can be provided via:
# 1. --api-key parameter (highest priority)
# 2. NUGET_API_KEY environment variable
# 3. .env file in project root (NUGET_API_KEY=...)

param(
    [switch]$Publish,
    [string]$Source = "",
    [string]$ApiKey = "",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$OdyPatchProject = "src/Tools/OdyPatch/OdyPatch.csproj"
$PackOutputDir = "src/Tools/OdyPatch/bin/$Configuration"

# Load .env file if it exists
if (Test-Path ".env") {
    Get-Content ".env" | ForEach-Object {
        if ($_ -match '^\s*([^#=]+)\s*=\s*(.+)\s*$') {
            $key = $matches[1].Trim()
            $value = $matches[2].Trim()
            [Environment]::SetEnvironmentVariable($key, $value, "Process")
        }
    }
}

if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    $ApiKey = $env:NUGET_API_KEY
}

if ([string]::IsNullOrWhiteSpace($Source)) {
    $Source = $env:NUGET_SOURCE
    if ([string]::IsNullOrWhiteSpace($Source)) {
        $Source = "https://api.nuget.org/v3/index.json"
    }
}

Write-Host "Building OdyPatch NuGet package..." -ForegroundColor Green

dotnet build $OdyPatchProject --configuration $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build OdyPatch" -ForegroundColor Red
    exit 1
}

dotnet pack $OdyPatchProject --configuration $Configuration --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build OdyPatch package" -ForegroundColor Red
    exit 1
}

$OdyPatchPackage = Get-ChildItem -Path $PackOutputDir -Recurse -Filter "OdyPatch.*.nupkg" |
    Where-Object { $_.Name -notlike "*.symbols.nupkg" } |
    Select-Object -First 1

if (-not $OdyPatchPackage) {
    Write-Host "OdyPatch package not found under $PackOutputDir" -ForegroundColor Red
    exit 1
}

Write-Host "OdyPatch package created: $($OdyPatchPackage.FullName)" -ForegroundColor Green

if ($Publish) {
    if ([string]::IsNullOrWhiteSpace($ApiKey)) {
        Write-Host "`nError: API key is required when using --publish" -ForegroundColor Red
        Write-Host "Provide it via:" -ForegroundColor Yellow
        Write-Host "  1. --api-key parameter" -ForegroundColor Yellow
        Write-Host "  2. NUGET_API_KEY environment variable" -ForegroundColor Yellow
        Write-Host "  3. .env file (NUGET_API_KEY=...)" -ForegroundColor Yellow
        exit 1
    }

    Write-Host "`nPublishing OdyPatch to $Source..." -ForegroundColor Yellow

    $pushArgs = @("nuget", "push", "--source", $Source, "--skip-duplicate", "--api-key", $ApiKey)

    & dotnet $pushArgs $OdyPatchPackage.FullName

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Failed to publish OdyPatch" -ForegroundColor Red
        exit 1
    }

    $OdyPatchSymbols = Get-ChildItem -Path $PackOutputDir -Recurse -Filter "OdyPatch.*.snupkg" | Select-Object -First 1

    if ($OdyPatchSymbols) {
        Write-Host "Publishing OdyPatch symbols..." -ForegroundColor Cyan
        & dotnet $pushArgs $OdyPatchSymbols.FullName
    }

    Write-Host "`nPackage published successfully!" -ForegroundColor Green
} else {
    Write-Host "`nPackage built successfully!" -ForegroundColor Green
    Write-Host "To publish: .\helper_scripts\build-nuget.ps1 --publish --api-key YOUR_API_KEY" -ForegroundColor Cyan
}
