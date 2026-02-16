#!/bin/bash
# Build NuGet packages for CSharpKOTOR and OdyPatch
# Usage: ./build-nuget.sh [--publish] [--source <feed-url>] [--api-key <key>]

set -e

PUBLISH=false
SOURCE="https://api.nuget.org/v3/index.json"
API_KEY=""
CONFIGURATION="Release"

while [[ $# -gt 0 ]]; do
    case $1 in
        --publish)
            PUBLISH=true
            shift
            ;;
        --source)
            SOURCE="$2"
            shift 2
            ;;
        --api-key)
            API_KEY="$2"
            shift 2
            ;;
        --configuration)
            CONFIGURATION="$2"
            shift 2
            ;;
        *)
            echo "Unknown option: $1"
            exit 1
            ;;
    esac
done

echo "Building NuGet packages..."

# Build CSharpKOTOR package
echo ""
echo "Building CSharpKOTOR..."
dotnet pack src/CSharpKOTOR/CSharpKOTOR.csproj --configuration "$CONFIGURATION" --no-build

# Build OdyPatch package
echo ""
echo "Building OdyPatch..."
dotnet pack src/OdyPatch/OdyPatch.csproj --configuration "$CONFIGURATION" --no-build

# Find package files
TSL_CORE_PACKAGE=$(find "src/CSharpKOTOR/bin/$CONFIGURATION" -name "*.nupkg" | head -n 1)
ODY_PATCH_PACKAGE=$(find "src/OdyPatch/bin/$CONFIGURATION" -name "*.nupkg" | head -n 1)

if [ -z "$TSL_CORE_PACKAGE" ]; then
    echo "CSharpKOTOR package not found!"
    exit 1
fi

if [ -z "$ODY_PATCH_PACKAGE" ]; then
    echo "OdyPatch package not found!"
    exit 1
fi

echo ""
echo "CSharpKOTOR package created: $TSL_CORE_PACKAGE"
echo "OdyPatch package created: $ODY_PATCH_PACKAGE"

# Publish if requested
if [ "$PUBLISH" = true ]; then
    if [ -z "$API_KEY" ]; then
        echo "Error: --api-key is required when using --publish"
        exit 1
    fi

    echo ""
    echo "Publishing packages to $SOURCE..."

    # Publish CSharpKOTOR
    echo "Publishing CSharpKOTOR..."
    dotnet nuget push "$TSL_CORE_PACKAGE" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate

    # Publish OdyPatch
    echo "Publishing OdyPatch..."
    dotnet nuget push "$ODY_PATCH_PACKAGE" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate

    # Publish symbol packages if they exist
    TSL_CORE_SYMBOLS=$(find "src/CSharpKOTOR/bin/$CONFIGURATION" -name "*.snupkg" | head -n 1)
    ODY_PATCH_SYMBOLS=$(find "src/OdyPatch/bin/$CONFIGURATION" -name "*.snupkg" | head -n 1)

    if [ -n "$TSL_CORE_SYMBOLS" ]; then
        echo "Publishing CSharpKOTOR symbols..."
        dotnet nuget push "$TSL_CORE_SYMBOLS" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate
    fi

    if [ -n "$ODY_PATCH_SYMBOLS" ]; then
        echo "Publishing OdyPatch symbols..."
        dotnet nuget push "$ODY_PATCH_SYMBOLS" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate
    fi

    echo ""
    echo "Packages published successfully!"
else
    echo ""
    echo "Packages built successfully!"
    echo "To publish, run: ./build-nuget.sh --publish --api-key YOUR_API_KEY"
fi

