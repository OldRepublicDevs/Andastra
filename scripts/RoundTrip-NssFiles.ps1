<#
.SYNOPSIS
    Performs round-trip testing: NSS -> NCS -> NSS -> NCS -> Compare.

.DESCRIPTION
    This script performs comprehensive round-trip testing of NSS compilation and NCS decompilation.
    It compiles NSS to NCS, decompiles back to NSS, recompiles, and compares the bytecode.
    Supports single file, multiple files, or recursive directory processing.

.PARAMETER InputPath
    Path to NSS file(s) or directory containing NSS files to test.
    Can be a single file, multiple files (comma-separated), or a directory.

.PARAMETER OutputDirectory
    Directory where intermediate and result files will be written.
    If not specified, uses a temporary directory.

.PARAMETER Game
    Target game version: "k1" (KOTOR) or "k2" (TSL). Defaults to "k2".

.PARAMETER AssemblyPath
    Path to the CSharpKOTOR.dll assembly. Defaults to the standard build output location.

.PARAMETER LibraryLookup
    Additional directories to search for included files. Can be specified multiple times.

.PARAMETER Recursive
    If InputPath is a directory, process all NSS files recursively in subdirectories.

.PARAMETER CompareMode
    Comparison mode: "bytecode" (strict byte-by-byte), "instructions" (compare instruction structure), or "both". Defaults to "both".

.PARAMETER KeepIntermediate
    Keep intermediate files (first NCS, decompiled NSS, second NCS) for debugging.

.PARAMETER StopOnFirstFailure
    Stop processing on the first failure instead of continuing with remaining files.

.PARAMETER WhatIf
    Show what would be tested without actually performing the round-trip.

.EXAMPLE
    .\scripts\RoundTrip-NssFiles.ps1 -InputPath "script.nss"

.EXAMPLE
    .\scripts\RoundTrip-NssFiles.ps1 -InputPath "C:\Scripts" -Recursive -CompareMode "bytecode"

.EXAMPLE
    .\scripts\RoundTrip-NssFiles.ps1 -InputPath "script.nss" -OutputDirectory "C:\RoundTrip" -KeepIntermediate
#>
[CmdletBinding(SupportsShouldProcess=$true)]
param(
    [Parameter(Mandatory=$true, Position=0)]
    [string[]]$InputPath,

    [Parameter(Mandatory=$false)]
    [string]$OutputDirectory,

    [ValidateSet("k1", "k2")]
    [string]$Game = "k2",

    [string]$AssemblyPath = "src/CSharpKOTOR/bin/Debug/net9/CSharpKOTOR.dll",

    [string[]]$LibraryLookup = @(),

    [switch]$Recursive,

    [ValidateSet("bytecode", "instructions", "both")]
    [string]$CompareMode = "both",

    [switch]$KeepIntermediate,

    [switch]$StopOnFirstFailure,

    [switch]$WhatIf
)

# Resolve workspace root
$workspaceRoot = $PSScriptRoot | Split-Path -Parent
$assemblyFullPath = if ([System.IO.Path]::IsPathRooted($AssemblyPath)) { 
    $AssemblyPath 
} else { 
    Join-Path $workspaceRoot $AssemblyPath 
}

# Validate assembly exists
if (-not (Test-Path $assemblyFullPath)) {
    Write-Error "Assembly not found: $assemblyFullPath"
    Write-Host "Hint: Build the CSharpKOTOR project first: dotnet build src/CSharpKOTOR/CSharpKOTOR.csproj"
    exit 1
}

# Load assembly
try {
    Add-Type -Path $assemblyFullPath | Out-Null
}
catch {
    Write-Error "Failed to load assembly: $_"
    exit 1
}

# Helper function to resolve paths
function Resolve-InputPath {
    param([string]$Path)
    
    if ([System.IO.Path]::IsPathRooted($Path)) {
        return $Path
    }
    return Join-Path $workspaceRoot $Path
}

# Collect all NSS files to process
$filesToProcess = @()

foreach ($path in $InputPath) {
    $resolvedPath = Resolve-InputPath $path
    
    if (-not (Test-Path $resolvedPath)) {
        Write-Warning "Path not found: $resolvedPath"
        continue
    }
    
    $item = Get-Item $resolvedPath
    
    if ($item -is [System.IO.DirectoryInfo]) {
        $nssFiles = Get-ChildItem -Path $resolvedPath -Filter "*.nss" -File -Recurse:$Recursive -ErrorAction SilentlyContinue
        $filesToProcess += $nssFiles
    }
    elseif ($item -is [System.IO.FileInfo]) {
        if ($item.Extension -eq ".nss") {
            $filesToProcess += $item
        } else {
            Write-Warning "File is not an NSS file: $resolvedPath"
        }
    }
}

if ($filesToProcess.Count -eq 0) {
    Write-Error "No NSS files found to process."
    exit 1
}

# Setup output directory
if ([string]::IsNullOrEmpty($OutputDirectory)) {
    $OutputDirectory = Join-Path $workspaceRoot "test-work" "roundtrip-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
}

