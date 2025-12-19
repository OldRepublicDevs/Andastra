# Mass Effect Code Cleanup List

**Generated:** December 19, 2025  
**Issue:** Mass Effect code incorrectly placed in Infinity and Eclipse engine implementations  
**Root Cause:** Mass Effect uses Unreal Engine 3, NOT Infinity or Eclipse engines

## Files Requiring Mass Effect Reference Removal/Update

### Infinity Engine Folder (16 files)

All of these files incorrectly reference MassEffect.exe or MassEffect2.exe:

1. `src/Andastra/Runtime/Games/Infinity/Components/InfinityAnimationComponent.cs`
   - Lines 12-16, 43-46: References MassEffect.exe animation systems
   - **Action:** Remove Mass Effect references, document that Infinity = Baldur's Gate/Icewind Dale/Planescape only

2. `src/Andastra/Runtime/Games/Infinity/Dialogue/InfinityDialogueCameraController.cs`
   - Lines 14-18, 43-127: References MassEffect.exe dialogue camera
   - **Action:** Remove Mass Effect references, document Infinity Engine dialogue (if implementing)

3. `src/Andastra/Runtime/Games/Infinity/GUI/InfinityGuiManager.cs`
   - Line 25, 31: References MassEffect.exe GUI
   - **Action:** Remove Mass Effect references

4. `src/Andastra/Runtime/Games/Infinity/Fonts/InfinityBitmapFont.cs`
   - Lines 22, 31, 133: References MassEffect.exe fonts
   - **Action:** Remove Mass Effect references

5. `src/Andastra/Runtime/Games/Infinity/InfinityNavigationMesh.cs`
   - Lines 15, 20: References MassEffect.exe navigation
   - **Action:** Remove Mass Effect references

6. `src/Andastra/Runtime/Games/Infinity/InfinityTimeManager.cs`
   - Multiple references to Mass Effect time systems
   - **Action:** Remove Mass Effect references

7. `src/Andastra/Runtime/Games/Infinity/InfinityDelayScheduler.cs`
   - References Mass Effect delay scheduler
   - **Action:** Remove Mass Effect references

8. `src/Andastra/Runtime/Games/Infinity/InfinityEntity.cs`
   - References Mass Effect entity systems
   - **Action:** Remove Mass Effect references

9. `src/Andastra/Runtime/Games/Infinity/Components/InfinityScriptHooksComponent.cs`
   - References Mass Effect script hooks
   - **Action:** Remove Mass Effect references

10. `src/Andastra/Runtime/Games/Infinity/Components/InfinityTriggerComponent.cs`
    - References Mass Effect triggers
    - **Action:** Remove Mass Effect references

11. `src/Andastra/Runtime/Games/Infinity/Components/InfinityTransformComponent.cs`
    - References Mass Effect transforms
    - **Action:** Remove Mass Effect references

12. `src/Andastra/Runtime/Games/Infinity/Systems/InfinityFactionManager.cs`
    - References Mass Effect faction systems
    - **Action:** Remove Mass Effect references

13. `src/Andastra/Runtime/Games/Infinity/Components/InfinityFactionComponent.cs`
    - References Mass Effect faction components
    - **Action:** Remove Mass Effect references

14. `src/Andastra/Runtime/Games/Infinity/Components/InfinityWaypointComponent.cs`
    - References Mass Effect waypoints
    - **Action:** Remove Mass Effect references

15. `src/Andastra/Runtime/Games/Infinity/Components/InfinityDoorComponent.cs`
    - References Mass Effect doors
    - **Action:** Remove Mass Effect references

16. `src/Andastra/Runtime/Games/Infinity/Components/InfinityItemComponent.cs`
    - References Mass Effect items
    - **Action:** Remove Mass Effect references

### Eclipse Engine Folder

#### Mass Effect-Specific Files (DELETE ENTIRE FOLDER)

**Folder:** `src/Andastra/Runtime/Games/Eclipse/MassEffect/`

Files to DELETE:
1. `MassEffectEngine.cs`
2. `MassEffectGameSession.cs`
3. `MassEffectModuleLoader.cs`
4. `MassEffectModuleLoaderBase.cs`
5. `Save/MassEffectSaveSerializer.cs`

