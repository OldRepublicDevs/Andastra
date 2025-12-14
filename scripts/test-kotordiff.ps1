# Test runner for KotorDiff.NET with proper output capture
$ErrorActionPreference = "Continue"

Write-Host "========================================"
Write-Host "KotorDiff.NET Test Runner"
Write-Host "========================================"
Write-Host ""

# Build first
Write-Host "Building test project..."
$buildResult = dotnet build src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "BUILD FAILED!" -ForegroundColor Red
    $buildResult | ForEach-Object { Write-Host $_ }
    exit 1
}
Write-Host "Build succeeded!" -ForegroundColor Green
Write-Host ""

# Run ResourceResolverTests
Write-Host "Running ResourceResolverTests..."
$test1 = dotnet test src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj --filter "FullyQualifiedName~ResourceResolverTests" --verbosity normal --no-build 2>&1
$test1 | ForEach-Object { Write-Host $_ }
Write-Host ""

# Run EmptyInstallations test
Write-Host "Running EmptyInstallations test..."
$test2 = dotnet test src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj --filter "FullyQualifiedName~EmptyInstallations" --verbosity normal --no-build 2>&1
$test2 | ForEach-Object { Write-Host $_ }
Write-Host ""

# Run all tests
Write-Host "Running all tests..."
$testAll = dotnet test src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj --verbosity normal --no-build 2>&1
$testAll | ForEach-Object { Write-Host $_ }

Write-Host ""
Write-Host "========================================"
Write-Host "Test execution complete"
Write-Host "========================================"


