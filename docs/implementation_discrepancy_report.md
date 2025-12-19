# Implementation Discrepancy Report
**Generated:** 2025-01-XX  
**Purpose:** Comprehensive audit of engine-specific and game-specific implementation completeness and 1:1 parity with reverse-engineered components

## Executive Summary

**CRITICAL FINDINGS:** The codebase does NOT have full 1:1 parity across all engines. Significant discrepancies exist in:
1. GUI control type rendering (missing ListBox, Progress, CheckBox, Slider in Aurora/Eclipse/Infinity)
2. Texture loading implementations (stubbed in Aurora/Eclipse/Infinity)
3. Partial implementations in various systems (55+ TODO markers found)

## GUI Manager Implementation Status

### Odyssey (KotorGuiManager) - ✅ MOSTLY COMPLETE
**Location:** `src/Andastra/Runtime/Graphics/MonoGame/GUI/KotorGuiManager.cs`

**Implemented:**
- ✅ LoadGui: Full implementation with GFF parsing
- ✅ UnloadGui: Full implementation
- ✅ SetCurrentGui: Full implementation
- ✅ Update: Full input handling (mouse/keyboard)
- ✅ Draw: Full rendering implementation
- ✅ Control Types Supported: Panel, Button, Label, **ListBox**, **Progress**, **CheckBox**, **Slider**

**Incomplete:**
- ⚠️ ListBox: Has TODOs (items rendering, scrollbar not fully implemented)
- ⚠️ CheckBox: Has TODO (checkmark rendering not implemented)
- ⚠️ Slider: Has TODO (thumb rendering not implemented)
- ⚠️ Texture Loading: Has TODO (TPC to Texture2D conversion not implemented)

**Ghidra References:**
- swkotor.exe: GUI system references throughout executable
- swkotor2.exe: FUN_0070a2e0 @ 0x0070a2e0 (GUI loading pattern)

---

### Aurora (AuroraGuiManager) - ⚠️ PARTIALLY COMPLETE
**Location:** `src/Andastra/Runtime/Games/Aurora/GUI/AuroraGuiManager.cs`

**Implemented:**
- ✅ LoadGui: Full implementation with GFF parsing (TODO PLACEHOLDER about format)
- ✅ UnloadGui: Full implementation
- ✅ SetCurrentGui: Full implementation
- ✅ Update: Full input handling (mouse/keyboard)
- ✅ Draw: Full rendering implementation
- ✅ Control Types Supported: Panel, Button, Label

**MISSING (Discrepancy with Odyssey):**
- ❌ **ListBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Progress**: Not implemented (falls through to RenderGenericControl)
- ❌ **CheckBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Slider**: Not implemented (falls through to RenderGenericControl)
- ❌ **Texture Loading**: Stubbed (returns null, TODO comment)

**Ghidra References:**
- nwmain.exe: GUI loading functions (address verification pending Ghidra analysis)

**Discrepancy Impact:** Aurora cannot render ListBox, Progress, CheckBox, or Slider controls that exist in GUI files, breaking compatibility with GUIs that use these controls.

---

### Eclipse (EclipseGuiManager) - ⚠️ PARTIALLY COMPLETE
**Location:** `src/Andastra/Runtime/Games/Eclipse/GUI/EclipseGuiManager.cs`

**Implemented:**
- ✅ LoadGui: Full implementation with GFF parsing (TODO PLACEHOLDER about format)
- ✅ UnloadGui: Full implementation
- ✅ SetCurrentGui: Full implementation
- ✅ Update: Full input handling (mouse/keyboard)
- ✅ Draw: Full rendering implementation
- ✅ Control Types Supported: Panel, Button, Label

**MISSING (Discrepancy with Odyssey):**
- ❌ **ListBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Progress**: Not implemented (falls through to RenderGenericControl)
- ❌ **CheckBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Slider**: Not implemented (falls through to RenderGenericControl)
- ❌ **Texture Loading**: Stubbed (returns null, TODO comment)