**Reason:** Mass Effect does NOT use Eclipse Engine. It uses Unreal Engine 3.

#### Eclipse Files Referencing Mass Effect (UPDATE)

1. `src/Andastra/Runtime/Games/Eclipse/EclipseArea.cs`
   - Line 26, 30, 38, 118, 381, 386, 517, 522, 646, 775, 1088, 1173, 1489, 1694: References Mass Effect
   - **Action:** Remove Mass Effect references, document that Eclipse = Dragon Age ONLY

2. `src/Andastra/Runtime/Games/Eclipse/GUI/EclipseGuiManager.cs`
   - Lines 22, 25, 33: References Mass Effect
   - **Action:** Remove Mass Effect references

3. `src/Andastra/Runtime/Games/Eclipse/Fonts/EclipseBitmapFont.cs`
   - Lines 18, 22, 33: References Mass Effect
   - **Action:** Remove Mass Effect references

4. `src/Andastra/Runtime/Games/Eclipse/Scene/EclipseSceneBuilder.cs`
   - Lines 13, 19, 54, 71, 150: References Mass Effect
   - **Action:** Remove Mass Effect references

5. `src/Andastra/Runtime/Games/Eclipse/Loading/EclipseNavigationMeshFactory.cs`
   - Line 20: References Mass Effect
   - **Action:** Remove Mass Effect references

6. `src/Andastra/Runtime/Games/Eclipse/EclipseNavigationMesh.cs`
   - Lines 22, 129, 305, 867, 1182, 1632, 1650, 1760, 1765, 1777, 1818: References Mass Effect
   - **Action:** Remove Mass Effect references

7. `src/Andastra/Runtime/Games/Eclipse/EclipseTimeManager.cs`
   - Lines 7, 155, 164, 181: References Mass Effect
   - **Action:** Remove Mass Effect references

8. `src/Andastra/Runtime/Content/Converters/BwmToEclipseNavigationMeshConverter.cs`
   - Line 14: References Mass Effect
   - **Action:** Remove Mass Effect references

### Graphics/Common Files

1. `src/Andastra/Runtime/Graphics/Common/Backends/InfinityGraphicsBackend.cs`
   - Lines 13, 17, 21, 63, 73: References Mass Effect
   - **Action:** Remove or clearly mark as incorrect

2. `src/Andastra/Runtime/Graphics/Common/Enums/GraphicsBackendType.cs`
   - Line 113: References Mass Effect with Infinity Engine
   - **Action:** Remove InfinityEngine backend type or document it's for Baldur's Gate ONLY

3. `src/Andastra/Runtime/Graphics/Common/Scene/BaseSceneBuilder.cs`
   - Line 35: References Mass Effect with Eclipse
   - **Action:** Remove Mass Effect references

4. `src/Andastra/Runtime/Graphics/Stride/Camera/StrideDialogueCameraController.cs`
   - Line 232: References MassEffect.exe
   - **Action:** Remove Mass Effect references

5. `src/Andastra/Runtime/Graphics/MonoGame/Camera/MonoGameDialogueCameraController.cs`
   - Line 250: References MassEffect.exe
   - **Action:** Remove Mass Effect references

### Core/Runtime Files

1. `src/Andastra/Runtime/Core/Camera/CameraController.cs`
   - Line 174: References MassEffect.exe
   - **Action:** Remove Mass Effect references

2. `src/Andastra/Runtime/Core/Interfaces/IDelayScheduler.cs`
   - References Mass Effect delay scheduler
   - **Action:** Remove Mass Effect references

### Documentation Files

1. `docs/time_manager_ghidra_verification_results.md`
   - Line 146: Lists MassEffect.exe as available
   - **Action:** Remove or mark as incorrect (Mass Effect is Unreal Engine 3)

2. `docs/time_manager_verification.md`
   - Line 100: References Mass Effect save formats
   - **Action:** Remove Mass Effect references

3. `docs/implementation_discrepancy_report.md`
   - Lines 83, 109, 223: References Mass Effect
   - **Action:** Remove Mass Effect references

