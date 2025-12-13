<#
.SYNOPSIS
    Decompiles NCS bytecode files to NSS source code.

.DESCRIPTION
    This script decompiles one or more NCS files to NSS source code using the CSharpKOTOR decompiler.
    Supports single file, multiple files, or recursive directory processing.

.PARAMETER InputPath
    Path to NCS file(s) or directory containing NCS files to decompile.
    Can be a single file, multiple files (comma-separated), or a directory.

.PARAMETER OutputPath
    Output path for decompiled NSS file(s). If InputPath is a directory, this should also be a directory.
    If not specified, outputs to the same location as input with .nss extension.

.PARAMETER Game
    Target game version: "k1" (KOTOR) or "k2" (TSL). Defaults to "k2".

.PARAMETER AssemblyPath
    Path to the CSharpKOTOR.dll assembly. Defaults to the standard build output location.

.PARAMETER Recursive
    If InputPath is a directory, process all NCS files recursively in subdirectories.

.PARAMETER Overwrite
    Overwrite existing output files. By default, skips files that already exist.

.PARAMETER WhatIf
    Show what would be decompiled without actually decompiling.

.EXAMPLE
    .\scripts\Decompile-NcsFiles.ps1 -InputPath "script.ncs"

.EXAMPLE
    .\scripts\Decompile-NcsFiles.ps1 -InputPath "script.ncs" -OutputPath "script_decompiled.nss" -Game "k1"

.EXAMPLE
    .\scripts\Decompile-NcsFiles.ps1 -InputPath "C:\Scripts" -OutputPath "C:\Decompiled" -Recursive

.EXAMPLE
    .\scripts\Decompile-NcsFiles.ps1 -InputPath "script1.ncs","script2.ncs" -Game "k2"
#>
[CmdletBinding(SupportsShouldProcess=$true)]
param(
    [Parameter(Mandatory=$true, Position=0)]
    [string[]]$InputPath,

    [Parameter(Mandatory=$false)]
    [string]$OutputPath,

    [ValidateSet("k1", "k2")]
    [string]$Game = "k2",

    [string]$AssemblyPath = "src/CSharpKOTOR/bin/Debug/net9/CSharpKOTOR.dll",

    [switch]$Recursive,

    [switch]$Overwrite,

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

# Helper function to get output path for a file
function Get-OutputPath {
    param(
        [string]$InputFile,
        [string]$BaseOutput
    )
    
    if ($BaseOutput) {
        if ([System.IO.Path]::IsPathRooted($BaseOutput)) {
            $output = $BaseOutput
        } else {
            $output = Join-Path $workspaceRoot $BaseOutput
        }
        
        # If output is a directory, append filename
        if ((Test-Path $output) -and (Get-Item $output) -is [System.IO.DirectoryInfo]) {
            $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".nss"
            return Join-Path $output $fileName
        }
        
        # If output doesn't exist and has no extension, treat as directory
        if (-not (Test-Path $output)) {
            $ext = [System.IO.Path]::GetExtension($output)
            if ([string]::IsNullOrEmpty($ext)) {
                $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".nss"
                return Join-Path $output $fileName
            }
        }
        
        return $output
    }
    
    # Default: same location as input with .nss extension
    $dir = [System.IO.Path]::GetDirectoryName($InputFile)
    $name = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".nss"
    return Join-Path $dir $name
}

# Collect all NCS files to process
$filesToProcess = @()

foreach ($path in $InputPath) {
    $resolvedPath = Resolve-InputPath $path
    
    if (-not (Test-Path $resolvedPath)) {
        Write-Warning "Path not found: $resolvedPath"
        continue
    }
    
    $item = Get-Item $resolvedPath
    
    if ($item -is [System.IO.DirectoryInfo]) {
        $searchOption = if ($Recursive) { [System.IO.SearchOption]::AllDirectories } else { [System.IO.SearchOption]::TopDirectoryOnly }
        $ncsFiles = Get-ChildItem -Path $resolvedPath -Filter "*.ncs" -File -Recurse:$Recursive -ErrorAction SilentlyContinue
        $filesToProcess += $ncsFiles
    }
    elseif ($item -is [System.IO.FileInfo]) {
        if ($item.Extension -eq ".ncs") {
            $filesToProcess += $item
        } else {
            Write-Warning "File is not an NCS file: $resolvedPath"
        }
    }
}

if ($filesToProcess.Count -eq 0) {
    Write-Error "No NCS files found to process."
    exit 1
}

Write-Host "Found $($filesToProcess.Count) NCS file(s) to decompile" -ForegroundColor Cyan
Write-Host "Game: $Game" -ForegroundColor Cyan
Write-Host ""

# Process each file
$successCount = 0
$skipCount = 0
$errorCount = 0

foreach ($file in $filesToProcess) {
    $inputFile = $file.FullName
    $outputFile = Get-OutputPath $inputFile $OutputPath
    
    # Create output directory if needed
    $outputDir = [System.IO.Path]::GetDirectoryName($outputFile)
    if (-not [string]::IsNullOrEmpty($outputDir) -and -not (Test-Path $outputDir)) {
        if ($WhatIf) {
            Write-Host "[WHATIF] Would create directory: $outputDir" -ForegroundColor Yellow
        } else {
            New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
        }
    }
    
    # Check if output exists
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
        
        # Load NCS file
        $ncs = [CSharpKOTOR.Formats.NCS.NCSAuto]::ReadNcs($inputFile)
        
        # Determine game type
        $gameType = if ($Game -eq "k1") { 
            [CSharpKOTOR.Common.Game]::K1 
        } else { 
            [CSharpKOTOR.Common.Game]::K2 
        }
        
        # Decompile
        $nssCode = [CSharpKOTOR.Formats.NCS.NCSAuto]::DecompileNcs($ncs, $gameType)
        
        # Write output
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

