@echo off
REM Matching PyKotor implementation at vendor/PyKotor/scripts/generate_scriptdefs.bat
REM Original: Wrapper script for generate_scriptdefs.py
REM This is a 1:1 equivalent wrapper for the C# GenerateScriptDefs tool
setlocal enabledelayedexpansion
set "SCRIPT_DIR=%~dp0"
set "PROJECT_FILE=%SCRIPT_DIR%GenerateScriptDefs.csproj"
set "EXE_PATH=%SCRIPT_DIR%bin\Debug\net9.0.0\GenerateScriptDefs.exe"

if not exist "!PROJECT_FILE!" (
    echo Error: GenerateScriptDefs.csproj not found at !PROJECT_FILE! >&2
    exit /b 1
)

REM Build the project if needed or if exe doesn't exist
if not exist "!EXE_PATH!" (
    echo Building GenerateScriptDefs...
    dotnet build "!PROJECT_FILE!" --configuration Debug
    if errorlevel 1 (
        echo Error: Failed to build GenerateScriptDefs >&2
        exit /b 1
    )
)

"!EXE_PATH!" %*
exit /b %ERRORLEVEL%

