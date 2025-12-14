#Requires -Version 5.1

<#
.SYNOPSIS
    Adds documentation to Stride game distribution packages.

.DESCRIPTION
    Copies documentation files from the documentation directory into game distribution
    packages. Organizes documentation according to industry best practices.

.PARAMETER BuildPath
    Path to the build output directory. Defaults to "dist"

.PARAMETER DocumentationPath
    Path to the documentation directory. Defaults to "docs"

.PARAMETER IncludeReadme
    Whether to include README.md from project root. Defaults to $true

.PARAMETER IncludeQuickStart
    Whether to include quick start guides. Defaults to $true

.PARAMETER IncludeChangelog
    Whether to include CHANGELOG.md if available. Defaults to $true

.PARAMETER CreateIndex
    Whether to create an index.html for documentation. Defaults to $false

.EXAMPLE
    .\Add-StrideGameDocumentation.ps1 -BuildPath "dist" -DocumentationPath "docs"

.EXAMPLE
    .\Add-StrideGameDocumentation.ps1 -IncludeReadme $false -CreateIndex $true
#>

[CmdletBinding()]
param(
    [string]$BuildPath = "dist",
    [string]$DocumentationPath = "docs",
    [bool]$IncludeReadme = $true,
    [bool]$IncludeQuickStart = $true,
    [bool]$IncludeChangelog = $true,
    [bool]$CreateIndex = $false
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

Write-Host "Adding documentation to Stride game packages..." -ForegroundColor Cyan

# Get all platform directories
$PlatformDirs = Get-ChildItem -Path $BuildPath -Directory -ErrorAction SilentlyContinue | 
    Where-Object { -not $_.Name -match "\.(zip|tar|gz)$" }

if ($PlatformDirs.Count -eq 0) {
    Write-Warning "No platform directories found in: $BuildPath"
    exit 0
}

foreach ($PlatformDir in $PlatformDirs) {
    $PlatformName = $PlatformDir.Name
    Write-Host "`nProcessing: $PlatformName" -ForegroundColor Green
    
    $DocDestPath = Join-Path $PlatformDir.FullName "Documentation"
    $DocDestPath = Normalize-Path $DocDestPath
    
    if (-not (Test-Path $DocDestPath)) {
        New-Item -Path $DocDestPath -ItemType Directory -Force | Out-Null
    }
    
    # Copy documentation directory contents
    if (Test-Path $DocumentationPath) {
        Write-Host "Copying documentation from: $DocumentationPath" -ForegroundColor Yellow
        
        $DocFiles = Get-ChildItem -Path $DocumentationPath -File -Recurse
        foreach ($File in $DocFiles) {
            $RelativePath = $File.FullName.Substring((Resolve-Path $DocumentationPath).Path.Length + 1)
            $DestFile = Join-Path $DocDestPath $RelativePath
            $DestDir = Split-Path -Parent $DestFile
            
            if (-not (Test-Path $DestDir)) {
                New-Item -Path $DestDir -ItemType Directory -Force | Out-Null
            }
            
            Copy-Item -Path $File.FullName -Destination $DestFile -Force
        }
        Write-Host "Copied $($DocFiles.Count) documentation file(s)" -ForegroundColor Gray
    }
    
    # Copy README.md from root if requested
    if ($IncludeReadme) {
        $RootReadme = Normalize-Path "README.md"
        if (Test-Path $RootReadme) {
            Write-Host "Copying README.md from project root" -ForegroundColor Yellow
            Copy-Item -Path $RootReadme -Destination (Join-Path $DocDestPath "README.md") -Force
        }
    }
    
    # Copy CHANGELOG.md if requested
    if ($IncludeChangelog) {
        $Changelog = Normalize-Path "CHANGELOG.md"
        if (Test-Path $Changelog) {
            Write-Host "Copying CHANGELOG.md" -ForegroundColor Yellow
            Copy-Item -Path $Changelog -Destination (Join-Path $DocDestPath "CHANGELOG.md") -Force
        }
    }
    
    # Create index.html if requested
    if ($CreateIndex) {
        $IndexPath = Join-Path $DocDestPath "index.html"
        $IndexContent = @"
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Documentation Index</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; background: #f5f5f5; }
        .container { max-width: 800px; margin: 0 auto; background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        h1 { color: #333; border-bottom: 2px solid #4CAF50; padding-bottom: 10px; }
        ul { list-style-type: none; padding: 0; }
        li { margin: 10px 0; }
        a { color: #2196F3; text-decoration: none; padding: 8px 12px; display: block; border-left: 3px solid #2196F3; background: #f9f9f9; }
        a:hover { background: #e3f2fd; }
    </style>
</head>
<body>
    <div class="container">
        <h1>Documentation</h1>
        <ul>
"@
        
        $DocFilesInDest = Get-ChildItem -Path $DocDestPath -File -Recurse | Where-Object { $_.Extension -in @(".md", ".txt", ".html") }
        foreach ($File in $DocFilesInDest) {
            $RelativePath = $File.FullName.Substring($DocDestPath.Length + 1)
            $RelativePath = $RelativePath -replace '\\', '/'
            $DisplayName = $File.BaseName -replace '_', ' ' -replace '-', ' '
            $IndexContent += "            <li><a href=""$RelativePath"">$DisplayName</a></li>`n"
        }
        
        $IndexContent += @"
        </ul>
    </div>
</body>
</html>
"@
        
        Set-Content -Path $IndexPath -Value $IndexContent -Encoding UTF8
        Write-Host "Created documentation index: index.html" -ForegroundColor Yellow
    }
}

Write-Host "`nDocumentation addition completed!" -ForegroundColor Green

