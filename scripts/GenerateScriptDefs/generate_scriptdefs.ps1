# Matching PyKotor implementation at vendor/PyKotor/scripts/generate_scriptdefs.ps1
# Original: Wrapper script for generate_scriptdefs.py
# This is a 1:1 equivalent wrapper for the C# GenerateScriptDefs tool
$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$projectPath = Join-Path $scriptPath "GenerateScriptDefs.csproj"
$exePath = Join-Path $scriptPath "bin" "Debug" "net8.0" "GenerateScriptDefs.exe"

# Check if project file exists
if (-not (Test-Path -LiteralPath $projectPath -ErrorAction SilentlyContinue)) {
    Write-Error "GenerateScriptDefs.csproj not found at $projectPath"
    exit 1
}

# Build the project if needed or if exe doesn't exist
if (-not (Test-Path -LiteralPath $exePath -ErrorAction SilentlyContinue)) {
    Write-Host "Building GenerateScriptDefs..."
    $buildResult = dotnet build $projectPath --configuration Debug
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to build GenerateScriptDefs"
        exit $LASTEXITCODE
    }
}

# Run the executable
& $exePath $args
exit $LASTEXITCODE

