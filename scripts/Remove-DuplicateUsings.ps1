# Removes duplicate "using" directives from C# files to fix CS0105 warnings.
# Run from repo root: .\scripts\Remove-DuplicateUsings.ps1 -Path "src\BioWare"

param([string]$Path = "src\BioWare")

$utf8NoBom = New-Object System.Text.UTF8Encoding $false
$count = 0
Get-ChildItem -Path $Path -Filter "*.cs" -Recurse -File | ForEach-Object {
    $fullName = $_.FullName
    $raw = [System.IO.File]::ReadAllText($fullName, [System.Text.Encoding]::UTF8)
    if (-not $raw) { return }
    $lineList = $raw -split "`r?`n"
    $seen = @{}
    $newLines = [System.Collections.ArrayList]@()
    foreach ($line in $lineList) {
        if ($line -match '^\s*using\s+([^;]+)\s*;\s*$') {
            $u = $matches[1].Trim()
            if ($u -and $seen[$u]) {
                $count++
                continue
            }
            if ($u) { $seen[$u] = $true }
        }
        [void]$newLines.Add($line)
    }
    if ($newLines.Count -lt $lineList.Count) {
        $newContent = ($newLines -join [Environment]::NewLine) + [Environment]::NewLine
        [System.IO.File]::WriteAllText($fullName, $newContent, $utf8NoBom)
        Write-Host "Fixed: $fullName"
    }
}
Write-Host "Total duplicate using directives removed: $count"
