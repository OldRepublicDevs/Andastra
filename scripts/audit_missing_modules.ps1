# Comprehensive audit script to identify missing PyKotor modules
$pykotorSrc = Join-Path $PSScriptRoot "..\vendor\PyKotor\Libraries\PyKotor\src"
$csharpSrc = Join-Path $PSScriptRoot "..\src\CSharpKOTOR"

Write-Host "=== PyKotor to CSharpKOTOR Porting Audit ===" -ForegroundColor Cyan

# Get all Python files (excluding __init__ and __pycache__)
$pyFiles = Get-ChildItem -Path $pykotorSrc -Recurse -Filter "*.py" -ErrorAction SilentlyContinue |
    Where-Object { 
        $_.FullName -notmatch '__pycache__' -and 
        $_.Name -ne '__init__.py' 
    } |
    ForEach-Object {
        $relPath = $_.FullName.Substring($pykotorSrc.Length + 1)
        $relPath = $relPath -replace '\\', '/'
        [PSCustomObject]@{
            Path = $relPath
            Name = $_.Name
            BaseName = $_.BaseName
            Directory = Split-Path -Parent $relPath
        }
    }

Write-Host "`nFound $($pyFiles.Count) Python source files to check" -ForegroundColor Green

# Get all C# files
$csFiles = Get-ChildItem -Path $csharpSrc -Recurse -Filter "*.cs" -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -notmatch '\.disabled$' } |
    ForEach-Object {
        $relPath = $_.FullName.Substring($csharpSrc.Length + 1)
        $relPath = $relPath -replace '\\', '/'
        [PSCustomObject]@{
            Path = $relPath
            Name = $_.Name
            BaseName = $_.BaseName
            Directory = Split-Path -Parent $relPath
        }
    }

Write-Host "Found $($csFiles.Count) C# files" -ForegroundColor Green

# Map Python module paths to C# paths
function Map-PyToCs {
    param([string]$pyPath)
    
    $mappings = @{
        '^pykotor/common/(.+)$' = 'Common/{0}'
        '^pykotor/extract/(.+)$' = 'Extract/{0}'
        '^pykotor/resource/formats/(.+)$' = 'Formats/{0}'
        '^pykotor/resource/generics/(.+)$' = 'Resource/Generics/{0}'
        '^pykotor/tools/(.+)$' = 'Tools/{0}'
        '^pykotor/tslpatcher/(.+)$' = 'TSLPatcher/{0}'
        '^pykotor/merge/(.+)$' = 'Merge/{0}'
        '^utility/(.+)$' = 'Utility/{0}'
    }
    
    foreach ($pattern in $mappings.Keys) {
        if ($pyPath -match $pattern) {
            $template = $mappings[$pattern]
            $name = $matches[1]
            # Convert Python naming to C# (snake_case to PascalCase)
            $parts = $name -split '/'
            $convertedParts = $parts | ForEach-Object {
                if ($_ -match '^(.+)/(.+)$') {
                    $dir = ($_ -split '/')[0] -replace '_([a-z])', { $_.Groups[1].Value.ToUpper() } -replace '^([a-z])', { $_.Groups[1].Value.ToUpper() }
                    $file = ($_ -split '/')[1] -replace '_([a-z])', { $_.Groups[1].Value.ToUpper() } -replace '^([a-z])', { $_.Groups[1].Value.ToUpper() }
                    "$dir/$file"
                } else {
                    $_ -replace '_([a-z])', { $_.Groups[1].Value.ToUpper() } -replace '^([a-z])', { $_.Groups[1].Value.ToUpper() }
                }
            }
            $finalName = ($convertedParts -join '/')
            return ($template -f $finalName) -replace '\.py$', '.cs'
        }
    }
    return $null
}

# Check each Python file
$missing = @()
$found = @()
$partial = @()

foreach ($pyFile in $pyFiles) {
    $expectedCs = Map-PyToCs -pyPath $pyFile.Path
    if ($expectedCs) {
        # Check if C# equivalent exists
        $foundCs = $csFiles | Where-Object { 
            $_.Name -eq ($pyFile.BaseName -replace '_([a-z])', { $_.Groups[1].Value.ToUpper() } -replace '^([a-z])', { $_.Groups[1].Value.ToUpper() }) + '.cs' -or
            $_.BaseName -eq ($pyFile.BaseName -replace '_', '')
        }
        if ($foundCs) {
            $found += [PSCustomObject]@{
                Python = $pyFile.Path
                CSharp = $foundCs.Path
            }
        } else {
            $missing += $pyFile
        }
    } else {
        $partial += $pyFile
    }
}

Write-Host "`n=== Results ===" -ForegroundColor Cyan
Write-Host "Found: $($found.Count) files" -ForegroundColor Green
Write-Host "Missing: $($missing.Count) files" -ForegroundColor Red
Write-Host "Unmapped: $($partial.Count) files" -ForegroundColor Yellow

if ($missing.Count -gt 0) {
    Write-Host "`n=== Missing Files by Module ===" -ForegroundColor Red
    $missing | Group-Object { ($_.Path -split '/')[0..1] -join '/' } | 
        Sort-Object Count -Descending | 
        ForEach-Object {
            Write-Host "`n$($_.Name): $($_.Count) files" -ForegroundColor Yellow
            $_.Group | Select-Object -First 5 | ForEach-Object {
                Write-Host "  - $($_.Path)" -ForegroundColor Gray
            }
            if ($_.Count -gt 5) {
                Write-Host "  ... and $($_.Count - 5) more" -ForegroundColor Gray
            }
        }
}

Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "Porting progress: $([math]::Round(($found.Count / ($found.Count + $missing.Count)) * 100, 1))%"
