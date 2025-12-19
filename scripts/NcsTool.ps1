#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Comprehensive tool for NCS/NSS file operations: compile, decompile, compare, round-trip, and generate definitions.

.DESCRIPTION
    Unified PowerShell tool for all NCS (bytecode) and NSS (source) file operations.
    Supports compilation, decompilation, comparison, round-trip testing, and script definition generation.

    Operations:
    - compile: Compile NSS source files to NCS bytecode
    - decompile: Decompile NCS bytecode files to NSS source
    - compare: Compare two NCS files (bytecode and/or instructions)
    - roundtrip: Perform round-trip testing (NSS -> NCS -> NSS -> NCS -> Compare)
    - generate-defs: Generate ScriptDefs.cs from nwscript.nss files

.PARAMETER Operation
    The operation to perform: compile, decompile, compare, roundtrip, or generate-defs.

.PARAMETER InputPath
    Input file(s) or directory. For compile/decompile/roundtrip operations.

.PARAMETER OutputPath
    Output file or directory. For compile/decompile operations.

.PARAMETER OriginalFile
    First NCS file for comparison (compare operation).

.PARAMETER RoundTripFile
    Second NCS file for comparison (compare operation).

.PARAMETER Game
    Target game version: "k1" (KOTOR) or "k2" (TSL). Defaults to "k2".

.PARAMETER AssemblyPath
    Path to the CSharpKOTOR.dll assembly. Defaults to the standard build output location.

.PARAMETER LibraryLookup
    Additional directories to search for included files (compile/roundtrip operations).

.PARAMETER Recursive
    Process files recursively in subdirectories (compile/decompile/roundtrip operations).

.PARAMETER Overwrite
    Overwrite existing output files (compile/decompile operations).

.PARAMETER CompareMode
    Comparison mode: "bytecode", "instructions", or "both" (compare/roundtrip operations). Defaults to "both".

.PARAMETER ShowOnly
    Show only specified file in comparison: "original", "roundtrip", or "both" (compare operation). Defaults to "both".

.PARAMETER Detailed
    Show detailed byte-by-byte comparison for mismatches (compare operation).

.PARAMETER OutputDirectory
    Directory for round-trip intermediate files (roundtrip operation).

.PARAMETER KeepIntermediate
    Keep intermediate files after round-trip testing (roundtrip operation).

.PARAMETER StopOnFirstFailure
    Stop on first failure instead of continuing (roundtrip operation).

.PARAMETER WhatIf
    Show what would be done without actually performing the operation.

.PARAMETER Verbose
    Show detailed progress information.

.EXAMPLE
    .\scripts\NcsTool.ps1 compile -InputPath "script.nss" -Game "k2"

.EXAMPLE
    .\scripts\NcsTool.ps1 decompile -InputPath "script.ncs" -OutputPath "script.nss" -Game "k1"

.EXAMPLE
    .\scripts\NcsTool.ps1 compare -OriginalFile "original.ncs" -RoundTripFile "roundtrip.ncs" -CompareMode "bytecode" -Detailed

.EXAMPLE
    .\scripts\NcsTool.ps1 roundtrip -InputPath "C:\Scripts" -Recursive -CompareMode "bytecode"

.EXAMPLE
    .\scripts\NcsTool.ps1 generate-defs

.EXAMPLE
    .\scripts\NcsTool.ps1 compile -InputPath "C:\Scripts" -Recursive -LibraryLookup "C:\Includes" -Overwrite
#>
[CmdletBinding(SupportsShouldProcess=$true)]
param(
    [Parameter(Mandatory=$true, Position=0)]
    [ValidateSet("compile", "decompile", "compare", "roundtrip", "generate-defs")]
    [string]$Operation,

    # Common parameters
    [Parameter(ParameterSetName="Compile", Mandatory=$true)]
    [Parameter(ParameterSetName="Decompile", Mandatory=$true)]
    [Parameter(ParameterSetName="RoundTrip", Mandatory=$true)]
    [string[]]$InputPath,

    [Parameter(ParameterSetName="Compile")]
    [Parameter(ParameterSetName="Decompile")]
    [string]$OutputPath,

    [Parameter(ParameterSetName="Compare", Mandatory=$true)]
    [string]$OriginalFile,

    [Parameter(ParameterSetName="Compare", Mandatory=$true)]
    [string]$RoundTripFile,

    [ValidateSet("k1", "k2")]
    [string]$Game = "k2",

    [string]$AssemblyPath = "src/CSharpKOTOR/bin/Debug/net9.0/CSharpKOTOR.dll",

    [Parameter(ParameterSetName="Compile")]
    [Parameter(ParameterSetName="RoundTrip")]
    [string[]]$LibraryLookup = @(),

    [Parameter(ParameterSetName="Compile")]
    [Parameter(ParameterSetName="Decompile")]
    [Parameter(ParameterSetName="RoundTrip")]
    [switch]$Recursive,

    [Parameter(ParameterSetName="Compile")]
    [Parameter(ParameterSetName="Decompile")]
    [switch]$Overwrite,

    [Parameter(ParameterSetName="Compare")]
    [Parameter(ParameterSetName="RoundTrip")]
    [ValidateSet("bytecode", "instructions", "both")]
    [string]$CompareMode = "both",

    [Parameter(ParameterSetName="Compare")]
    [ValidateSet("original", "roundtrip", "both")]
    [string]$ShowOnly = "both",

    [Parameter(ParameterSetName="Compare")]
    [switch]$Detailed,

    [Parameter(ParameterSetName="RoundTrip")]
    [string]$OutputDirectory,

    [Parameter(ParameterSetName="RoundTrip")]
    [switch]$KeepIntermediate,

    [Parameter(ParameterSetName="RoundTrip")]
    [switch]$StopOnFirstFailure
)

