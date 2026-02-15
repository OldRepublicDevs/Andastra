# Andastra publish script (unified: predefined profiles + pubxml-based).
# Publishes executable csproj(s) for multiple RIDs; optionally creates zip archives.
# Modes: -PublishProfilesDir (use project's .pubxml) or predefined profiles.
# Cross-platform: discovers 7z/7za dynamically; use -SevenZipPath to override.
# Run from repo root: .\helper_scripts\publish.ps1 -Project src/Andastra/Andastra.csproj
# HoloPatcher with pubxml: .\helper_scripts\publish.ps1 -Project src/Tools/HoloPatcher/HoloPatcher.csproj -PublishProfilesDir src/Tools/HoloPatcher/Properties/PublishProfiles -CreateArchives

param(
    [string]$Version = "v1.0.0",
    [string]$ProjectFile = "",
    [string]$SolutionPath = "Andastra.sln",
    [string]$PublishProfilesDir = "",  # When set: use .pubxml profiles from this dir (single-project mode)
    [string]$SevenZipPath = "",        # Leave empty to auto-discover 7z/7za
    [string]$OutputDir = "dist",
    [string]$TargetFramework = "net9.0",
    [switch]$CreateArchives,
    [switch]$Verbose,
    [switch]$Debug,
    [ValidateSet("SilentlyContinue", "Stop", "Continue", "Inquire", "Ignore", "Suspend")]
    [string]$ErrorAction = "Inquire"
)

# Repo root = parent of helper_scripts
$ScriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Path }
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir "..")).Path
Set-Location $RepoRoot

$BuildTimestamp = Get-Date -Format 'yyyy-MM-yy-HH-mm'
$LogFile = Join-Path $RepoRoot "publish_$BuildTimestamp.log"
$ErrorActionPreference = $ErrorAction

# Publishable projects: discovered from solution (executable csproj only; excludes tests and libraries)
function Get-SolutionProjectPaths {
    param([string]$SolutionPath)
    $slnPath = if ([IO.Path]::IsPathRooted($SolutionPath)) { $SolutionPath } else { Join-Path $RepoRoot $SolutionPath }
    if (-not (Test-Path $slnPath)) { return @() }
    $content = Get-Content $slnPath -Raw -ErrorAction SilentlyContinue
    $matches = [regex]::Matches($content, 'Project\("[^"]+"\)\s*=\s*"[^"]+",\s*"([^"]+\.csproj)"')
    $slnDir = Split-Path $slnPath -Parent
    $projects = foreach ($m in $matches) {
        $rel = $m.Groups[1].Value -replace '\\', '/'
        $full = Join-Path $slnDir $rel
        if (Test-Path $full) { $rel }
    }
    return $projects
}

function Test-IsPublishableProject {
    param([string]$CsprojPath)
    $full = if ([IO.Path]::IsPathRooted($CsprojPath)) { $CsprojPath } else { Join-Path $RepoRoot $CsprojPath }
    if (-not (Test-Path $full)) { return $false }
    $content = Get-Content $full -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return $false }
    if ($content -match 'IsTestProject\s*=\s*true') { return $false }
    if ($content -match '<OutputType>\s*(Exe|WinExe)\s*</OutputType>') { return $true }
    return $false
}

function Write-Log {
    [CmdletBinding()]
    param(
        [string]$Message,
        [string]$Level = "INFO",
        [hashtable]$Variables = @{}
    )
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] [$Level] $Message"
    if ($Variables.Count -gt 0) {
        $logMessage += " | Variables: " + ($Variables.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join ", "
    }
    switch ($Level.ToUpper()) {
        "ERROR" { Write-Host $logMessage -ForegroundColor "Red"; Add-Content -Path $LogFile -Value $logMessage }
        "WARN"  { Write-Host $logMessage -ForegroundColor "Yellow"; Add-Content -Path $LogFile -Value $logMessage }
        "INFO"  { Write-Host $logMessage -ForegroundColor "White"; Add-Content -Path $LogFile -Value $logMessage }
        "DEBUG" { $oc = $Host.UI.RawUI.ForegroundColor; $Host.UI.RawUI.ForegroundColor = "DarkGray"; Write-Debug $logMessage; $Host.UI.RawUI.ForegroundColor = $oc; Add-Content -Path $LogFile -Value $logMessage }
        "VERBOSE" { $oc = $Host.UI.RawUI.ForegroundColor; $Host.UI.RawUI.ForegroundColor = "Gray"; Write-Verbose $logMessage; $Host.UI.RawUI.ForegroundColor = $oc; Add-Content -Path $LogFile -Value $logMessage }
        default { Write-Host $logMessage -ForegroundColor "Gray"; Add-Content -Path $LogFile -Value $logMessage }
    }
}

