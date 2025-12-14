$ErrorActionPreference = 'Continue'

# Run tests with performance profiling and timeout enforcement
Write-Host "Running tests with performance profiling..."
Write-Host "All tests have 2-minute timeout enforcement."
Write-Host ""

& "$PSScriptRoot\scripts\RunAllTestsWithProfiling.ps1" `
    -Projects @("src\TSLPatcher.Tests\TSLPatcher.Tests.csproj") `
    -Filter "FullyQualifiedName~NCSRoundtripTests" `
    -MaxSeconds 120 `
    -EnableProfiling:$true `
    -OutputDir "profiles"

Write-Host ""
Write-Host "Test execution completed. Check profiles/ directory for performance reports."



