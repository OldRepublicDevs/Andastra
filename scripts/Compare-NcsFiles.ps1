<#
.SYNOPSIS
    Compares two NCS files and displays their instructions side-by-side.

.DESCRIPTION
    This script loads two NCS files, parses their instructions, and displays them
    in a format that makes it easy to identify differences. Useful for debugging
    bytecode mismatches in round-trip tests.
    
    Can compare bytecode byte-by-byte, instruction structure, or both.

.PARAMETER OriginalFile
    Path to the first NCS file to compare (typically the original compiled file).

.PARAMETER RoundTripFile
    Path to the second NCS file to compare (typically the round-trip compiled file).

.PARAMETER AssemblyPath
    Path to the CSharpKOTOR.dll assembly. Defaults to the standard build output location.

.PARAMETER ShowOnly
    If specified, only show instructions from the specified file ("original" or "roundtrip").
    By default, shows both files side-by-side.

.PARAMETER CompareMode
    Comparison mode: "bytecode" (strict byte-by-byte), "instructions" (compare instruction structure), or "both". Defaults to "both".

.PARAMETER Detailed
    Show detailed byte-by-byte comparison for mismatches.

.EXAMPLE
    .\scripts\Compare-NcsFiles.ps1 -OriginalFile "test.ncs" -RoundTripFile "test.rt.ncs"

.EXAMPLE
    .\scripts\Compare-NcsFiles.ps1 -OriginalFile "test.ncs" -RoundTripFile "test.rt.ncs" -ShowOnly "original"

.EXAMPLE
    .\scripts\Compare-NcsFiles.ps1 -OriginalFile "test.ncs" -RoundTripFile "test.rt.ncs" -CompareMode "bytecode" -Detailed
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$OriginalFile,

    [Parameter(Mandatory=$true)]
    [string]$RoundTripFile,

    [string]$AssemblyPath = "src/CSharpKOTOR/bin/Debug/net9/CSharpKOTOR.dll",

    [ValidateSet("original", "roundtrip", "both")]
    [string]$ShowOnly = "both",

    [ValidateSet("bytecode", "instructions", "both")]
    [string]$CompareMode = "both",

    [switch]$Detailed
)

# Resolve paths relative to workspace root
$workspaceRoot = $PSScriptRoot | Split-Path -Parent
$originalPath = if ([System.IO.Path]::IsPathRooted($OriginalFile)) { $OriginalFile } else { Join-Path $workspaceRoot $OriginalFile }
$roundTripPath = if ([System.IO.Path]::IsPathRooted($RoundTripFile)) { $RoundTripFile } else { Join-Path $workspaceRoot $RoundTripFile }
$assemblyFullPath = if ([System.IO.Path]::IsPathRooted($AssemblyPath)) { $AssemblyPath } else { Join-Path $workspaceRoot $AssemblyPath }

# Validate files exist
if (-not (Test-Path $originalPath)) {
    Write-Error "Original file not found: $originalPath"
    exit 1
}

if (-not (Test-Path $roundTripPath)) {
    Write-Error "Round-trip file not found: $roundTripPath"
    exit 1
}

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
        $minLen = [Math]::Min($origBytes.Length, $rtBytes.Length)
        Write-Host "  First $minLen bytes match, then files diverge." -ForegroundColor Yellow
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
        
        if ($Detailed) {
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
    
    # Load NCS files
    try {
        $origNcs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($originalPath)
        $rtNcs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($roundTripPath)
    }
    catch {
        Write-Error "Failed to load NCS files: $_"
        exit 1
    }
    
    # Format instruction for display
    function Format-Instruction {
        param(
            [int]$Index,
            [object]$Instruction
        )
    
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
    
    # Display instructions
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
    
    # Side-by-side comparison if both are shown
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
        Write-Host "  Difference: $([Math]::Abs($origNcs.Instructions.Count - $rtNcs.Instructions.Count)) instructions"
    }
}
