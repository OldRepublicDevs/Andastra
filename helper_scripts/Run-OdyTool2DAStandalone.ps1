# Run OdyTool2DA.Standalone (2DA editor) from repo root.
# Builds and runs for net9.0 only to avoid net48 restore/type-forwarder issues.
# Usage from repo root: .\helper_scripts\Run-OdyTool2DAStandalone.ps1

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
$ProjectPath = Join-Path $RepoRoot "src\Tools\OdyTools\Editors\OdyTool2DA.Standalone.csproj"

if (-not (Test-Path $ProjectPath)) {
    Write-Error "Project not found: $ProjectPath. Run from repo root."
}

Push-Location $RepoRoot
try {
    dotnet run -f net9.0 --project $ProjectPath @args
} finally {
    Pop-Location
}
