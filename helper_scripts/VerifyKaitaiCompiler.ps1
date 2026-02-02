# Quick verification script for Kaitai Struct compiler setup
# This script verifies that the compiler is available and can compile BWM.ksy

param(
    [string]$KsyFile = "src\Andastra\Parsing\Resource\Formats\BWM\BWM.ksy",
    [string[]]$Languages = @("python", "java", "javascript", "csharp")
)

$ErrorActionPreference = "Continue"

Write-Host "Verifying Kaitai Struct Compiler Setup..." -ForegroundColor Green
Write-Host ""

# Check Java
Write-Host "1. Checking Java..." -ForegroundColor Yellow
try {
    $javaOutput = java -version 2>&1 | Out-String
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✓ Java is available" -ForegroundColor Green
        $javaOutput -split "`n" | Select-Object -First 1 | ForEach-Object { Write-Host "   $_" -ForegroundColor Gray }
    } else {
        Write-Host "   ✗ Java is not available" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "   ✗ Java is not available: $_" -ForegroundColor Red
    exit 1
}

# Find compiler JAR
Write-Host ""
Write-Host "2. Finding Kaitai Struct Compiler..." -ForegroundColor Yellow
$jarPath = $null

# Check environment variable
$envJar = $env:KAITAI_COMPILER_JAR
if ($envJar -and (Test-Path $envJar)) {
    $jarPath = $envJar
    Write-Host "   ✓ Found via KAITAI_COMPILER_JAR: $jarPath" -ForegroundColor Green
} else {
    # Check common locations
    $searchPaths = @(
        "$env:USERPROFILE\.kaitai\kaitai-struct-compiler.jar",
        "kaitai-struct-compiler.jar",
        ".\kaitai-struct-compiler.jar"
    )

    foreach ($path in $searchPaths) {
        if (Test-Path $path) {
            $jarPath = (Resolve-Path $path).Path
            Write-Host "   ✓ Found at: $jarPath" -ForegroundColor Green
            break
        }
    }
}

if (-not $jarPath) {
    Write-Host "   ✗ Kaitai Struct Compiler not found" -ForegroundColor Red
    Write-Host "   Run: pwsh -ExecutionPolicy Bypass -File scripts\SetupKaitaiCompiler.ps1" -ForegroundColor Yellow
    exit 1
}

# Check .ksy file
Write-Host ""
Write-Host "3. Checking BWM.ksy file..." -ForegroundColor Yellow
$ksyPath = Resolve-Path $KsyFile -ErrorAction SilentlyContinue
if (-not $ksyPath) {
    Write-Host "   ✗ BWM.ksy not found at: $KsyFile" -ForegroundColor Red
    exit 1
}
Write-Host "   ✓ Found at: $ksyPath" -ForegroundColor Green

# Test compilation
Write-Host ""
Write-Host "4. Testing compilation to languages..." -ForegroundColor Yellow
$outputDir = "test_files\kaitai_compiled_verify"
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}
New-Item -ItemType Directory -Path $outputDir -Force | Out-Null

$successCount = 0
$failCount = 0

foreach ($lang in $Languages) {
    Write-Host "   Compiling to $lang..." -ForegroundColor Cyan -NoNewline
    $langOutputDir = Join-Path $outputDir $lang
    New-Item -ItemType Directory -Path $langOutputDir -Force | Out-Null

    $process = Start-Process -FilePath "java" -ArgumentList @(
        "-jar", "`"$jarPath`"",
        "-t", $lang,
        "-d", "`"$langOutputDir`"",
        "`"$ksyPath`""
    ) -Wait -NoNewWindow -PassThru -RedirectStandardOutput "$langOutputDir\compile_output.txt" -RedirectStandardError "$langOutputDir\compile_error.txt"

    if ($process.ExitCode -eq 0) {
        $files = Get-ChildItem -Path $langOutputDir -File -Recurse | Where-Object { $_.Name -ne "compile_output.txt" -and $_.Name -ne "compile_error.txt" }
        if ($files.Count -gt 0) {
            Write-Host " ✓ ($($files.Count) files generated)" -ForegroundColor Green
            $successCount++
        } else {
            Write-Host " ⚠ (compiled but no files generated)" -ForegroundColor Yellow
            $failCount++
        }
    } else {
        $errorText = Get-Content "$langOutputDir\compile_error.txt" -ErrorAction SilentlyContinue | Select-Object -First 3
        Write-Host " ✗ (exit code: $($process.ExitCode))" -ForegroundColor Red
        if ($errorText) {
            Write-Host "     $($errorText[0])" -ForegroundColor Gray
        }
        $failCount++
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Green
Write-Host "  Successful: $successCount" -ForegroundColor Green
Write-Host "  Failed: $failCount" -ForegroundColor $(if ($failCount -eq 0) { "Green" } else { "Yellow" })
Write-Host ""
Write-Host "Output directory: $outputDir" -ForegroundColor Cyan

if ($successCount -gt 0) {
    Write-Host ""
    Write-Host "✓ Kaitai Struct Compiler is functional!" -ForegroundColor Green
    exit 0
} else {
    Write-Host ""
    Write-Host "✗ No languages compiled successfully" -ForegroundColor Red
    exit 1
}

