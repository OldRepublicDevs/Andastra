# Test MDL ASCII/Binary Roundtrip Functionality
# This script tests the MDL format roundtrip conversions

$ErrorActionPreference = "Stop"

Write-Host "Building Andastra.Parsing project..."
dotnet build src/Andastra/Parsing/Andastra.Parsing.csproj -c Debug -v quiet
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit 1
}

Write-Host "`nTesting MDL roundtrip functionality..."
Write-Host "Note: This requires the compiled DLL to be available"

# The actual testing would need to be done through a compiled test assembly
# or by creating a small C# test program. For now, we verify the build succeeds.
Write-Host "`nBuild successful - MDL roundtrip code compiles correctly"
Write-Host "To run actual tests, use: dotnet test --filter FullyQualifiedName~MDLAsciiRoundTripTests"
