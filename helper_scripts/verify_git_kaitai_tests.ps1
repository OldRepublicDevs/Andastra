# Verification script for GIT Kaitai Struct tests
# Verifies that all components are in place for comprehensive testing

$ErrorActionPreference = "Stop"

Write-Host "Verifying GIT Kaitai Struct test infrastructure..." -ForegroundColor Cyan
Write-Host ""

$allGood = $true

# 1. Check GIT.ksy file exists
Write-Host "[1/5] Checking GIT.ksy file..." -NoNewline
$gitKsy = "src\Andastra\Parsing\Resource\Formats\GFF\Generics\GIT\GIT.ksy"
if (Test-Path $gitKsy) {
    $content = Get-Content $gitKsy -Raw
    if ($content -match "id: git" -and $content -match "gff_header" -and $content -match "gff_field_type") {
        Write-Host " PASS" -ForegroundColor Green
    } else {
        Write-Host " FAIL (missing required components)" -ForegroundColor Red
        $allGood = $false
    }
} else {
    Write-Host " FAIL (file not found)" -ForegroundColor Red
    $allGood = $false
}

# 2. Check test file exists
Write-Host "[2/5] Checking GITKaitaiCompilerTests.cs..." -NoNewline
$testFile = "src\Andastra\Tests\Formats\GITKaitaiCompilerTests.cs"
if (Test-Path $testFile) {
    $testContent = Get-Content $testFile -Raw
    if ($testContent -match "TestCompileGITToAllLanguages" -and
        $testContent -match "SupportedLanguages" -and
        $testContent -match "python.*java.*javascript") {
        # Count languages
        $langMatches = [regex]::Matches($testContent, 'TestCompileGITTo\w+\(\)')
        $langCount = $langMatches.Count
        Write-Host " PASS ($langCount individual language tests)" -ForegroundColor Green
    } else {
        Write-Host " FAIL (missing required test methods)" -ForegroundColor Red
        $allGood = $false
    }
} else {
    Write-Host " FAIL (file not found)" -ForegroundColor Red
    $allGood = $false
}

# 3. Check supported languages count
Write-Host "[3/5] Checking supported languages..." -NoNewline
if (Test-Path $testFile) {
    $testContent = Get-Content $testFile -Raw
    $langArrayMatch = [regex]::Match($testContent, 'SupportedLanguages\s*=\s*new\[\]\s*\{([^}]+)\}')
    if ($langArrayMatch.Success) {
        $langs = $langArrayMatch.Groups[1].Value -split ',' | ForEach-Object { $_.Trim().Trim('"') } | Where-Object { $_ }
        if ($langs.Count -ge 12) {
            Write-Host " PASS ($($langs.Count) languages - exceeds 12 requirement)" -ForegroundColor Green
        } else {
            Write-Host " FAIL (only $($langs.Count) languages, need at least 12)" -ForegroundColor Red
            $allGood = $false
        }
    } else {
        Write-Host " WARN (could not parse languages)" -ForegroundColor Yellow
    }
} else {
    Write-Host " SKIP" -ForegroundColor Yellow
}

# 4. Check for test infrastructure scripts
Write-Host "[4/5] Checking test infrastructure scripts..." -NoNewline
$scripts = @(
    "scripts\test_kaitai_git.ps1",
    "scripts\test_kaitai_multilang.ps1",
    "scripts\validate_kaitai_git.ps1"
)
$scriptsFound = 0
foreach ($script in $scripts) {
    if (Test-Path $script) {
        $scriptsFound++
    }
}
if ($scriptsFound -eq $scripts.Count) {
    Write-Host " PASS (all $($scripts.Count) scripts found)" -ForegroundColor Green
} else {
    Write-Host " WARN ($scriptsFound/$($scripts.Count) scripts found)" -ForegroundColor Yellow
}

# 5. Check Java availability (required for compiler)
Write-Host "[5/5] Checking Java (required for compiler)..." -NoNewline
try {
    $null = java -version 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host " PASS" -ForegroundColor Green
    } else {
        Write-Host " FAIL" -ForegroundColor Red
        $allGood = $false
    }
} catch {
    Write-Host " FAIL (Java not found)" -ForegroundColor Red
    $allGood = $false
}

Write-Host ""
Write-Host "=" * 60 -ForegroundColor Cyan
if ($allGood) {
    Write-Host "All checks passed! Test infrastructure is ready." -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "  1. Install Kaitai Struct compiler:" -ForegroundColor White
    Write-Host "     .\scripts\SetupKaitaiCompiler.ps1" -ForegroundColor Gray
    Write-Host "  2. Run tests:" -ForegroundColor White
    Write-Host "     dotnet test --filter `"FullyQualifiedName~GITKaitaiCompilerTests`"" -ForegroundColor Gray
    Write-Host "  3. Or run PowerShell test scripts:" -ForegroundColor White
    Write-Host "     .\scripts\test_kaitai_multilang.ps1" -ForegroundColor Gray
    exit 0
} else {
    Write-Host "Some checks failed. Please review the issues above." -ForegroundColor Red
    exit 1
}

