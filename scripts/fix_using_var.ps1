$files = Get-ChildItem -Path "src/TSLPatcher.tests" -Recurse -Filter "*.cs"
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $original = $content

    # Convert "using var x = ...;" to "using (var x = ...) { }"
    # This is a simple pattern - may need refinement for complex cases
    $content = $content -replace 'using var ([a-zA-Z0-9_]+) = ([^;]+);', 'using (var $1 = $2) { }'

    if ($content -ne $original) {
        Set-Content $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.Name)"
    }
}

