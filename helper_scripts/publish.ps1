# PowerShell version of publish_release.bat - multi-RID publish + zip archives.
# Data-driven profiles; comprehensive logging. Run from project root.
# When -ProjectFile is omitted, discovers all .csproj from the solution (Andastra.sln). This script never writes or modifies any .sln file.
# Projects under toplevel .history and vendor are excluded from discovery and build.
#
# Examples:
#   .\helper_scripts\publish.ps1
#   .\helper_scripts\publish.ps1 -SolutionPath Andastra.sln
#   .\helper_scripts\publish.ps1 -ProjectFile "MyApp\MyApp.csproj"
#   .\helper_scripts\publish.ps1 -KeepLogsAndBuilds 5   # keep 5 most recent logs/builds (default 3); 0 = disable
#   .\helper_scripts\publish.ps1 -BuildOdyToolsInstaller   # stage OdyTools AIO + editors and build WiX installer

param(
    [Alias("Project", "P")]
    [string]$ProjectFile = "",              # When empty: discover publishable projects from solution.
    [Alias("Solution", "S")]
    [string]$SolutionPath = "",              # Path to .sln (required when -ProjectFile is empty and multiple .sln exist in CWD).
    [string]$FrameworkDependent = "net48",  # net48 or net472 for non-self-contained profiles
    [Alias("Framework")]
    [string]$FrameworkVersion = "net9.0",
    [Alias("SevenZip", "7z")]
    [string]$SevenZipPath = "C:\Program Files\7-Zip\7z.exe",
    [Alias("Output", "Out", "O")]
    [string]$OutputDir = "dist",
    [Alias("V")]
    [switch]$Verbose,
    [Alias("D")]
    [switch]$Debug,
    [ValidateSet("SilentlyContinue", "Stop", "Continue", "Inquire", "Ignore", "Suspend")]
    [string]$ErrorAction = "Inquire",
    [Alias("Archive", "Archives", "A")]
    [switch]$CreateArchives = $true,
    # Keep only the N most recent publish logs and build_* directories; 0 = disable rotation.
    [Alias("Keep", "Rotate")]
    [int]$KeepLogsAndBuilds = 3,
    # Stage OdyTools AIO + standalone editors and build the WiX installer (Windows only).
    [switch]$BuildOdyToolsInstaller = $false,
    # Run ONLY the OdyTools installer staging + WiX build (skip main project publishes). Use for quick installer pipeline verification.
    [switch]$BuildOdyToolsInstallerOnly = $false
)

$ScriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
$InitialCwd = (Get-Location).Path
$RepoRootFromScript = (Resolve-Path (Join-Path $ScriptDir "..")).Path

# Resolve solution path. When -SolutionPath is empty: use single .sln in CWD; if multiple .sln exist, require -SolutionPath.
function Resolve-SolutionPath {
    if (-not [string]::IsNullOrWhiteSpace($SolutionPath)) {
        $p = if ([IO.Path]::IsPathRooted($SolutionPath)) { $SolutionPath } else { Join-Path $InitialCwd $SolutionPath }
        if (-not (Test-Path -LiteralPath $p -PathType Leaf)) { $p = Join-Path $RepoRootFromScript $SolutionPath }
        if (-not (Test-Path -LiteralPath $p -PathType Leaf)) { throw "Solution file not found: $SolutionPath" }
        $resolved = (Resolve-Path -LiteralPath $p).Path
        $relFromRepo = $resolved.Substring($RepoRootFromScript.TrimEnd([IO.Path]::DirectorySeparatorChar).Length).TrimStart([IO.Path]::DirectorySeparatorChar)
        if (Test-PathExcluded -PathOrRel $relFromRepo) { throw "Solution path is under .history or vendor: $SolutionPath" }
        return $resolved
    }
    # Exclude .sln files under .history or vendor (toplevel or any path segment)
    $slnExclude = { param($f) $norm = ($f.FullName -replace '\\', '/').ToLowerInvariant(); $norm -match '/\.history(/|$)|/vendor(/|$)' }
    $slnsInCwd = @(Get-ChildItem -Path $InitialCwd -Filter "*.sln" -File -ErrorAction SilentlyContinue | Where-Object { -not (& $slnExclude $_) })
    if ($slnsInCwd.Count -gt 1) {
        throw "Multiple solution files found in current directory. Specify -SolutionPath (e.g. -SolutionPath Andastra.sln)"
    }
    if ($slnsInCwd.Count -eq 1) {
        return (Resolve-Path -LiteralPath $slnsInCwd[0].FullName).Path
    }
    $slnsInRepo = @(Get-ChildItem -Path $RepoRootFromScript -Filter "*.sln" -File -ErrorAction SilentlyContinue | Where-Object { -not (& $slnExclude $_) })
    if ($slnsInRepo.Count -gt 1) {
        throw "Multiple solution files found in repo root. Specify -SolutionPath (e.g. -SolutionPath Andastra.sln)"
    }
    if ($slnsInRepo.Count -eq 1) {
        return (Resolve-Path -LiteralPath $slnsInRepo[0].FullName).Path
    }
    return $null
}

# Exclude paths under toplevel .history or vendor (or any path segment)
function Test-PathExcluded {
    param([string]$PathOrRel)
    $norm = ($PathOrRel -replace '\\', '/').TrimStart('/').ToLowerInvariant()
    return ($norm -match '(^|/)(\.history|vendor)(/|$)')
}