if (-not [System.IO.Path]::IsPathRooted($OutputDirectory)) {
    $OutputDirectory = Join-Path $workspaceRoot $OutputDirectory
}

if (-not (Test-Path $OutputDirectory)) {
    if ($WhatIf) {
        Write-Host "[WHATIF] Would create directory: $OutputDirectory" -ForegroundColor Yellow
    } else {
        New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
    }
}

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "ROUND-TRIP TESTING" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Found $($filesToProcess.Count) NSS file(s) to test" -ForegroundColor Cyan
Write-Host "Game: $Game" -ForegroundColor Cyan
Write-Host "Compare mode: $CompareMode" -ForegroundColor Cyan
Write-Host "Output directory: $OutputDirectory" -ForegroundColor Cyan
Write-Host ""

# Build library lookup list
$libraryLookupList = New-Object System.Collections.Generic.List[string]

# Add parent directories of input files
foreach ($file in $filesToProcess) {
    $parentDir = [System.IO.Path]::GetDirectoryName($file.FullName)
    if (-not [string]::IsNullOrEmpty($parentDir) -and -not $libraryLookupList.Contains($parentDir)) {
        $libraryLookupList.Add($parentDir)
    }
}

# Add user-specified lookup paths
foreach ($lookup in $LibraryLookup) {
    $resolvedLookup = Resolve-InputPath $lookup
    if (Test-Path $resolvedLookup) {
        if (-not $libraryLookupList.Contains($resolvedLookup)) {
            $libraryLookupList.Add($resolvedLookup)
        }
    } else {
        Write-Warning "Library lookup path not found: $resolvedLookup"
    }
}

# Determine game type
$gameType = if ($Game -eq "k1") { 
    [CSharpKOTOR.Common.Game]::K1 
} else { 
    [CSharpKOTOR.Common.Game]::K2 
}

# Statistics
$successCount = 0
$failureCount = 0
$skipCount = 0