$ErrorActionPreference = "Stop"

# Resolve workspace root
$script:WorkspaceRoot = $PSScriptRoot | Split-Path -Parent
$script:AssemblyPath = if ([System.IO.Path]::IsPathRooted($AssemblyPath)) {
    $AssemblyPath
} else {
    Join-Path $script:WorkspaceRoot $AssemblyPath
}

# Helper function to resolve paths
function Resolve-WorkspacePath {
    param([string]$Path)

    if ([string]::IsNullOrEmpty($Path)) {
        return $null
    }

    if ([System.IO.Path]::IsPathRooted($Path)) {
        return $Path
    }
    return Join-Path $script:WorkspaceRoot $Path
}

# Helper function to load assembly
function Load-Assembly {
    if (-not (Test-Path $script:AssemblyPath)) {
        Write-Error "Assembly not found: $script:AssemblyPath"
        Write-Host "Hint: Build the CSharpKOTOR project first: dotnet build src/CSharpKOTOR/CSharpKOTOR.csproj"
        exit 1
    }

    try {
        Add-Type -Path $script:AssemblyPath | Out-Null
        Write-Verbose "Loaded assembly: $script:AssemblyPath"
    }
    catch {
        Write-Error "Failed to load assembly: $_"
        exit 1
    }
}

# Helper function to get output path for a file
function Get-OutputPath {
    param(
        [string]$InputFile,
        [string]$BaseOutput,
        [string]$Extension
    )

    if ($BaseOutput) {
        $output = Resolve-WorkspacePath $BaseOutput

        # If output is a directory, append filename
        if ((Test-Path $output) -and (Get-Item $output) -is [System.IO.DirectoryInfo]) {
            $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + $Extension
            return Join-Path $output $fileName
        }

        # If output doesn't exist and has no extension, treat as directory
        if (-not (Test-Path $output)) {
            $ext = [System.IO.Path]::GetExtension($output)
            if ([string]::IsNullOrEmpty($ext)) {
                $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + $Extension
                return Join-Path $output $fileName
            }
        }

        return $output
    }

    # Default: same location as input with new extension
    $dir = [System.IO.Path]::GetDirectoryName($InputFile)
    $name = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + $Extension
    return Join-Path $dir $name
}

# Helper function to collect files
function Get-FilesToProcess {
    param(
        [string[]]$Paths,
        [string]$Extension,
        [bool]$Recurse
    )

    $files = @()

    foreach ($path in $Paths) {
        $resolvedPath = Resolve-WorkspacePath $path

        if (-not (Test-Path $resolvedPath)) {
            Write-Warning "Path not found: $resolvedPath"
            continue
        }

        $item = Get-Item $resolvedPath

        if ($item -is [System.IO.DirectoryInfo]) {
            $foundFiles = Get-ChildItem -Path $resolvedPath -Filter "*$Extension" -File -Recurse:$Recurse -ErrorAction SilentlyContinue
            $files += $foundFiles
        }
        elseif ($item -is [System.IO.FileInfo]) {
            if ($item.Extension -eq $Extension) {
                $files += $item
            } else {
                Write-Warning "File is not a $Extension file: $resolvedPath"
            }
        }
    }

    return $files
}

