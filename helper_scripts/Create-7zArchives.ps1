# Create .7z archives for each top-level folder under a given path.
# Usage: .\Create-7zArchives.ps1 -BasePath "dist\build_2026-02-26-02-19\OdyTool2DA.Standalone\net9.0"
# Archives are created in BasePath and named <folder>.7z (e.g. win-x86.7z).

param(
    [Parameter(Mandatory = $true)]
    [string]$BasePath,
    [string]$SevenZipPath = ""
)

$ScriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir "..")).Path
$absBase = if ([IO.Path]::IsPathRooted($BasePath)) { $BasePath } else { Join-Path $RepoRoot $BasePath }

if (-not (Test-Path $absBase -PathType Container)) {
    Write-Error "Base path not found: $absBase"
    exit 1
}

function Find-SevenZip {
    if (-not [string]::IsNullOrWhiteSpace($SevenZipPath)) {
        if (Test-Path -LiteralPath $SevenZipPath -PathType Leaf) { return (Resolve-Path -LiteralPath $SevenZipPath).Path }
        return $null
    }
    $candidates = @(
        (Join-Path $env:ProgramFiles "7-Zip\7z.exe"),
        (Join-Path ${env:ProgramFiles(x86)} "7-Zip\7z.exe"),
        "C:\Program Files\7-Zip\7z.exe",
        "C:\Program Files (x86)\7-Zip\7z.exe"
    )
    foreach ($p in $candidates) {
        if ($p -and (Test-Path -LiteralPath $p -PathType Leaf -ErrorAction SilentlyContinue)) {
            return (Resolve-Path -LiteralPath $p -ErrorAction SilentlyContinue).Path
        }
    }
    $inPath = (Get-Command 7z -ErrorAction SilentlyContinue).Source
    if ($inPath) { return $inPath }
    return $null
}

$exe = Find-SevenZip
if (-not $exe) {
    Write-Error "7-Zip not found. Install 7-Zip or set -SevenZipPath."
    exit 1
}

$dirs = Get-ChildItem -Path $absBase -Directory
foreach ($d in $dirs) {
    $arcName = "$($d.Name).7z"
    $arcPath = Join-Path $absBase $arcName
    Write-Host "Creating $arcName..."
    Push-Location $absBase
    try {
        & $exe a -t7z -y $arcPath $d.Name
        if ($LASTEXITCODE -eq 0) {
            $sizeMB = [math]::Round((Get-Item $arcPath).Length / 1MB, 2)
            Write-Host "  OK: $arcName ($sizeMB MB)"
        } else {
            Write-Warning "  FAIL: $arcName (exit $LASTEXITCODE)"
        }
    } finally {
        Pop-Location
    }
}
Write-Host "Done. Archives in: $absBase"