# Parse .sln for all .csproj paths (full path); excludes .history and vendor path segments
function Get-SolutionProjectPaths {
    param([string]$SlnPath, [string]$Root)
    if (-not (Test-Path -LiteralPath $SlnPath -PathType Leaf)) { return @() }
    $content = Get-Content -LiteralPath $SlnPath -Raw -ErrorAction SilentlyContinue
    $regexMatches = [regex]::Matches($content, 'Project\("[^"]+"\)\s*=\s*"[^"]+",\s*"([^"]+\.csproj)"')
    $slnDir = Split-Path $SlnPath -Parent
    $projects = foreach ($m in $regexMatches) {
        $rel = $m.Groups[1].Value -replace '\\', '/'
        if (Test-PathExcluded -PathOrRel $rel) { continue }
        $full = Join-Path $slnDir ($rel -replace '/', [IO.Path]::DirectorySeparatorChar)
        if (Test-Path -LiteralPath $full) { (Resolve-Path -LiteralPath $full).Path }
    }
    return @($projects)
}

$RepoRoot = $RepoRootFromScript
if (-not [string]::IsNullOrWhiteSpace($ProjectFile)) {
    $ResolvedSolutionPath = $null
} else {
    $ResolvedSolutionPath = Resolve-SolutionPath
    if ($ResolvedSolutionPath) { $RepoRoot = (Resolve-Path (Split-Path -Parent $ResolvedSolutionPath)).Path }
}
Set-Location $RepoRoot
# Absolute path to the folder containing the .sln (trailing separator) for MSBuild /p:SolutionDir
$Script:SolutionDirAbsolute = $RepoRoot.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar

$BuildTimestamp = Get-Date -Format 'yyyy-MM-yy-HH-mm'
$LogFile = Join-Path $RepoRoot "publish_release_$BuildTimestamp.log"
$ErrorActionPreference = $ErrorAction

# --- Logging ---
$Script:LogColors = @{ ERROR = "Red"; WARN = "Yellow"; INFO = "White"; DEBUG = "DarkGray"; VERBOSE = "Gray" }
function Write-Log {
    [CmdletBinding()]
    param([string]$Message, [string]$Level = "INFO", [hashtable]$Variables = @{})
    $ts = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logLine = "[$ts] [$Level] $Message"
    if ($Variables.Count -gt 0) { $logLine += " | Variables: " + ($Variables.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join ", " }
    $color = $Script:LogColors[$Level]
    if ($color) { Write-Host $logLine -ForegroundColor $color } else { Write-Host $logLine -ForegroundColor Gray }
    if ($Level -eq "DEBUG") { Write-Debug $logLine }
    elseif ($Level -eq "VERBOSE") { Write-Verbose $logLine }
    Add-Content -Path $LogFile -Value $logLine -ErrorAction SilentlyContinue
}

if ($Debug) { $DebugPreference = "Continue"; $VerbosePreference = "Continue"; Write-Log "Debug logging enabled" -Level "INFO" }
elseif ($Verbose) { $VerbosePreference = "Continue"; Write-Log "Verbose logging enabled" -Level "INFO" }

# --- Platform display names ---
function Get-PlatformDisplayName {
    param([string]$Rid)
    $map = @{ "win-x64" = "Windows 64-bit"; "win-x86" = "Windows 32-bit"; "linux-x64" = "Linux 64-bit"; "linux-arm64" = "Linux ARM64"; "osx-x64" = "macOS Intel"; "osx-arm64" = "macOS Apple Silicon" }
    if ($map[$Rid]) { return $map[$Rid] }; return $Rid
}

# --- Path helpers ---
function Get-PublishFolderPath {
    param([string]$Framework, [string]$Rid, [string]$LastSection, [string]$Timestamp, [string]$ProjectName = "", [switch]$WithLastSection)
    $segments = @($OutputDir, "build_$Timestamp")
    if (-not [string]::IsNullOrEmpty($ProjectName)) { $segments += $ProjectName }
    if ($WithLastSection -and -not [string]::IsNullOrEmpty($LastSection)) { $segments += $LastSection }
    $segments += $Framework, $Rid
    return ".\" + ($segments -join "\")
}

# --- Profile sort order (lower = earlier) ---
function Get-PublishProfileSortOrder {
    param([string]$Rid)
    if ($Rid -eq "win-x64") { return 1 }
    if ($Rid -eq "win-x86") { return 2 }
    if ($Rid -like "linux-*") { return 3 }
    if ($Rid -like "osx-*") { return 4 }
    return 99
}

# --- Strip '.Standalone' from project names for *.Standalone.csproj (output exe/folders) ---
function Get-ProjectDisplayName {
    param([string]$ProjectPath)
    if ([string]::IsNullOrWhiteSpace($ProjectPath)) { return "" }
    $name = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)
    $fileName = [System.IO.Path]::GetFileName($ProjectPath)
    if ($fileName -match '\.Standalone\.csproj$') { return $name -replace '\.Standalone$', '' }
    return $name
}
function Test-IsStandaloneProject {
    param([string]$ProjectPath)
    if ([string]::IsNullOrWhiteSpace($ProjectPath)) { return $false }
    $fileName = [System.IO.Path]::GetFileName($ProjectPath)
    return $fileName -match '\.Standalone\.csproj$'
}