# Operation: Compile
function Invoke-Compile {
    param(
        [string[]]$InputPath,
        [string]$OutputPath,
        [string]$Game,
        [string[]]$LibraryLookup,
        [bool]$Recursive,
        [bool]$Overwrite,
        [bool]$WhatIf
    )

    Load-Assembly

    $filesToProcess = Get-FilesToProcess -Paths $InputPath -Extension ".nss" -Recurse $Recursive

    if ($filesToProcess.Count -eq 0) {
        Write-Error "No NSS files found to process."
        exit 1
    }

    Write-Host "Found $($filesToProcess.Count) NSS file(s) to compile" -ForegroundColor Cyan
    Write-Host "Game: $Game" -ForegroundColor Cyan
    if ($LibraryLookup.Count -gt 0) {
        Write-Host "Library lookup paths: $($LibraryLookup.Count)" -ForegroundColor Cyan
    }
    Write-Host ""

    # Build library lookup list
    $libraryLookupList = New-Object System.Collections.Generic.List[string]

    foreach ($file in $filesToProcess) {
        $parentDir = [System.IO.Path]::GetDirectoryName($file.FullName)
        if (-not [string]::IsNullOrEmpty($parentDir) -and -not $libraryLookupList.Contains($parentDir)) {
            $libraryLookupList.Add($parentDir)
        }
    }

    foreach ($lookup in $LibraryLookup) {
        $resolvedLookup = Resolve-WorkspacePath $lookup
        if (Test-Path $resolvedLookup) {
            if (-not $libraryLookupList.Contains($resolvedLookup)) {
                $libraryLookupList.Add($resolvedLookup)
            }
        } else {
            Write-Warning "Library lookup path not found: $resolvedLookup"
        }
    }

    $gameType = if ($Game -eq "k1") { [CSharpKOTOR.Common.Game]::K1 } else { [CSharpKOTOR.Common.Game]::K2 }

    $successCount = 0
    $skipCount = 0
    $errorCount = 0

    foreach ($file in $filesToProcess) {
        $inputFile = $file.FullName
        $outputFile = Get-OutputPath -InputFile $inputFile -BaseOutput $OutputPath -Extension ".ncs"

        $outputDir = [System.IO.Path]::GetDirectoryName($outputFile)
        if (-not [string]::IsNullOrEmpty($outputDir) -and -not (Test-Path $outputDir)) {
            if ($WhatIf) {
                Write-Host "[WHATIF] Would create directory: $outputDir" -ForegroundColor Yellow
            } else {
                New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
            }
        }

        if ((Test-Path $outputFile) -and -not $Overwrite) {
            Write-Host "Skipping (already exists): $([System.IO.Path]::GetFileName($inputFile))" -ForegroundColor Yellow
            $skipCount++
            continue
        }

        if ($WhatIf) {
            Write-Host "[WHATIF] Would compile: $inputFile -> $outputFile" -ForegroundColor Yellow
            continue
        }

        try {
            Write-Host "Compiling: $([System.IO.Path]::GetFileName($inputFile))" -NoNewline

            $source = [System.IO.File]::ReadAllText($inputFile, [System.Text.Encoding]::UTF8)
            $ncs = [CSharpKOTOR.Formats.NCS.NCSAuto]::CompileNss($source, $gameType, $null, $null, $libraryLookupList)
            [CSharpKOTOR.Formats.NCS.NCSAuto]::WriteNcs($ncs, $outputFile)

            $fileSize = (Get-Item $outputFile).Length
            $instructionCount = $ncs.Instructions.Count
            Write-Host " ✓ ($fileSize bytes, $instructionCount instructions)" -ForegroundColor Green
            $successCount++
        }
        catch {
            Write-Host " ✗ FAILED" -ForegroundColor Red
            Write-Error "Failed to compile $inputFile : $_"
            $errorCount++
        }
    }

    Write-Host ""
    Write-Host "Summary:" -ForegroundColor Cyan
    Write-Host "  Success: $successCount"
    Write-Host "  Skipped: $skipCount"
    Write-Host "  Errors:  $errorCount"

    if ($errorCount -gt 0) {
        exit 1
    }
}

# Operation: Decompile
function Invoke-Decompile {
    param(
        [string[]]$InputPath,
        [string]$OutputPath,
        [string]$Game,
        [bool]$Recursive,
        [bool]$Overwrite,
        [bool]$WhatIf
    )

    Load-Assembly

    $filesToProcess = Get-FilesToProcess -Paths $InputPath -Extension ".ncs" -Recurse $Recursive

    if ($filesToProcess.Count -eq 0) {
        Write-Error "No NCS files found to process."
        exit 1
    }

    Write-Host "Found $($filesToProcess.Count) NCS file(s) to decompile" -ForegroundColor Cyan
    Write-Host "Game: $Game" -ForegroundColor Cyan
    Write-Host ""

    $gameType = if ($Game -eq "k1") { [CSharpKOTOR.Common.Game]::K1 } else { [CSharpKOTOR.Common.Game]::K2 }

    $successCount = 0
    $skipCount = 0
    $errorCount = 0

    foreach ($file in $filesToProcess) {
        $inputFile = $file.FullName
        $outputFile = Get-OutputPath -InputFile $inputFile -BaseOutput $OutputPath -Extension ".nss"

        $outputDir = [System.IO.Path]::GetDirectoryName($outputFile)
        if (-not [string]::IsNullOrEmpty($outputDir) -and -not (Test-Path $outputDir)) {
            if ($WhatIf) {
                Write-Host "[WHATIF] Would create directory: $outputDir" -ForegroundColor Yellow
            } else {
                New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
            }
        }

        if ((Test-Path $outputFile) -and -not $Overwrite) {
            Write-Host "Skipping (already exists): $([System.IO.Path]::GetFileName($inputFile))" -ForegroundColor Yellow
            $skipCount++
            continue
        }

        if ($WhatIf) {
            Write-Host "[WHATIF] Would decompile: $inputFile -> $outputFile" -ForegroundColor Yellow
            continue
        }

        try {
            Write-Host "Decompiling: $([System.IO.Path]::GetFileName($inputFile))" -NoNewline

            $ncs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($inputFile)
            $nssCode = [CSharpKOTOR.Formats.NCS.NCSAuto]::DecompileNcs($ncs, $gameType)
            [System.IO.File]::WriteAllText($outputFile, $nssCode, [System.Text.Encoding]::UTF8)

            Write-Host " ✓ ($($nssCode.Length) chars)" -ForegroundColor Green
            $successCount++
        }
        catch {
            Write-Host " ✗ FAILED" -ForegroundColor Red
            Write-Error "Failed to decompile $inputFile : $_"
            $errorCount++
        }
    }

    Write-Host ""
    Write-Host "Summary:" -ForegroundColor Cyan
    Write-Host "  Success: $successCount"
    Write-Host "  Skipped: $skipCount"
    Write-Host "  Errors:  $errorCount"

    if ($errorCount -gt 0) {
        exit 1
    }
}

