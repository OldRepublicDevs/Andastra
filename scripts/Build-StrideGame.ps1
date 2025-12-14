#Requires -Version 5.1

<#
.SYNOPSIS
    Builds a Stride game project for distribution.

.DESCRIPTION
    Builds the Odyssey Game project in Release mode for the specified platform(s).
    Supports Windows, Linux, and macOS with x64 and x86 architectures.
    Follows Stride game engine best practices for release builds.

.PARAMETER ProjectPath
    Path to the game project (.csproj file). Defaults to src/OdysseyRuntime/Odyssey.Game/Odyssey.Game.csproj

.PARAMETER Configuration
    Build configuration. Defaults to "Release".

.PARAMETER Platforms
    Comma-separated list of platforms to build for. Options: Windows, Linux, macOS, All
    Defaults to "All"

.PARAMETER Architectures
    Comma-separated list of architectures to build for. Options: x64, x86, arm64, All
    Defaults to "x64"

.PARAMETER SelfContained
    Whether to create self-contained deployments (includes .NET runtime).
    Defaults to $true

.PARAMETER SingleFile
    Whether to publish as a single-file executable.
    Defaults to $false

.PARAMETER OutputPath
    Base output path for builds. Defaults to "dist"

.PARAMETER Clean
    Clean build directories before building.

.PARAMETER Restore
    Restore NuGet packages before building.

.EXAMPLE
    .\Build-StrideGame.ps1

.EXAMPLE
    .\Build-StrideGame.ps1 -Platforms "Windows" -Architectures "x64,x86"

.EXAMPLE
    .\Build-StrideGame.ps1 -Platforms "Linux" -SelfContained $false
#>

[CmdletBinding()]
param(
    [string]$ProjectPath = "src/OdysseyRuntime/Odyssey.Game/Odyssey.Game.csproj",
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",
    [string[]]$Platforms = @("All"),
    [string[]]$Architectures = @("x64"),
    [bool]$SelfContained = $true,
    [bool]$SingleFile = $false,
    [string]$OutputPath = "dist",
    [switch]$Clean,
    [switch]$Restore
)

$ErrorActionPreference = "Stop"

# Cross-platform path handling
if ($IsWindows -or $env:OS -eq "Windows_NT") {
    $IsWindowsPlatform = $true
} else {
    $IsWindowsPlatform = $false
}

function Normalize-Path {
    param([string]$Path)
    if ($IsWindowsPlatform) {
        return $Path -replace '/', '\'
    } else {
        return $Path -replace '\\', '/'
    }
}

$ProjectPath = Normalize-Path $ProjectPath
$OutputPath = Normalize-Path $OutputPath

# Validate project exists
if (-not (Test-Path $ProjectPath)) {
    Write-Error "Project file not found: $ProjectPath"
    exit 1
}

# Platform and architecture mappings
$PlatformRIDs = @{
    Windows = @{
        x64 = "win-x64"
        x86 = "win-x86"
        arm64 = "win-arm64"
    }
    Linux = @{
        x64 = "linux-x64"
        x86 = "linux-x86"
        arm64 = "linux-arm64"
    }
    macOS = @{
        x64 = "osx-x64"
        arm64 = "osx-arm64"
    }
}

# Expand "All" keywords
if ($Platforms -contains "All") {
    $Platforms = @("Windows", "Linux", "macOS")
}

if ($Architectures -contains "All") {
    $Architectures = @("x64", "x86", "arm64")
}

# Filter valid platform/architecture combinations
$BuildTargets = @()
foreach ($Platform in $Platforms) {
    if (-not $PlatformRIDs.ContainsKey($Platform)) {
        Write-Warning "Unknown platform: $Platform. Skipping."
        continue
    }
    
    foreach ($Arch in $Architectures) {
        if ($PlatformRIDs[$Platform].ContainsKey($Arch)) {
            $RID = $PlatformRIDs[$Platform][$Arch]
            $BuildTargets += @{
                Platform = $Platform
                Architecture = $Arch
                RID = $RID
            }
        } else {
            Write-Warning "Architecture $Arch not supported for platform $Platform. Skipping."
        }
    }
}

if ($BuildTargets.Count -eq 0) {
    Write-Error "No valid build targets specified."
    exit 1
}

Write-Host "Building Stride Game for $($BuildTargets.Count) target(s)..." -ForegroundColor Cyan

# Restore packages if requested
if ($Restore) {
    Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
    dotnet restore $ProjectPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Package restore failed."
        exit 1
    }
}

# Build each target
foreach ($Target in $BuildTargets) {
    $RID = $Target.RID
    $Platform = $Target.Platform
    $Arch = $Target.Architecture
    
    $PublishDir = Join-Path $OutputPath "$Platform-$Arch"
    $PublishDir = Normalize-Path $PublishDir
    
    Write-Host "`nBuilding for $Platform ($Arch) - RID: $RID" -ForegroundColor Green
    
    # Clean if requested
    if ($Clean -and (Test-Path $PublishDir)) {
        Write-Host "Cleaning output directory: $PublishDir" -ForegroundColor Yellow
        Remove-Item -Path $PublishDir -Recurse -Force -ErrorAction SilentlyContinue
    }
    
    # Build publish arguments
    $PublishArgs = @(
        "publish"
        $ProjectPath
        "--configuration", $Configuration
        "--runtime", $RID
        "--output", $PublishDir
        "-p:SelfContained=$SelfContained"
        "-p:PublishReadyToRun=true"
    )
    
    if ($SingleFile) {
        $PublishArgs += "-p:PublishSingleFile=true"
    }
    
    # Execute publish
    Write-Host "Executing: dotnet $($PublishArgs -join ' ')" -ForegroundColor Gray
    & dotnet $PublishArgs
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Build failed for $Platform ($Arch)"
        exit 1
    }
    
    Write-Host "Build completed: $PublishDir" -ForegroundColor Green
}

Write-Host "`nAll builds completed successfully!" -ForegroundColor Cyan
Write-Host "Output directory: $OutputPath" -ForegroundColor Cyan

