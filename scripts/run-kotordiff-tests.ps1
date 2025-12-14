# Run KotorDiff.NET tests and capture output
$ErrorActionPreference = "Continue"
$output = @()

Write-Host "Building test project..."
$buildOutput = dotnet build src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj 2>&1
$output += $buildOutput
$output | ForEach-Object { Write-Host $_ }

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!"
    exit 1
}

Write-Host "`nRunning tests..."
$testOutput = dotnet test src/KotorDiff.NET.Tests/KotorDiff.NET.Tests.csproj --verbosity normal --logger "console;verbosity=normal" 2>&1
$output += $testOutput
$output | ForEach-Object { Write-Host $_ }

Write-Host "`nTest execution complete. Exit code: $LASTEXITCODE"
exit $LASTEXITCODE


