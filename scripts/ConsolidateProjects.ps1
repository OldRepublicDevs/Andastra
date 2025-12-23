# ConsolidateProjects.ps1
# Dynamically consolidates all nested .csproj files into their parent directory projects
# Pattern: ./src/**/*.csproj -> ./src/<foldername>/<foldername>.csproj
# Usage: .\scripts\ConsolidateProjects.ps1 [-DryRun] [-WhatIf]

param(
    [switch]$DryRun,
    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"
$script:RootPath = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $script:RootPath

Write-Host "Project Consolidation Script" -ForegroundColor Cyan
Write-Host "Root: $script:RootPath" -ForegroundColor Gray
if ($DryRun -or $WhatIf) {
    Write-Host "DRY RUN MODE - No files will be modified" -ForegroundColor Yellow
}

function Get-RelativePath {
    param(
        [string]$FullPath,
        [string]$BasePath
    )
    $full = [System.IO.Path]::GetFullPath($FullPath)
    $base = [System.IO.Path]::GetFullPath($BasePath)
    if ($full.StartsWith($base)) {
        return $full.Substring($base.Length).TrimStart('\', '/')
    }
    return $FullPath
}

function Get-TargetProjectPath {
    param([string]$SourceProjectPath)

    # Get relative path from root
    $relative = Get-RelativePath -FullPath $SourceProjectPath -BasePath $script:RootPath

    # Split into parts
    $parts = $relative -split '[\\/]'

    # Pattern: src/<foldername>/.../file.csproj -> src/<foldername>/<foldername>.csproj
    # Pattern: src/Tools/<toolname>/.../file.csproj -> src/Tools/<toolname>/<toolname>.csproj
    # Pattern: scripts/<foldername>/.../file.csproj -> scripts/<foldername>/<foldername>.csproj
    if ($parts.Count -ge 2) {
        $baseDir = $parts[0]  # "src" or "scripts"

        # Special handling for src/Tools - use the tool name, not "Tools"
        if ($baseDir -eq "src" -and $parts.Count -ge 3 -and $parts[1] -eq "Tools") {
            $toolName = $parts[2]  # The actual tool name
            $targetPath = Join-Path $script:RootPath $baseDir "Tools" $toolName "$toolName.csproj"
            return $targetPath
        }

        # For other cases, use the first folder after base
        $folderName = $parts[1]  # Top-level folder name
        $targetPath = Join-Path $script:RootPath $baseDir $folderName "$folderName.csproj"
        return $targetPath
    }

    return $null
}

function Get-AllNestedProjects {
    param([string]$BasePath)

    $projects = @()
    $baseFull = Join-Path $script:RootPath $BasePath

    if (-not (Test-Path $baseFull)) {
        return $projects
    }

    # Find all .csproj files recursively
    $allProjects = Get-ChildItem -Path $baseFull -Filter "*.csproj" -Recurse -File

    foreach ($proj in $allProjects) {
        $relative = Get-RelativePath -FullPath $proj.FullName -BasePath $script:RootPath
        $parts = $relative -split '[\\/]'

        # Only process nested projects (depth > 2: base/folder/sub/.../file.csproj)
        if ($parts.Count -gt 2) {
            $targetPath = Get-TargetProjectPath $proj.FullName

            if ($null -ne $targetPath) {
                $targetRelative = Get-RelativePath -FullPath $targetPath -BasePath $script:RootPath

                # Normalize paths for comparison
                $relativeNormalized = $relative.Replace('\', '/').ToLowerInvariant()
                $targetNormalized = $targetRelative.Replace('\', '/').ToLowerInvariant()

                # Skip if source and target are the same (already at target level)
                # Also check if the filename matches the target pattern (src/X/X.csproj)
                $sourceFileName = [System.IO.Path]::GetFileNameWithoutExtension($relative)
                $targetFileName = [System.IO.Path]::GetFileNameWithoutExtension($targetRelative)
                $parentFolderName = $parts[1]  # The folder name that should match

                $isAtTargetLevel = ($relativeNormalized -eq $targetNormalized) -or
                                   ($sourceFileName -eq $parentFolderName -and $parts.Count -eq 3)

                if (-not $isAtTargetLevel) {
                    $projects += @{
                        Source = $relative
                        Target = $targetRelative
                        SourceFull = $proj.FullName
                        TargetFull = $targetPath
                    }
                }
            }
        }
    }

    return $projects
}

function Get-ProjectContent {
    param([string]$Path)
    if (Test-Path $Path) {
        try {
            $content = Get-Content $Path -Raw -Encoding UTF8
            $xml = New-Object System.Xml.XmlDocument
            $xml.PreserveWhitespace = $false
            $xml.LoadXml($content)
            return $xml
        } catch {
            Write-Host "    ⚠ Error reading $Path : $_" -ForegroundColor Yellow
            return $null
        }
    }
    return $null
}

function Create-NewProject {
    param([string]$ProjectName)

    $xml = New-Object System.Xml.XmlDocument
    $xml.AppendChild($xml.CreateXmlDeclaration("1.0", "UTF-8", $null)) | Out-Null
    $project = $xml.CreateElement("Project")
    $project.SetAttribute("Sdk", "Microsoft.NET.Sdk")
    $xml.AppendChild($project) | Out-Null

    $propGroup = $xml.CreateElement("PropertyGroup")
    $project.AppendChild($propGroup) | Out-Null

    $targetFramework = $xml.CreateElement("TargetFrameworks")
    $targetFramework.InnerText = "net9.0"
    $propGroup.AppendChild($targetFramework) | Out-Null

    $langVersion = $xml.CreateElement("LangVersion")
    $langVersion.InnerText = "7.3"
    $propGroup.AppendChild($langVersion) | Out-Null

    $nullable = $xml.CreateElement("Nullable")
    $nullable.InnerText = "disable"
    $propGroup.AppendChild($nullable) | Out-Null

    $implicitUsings = $xml.CreateElement("ImplicitUsings")
    $implicitUsings.InnerText = "disable"
    $propGroup.AppendChild($implicitUsings) | Out-Null

    return $xml
}

function Merge-PropertyGroup {
    param(
        [xml]$Target,
        [xml]$Source
    )

    $targetProps = $Target.Project.PropertyGroup
    $sourceProps = $Source.Project.PropertyGroup

    if (-not $sourceProps) { return }

    # Get or create first PropertyGroup in target
    if (-not $targetProps -or $targetProps.Count -eq 0) {
        $newPropGroup = $Target.CreateElement("PropertyGroup")
        $Target.Project.AppendChild($newPropGroup) | Out-Null
        $targetProps = @($newPropGroup)
    }

    $targetPropGroup = $targetProps[0]

    if ($null -eq $targetPropGroup) { return }

    foreach ($sourceProp in $sourceProps) {
        foreach ($child in $sourceProp.ChildNodes) {
            if ($child.NodeType -eq [System.Xml.XmlNodeType]::Element) {
                $existing = $targetPropGroup.SelectSingleNode($child.Name)
                if ($null -eq $existing) {
                    $newNode = $Target.ImportNode($child, $true)
                    $targetPropGroup.AppendChild($newNode) | Out-Null
                } elseif ($null -ne $existing -and $existing.InnerText -ne $child.InnerText) {
                    # Keep target value if different
                    Write-Host "      ⚠ Property conflict: $($child.Name) (keeping target)" -ForegroundColor DarkYellow
                }
            }
        }
    }
}

function Merge-ItemGroup {
    param(
        [xml]$Target,
        [xml]$Source,
        [string]$ItemType
    )

    $targetItems = $Target.Project.ItemGroup.$ItemType
    $sourceItems = $Source.Project.ItemGroup.$ItemType

    if (-not $sourceItems) { return }

    # Get or create ItemGroup in target
    $targetItemGroup = $null
    foreach ($ig in $Target.Project.ItemGroup) {
        if ($ig.$ItemType.Count -gt 0) {
            $targetItemGroup = $ig
            break
        }
    }

    if ($null -eq $targetItemGroup) {
        $targetItemGroup = $Target.CreateElement("ItemGroup")
        $Target.Project.AppendChild($targetItemGroup) | Out-Null
    }

    foreach ($sourceItem in $sourceItems) {
        if ($null -eq $sourceItem -or $sourceItem.NodeType -ne [System.Xml.XmlNodeType]::Element) {
            continue
        }

        $include = $sourceItem.Include
        $condition = $sourceItem.Condition

        # Check if item already exists
        $exists = $false
        if ($targetItems) {
            foreach ($targetItem in $targetItems) {
                if ($null -ne $targetItem) {
                    $targetInclude = $targetItem.Include
                    $targetCondition = $targetItem.Condition

                    if ($targetInclude -eq $include -and $targetCondition -eq $condition) {
                        $exists = $true
                        break
                    }
                }
            }
        }

        if (-not $exists) {
            try {
                $newNode = $Target.ImportNode($sourceItem, $true)
                $targetItemGroup.AppendChild($newNode) | Out-Null
            } catch {
                Write-Host "      ⚠ Error importing node: $_" -ForegroundColor Yellow
            }
        }
    }
}

function Merge-CompileItems {
    param(
        [xml]$Target,
        [string]$SourceDir
    )

    if (-not (Test-Path $SourceDir)) { return }

    # Find all .cs files in source directory (excluding obj/bin)
    $csFiles = Get-ChildItem -Path $SourceDir -Filter "*.cs" -Recurse -File |
        Where-Object {
            $_.FullName -notlike "*\obj\*" -and
            $_.FullName -notlike "*\bin\*" -and
            $_.FullName -notlike "*\*.Designer.cs" -and
            $_.Name -ne "AssemblyInfo.cs"
        }

    if ($csFiles.Count -eq 0) { return }

    # Get or create ItemGroup for Compile items
    $targetItemGroup = $null
    foreach ($ig in $Target.Project.ItemGroup) {
        if ($ig.Compile.Count -gt 0) {
            $targetItemGroup = $ig
            break
        }
    }

    if ($null -eq $targetItemGroup) {
        $targetItemGroup = $Target.CreateElement("ItemGroup")
        $Target.Project.AppendChild($targetItemGroup) | Out-Null
    }

    foreach ($csFile in $csFiles) {
        $relativePath = Get-RelativePath -FullPath $csFile.FullName -BasePath $script:RootPath

        # Check if already included
        $exists = $false
        foreach ($compile in $targetItemGroup.Compile) {
            if ($compile.Include -eq $relativePath) {
                $exists = $true
                break
            }
        }

        if (-not $exists) {
            $compileNode = $Target.CreateElement("Compile")
            $compileNode.SetAttribute("Include", $relativePath)
            $targetItemGroup.AppendChild($compileNode) | Out-Null
        }
    }
}

function Merge-ProjectFiles {
    param(
        [hashtable]$SourceInfo,
        [string]$TargetPath
    )

    $sourceFull = $SourceInfo.SourceFull
    $targetFull = $TargetPath

    Write-Host "    Merging: $($SourceInfo.Source)" -ForegroundColor Gray

    # Load or create target project
    $targetProject = Get-ProjectContent $targetFull
    if ($null -eq $targetProject) {
        $targetDir = Split-Path $targetFull -Parent
        $projectName = Split-Path $targetDir -Leaf
        Write-Host "      Creating new project: $targetFull" -ForegroundColor Cyan
        $targetProject = Create-NewProject -ProjectName $projectName
    }

    # Load source project
    $sourceProject = Get-ProjectContent $sourceFull
    if ($null -eq $sourceProject) {
        Write-Host "      ⚠ Source project not found: $($SourceInfo.Source)" -ForegroundColor Yellow
        return $false
    }

    # Merge PropertyGroups
    Merge-PropertyGroup -Target $targetProject -Source $sourceProject

    # Merge ItemGroups (ProjectReference, PackageReference, etc.)
    $itemTypes = @("ProjectReference", "PackageReference", "Content", "None", "EmbeddedResource", "MonoGameContentReference")
    foreach ($itemType in $itemTypes) {
        Merge-ItemGroup -Target $targetProject -Source $sourceProject -ItemType $itemType
    }

    # Add source files from source directory
    $sourceDir = Split-Path $sourceFull -Parent
    Merge-CompileItems -Target $targetProject -SourceDir $sourceDir

    # Save target project
    if (-not $DryRun -and -not $WhatIf) {
        $targetDir = Split-Path $targetFull -Parent
        if (-not (Test-Path $targetDir)) {
            New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
        }

        $settings = New-Object System.Xml.XmlWriterSettings
        $settings.Indent = $true
        $settings.IndentChars = "  "
        $settings.NewLineChars = "`r`n"
        $settings.Encoding = [System.Text.Encoding]::UTF8
        $settings.OmitXmlDeclaration = $false

        $writer = [System.Xml.XmlWriter]::Create($targetFull, $settings)
        $targetProject.Save($writer)
        $writer.Close()

        Write-Host "      ✓ Saved: $targetFull" -ForegroundColor Green
    } else {
        Write-Host "      [DRY RUN] Would save: $targetFull" -ForegroundColor Yellow
    }

    return $true
}

# Main execution
Write-Host "`n=== Discovering Projects ===" -ForegroundColor Cyan

# Group projects by target
$projectGroups = @{}

# Process src/ directory
$srcProjects = Get-AllNestedProjects "src"
foreach ($proj in $srcProjects) {
    if (-not $projectGroups.ContainsKey($proj.TargetFull)) {
        $projectGroups[$proj.TargetFull] = @()
    }
    $projectGroups[$proj.TargetFull] += $proj
}

# Process scripts/ directory
$scriptsProjects = Get-AllNestedProjects "scripts"
foreach ($proj in $scriptsProjects) {
    if (-not $projectGroups.ContainsKey($proj.TargetFull)) {
        $projectGroups[$proj.TargetFull] = @()
    }
    $projectGroups[$proj.TargetFull] += $proj
}

Write-Host "Found $($projectGroups.Count) target projects to consolidate" -ForegroundColor White

# Process each target project
Write-Host "`n=== Consolidating Projects ===" -ForegroundColor Cyan

foreach ($targetPath in $projectGroups.Keys | Sort-Object) {
    $sources = $projectGroups[$targetPath]
    $targetRelative = Get-RelativePath -FullPath $targetPath -BasePath $script:RootPath

    Write-Host "`n[$targetRelative]" -ForegroundColor Cyan
    Write-Host "  Consolidating $($sources.Count) source project(s)" -ForegroundColor White

    # Merge all source projects into target
    foreach ($source in $sources) {
        Merge-ProjectFiles -SourceInfo $source -TargetPath $targetPath
    }

    # Mark source projects for deletion
    if (-not $DryRun -and -not $WhatIf) {
        foreach ($source in $sources) {
            if (Test-Path $source.SourceFull) {
                Remove-Item -Path $source.SourceFull -Force
                Write-Host "    ✓ Deleted: $($source.Source)" -ForegroundColor DarkGreen
            }
        }
    } else {
        foreach ($source in $sources) {
            Write-Host "    [DRY RUN] Would delete: $($source.Source)" -ForegroundColor Yellow
        }
    }
}

# Handle test project renames
Write-Host "`n=== Renaming Test Projects ===" -ForegroundColor Cyan

$renames = @{
    "src\Tests\KotorDiff.Tests\KotorDiff.NET.Tests.csproj" = "src\Tests\KotorDiff.Tests\KotorDiff.Tests.csproj"
    "src\Tests\HolocronToolset.Tests\HolocronToolset.NET.Tests.csproj" = "src\Tests\HolocronToolset.Tests\HolocronToolset.Tests.csproj"
}

foreach ($oldPath in $renames.Keys) {
    $newPath = $renames[$oldPath]
    $oldFull = Join-Path $script:RootPath $oldPath
    $newFull = Join-Path $script:RootPath $newPath

    if (Test-Path $oldFull) {
        Write-Host "  Rename: $oldPath -> $newPath" -ForegroundColor White
        if (-not $DryRun -and -not $WhatIf) {
            Move-Item -Path $oldFull -Destination $newFull -Force
            Write-Host "    ✓ Renamed" -ForegroundColor Green
        } else {
            Write-Host "    [DRY RUN] Would rename" -ForegroundColor Yellow
        }
    }
}

Write-Host "`n=== Consolidation Complete ===" -ForegroundColor Green
if ($DryRun -or $WhatIf) {
    Write-Host "This was a DRY RUN. Run without -DryRun to apply changes." -ForegroundColor Yellow
}
