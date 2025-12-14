# Script to add timeout attributes to all test methods
# This ensures all tests fail if they exceed 2 minutes

param(
    [string]$TestProjectPath = "src\TSLPatcher.Tests\TSLPatcher.Tests.csproj"
)

$ErrorActionPreference = "Continue"

Write-Host "Adding timeout attributes to all test methods..."
Write-Host "Project: $TestProjectPath"
Write-Host ""

# Find all test files
$testFiles = Get-ChildItem -Path "src\TSLPatcher.Tests" -Filter "*.cs" -Recurse | Where-Object {
    $content = Get-Content $_.FullName -Raw
    $content -match '\[Fact\]|\[Theory\]'
}

$modifiedCount = 0

foreach ($file in $testFiles)
{
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Add timeout to [Fact] attributes
    $content = $content -replace '\[Fact\]', '[Fact(Timeout = 120000)] // 2 minutes timeout'
    
    # Add timeout to [Theory] attributes
    $content = $content -replace '\[Theory\]', '[Theory(Timeout = 120000)] // 2 minutes timeout'
    
    if ($content -ne $originalContent)
    {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Updated: $($file.Name)"
        $modifiedCount++
    }
}

Write-Host ""
Write-Host "Updated $modifiedCount test files with timeout attributes."
Write-Host "All tests will now fail if they exceed 2 minutes."
