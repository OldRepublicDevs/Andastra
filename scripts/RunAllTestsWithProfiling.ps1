# Comprehensive test runner with performance profiling and timeout enforcement
# Generates cProfile-like output for bottleneck analysis

param(
    [string[]]$Projects = @("src\TSLPatcher.Tests\TSLPatcher.Tests.csproj", "tests\CSharpKOTOR.Tests\CSharpKOTOR.Tests.csproj"),
    [string]$Filter = "",
    [int]$MaxSeconds = 120,
    [switch]$EnableProfiling = $true,
    [string]$OutputDir = "profiles",
    [switch]$StopOnFailure = $false
)

$ErrorActionPreference = "Continue"

Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Test Runner with Performance Profiling"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Max time per test: $MaxSeconds seconds (2 minutes)"
Write-Host "Profiling enabled: $EnableProfiling"
Write-Host "Output directory: $OutputDir"
Write-Host ""

# Create profiles directory
$profileDir = Join-Path (Get-Location) $OutputDir
if (-not (Test-Path $profileDir))
{
    New-Item -ItemType Directory -Path $profileDir -Force | Out-Null
    Write-Host "Created profiles directory: $profileDir"
}

$allResults = @()
$totalStartTime = Get-Date

foreach ($projectPath in $Projects)
{
    if (-not (Test-Path $projectPath))
    {
        Write-Warning "Test project not found: $projectPath"
        continue
    }

    Write-Host ""
    Write-Host "───────────────────────────────────────────────────────────"
    Write-Host "Running tests in: $projectPath"
    Write-Host "───────────────────────────────────────────────────────────"
    Write-Host ""

    $projectStartTime = Get-Date

    # Build filter arguments
    $testArgs = @(
        "test",
        $projectPath,
        "--no-build",
        "--logger", "console;verbosity=normal",
        "--logger", "trx;LogFileName=test-results-$([System.IO.Path]::GetFileNameWithoutExtension($projectPath)).trx"
    )

    if (-not [string]::IsNullOrEmpty($Filter))
    {
        $testArgs += "--filter"
        $testArgs += $Filter
    }

    if ($StopOnFailure)
    {
        $testArgs += "--stop-on-first-failure"
    }

    Write-Host "Executing: dotnet $($testArgs -join ' ')"
    Write-Host ""

    $result = & dotnet $testArgs 2>&1
    $projectEndTime = Get-Date
    $projectDuration = $projectEndTime - $projectStartTime

    # Write output to file
    $outputFile = "test-output-$([System.IO.Path]::GetFileNameWithoutExtension($projectPath)).txt"
    $result | Out-File -FilePath $outputFile -Encoding utf8

    # Parse results
    $passedCount = ($result | Select-String -Pattern "Passed!|passed").Count
    $failedCount = ($result | Select-String -Pattern "Failed!|failed|Failed:").Count
    $skippedCount = ($result | Select-String -Pattern "Skipped|skipped").Count

    $allResults += [PSCustomObject]@{
        Project = $projectPath
        Duration = $projectDuration
        Passed = $passedCount
        Failed = $failedCount
        Skipped = $skippedCount
    }

    Write-Host ""
    Write-Host "Project Summary:"
    Write-Host "  Duration: $($projectDuration.TotalSeconds.ToString('F2')) seconds"
    Write-Host "  Passed: $passedCount"
    Write-Host "  Failed: $failedCount"
    Write-Host "  Skipped: $skippedCount"
}

$totalEndTime = Get-Date
$totalDuration = $totalEndTime - $totalStartTime

# Display overall summary
Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Overall Test Execution Summary"
Write-Host "═══════════════════════════════════════════════════════════"
Write-Host "Total execution time: $($totalDuration.TotalSeconds.ToString('F2')) seconds"
Write-Host ""

$totalPassed = ($allResults | Measure-Object -Property Passed -Sum).Sum
$totalFailed = ($allResults | Measure-Object -Property Failed -Sum).Sum
$totalSkipped = ($allResults | Measure-Object -Property Skipped -Sum).Sum

Write-Host "Total Tests:"
Write-Host "  Passed: $totalPassed"
Write-Host "  Failed: $totalFailed"
Write-Host "  Skipped: $totalSkipped"
Write-Host ""

# Check for profile files
$profileFiles = Get-ChildItem -Path $profileDir -Filter "*.profile.txt" -ErrorAction SilentlyContinue
if ($profileFiles)
{
    Write-Host "Performance profiles generated: $($profileFiles.Count) files"
    Write-Host ""
    Write-Host "Top 10 slowest tests (by profile file size - larger files indicate longer execution):"
    Write-Host "───────────────────────────────────────────────────────────"
    
    $slowTests = $profileFiles | 
        Sort-Object Length -Descending | 
        Select-Object -First 10 | 
        ForEach-Object {
            $content = Get-Content $_.FullName -Raw
            if ($content -match "Elapsed Time: ([\d.]+) seconds")
            {
                [PSCustomObject]@{
                    Test = $_.BaseName
                    Time = [double]$matches[1]
                    File = $_.Name
                }
            }
        } | 
        Sort-Object Time -Descending

    $rank = 1
    foreach ($test in $slowTests)
    {
        Write-Host "$rank. $($test.Test): $($test.Time.ToString('F3'))s"
        $rank++
    }
    
    Write-Host ""
    Write-Host "To analyze bottlenecks, review the profile files in: $profileDir"
    Write-Host "Profile files contain detailed timing, memory, and CPU statistics."
}

# Check for tests that exceeded timeout
$timeoutTests = $result | Select-String -Pattern "exceeded.*timeout|TimeoutException|exceeded maximum execution time"
if ($timeoutTests)
{
    Write-Host ""
    Write-Host "⚠️  WARNING: Tests that exceeded timeout:"
    foreach ($timeout in $timeoutTests)
    {
        Write-Host "  - $($timeout.Line.Trim())"
    }
}

# Exit with appropriate code
if ($totalFailed -gt 0)
{
    Write-Host ""
    Write-Host "❌ Tests failed! Check test output files and profile reports for details."
    exit 1
}
else
{
    Write-Host ""
    Write-Host "✅ All tests passed!"
    exit 0
}
