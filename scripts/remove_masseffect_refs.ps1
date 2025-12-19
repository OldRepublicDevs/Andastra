# Script to remove Mass Effect references from codebase
# Usage: .\scripts\remove_masseffect_refs.ps1

$patterns = @(
    @{ Pattern = '(?i)MassEffect\.exe'; Replacement = '' },
    @{ Pattern = '(?i)MassEffect2\.exe'; Replacement = '' },
    @{ Pattern = '(?i)Mass Effect'; Replacement = '' },
    @{ Pattern = '(?i)MassEffect'; Replacement = '' },
    @{ Pattern = '(?i)\bME1\b'; Replacement = '' },
    @{ Pattern = '(?i)\bME2\b'; Replacement = '' },
    @{ Pattern = '(?i)\bME3\b'; Replacement = '' },
    @{ Pattern = '(?i)ME1/ME2'; Replacement = '' },
    @{ Pattern = '(?i)ME1 format'; Replacement = 'Dragon Age format' },
    @{ Pattern = '(?i)ME2 format'; Replacement = 'Dragon Age 2 format' }
)

$directories = @(
    'src\Andastra\Runtime\Games\Eclipse',
    'src\Andastra\Runtime\Games\Infinity',
    'src\Andastra\Runtime\Core',
    'src\Andastra\Runtime\Graphics',
    'src\Andastra\Parsing',
    'docs'
)

foreach ($dir in $directories) {
    if (Test-Path $dir) {
        Write-Host "Processing $dir..."
        Get-ChildItem -Path $dir -Recurse -Filter '*.cs' | ForEach-Object {
            $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
            if ($content) {
                $modified = $false
                $newContent = $content
                foreach ($p in $patterns) {
                    if ($newContent -match $p.Pattern) {
                        $newContent = $newContent -replace $p.Pattern, $p.Replacement
                        $modified = $true
                    }
                }
                if ($modified) {
                    Set-Content -Path $_.FullName -Value $newContent -NoNewline
                    Write-Host "  Modified: $($_.Name)"
                }
            }
        }
    }
}

Write-Host "Done!"

