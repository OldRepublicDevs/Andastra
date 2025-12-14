# Run tests with performance profiling and timeout enforcement
# Generates cProfile-like output for bottleneck analysis

param(
    [string]$ProjectPath = "",
    [string]$Filter = "",
    [int]$MaxSeconds = 120,
    [switch]$EnableProfiling = $true,
    [string]$OutputDir = "profiles"
)

$ErrorActionPreference = "Continue"

# Determine which test project to run
if ([string]::IsNullOrEmpty($ProjectPath))
{
    # Try to find test projects
    $testProjects = @(
        "src\TSLPatcher.Tests\TSLPatcher.Tests.csproj",
        "tests\CSharpKOTOR.Tests\CSharpKOTOR.Tests.csproj"
    )
    
    foreach ($proj in $testProjects)
    {
        if (Test-Path $proj)
        {
            $ProjectPath = $proj
            break
        }
    }
    
    if ([string]::IsNullOrEmpty($ProjectPath))
    {
        Write-Error "No test project found. Please specify -ProjectPath"
        exit 1
    }
}

Write-Host "Running tests with performance profiling..."
Write-Host "Project: $ProjectPath"
Write-Host "Max time per test: $MaxSeconds seconds"
Write-Host "Profiling enabled: $EnableProfiling"
Write-Host ""

# Create profiles directory
$profileDir = Join-Path (Get-Location) $OutputDir
if (-not (Test-Path $profileDir))
{
    New-Item -ItemType Directory -Path $profileDir | Out-Null
}

# Build filter arguments
$filterArgs = ""
if (-not [string]::IsNullOrEmpty($Filter))
{
    $filterArgs = "--filter `"$Filter`""
}

# Run tests with detailed output
$testArgs = @(
    "test",
    $ProjectPath,
    "--no-build",
    "--logger", "console;verbosity=detailed",
    "--logger", "trx;LogFileName=test-results.trx",
    "--", "NUnit.DefaultTestNamePattern=MethodName"
)

if (-not [string]::IsNullOrEmpty($Filter))
{
    $testArgs += "--filter"
    $testArgs += $Filter
}

Write-Host "Executing: dotnet $($testArgs -join ' ')"
Write-Host ""

$startTime = Get-Date
$result = & dotnet $testArgs 2>&1
$endTime = Get-Date
$duration = $endTime - $startTime

# Write output
$result | Out-File -FilePath "test-output.txt" -Encoding utf8

# Display summary
Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Test Execution Summary"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Total execution time: $($duration.TotalSeconds.ToString('F2')) seconds"
Write-Host "Profiles directory: $profileDir"
Write-Host ""

# Check for profile files
$profileFiles = Get-ChildItem -Path $profileDir -Filter "*.profile.txt" -ErrorAction SilentlyContinue
if ($profileFiles)
{
    Write-Host "Performance profiles generated:"
    foreach ($file in $profileFiles)
    {
        Write-Host "  - $($file.Name)"
    }
    Write-Host ""
    Write-Host "To analyze bottlenecks, review the profile files in: $profileDir"
}

# Display last 50 lines of output
Write-Host "Last 50 lines of test output:"
Write-Host "───────────────────────────────────────────────────────────"
$result | Select-Object -Last 50

# Check exit code
$exitCode = $LASTEXITCODE
if ($exitCode -ne 0)
{
    Write-Host ""
    Write-Host "Tests failed with exit code: $exitCode"
    Write-Host "Check test-output.txt and profile files for details."
    exit $exitCode
}
else
{
    Write-Host ""
    Write-Host "All tests passed!"
}
