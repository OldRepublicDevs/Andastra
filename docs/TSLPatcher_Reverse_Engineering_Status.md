# TSLPatcher Reverse Engineering Status

## Current Status: IN PROGRESS

This document tracks the progress of reverse engineering TSLPatcher.exe to achieve 1:1 parity in Delphi source code.

## Completed Components

### 1. Analysis & Documentation ✅

- ✅ Application purpose and scope identified
- ✅ File format support catalogued (2DA, TLK, GFF, NSS, SSF, ERF, RIM)
- ✅ Core features identified from string analysis
- ✅ UI components identified
- ✅ Error handling patterns documented
- ✅ Imported libraries catalogued
- ✅ Memory structure mapped

### 2. Delphi Source Structure ✅

- ✅ Main unit structure (`TSLPatcher.pas`)
- ✅ Core classes defined:
  - `TMainForm` - Main application form
  - `TTSLPatcherConfig` - Configuration manager
  - `TTwoDAPatcher` - 2DA file patcher
  - `TTLKPatcher` - TLK file patcher
  - `TGFFPatcher` - GFF file patcher
  - `TNSSPatcher` - NSS/NCS patcher
  - `TSSFPatcher` - SSF file patcher
  - `TERFPatcher` - ERF file patcher
  - `TRIMPatcher` - RIM file patcher
  - `TBackupManager` - Backup management

### 3. 2DA Patcher Implementation ✅

- ✅ Basic structure and class definitions
- ✅ File loading/saving framework
- ✅ Row and column management
- ✅ Modification application logic
- ✅ Exclusive row checking framework

## In Progress Components

### 1. Function Identification 🔄

- 🔄 Manual function identification in Ghidra
- 🔄 Entry point location
- 🔄 Main initialization sequence
- 🔄 Event handlers

### 2. File Format Parsers 🔄

- 🔄 Complete 2DA parser implementation
- 🔄 TLK parser (dialog appending, entry modification)
- 🔄 GFF parser (field path matching, value modification)
- 🔄 NSS compiler integration
- 🔄 NCS integer hack implementation
- 🔄 SSF parser
- 🔄 ERF parser
- 🔄 RIM parser

### 3. Configuration System 🔄

- 🔄 `install.ini` file format specification
- 🔄 Blueprint file format
- 🔄 Manual source format
- 🔄 Configuration loading logic

### 4. UI Components 🔄

- 🔄 Form layout reconstruction
- 🔄 Control definitions
- 🔄 Event handler implementations
- 🔄 RichEdit progress log integration

## Remaining Work for 1:1 Parity

### Critical Missing Components

1. **Complete Function Reverse Engineering**
   - Need to identify and decompile ALL functions in the CODE section
   - Map function addresses to functionality
   - Document function signatures and calling conventions
   - Identify all algorithms and data structures

2. **File Format Parser Details**
   - **2DA**: Complete tab-separated parsing, label column handling, memory token system
   - **TLK**: Binary format parsing, entry appending, string modification
   - **GFF**: Complete GFF structure parsing, field path resolution, type handling
   - **NSS**: Script compilation parameter passing, error handling
   - **NCS**: Integer hack location and application
   - **SSF**: Soundset file format
   - **ERF**: ERF archive format
   - **RIM**: RIM archive format

3. **Configuration File Format**
   - Complete `install.ini` structure
   - Section definitions
   - Key-value mappings
   - Blueprint file format specification
   - Manual source format specification

4. **UI Implementation**
   - Complete form definitions with all controls
   - Dialog resource reconstruction
   - Event handler implementations
   - RichEdit formatting and line insertion
   - Menu system
   - Settings dialog

5. **Backup System**
   - Backup file naming convention
   - Backup location
   - Restore functionality

6. **Error Handling**
   - Complete error message catalog
   - Error recovery mechanisms
   - Warning handling
   - Progress reporting

7. **Script Compilation Integration**
   - nwnsscomp.exe parameter passing
   - Compilation error parsing
   - Output file handling

8. **File Installation**
   - Override folder detection
   - File copying logic
   - Conflict detection
   - BIF file handling (skip logic)

## Next Steps

### Immediate Priority

1. **Function Identification**
   - Use Ghidra to manually identify functions at key addresses
   - Start with entry point and main initialization
   - Map string references to functions
   - Decompile critical functions

2. **Configuration Format Reverse Engineering**
   - Analyze `install.ini` structure from string references
   - Document all configuration options
   - Implement complete configuration loader

3. **2DA Parser Completion**
   - Complete file format parsing
   - Implement memory token system (2DAMEMORY)
   - Complete exclusive row checking
   - Handle all edge cases from error messages

4. **TLK Parser Implementation**
   - Reverse engineer TLK binary format
   - Implement dialog appending
   - Implement entry modification

5. **GFF Parser Implementation**
   - Use existing GFF knowledge from codebase
   - Implement field path matching
   - Implement value modification

### Medium Priority

6. **UI Component Completion**
   - Reconstruct all forms
   - Implement all event handlers
   - Complete RichEdit integration

7. **Remaining File Format Parsers**
   - SSF, ERF, RIM parsers
   - NCS integer hacks

8. **Testing & Verification**
   - Create test cases
   - Verify behavior matches original
   - Test all error paths

## Estimated Completion

**Current Progress**: ~15% complete

**Remaining Work**: ~85%

**Estimated Time for Full 1:1 Parity**:

- Function identification: 20-30 hours
- File format parsers: 40-60 hours
- UI reconstruction: 15-20 hours
- Configuration system: 10-15 hours
- Testing & verification: 20-30 hours

**Total Estimated**: 105-155 hours of reverse engineering work

## Notes

- The original TSLPatcher.exe is a Delphi application, so all source code must be in Delphi/Object Pascal
- Many functions are not automatically identified by Ghidra due to Delphi's compilation model
- Manual function identification and decompilation is required
- String analysis has provided significant insight into functionality
- File format knowledge from the codebase can accelerate GFF/TLK/2DA parser development

## Files Created

1. `docs/TSLPatcher_Reverse_Engineering_Analysis.md` - Initial analysis
2. `src/Tools/TSLPatcher/Delphi/TSLPatcher.pas` - Main unit
3. `src/Tools/TSLPatcher/Delphi/FileFormats/TwoDAPatcher.pas` - 2DA patcher

## Conclusion

Significant progress has been made in understanding the application structure and creating the initial Delphi source code framework. However, achieving full 1:1 parity requires extensive additional reverse engineering work, particularly:

1. Manual function identification and decompilation
2. Complete file format parser implementations
3. Full UI reconstruction
4. Complete configuration system

The foundation is in place, but substantial work remains to achieve complete parity with the original TSLPatcher.exe.