# --- Read Version and AppName (Product/AssemblyName) from .csproj ---
function Get-ProjectAppInfo {
    param([string]$ProjectPath)
    $appName = [System.IO.Path]::GetFileNameWithoutExtension($ProjectPath)
    $version = "1.0.0"
    if ([string]::IsNullOrWhiteSpace($ProjectPath) -or -not (Test-Path -LiteralPath $ProjectPath -PathType Leaf)) {
        return @{ AppName = $appName; Version = $version }
    }
    $content = Get-Content -LiteralPath $ProjectPath -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return @{ AppName = $appName; Version = $version } }
    if ($content -match '<Product>([^<]*)</Product>') { $appName = $Matches[1].Trim() }
    elseif ($content -match '<AssemblyName>([^<]*)</AssemblyName>') { $appName = $Matches[1].Trim() }
    if ($content -match '<Version>([^<]*)</Version>') { $version = $Matches[1].Trim() }
    elseif ($content -match '<AssemblyVersion>([^<]*)</AssemblyVersion>') {
        $av = $Matches[1].Trim()
        $parts = $av -split '\.'
        $version = if ($parts.Length -ge 3) { "$($parts[0]).$($parts[1]).$($parts[2])" } else { $av }
    }
    if ($version -and $version -notmatch '^v') { $version = "v$version" }
    return @{ AppName = $appName; Version = $version }
}

# --- Resolve framework-dependent target from project (net472 vs net48) ---
function Get-ProjectTargetFrameworks {
    param([string]$ProjectPath)
    if ([string]::IsNullOrWhiteSpace($ProjectPath) -or -not (Test-Path -LiteralPath $ProjectPath -PathType Leaf)) {
        return @()
    }
    $content = Get-Content -LiteralPath $ProjectPath -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return @() }
    if ($content -match 'TargetFrameworks[^>]*>([^<]+)<') {
        return @($Matches[1] -split '[;,]' | ForEach-Object { $_.Trim() } | Where-Object { $_ })
    }
    if ($content -match 'TargetFramework[^s][^>]*>([^<]+)<') {
        return @($Matches[1].Trim())
    }
    return @()
}

function Get-ProjectFrameworkDependent {
    param([string]$ProjectPath)
    $tfs = Get-ProjectTargetFrameworks -ProjectPath $ProjectPath
    if ($tfs -contains 'net472') { return 'net472' }
    return $FrameworkDependent
}

# --- Data-driven profile specs: Release = framework-dependent only (compact/slim, no self-contained) ---
$ProfileSpecs = @(
    @{ Framework = $FrameworkDependent; Rid = "win-x64";    SelfContained = $false },
    @{ Framework = $FrameworkDependent; Rid = "win-x86";    SelfContained = $false },
    @{ Framework = $FrameworkVersion;   Rid = "linux-x64";  SelfContained = $false },
    @{ Framework = $FrameworkVersion;   Rid = "linux-arm64"; SelfContained = $false },
    @{ Framework = $FrameworkVersion;   Rid = "osx-x64";    SelfContained = $false; Osx = $true },
    @{ Framework = $FrameworkVersion;   Rid = "osx-arm64";  SelfContained = $false; Osx = $true }
)

# OSX bundle defaults; CFBundleDisplayName/CFBundleExecutable are overridden per-project in New-MsBuildProperties
$OsxBundleProps = [ordered]@{
    PublishTrimmed = "false"; UseAppHost = "true"; CFBundleDisplayName = ""
    CFBundleIdentifier = "com.th3w1zard1.kotormodsync"; CFBundleShortVersionString = "v0.10"
    CFBundleVersion = "v0.10.43"; LSMinimumSystemVersion = "10.13"; CFBundleIconFile = "icon53"
    CFBundleExecutable = ""; LSApplicationCategoryType = "public.app-category.utilities"
}

