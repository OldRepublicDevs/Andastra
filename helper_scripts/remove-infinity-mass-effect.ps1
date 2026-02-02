# Script to remove Infinity Engine and Mass Effect references from codebase
# This script removes references to engines we don't support

$files = Get-ChildItem -Path "src\Andastra" -Recurse -Include *.cs,*.md,*.csproj | Where-Object { $_.FullName -notmatch "vendor" }

$replacements = @(
    @{ Pattern = "MassEffect\.exe|MassEffect2\.exe"; Replacement = "" },
    @{ Pattern = "Mass Effect|mass effect"; Replacement = "" },
    @{ Pattern = "ME1|ME2"; Replacement = "" },
    @{ Pattern = "Infinity Engine|Infinity engine"; Replacement = "" },
    @{ Pattern = "InfinityEngine|InfinityGraphicsBackend"; Replacement = "" },
    @{ Pattern = "EngineFamily\.Infinity"; Replacement = "" },
    @{ Pattern = "Baldur's Gate|BaldurGate"; Replacement = "" },
    @{ Pattern = "Icewind Dale|IcewindDale"; Replacement = "" },
    @{ Pattern = "Planescape: Torment|PlanescapeTorment"; Replacement = "" }
)

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
    if ($null -eq $content) { continue }
    
    $originalContent = $content
    $modified = $false
    
    foreach ($replacement in $replacements) {
        if ($content -match $replacement.Pattern) {
            $content = $content -replace $replacement.Pattern, $replacement.Replacement
            $modified = $true
        }
    }
    
    if ($modified) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Modified: $($file.FullName)"
    }
}

Write-Host "Cleanup complete!"

