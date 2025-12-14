#Requires -Version 5.1

<#
.SYNOPSIS
    Packages a Stride game build for distribution.

.DESCRIPTION
    Cleans up release builds by removing debug files, unnecessary files, and organizing
    the distribution according to Stride game engine best practices. Creates ZIP archives
    for distribution.

.PARAMETER BuildPath
    Path to the build output directory. Defaults to "dist"

.PARAMETER GameName
    Name of the game for packaging. Defaults to "Odyssey"

.PARAMETER Version
    Version string for the package. Defaults to "1.0.0"

.PARAMETER CreateArchive
    Whether to create ZIP archives. Defaults to $true

.PARAMETER ArchiveFormat
    Archive format. Options: zip, tar, tar.gz. Defaults to "zip"

.PARAMETER IncludeDocumentation
    Whether to include documentation files. Defaults to $true

.PARAMETER IncludeLicenses
    Whether to include license files. Defaults to $true

.PARAMETER DocumentationPath
    Path to documentation directory. Defaults to "docs"

.PARAMETER KeepDebugSymbols
    Whether to keep .pdb files. Defaults to $false

.EXAMPLE
    .\Package-StrideGame.ps1 -BuildPath "dist" -GameName "Odyssey" -Version "1.0.0"

.EXAMPLE
    .\Package-StrideGame.ps1 -BuildPath "dist" -CreateArchive $false -KeepDebugSymbols $true
#>