function New-MsBuildProperties {
    param([hashtable]$Spec, [string]$ProjectName = "", [string]$SolutionDirAbsolute = "", [string]$AppNameForBundle = "", [switch]$IsStandaloneProject)
    $fw = $Spec.Framework; $rid = $Spec.Rid; $sc = $Spec.SelfContained
    $platform = if ($rid -match "-") { ($rid -split "-")[1] } else { $rid }
    $lastSection = if ($sc) { "selfcontained" } else { $null }
    $mid = if (-not [string]::IsNullOrEmpty($ProjectName)) { "$OutputDir\build\$ProjectName\" } else { "$OutputDir\build\" }
    $buildPath = if ($lastSection) { "${mid}$lastSection\$fw\$rid\" } else { "${mid}$fw\$rid\" }
    $solutionDir = if ([string]::IsNullOrEmpty($SolutionDirAbsolute)) { $Script:SolutionDirAbsolute } else { $SolutionDirAbsolute }
    $publishDir = $solutionDir + $buildPath

    $base = [ordered]@{
        SolutionDir = $solutionDir; SelfContained = $sc.ToString().ToLower(); TargetFramework = $fw
        Platform = $platform; RuntimeIdentifier = $rid; PublishDir = $publishDir
        _TargetId = "Folder"; PublishProtocol = "FileSystem"; Configuration = "Release"
    }
    # Do not set AssemblyName here: /p:AssemblyName would apply to all projects in the graph (e.g. BioWare) and break the build. Set AssemblyName in the standalone .csproj instead.
    if ($sc) {
        $base["IncludeNativeLibrariesForSelfExtract"] = "true"
        $base["PublishSingleFile"] = "true"
        if (-not $Spec.Osx) { $base["PublishReadyToRun"] = "true" }
    } else {
        $base["PublishReadyToRun"] = "true"
    }
    if ($Spec.Osx) {
        foreach ($k in $OsxBundleProps.Keys) { $base[$k] = $OsxBundleProps[$k] }
        if (-not [string]::IsNullOrEmpty($AppNameForBundle)) {
            $base["CFBundleDisplayName"] = $AppNameForBundle
            $base["CFBundleExecutable"] = $AppNameForBundle
        }
    }
    return $base
}

function Get-PredefinedPublishProfiles {
    param([string]$ProjectName = "", [string]$SolutionDirAbsolute = "", [string]$ProjectPath = "", [string]$AppNameForBundle = "", [switch]$IsStandaloneProject)
    $fdFramework = Get-ProjectFrameworkDependent -ProjectPath $ProjectPath
    $projectTfs = Get-ProjectTargetFrameworks -ProjectPath $ProjectPath
    $profiles = $ProfileSpecs | ForEach-Object {
        $spec = $_.Clone()
        if (-not $spec.SelfContained -and $spec.Rid -match '^win-') { $spec.Framework = $fdFramework }
        $name = if ($spec.SelfContained) { "$($spec.Framework)_$($spec.Rid)_selfcontained" } else { "$($spec.Framework)_$($spec.Rid)" }
        [pscustomobject]@{
            Name = $name; BaseName = $name; TargetFramework = $spec.Framework
            RuntimeIdentifier = $spec.Rid; SelfContained = $spec.SelfContained
            MsBuildProperties = New-MsBuildProperties -Spec $spec -ProjectName $ProjectName -SolutionDirAbsolute $SolutionDirAbsolute -AppNameForBundle $AppNameForBundle -IsStandaloneProject:$IsStandaloneProject
        }
    }
    # Only include profiles for frameworks the project actually targets (skip net48/net472 FD if project is net9.0-only)
    if ($projectTfs.Count -gt 0) {
        $profiles = @($profiles | Where-Object { $projectTfs -contains $_.TargetFramework })
    }
    return $profiles
}

function Get-PublishProfileInfo {
    param([Parameter(Mandatory)] [pscustomobject]$PublishProfile)
    $name = if ($PublishProfile.BaseName) { $PublishProfile.BaseName } else { $PublishProfile.Name }
    $parts = if ($name) { $name -split "_" } else { @() }
    $lastSection = if ($parts.Length -gt 2) { $parts[2] } else { "" }
    $rid = $PublishProfile.RuntimeIdentifier
    $cpu = if ($rid -match "-") { ($rid -split "-")[1] } else { $rid }
    return @{
        Name = $name; Framework = $PublishProfile.TargetFramework; Rid = $rid; Cpu = $cpu
        LastSection = $lastSection; MsBuildProperties = $PublishProfile.MsBuildProperties
        SelfContained = [bool]$PublishProfile.SelfContained
    }
}

# --- Exhaustive error diagnostics for dotnet/MSBuild failures ---
# Call from catch block: Write-PublishErrorDiagnostics -ErrorRecord $_
# Prints full stack trace, exception chain, script location, .NET stack, and build-relevant context.
# Implicitly: without explicit per-field formatting; uses Format-List -Force to dump all properties.
function Write-PublishErrorDiagnostics {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,
        [hashtable]$Context = @{}
    )
    $e = $ErrorRecord
    $sep = "=" * 80
    $sub = "-" * 60

    Write-Host ""
    Write-Host $sep -ForegroundColor Red
    Write-Host " PUBLISH ERROR DIAGNOSTICS " -ForegroundColor Red
    Write-Host $sep -ForegroundColor Red

    # Context (project, profile, RID, etc.)
    if ($Context.Count -gt 0) {
        Write-Host "`n[Context]" -ForegroundColor Yellow
        foreach ($k in ($Context.Keys | Sort-Object)) {
            $v = $Context[$k]
            if ($null -ne $v -and $v -ne "") {
                Write-Host "  $k : $v"
            }
        }
    }

    # Primary error message
    Write-Host "`n[Error Message]" -ForegroundColor Yellow
    Write-Host "  $($e.Exception.Message)" -ForegroundColor White

    # ErrorRecord core (CategoryInfo, FullyQualifiedErrorId - useful for classifying)
    if ($e.CategoryInfo) {
        Write-Host "`n[Category]" -ForegroundColor Yellow
        Write-Host "  $($e.CategoryInfo.Category) : $($e.CategoryInfo.Reason)" -ForegroundColor Gray
    }
    if ($e.FullyQualifiedErrorId) {
        Write-Host "  FullyQualifiedErrorId : $($e.FullyQualifiedErrorId)" -ForegroundColor Gray
    }

    # InvocationInfo - where in our script did this occur
    if ($e.InvocationInfo) {
        Write-Host "`n[Script Location]" -ForegroundColor Yellow
        $inv = $e.InvocationInfo
        if ($inv.ScriptName) { Write-Host "  Script     : $($inv.ScriptName)" }
        if ($inv.ScriptLineNumber) { Write-Host "  Line       : $($inv.ScriptLineNumber)" }
        if ($inv.OffsetInLine) { Write-Host "  Column     : $($inv.OffsetInLine)" }
        if ($inv.Line) { Write-Host "  Code       : $($inv.Line.Trim())" }
        if ($inv.PositionMessage) {
            Write-Host "  Position   :" -ForegroundColor Gray
            $inv.PositionMessage -split "`n" | ForEach-Object { Write-Host "    $_" -ForegroundColor Gray }
        }
    }

    # ScriptStackTrace - our PowerShell call stack
    if ($e.ScriptStackTrace) {
        Write-Host "`n[PowerShell Call Stack]" -ForegroundColor Yellow
        foreach ($line in ($e.ScriptStackTrace -split "`n")) {
            $line = $line.Trim()
            if ($line) { Write-Host "  $line" -ForegroundColor Cyan }
        }
    }

    # Exception chain (Message, Type, .NET StackTrace - where build failures often surface)
    $depth = 0
    $ex = $e.Exception
    while ($ex) {
        Write-Host "`n[Exception #$depth] $($ex.GetType().FullName)" -ForegroundColor Yellow
        Write-Host "  Message : $($ex.Message)" -ForegroundColor White
        if ($ex.StackTrace) {
            Write-Host "  .NET StackTrace :" -ForegroundColor Gray
            foreach ($frame in ($ex.StackTrace -split "`n")) {
                $frame = $frame.Trim()
                if ($frame) {
                    $highlight = $frame -match '\.(csproj|targets|props|cs)\('
                    if ($highlight) { Write-Host "    $frame" -ForegroundColor White } else { Write-Host "    $frame" -ForegroundColor DarkGray }
                }
            }
        }
        if ($ex -is [System.IO.FileNotFoundException] -and $ex.FileName) {
            Write-Host "  FileName : $($ex.FileName)" -ForegroundColor White
        }
        if ($ex -is [System.IO.DirectoryNotFoundException] -and $ex.Message -match '[\w\\.]+') {
            Write-Host "  Path (from message) : $($Matches[0])" -ForegroundColor Gray
        }
        $ex = $ex.InnerException
        $depth++
    }

    # ErrorDetails (recommended action, if present)
    if ($e.ErrorDetails -and $e.ErrorDetails.Message) {
        Write-Host "`n[Recommended Action]" -ForegroundColor Yellow
        Write-Host "  $($e.ErrorDetails.Message)" -ForegroundColor Gray
    }

    # TargetObject (exit code, path, etc. when applicable)
    if ($null -ne $e.TargetObject -and $e.TargetObject -isnot [System.Management.Automation.ErrorRecord]) {
        $toStr = if ($e.TargetObject -is [string]) { $e.TargetObject } else { $e.TargetObject | Out-String }
        if ($toStr -match '\S') {
            Write-Host "`n[Target Object]" -ForegroundColor Yellow
            Write-Host "  $($toStr.Trim() -replace "`n", "`n  ")" -ForegroundColor Gray
        }
    }

    # Raw ErrorRecord dump (Format-List -Force - idiomatic, exhaustive)
    Write-Host "`n$sub" -ForegroundColor DarkGray
    Write-Host " [Raw ErrorRecord - Format-List * -Force]" -ForegroundColor DarkGray
    Write-Host $sub -ForegroundColor DarkGray
    $e | Format-List * -Force | Out-String | ForEach-Object { $_.Trim() } | Where-Object { $_ } | ForEach-Object {
        Write-Host $_ -ForegroundColor DarkGray
    }

    Write-Host "`n$sep" -ForegroundColor Red
    Write-Host ""
}

# --- Rotate old logs and build directories (keep N most recent) ---
function Remove-OldLogsAndBuilds {
    param([int]$Keep, [string]$RepoRootPath, [string]$OutDir)
    if ($Keep -le 0) { return }
    $logPattern = "publish_release_*.log"
    $logDir = $RepoRootPath
    $logs = @(Get-ChildItem -Path $logDir -Filter $logPattern -File -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending)
    if ($logs.Count -gt $Keep) {
        $toRemove = $logs | Select-Object -Skip $Keep
        foreach ($f in $toRemove) {
            Remove-Item -LiteralPath $f.FullName -Force -ErrorAction SilentlyContinue
            Write-Log "Rotated old log" -Level "INFO" -Variables @{ Path = $f.Name }
        }
    }
    $buildRoot = Join-Path $RepoRootPath $OutDir
    if (-not (Test-Path -LiteralPath $buildRoot -PathType Container)) { return }
    $buildDirs = @(Get-ChildItem -Path $buildRoot -Directory -Filter "build_*" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending)
    if ($buildDirs.Count -gt $Keep) {
        $toRemove = $buildDirs | Select-Object -Skip $Keep
        foreach ($d in $toRemove) {
            Remove-Item -LiteralPath $d.FullName -Recurse -Force -ErrorAction SilentlyContinue
            Write-Log "Rotated old build" -Level "INFO" -Variables @{ Path = $d.Name }
        }
    }
}

# --- Tool checks ---
function Test-RequiredTools {
    Write-Log "Checking required tools" -Level "DEBUG" -Variables @{ SevenZipPath = $SevenZipPath }
    if (-not (Test-Path $SevenZipPath)) {
        Write-Log "7-Zip not found" -Level "ERROR" -Variables @{ SevenZipPath = $SevenZipPath }; throw "7-Zip not found: $SevenZipPath"
    }
    try {
        $ver = dotnet --version
        Write-Log "Dotnet found" -Level "DEBUG" -Variables @{ Version = $ver }
    } catch {
        Write-Log ".NET SDK not found" -Level "ERROR"; throw "Dotnet CLI not found"
    }
}

# --- Publish ---
function Invoke-DotnetPublish {
    param([hashtable]$ProfileInfo, [string]$ProjectFile, [string]$ProjectName = "")
    $fw = $ProfileInfo.Framework; $rid = $ProfileInfo.Rid; $lastSection = $ProfileInfo.LastSection
    $props = $ProfileInfo.MsBuildProperties; $sc = $ProfileInfo.SelfContained
    $platform = Get-PlatformDisplayName $rid
    $logPrefix = if ($ProjectName) { "[$ProjectName] " } else { "" }

    Write-Log "${logPrefix}Building for $platform" -Level "INFO"
    Write-Log "Publish profile" -Level "DEBUG" -Variables @{ Framework = $fw; Rid = $rid; ProfileName = $ProfileInfo.Name }

    $publishArgs = @("publish", $ProjectFile, "-c", "Release", "--framework", $fw, "--no-restore")
    if ($rid) { $publishArgs += @("-r", $rid) }
    $publishArgs += if ($sc) { "--self-contained" } else { "--no-self-contained" }
    if ($props) {
        foreach ($k in $props.Keys) {
            $v = $props[$k]
            if ($null -ne $v -and $v -ne "") { $publishArgs += "/p:$k=$v" }
        }
    }
    $cmdPreview = "dotnet " + (($publishArgs | ForEach-Object { if ($_ -match '\s') { """$_""" } else { $_ } }) -join " ")
    # Restore first with explicit TargetFramework+RID+Platform to avoid RID/Platform mismatch (NETSDK1032) when project multi-targets
    $plat = if ($props -and $props.Platform) { $props.Platform } else { if ($rid -match "-") { ($rid -split "-")[1] } else { $rid } }
    $restoreArgs = @("restore", $ProjectFile, "/p:TargetFramework=$fw", "/p:Configuration=Release", "/p:RuntimeIdentifier=$rid", "/p:Platform=$plat")
    $restoreResult = & dotnet @restoreArgs 2>&1
    if ($LASTEXITCODE -ne 0) {
        $restoreText = if ($restoreResult) { ($restoreResult | Out-String).Trim() } else { "(no output)" }
        throw "dotnet restore failed.`n`n--- restore output ---`n$restoreText"
    }
    Write-Log "${logPrefix}Compiling application..." -Level "INFO"
    Write-Log "Executing" -Level "DEBUG" -Variables @{ Command = $cmdPreview }
    $dotnetOutput = & dotnet @publishArgs 2>&1

    if ($LASTEXITCODE -ne 0) {
        $outputText = if ($dotnetOutput) { ($dotnetOutput | Out-String).Trim() } else { "(no output captured)" }
        $detail = "dotnet publish exited with code $LASTEXITCODE.`n`n--- dotnet output ---`n$outputText"
        throw $detail
    }
    Write-Log "${logPrefix}Build completed for $platform" -Level "INFO"

    $withSection = -not [string]::IsNullOrEmpty($lastSection)
    $defaultFolder = Get-PublishFolderPath -Framework $fw -Rid $rid -LastSection $lastSection -Timestamp $BuildTimestamp -ProjectName $ProjectName -WithLastSection:$withSection
    $defaultFolder = $defaultFolder -replace "build_$BuildTimestamp", "build"
    $timestampedFolder = Get-PublishFolderPath -Framework $fw -Rid $rid -LastSection $lastSection -Timestamp $BuildTimestamp -ProjectName $ProjectName -WithLastSection:$withSection

    if (-not (Test-Path $defaultFolder)) {
        Write-Log "Publish folder not found" -Level "ERROR" -Variables @{ Path = $defaultFolder }; throw "Folder not found: $defaultFolder"
    }
    $parent = Split-Path $timestampedFolder -Parent
    if (-not (Test-Path $parent)) { New-Item -ItemType Directory -Path $parent -Force | Out-Null }
    if (Test-Path $timestampedFolder) { throw "Timestamped folder exists: $timestampedFolder" }
    Move-Item $defaultFolder $timestampedFolder
    Write-Log "${logPrefix}Moved output to $timestampedFolder" -Level "INFO"
}

# --- OdyTools installer staging + WiX build (uses project list from solution only; no hardcoded paths) ---
function Publish-OdyToolsInstallerPayload {
    param([string]$RepoRootPath, [string]$StagingRoot, [string[]]$SolutionProjectPaths)
    $norm = { param($p) ($p -replace '\\', '/').ToLowerInvariant() }
    $aioProject = $SolutionProjectPaths | Where-Object { (& $norm $_) -match 'odytools[\/]odytools\.csproj$' } | Select-Object -First 1
    $editorProjects = @($SolutionProjectPaths | Where-Object { ($n = & $norm $_) -match 'odytools[\/]editors[\/].*\.standalone\.csproj$' })
    $aioOut = Join-Path $StagingRoot "AIO"
    $editorsOut = Join-Path $StagingRoot "Editors"
    if (Test-Path $aioOut) { Remove-Item -Recurse -Force $aioOut }
    if (Test-Path $editorsOut) { Remove-Item -Recurse -Force $editorsOut }
    New-Item -ItemType Directory -Path $aioOut -Force | Out-Null
    New-Item -ItemType Directory -Path $editorsOut -Force | Out-Null
    if (-not $aioProject -or -not (Test-Path -LiteralPath $aioProject -PathType Leaf)) {
        Write-Log "[Installer] OdyTools.csproj not found in solution project list, skipping AIO payload." -Level "WARN"
    } else {
        Write-Log "[Installer] Publishing OdyTools AIO payload (net472/win-x64)" -Level "INFO"
        & dotnet publish $aioProject -c Release --framework net472 -r win-x64 --no-self-contained -o $aioOut
        if ($LASTEXITCODE -ne 0) { throw "Failed publishing OdyTools AIO for installer staging." }
    }
    foreach ($proj in $editorProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension(([IO.Path]::GetFileName($proj)))
        $displayName = $name -replace '\.Standalone$', ''
        Write-Log "[Installer] Publishing $displayName (net472/win-x64)" -Level "INFO"
        & dotnet publish $proj -c Release --framework net472 -r win-x64 --no-self-contained -o $editorsOut
        if ($LASTEXITCODE -ne 0) {
            Write-Log "[Installer] Failed publishing $displayName, continuing." -Level "WARN"
            continue
        }
        $candidateExe = Join-Path $editorsOut "$name.exe"
        $targetExe = Join-Path $editorsOut "$displayName.exe"
        if (Test-Path $candidateExe) { Move-Item -LiteralPath $candidateExe -Destination $targetExe -Force }
    }
}

function Build-OdyToolsInstaller {
    param([string]$RepoRootPath, [string]$InstallerProjectPath)
    $projectPath = if ([IO.Path]::IsPathRooted($InstallerProjectPath)) { $InstallerProjectPath } else { Join-Path $RepoRootPath $InstallerProjectPath }
    if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
        Write-Log "[Installer] WiX project not found, skipping installer build." -Level "WARN" -Variables @{ Project = $projectPath }
        return
    }
    Write-Log "[Installer] Building WiX installer project" -Level "INFO" -Variables @{ Project = $projectPath }
    & dotnet build $projectPath -c Release
    if ($LASTEXITCODE -ne 0) { throw "Failed building WiX installer project." }
}