# Operation: Compare
function Invoke-Compare {
    param(
        [string]$OriginalFile,
        [string]$RoundTripFile,
        [string]$CompareMode,
        [string]$ShowOnly,
        [bool]$Detailed
    )

    Load-Assembly

    $originalPath = Resolve-WorkspacePath $OriginalFile
    $roundTripPath = Resolve-WorkspacePath $RoundTripFile

    if (-not (Test-Path $originalPath)) {
        Write-Error "Original file not found: $originalPath"
        exit 1
    }

    if (-not (Test-Path $roundTripPath)) {
        Write-Error "Round-trip file not found: $roundTripPath"
        exit 1
    }

    # Bytecode comparison
    if ($CompareMode -eq "bytecode" -or $CompareMode -eq "both") {
        Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host "BYTECODE COMPARISON" -ForegroundColor Cyan
        Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host ""

        $origBytes = [System.IO.File]::ReadAllBytes($originalPath)
        $rtBytes = [System.IO.File]::ReadAllBytes($roundTripPath)

        Write-Host "File sizes:" -ForegroundColor Yellow
        Write-Host "  Original:   $($origBytes.Length) bytes"
        Write-Host "  Round-trip:  $($rtBytes.Length) bytes"
        Write-Host ""

        if ($origBytes.Length -ne $rtBytes.Length) {
            Write-Host "✗ File sizes differ!" -ForegroundColor Red
        }

        $mismatches = @()
        $maxCompare = [Math]::Min($origBytes.Length, $rtBytes.Length)

        for ($i = 0; $i -lt $maxCompare; $i++) {
            if ($origBytes[$i] -ne $rtBytes[$i]) {
                $mismatches += @{
                    Offset = $i
                    Original = $origBytes[$i]
                    RoundTrip = $rtBytes[$i]
                }

                if (-not $Detailed -and $mismatches.Count -ge 10) {
                    break
                }
            }
        }

        if ($mismatches.Count -eq 0 -and $origBytes.Length -eq $rtBytes.Length) {
            Write-Host "✓ Bytecode matches exactly (byte-by-byte)" -ForegroundColor Green
        } else {
            Write-Host "✗ Bytecode mismatch detected!" -ForegroundColor Red
            Write-Host ""
            Write-Host "Mismatches:" -ForegroundColor Yellow

            $showCount = if ($Detailed) { $mismatches.Count } else { [Math]::Min(10, $mismatches.Count) }

            for ($i = 0; $i -lt $showCount; $i++) {
                $m = $mismatches[$i]
                Write-Host "  Offset 0x$($m.Offset.ToString('X4')) ($($m.Offset)): 0x$($m.Original.ToString('X2')) vs 0x$($m.RoundTrip.ToString('X2'))" -ForegroundColor Red
            }

            if ($mismatches.Count -gt $showCount) {
                Write-Host "  ... and $($mismatches.Count - $showCount) more (use -Detailed to see all)" -ForegroundColor Yellow
            }

            if ($Detailed -and $mismatches.Count -gt 0) {
                Write-Host ""
                Write-Host "Hex context around first mismatch:" -ForegroundColor Yellow
                $firstMismatch = $mismatches[0]
                $start = [Math]::Max(0, $firstMismatch.Offset - 16)
                $end = [Math]::Min($origBytes.Length, $firstMismatch.Offset + 16)

                Write-Host "  Original:  " -NoNewline
                for ($i = $start; $i -lt $end; $i++) {
                    $color = if ($i -eq $firstMismatch.Offset) { "Red" } else { "White" }
                    Write-Host ("{0:X2} " -f $origBytes[$i]) -NoNewline -ForegroundColor $color
                }
                Write-Host ""

                Write-Host "  Round-trip: " -NoNewline
                for ($i = $start; $i -lt $end; $i++) {
                    $color = if ($i -eq $firstMismatch.Offset) { "Red" } else { "White" }
                    Write-Host ("{0:X2} " -f $rtBytes[$i]) -NoNewline -ForegroundColor $color
                }
                Write-Host ""
            }
        }

        Write-Host ""
    }

    # Instruction comparison
    if ($CompareMode -eq "instructions" -or $CompareMode -eq "both") {
        Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host "INSTRUCTION COMPARISON" -ForegroundColor Cyan
        Write-Host "═══════════════════════════════════════════════════════════" -ForegroundColor Cyan
        Write-Host ""

        try {
            $origNcs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($originalPath)
            $rtNcs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($roundTripPath)
        }
        catch {
            Write-Error "Failed to load NCS files: $_"
            exit 1
        }

        function Format-Instruction {
            param([int]$Index, [object]$Instruction)

            $argsStr = if ($Instruction.Args -and $Instruction.Args.Count -gt 0) {
                $Instruction.Args | ForEach-Object {
                    if ($_ -is [string]) { "'$_'" } else { $_ }
                } | Join-String -Separator ", "
            } else {
                ""
            }

            $jumpStr = if ($Instruction.Jump) { " jump=→[$($Instruction.Jump.Offset)]" } else { "" }
            return "[$Index] $($Instruction.InsType) args=[$argsStr]$jumpStr offset=$($Instruction.Offset)"
        }

        if ($ShowOnly -eq "original" -or $ShowOnly -eq "both") {
            Write-Host "=== ORIGINAL NCS INSTRUCTIONS ($($origNcs.Instructions.Count) total) ===" -ForegroundColor Cyan
            Write-Host "File: $originalPath"
            Write-Host ""
            for ($i = 0; $i -lt $origNcs.Instructions.Count; $i++) {
                Write-Host (Format-Instruction $i $origNcs.Instructions[$i])
            }
            Write-Host ""
        }

        if ($ShowOnly -eq "roundtrip" -or $ShowOnly -eq "both") {
            Write-Host "=== ROUND-TRIP NCS INSTRUCTIONS ($($rtNcs.Instructions.Count) total) ===" -ForegroundColor Cyan
            Write-Host "File: $roundTripPath"
            Write-Host ""
            for ($i = 0; $i -lt $rtNcs.Instructions.Count; $i++) {
                Write-Host (Format-Instruction $i $rtNcs.Instructions[$i])
            }
            Write-Host ""
        }

        if ($ShowOnly -eq "both") {
            Write-Host "=== SIDE-BY-SIDE COMPARISON ===" -ForegroundColor Yellow
            Write-Host ""

            $maxCount = [Math]::Max($origNcs.Instructions.Count, $rtNcs.Instructions.Count)
            $matchCount = 0
            $mismatchCount = 0

            for ($i = 0; $i -lt $maxCount; $i++) {
                $origInst = if ($i -lt $origNcs.Instructions.Count) { $origNcs.Instructions[$i] } else { $null }
                $rtInst = if ($i -lt $rtNcs.Instructions.Count) { $rtNcs.Instructions[$i] } else { $null }

                if ($origInst -and $rtInst) {
                    $argsMatch = $true
                    if ($origInst.Args.Count -eq $rtInst.Args.Count) {
                        for ($j = 0; $j -lt $origInst.Args.Count; $j++) {
                            if ($origInst.Args[$j] -ne $rtInst.Args[$j]) {
                                $argsMatch = $false
                                break
                            }
                        }
                    } else {
                        $argsMatch = $false
                    }

                    if ($origInst.InsType -eq $rtInst.InsType -and $argsMatch) {
                        Write-Host "[$i] ✓ MATCH: $($origInst.InsType)" -ForegroundColor Green
                        $matchCount++
                    } else {
                        Write-Host "[$i] ✗ DIFFER: " -ForegroundColor Red -NoNewline
                        Write-Host "Orig=$($origInst.InsType) RT=$($rtInst.InsType)"
                        $mismatchCount++
                    }
                }
                elseif ($origInst) {
                    Write-Host "[$i] ✗ MISSING IN ROUND-TRIP: $($origInst.InsType)" -ForegroundColor Red
                    $mismatchCount++
                }
                elseif ($rtInst) {
                    Write-Host "[$i] ✗ EXTRA IN ROUND-TRIP: $($rtInst.InsType)" -ForegroundColor Red
                    $mismatchCount++
                }
            }

            Write-Host ""
            Write-Host "Summary:" -ForegroundColor Yellow
            Write-Host "  Original: $($origNcs.Instructions.Count) instructions"
            Write-Host "  Round-trip: $($rtNcs.Instructions.Count) instructions"
            Write-Host "  Matches: $matchCount"
            Write-Host "  Mismatches: $mismatchCount"
        }
    }
}

