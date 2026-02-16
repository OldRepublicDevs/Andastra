# OdyTools WiX Installer

This folder contains the optional WiX v5 setup installer for OdyTools.

## Staging Layout

Before building `OdyTools.Installer.wixproj`, stage files into:

- `dist/installer/staging/AIO/OdyTools.exe`
- `dist/installer/staging/Editors/*.exe`

The initial implementation keeps installer authoring explicit so feature selection can map 1:1 to standalone editor executables.

## Build

From repository root:

- `dotnet build "installer/odytools/OdyTools.Installer.wixproj" -c Release`

To stage AIO + editors and build the installer in one step:

- `.\helper_scripts\publish.ps1 -BuildOdyToolsInstaller`

## Validation

See [docs/ODYTOOLS_VALIDATION.md](../../docs/ODYTOOLS_VALIDATION.md) for manual and automated validation steps covering install/modify/repair/uninstall, file associations, context menus, shell entry points, diff workflow, and NetSparkle updates.

## Notes

- This installer is optional and complements portable releases.
- File associations and context-menu registry authoring is intentionally added in separate installer fragments to keep rollout controlled.
