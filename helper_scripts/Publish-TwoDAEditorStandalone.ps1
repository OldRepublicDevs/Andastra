# Publish OdyTool2DA.Standalone to a single Windows EXE (no DLLs or other files).
# Uses .NET 9.0; single-file publish is not supported for .NET Framework (net472).
# Run from repo root: .\helper_scripts\Publish-TwoDAEditorStandalone.ps1

param(
    [ValidateSet("net9.0", "net472")]
    [string]$TargetFramework = "net9.0",
    [ValidateSet("win-x64", "win-x86")]
    [string]$Runtime = "win-x64",
    [string]$OutputDir = "dist\OdyTool2DA",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
$ProjectPath = Join-Path $RepoRoot "src\Tools\OdyTools\Editors\OdyTool2DA.Standalone.csproj"

if (-not (Test-Path $ProjectPath)) {
    Write-Error "Project not found: $ProjectPath"
}

# Single-file with no DLLs is only supported on .NET Core/5+ (net9.0). net472 would produce exe + many DLLs.
if ($TargetFramework -eq "net472") {
    Write-Host "Publishing for net472: output will be exe + DLLs (single-file is not supported on .NET Framework)." -ForegroundColor Yellow
    $SelfContained = $false
    $PublishSingleFile = $false
} else {
    $SelfContained = $true
    $PublishSingleFile = $true
}

$PublishDir = Join-Path $RepoRoot $OutputDir
$PublishDir = Join-Path $PublishDir "$TargetFramework-$Runtime"
if (-not (Test-Path (Split-Path $PublishDir -Parent))) {
    New-Item -ItemType Directory -Path (Split-Path $PublishDir -Parent) -Force | Out-Null
}

Write-Host "Publishing OdyTool2DA.Standalone to a single EXE (Windows)..." -ForegroundColor Cyan
Write-Host "  Framework: $TargetFramework | Runtime: $Runtime | Output: $PublishDir" -ForegroundColor Gray

$args = @(
    "publish",
    $ProjectPath,
    "-c", $Configuration,
    "-f", $TargetFramework,
    "-r", $Runtime,
    "-o", $PublishDir,
    "/p:PublishSingleFile=$PublishSingleFile",
    "/p:IncludeNativeLibrariesForSelfExtract=true",
    "/p:SelfContained=$SelfContained",
    "/p:EnableCompressionInSingleFile=true"
)

if ($PublishSingleFile) {
    # Exclude everything except the single exe from output (no pdbs, no extra files)
    $args += "/p:DebugType=None"
    $args += "/p:DebugSymbols=false"
}

& dotnet @args
if ($LASTEXITCODE -ne 0) {
    Write-Error "Publish failed with exit code $LASTEXITCODE"
}

# When single-file, the output should be one exe; remove any stray files (e.g. .pdb) if present
if ($PublishSingleFile) {
    $exeName = "OdyTool2DA.Standalone.exe"
    $exePath = Join-Path $PublishDir $exeName
    if (Test-Path $exePath) {
        Get-ChildItem $PublishDir -File | Where-Object { $_.Name -ne $exeName } | Remove-Item -Force
        Write-Host "Published single EXE: $exePath" -ForegroundColor Green
    }
} else {
    Write-Host "Published to: $PublishDir" -ForegroundColor Green
}