# Operation: RoundTrip
function Invoke-RoundTrip {
    param(
        [string[]]$InputPath,
        [string]$OutputDirectory,
        [string]$Game,
        [string[]]$LibraryLookup,
        [bool]$Recursive,
        [string]$CompareMode,
        [bool]$KeepIntermediate,
        [bool]$StopOnFirstFailure,
        [bool]$WhatIf
    )

    Load-Assembly

    $filesToProcess = Get-FilesToProcess -Paths $InputPath -Extension ".nss" -Recurse $Recursive

    if ($filesToProcess.Count -eq 0) {
        Write-Error "No NSS files found to process."
        exit 1
    }

    if ([string]::IsNullOrEmpty($OutputDirectory)) {
        $OutputDirectory = Join-Path $script:WorkspaceRoot "test-work" "roundtrip-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
    }

    $OutputDirectory = Resolve-WorkspacePath $OutputDirectory

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

    $libraryLookupList = New-Object System.Collections.Generic.List[string]

    foreach ($file in $filesToProcess) {
        $parentDir = [System.IO.Path]::GetDirectoryName($file.FullName)
        if (-not [string]::IsNullOrEmpty($parentDir) -and -not $libraryLookupList.Contains($parentDir)) {
            $libraryLookupList.Add($parentDir)
        }
    }

    foreach ($lookup in $LibraryLookup) {
        $resolvedLookup = Resolve-WorkspacePath $lookup
        if (Test-Path $resolvedLookup) {
            if (-not $libraryLookupList.Contains($resolvedLookup)) {
                $libraryLookupList.Add($resolvedLookup)
            }
        }
    }

    $gameType = if ($Game -eq "k1") { [CSharpKOTOR.Common.Game]::K1 } else { [CSharpKOTOR.Common.Game]::K2 }

    $successCount = 0
    $failureCount = 0

    foreach ($file in $filesToProcess) {
        $inputFile = $file.FullName
        $baseName = [System.IO.Path]::GetFileNameWithoutExtension($inputFile)
        $relativeDir = $file.DirectoryName.Replace($script:WorkspaceRoot, "").TrimStart('\', '/')

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

            Write-Host "  [2/4] Decompiling NCS..." -NoNewline
            $ncs1Loaded = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($firstNcs)
            $decompiled = [CSharpKOTOR.Formats.NCS.NCSAuto]::DecompileNcs($ncs1Loaded, $gameType)
            [System.IO.File]::WriteAllText($decompiledNss, $decompiled, [System.Text.Encoding]::UTF8)

            if (-not (Test-Path $decompiledNss)) {
                throw "Decompilation did not produce output file"
            }

            $decompiledLength = $decompiled.Length
            Write-Host " ✓ ($decompiledLength chars)" -ForegroundColor Green

            Write-Host "  [3/4] Recompiling decompiled NSS..." -NoNewline
            $ncs2 = [CSharpKOTOR.Formats.NCS.NCSAuto]::CompileNss($decompiled, $gameType, $null, $null, $libraryLookupList)
            [CSharpKOTOR.Formats.NCS.NCSAuto]::WriteNcs($ncs2, $secondNcs)

            if (-not (Test-Path $secondNcs)) {
                throw "Second compilation did not produce output file"
            }

            $size2 = (Get-Item $secondNcs).Length
            $inst2 = $ncs2.Instructions.Count
            Write-Host " ✓ ($size2 bytes, $inst2 instructions)" -ForegroundColor Green

            Write-Host "  [4/4] Comparing bytecode..." -NoNewline

            $bytecodeMatch = $true
            $instructionsMatch = $true

            if ($CompareMode -eq "bytecode" -or $CompareMode -eq "both") {
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
                    }
                }
            }

            if ($bytecodeMatch -and $instructionsMatch) {
                Write-Host " ✓ MATCH" -ForegroundColor Green
                $successCount++

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
    Write-Host ""

    if ($failureCount -gt 0) {
        exit 1
    }
}

# Operation: Generate ScriptDefs
function Invoke-GenerateDefs {
    param([bool]$WhatIf)

    $K1NssPath = Join-Path $script:WorkspaceRoot "include\k1_nwscript.nss"
    $K2NssPath = Join-Path $script:WorkspaceRoot "include\k2_nwscript.nss"
    $OutputPath = Join-Path $script:WorkspaceRoot "src\CSharpKOTOR\Common\Script\ScriptDefs.cs"

    Write-Host "Generating ScriptDefs.cs from NSS files..." -ForegroundColor Cyan
    Write-Host "  K1: $K1NssPath"
    Write-Host "  K2: $K2NssPath"
    Write-Host "  Output: $OutputPath"

    if ($WhatIf) {
        Write-Host "[WHATIF] Would generate ScriptDefs.cs" -ForegroundColor Yellow
        return
    }

    # Type mapping
    $TypeMap = @{
        'int'       = 'DataType.Int'
        'float'     = 'DataType.Float'
        'string'    = 'DataType.String'
        'void'      = 'DataType.Void'
        'object'    = 'DataType.Object'
        'vector'    = 'DataType.Vector'
        'location'  = 'DataType.Location'
        'effect'    = 'DataType.Effect'
        'event'     = 'DataType.Event'
        'talent'    = 'DataType.Talent'
        'action'    = 'DataType.Action'
        'object_id' = 'DataType.Object'
    }

    $script:allConstants = @{}

    function Get-DataType {
        param([string]$TypeName)
        $normalizedType = $TypeName.ToLower()
        if ($TypeMap.ContainsKey($normalizedType)) {
            return $TypeMap[$normalizedType]
        }
        return $null
    }

    function Parse-NssConstant {
        param([string]$Line)
        if ($Line -match '^\s*(int|float|string)\s+([A-Z_][A-Z0-9_]*)\s*=\s*(.+?)\s*;') {
            $type = $Matches[1]
            $name = $Matches[2]
            $value = $Matches[3].Trim()

            if ($name -cmatch '[a-z]') {
                return $null
            }

            if ($type -eq 'string') {
                $formattedValue = $value
            }
            elseif ($type -eq 'float') {
                if ($value -notmatch 'f$') {
                    $formattedValue = "${value}f"
                } else {
                    $formattedValue = $value
                }
            }
            else {
                if ($value -match '^0x') {
                    $formattedValue = [Convert]::ToInt32($value, 16).ToString()
                } else {
                    $formattedValue = $value
                }
            }

            $dataType = Get-DataType -TypeName $type
            if (-not $dataType) {
                return $null
            }

            return @{
                Type  = $dataType
                Name  = $name
                Value = $formattedValue
            }
        }
        return $null
    }

    function Parse-NssFunction {
        param([string[]]$Lines, [int]$StartIndex)

        $line = $Lines[$StartIndex]
        if ($line -match '^\s*(\w+)\s+(\w+)\s*\(([^)]*)\)\s*;') {
            $returnType = $Matches[1]
            $functionName = $Matches[2]
            $paramsString = $Matches[3].Trim()

            $docLines = @()
            for ($i = $StartIndex - 1; $i -ge 0 -and $i -ge ($StartIndex - 50); $i--) {
                $prevLine = $Lines[$i]
                if ($prevLine -match '^\s*//') {
                    $docLines = @($prevLine) + $docLines
                }
                elseif ($prevLine -match '^\s*$') {
                    continue
                }
                else {
                    break
                }
            }

            $docLines += $line.TrimEnd()
            $documentation = ($docLines -join [Environment]::NewLine)
            $documentation = $documentation.Replace('\', '\\').Replace('"', '\"').Replace("`r", '\r').Replace("`n", '\n')

            $params = @()
            if ($paramsString) {
                $paramParts = @()
                $currentParam = ""
                $depth = 0
                $bracketDepth = 0

                for ($i = 0; $i -lt $paramsString.Length; $i++) {
                    $char = $paramsString[$i]
                    if ($char -eq '(') { $depth++ }
                    elseif ($char -eq ')') { $depth-- }
                    elseif ($char -eq '[') { $bracketDepth++ }
                    elseif ($char -eq ']') { $bracketDepth-- }
                    elseif ($char -eq ',' -and $depth -eq 0 -and $bracketDepth -eq 0) {
                        $paramParts += $currentParam.Trim()
                        $currentParam = ""
                        continue
                    }
                    $currentParam += $char
                }
                if ($currentParam) {
                    $paramParts += $currentParam.Trim()
                }

                foreach ($paramPart in $paramParts) {
                    if ($paramPart -match '^\s*(\w+)\s+(\w+)(?:\s*=\s*(.+))?\s*$') {
                        $paramType = $Matches[1]
                        $paramName = $Matches[2]
                        $paramDefault = if ($Matches[3]) { $Matches[3].Trim() } else { $null }

                        $paramDataType = Get-DataType -TypeName $paramType
                        if (-not $paramDataType) {
                            continue
                        }

                        $formattedDefault = if ($paramDefault) {
                            if ($paramDefault -match '^-?\d+$') {
                                $paramDefault
                            }
                            elseif ($paramDefault -match '^-?\d+\.\d+f?$') {
                                if ($paramDefault -notmatch 'f$') {
                                    "${paramDefault}f"
                                } else {
                                    $paramDefault
                                }
                            }
                            elseif ($paramDefault -match '^".*"$') {
                                $paramDefault
                            }
                            elseif ($paramDefault -match '^\[[\d\.,\s]+\]$') {
                                $vectorParts = $paramDefault -replace '[\[\]]', '' -split ',' | Where-Object { $_ }
                                if ($vectorParts.Count -eq 3) {
                                    $x = $vectorParts[0].Trim()
                                    $y = $vectorParts[1].Trim()
                                    $z = $vectorParts[2].Trim()
                                    "new Vector3(${x}f, ${y}f, ${z}f)"
                                } else {
                                    $paramDefault
                                }
                            }
                            else {
                                if ($paramDefault -eq 'OBJECT_SELF') {
                                    'OBJECT_SELF'
                                }
                                elseif ($paramDefault -eq 'OBJECT_INVALID') {
                                    'OBJECT_INVALID'
                                }
                                elseif ($script:allConstants.ContainsKey($paramDefault)) {
                                    $script:allConstants[$paramDefault]
                                }
                                else {
                                    $paramDefault
                                }
                            }
                        } else {
                            'null'
                        }

                        $params += @{
                            Type    = $paramDataType
                            Name    = $paramName
                            Default = $formattedDefault
                        }
                    }
                }
            }

            $returnDataType = Get-DataType -TypeName $returnType
            if (-not $returnDataType) {
                return $null
            }

            return @{
                ReturnType    = $returnDataType
                Name          = $functionName
                Params        = $params
                Documentation = $documentation
            }
        }
        return $null
    }

    function Parse-NssConstants {
        param([string]$FilePath)
        Write-Host "  Parsing constants from $FilePath..." -ForegroundColor Yellow
        $lines = Get-Content $FilePath
        $constants = @()

        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]
            if ($line -match '^\s*#') {
                continue
            }
            $constant = Parse-NssConstant -Line $line
            if ($constant) {
                $constants += $constant
            }
        }

        Write-Host "    Found $($constants.Count) constants" -ForegroundColor Green
        return $constants
    }

    function Parse-NssFunctions {
        param([string]$FilePath)
        Write-Host "  Parsing functions from $FilePath..." -ForegroundColor Yellow
        $lines = Get-Content $FilePath
        $functions = @()

        for ($i = 0; $i -lt $lines.Count; $i++) {
            $line = $lines[$i]
            if ($line -match '^\s*#') {
                continue
            }
            if ($line -match '^\s*(int|float|string)\s+[A-Z_][A-Z0-9_]*\s*=') {
                continue
            }

            try {
                $function = Parse-NssFunction -Lines $lines -StartIndex $i
                if ($function) {
                    $functions += $function
                }
            }
            catch {
                Write-Warning "Failed to parse function at line $i"
            }
        }

        Write-Host "    Found $($functions.Count) functions" -ForegroundColor Green
        return $functions
    }

    function Generate-ConstantCode {
        param($Constant, [bool]$IsLast = $false)
        $comma = if ($IsLast) { "" } else { "," }
        return "        new ScriptConstant($($Constant.Type), `"$($Constant.Name)`", $($Constant.Value))$comma"
    }

    function Generate-FunctionCode {
        param($Function, [bool]$IsLast = $false)
        $paramCode = @()
        foreach ($param in $Function.Params) {
            $paramCode += "new ScriptParam($($param.Type), `"$($param.Name)`", $($param.Default))"
        }

        $paramListCode = if ($paramCode.Count -gt 0) {
            "new List<ScriptParam>() { $($paramCode -join ', ') }"
        } else {
            "new List<ScriptParam>()"
        }

        $doc = $Function.Documentation
        $comma = if ($IsLast) { "" } else { "," }

        return @"
        new ScriptFunction(
            $($Function.ReturnType),
            "$($Function.Name)",
            $paramListCode,
            "$doc",
            "$doc"
        )$comma
