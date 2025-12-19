# Compile Kaitai Struct .ksy files to multiple target languages
# Usage: .\scripts\Compile-KaitaiStruct.ps1 -KsyFile "path/to/file.ksy" -OutputDir "output/" [-Languages @("python","java","javascript")]

param(
    [Parameter(Mandatory=$true)]
    [string]$KsyFile,

    [Parameter(Mandatory=$true)]
    [string]$OutputDir,

    [Parameter(Mandatory=$false)]
    [string[]]$Languages = @(
        "python", "java", "javascript", "csharp", "cpp_stl", "go", "ruby",
        "php", "rust", "swift", "perl", "nim", "lua", "kotlin", "typescript"
    ),

    [switch]$WhatIf
)

$ErrorActionPreference = "Stop"

# Check if kaitai-struct-compiler is available
$compiler = "kaitai-struct-compiler"
if (-not (Get-Command $compiler -ErrorAction SilentlyContinue)) {
    Write-Error "kaitai-struct-compiler not found. Please install it first."
    Write-Host "Installation:"
    Write-Host "  Windows: choco install kaitai-struct-compiler"
    Write-Host "  Linux: wget https://packages.kaitai.io/dists/unstable/main/binary-amd64/kaitai-struct-compiler_0.10_all.deb && sudo dpkg -i kaitai-struct-compiler_0.10_all.deb"
    exit 1
}

# Validate input file
if (-not (Test-Path $KsyFile)) {
    Write-Error "Kaitai Struct file not found: $KsyFile"
    exit 1
}

# Create output directory
if (-not $WhatIf) {
    New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null
}

Write-Host "Compiling $KsyFile to multiple languages..."
Write-Host "Output directory: $OutputDir"
Write-Host "Languages: $($Languages -join ', ')"

$successCount = 0
$failCount = 0
$results = @()

foreach ($lang in $Languages) {
    $langOutputDir = Join-Path $OutputDir $lang
    Write-Host "`nCompiling to $lang..." -ForegroundColor Cyan

    if ($WhatIf) {
        Write-Host "  [WHATIF] Would compile: $compiler -t $lang `"$KsyFile`" -d `"$langOutputDir`""
        $results += [PSCustomObject]@{
            Language = $lang
            Status = "Would compile"
            OutputDir = $langOutputDir
        }
        continue
    }

    try {
        # Create language-specific output directory
        New-Item -ItemType Directory -Force -Path $langOutputDir | Out-Null

        # Compile
        $process = Start-Process -FilePath $compiler -ArgumentList @("-t", $lang, "`"$KsyFile`"", "-d", "`"$langOutputDir`"") -Wait -NoNewWindow -PassThru -RedirectStandardOutput "$langOutputDir\compile_stdout.txt" -RedirectStandardError "$langOutputDir\compile_stderr.txt"

        if ($process.ExitCode -eq 0) {
            Write-Host "  ✓ Successfully compiled to $lang" -ForegroundColor Green
            $successCount++
            $results += [PSCustomObject]@{
                Language = $lang
                Status = "Success"
                OutputDir = $langOutputDir
                ExitCode = $process.ExitCode
            }
        } else {
            Write-Host "  ✗ Failed to compile to $lang (exit code: $($process.ExitCode))" -ForegroundColor Red
            $failCount++
            $results += [PSCustomObject]@{
                Language = $lang
                Status = "Failed"
                OutputDir = $langOutputDir
                ExitCode = $process.ExitCode
                Error = Get-Content "$langOutputDir\compile_stderr.txt" -ErrorAction SilentlyContinue
            }
        }
    } catch {
        Write-Host "  ✗ Error compiling to $lang : $_" -ForegroundColor Red
        $failCount++
        $results += [PSCustomObject]@{
            Language = $lang
            Status = "Error"
            OutputDir = $langOutputDir
            Error = $_.Exception.Message
        }
    }
}

Write-Host "`n" + "="*60
Write-Host "Compilation Summary:"
Write-Host "  Successful: $successCount"
Write-Host "  Failed: $failCount"
Write-Host "  Total: $($Languages.Count)"
Write-Host "="*60

# Return results
return $results
