# Download PyKotor wiki documentation to a local wiki folder.
# Source: https://github.com/OldRepublicDevs/PyKotor/wiki
# Raw URL format: https://raw.githubusercontent.com/wiki/OldRepublicDevs/PyKotor/Page-Name.md

$ErrorActionPreference = "Stop"
$BaseUrl = "https://raw.githubusercontent.com/wiki/OldRepublicDevs/PyKotor"

# All wiki page names used by OdyTools editors (from EditorWikiMapping) plus index pages
$WikiPages = @(
    "2DA-File-Format",
    "BWM-File-Format",
    "ERF-File-Format",
    "GFF-File-Format",
    "GFF-ARE",
    "GFF-DLG",
    "GFF-FAC",
    "GFF-GIT",
    "GFF-GUI",
    "GFF-IFO",
    "GFF-JRL",
    "GFF-PTH",
    "GFF-UTC",
    "GFF-UTD",
    "GFF-UTE",
    "GFF-UTI",
    "GFF-UTM",
    "GFF-UTP",
    "GFF-UTS",
    "GFF-UTT",
    "GFF-UTW",
    "LIP-File-Format",
    "LTR-File-Format",
    "LYT-File-Format",
    "MDL-MDX-File-Format",
    "NCS-File-Format",
    "NSS-File-Format",
    "SSF-File-Format",
    "TLK-File-Format",
    "TPC-File-Format",
    "WAV-File-Format",
    "Bioware-Aurora-2DA",
    "Bioware-Aurora-AreaFile",
    "Bioware-Aurora-CommonGFFStructs",
    "Bioware-Aurora-Conversation",
    "Bioware-Aurora-Creature",
    "Bioware-Aurora-DoorPlaceableGFF",
    "Bioware-Aurora-Encounter",
    "Bioware-Aurora-ERF",
    "Bioware-Aurora-GFF",
    "Bioware-Aurora-IFO",
    "Bioware-Aurora-Item",
    "Bioware-Aurora-Journal",
    "Bioware-Aurora-KeyBIF",
    "Bioware-Aurora-Merchant",
    "Bioware-Aurora-SSF",
    "Bioware-Aurora-SoundObject",
    "Bioware-Aurora-TalkTable",
    "Bioware-Aurora-Trigger",
    "Bioware-Aurora-Waypoint",
    "Home",
    "README"
)

# Resolve wiki directory: prefer repo root wiki, then current directory
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir "..")).Path
$WikiDir = Join-Path $RepoRoot "wiki"

if (-not (Test-Path $WikiDir)) {
    New-Item -ItemType Directory -Path $WikiDir | Out-Null
    Write-Host "Created wiki directory: $WikiDir"
}

$Downloaded = 0
$Failed = @()

foreach ($page in $WikiPages) {
    $fileName = "$page.md"
    $outPath = Join-Path $WikiDir $fileName
    $url = "$BaseUrl/$fileName"
    try {
        Invoke-WebRequest -Uri $url -OutFile $outPath -UseBasicParsing -ErrorAction Stop
        Write-Host "  OK $fileName"
        $Downloaded++
    } catch {
        if ($_.Exception.Response.StatusCode -eq 404) {
            Write-Host "  -- $fileName (not found on wiki, skipped)"
        } else {
            Write-Host "  FAIL $fileName - $($_.Exception.Message)"
            $Failed += $fileName
        }
    }
}

Write-Host ""
Write-Host "Downloaded $Downloaded files to $WikiDir"
if ($Failed.Count -gt 0) {
    Write-Host "Failed: $($Failed -join ', ')"
    exit 1
}