"@
    }

    Write-Host "`nStep 1: Parsing constants..." -ForegroundColor Cyan
    $k1Constants = Parse-NssConstants -FilePath $K1NssPath
    $k2Constants = Parse-NssConstants -FilePath $K2NssPath

    Write-Host "`nStep 2: Building constants lookup table..." -ForegroundColor Cyan
    foreach ($const in $k1Constants) {
        $script:allConstants[$const.Name] = $const.Value
    }
    foreach ($const in $k2Constants) {
        if (-not $script:allConstants.ContainsKey($const.Name)) {
            $script:allConstants[$const.Name] = $const.Value
        }
    }
    Write-Host "  Built lookup table with $($script:allConstants.Count) unique constants" -ForegroundColor Green

    Write-Host "`nStep 3: Parsing functions..." -ForegroundColor Cyan
    $k1Functions = Parse-NssFunctions -FilePath $K1NssPath
    $k2Functions = Parse-NssFunctions -FilePath $K2NssPath

    Write-Host "`nStep 4: Generating C# code..." -ForegroundColor Cyan

    $k1ConstantsCode = ""
    for ($i = 0; $i -lt $k1Constants.Count; $i++) {
        $k1ConstantsCode += (Generate-ConstantCode -Constant $k1Constants[$i] -IsLast ($i -eq $k1Constants.Count - 1)) + "`r`n"
    }

    $k2ConstantsCode = ""
    for ($i = 0; $i -lt $k2Constants.Count; $i++) {
        $k2ConstantsCode += (Generate-ConstantCode -Constant $k2Constants[$i] -IsLast ($i -eq $k2Constants.Count - 1)) + "`r`n"
    }

    $k1FunctionsCode = ""
    for ($i = 0; $i -lt $k1Functions.Count; $i++) {
        $k1FunctionsCode += (Generate-FunctionCode -Function $k1Functions[$i] -IsLast ($i -eq $k1Functions.Count - 1)) + "`r`n"
    }

    $k2FunctionsCode = ""
    for ($i = 0; $i -lt $k2Functions.Count; $i++) {
        $k2FunctionsCode += (Generate-FunctionCode -Function $k2Functions[$i] -IsLast ($i -eq $k2Functions.Count - 1)) + "`r`n"
    }

    $csCode = @"