# Process each file
foreach ($file in $filesToProcess) {
    $inputFile = $file.FullName
    $baseName = [System.IO.Path]::GetFileNameWithoutExtension($inputFile)
    $relativeDir = $file.DirectoryName.Replace($workspaceRoot, "").TrimStart('\', '/')
    
    # Create subdirectory structure in output
    $fileOutputDir = if ([string]::IsNullOrEmpty($relativeDir)) {
        $OutputDirectory
    } else {
        Join-Path $OutputDirectory $relativeDir
    }
    
    if (-not (Test-Path $fileOutputDir)) {
        if ($WhatIf) {
            Write-Host "[WHATIF] Would create directory: $fileOutputDir" -ForegroundColor Yellow
        } else {
            New-Item -ItemType Directory -Path $fileOutputDir -Force | Out-Null
        }
    }
    
    $firstNcs = Join-Path $fileOutputDir "$baseName.first.ncs"
    $decompiledNss = Join-Path $fileOutputDir "$baseName.decompiled.nss"
    $secondNcs = Join-Path $fileOutputDir "$baseName.second.ncs"
    
    Write-Host "[$($successCount + $failureCount + 1)/$($filesToProcess.Count)] $([System.IO.Path]::GetFileName($inputFile))" -ForegroundColor Yellow
    
    if ($WhatIf) {
        Write-Host "[WHATIF] Would perform round-trip: $inputFile" -ForegroundColor Yellow
        Write-Host ""
        continue
    }
    
    $failed = $false
    $errorMessage = $null
    
    try {
        # Step 1: Compile original NSS -> NCS
        Write-Host "  [1/4] Compiling original NSS..." -NoNewline
        $source = [System.IO.File]::ReadAllText($inputFile, [System.Text.Encoding]::UTF8)
        $ncs1 = [CSharpKOTOR.Formats.NCS.NCSAuto]::CompileNss($source, $gameType, $null, $null, $libraryLookupList)
        [CSharpKOTOR.Formats.NCS.NCSAuto]::WriteNcs($ncs1, $firstNcs)
        
        if (-not (Test-Path $firstNcs)) {
            throw "First compilation did not produce output file"
        }
        
        $size1 = (Get-Item $firstNcs).Length
        $inst1 = $ncs1.Instructions.Count
        Write-Host " ✓ ($size1 bytes, $inst1 instructions)" -ForegroundColor Green
        
        # Step 2: Decompile NCS -> NSS
        Write-Host "  [2/4] Decompiling NCS..." -NoNewline
        $ncs1Loaded = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($firstNcs)
        $decompiled = [CSharpKOTOR.Formats.NCS.NCSAuto]::DecompileNcs($ncs1Loaded, $gameType)
        [System.IO.File]::WriteAllText($decompiledNss, $decompiled, [System.Text.Encoding]::UTF8)
        
        if (-not (Test-Path $decompiledNss)) {
            throw "Decompilation did not produce output file"
        }
        
        $decompiledLength = $decompiled.Length
        Write-Host " ✓ ($decompiledLength chars)" -ForegroundColor Green
        
        # Step 3: Recompile decompiled NSS -> NCS
        Write-Host "  [3/4] Recompiling decompiled NSS..." -NoNewline
        $ncs2 = [CSharpKOTOR.Formats.NCS.NCSAuto]::CompileNss($decompiled, $gameType, $null, $null, $libraryLookupList)
        [CSharpKOTOR.Formats.NCS.NCSAuto]::WriteNcs($ncs2, $secondNcs)
        
        if (-not (Test-Path $secondNcs)) {
            throw "Second compilation did not produce output file"
        }
        
        $size2 = (Get-Item $secondNcs).Length
        $inst2 = $ncs2.Instructions.Count
        Write-Host " ✓ ($size2 bytes, $inst2 instructions)" -ForegroundColor Green
        
        # Step 4: Compare bytecode
        Write-Host "  [4/4] Comparing bytecode..." -NoNewline
        
        $bytecodeMatch = $true
        $instructionsMatch = $true
        
        if ($CompareMode -eq "bytecode" -or $CompareMode -eq "both") {
            # Byte-by-byte comparison
            $bytes1 = [System.IO.File]::ReadAllBytes($firstNcs)
            $bytes2 = [System.IO.File]::ReadAllBytes($secondNcs)
            
            if ($bytes1.Length -ne $bytes2.Length) {
                $bytecodeMatch = $false
                $errorMessage = "Bytecode size mismatch: $($bytes1.Length) vs $($bytes2.Length) bytes"
            } else {
                for ($i = 0; $i -lt $bytes1.Length; $i++) {
                    if ($bytes1[$i] -ne $bytes2[$i]) {
                        $bytecodeMatch = $false
                        $errorMessage = "Bytecode mismatch at offset $i (0x$($i.ToString('X'))): 0x$($bytes1[$i].ToString('X2')) vs 0x$($bytes2[$i].ToString('X2'))"
                        break
                    }
                }
            }
        }
        
        if ($CompareMode -eq "instructions" -or $CompareMode -eq "both") {
            # Instruction comparison
            if ($ncs1.Instructions.Count -ne $ncs2.Instructions.Count) {
                $instructionsMatch = $false
                if ([string]::IsNullOrEmpty($errorMessage)) {
                    $errorMessage = "Instruction count mismatch: $($ncs1.Instructions.Count) vs $($ncs2.Instructions.Count)"
                }
            } else {
                for ($i = 0; $i -lt $ncs1.Instructions.Count; $i++) {
                    $inst1 = $ncs1.Instructions[$i]
                    $inst2 = $ncs2.Instructions[$i]
                    
                    if ($inst1.InsType -ne $inst2.InsType) {
                        $instructionsMatch = $false
                        if ([string]::IsNullOrEmpty($errorMessage)) {
                            $errorMessage = "Instruction type mismatch at index $i : $($inst1.InsType) vs $($inst2.InsType)"
                        }
                        break
                    }
                    
                    # Compare arguments
                    if ($inst1.Args.Count -ne $inst2.Args.Count) {
                        $instructionsMatch = $false
                        if ([string]::IsNullOrEmpty($errorMessage)) {
                            $errorMessage = "Instruction argument count mismatch at index $i"
                        }
                        break
                    }
                }
            }
        }
        
        if ($bytecodeMatch -and $instructionsMatch) {
            Write-Host " ✓ MATCH" -ForegroundColor Green
            $successCount++
            
            # Clean up intermediate files if not keeping them
            if (-not $KeepIntermediate) {
                Remove-Item $firstNcs -ErrorAction SilentlyContinue
                Remove-Item $decompiledNss -ErrorAction SilentlyContinue
                Remove-Item $secondNcs -ErrorAction SilentlyContinue
            }
        } else {
            Write-Host " ✗ MISMATCH" -ForegroundColor Red
            Write-Host "    $errorMessage" -ForegroundColor Red
            $failureCount++
            $failed = $true
            
            if ($StopOnFirstFailure) {
                throw $errorMessage
            }
        }
    }
    catch {
        Write-Host " ✗ FAILED" -ForegroundColor Red
        Write-Host "    $_" -ForegroundColor Red
        $failureCount++
        $failed = $true
        $errorMessage = $_.ToString()
        
        if ($StopOnFirstFailure) {
            throw
        }
    }
    
    Write-Host ""
}

Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "SUMMARY" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "Total:    $($filesToProcess.Count)" -ForegroundColor Cyan
Write-Host "Success:  $successCount" -ForegroundColor Green
Write-Host "Failed:   $failureCount" -ForegroundColor $(if ($failureCount -eq 0) { "Green" } else { "Red" })
Write-Host "Skipped:  $skipCount" -ForegroundColor Yellow
Write-Host ""

if ($KeepIntermediate -and $failureCount -gt 0) {
    Write-Host "Intermediate files kept in: $OutputDirectory" -ForegroundColor Yellow
}

if ($failureCount -gt 0) {
    exit 1
}

