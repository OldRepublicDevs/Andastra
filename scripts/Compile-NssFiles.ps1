<#
.SYNOPSIS
    Compiles NSS source files to NCS bytecode.

.DESCRIPTION
    This script compiles one or more NSS files to NCS bytecode using the CSharpKOTOR inbuilt compiler.
    Supports single file, multiple files, or recursive directory processing.

.PARAMETER InputPath
    Path to NSS file(s) or directory containing NSS files to compile.
    Can be a single file, multiple files (comma-separated), or a directory.

.PARAMETER OutputPath
    Output path for compiled NCS file(s). If InputPath is a directory, this should also be a directory.
    If not specified, outputs to the same location as input with .ncs extension.

.PARAMETER Game
    Target game version: "k1" (KOTOR) or "k2" (TSL). Defaults to "k2".

.PARAMETER AssemblyPath
    Path to the CSharpKOTOR.dll assembly. Defaults to the standard build output location.

.PARAMETER LibraryLookup
    Additional directories to search for included files. Can be specified multiple times.

.PARAMETER Recursive
    If InputPath is a directory, process all NSS files recursively in subdirectories.

.PARAMETER Overwrite
    Overwrite existing output files. By default, skips files that already exist.

.PARAMETER WhatIf
    Show what would be compiled without actually compiling.

.EXAMPLE
    .\scripts\Compile-NssFiles.ps1 -InputPath "script.nss"

.EXAMPLE
    .\scripts\Compile-NssFiles.ps1 -InputPath "script.nss" -OutputPath "script.ncs" -Game "k1"

.EXAMPLE
    .\scripts\Compile-NssFiles.ps1 -InputPath "C:\Scripts" -OutputPath "C:\Compiled" -Recursive -LibraryLookup "C:\Includes"

.EXAMPLE
    .\scripts\Compile-NssFiles.ps1 -InputPath "script1.nss","script2.nss" -Game "k2"
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

    [string[]]$LibraryLookup = @(),

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
            $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".ncs"
            return Join-Path $output $fileName
        }
        
        # If output doesn't exist and has no extension, treat as directory
        if (-not (Test-Path $output)) {
            $ext = [System.IO.Path]::GetExtension($output)
            if ([string]::IsNullOrEmpty($ext)) {
                $fileName = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".ncs"
                return Join-Path $output $fileName
            }
        }
        
        return $output
    }
    
    # Default: same location as input with .ncs extension
    $dir = [System.IO.Path]::GetDirectoryName($InputFile)
    $name = [System.IO.Path]::GetFileNameWithoutExtension($InputFile) + ".ncs"
    return Join-Path $dir $name
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
        $searchOption = if ($Recursive) { [System.IO.SearchOption]::AllDirectories } else { [System.IO.SearchOption]::TopDirectoryOnly }
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

Write-Host "Found $($filesToProcess.Count) NSS file(s) to compile" -ForegroundColor Cyan
Write-Host "Game: $Game" -ForegroundColor Cyan
if ($LibraryLookup.Count -gt 0) {
    Write-Host "Library lookup paths: $($LibraryLookup.Count)" -ForegroundColor Cyan
}
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
        Write-Host "[WHATIF] Would compile: $inputFile -> $outputFile" -ForegroundColor Yellow
        continue
    }
    
    try {
        Write-Host "Compiling: $([System.IO.Path]::GetFileName($inputFile))" -NoNewline
        
        # Read source
        $source = [System.IO.File]::ReadAllText($inputFile, [System.Text.Encoding]::UTF8)
        
        # Determine game type
        $gameType = if ($Game -eq "k1") { 
            [CSharpKOTOR.Common.Game]::K1 
        } else { 
            [CSharpKOTOR.Common.Game]::K2 
        }
        
        # Compile
        $ncs = [CSharpKOTOR.Formats.NCS.NCSAuto]::CompileNss($source, $gameType, $null, $null, $libraryLookupList)
        
        # Write output
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

