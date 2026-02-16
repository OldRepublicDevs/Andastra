# Rename basename.cs to basename.axaml.cs for all AXAML code-behind files in OdyTools
# Usage: Run from repo root. Uses Move-Item for renames.
# Example: .\helper_scripts\RenameAxamlCodebehind.ps1

$root = "src\Tools\OdyTools"

# Editors: basename.cs -> basename.axaml.cs (Move-Item)
$editorRenames = @(
    "Editors\OdyToolIFO",
    "Editors\OdyToolGIT",
    "Editors\OdyToolJRL",
    "Editors\OdyToolLTR",
    "Editors\OdyToolSSF",
    "Editors\OdyToolTLK",
    "Editors\OdyToolTXT",
    "Editors\OdyToolUTW",
    "Editors\OdyToolWAV",
    "Editors\OdyToolGFF"
)

# Widgets
$widgetRenames = @(
    "Widgets\ResourceList",
    "Widgets\Edit\ComboBox2DA",
    "Widgets\LocalizedStringEdit",
    "Widgets\Edit\ColorEdit",
    "Widgets\Settings\InstallationsWidget",
    "Widgets\Settings\GITSettingsWidget",
    "Widgets\Settings\ApplicationSettingsWidget",
    "Widgets\Edit\GFFFieldSpinBox",
    "Widgets\SetBindWidget",
    "Widgets\MediaPlayerWidget",
    "Widgets\LongSpinBox"
)

foreach ($base in $editorRenames + $widgetRenames) {
    $csPath = Join-Path $root "$base.cs"
    $axamlCsPath = Join-Path $root "$base.axaml.cs"
    if (Test-Path $csPath) {
        Move-Item -Path $csPath -Destination $axamlCsPath -Force
        Write-Host "Renamed: $base.cs -> $base.axaml.cs"
    }
}

# Standalone csproj: Update Compile Include from .cs to .axaml.cs (Editors renames done via Move-Item)
$csprojUpdates = @{
    "OdyToolERF.cs" = "OdyToolERF.axaml.cs"  # Already done
    "OdyToolGIT.cs" = "OdyToolGIT.axaml.cs"
    "OdyToolGFF.cs" = "OdyToolGFF.axaml.cs"
    "OdyToolJRL.cs" = "OdyToolJRL.axaml.cs"
    "OdyToolSSF.cs" = "OdyToolSSF.axaml.cs"
    "OdyToolTLK.cs" = "OdyToolTLK.axaml.cs"
    "OdyToolTXT.cs" = "OdyToolTXT.axaml.cs"
    "OdyToolUTW.cs" = "OdyToolUTW.axaml.cs"
    "OdyToolWAV.cs" = "OdyToolWAV.axaml.cs"
    "ColorEdit.cs" = "ColorEdit.axaml.cs"
    "LocalizedStringEdit.cs" = "LocalizedStringEdit.axaml.cs"
    "ComboBox2DA.cs" = "ComboBox2DA.axaml.cs"
}

$standaloneCsprojs = Get-ChildItem -Path (Join-Path $root "Editors") -Filter "*.Standalone.csproj" -Recurse
foreach ($csproj in $standaloneCsprojs) {
    $content = Get-Content $csproj.FullName -Raw
    foreach ($old in $csprojUpdates.Keys) {
        $new = $csprojUpdates[$old]
        $content = $content -replace [regex]::Escape($old), $new
    }
    Set-Content -Path $csproj.FullName -Value $content -NoNewline
}

# InsertInstanceDialog: Merge InsertInstanceDialog.cs into InsertInstanceDialog.axaml.cs
# (Manual step - copy content from .cs into .axaml.cs, then delete .cs)
