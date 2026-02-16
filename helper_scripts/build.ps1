# Build script for OdyPatch.NET (PowerShell)

Write-Host "Building OdyPatch.NET..." -ForegroundColor Green

# Restore dependencies
dotnet restore OdyPatch.sln

# Build the solution
dotnet build OdyPatch.sln --configuration Release

# Run tests
Write-Host "Running tests..." -ForegroundColor Cyan
dotnet test OdyPatch.sln --configuration Release --no-build

Write-Host "Build complete!" -ForegroundColor Green

