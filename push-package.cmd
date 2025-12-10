@echo off
cd /d g:\GitHub\HoloPatcher.NET
echo Checking package...
if not exist "src\CSharpKOTOR\bin\Release\CSharpKOTOR.2.0.0-alpha1.nupkg" (
    echo ERROR: Package not found!
    exit /b 1
)
echo Package found!
echo.
echo Pushing to NuGet.org...
dotnet nuget push "src\CSharpKOTOR\bin\Release\CSharpKOTOR.2.0.0-alpha1.nupkg" --api-key "YOUR_NUGET_API_KEY_HERE" --source "https://api.nuget.org/v3/index.json" --skip-duplicate
echo.
echo Exit code: %ERRORLEVEL%
if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: Package pushed!
) else (
    echo FAILED: Push failed with exit code %ERRORLEVEL%
)
pause

