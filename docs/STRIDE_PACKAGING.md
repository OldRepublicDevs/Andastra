# Stride Game Packaging Guide

This document describes industry-standard practices for packaging and distributing Stride game engine projects, and how to use the provided packaging scripts.

## Industry Standards & Best Practices

### Build Configuration

1. **Release Mode Only**: Always build in Release configuration for distribution
   - Optimizes code and removes debug overhead
   - Reduces file size and improves performance

2. **Self-Contained Deployment**: Preferred for game distribution
   - Bundles .NET runtime with the application
   - Users don't need to install .NET separately
   - Larger file size but better user experience

3. **Platform-Specific Builds**: Build separately for each target platform
   - Windows: `win-x64`, `win-x86`, `win-arm64`
   - Linux: `linux-x64`, `linux-x86`, `linux-arm64`
   - macOS: `osx-x64`, `osx-arm64`

4. **ReadyToRun (R2R)**: Enable for faster startup times
   - Pre-compiles code to native format
   - Reduces JIT compilation overhead

### File Cleanup

According to Stride documentation, the following files should be removed from release builds:

1. **Debug Files**:
   - `*.pdb` - Debug symbol files
   - `*.xml` - XML documentation files
   - `*vshost*` - Visual Studio host process files

2. **Unnecessary Folders**:
   - Keep only: `x64`, `x86`, `Data` folders (and `arm64` for macOS)
   - Remove other intermediate/output folders

3. **Custom Configuration Files**:
   - Remove development-only configuration files not created by Stride

### Folder Structure

Industry-standard Stride game distribution structure:

```
GameName-v1.0.0-Windows-x64/
├── x64/                          # 64-bit executable and dependencies
│   ├── GameName.exe
│   ├── *.dll
│   └── ...
├── x86/                          # 32-bit executable (optional)
│   ├── GameName.exe
│   └── ...
├── Data/                         # Game assets and resources
│   ├── Assets/
│   └── Resources/
├── Documentation/                # User documentation
│   ├── README.md
│   ├── CHANGELOG.md
│   └── ...
├── LICENSE                       # License file
└── README.txt                    # Quick start guide
```

### Documentation Requirements

Include the following in every distribution:

1. **README.txt**: Quick start guide with:
   - Game name and version
   - System requirements
   - Installation instructions
   - Running instructions
   - License reference

2. **Documentation Folder**: User-facing documentation
   - User guides
   - Technical documentation
   - API references (if applicable)

3. **License File**: Open source license or EULA
   - `LICENSE`, `LICENSE.txt`, or `LICENSE.md`
   - Or `COPYING` / `COPYING.txt`

### System Requirements

Document minimum system requirements:

- **.NET Runtime**: Specify if framework-dependent
- **Graphics API**: DirectX 11, OpenGL 4.2, or Vulkan 1.0
- **Operating System**: Minimum version (Windows 7+, Linux distro, macOS version)
- **RAM**: Minimum and recommended amounts
- **Disk Space**: Installation size requirements

### Archive Formats

Standard distribution formats:

- **Windows**: ZIP (universal support)
- **Linux**: TAR.GZ (standard) or ZIP
- **macOS**: ZIP (universal) or TAR.GZ
- **Universal**: ZIP (works everywhere)

### Versioning

Follow semantic versioning: `MAJOR.MINOR.PATCH`

- **MAJOR**: Incompatible API changes
- **MINOR**: Backward-compatible functionality additions
- **PATCH**: Backward-compatible bug fixes

Include version in:
- Archive filename: `GameName-v1.0.0-Windows-x64.zip`
- README.txt
- Documentation

## Scripts Overview

### Build-StrideGame.ps1

Builds the game project for specified platforms and architectures.

**Usage:**
```powershell
.\Build-StrideGame.ps1 [-ProjectPath <path>] [-Configuration Release] `
    [-Platforms Windows,Linux,macOS] [-Architectures x64,x86] `
    [-SelfContained] [-SingleFile] [-OutputPath dist]
```

**Examples:**
```powershell
# Build for all platforms (x64 only)
.\Build-StrideGame.ps1

# Build for Windows only (x64 and x86)
.\Build-StrideGame.ps1 -Platforms Windows -Architectures x64,x86

# Build self-contained single-file executables
.\Build-StrideGame.ps1 -SingleFile -SelfContained

# Clean build (removes old artifacts first)
.\Build-StrideGame.ps1 -Clean
```

### Package-StrideGame.ps1

Cleans up build artifacts and creates distribution packages.