[CmdletBinding()]
param(
    [string]$BuildPath = "dist",
    [string]$GameName = "Odyssey",
    [string]$Version = "1.0.0",
    [bool]$CreateArchive = $true,
    [ValidateSet("zip", "tar", "tar.gz")]
    [string]$ArchiveFormat = "zip",
    [bool]$IncludeDocumentation = $true,
    [bool]$IncludeLicenses = $true,
    [string]$DocumentationPath = "docs",
    [bool]$KeepDebugSymbols = $false
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

$BuildPath = Normalize-Path $BuildPath
$DocumentationPath = Normalize-Path $DocumentationPath

if (-not (Test-Path $BuildPath)) {
    Write-Error "Build path not found: $BuildPath"
    exit 1
}

Write-Host "Packaging Stride game: $GameName v$Version" -ForegroundColor Cyan

# Get all platform directories
$PlatformDirs = Get-ChildItem -Path $BuildPath -Directory -ErrorAction SilentlyContinue

if ($PlatformDirs.Count -eq 0) {
    Write-Error "No platform directories found in: $BuildPath"
    exit 1
}

foreach ($PlatformDir in $PlatformDirs) {
    $PlatformName = $PlatformDir.Name
    Write-Host "`nProcessing: $PlatformName" -ForegroundColor Green
    
    $PlatformPath = $PlatformDir.FullName
    
    # Files and patterns to remove (Stride best practices)
    $FilesToRemove = @(
        "*.pdb",           # Debug symbols
        "*.xml",           # XML documentation
        "*vshost*",        # Visual Studio host process files
        "*.vshost.exe",
        "*.vshost.exe.manifest",
        "*.vshost.exe.config"
    )
    
    if (-not $KeepDebugSymbols) {
        Write-Host "Removing debug files..." -ForegroundColor Yellow
        foreach ($Pattern in $FilesToRemove) {
            Get-ChildItem -Path $PlatformPath -Filter $Pattern -Recurse -ErrorAction SilentlyContinue | 
                Remove-Item -Force -ErrorAction SilentlyContinue
        }
    }
    
    # Remove unnecessary folders (keep only x64, x86, Data, and platform-specific folders)
    $ValidFolders = @("x64", "x86", "Data", "data", "Assets", "Resources")
    $FoldersToKeep = @()
    
    # Determine which folders to keep based on platform
    if ($PlatformName -match "Windows") {
        $FoldersToKeep = @("x64", "x86", "Data")
    } elseif ($PlatformName -match "Linux") {
        $FoldersToKeep = @("x64", "x86", "Data")
    } elseif ($PlatformName -match "macOS") {
        $FoldersToKeep = @("x64", "arm64", "Data")
    }
    
    # Get all directories at root level
    $RootDirs = Get-ChildItem -Path $PlatformPath -Directory -ErrorAction SilentlyContinue
    
    foreach ($Dir in $RootDirs) {
        $DirName = $Dir.Name
        if ($FoldersToKeep -notcontains $DirName -and $ValidFolders -notcontains $DirName) {
            # Check if it's a known output folder (contains executables)
            $HasExe = Get-ChildItem -Path $Dir.FullName -Filter "*.exe" -ErrorAction SilentlyContinue
            $HasDll = Get-ChildItem -Path $Dir.FullName -Filter "*.dll" -ErrorAction SilentlyContinue
            $HasAppHost = Get-ChildItem -Path $Dir.FullName -Filter "*" -File -ErrorAction SilentlyContinue | 
                Where-Object { $_.Extension -eq "" -and ($_.Attributes -band [System.IO.FileAttributes]::Executable) }
            
            if (-not $HasExe -and -not $HasDll -and -not $HasAppHost) {
                Write-Host "Removing unnecessary folder: $DirName" -ForegroundColor Yellow
                Remove-Item -Path $Dir.FullName -Recurse -Force -ErrorAction SilentlyContinue
            }
        }
    }
    
    # Add documentation if requested
    if ($IncludeDocumentation -and (Test-Path $DocumentationPath)) {
        $DocDestPath = Join-Path $PlatformPath "Documentation"
        $DocDestPath = Normalize-Path $DocDestPath
        
        if (-not (Test-Path $DocDestPath)) {
            New-Item -Path $DocDestPath -ItemType Directory -Force | Out-Null
        }
        
        Write-Host "Copying documentation..." -ForegroundColor Yellow
        Copy-Item -Path (Join-Path $DocumentationPath "*") -Destination $DocDestPath -Recurse -Force -ErrorAction SilentlyContinue
    }
    
    # Add license files if requested
    if ($IncludeLicenses) {
        $LicenseFiles = @("LICENSE", "LICENSE.txt", "LICENSE.md", "COPYING", "COPYING.txt")
        $FoundLicense = $false
        
        foreach ($LicenseFile in $LicenseFiles) {
            $LicensePath = Normalize-Path $LicenseFile
            if (Test-Path $LicensePath) {
                Write-Host "Copying license file: $LicenseFile" -ForegroundColor Yellow
                Copy-Item -Path $LicensePath -Destination $PlatformPath -Force -ErrorAction SilentlyContinue
                $FoundLicense = $true
                break
            }
        }
        
        # Check root directory for license files
        $RootLicensePath = Join-Path (Split-Path -Parent $BuildPath) "LICENSE*"
        $RootLicenses = Get-ChildItem -Path $RootLicensePath -ErrorAction SilentlyContinue
        if ($RootLicenses -and -not $FoundLicense) {
            Write-Host "Copying license file from root: $($RootLicenses[0].Name)" -ForegroundColor Yellow
            Copy-Item -Path $RootLicenses[0].FullName -Destination $PlatformPath -Force -ErrorAction SilentlyContinue
        }
    }
    
    # Create README for distribution
    $ReadmePath = Join-Path $PlatformPath "README.txt"
    $ReadmeContent = @"
$GameName v$Version
Platform: $PlatformName

SYSTEM REQUIREMENTS:
- $(if ($PlatformName -match "Windows") { ".NET Runtime (if framework-dependent) or Windows 7+" } elseif ($PlatformName -match "Linux") { ".NET Runtime (if framework-dependent) or compatible Linux distribution" } else { ".NET Runtime (if framework-dependent) or macOS 10.12+" })
- DirectX 11, OpenGL 4.2, or Vulkan 1.0 compatible graphics card
- 2GB+ RAM recommended

INSTALLATION:
Extract this archive to your desired installation directory.

RUNNING:
$(if ($PlatformName -match "Windows") { "Run the .exe file in the x64 or x86 folder (depending on your system)." } elseif ($PlatformName -match "Linux") { "Run the executable file in the x64 or x86 folder. You may need to make it executable first: chmod +x <executable>" } else { "Run the executable file in the x64 or arm64 folder (depending on your Mac)." })

For more information, see Documentation/ folder.

$(if ($IncludeLicenses) { "LICENSE: See LICENSE file for license information." })
"@
    
    Set-Content -Path $ReadmePath -Value $ReadmeContent -Encoding UTF8
    
    # Create archive if requested
    if ($CreateArchive) {
        $ArchiveName = "$GameName-v$Version-$PlatformName.$ArchiveFormat"
        $ArchivePath = Join-Path $BuildPath $ArchiveName
        $ArchivePath = Normalize-Path $ArchivePath
        
        Write-Host "Creating archive: $ArchiveName" -ForegroundColor Yellow
        
        if ($ArchiveFormat -eq "zip") {
            # Cross-platform ZIP creation
            if ($IsWindowsPlatform) {
                # Use .NET compression on Windows (works on PowerShell 5.1+)
                Add-Type -AssemblyName System.IO.Compression.FileSystem
                if (Test-Path $ArchivePath) {
                    Remove-Item $ArchivePath -Force
                }
                [System.IO.Compression.ZipFile]::CreateFromDirectory($PlatformPath, $ArchivePath)
            } else {
                # Use zip command on Unix-like systems
                $ZipCmd = Get-Command zip -ErrorAction SilentlyContinue
                if ($ZipCmd) {
                    Push-Location $BuildPath
                    zip -r $ArchiveName $PlatformName
                    Pop-Location
                } else {
                    Write-Warning "zip command not found. Skipping archive creation."
                }
            }
        } elseif ($ArchiveFormat -eq "tar" -or $ArchiveFormat -eq "tar.gz") {
            # Use tar command (available on Linux/macOS and Windows 10+)
            $TarCmd = Get-Command tar -ErrorAction SilentlyContinue
            if ($TarCmd) {
                Push-Location $BuildPath
                $PlatformDirName = Split-Path -Leaf $PlatformPath
                $TarArgs = @("cf", $ArchiveName, $PlatformDirName)
                if ($ArchiveFormat -eq "tar.gz") {
                    $TarArgs = @("czf", $ArchiveName, $PlatformDirName)
                }
                & tar $TarArgs
                Pop-Location
            } else {
                Write-Warning "tar command not found. Skipping archive creation."
            }
        }
        
        if (Test-Path $ArchivePath) {
            $ArchiveSize = (Get-Item $ArchivePath).Length / 1MB
            Write-Host "Archive created: $ArchiveName ($([math]::Round($ArchiveSize, 2)) MB)" -ForegroundColor Green
        }
    }
}

Write-Host "`nPackaging completed!" -ForegroundColor Cyan

