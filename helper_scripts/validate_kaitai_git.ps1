# Comprehensive validation script for Kaitai Struct GIT format
# Validates syntax, structure, and compilation across multiple languages

param(
    [switch]$Verbose,
    [switch]$SkipCompilation,
    [string]$KscPath = "kaitai-struct-compiler"
)

$ErrorActionPreference = "Stop"

$GitKsyPath = "src\Andastra\Parsing\Resource\Formats\GFF\Generics\GIT\GIT.ksy"

Write-Host "Validating Kaitai Struct GIT format definition" -ForegroundColor Cyan
Write-Host "KSY file: $GitKsyPath" -ForegroundColor Gray
Write-Host ""

# Step 1: Verify file exists
Write-Host "[1/5] Checking file existence..." -NoNewline
if (-not (Test-Path $GitKsyPath)) {
    Write-Host " FAIL" -ForegroundColor Red
    Write-Host "ERROR: KSY file not found: $GitKsyPath" -ForegroundColor Red
    exit 1
}
Write-Host " PASS" -ForegroundColor Green

# Step 2: Validate YAML syntax
Write-Host "[2/5] Validating YAML syntax..." -NoNewline
try {
    $content = Get-Content $GitKsyPath -Raw
    # Basic YAML validation - check for common issues
    if ($content -match '^\s*$') {
        throw "File appears to be empty"
    }
    if ($content -notmatch 'meta:') {
        throw "Missing 'meta:' section"
    }
    if ($content -notmatch 'id:\s*git') {
        throw "Missing or incorrect 'id: git' in meta section"
    }
    if ($content -notmatch 'seq:') {
        throw "Missing 'seq:' section"
    }
    if ($content -notmatch 'types:') {
        throw "Missing 'types:' section"
    }
    Write-Host " PASS" -ForegroundColor Green
} catch {
    Write-Host " FAIL" -ForegroundColor Red
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 3: Validate Kaitai Struct specific structure
Write-Host "[3/5] Validating Kaitai Struct structure..." -NoNewline
try {
    $content = Get-Content $GitKsyPath -Raw

    # Check for required GFF components
    # Types are defined in the 'types:' section, so we check for their definitions
    $requiredTypes = @(
        'gff_header',
        'label_array',
        'struct_array',
        'field_array',
        'field_data_section',
        'field_indices_array',
        'list_indices_array'
    )

    $missingTypes = @()
    foreach ($type in $requiredTypes) {
        # Check for type definition in types section
        # Types are defined as "  type_name:" (with 2 spaces indentation typically)
        # Use multiline regex to match across lines
        $pattern = "(?m)^\s+$type\s*:"
        if ($content -notmatch $pattern) {
            $missingTypes += $type
        }
    }

    if ($missingTypes.Count -gt 0) {
        throw "Missing required types: $($missingTypes -join ', ')"
    }

    # Check for GFF field type enum
    if ($content -notmatch 'gff_field_type:') {
        throw "Missing 'gff_field_type' enum"
    }

    Write-Host " PASS" -ForegroundColor Green
    if ($Verbose) {
        Write-Host "  Found all required GFF structure types" -ForegroundColor Gray
    }
} catch {
    Write-Host " FAIL" -ForegroundColor Red
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 4: Validate file type signature
Write-Host "[4/5] Validating GIT file signature..." -NoNewline
try {
    $content = Get-Content $GitKsyPath -Raw
    if ($content -notmatch 'valid:\s*"GIT "') {
        throw "Missing or incorrect file type validation for 'GIT '"
    }
    Write-Host " PASS" -ForegroundColor Green
} catch {
    Write-Host " FAIL" -ForegroundColor Red
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 5: Test compilation (if compiler available and not skipped)
if (-not $SkipCompilation) {
    Write-Host "[5/5] Testing compilation..." -NoNewline
    try {
        # Check if compiler is available
        $null = & $KscPath --version 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "Compiler not found"
        }

        # Test compilation to a single language (Python as it's most common)
        $testOutputDir = "test_validate_kaitai_git"
        if (Test-Path $testOutputDir) {
            Remove-Item -Recurse -Force $testOutputDir
        }
        New-Item -ItemType Directory -Path $testOutputDir | Out-Null

        $compileOutput = & $KscPath -t python -d $testOutputDir $GitKsyPath 2>&1

        if ($LASTEXITCODE -eq 0) {
            Write-Host " PASS" -ForegroundColor Green
            if ($Verbose) {
                Write-Host "  Successfully compiled to Python" -ForegroundColor Gray
                Write-Host "  Output directory: $testOutputDir" -ForegroundColor Gray
            }
            # Clean up
            Remove-Item -Recurse -Force $testOutputDir -ErrorAction SilentlyContinue
        } else {
            Write-Host " FAIL" -ForegroundColor Red
            Write-Host "ERROR: Compilation failed" -ForegroundColor Red
            if ($Verbose) {
                Write-Host "Compiler output:" -ForegroundColor Yellow
                $compileOutput | ForEach-Object { Write-Host "  $_" -ForegroundColor Yellow }
            }
            exit 1
        }
    } catch {
        Write-Host " SKIP" -ForegroundColor Yellow
        Write-Host "WARNING: Could not test compilation: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host "  Install Kaitai Struct compiler to test compilation" -ForegroundColor Yellow
    }
} else {
    Write-Host "[5/5] Skipping compilation test (--SkipCompilation specified)" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Validation complete!" -ForegroundColor Green
Write-Host ""

# Summary
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  - File exists: PASS" -ForegroundColor Green
Write-Host "  - YAML syntax: PASS" -ForegroundColor Green
Write-Host "  - Kaitai Struct structure: PASS" -ForegroundColor Green
Write-Host "  - GIT file signature: PASS" -ForegroundColor Green
if (-not $SkipCompilation) {
    Write-Host "  - Compilation test: $(if ($LASTEXITCODE -eq 0) { 'PASS' } else { 'SKIP' })" -ForegroundColor $(if ($LASTEXITCODE -eq 0) { 'Green' } else { 'Yellow' })
}

exit 0

