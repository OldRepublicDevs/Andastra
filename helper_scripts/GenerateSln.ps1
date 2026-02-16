# One-time script to generate Andastra.sln with all .csproj (excluding .history and vendor).
# Run from repo root. Does not modify publish.ps1.

$ErrorActionPreference = "Stop"
$Root = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $Root

function Test-PathExcluded {
    param([string]$PathOrRel)
    $norm = ($PathOrRel -replace '\\', '/').ToLowerInvariant()
    return ($norm -match '(^|/)(\.history|vendor)(/|$)')
}

$allCsproj = Get-ChildItem -Path $Root -Filter "*.csproj" -Recurse -File | Where-Object {
    $rel = $_.FullName.Substring($Root.Length).TrimStart([IO.Path]::DirectorySeparatorChar) -replace '/', '\'
    -not (Test-PathExcluded -PathOrRel $rel)
}
$rootLen = $Root.Path.TrimEnd([IO.Path]::DirectorySeparatorChar).Length
$projects = $allCsproj | ForEach-Object {
    $rel = $_.FullName.Substring($rootLen).TrimStart([IO.Path]::DirectorySeparatorChar) -replace '/', '\'
    $name = [IO.Path]::GetFileNameWithoutExtension($_.Name)
    $guid = "{" + [guid]::NewGuid().ToString().ToUpperInvariant() + "}"
    @{ Name = $name; Path = $rel; Guid = $guid }
} | Sort-Object { $_.Path }

$configs = @("Debug|Any CPU", "Debug|x64", "Debug|x86", "Release|Any CPU", "Release|x64", "Release|x86")
$sb = [System.Text.StringBuilder]::new()
[void]$sb.AppendLine("")
[void]$sb.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00")
[void]$sb.AppendLine("# Visual Studio Version 17")
[void]$sb.AppendLine("VisualStudioVersion = 17.0.31903.59")
[void]$sb.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1")
foreach ($p in $projects) {
    [void]$sb.AppendLine("Project(`"{9A19103F-16F7-4668-BE54-9A1E7A4F7556}`") = `"$($p.Name)`", `"$($p.Path)`", `"$($p.Guid)`"")
    [void]$sb.AppendLine("EndProject")
}
[void]$sb.AppendLine("Global")
[void]$sb.AppendLine("	GlobalSection(SolutionConfigurationPlatforms) = preSolution")
foreach ($c in $configs) { [void]$sb.AppendLine("		$c = $c") }
[void]$sb.AppendLine("	EndGlobalSection")
[void]$sb.AppendLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution")
foreach ($p in $projects) {
    $guid = $p.Guid.Trim('{', '}')
    foreach ($c in $configs) {
        [void]$sb.AppendLine("		{$guid}.$c.ActiveCfg = $c")
        [void]$sb.AppendLine("		{$guid}.$c.Build.0 = $c")
    }
}
[void]$sb.AppendLine("	EndGlobalSection")
[void]$sb.AppendLine("	GlobalSection(SolutionProperties) = preSolution")
[void]$sb.AppendLine("		HideSolutionNode = FALSE")
[void]$sb.AppendLine("	EndGlobalSection")
[void]$sb.AppendLine("	GlobalSection(ExtensibilityGlobals) = postSolution")
[void]$sb.AppendLine("		SolutionGuid = {02ABA7B4-132C-4F35-BBFC-1B1E935240DE}")
[void]$sb.AppendLine("	EndGlobalSection")
[void]$sb.AppendLine("EndGlobal")

$outPath = Join-Path $Root "Andastra.sln"
$sb.ToString() | Set-Content -LiteralPath $outPath -Encoding UTF8
Write-Host "Wrote $($projects.Count) projects to Andastra.sln"