if ($Debug) {
    $DebugPreference = "Continue"
    $VerbosePreference = "Continue"
    Write-Log "Debug logging enabled" -Level "INFO"
}
elseif ($Verbose) {
    $VerbosePreference = "Continue"
    Write-Log "Verbose logging enabled" -Level "INFO"
}

# Platform detection (works in PS 5.1 and PowerShell Core; avoid $IsWindows/$IsMacOS/$IsLinux as they are read-only in PS Core)
$script:RunningOnWindows = $env:OS -eq "Windows_NT"
$script:RunningOnMacOS = -not $script:RunningOnWindows -and ($IsMacOS -or (Test-Path "/Applications" -ErrorAction SilentlyContinue))
$script:RunningOnLinux = -not $script:RunningOnWindows -and -not $script:RunningOnMacOS

function Find-SevenZip {
    if (-not [string]::IsNullOrWhiteSpace($SevenZipPath)) {
        if (Test-Path -LiteralPath $SevenZipPath -PathType Leaf) {
            return (Resolve-Path -LiteralPath $SevenZipPath).Path
        }
        return $null
    }
    $candidates = @()
    if ($script:RunningOnWindows) {
        $pf = $env:ProgramFiles
        $pf86 = ${env:ProgramFiles(x86)}
        $localAppData = $env:LOCALAPPDATA
        $userProfile = $env:USERPROFILE
        $programData = $env:ProgramData
        $candidates = @(
            (Join-Path (Join-Path $pf "7-Zip") "7z.exe"),
            (Join-Path (Join-Path $pf86 "7-Zip") "7z.exe"),
            (Join-Path (Join-Path (Join-Path $localAppData "Programs") "7-Zip") "7z.exe"),
            (Join-Path (Join-Path $pf "7-Zip") "7za.exe"),
            (Join-Path (Join-Path $pf86 "7-Zip") "7za.exe"),
            (Join-Path (Join-Path (Join-Path (Join-Path (Join-Path $userProfile "scoop") "apps") "7zip") "current") "7z.exe"),
            (Join-Path (Join-Path (Join-Path (Join-Path (Join-Path $userProfile "scoop") "apps") "7zip") "current") "7za.exe"),
            (Join-Path (Join-Path (Join-Path $programData "chocolatey") "bin") "7z.exe"),
            (Join-Path (Join-Path (Join-Path $programData "chocolatey") "bin") "7za.exe"),
            (Join-Path (Join-Path (Join-Path (Join-Path $userProfile ".local") "share") "win-7zip") "7z.exe"),
            "C:\Program Files\7-Zip\7z.exe",
            "C:\Program Files (x86)\7-Zip\7z.exe",
            "C:\7-Zip\7z.exe"
        )
    } elseif ($script:RunningOnMacOS) {
        $candidates = @(
            "/opt/homebrew/bin/7z", "/opt/homebrew/bin/7za", "/opt/homebrew/bin/7zr",
            "/usr/local/bin/7z", "/usr/local/bin/7za", "/usr/local/bin/7zr",
            "/opt/local/bin/7z", "/opt/local/bin/7za",
            "/usr/bin/7z", "/usr/bin/7za"
        )
    } else {
        $candidates = @(
            "/usr/bin/7z", "/usr/bin/7za", "/usr/bin/7zr",
            "/usr/local/bin/7z", "/usr/local/bin/7za", "/usr/local/bin/7zr",
            "/opt/p7zip/bin/7z", "/opt/p7zip/bin/7za",
            "/snap/bin/7z"
        )
    }
    foreach ($p in $candidates) {
        if ($p -and (Test-Path -LiteralPath $p -PathType Leaf -ErrorAction SilentlyContinue)) {
            return (Resolve-Path -LiteralPath $p -ErrorAction SilentlyContinue).Path
        }
    }
    $inPath = @("7z", "7za", "7zr") | ForEach-Object {
        $cmd = Get-Command $_ -ErrorAction SilentlyContinue
        if ($cmd) { $cmd.Source }
    } | Select-Object -First 1
    if ($inPath) { return $inPath }
    return $null
}

