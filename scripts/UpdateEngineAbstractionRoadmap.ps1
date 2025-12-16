# PowerShell script to update engine abstraction roadmap with complete file list
# This script generates a comprehensive list of all CSharpKOTOR files and their migration status

$repoRoot = Split-Path -Parent $PSScriptRoot
$roadmapPath = Join-Path $repoRoot "docs\odyssey_runtime_ghidra_refactoring_roadmap.md"
$filesListPath = Join-Path $repoRoot "temp_csharpkotor_files.txt"

Write-Host "Reading file list from $filesListPath"
$allFiles = Get-Content $filesListPath | Where-Object { $_ -match 'src\\CSharpKOTOR' }

Write-Host "Found $($allFiles.Count) files to process"

# Read current roadmap
$roadmapContent = Get-Content $roadmapPath -Raw

# Find the Engine Abstraction Refactoring section
$sectionStart = $roadmapContent.IndexOf("## Engine Abstraction Refactoring")
if ($sectionStart -eq -1) {
    Write-Error "Could not find '## Engine Abstraction Refactoring' section in roadmap"
    exit 1
}

# Find the end of the file or next major section
$sectionEnd = $roadmapContent.IndexOf("`n## ", $sectionStart + 1)
if ($sectionEnd -eq -1) {
    $sectionEnd = $roadmapContent.Length
}

# Extract existing content before the section
$beforeSection = $roadmapContent.Substring(0, $sectionStart)

# Generate new section content
$newSection = @"
## Engine Abstraction Refactoring

**Status**: IN PROGRESS  
**Started**: 2025-01-15  
**Current Phase**: Identifying and moving KOTOR-specific code from AuroraEngine.Common to Odyssey.Engines.Odyssey  
**Goal**: Move all KOTOR/Odyssey-specific code from AuroraEngine.Common to Odyssey.Engines.Odyssey following xoreos pattern

### Strategy

1. Identify KOTOR-specific code in AuroraEngine.Common
2. Move to Odyssey.Engines.Odyssey (shared KOTOR code, like xoreos's kotorbase)
3. Create engine-specific projects following xoreos pattern
4. Maximize code in base classes, minimize duplication
5. Ensure 1:1 parity with original KOTOR 2 engine (Ghidra verification)
6. Maintain compatibility with patcher tools (HoloPatcher.NET, HolocronToolset, NCSDecomp, KotorDiff)

### Architecture Pattern (Following xoreos)

- **AuroraEngine.Common** (src/CSharpKOTOR/): File format parsers (engine-agnostic), installation detection (currently KOTOR-specific but structure allows expansion)
- **Odyssey.Engines.Common**: Base engine interfaces and classes
- **Odyssey.Engines.Odyssey**: KOTOR 1/2 shared runtime code (like xoreos's kotorbase)
- **Odyssey.Engines.Aurora** (future): NWN/NWN2 shared code
- **Odyssey.Engines.Eclipse** (future): Dragon Age/Mass Effect shared code

### Files to Move from AuroraEngine.Common

**Total Files**: $($allFiles.Count)

#### Common/ (KOTOR-Specific)

- [x] Common\Game.cs - Game enum (K1, K2, etc.) and extensions
  - Status: Kept in AuroraEngine.Common for patcher tools compatibility (documented as KOTOR-specific)
  - Note: Used by HoloPatcher.NET, HolocronToolset, NCSDecomp, KotorDiff

- [ ] Common\Module.cs - KModuleType enum and Module class (1963 lines)
  - Target: Odyssey.Engines.Odyssey.Module\Module.cs
  - Status: [ ] Identify all usages, [ ] Move, [ ] Update references
  - Note: Used by patcher tools - may need abstraction layer

- [ ] Common\ModuleDataLoader.cs - KOTOR-specific module data loading
  - Target: Odyssey.Engines.Odyssey.Module\ModuleDataLoader.cs
  - Status: [ ] Review if KOTOR-specific, [ ] Move if needed

#### Resource/Generics/ (KOTOR/Odyssey GFF Templates)

**Entity Templates (KOTOR-specific - Move to Odyssey.Engines.Odyssey.Templates\):**

"@

# Add file list
foreach ($file in $allFiles) {
    $relativePath = $file.Replace("src\CSharpKOTOR\", "")
    $fileName = Split-Path -Leaf $relativePath
    $directory = Split-Path -Parent $relativePath
    
    # Determine target based on file type
    $target = ""
    $status = "[ ] Review, [ ] Move if needed"
    
    if ($relativePath -match "Resource\\Generics\\(UTC|UTD|UTE|UTI|UTM|UTP|UTS|UTT|UTW)") {
        $templateName = $matches[1]
        $target = "Odyssey.Engines.Odyssey.Templates\$fileName"
        $status = "[ ] Move, [ ] Update references"
    }
    elseif ($relativePath -match "Resource\\Generics\\(IFO|ARE|GIT|JRL|PTH)") {
        $target = "Odyssey.Engines.Odyssey.Module\$fileName (if KOTOR-specific)"
        $status = "[ ] Review, [ ] Move if needed"
    }
    elseif ($relativePath -match "Resource\\Generics\\DLG") {
        $target = "Odyssey.Engines.Odyssey.Dialogue\$fileName (if KOTOR-specific)"
        $status = "[ ] Review, [ ] Move if needed"
    }
    elseif ($relativePath -match "Resource\\Generics\\GUI") {
        $target = "Odyssey.Engines.Odyssey.GUI\$fileName (if KOTOR-specific)"
        $status = "[ ] Review, [ ] Move if needed"
    }
    elseif ($relativePath -match "Common\\(Module|ModuleDataLoader)") {
        $target = "Odyssey.Engines.Odyssey.Module\$fileName"
        $status = "[ ] Move, [ ] Update references"
    }
    else {
        $target = "Review if KOTOR-specific"
        $status = "[ ] Review"
    }
    
    $newSection += "`n- [ ] $relativePath"
    if ($target) {
        $newSection += "`n  - Target: $target"
    }
    $newSection += "`n  - Status: $status"
}

$newSection += @"


### Migration Progress

- [x] Create Odyssey.Engines.Common with base interfaces and classes
- [x] Create Odyssey.Engines.Odyssey project structure
- [x] Create Odyssey.Engines.Aurora placeholder
- [x] Create Odyssey.Engines.Eclipse placeholder
- [x] Move EngineApi classes to Odyssey.Engines.Odyssey
- [x] Move ModuleLoader to Odyssey.Engines.Odyssey
- [ ] Move Module class (deferred - used by patcher tools)
- [ ] Move GFF templates to Odyssey.Engines.Odyssey.Templates
- [ ] Update all references
- [ ] Verify compilation
- [ ] Update patcher tools to use new structure

### Notes

- Game enum kept in AuroraEngine.Common for patcher tools compatibility
- Module class migration deferred due to patcher tools dependencies
- Follow xoreos pattern: kotorbase for shared KOTOR code, engine-specific projects for game-specific code
- Maximize code in base classes, minimize duplication
- Ensure C# 7.3 compatibility
- Maintain 1:1 parity with original KOTOR 2 engine (Ghidra verification)

"@

# Write updated roadmap
$afterSection = $roadmapContent.Substring($sectionEnd)
$updatedContent = $beforeSection + $newSection + $afterSection

Set-Content -Path $roadmapPath -Value $updatedContent -Encoding UTF8

Write-Host "Roadmap updated successfully!"
Write-Host "Processed $($allFiles.Count) files"

