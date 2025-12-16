# Fix double-replaced RawBinaryReader references
param(
    [string]$RootPath = "G:\GitHub\HoloPatcher.NET\src\Andastra\Parsing"
)

$files = Get-ChildItem -Path $RootPath -Filter "*.cs" -Recurse

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Fix double-replaced RawBinaryReader
    $content = $content -replace 'Andastra\.Parsing\.Common\.Andastra\.Parsing\.Common\.RawBinaryReader', 'Andastra.Parsing.Common.RawBinaryReader'
    
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.FullName)"
    }
}

Write-Host "Done fixing double replacements"