function Test-RequiredTools {
    if ($CreateArchives) {
        $resolved = Find-SevenZip
        if (-not $resolved) {
            $hint = if ($script:RunningOnWindows) { "Install 7-Zip or add it to PATH" } else { "Install p7zip (e.g. apt install p7zip-full, brew install p7zip)" }
            Write-Log "7-Zip/p7zip not found. $hint" -Level "ERROR"
            throw "7-Zip/p7zip not found. $hint"
        }
        $script:ResolvedSevenZip = $resolved
        if ($Verbose) { Write-Log "Using archiver: $resolved" -Level "VERBOSE" }
    }
    try { dotnet --version | Out-Null }
    catch {
        Write-Log ".NET SDK not found in PATH" -Level "ERROR"
        throw "dotnet CLI not found"
    }
}

# --- Predefined profiles (used when -PublishProfilesDir is not set) ---
function Get-PredefinedPublishProfiles {
    $tf = $TargetFramework
    [pscustomobject[]]@(
        [pscustomobject]@{ Name = "${tf}_win-x64";   TargetFramework = $tf; RuntimeIdentifier = "win-x64";   SelfContained = $true; MsBuildProperties = @{}; LastSection = "" },
        [pscustomobject]@{ Name = "${tf}_win-x86";   TargetFramework = $tf; RuntimeIdentifier = "win-x86";   SelfContained = $true; MsBuildProperties = @{}; LastSection = "" },
        [pscustomobject]@{ Name = "${tf}_linux-x64";  TargetFramework = $tf; RuntimeIdentifier = "linux-x64";  SelfContained = $true; MsBuildProperties = @{}; LastSection = "" },
        [pscustomobject]@{ Name = "${tf}_linux-arm64"; TargetFramework = $tf; RuntimeIdentifier = "linux-arm64"; SelfContained = $true; MsBuildProperties = @{}; LastSection = "" },
        [pscustomobject]@{ Name = "${tf}_osx-x64";    TargetFramework = $tf; RuntimeIdentifier = "osx-x64";    SelfContained = $true; MsBuildProperties = @{}; LastSection = "" },
        [pscustomobject]@{ Name = "${tf}_osx-arm64"; TargetFramework = $tf; RuntimeIdentifier = "osx-arm64"; SelfContained = $true; MsBuildProperties = @{}; LastSection = "" }
    )
}

function Get-PublishProfileInfoFromName {
    param([string]$FileName)
    $parts = $FileName -split "_"
    $framework = $parts[0]
    $rid = $parts[1]
    $lastSection = if ($parts.Count -gt 2) { $parts[2] } else { "" }
    $cpu = if ($rid -match "-") { ($rid -split "-")[1] } else { $rid }
    return @{ Framework = $framework; Rid = $rid; Cpu = $cpu; LastSection = $lastSection; FullName = $FileName }
}

function Get-PublishProfileInfo {
    param([Parameter(Mandatory)] [pscustomobject]$Profile)
    $name = $Profile.Name
    $parts = $name -split "_"
    $lastSection = if ($parts.Length -gt 2) { $parts[2] } else { "" }
    $rid = $Profile.RuntimeIdentifier
    $cpu = if ($rid -match "-") { ($rid -split "-")[1] } else { $rid }
    return @{
        Name = $name; Framework = $Profile.TargetFramework; Rid = $rid; Cpu = $cpu; LastSection = $lastSection
        MsBuildProperties = $Profile.MsBuildProperties; SelfContained = [bool]$Profile.SelfContained; FullName = $name
    }
}

