# CleanupProjects.ps1
# Removes incorrect Compile items and fixes project references
# Usage: .\scripts\CleanupProjects.ps1

$ErrorActionPreference = "Stop"
$script:RootPath = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $script:RootPath

Write-Host "Cleaning up consolidated projects" -ForegroundColor Cyan

# Find all .csproj files in src and scripts (exclude old/temp files)
$projects = Get-ChildItem -Path $script:RootPath -Filter "*.csproj" -Recurse -File -ErrorAction SilentlyContinue |
    Where-Object {
        $_.FullName -notlike "*\vendor\*" -and
        $_.FullName -notlike "*\tests\*" -and
        $_.FullName -notlike "*\DELETEME_*" -and
        $_.Name -notlike "*_20*" -and  # Exclude timestamped backup files
        ($_.FullName -like "*\src\*" -or $_.FullName -like "*\scripts\*")
    }

foreach ($projFile in $projects) {
    $content = Get-Content $projFile.FullName -Raw
    $xml = [xml]$content
    $modified = $false

    # Remove all Compile items (SDK auto-includes them)
    $compileItems = $xml.Project.ItemGroup.Compile
    if ($compileItems) {
        foreach ($itemGroup in $xml.Project.ItemGroup) {
            $compilesToRemove = @()
            foreach ($compile in $itemGroup.Compile) {
                $compilesToRemove += $compile
            }
            foreach ($compile in $compilesToRemove) {
                $itemGroup.RemoveChild($compile) | Out-Null
                $modified = $true
            }
        }
    }

    # Fix project references
    $referenceMap = @{
        "..\\..\\Andastra\\Parsing\\Andastra.Parsing.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\..\\Andastra\\Game\\Andastra.Game.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\..\\Andastra\\Runtime\\Core\\Andastra.Runtime.Core.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\..\\Andastra\\Runtime\\Content\\Andastra.Runtime.Content.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\..\\Andastra\\Runtime\\Graphics\\Andastra.Runtime.Graphics.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\..\\Andastra\\Runtime\\Scripting\\Andastra.Runtime.Scripting.csproj" = "..\\..\\Andastra\\Andastra.csproj"
        "..\\HoloPatcher.UI\\Andastra.Patcher.UI.csproj" = "..\\HoloPatcher.UI\\HoloPatcher.UI.csproj"
        "..\\HoloPatcher.UI\\Views\\Views.csproj" = "..\\HoloPatcher.UI\\HoloPatcher.UI.csproj"
        "..\\HoloPatcher.UI\\ViewModels\\ViewModels.csproj" = "..\\HoloPatcher.UI\\HoloPatcher.UI.csproj"
        "..\\HoloPatcher.UI\\Update\\Update.csproj" = "..\\HoloPatcher.UI\\HoloPatcher.UI.csproj"
        "..\\HoloPatcher.UI\\Rte\\Rte.csproj" = "..\\HoloPatcher.UI\\HoloPatcher.UI.csproj"
        "Data\\Data.csproj" = ""
        "Windows\\Windows.csproj" = ""
        "Utils\\Utils.csproj" = ""
        "Editors\\Editors.csproj" = ""
        "Dialogs\\Dialogs.csproj" = ""
        "Widgets\\Widgets.csproj" = ""
        "..\\Data\\Data.csproj" = ""
        "..\\Utils\\Utils.csproj" = ""
        "..\\Widgets\\Widgets.csproj" = ""
        "..\\Dialogs\\Dialogs.csproj" = ""
        "..\\Editors\\Editors.csproj" = ""
        "..\\KotorDiff\\App\\App.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Cli\\Cli.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Cache\\Cache.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Diff\\Diff.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Differ\\Differ.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Formatters\\Formatters.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Logger\\Logger.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\Resolution\\Resolution.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\KotorDiff\\KotorDiff.NET.csproj" = "..\\KotorDiff\\KotorDiff.csproj"
        "..\\NSSComp\\NSSComp.NET.csproj" = "..\\NSSComp\\NSSComp.csproj"
        "..\\HolocronToolset\\HolocronToolset.NET.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    }

    foreach ($itemGroup in $xml.Project.ItemGroup) {
        foreach ($projRef in $itemGroup.ProjectReference) {
            $include = $projRef.Include
            if ($referenceMap.ContainsKey($include)) {
                $newRef = $referenceMap[$include]
                if ($newRef -eq "") {
                    # Remove reference (merged into same project)
                    $itemGroup.RemoveChild($projRef) | Out-Null
                    $modified = $true
                    Write-Host "  Removed: $include from $($projFile.Name)" -ForegroundColor Gray
                } else {
                    $projRef.Include = $newRef
                    $modified = $true
                    Write-Host "  Updated: $include -> $newRef in $($projFile.Name)" -ForegroundColor Gray
                }
            }
        }
    }

    # Remove empty ItemGroups
    $itemGroupsToRemove = @()
    foreach ($itemGroup in $xml.Project.ItemGroup) {
        if ($itemGroup.ChildNodes.Count -eq 0) {
            $itemGroupsToRemove += $itemGroup
        }
    }
    foreach ($itemGroup in $itemGroupsToRemove) {
        if ($itemGroup.ParentNode -ne $null) {
            $itemGroup.ParentNode.RemoveChild($itemGroup) | Out-Null
            $modified = $true
        }
    }

    if ($modified) {
        $settings = New-Object System.Xml.XmlWriterSettings
        $settings.Indent = $true
        $settings.IndentChars = "  "
        $settings.NewLineChars = "`r`n"
        $settings.Encoding = [System.Text.Encoding]::UTF8
        $settings.OmitXmlDeclaration = $false

        $writer = [System.Xml.XmlWriter]::Create($projFile.FullName, $settings)
        $xml.Save($writer)
        $writer.Close()

        Write-Host "  ✓ Updated: $($projFile.Name)" -ForegroundColor Green
    }
}

Write-Host "`n✓ Cleanup complete" -ForegroundColor Green