using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Common.Script;

namespace CSharpKOTOR.Common.Script
{

    /// <summary>
    /// NWScript constant and function definitions for KOTOR and TSL.
    /// Generated from k1_nwscript.nss and k2_nwscript.nss using NcsTool.ps1.
    /// </summary>
    public static class ScriptDefs
    {
        // Built-in constants (not defined in NSS files but used as defaults or commonly referenced)
        public const int OBJECT_SELF = 0;
        public const int OBJECT_INVALID = 1;
        public const int TRUE = 1;
        public const int FALSE = 0;

        /// <summary>
        /// KOTOR (Knights of the Old Republic) script constants.
        /// </summary>
        public static readonly List<ScriptConstant> KOTOR_CONSTANTS = new List<ScriptConstant>()
        {
$k1ConstantsCode        };

        /// <summary>
        /// TSL (The Sith Lords, also known as K2) script constants.
        /// </summary>
        public static readonly List<ScriptConstant> TSL_CONSTANTS = new List<ScriptConstant>()
        {
$k2ConstantsCode        };

        /// <summary>
        /// KOTOR (Knights of the Old Republic) script functions.
        /// </summary>
        public static readonly List<ScriptFunction> KOTOR_FUNCTIONS = new List<ScriptFunction>()
        {
$k1FunctionsCode        };

