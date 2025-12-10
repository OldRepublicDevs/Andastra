#!/bin/bash
# Matching PyKotor implementation at vendor/PyKotor/scripts/generate_scriptdefs.sh
# Original: Wrapper script for generate_scriptdefs.py
# This is a 1:1 equivalent wrapper for the C# GenerateScriptDefs tool
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_FILE="$SCRIPT_DIR/GenerateScriptDefs.csproj"
EXE_PATH="$SCRIPT_DIR/bin/Debug/net8.0/GenerateScriptDefs.exe"

if [[ ! -f "$PROJECT_FILE" ]]; then
    echo "Error: GenerateScriptDefs.csproj not found at $PROJECT_FILE" >&2
    exit 1
fi

# Build the project if needed or if exe doesn't exist
if [[ ! -f "$EXE_PATH" ]]; then
    echo "Building GenerateScriptDefs..."
    dotnet build "$PROJECT_FILE" --configuration Debug
    if [[ $? -ne 0 ]]; then
        echo "Error: Failed to build GenerateScriptDefs" >&2
        exit 1
    fi
fi

exec "$EXE_PATH" "$@"

