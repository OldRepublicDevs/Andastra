# TODO Analysis: Blocking vs Workable

**Purpose**: Categorize all TODOs to determine which are **BLOCKING** (must be fixed immediately) vs **WORKABLE** (can be expanded later)

---

## BLOCKING TODOs (Must Fix First)

These TODOs will cause crashes, null references, or prevent basic functionality from working.

### Phase 0: Critical Foundation

#### 0.1 Game Initialization
- **File**: `src/Andastra/Game/Core/OdysseyGame.cs`
  - **Status**: ✅ WORKABLE - Game initializes, has menu system
  - **Note**: Core initialization works, but some systems may need expansion

#### 0.2 Resource Provider
- **File**: `src/Andastra/Runtime/Content/ResourceProviders/GameResourceProvider.cs`
  - **Line 146**: `TODO: STUB - EnumerateResources returns empty`
  - **Status**: ⚠️ **BLOCKING** if resource enumeration is needed, otherwise WORKABLE
  - **Impact**: Resource lookup works, but enumeration fails
  - **Workaround**: Direct resource access works via `OpenResourceAsync`

#### 0.3 Content Cache
- **File**: `src/Andastra/Runtime/Content/Cache/ContentCache.cs`
  - **Lines 101-102**: `TODO: SIMPLIFIED/PLACEHOLDER - Cache always returns miss`
  - **Status**: ✅ **WORKABLE** - Cache misses work, just slower (no caching)
  - **Impact**: Performance hit, but functionality works
  - **Note**: Can defer optimization

#### 0.4 Entity Factory (CRITICAL BLOCKER)
- **File**: `src/Andastra/Runtime/Games/Odyssey/EngineApi/TheSithLords.cs`
  - **Lines 315-318, 340**: `TODO: STUB - EntityFactory not accessible, returns null`
  - **Status**: 🔴 **BLOCKING** - Cannot spawn entities, breaks `CreateObject`, `CreateCreature`
  - **Impact**: No NPCs, no items, no placeables can spawn
  - **Must Fix**: Phase 0 or Phase 3.1

#### 0.5 Template Loader
- **File**: `src/Andastra/Runtime/Content/Loaders/TemplateLoader.cs`
  - **Line 584**: `TODO: SIMPLIFIED - Localization returns empty`
  - **Status**: ✅ **WORKABLE** - Templates load, just no localized names
  - **Impact**: Minor - entities work but may show empty names

### Phase 1: Basic Rendering

#### 1.1 Room Mesh Renderer
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Converters/RoomMeshRenderer.cs`
  - **Status**: ⚠️ **BLOCKING** if area doesn't render
  - **Note**: Need to check if basic rendering works

#### 1.2 Odyssey Renderer
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/OdysseyRenderer.cs`
  - **Lines 361-363, 378, 388, 395**: `TODO: SIMPLIFIED/STUB - Custom backends disabled`
  - **Status**: ✅ **WORKABLE** - MonoGame rendering works, just no Vulkan/DirectX
  - **Impact**: None for playable demo (MonoGame is sufficient)

### Phase 2: Movement & Navigation

#### 2.1 Pathfinding
- **File**: `src/Andastra/Runtime/Core/Actions/ActionMoveToLocation.cs`
  - **Lines 216-217**: `TODO: SIMPLIFIED - No obstacle avoidance, fails on collision`
  - **Status**: ⚠️ **PARTIALLY BLOCKING** - Movement works but stops on obstacles
  - **Impact**: Player can move but gets stuck easily
  - **Workaround**: Works for open areas, fails in tight spaces
  - **Priority**: Medium - can work for demo but frustrating

- **Lines 304-305**: `TODO: SIMPLIFIED - Simple radius collision`
  - **Status**: ✅ **WORKABLE** - Basic collision works, just not precise
  - **Impact**: Minor - may have some collision issues but functional

- **Line 363**: `TODO: SIMPLIFIED - Size-based defaults for creature radius`
  - **Status**: ✅ **WORKABLE** - Uses defaults, works but not accurate
  - **Impact**: Minor - collision may be slightly off

### Phase 3: Basic Interaction

#### 3.1 Party System
- **File**: `src/Andastra/Runtime/Core/Party/PartySystem.cs`
  - **Lines 842-843**: `TODO: SIMPLIFIED/PLACEHOLDER - Basic entity creation`
  - **Status**: ⚠️ **BLOCKING** if party members needed, otherwise WORKABLE
  - **Impact**: Party members may not have proper templates/stats
  - **Note**: Depends on EntityFactory (0.4) being fixed first