        /// <summary>
        /// TSL (The Sith Lords, also known as K2) script functions.
        /// </summary>
        public static readonly List<ScriptFunction> TSL_FUNCTIONS = new List<ScriptFunction>()
        {
$k2FunctionsCode        };
    }
}
"@

    Write-Host "Writing output to $OutputPath..." -ForegroundColor Cyan
    [System.IO.File]::WriteAllText($OutputPath, $csCode, [System.Text.Encoding]::UTF8)

    Write-Host "`nDone! Generated ScriptDefs.cs successfully." -ForegroundColor Green
    Write-Host "  Total K1 Constants: $($k1Constants.Count)"
    Write-Host "  Total K1 Functions: $($k1Functions.Count)"
    Write-Host "  Total K2 Constants: $($k2Constants.Count)"
    Write-Host "  Total K2 Functions: $($k2Functions.Count)"
}

# Main dispatch
try {
    switch ($Operation) {
        "compile" {
            Invoke-Compile -InputPath $InputPath -OutputPath $OutputPath -Game $Game -LibraryLookup $LibraryLookup -Recursive $Recursive -Overwrite $Overwrite -WhatIf $WhatIf
        }
        "decompile" {
            Invoke-Decompile -InputPath $InputPath -OutputPath $OutputPath -Game $Game -Recursive $Recursive -Overwrite $Overwrite -WhatIf $WhatIf
        }
        "compare" {
            Invoke-Compare -OriginalFile $OriginalFile -RoundTripFile $RoundTripFile -CompareMode $CompareMode -ShowOnly $ShowOnly -Detailed $Detailed
        }
        "roundtrip" {
            Invoke-RoundTrip -InputPath $InputPath -OutputDirectory $OutputDirectory -Game $Game -LibraryLookup $LibraryLookup -Recursive $Recursive -CompareMode $CompareMode -KeepIntermediate $KeepIntermediate -StopOnFirstFailure $StopOnFirstFailure -WhatIf $WhatIf
        }
        "generate-defs" {
            Invoke-GenerateDefs -WhatIf $WhatIf
        }
        default {
            Write-Error "Unknown operation: $Operation"
            exit 1
        }
    }
}
catch {
    Write-Error "Operation failed: $_"
    exit 1
}