4. `docs/engine_roadmap.md` (if exists)
   - **Action:** Check for Mass Effect references and remove

### Test Files

1. `src/Tools/HolocronToolset/Editors/DLGEditor.cs`
   - Line 30: References MassEffect.exe
   - **Action:** Remove Mass Effect references

2. `src/Tests/HolocronToolset.Tests/Formats/DLGFormatTests.cs`
   - Line 22: References MassEffect.exe
   - **Action:** Remove Mass Effect references

3. Test files for Infinity fonts/GUI:
   - `src/Andastra/Tests/Runtime/Games/Infinity/InfinityBitmapFontTests.cs`
   - `src/Andastra/Tests/Runtime/Games/Infinity/InfinityGuiManagerTests.cs`
   - **Action:** Check for Mass Effect references

## Summary Statistics

- **Total files affected:** 40+ files
- **Files in Infinity folder:** 16 files
- **Files in Eclipse folder:** 10+ files
- **Files in Eclipse/MassEffect folder:** 5 files (DELETE ENTIRE FOLDER)
- **Documentation files:** 4+ files
- **Graphics/Core files:** 6+ files
- **Test files:** 3+ files

## Recommended Cleanup Strategy

### Phase 1: Delete Mass Effect Folder (Immediate)
1. Delete `src/Andastra/Runtime/Games/Eclipse/MassEffect/` folder entirely
2. Remove any project references to MassEffect files
3. Commit: "chore: remove Mass Effect implementation (uses Unreal Engine 3, not Eclipse)"

### Phase 2: Update Infinity Engine Documentation (High Priority)
1. Update all Infinity files to document Baldur's Gate/Icewind Dale/Planescape ONLY
2. Remove all MassEffect.exe references from Infinity folder
3. Commit: "docs: correct Infinity Engine documentation (Baldur's Gate, not Mass Effect)"

### Phase 3: Update Eclipse Engine Documentation (High Priority)
1. Update all Eclipse files to document Dragon Age ONLY
2. Remove all MassEffect.exe references from Eclipse folder
3. Commit: "docs: correct Eclipse Engine documentation (Dragon Age, not Mass Effect)"

### Phase 4: Update Common/Graphics Files (Medium Priority)
1. Remove Mass Effect references from common infrastructure
2. Update graphics backend documentation
3. Commit: "refactor: remove Mass Effect references from common infrastructure"

### Phase 5: Update Documentation (High Priority)
1. Update all documentation files to remove Mass Effect
2. Create clear statement about engine scope
3. Commit: "docs: clarify engine scope (no Mass Effect - Unreal Engine 3)"

### Phase 6: Update Tests (Low Priority)
1. Remove Mass Effect references from tests
2. Update test documentation
3. Commit: "test: remove Mass Effect references from tests"

## Why This Cleanup Is Critical

1. **Architectural Confusion:** Developers may try to implement Mass Effect features using BioWare engine patterns
2. **Wasted Effort:** Reverse engineering MassEffect.exe won't help (it's Unreal Engine 3)
3. **Impossible Implementation:** Mass Effect systems are in Unreal Engine 3 DLLs, not game executables
4. **Misleading Documentation:** Comments claim Mass Effect uses Infinity/Eclipse when it doesn't
5. **Code Quality:** Mixing unrelated engines leads to unmaintainable code

## Correct Engine Scope

**Andastra should focus on:**
- ✅ Odyssey Engine (KOTOR 1 & 2) - You own these games
- ✅ Aurora Engine (Neverwinter Nights) - Well-documented
- ✅ Eclipse Engine (Dragon Age Origins & 2) - Proprietary BioWare
- ❌ Infinity Engine (Baldur's Gate) - You don't own these games
- ❌ Mass Effect (Unreal Engine 3) - Completely different engine

**If Mass Effect support is desired in the future:**
- Create separate Unreal Engine 3 implementation
- Study Unreal Engine 3 architecture (NOT BioWare engines)
- Use existing Mass Effect modding tools (ME3Explorer)
- DO NOT try to fit into BioWare engine architecture

---

**Next Steps:** Review with repository owner and begin cleanup phases