**Ghidra References:**
- daorigins.exe: GUI loading functions (address verification pending Ghidra analysis)
- DragonAge2.exe: GUI loading functions (address verification pending Ghidra analysis)
- MassEffect.exe: GUI loading functions (address verification pending Ghidra analysis)
- MassEffect2.exe: GUI loading functions (address verification pending Ghidra analysis)

**Discrepancy Impact:** Eclipse cannot render ListBox, Progress, CheckBox, or Slider controls that exist in GUI files, breaking compatibility with GUIs that use these controls.

---

### Infinity (InfinityGuiManager) - ⚠️ PARTIALLY COMPLETE
**Location:** `src/Andastra/Runtime/Games/Infinity/GUI/InfinityGuiManager.cs`

**Implemented:**
- ✅ LoadGui: Full implementation with GFF parsing (TODO PLACEHOLDER about Unreal Engine format)
- ✅ UnloadGui: Full implementation
- ✅ SetCurrentGui: Full implementation
- ✅ Update: Full input handling (mouse/keyboard)
- ✅ Draw: Full rendering implementation
- ✅ Control Types Supported: Panel, Button, Label

**MISSING (Discrepancy with Odyssey):**
- ❌ **ListBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Progress**: Not implemented (falls through to RenderGenericControl)
- ❌ **CheckBox**: Not implemented (falls through to RenderGenericControl)
- ❌ **Slider**: Not implemented (falls through to RenderGenericControl)
- ❌ **Texture Loading**: Stubbed (returns null, TODO comment)

**Ghidra References:**
- MassEffect.exe: GUI loading functions (address verification pending Ghidra analysis)
- MassEffect2.exe: GUI loading functions (address verification pending Ghidra analysis)

**Discrepancy Impact:** Infinity cannot render ListBox, Progress, CheckBox, or Slider controls that exist in GUI files, breaking compatibility with GUIs that use these controls.

---

## Critical Discrepancies Summary

### 1. Missing Control Type Rendering (Aurora/Eclipse/Infinity)

**Issue:** Aurora, Eclipse, and Infinity GUI managers only render Panel, Button, and Label controls. They are missing implementations for:
- ListBox (GUIControlType.ListBox = 11)
- Progress (GUIControlType.Progress = 10)
- CheckBox (GUIControlType.CheckBox = 7)
- Slider (GUIControlType.Slider = 8)

**Odyssey Implementation Reference:**
- `RenderListBox()` - Lines 696-711 in KotorGuiManager.cs (has TODOs for items/scrollbar)
- `RenderProgressBar()` - Lines 716-745 in KotorGuiManager.cs (fully implemented)
- `RenderCheckBox()` - Lines 750-764 in KotorGuiManager.cs (has TODO for checkmark)
- `RenderSlider()` - Lines 769-783 in KotorGuiManager.cs (has TODO for thumb)

**Impact:** GUIs that use these control types will not render correctly in Aurora/Eclipse/Infinity engines, causing visual bugs and potential functionality issues.

**Required Action:** Implement RenderListBox, RenderProgressBar, RenderCheckBox, and RenderSlider in AuroraGuiManager, EclipseGuiManager, and InfinityGuiManager to match Odyssey implementation.

---

### 2. Texture Loading Stubbed (Aurora/Eclipse/Infinity)

**Issue:** All three engines have texture loading stubbed with TODO comments:
- Aurora: `LoadTexture()` returns null (line 628-630)
- Eclipse: `LoadTexture()` returns null (line 530-531)
- Infinity: `LoadTexture()` returns null (line 507-508)

**Odyssey Implementation Reference:**
- `LoadTexture()` - Lines 812-846 in KotorGuiManager.cs (has TODO for TPC conversion, but structure exists)

**Impact:** GUI textures will not load, causing GUIs to render without backgrounds, button images, or other texture-based elements.

**Required Action:** Implement texture loading in all three engines. May need engine-specific texture format support (TPC for Odyssey/Aurora/Eclipse, DDS/Unreal formats for Infinity).

---

