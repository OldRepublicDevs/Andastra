# FixProjectReferences.ps1
# Fixes project references in consolidated projects
# Usage: .\scripts\FixProjectReferences.ps1

$ErrorActionPreference = "Stop"
$script:RootPath = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $script:RootPath

Write-Host "Fixing Project References" -ForegroundColor Cyan

# Map old project paths to new consolidated paths
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
    "..\\HolocronToolset\\Data\\Data.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    "..\\HolocronToolset\\Dialogs\\Dialogs.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    "..\\HolocronToolset\\Editors\\Editors.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    "..\\HolocronToolset\\Utils\\Utils.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    "..\\HolocronToolset\\Widgets\\Widgets.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
    "..\\HolocronToolset\\Windows\\Windows.csproj" = "..\\HolocronToolset\\HolocronToolset.csproj"
}

# Find all .csproj files
$projects = Get-ChildItem -Path $script:RootPath -Filter "*.csproj" -Recurse -File |
    Where-Object { $_.FullName -notlike "*\vendor\*" -and $_.FullName -notlike "*\tests\*" }

foreach ($projFile in $projects) {
    $content = Get-Content $projFile.FullName -Raw
    $modified = $false

    foreach ($oldRef in $referenceMap.Keys) {
        $newRef = $referenceMap[$oldRef]
        if ($content -match [regex]::Escape($oldRef)) {
            $content = $content -replace [regex]::Escape($oldRef), $newRef
            $modified = $true
            Write-Host "  Updated: $($projFile.Name)" -ForegroundColor Gray
            Write-Host "    $oldRef -> $newRef" -ForegroundColor DarkGray
        }
    }

    if ($modified) {
        Set-Content -Path $projFile.FullName -Value $content -Encoding UTF8 -NoNewline
    }
}

Write-Host "`n✓ Project references updated" -ForegroundColor Green