#### 3.2 Engine API Functions
- **File**: `src/Andastra/Runtime/Games/Odyssey/EngineApi/TheSithLords.cs`
  - **Lines 428, 441**: `TODO: STUB - StealthXPEnabled always returns enabled`
  - **Status**: ✅ **WORKABLE** - Returns default value, doesn't break
  - **Impact**: Minor - stealth XP always enabled (not game-breaking)

### Phase 4: Core Gameplay

#### 4.1 UI Systems
- **File**: `src/Andastra/Runtime/Games/Odyssey/UI/OdysseyUpgradeScreen.cs`
  - **Lines 120, 134, 190, 251, 268, 314**: `TODO: STUB - UI rendering not implemented`
  - **Status**: 🔴 **BLOCKING** for upgrade screen functionality
  - **Impact**: Upgrade screen doesn't work at all
  - **Note**: Can defer if not needed for playable demo

---

## WORKABLE TODOs (Can Expand Later)

These TODOs have functional implementations that work but are simplified or placeholder.

### Performance Optimizations (All Workable)

#### Content Cache
- **File**: `src/Andastra/Runtime/Content/Cache/ContentCache.cs`
  - **Line 289**: `TODO: SIMPLIFIED - Rough size estimate`
  - **Status**: ✅ **WORKABLE** - Estimates work, just not precise
  - **Impact**: Cache size tracking may be inaccurate, but doesn't break functionality

#### Resource Preloader
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/ResourcePreloader.cs`
  - **Lines 126-131, 149-150**: `TODO: SIMPLIFIED - Basic preloading`
  - **Status**: ✅ **WORKABLE** - Preloading works, just not optimal
  - **Impact**: May load more than needed, but functional

#### Vertex Cache Optimizer
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/VertexCacheOptimizer.cs`
  - **Status**: ✅ **WORKABLE** - Optimization can be deferred
  - **Impact**: Performance, not functionality

### Rendering Enhancements (All Workable)

#### GPU Instancing
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/GPUInstancing.cs`
  - **Status**: ✅ **WORKABLE** - Can render without instancing
  - **Impact**: Performance only

#### Occlusion Culling
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Culling/OcclusionCuller.cs`
  - **Status**: ✅ **WORKABLE** - Can render without occlusion culling
  - **Impact**: Performance only

#### Camera Systems
- **Files**: `MonoGameDialogueCameraController.cs`, `StrideDialogueCameraController.cs`
  - **Lines 101, 103, 118, 119**: `TODO: SIMPLIFIED/PLACEHOLDER - Basic camera`
  - **Status**: ✅ **WORKABLE** - Basic camera works, just no advanced features
  - **Impact**: Camera follows player, just no dialogue-specific animations

### Audio Systems (All Workable)

#### Stride Voice Player
- **File**: `src/Andastra/Runtime/Graphics/Stride/Audio/StrideVoicePlayer.cs`
  - **Lines 34-35, 77-79, 86, 107, 121, 133-134, 143**: `TODO: STUB/PLACEHOLDER`
  - **Status**: ✅ **WORKABLE** - MonoGame audio works, Stride is alternative
  - **Impact**: None if using MonoGame backend

#### MonoGame Sound Player
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Audio/MonoGameSoundPlayer.cs`
  - **Status**: ⚠️ Need to check implementation
  - **Note**: If STUB, may be blocking for audio

### Advanced Features (All Workable)

#### Raytracing
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Raytracing/NativeRaytracingSystem.cs`
  - **Status**: ✅ **WORKABLE** - Not needed for playable demo
  - **Impact**: None - advanced feature

#### DLSS
- **File**: `src/Andastra/Runtime/Graphics/Stride/Upscaling/StrideDlssSystem.cs`
  - **Status**: ✅ **WORKABLE** - Not needed for playable demo
  - **Impact**: None - advanced feature

#### LZMA Support
- **File**: `src/Andastra/Utility/LZMA/LzmaHelper.cs`
  - **Lines 11-12, 21, 27**: `TODO: STUB/PLACEHOLDER - LZMA not implemented`
  - **Status**: ⚠️ **BLOCKING** if BZF files are used, otherwise WORKABLE
  - **Impact**: Cannot load compressed BZF files
  - **Note**: Most KOTOR2 files are not BZF, so may be workable

### Other Engine Support (All Workable)

#### Aurora Engine
- **File**: `src/Andastra/Runtime/Games/Aurora/EngineApi/AuroraEngineApi.cs`
  - **Status**: ✅ **WORKABLE** - Not needed for KOTOR2 demo
  - **Impact**: None for KOTOR2