**Usage:**
```powershell
.\Package-StrideGame.ps1 [-BuildPath dist] [-GameName Odyssey] `
    [-Version 1.0.0] [-CreateArchive] [-ArchiveFormat zip] `
    [-IncludeDocumentation] [-IncludeLicenses]
```

**Examples:**
```powershell
# Package with default settings
.\Package-StrideGame.ps1 -GameName "Odyssey" -Version "1.0.0"

# Package without creating archives
.\Package-StrideGame.ps1 -CreateArchive $false

# Package with TAR.GZ archives (Linux/macOS)
.\Package-StrideGame.ps1 -ArchiveFormat tar.gz

# Keep debug symbols (for testing)
.\Package-StrideGame.ps1 -KeepDebugSymbols $true
```

### Distribute-StrideGame.ps1

Complete distribution pipeline: builds and packages in one command.

**Usage:**
```powershell
.\Distribute-StrideGame.ps1 [-ProjectPath <path>] [-GameName Odyssey] `
    [-Version 1.0.0] [-Platforms All] [-Architectures x64] `
    [-SelfContained] [-ArchiveFormat zip] [-Clean]
```

**Examples:**
```powershell
# Complete distribution for all platforms
.\Distribute-StrideGame.ps1 -GameName "Odyssey" -Version "1.0.0"

# Build and package for specific platforms only
.\Distribute-StrideGame.ps1 -Platforms Windows,Linux -Architectures x64

# Skip build step (use existing artifacts)
.\Distribute-StrideGame.ps1 -SkipBuild

# Skip packaging step (build only)
.\Distribute-StrideGame.ps1 -SkipPackage
```

### Add-StrideGameDocumentation.ps1

Adds documentation to existing distribution packages.

**Usage:**
```powershell
.\Add-StrideGameDocumentation.ps1 [-BuildPath dist] `
    [-DocumentationPath docs] [-IncludeReadme] [-IncludeChangelog] `
    [-CreateIndex]
```

**Examples:**
```powershell
# Add documentation with default settings
.\Add-StrideGameDocumentation.ps1

# Add documentation and create HTML index
.\Add-StrideGameDocumentation.ps1 -CreateIndex $true

# Add documentation without README
.\Add-StrideGameDocumentation.ps1 -IncludeReadme $false
```

## Complete Workflow Example

### Step 1: Build for All Platforms

```powershell
.\Build-StrideGame.ps1 -Platforms All -Architectures x64 -SelfContained
```

This creates builds in `dist/Windows-x64/`, `dist/Linux-x64/`, `dist/macOS-x64/`

### Step 2: Package Distribution

```powershell
.\Package-StrideGame.ps1 -GameName "Odyssey" -Version "1.0.0" -BuildPath "dist"
```

This:
- Cleans up debug files
- Adds documentation
- Adds license files
- Creates README.txt
- Creates ZIP archives

### Step 3: (Optional) Add Additional Documentation

```powershell
.\Add-StrideGameDocumentation.ps1 -BuildPath "dist" -CreateIndex $true
```

### Or Use the All-in-One Command

```powershell
.\Distribute-StrideGame.ps1 -GameName "Odyssey" -Version "1.0.0" `
    -Platforms All -Architectures x64 -Clean
```

## Cross-Platform Compatibility

All scripts are designed to work on Windows, Linux, and macOS:

- **Windows**: PowerShell 5.1+ or PowerShell Core 6+
- **Linux**: PowerShell Core 6+ (install via package manager)
- **macOS**: PowerShell Core 6+ (install via Homebrew: `brew install powershell`)

The scripts automatically detect the platform and use appropriate commands:
- Path separators (`\` vs `/`)
- Archive creation tools (built-in .NET vs system commands)
- File permissions handling

## Distribution Checklist

Before distributing your game, verify:

- [ ] All builds are in Release configuration
- [ ] Debug files (.pdb, .xml) are removed
- [ ] vshost files are removed
- [ ] Unnecessary folders are cleaned up
- [ ] Documentation folder is included
- [ ] License file is included
- [ ] README.txt is present with system requirements
- [ ] Version is consistent across all files
- [ ] Archives are properly named with version
- [ ] Archives can be extracted and run on target platforms

## References

- [Stride Distribution Documentation](https://doc.stride3d.net/latest/en/manual/files-and-folders/distribute-a-game.html)
- [.NET Application Publishing Overview](https://learn.microsoft.com/en-us/dotnet/core/deploying/)
- [Stride Project Structure](https://doc.stride3d.net/latest/en/manual/files-and-folders/project-structure.html)