# --- Archive ---
function Get-DocsToCopy {
    param([string]$AppNameForDocs)
    $name = $AppNameForDocs
    return @(
        @{ Source = "LICENSE.TXT"; Dest = "LICENSE.TXT" },
        @{ Source = "$name - Official Documentation.txt"; Dest = "$name - Official Documentation.txt" }
    )
}

function New-Archive {
    param([hashtable]$ProfileInfo, [string]$AppVersion, [string]$SevenZipPath, [string]$OutputDir, [string]$ArchiveAppName)
    $appName = $ArchiveAppName
    $fw = $ProfileInfo.Framework; $rid = $ProfileInfo.Rid; $lastSection = $ProfileInfo.LastSection
    $topLevelFolder = "$appName $AppVersion-$rid"
    $withSection = -not [string]::IsNullOrEmpty($lastSection)
    $projectName = if ($ArchiveAppName) { $ArchiveAppName } else { "" }
    $publishFolder = Get-PublishFolderPath -Framework $fw -Rid $rid -LastSection $lastSection -Timestamp $BuildTimestamp -ProjectName $projectName -WithLastSection:$withSection
    $platform = Get-PlatformDisplayName $rid
    $logPrefix = if ($appName) { "[$appName] " } else { "" }

    Write-Log "${logPrefix}Creating archive for $platform" -Level "INFO"

    if (-not (Test-Path $publishFolder)) {
        Write-Log "Publish folder not found" -Level "ERROR" -Variables @{ Path = $publishFolder }; throw "Publish folder not found"
    }

    $renamedFolder = (Split-Path $publishFolder -Parent) + "\$topLevelFolder"
    if (Test-Path $renamedFolder) { throw "Target folder exists: $renamedFolder" }
    Move-Item $publishFolder $renamedFolder
    $publishFolder = $renamedFolder

    $docsFolder = Join-Path $publishFolder "docs"
    New-Item -ItemType Directory -Path $docsFolder -Force | Out-Null
    $docsToCopy = Get-DocsToCopy -AppNameForDocs $appName
    foreach ($f in $docsToCopy) {
        if (Test-Path $f.Source) { Copy-Item $f.Source (Join-Path $docsFolder $f.Dest) -Force }
        else { Write-Log "Source not found, skipping" -Level "WARN" -Variables @{ File = $f.Source } }
    }

    $archiveFile = if ($appName) { "$OutputDir\$appName-$AppVersion-$rid.zip" } else { "$OutputDir\$rid.zip" }
    if (Test-Path $archiveFile) { Remove-Item $archiveFile -Force }
    Write-Log "${logPrefix}Compressing files..." -Level "INFO"

    & $SevenZipPath a -tzip $archiveFile "$publishFolder\*"
    if ($LASTEXITCODE -ne 0) {
        Write-Log "${logPrefix}Archive failed for $platform" -Level "ERROR" -Variables @{ ExitCode = $LASTEXITCODE }; throw "Archive failed: $LASTEXITCODE"
    }
    $sizeMB = [math]::Round((Get-Item $archiveFile).Length / 1MB, 1)
    Write-Log "${logPrefix}Archive created for $platform ($sizeMB MB)" -Level "INFO"
    Write-Log "Built files preserved in dist/build_$BuildTimestamp" -Level "INFO"
}