function Get-PublishProfileSortOrder {
    param([string]$Rid)
    switch ($Rid) {
        "win-x64"   { return 1 }; "win-x86"   { return 2 }
        "win7-x64"  { return 3 }; "win7-x86"  { return 4 }
        default { if ($Rid -like "linux-*") { return 5 } elseif ($Rid -like "osx-*") { return 6 } else { return 99 } }
    }
}

# --- Predefined-mode publish (dotnet publish with -r/-f) ---
function Invoke-DotnetPublishOne {
    param([hashtable]$ProfileInfo, [string]$ProjectFile, [string]$ProjectName)
    $framework = $ProfileInfo.Framework
    $rid = $ProfileInfo.Rid
    $platformName = switch ($rid) { "win-x64" { "Windows 64-bit" }; "win-x86" { "Windows 32-bit" }; "win7-x64" { "Windows 64-bit" }; "win7-x86" { "Windows 32-bit" }; "linux-x64" { "Linux 64-bit" }; "linux-arm64" { "Linux ARM64" }; "osx-x64" { "macOS Intel" }; "osx-arm64" { "macOS Apple Silicon" }; default { $rid } }
    Write-Log "[$ProjectName] Building for $platformName" -Level "INFO"
    $outDir = Join-Path $RepoRoot ([IO.Path]::Combine($OutputDir, "build_$BuildTimestamp", $ProjectName, $TargetFramework, $rid))
    $parent = Split-Path $outDir -Parent
    if (-not (Test-Path $parent)) { New-Item -ItemType Directory -Path $parent -Force | Out-Null }
    if (Test-Path $outDir) { throw "Output folder already exists: $outDir" }
    $arguments = @("publish", $ProjectFile, "-c", "Release", "--framework", $framework, "-r", $rid, "-o", $outDir)
    if ($ProfileInfo.SelfContained) { $arguments += "--self-contained" } else { $arguments += "--no-self-contained" }
    $arguments += "/p:PublishSingleFile=true", "/p:PublishReadyToRun=true", "/p:IncludeNativeLibrariesForSelfExtract=true"
    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        Write-Log "[$ProjectName] Publish failed for $platformName" -Level "ERROR" -Variables @{ ExitCode = $LASTEXITCODE }
        throw "Publish failed with exit code: $LASTEXITCODE"
    }
    Write-Log "[$ProjectName] Published to $outDir" -Level "INFO"
}

# --- Pubxml helpers: resolve PublishDir from .pubxml ---
function Get-PublishDirFromPubxml {
    param([string]$PubxmlPath, [string]$ProjectFile)
    [xml]$xml = Get-Content $PubxmlPath -ErrorAction SilentlyContinue
    $publishDir = ($xml.Project.PropertyGroup | Where-Object { $_.PublishDir } | Select-Object -First 1).PublishDir
    if (-not $publishDir) { return $null }
    $projectDir = Split-Path $ProjectFile -Parent
    $solDir = $RepoRoot.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
    $publishDir = $publishDir -replace '\$\(SolutionDir\)', $solDir -replace '\\', [IO.Path]::DirectorySeparatorChar -replace '/', [IO.Path]::DirectorySeparatorChar
    $publishDir = $publishDir.TrimEnd([IO.Path]::DirectorySeparatorChar)
    if (-not [IO.Path]::IsPathRooted($publishDir)) {
        $publishDir = Join-Path $projectDir $publishDir
    }
    return $publishDir
}