### 3. Partial Control Type Implementations (Odyssey)

**Issue:** Even Odyssey has incomplete implementations:
- ListBox: Missing item rendering and scrollbar (TODOs on lines 709-710)
- CheckBox: Missing checkmark rendering (TODO on line 763)
- Slider: Missing thumb rendering (TODO on line 782)

**Impact:** These controls render partially but may not function correctly or display all visual elements.

**Required Action:** Complete ListBox, CheckBox, and Slider rendering in Odyssey, then port to other engines.

---

## Other System Discrepancies

### AI Controllers
- **AuroraAIController**: TODO STUB for perception checking and combat AI (lines 82, 93)
- **EclipseAIController**: TODO STUB for perception checking and combat AI (lines 150, 160)
- **InfinityAIController**: TODO STUB for perception checking and combat AI (lines 150, 160)

### Animation Components
- **EclipseAnimationComponent**: TODO PLACEHOLDER for animation duration loading (lines 46, 55)
- **InfinityAnimationComponent**: TODO PLACEHOLDER for animation duration loading (lines 43, 52)

### Faction Components
- **AuroraFactionComponent**: TODO PLACEHOLDER for faction relationship lookup (lines 63, 107)

### Module Loaders
- **InfinityModuleLoader**: TODO PLACEHOLDER for module loading (lines 55, 78, 86)

### Data Providers
- **InfinityGameDataProvider**: TODO PLACEHOLDER for data lookup (lines 32, 52)

### Upgrade Screens
- **EclipseUpgradeScreen**: TODO STUB for UI rendering (lines 65, 88)

### Engine Initialization
- **AuroraEngine**: TODO STUB for AuroraGameSession and resource provider (lines 41, 47)

---

## Inheritance Structure Verification

### BaseGuiManager (Common)
✅ **Correct:** Contains only abstract methods and common functionality (text alignment, button events)

### Engine-Specific Subclasses
✅ **Correct Structure:**
- Each engine has its own GUI manager subclass
- Common patterns (dictionary lookup, LoadedGui structure) are consistent
- Engine-specific details (font types, texture formats) are in subclasses

⚠️ **Issue:** Missing control type rendering creates functional discrepancy - all engines should support the same control types as defined in GUIControlType enum.

---

## Ghidra Analysis Requirements

### Critical Missing Analysis:
1. **Aurora (nwmain.exe):**
   - GUI switching function addresses
   - Texture loading function addresses
   - Control type rendering function addresses

2. **Eclipse (daorigins.exe, DragonAge2.exe):**
   - GUI switching function addresses
   - Texture loading function addresses
   - Control type rendering function addresses

3. **Infinity (MassEffect.exe, MassEffect2.exe):**
   - GUI switching function addresses
   - Texture loading function addresses
   - Control type rendering function addresses
   - Unreal Engine GUI format verification

---

## Recommendations

### Immediate Actions Required:
1. **Implement missing control types** in Aurora/Eclipse/Infinity GUI managers
2. **Implement texture loading** in Aurora/Eclipse/Infinity GUI managers
3. **Complete partial implementations** in Odyssey (ListBox items, CheckBox checkmark, Slider thumb)
4. **Verify GUI format compatibility** via Ghidra analysis for all engines

### Priority Order:
1. **HIGH:** Texture loading (blocks all GUI rendering with textures)
2. **HIGH:** Missing control types (breaks GUIs using ListBox/Progress/CheckBox/Slider)
3. **MEDIUM:** Complete partial implementations in Odyssey
4. **LOW:** Ghidra address verification (documentation improvement)

---

## Conclusion

**The codebase does NOT have full 1:1 parity.** Significant discrepancies exist in GUI rendering capabilities across engines. Aurora, Eclipse, and Infinity are missing 4 control types and texture loading compared to Odyssey. These discrepancies will cause visual bugs and functionality issues when GUIs use unsupported control types.

**Status:** ⚠️ **NOT FULLY EXHAUSTIVE** - Requires implementation of missing control types and texture loading to achieve parity.

