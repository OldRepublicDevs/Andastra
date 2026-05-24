#!/bin/bash
# Build and optionally publish the OdyPatch NuGet package.
# Usage: ./helper_scripts/build-nuget.sh [--publish] [--source <feed-url>] [--api-key <key>]
#
# From repo root:
#   ./helper_scripts/build-nuget.sh
#   ./helper_scripts/build-nuget.sh --publish --api-key YOUR_KEY

set -e

PUBLISH=false
SOURCE="https://api.nuget.org/v3/index.json"
API_KEY=""
CONFIGURATION="Release"
ODY_PATCH_PROJECT="src/Tools/OdyPatch/OdyPatch.csproj"
PACK_OUTPUT_DIR="src/Tools/OdyPatch/bin/${CONFIGURATION}"

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
            PACK_OUTPUT_DIR="src/Tools/OdyPatch/bin/${CONFIGURATION}"
            shift 2
            ;;
        *)
            echo "Unknown option: $1"
            exit 1
            ;;
    esac
done

echo "Building OdyPatch NuGet package..."

dotnet build "$ODY_PATCH_PROJECT" --configuration "$CONFIGURATION" -f net9.0
dotnet pack "$ODY_PATCH_PROJECT" --configuration "$CONFIGURATION" --no-build -p:TargetFrameworks=net9.0

ODY_PATCH_PACKAGE=$(find "$PACK_OUTPUT_DIR" -name "OdyPatch.*.nupkg" ! -name "*.symbols.nupkg" | head -n 1)

if [ -z "$ODY_PATCH_PACKAGE" ]; then
    echo "OdyPatch package not found under $PACK_OUTPUT_DIR"
    exit 1
fi

echo ""
echo "OdyPatch package created: $ODY_PATCH_PACKAGE"

if [ "$PUBLISH" = true ]; then
    if [ -z "$API_KEY" ]; then
        if [ -n "$NUGET_API_KEY" ]; then
            API_KEY="$NUGET_API_KEY"
        else
            echo "Error: --api-key or NUGET_API_KEY is required when using --publish"
            exit 1
        fi
    fi

    echo ""
    echo "Publishing OdyPatch to $SOURCE..."

    dotnet nuget push "$ODY_PATCH_PACKAGE" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate

    ODY_PATCH_SYMBOLS=$(find "$PACK_OUTPUT_DIR" -name "OdyPatch.*.snupkg" | head -n 1)

    if [ -n "$ODY_PATCH_SYMBOLS" ]; then
        echo "Publishing OdyPatch symbols..."
        dotnet nuget push "$ODY_PATCH_SYMBOLS" --api-key "$API_KEY" --source "$SOURCE" --skip-duplicate
    fi

    echo ""
    echo "Package published successfully!"
else
    echo ""
    echo "To publish, run: ./helper_scripts/build-nuget.sh --publish --api-key YOUR_API_KEY"
fi
