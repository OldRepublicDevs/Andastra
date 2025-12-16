# Project Reorganization Plan

## Current Issues

1. **Naming Inconsistency**: Projects use mixed naming (`BioWareCSharp` vs `CSharpKOTOR`, `.NET` suffix inconsistency)
2. **Poor Separation of Concerns**: Tooling projects mixed with core libraries
3. **Cleanup Artifacts**: `~dotnetcleanup-*` directories scattered throughout
4. **Unclear Dependencies**: `BioWareEngines` references `CSharpKOTOR` which doesn't exist (should be `BioWareCSharp`)
5. **Flat Structure**: All projects at same level in `src/` makes navigation difficult

## Proposed Structure

```
src/
├── Libraries/                    # Core reusable libraries (no runtime dependencies)
│   ├── CSharpKOTOR/             # File format library (renamed from BioWareCSharp)
│   │   ├── Formats/             # GFF, 2DA, TLK, NCS, NSS, MDL, etc.
│   │   ├── Installation/        # Game installation management
│   │   ├── Resource/            # Resource access and caching
│   │   └── Common/              # Shared utilities
│   │
│   └── TSLPatcher.Core/         # Patching engine (if separate from CSharpKOTOR)
│
├── Engines/                     # Runtime engine implementations
│   └── Odyssey/                 # Renamed from BioWareEngines
│       ├── Core/                # Domain logic, entities, components
│       ├── Content/             # Asset loading and conversion
│       ├── Scripting/           # NCS VM and NWScript API
│       ├── Graphics/            # Graphics abstractions
│       │   ├── Common/          # Graphics interfaces
│       │   ├── MonoGame/        # MonoGame backend
│       │   └── Stride/          # Stride backend
│       ├── Kotor/               # KOTOR-specific game rules
│       ├── Game/                # Executable launcher
│       ├── Tests/               # Engine tests
│       └── Tooling/             # Engine development tools
│
└── Tools/                       # Standalone tooling applications
    ├── HoloPatcher/             # Main patcher application
    ├── HoloPatcher.UI/          # Patcher UI (if separate)
    ├── HolocronToolset/         # Renamed from HolocronToolset.NET
    ├── KNSSComp/                # Renamed from KNSSComp.NET
    ├── KotorDiff/               # Renamed from KotorDiff.NET
    └── NCSDecomp/               # NCS decompiler
```

## Naming Conventions

### Libraries
- **CSharpKOTOR**: File format library (BioWare Aurora engine file formats)
- **TSLPatcher.Core**: Patching engine core library

### Engines
- **Odyssey**: Runtime engine (renamed from BioWareEngines for clarity)
  - Sub-projects: `Core`, `Content`, `Scripting`, `Graphics.*`, `Kotor`, `Game`, `Tests`, `Tooling`

### Tools
- Remove `.NET` suffix from all tool projects
- Use PascalCase: `HoloPatcher`, `HolocronToolset`, `KNSSComp`, `KotorDiff`, `NCSDecomp`

## Migration Steps

1. **Rename BioWareCSharp → CSharpKOTOR**
   - Update namespace from `BioWareCSharp.*` to `CSharpKOTOR.*`
   - Update all project references
   - Update solution file

2. **Reorganize Directory Structure**
   - Create `src/Libraries/` and move `CSharpKOTOR` there
   - Create `src/Engines/` and move `BioWareEngines` → `Odyssey` there
   - Create `src/Tools/` and move all tooling projects there

3. **Standardize Naming**
   - Remove `.NET` suffixes from tool projects
   - Update project files, namespaces, and references

4. **Clean Up Artifacts**
   - Remove all `~dotnetcleanup-*` directories
   - Update `.gitignore` to prevent future cleanup directories

5. **Update Solution File**
   - Reorganize solution folders to match new structure
   - Update all project paths

6. **Update Documentation**
   - Update README.md with new structure
   - Update QUICKSTART.md
   - Update any architecture documentation

## Benefits

1. **Clear Separation**: Libraries, Engines, and Tools are clearly separated
2. **Better Navigation**: Logical grouping makes finding code easier
3. **Consistent Naming**: All projects follow same naming conventions
4. **Cleaner Repository**: No cleanup artifacts cluttering directories
5. **Easier Onboarding**: New developers can understand structure immediately

## Backward Compatibility

- Git history preserved (renames tracked)
- All functionality remains identical
- Only structural changes, no code logic changes

