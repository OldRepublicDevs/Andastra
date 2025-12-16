# Test script with proper timeout using Start-Process and taskkill
param(
    [int]$TimeoutSeconds = 120
)

$ErrorActionPreference = 'Continue'

Write-Host "=== TEST WITH TIMEOUT ($TimeoutSeconds seconds) ===" -ForegroundColor Cyan

$testDir = "test_kill_$(Get-Random)"
if (Test-Path $testDir) { Remove-Item $testDir -Recurse -Force }
New-Item -ItemType Directory -Path $testDir | Out-Null
"namespace Test.Old { }" | Out-File -FilePath "$testDir\test.cs" -Encoding utf8

Write-Host "Created test directory: $testDir"
Write-Host "BEFORE:"
Get-Content "$testDir\test.cs"

Write-Host "`nStarting script with timeout..."
$scriptPath = Join-Path $PWD "scripts\EngineNamespaceRenamer.ps1"
$processArgs = @(
    "-ExecutionPolicy", "Bypass",
    "-File", $scriptPath,
    "-RootPath", (Join-Path $PWD $testDir),
    "-OldFolderName", "Old",
    "-NewFolderName", "New",
    "-OldNamespace", "Test.Old",
    "-NewNamespace", "Test.New",
    "-NoValidation",
    "-NoFolderRename",
    "-Timeout", "30"
)

$startTime = Get-Date
$process = Start-Process -FilePath "powershell.exe" -ArgumentList $processArgs -PassThru -NoNewWindow

# Wait with timeout
$timedOut = $false
$waited = 0
while (-not $process.HasExited -and $waited -lt $TimeoutSeconds) {
    Start-Sleep -Seconds 1
    $waited++
    if ($waited % 10 -eq 0) {
        Write-Host "  Still running... ($waited/$TimeoutSeconds seconds)" -ForegroundColor Yellow
    }
}

if (-not $process.HasExited) {
    Write-Host "`nTIMEOUT: Killing process PID $($process.Id)..." -ForegroundColor Red
    taskkill /F /PID $process.Id 2>&1 | Out-Null
    Start-Sleep -Seconds 1
    if (-not $process.HasExited) {
        Write-Host "Process still running, force killing..." -ForegroundColor Red
        taskkill /F /IM powershell.exe /FI "PID eq $($process.Id)" 2>&1 | Out-Null
    }
    $timedOut = $true
    Write-Host "Process killed"
} else {
    $elapsed = ((Get-Date) - $startTime).TotalSeconds
    Write-Host "`nProcess completed in $elapsed seconds" -ForegroundColor Green
}

# Verify results
if (-not $timedOut) {
    Write-Host "`nAFTER:" -ForegroundColor Cyan
    if (Test-Path "$testDir\test.cs") {
        Get-Content "$testDir\test.cs"
        $content = Get-Content "$testDir\test.cs" -Raw
        $hasNew = $content -match "Test\.New"
        $hasOld = $content -match "Test\.Old"
        if ($hasNew -and -not $hasOld) {
            Write-Host "`nSUCCESS!" -ForegroundColor Green
        } else {
            Write-Host "`nFAILED: Content not updated" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "File missing!" -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "`nTest timed out" -ForegroundColor Red
    exit 1
}

Remove-Item $testDir -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "`nTest passed!" -ForegroundColor Green