#### Eclipse Engine
- **File**: `src/Andastra/Runtime/Games/Eclipse/EngineApi/EclipseEngineApi.cs`
  - **Status**: ✅ **WORKABLE** - Not needed for KOTOR2 demo
  - **Impact**: None for KOTOR2

---

## Summary by Priority

### 🔴 CRITICAL BLOCKERS (Must Fix for Playable Demo)

1. **EntityFactory Access** (`TheSithLords.cs` lines 315-318, 340)
   - **Why**: Cannot spawn any entities (NPCs, items, placeables)
   - **Blocks**: Entity spawning, party system, module loading
   - **Phase**: 0.4 or 3.1

2. **Template Loading** (if EntityFactory depends on it)
   - **Why**: Entities need templates to have proper stats/behavior
   - **Blocks**: Entity creation
   - **Phase**: 0.5 or 3.1

3. **Room Mesh Rendering** (if not working)
   - **Why**: Cannot see the game world
   - **Blocks**: Visual feedback
   - **Phase**: 1.1

4. **Basic Pathfinding** (if movement completely broken)
   - **Why**: Player cannot navigate
   - **Blocks**: Core gameplay
   - **Phase**: 2.1

### ⚠️ PARTIAL BLOCKERS (Work but Limited)

1. **Pathfinding Obstacle Avoidance** (`ActionMoveToLocation.cs` lines 216-217)
   - **Why**: Movement works but stops on obstacles
   - **Impact**: Frustrating but functional
   - **Priority**: Medium

2. **Party System Entity Creation** (`PartySystem.cs` lines 842-843)
   - **Why**: Party members may not have proper templates
   - **Impact**: Party works but may be incomplete
   - **Priority**: Medium (if party needed)

3. **Resource Enumeration** (`GameResourceProvider.cs` line 146)
   - **Why**: Cannot enumerate resources
   - **Impact**: Some features may fail, but direct access works
   - **Priority**: Low (if enumeration not used)

### ✅ WORKABLE (Can Defer)

- Content cache (always misses - performance only)
- Rendering optimizations (GPU instancing, occlusion culling)
- Advanced camera features (dialogue camera animations)
- Audio systems (Stride backend - MonoGame works)
- UI systems (upgrade screen - not critical for demo)
- Advanced features (raytracing, DLSS, LZMA if not used)
- Other engine support (Aurora, Eclipse, Infinity)

---

## Recommended Fix Order for Playable Demo

### Phase 0 (Critical)
1. ✅ EntityFactory access - **MUST FIX**
2. ✅ Template loading integration - **MUST FIX**
3. ⚠️ Resource enumeration - **CHECK IF USED**

### Phase 1 (Rendering)
1. ⚠️ Room mesh rendering - **VERIFY WORKS**
2. ✅ Rendering backend - **WORKABLE** (MonoGame sufficient)

### Phase 2 (Movement)
1. ⚠️ Basic pathfinding - **VERIFY WORKS**
2. ⚠️ Obstacle avoidance - **MEDIUM PRIORITY** (works but limited)

### Phase 3 (Interaction)
1. ✅ Party system - **DEPENDS ON EntityFactory**
2. ✅ Engine API functions - **MOST WORKABLE**

### Phase 4+ (Can Defer)
- All optimization TODOs
- All advanced feature TODOs
- All other engine TODOs

---

## Quick Reference: Blocking vs Workable

| Category | Blocking | Workable |
|----------|----------|----------|
| **Entity Spawning** | EntityFactory access | Party system (depends on EntityFactory) |
| **Rendering** | Room mesh (if broken) | GPU instancing, occlusion culling |
| **Movement** | Basic pathfinding (if broken) | Obstacle avoidance, collision precision |
| **Audio** | MonoGame audio (if broken) | Stride audio, advanced features |
| **UI** | Core UI (if broken) | Upgrade screen, advanced menus |
| **Performance** | None | All optimizations |
| **Advanced Features** | None | Raytracing, DLSS, LZMA (if not used) |
| **Other Engines** | None | Aurora, Eclipse, Infinity |

---

## Action Items

1. **Verify EntityFactory** - Check if this is the main blocker
2. **Test Room Rendering** - Verify areas actually render
3. **Test Basic Movement** - Verify pathfinding works at all
4. **Check Resource Enumeration Usage** - See if any code depends on it
5. **Prioritize Based on Testing** - Fix actual blockers first

---

**Last Updated**: Based on code analysis of TODO implementations
**Next Step**: Test actual game to identify real blockers vs theoretical ones