# --- Resolve projects to publish ---
if ($BuildOdyToolsInstallerOnly -or [string]::IsNullOrWhiteSpace($ProjectFile)) {
    if (-not $ResolvedSolutionPath) { $ResolvedSolutionPath = Resolve-SolutionPath }
    if (-not $ResolvedSolutionPath) { throw "No solution found. Specify -SolutionPath or -ProjectFile." }
    $ProjectsToPublish = @(Get-SolutionProjectPaths -SlnPath $ResolvedSolutionPath -Root $RepoRoot)
    if ($ProjectsToPublish.Count -eq 0) {
        throw "No projects found in solution (after excluding .history and vendor): $ResolvedSolutionPath"
    }
    Write-Log "Discovered $($ProjectsToPublish.Count) project(s) from solution" -Level "INFO" -Variables @{ Solution = $ResolvedSolutionPath }
} else {
    $absProject = if ([IO.Path]::IsPathRooted($ProjectFile)) { $ProjectFile } else { Join-Path $RepoRoot $ProjectFile }
    if (-not (Test-Path $absProject)) { throw "Project file not found: $absProject" }
    $ProjectsToPublish = @((Resolve-Path -LiteralPath $absProject).Path)
}

# --- Main ---
try {
    if ($BuildOdyToolsInstallerOnly) {
        Write-Log "OdyTools installer-only build (skipping project publishes)" -Level "INFO"
        Test-RequiredTools
        $stagingRoot = Join-Path $RepoRoot "dist\installer\staging"
        Publish-OdyToolsInstallerPayload -RepoRootPath $RepoRoot -StagingRoot $stagingRoot -SolutionProjectPaths $ProjectsToPublish
        Build-OdyToolsInstaller -RepoRootPath $RepoRoot -InstallerProjectPath "installer\odytools\OdyTools.Installer.wixproj"
        Write-Log "Installer build complete" -Level "INFO"
    } else {
        Write-Log "Starting release build" -Level "INFO" -Variables @{ Projects = $ProjectsToPublish.Count }
        Test-RequiredTools

        $distDir = ".\$OutputDir"
        $buildDir = ".\$OutputDir\build_$BuildTimestamp"
        foreach ($d in @($distDir, $buildDir)) {
            if (-not (Test-Path $d)) { New-Item -ItemType Directory -Path $d -Force | Out-Null }
        }

        $successCount = 0
        $failureCount = 0
        $total = 0

        foreach ($proj in $ProjectsToPublish) {
        $projectName = Get-ProjectDisplayName -ProjectPath $proj
        $appInfo = Get-ProjectAppInfo -ProjectPath $proj
        $projectAppName = if (Test-IsStandaloneProject -ProjectPath $proj) { $projectName } else { $appInfo.AppName }
        $projectVersion = $appInfo.Version

        $profiles = Get-PredefinedPublishProfiles -ProjectName $projectName -SolutionDirAbsolute $Script:SolutionDirAbsolute -ProjectPath $proj -AppNameForBundle $projectAppName -IsStandaloneProject:(Test-IsStandaloneProject -ProjectPath $proj) | ForEach-Object {
            $pi = Get-PublishProfileInfo -PublishProfile $_
            $_ | Add-Member -NotePropertyName SortOrder -NotePropertyValue (Get-PublishProfileSortOrder $pi.Rid) -Force
            $_ | Add-Member -NotePropertyName Framework -NotePropertyValue $pi.Framework -Force
            $_
        } | Sort-Object SortOrder, Framework, BaseName

        $total += $profiles.Count
        Write-Log "[$projectName] $($profiles.Count) build target(s)" -Level "INFO"

        foreach ($prof in $profiles) {
            try {
                $pi = Get-PublishProfileInfo -PublishProfile $prof
                Invoke-DotnetPublish -ProfileInfo $pi -ProjectFile $proj -ProjectName $projectName
                if ($CreateArchives) {
                    New-Archive -ProfileInfo $pi -AppVersion $projectVersion -SevenZipPath $SevenZipPath -OutputDir $OutputDir -ArchiveAppName $projectAppName
                }
                $successCount++
            } catch {
                $failureCount++
                Write-Log "[$projectName] Build/archive failed ($($pi.Name))" -Level "ERROR"
                # When failure is from dotnet publish/restore, show only the dotnet output (no PowerShell stack)
                if ($_.Exception.Message -match '--- (dotnet|restore) output ---') {
                    Write-Host ""
                    Write-Host $_.Exception.Message -ForegroundColor Red
                    Write-Host ""
                } else {
                    $ctx = @{
                        Project = $projectName
                        ProfileName = $pi.Name
                        Framework = $pi.Framework
                        RuntimeIdentifier = $pi.Rid
                        ProjectFile = $proj
                    }
                    Write-PublishErrorDiagnostics -ErrorRecord $_ -Context $ctx
                }
            }
        }
    }

        $msg = if ($failureCount -eq 0) { "All builds completed ($successCount/$total)" } else { "Completed with $failureCount failures ($successCount/$total)" }
        Write-Log $msg -Level $(if ($failureCount -eq 0) { "INFO" } else { "WARN" })

        if ($BuildOdyToolsInstaller) {
            $stagingRoot = Join-Path $RepoRoot "dist\installer\staging"
            Publish-OdyToolsInstallerPayload -RepoRootPath $RepoRoot -StagingRoot $stagingRoot -SolutionProjectPaths $ProjectsToPublish
            Build-OdyToolsInstaller -RepoRootPath $RepoRoot -InstallerProjectPath "installer\odytools\OdyTools.Installer.wixproj"
        }

        if ($KeepLogsAndBuilds -gt 0) {
            Remove-OldLogsAndBuilds -Keep $KeepLogsAndBuilds -RepoRootPath $RepoRoot -OutDir $OutputDir
        }
    }
}
catch {
    Write-Log "Build process failed" -Level "ERROR" -Variables @{ LogFile = $LogFile }
    if ($_.Exception.Message -match '--- (dotnet|restore) output ---') {
        Write-Host ""
        Write-Host $_.Exception.Message -ForegroundColor Red
        Write-Host ""
    } else {
        Write-PublishErrorDiagnostics -ErrorRecord $_ -Context @{ LogFile = $LogFile }
    }
    throw
}

Write-Host "Press any key to continue..."
