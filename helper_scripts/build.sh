#!/bin/bash
# Build script for OdyPatch.NET

echo "Building OdyPatch.NET..."

# Restore dependencies
dotnet restore OdyPatch.sln

# Build the solution
dotnet build OdyPatch.sln --configuration Release

# Run tests
echo "Running tests..."
dotnet test OdyPatch.sln --configuration Release --no-build

echo "Build complete!"