# --- Pubxml-mode publish (dotnet publish /p:PublishProfile=) ---
function Invoke-DotnetPublishPubxml {
    param(
        [hashtable]$ProfileInfo,
        [string]$ProjectFile,
        [string]$ProjectName,
        [string]$ProfilesDir
    )
    $framework = $ProfileInfo.Framework
    $rid = $ProfileInfo.Rid
    $lastSection = $ProfileInfo.LastSection
    $fileName = $ProfileInfo.FullName
    $platformName = switch ($rid) {
        "win7-x64" { "Windows 64-bit" }; "win7-x86" { "Windows 32-bit" }
        "linux-x64" { "Linux 64-bit" }; "linux-arm64" { "Linux ARM64" }
        "osx-x64" { "macOS Intel" }; "osx-arm64" { "macOS Apple Silicon" }
        default { $rid }
    }
    Write-Log "[$ProjectName] Building for $platformName (profile: $fileName)" -Level "INFO"
    $publishCmd = "dotnet publish `"$ProjectFile`" -c Release --framework $framework /p:PublishProfile=$fileName.pubxml"
    Invoke-Expression $publishCmd
    if ($LASTEXITCODE -ne 0) {
        Write-Log "[$ProjectName] Publish failed for $platformName" -Level "ERROR" -Variables @{ ExitCode = $LASTEXITCODE }
        throw "Publish failed with exit code: $LASTEXITCODE"
    }
    $pubxmlPath = Join-Path $ProfilesDir "$fileName.pubxml"
    $defaultPublishFolder = Get-PublishDirFromPubxml -PubxmlPath $pubxmlPath -ProjectFile $ProjectFile
    if (-not $defaultPublishFolder) {
        $projectDir = Split-Path $ProjectFile -Parent
        $base = if ($framework -eq "net48") { Join-Path $RepoRoot $OutputDir } else { Join-Path $projectDir $OutputDir }
        $base = Join-Path $base "build"
        $defaultPublishFolder = if ([string]::IsNullOrEmpty($lastSection)) { Join-Path $base ([IO.Path]::Combine($framework, $rid)) } else { Join-Path $base ([IO.Path]::Combine($lastSection, $framework, $rid)) }
    }
    $timestampedFolder = Join-Path $RepoRoot ([IO.Path]::Combine($OutputDir, "build_$BuildTimestamp"))
    $timestampedFolder = if ([string]::IsNullOrEmpty($lastSection)) { Join-Path $timestampedFolder ([IO.Path]::Combine($framework, $rid)) } else { Join-Path $timestampedFolder ([IO.Path]::Combine($lastSection, $framework, $rid)) }
    if (-not (Test-Path $defaultPublishFolder)) {
        Write-Log "[$ProjectName] Publish folder not found: $defaultPublishFolder" -Level "ERROR"
        throw "Publish folder not found: $defaultPublishFolder"
    }
    $parent = Split-Path $timestampedFolder -Parent
    if (-not (Test-Path $parent)) { New-Item -ItemType Directory -Path $parent -Force | Out-Null }
    if (Test-Path $timestampedFolder) { throw "Output folder already exists: $timestampedFolder" }
    Move-Item $defaultPublishFolder $timestampedFolder -Force
    Write-Log "[$ProjectName] Published to $timestampedFolder" -Level "INFO"
}

# --- Archive creation (shared) ---
function New-ArchiveOne {
    param(
        [hashtable]$ProfileInfo,
        [string]$Version,
        [string]$ProjectName,
        [switch]$UsePubxmlPaths
    )
    $rid = $ProfileInfo.Rid
    $framework = $ProfileInfo.Framework
    $lastSection = $ProfileInfo.LastSection
    if ($UsePubxmlPaths) {
        $publishFolder = Join-Path $RepoRoot ([IO.Path]::Combine($OutputDir, "build_$BuildTimestamp"))
        if (-not [string]::IsNullOrEmpty($lastSection)) {
            $publishFolder = Join-Path $publishFolder ([IO.Path]::Combine($lastSection, $framework, $rid))
        } else {
            $publishFolder = Join-Path $publishFolder ([IO.Path]::Combine($framework, $rid))
        }
    } else {
        $publishFolder = Join-Path $RepoRoot ([IO.Path]::Combine($OutputDir, "build_$BuildTimestamp", $ProjectName, $TargetFramework, $rid))
    }
    if (-not (Test-Path $publishFolder)) {
        Write-Log "[$ProjectName] Publish folder not found: $publishFolder" -Level "ERROR"
        throw "Publish folder not found: $publishFolder"
    }
    $topLevelFolder = "$ProjectName $Version-$rid"
    $parentDir = Split-Path $publishFolder -Parent
    $renamedFolder = Join-Path $parentDir $topLevelFolder
    if (Test-Path $renamedFolder) { throw "Target folder already exists: $renamedFolder" }
    Copy-Item -Path $publishFolder -Destination $renamedFolder -Recurse -Force
    $docsFolder = Join-Path $renamedFolder "docs"
    New-Item -ItemType Directory -Path $docsFolder -Force | Out-Null
    $docFiles = @("LICENSE", "LICENSE.txt", "LICENSE.TXT", "README.md")
    foreach ($f in $docFiles) {
        $src = Join-Path $RepoRoot $f
        if (Test-Path $src) { Copy-Item $src $docsFolder -Force }
    }
    $archiveFile = Join-Path $RepoRoot ([IO.Path]::Combine($OutputDir, "${ProjectName}-${Version}-${rid}.zip"))
    if (Test-Path $archiveFile) { Remove-Item $archiveFile -Force }
    $archiveSource = Join-Path $renamedFolder "*"
    & $script:ResolvedSevenZip "a", "-tzip", $archiveFile, $archiveSource
    if ($LASTEXITCODE -ne 0) { throw "Archive creation failed: $archiveFile" }
    Remove-Item $renamedFolder -Recurse -Force
    $sizeMB = [math]::Round((Get-Item $archiveFile).Length / 1MB, 1)
    Write-Log "[$ProjectName] Archive: $archiveFile ($sizeMB MB)" -Level "INFO"
}

# --- Resolve projects and mode ---
$UsePubxmlMode = -not [string]::IsNullOrWhiteSpace($PublishProfilesDir)
if ($UsePubxmlMode) {
    if ([string]::IsNullOrWhiteSpace($ProjectFile)) {
        Write-Log "-PublishProfilesDir requires -ProjectFile" -Level "ERROR"
        throw "-PublishProfilesDir requires -ProjectFile"
    }
    $absProject = if ([System.IO.Path]::IsPathRooted($ProjectFile)) { $ProjectFile } else { Join-Path $RepoRoot $ProjectFile }
    $absProfilesDir = if ([System.IO.Path]::IsPathRooted($PublishProfilesDir)) { $PublishProfilesDir } else { Join-Path $RepoRoot $PublishProfilesDir }
    if (-not (Test-Path $absProject)) {
        Write-Log "Project not found: $absProject" -Level "ERROR"
        throw "Project not found: $ProjectFile"
    }
    if (-not (Test-Path $absProfilesDir)) {
        Write-Log "PublishProfilesDir not found: $absProfilesDir" -Level "ERROR"
        throw "PublishProfilesDir not found: $PublishProfilesDir"
    }
    $ProjectsToPublish = @($absProject)
    $profilesDir = $absProfilesDir
} else {
    if ([string]::IsNullOrWhiteSpace($ProjectFile)) {
        $slnPath = if ([IO.Path]::IsPathRooted($SolutionPath)) { $SolutionPath } else { Join-Path $RepoRoot $SolutionPath }
        $candidatePaths = Get-SolutionProjectPaths -SolutionPath $slnPath
        $ProjectsToPublish = @($candidatePaths | Where-Object {
            $p = $_
            (Test-Path (Join-Path $RepoRoot $p)) -and (Test-IsPublishableProject -CsprojPath (Join-Path $RepoRoot $p))
        } | ForEach-Object { Join-Path $RepoRoot $_ })
        if ($ProjectsToPublish.Count -eq 0) {
            Write-Log "No publishable projects found in solution (executable csproj only, excludes tests)" -Level "ERROR"
            throw "No projects to publish"
        }
        Write-Log "Publishing $($ProjectsToPublish.Count) executable projects from solution" -Level "INFO"
    } else {
        $abs = if ([System.IO.Path]::IsPathRooted($ProjectFile)) { $ProjectFile } else { Join-Path $RepoRoot $ProjectFile }
        if (-not (Test-Path $abs)) {
            Write-Log "Project not found: $abs" -Level "ERROR"
            throw "Project not found: $ProjectFile"
        }
        $ProjectsToPublish = @($abs)
    }
}

try {
    Write-Log "Andastra publish started (Version=$Version, OutputDir=$OutputDir, CreateArchives=$CreateArchives, PubxmlMode=$UsePubxmlMode)" -Level "INFO"
    Test-RequiredTools
    $distDir = Join-Path $RepoRoot $OutputDir
    $buildDir = Join-Path $distDir "build_$BuildTimestamp"
    if (-not (Test-Path $distDir)) { New-Item -ItemType Directory -Path $distDir -Force | Out-Null }
    if (-not (Test-Path $buildDir)) { New-Item -ItemType Directory -Path $buildDir -Force | Out-Null }

    $successCount = 0
    $failureCount = 0

    if ($UsePubxmlMode) {
        if (-not $CreateArchives) {
            Write-Log "Pubxml mode typically creates archives; -CreateArchives will be required for archiving" -Level "INFO"
        }
        $publishProfiles = Get-ChildItem $profilesDir -Filter "*.pubxml"
        $sortedProfiles = $publishProfiles | ForEach-Object {
            $info = Get-PublishProfileInfoFromName -FileName $_.BaseName
            $_ | Add-Member -NotePropertyName SortOrder -NotePropertyValue (Get-PublishProfileSortOrder -Rid $info.Rid) -Force
            $_ | Add-Member -NotePropertyName ProfileInfo -NotePropertyValue $info -Force
            $_
        } | Sort-Object -Property SortOrder, { $_.ProfileInfo.Framework }, FullName
        $total = $ProjectsToPublish.Count * $sortedProfiles.Count
        foreach ($proj in $ProjectsToPublish) {
            $projectName = [System.IO.Path]::GetFileNameWithoutExtension($proj)
            foreach ($pf in $sortedProfiles) {
                try {
                    $info = $pf.ProfileInfo
                    Invoke-DotnetPublishPubxml -ProfileInfo $info -ProjectFile $proj -ProjectName $projectName -ProfilesDir $profilesDir
                    if ($CreateArchives) { New-ArchiveOne -ProfileInfo $info -Version $Version -ProjectName $projectName -UsePubxmlPaths }
                    $successCount++
                } catch {
                    $failureCount++
                    Write-Log "[$projectName] $($info.FullName): $($_.Exception.Message)" -Level "ERROR"
                }
            }
        }
    } else {
        $profiles = Get-PredefinedPublishProfiles
        $sortedProfiles = $profiles | ForEach-Object {
            $info = Get-PublishProfileInfo -Profile $_
            $_ | Add-Member -NotePropertyName SortOrder -NotePropertyValue (Get-PublishProfileSortOrder -Rid $info.Rid) -Force
            $_
        } | Sort-Object -Property SortOrder, Name
        $total = $ProjectsToPublish.Count * $sortedProfiles.Count
        foreach ($proj in $ProjectsToPublish) {
            $projectName = [System.IO.Path]::GetFileNameWithoutExtension($proj)
            foreach ($profile in $sortedProfiles) {
                try {
                    $info = Get-PublishProfileInfo -Profile $profile
                    Invoke-DotnetPublishOne -ProfileInfo $info -ProjectFile $proj -ProjectName $projectName
                    if ($CreateArchives) { New-ArchiveOne -ProfileInfo $info -Version $Version -ProjectName $projectName }
                    $successCount++
                } catch {
                    $failureCount++
                    Write-Log "[$projectName] $($profile.Name): $($_.Exception.Message)" -Level "ERROR"
                }
            }
        }
    }

    Write-Log "Publish finished: $successCount/$total succeeded, $failureCount failed. Log: $LogFile" -Level $(if ($failureCount -eq 0) { "INFO" } else { "WARN" })
}
catch {
    Write-Log "Publish failed: $($_.Exception.Message)" -Level "ERROR" -Variables @{ LogFile = $LogFile }
    throw
}
