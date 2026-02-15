# Run OdyTools editor tests. Stops any running testhost so the build can copy DLLs, then builds and runs tests.
# Usage: .\Run-Tests.ps1   or   pwsh -File Run-Tests.ps1
# Note: If build fails with "file in use" (OdyTools.NET), close the OdyTools GUI and run again.
$ErrorActionPreference = "Stop"
$projectDir = $PSScriptRoot
$csproj = Join-Path $projectDir "OdyTools.Tests.csproj"

Write-Host "Stopping any running testhost processes so build can copy DLLs..."
Get-Process -Name "testhost" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 1

Write-Host "Building (close OdyTools GUI if build reports file in use)..."
& dotnet build $csproj -c Debug
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Running editor tests..."
& dotnet test $csproj --filter "FullyQualifiedName~OdyTools.Tests" -c Debug --no-build --logger "console;verbosity=normal"
exit $LASTEXITCODE
