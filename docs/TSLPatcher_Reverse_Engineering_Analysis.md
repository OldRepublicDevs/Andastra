# TSLPatcher Reverse Engineering Analysis

## Executive Summary

TSLPatcher.exe is a Delphi-based application that serves as a comprehensive file patcher for KotOR/TSL games. It supports patching multiple file formats: 2DA, TLK, GFF, NSS, SSF, ERF, and RIM files.

## Application Overview

**Purpose**: Game Data Patcher for KotOR/TSL
**Language**: Delphi (Object Pascal)
**Architecture**: Windows GUI Application
**Entry Point**: Standard Delphi application entry point (likely around 0x00401000+)

## Key Functionality Identified

### 1. File Format Support

- **2DA Files**: Tabular data files with row/column modifications
- **TLK Files**: Talk/dialogue files with string appending and modification
- **GFF Files**: Generic File Format (BioWare) with field-level modifications
- **NSS Files**: NWScript source files (compiled to NCS)
- **SSF Files**: Soundset files
- **ERF Files**: Encapsulated Resource Format
- **RIM Files**: Resource Information Manager files

### 2. Core Features

- Configuration file parsing (from `tslpatchdata` folder)
- File blueprint system (with fallback to manual source)
- Backup creation before patching
- Progress logging (4 levels: 0=None, 1=Errors, 2=Errors+Warnings, 3=Standard, 4=Debug)
- Installation path selection
- Script compilation integration (nwnsscomp.exe)
- Dialog TLK appending
- 2DA file row/column modifications
- GFF file field modifications
- NCS file integer hacks
- Soundset file installation

### 3. UI Components

- Main form with configuration summary
- Game folder selection dialog
- Progress log (RichEdit control)
- Settings dialog
- Install/Start patching button
- Configuration display

### 4. Error Handling

- Error counting and reporting
- Warning counting and reporting
- Detailed error messages with file paths
- Unhandled exception handling

## Imported Libraries

### Windows API

- **USER32.DLL**: Window management, dialogs, controls
- **KERNEL32.DLL**: File operations, process management
- **GDI32.DLL**: Graphics operations
- **COMCTL32.DLL**: Common controls (ImageList, etc.)
- **COMDLG32.DLL**: Common dialogs (GetOpenFileName)
- **SHELL32.DLL**: Shell operations (SHBrowseForFolder)
- **OLEAUT32.DLL**: OLE automation (Variant, SafeArray)
- **WINSPOOL.DRV**: Printer operations
- **VERSION.DLL**: Version information

## String Analysis

### Key Strings Identified

- "Game Data Patcher for KotOR/TSL"
- "2DA/TLK/GFF/NSS/SSF/ERF/RIM file patcher for KotOR/TSL."
- "tslpatchdata" folder references
- File operation messages (patch, install, backup)
- Error and warning messages
- Progress log messages

### File Operation Patterns

- File existence checking
- Path resolution
- Backup creation
- File modification/overwriting
- Script compilation
- Blueprint file loading

## Memory Structure

### Memory Blocks

- **Headers**: 0x00400000-0x004003FF
- **CODE**: 0x00401000-0x0048B3FF (566,272 bytes)
- **DATA**: 0x0048C000-0x0048D5FF (5,632 bytes)
- **BSS**: 0x0048E000-0x0048EC40 (3,137 bytes)
- **.idata**: 0x0048F000-0x004915FF (9,728 bytes)
- **.tls**: 0x00492000-0x0049200F (16 bytes)
- **.rdata**: 0x00493000-0x004931FF (512 bytes)
- **.reloc**: 0x00494000-0x0049D5FF (38,400 bytes)
- **.rsrc**: 0x0049E000-0x004AD7FF (63,488 bytes)

## Reverse Engineering Status

### Completed

- ✅ Application purpose and scope identified
- ✅ File format support identified
- ✅ Core features catalogued
- ✅ UI components identified
- ✅ Error handling patterns identified
- ✅ Imported libraries catalogued
- ✅ String resources analyzed

### In Progress

- 🔄 Function identification and decompilation
- 🔄 File format parser implementations
- 🔄 Patching logic algorithms
- 🔄 Configuration file format
- 🔄 UI component implementations

### Remaining

- ⏳ Complete function-by-function reverse engineering
- ⏳ Detailed algorithm analysis
- ⏳ Data structure definitions
- ⏳ Complete UI layout reconstruction
- ⏳ Configuration file format specification
- ⏳ Blueprint file format specification

## Next Steps for Full 1:1 Parity

1. **Function Identification**
   - Manually identify all functions in CODE section
   - Map function addresses to functionality
   - Document function signatures and calling conventions

2. **Algorithm Analysis**
   - Decompile each patching algorithm
   - Document 2DA modification logic
   - Document TLK appending logic
   - Document GFF modification logic
   - Document script compilation integration

3. **Data Structure Analysis**
   - Identify all data structures
   - Document configuration file format
   - Document blueprint file format
   - Document internal state management

4. **UI Reconstruction**
   - Identify all dialog resources
   - Document control layouts
   - Document event handlers
   - Document message processing

5. **Testing and Verification**
   - Create test cases for each file format
   - Verify patching behavior matches original
   - Test error handling paths
   - Verify UI behavior

## Implementation Strategy

The C# implementation should:

1. Use .NET Framework 4.x (C# 7.3) for compatibility
2. Implement all file format parsers
3. Replicate all patching algorithms
4. Match UI behavior exactly
5. Preserve all error messages and logging
6. Support all configuration options
