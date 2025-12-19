# KOTOR2 Playable Demo Roadmap

**Goal**: Achieve a bare minimum playable demo for KOTOR2 (The Sith Lords) in `./src/Andastra`

**Total TODOs**: 312 across 93 files

**Workflow**: Each TODO will be resolved using the standardized prompt:
```
please grep the whole codebase for the term 'TODO: ' using `rg "TODO: " --type cs`. Then please run scripts/Get-RandomNumber.ps1 using upper bounds of the total number of results you've found. Depending on random number chosen, please implement comprehensively the canonical/expert level requirements exhaustively and completely to resolve the TODO, replacing a 'simplification' or 'placeholder' with fully functional production-grade code. Use industry standards and widely adopted practices, following @.cursorrules and any roadmap implementations in @README.md and @.cursor 

use `rg "TODO: " --type cs | Select-Object -Index <the random number generated>` specifically

NOTE: please exclusively start in ./src/Andastra
```

---

## Phase 0: Critical Foundation (Must Work First)

**Priority**: CRITICAL - Blocks everything else  
**Goal**: Game launches, basic systems initialize, no crashes

### 0.1 Game Initialization & Core Systems
- **File**: `src/Andastra/Game/Core/OdysseyGame.cs`
  - TODO: STUB - Game loop initialization
  - TODO: STUB - Graphics backend initialization
  - TODO: STUB - Resource provider setup
  - TODO: STUB - Module loading system
  - TODO: STUB - Save system initialization
  - TODO: STUB - Input system setup
  - TODO: STUB - Audio system initialization
  - TODO: STUB - Script executor initialization
  - TODO: STUB - World initialization
  - TODO: STUB - Entity system setup
  - TODO: STUB - Game session management

### 0.2 Resource Provider & Content Loading
- **File**: `src/Andastra/Runtime/Content/ResourceProviders/GameResourceProvider.cs`
  - TODO: STUB - Resource path resolution
  - TODO: STUB - File format detection
  - TODO: STUB - Resource precedence chain (override → module → save → chitin)
  - TODO: STUB - Installation path detection

- **File**: `src/Andastra/Runtime/Content/Cache/ContentCache.cs`
  - TODO: SIMPLIFIED - Cache hit/miss logic (lines 101-102)
  - TODO: PLACEHOLDER - Cache serialization (line 158)
  - TODO: SIMPLIFIED - Cache size estimation (line 289)

### 0.3 Content Converters (Critical Formats)
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Converters/TpcToMonoGameTextureConverter.cs`
  - TODO: STUB - TPC texture loading
  - TODO: STUB - Texture format conversion
  - TODO: STUB - Mipmap generation

- **File**: `src/Andastra/Runtime/Content/Converters/BwmToNavigationMeshConverter.cs`
  - TODO: STUB - BWM walkmesh parsing
  - TODO: SIMPLIFIED - Navigation mesh construction
  - TODO: PLACEHOLDER - Surface material mapping

### 0.4 Scripting Foundation
- **File**: `src/Andastra/Runtime/Scripting/VM/NcsVm.cs`
  - TODO: STUB - VM initialization
  - TODO: STUB - Instruction execution loop
  - TODO: SIMPLIFIED - Stack management

- **File**: `src/Andastra/Runtime/Scripting/ScriptExecutor.cs`
  - TODO: STUB - Script execution context
  - TODO: STUB - Delay wheel management
  - TODO: STUB - Action queue processing

- **File**: `src/Andastra/Runtime/Scripting/EngineApi/BaseEngineApi.cs`
  - TODO: STUB - Engine API function dispatch
  - TODO: SIMPLIFIED - Function registration

### 0.5 Module Loading
- **File**: `src/Andastra/Runtime/Games/Odyssey/Game/ModuleLoader.cs`
  - TODO: STUB - Module file parsing
  - TODO: STUB - Area loading
  - TODO: STUB - GIT parsing (entity placement)
  - TODO: PLACEHOLDER - Resource dependency resolution

---

## Phase 1: Basic Rendering (See the World)

**Priority**: HIGH - Required for visual feedback  
**Goal**: Area renders, entities visible, basic camera

### 1.1 Area Rendering
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Converters/RoomMeshRenderer.cs`
  - TODO: STUB - Room mesh loading
  - TODO: STUB - LYT layout parsing
  - TODO: STUB - VIS visibility culling
  - TODO: SIMPLIFIED - Mesh batching
  - TODO: PLACEHOLDER - Texture coordinate handling
  - TODO: SIMPLIFIED - Material system
  - TODO: STUB - Lighting setup
  - TODO: PLACEHOLDER - Fog rendering

### 1.2 Entity Rendering
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/OdysseyRenderer.cs`
  - TODO: STUB - Entity model rendering (line 116)
  - TODO: SIMPLIFIED - MonoGame rendering backend (lines 361-363)
  - TODO: PLACEHOLDER - Multi-backend support (line 378)
  - TODO: STUB - Vulkan backend (line 388)
  - TODO: STUB - DirectX backends (line 395)

- **File**: `src/Andastra/Runtime/Graphics/Stride/Graphics/StrideEntityModelRenderer.cs`
  - TODO: STUB - Stride entity rendering
  - TODO: PLACEHOLDER - Model loading

### 1.3 Camera System
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Camera/MonoGameDialogueCameraController.cs`
  - TODO: SIMPLIFIED - Camera angle changes (line 101)
  - TODO: PLACEHOLDER - Camera animation system (line 103)
  - TODO: SIMPLIFIED - Player entity tracking (line 118)
  - TODO: SIMPLIFIED - Chase mode (line 119)

- **File**: `src/Andastra/Runtime/Graphics/Stride/Camera/StrideDialogueCameraController.cs`
  - TODO: SIMPLIFIED - Camera angle changes (line 101)
  - TODO: PLACEHOLDER - Camera animation system (line 103)
  - TODO: SIMPLIFIED - Player entity tracking (line 118)
  - TODO: SIMPLIFIED - Chase mode (line 119)

### 1.4 Rendering Optimization
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/ResourcePreloader.cs`
  - TODO: SIMPLIFIED - Resource preloading (lines 126-131)
  - TODO: SIMPLIFIED - Spatial prediction (lines 149-150)

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Culling/OcclusionCuller.cs`
  - TODO: STUB - Occlusion culling
  - TODO: SIMPLIFIED - Frustum culling
  - TODO: PLACEHOLDER - Portal culling
  - TODO: STUB - Hierarchical Z-buffer
  - TODO: SIMPLIFIED - Visibility queries

---

## Phase 2: Basic Movement & Navigation (Walk Around)

**Priority**: HIGH - Core gameplay mechanic  
**Goal**: Player can move, pathfinding works, collision detection

### 2.1 Player Movement
- **File**: `src/Andastra/Runtime/Core/Movement/PlayerInputHandler.cs`
  - TODO: STUB - Click-to-move
  - TODO: SIMPLIFIED - Input processing
  - TODO: PLACEHOLDER - Keyboard movement

- **File**: `src/Andastra/Runtime/Games/Odyssey/Game/PlayerController.cs`
  - TODO: STUB - Player controller setup
  - TODO: STUB - Movement state machine
  - TODO: PLACEHOLDER - Animation integration

### 2.2 Pathfinding & Navigation
- **File**: `src/Andastra/Runtime/Core/Navigation/NavigationMesh.cs`
  - TODO: STUB - A* pathfinding algorithm
  - TODO: SIMPLIFIED - Path smoothing
  - TODO: PLACEHOLDER - Dynamic obstacle avoidance
  - TODO: STUB - Surface material costs
  - TODO: SIMPLIFIED - Multi-level navigation

- **File**: `src/Andastra/Runtime/Core/Actions/ActionMoveToLocation.cs`
  - TODO: SIMPLIFIED - Obstacle avoidance (lines 216-217)
  - TODO: SIMPLIFIED - Collision detection (lines 304-305)
  - TODO: SIMPLIFIED - GameDataManager access (line 363)

- **File**: `src/Andastra/Runtime/Core/Actions/ActionMoveToObject.cs`
  - TODO: STUB - Move to object
  - TODO: SIMPLIFIED - Target tracking
  - TODO: PLACEHOLDER - Arrival detection

### 2.3 Collision Detection
- **File**: `src/Andastra/Runtime/Core/Actions/ActionMoveToLocation.cs`
  - TODO: SIMPLIFIED - Bounding box collision (lines 304-305)
  - TODO: STUB - Walkmesh collision
  - TODO: PLACEHOLDER - Entity-to-entity collision

---

## Phase 3: Basic Interaction (Scripts & Entities)

**Priority**: MEDIUM-HIGH - Required for gameplay  
**Goal**: Scripts execute, entities spawn, basic interactions work

### 3.1 Entity System
- **File**: `src/Andastra/Runtime/Content/Loaders/TemplateLoader.cs`
  - TODO: STUB - Template loading
  - TODO: STUB - Creature template parsing
  - TODO: PLACEHOLDER - Item template parsing

- **File**: `src/Andastra/Runtime/Core/Party/PartySystem.cs`
  - TODO: SIMPLIFIED - Party member creation (lines 842-843)
  - TODO: STUB - Party management
  - TODO: PLACEHOLDER - Party member spawning

### 3.2 Entity Components
- **File**: `src/Andastra/Runtime/Games/Odyssey/Components/StatsComponent.cs`
  - TODO: STUB - Stats calculation
  - TODO: SIMPLIFIED - Ability scores
  - TODO: PLACEHOLDER - Skill ranks

- **File**: `src/Andastra/Runtime/Games/Odyssey/Components/AnimationComponent.cs`
  - TODO: STUB - Animation playback
  - TODO: PLACEHOLDER - Animation blending

- **File**: `src/Andastra/Runtime/Games/Odyssey/Components/PerceptionComponent.cs`
  - TODO: STUB - Perception system integration

### 3.3 Action System
- **File**: `src/Andastra/Runtime/Core/Actions/ActionQueue.cs`
  - TODO: STUB - Action queue management
  - TODO: STUB - Action priority system
  - TODO: PLACEHOLDER - Action cancellation

- **File**: `src/Andastra/Runtime/Core/Actions/ActionUseItem.cs`
  - TODO: STUB - Item usage
  - TODO: SIMPLIFIED - Item validation
  - TODO: PLACEHOLDER - Item effects

- **File**: `src/Andastra/Runtime/Core/Actions/ActionCastSpellAtLocation.cs`
  - TODO: STUB - Spell casting
  - TODO: STUB - Spell effects
  - TODO: PLACEHOLDER - Spell validation

### 3.4 Script Execution
- **File**: `src/Andastra/Runtime/Games/Odyssey/Game/ScriptExecutor.cs`
  - TODO: STUB - Script execution context
  - TODO: STUB - Event handling
  - TODO: PLACEHOLDER - Script debugging

### 3.5 Engine API (Critical Functions)
- **File**: `src/Andastra/Runtime/Games/Odyssey/EngineApi/TheSithLords.cs`
  - TODO: STUB - EntityFactory access (lines 315-316, 318, 340)
  - TODO: STUB - StealthXPEnabled (lines 428, 441)
  - TODO: STUB - GetPosition
  - TODO: STUB - GetFacing
  - TODO: STUB - SetPosition
  - TODO: STUB - SetFacing
  - TODO: STUB - GetArea
  - TODO: STUB - GetModuleFileName

- **File**: `src/Andastra/Runtime/Games/Odyssey/EngineApi/Kotor1.cs`
  - TODO: STUB - KOTOR1-specific functions
  - TODO: SIMPLIFIED - Common function implementations
  - TODO: PLACEHOLDER - Function registration

### 3.6 Perception System
- **File**: `src/Andastra/Runtime/Games/Odyssey/Systems/PerceptionManager.cs`
  - TODO: SIMPLIFIED - Entity detection (line 267)
  - TODO: STUB - Perception range calculation
  - TODO: PLACEHOLDER - Perception events

- **File**: `src/Andastra/Runtime/Games/Odyssey/Systems/AIController.cs`
  - TODO: STUB - AI behavior tree
  - TODO: SIMPLIFIED - AI state machine
  - TODO: PLACEHOLDER - AI decision making

---

## Phase 4: Core Gameplay Systems

**Priority**: MEDIUM - Required for full gameplay  
**Goal**: Combat works, dialogue functional, inventory usable

### 4.1 Combat System
- **File**: `src/Andastra/Runtime/Games/Odyssey/Combat/CombatManager.cs`
  - TODO: STUB - Combat round management
  - TODO: STUB - Attack resolution
  - TODO: PLACEHOLDER - Damage calculation

- **File**: `src/Andastra/Runtime/Games/Odyssey/Combat/WeaponDamageCalculator.cs`
  - TODO: STUB - Weapon damage calculation
  - TODO: SIMPLIFIED - Damage dice rolling
  - TODO: PLACEHOLDER - Critical hit calculation
  - TODO: STUB - Damage type application
  - TODO: SIMPLIFIED - Damage reduction

### 4.2 Dialogue System
- **File**: `src/Andastra/Runtime/Games/Odyssey/Dialogue/DialogueManager.cs`
  - TODO: STUB - DLG conversation loading
  - TODO: STUB - Dialogue tree navigation
  - TODO: PLACEHOLDER - Voice-over playback

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Audio/MonoGameSoundPlayer.cs`
  - TODO: STUB - Sound playback
  - TODO: SIMPLIFIED - Audio streaming
  - TODO: PLACEHOLDER - Spatial audio

- **File**: `src/Andastra/Runtime/Graphics/Stride/Audio/StrideVoicePlayer.cs`
  - TODO: PLACEHOLDER - SoundInstance (line 34)
  - TODO: PLACEHOLDER - Sound (line 35)
  - TODO: STUB - Stride audio playback (lines 77-79)
  - TODO: STUB - Sound instance management (line 86)
  - TODO: STUB - Stop sound (line 107)
  - TODO: STUB - Play state checking (line 121)
  - TODO: PLACEHOLDER - Playback position (lines 133-134)
  - TODO: STUB - Callback on stop (line 143)

### 4.3 Inventory & Items
- **File**: `src/Andastra/Runtime/Games/Odyssey/UI/OdysseyUpgradeScreen.cs`
  - TODO: STUB - UI rendering (lines 120, 134)
  - TODO: STUB - 2DA parsing (line 190)
  - TODO: STUB - Inventory integration (lines 251, 314)
  - TODO: STUB - Item stat calculation (line 268)

### 4.4 Module Transitions
- **File**: `src/Andastra/Runtime/Core/Module/ModuleTransitionSystem.cs`
  - TODO: STUB - Module transitions
  - TODO: STUB - Door triggers
  - TODO: PLACEHOLDER - Area transitions

### 4.5 Game Session
- **File**: `src/Andastra/Runtime/Games/Odyssey/Game/GameSession.cs`
  - TODO: STUB - Session management
  - TODO: STUB - State persistence
  - TODO: PLACEHOLDER - Session restoration

---

## Phase 5: Save/Load System

**Priority**: MEDIUM - Required for persistence  
**Goal**: Save and load games work correctly

### 5.1 Save System
- **File**: `src/Andastra/Runtime/Core/Save/SaveSystem.cs`
  - TODO: STUB - Save game creation
  - TODO: STUB - Save game loading
  - TODO: SIMPLIFIED - Save game listing
  - TODO: PLACEHOLDER - Save game deletion
  - TODO: STUB - Save game validation
  - TODO: SIMPLIFIED - Save game metadata

- **File**: `src/Andastra/Runtime/Games/Odyssey/Save/SaveGameManager.cs`
  - TODO: STUB - KOTOR2 save format
  - TODO: STUB - GFF serialization
  - TODO: PLACEHOLDER - Save compression
  - TODO: SIMPLIFIED - Save validation

- **File**: `src/Andastra/Runtime/Content/Save/SaveSerializer.cs`
  - TODO: STUB - Save serialization
  - TODO: STUB - Entity serialization
  - TODO: SIMPLIFIED - Global variable serialization
  - TODO: PLACEHOLDER - Quest state serialization
  - TODO: STUB - Party state serialization

---

## Phase 6: Polish & Optimization

**Priority**: LOW - Nice to have  
**Goal**: Performance improvements, advanced features

### 6.1 Rendering Optimization
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/GPUInstancing.cs`
  - TODO: STUB - GPU instancing
  - TODO: STUB - Instance batching
  - TODO: PLACEHOLDER - Instance culling

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/VertexCacheOptimizer.cs`
  - TODO: STUB - Vertex cache optimization
  - TODO: SIMPLIFIED - Index buffer optimization
  - TODO: PLACEHOLDER - Triangle strip generation

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Textures/TextureStreamingManager.cs`
  - TODO: STUB - Texture streaming
  - TODO: STUB - Mipmap streaming
  - TODO: PLACEHOLDER - Texture compression

### 6.2 Advanced Rendering
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Raytracing/NativeRaytracingSystem.cs`
  - TODO: STUB - Raytracing setup
  - TODO: STUB - Raytracing pipeline
  - TODO: SIMPLIFIED - Raytracing shaders
  - TODO: PLACEHOLDER - Raytracing acceleration structures
  - TODO: STUB - Raytracing integration

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Backends/BackendFactory.cs`
  - TODO: STUB - Backend factory
  - TODO: SIMPLIFIED - Backend selection
  - TODO: PLACEHOLDER - Backend switching

- **File**: `src/Andastra/Runtime/Graphics/Stride/Upscaling/StrideDlssSystem.cs`
  - TODO: STUB - DLSS integration
  - TODO: PLACEHOLDER - DLSS setup

### 6.3 Lighting & Effects
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Lighting/LightProbeSystem.cs`
  - TODO: STUB - Light probe system
  - TODO: STUB - Light probe baking
  - TODO: PLACEHOLDER - Light probe interpolation

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Lighting/ClusteredLightCulling.cs`
  - TODO: STUB - Clustered light culling
  - TODO: STUB - Light clustering
  - TODO: PLACEHOLDER - Light assignment

### 6.4 Audio System
- **File**: `src/Andastra/Runtime/Graphics/Stride/Graphics/StrideSpatialAudio.cs`
  - TODO: STUB - Spatial audio
  - TODO: STUB - Audio positioning
  - TODO: PLACEHOLDER - Audio occlusion

- **File**: `src/Andastra/Runtime/Graphics/Stride/Audio/StrideSoundPlayer.cs`
  - TODO: STUB - Stride sound player
  - TODO: STUB - Sound instance management
  - TODO: PLACEHOLDER - Sound effects

### 6.5 Game Loop
- **File**: `src/Andastra/Runtime/Core/GameLoop/FixedTimestepGameLoop.cs`
  - TODO: STUB - Fixed timestep implementation
  - TODO: STUB - Frame rate limiting
  - TODO: PLACEHOLDER - Variable timestep support

### 6.6 Utility Systems
- **File**: `src/Andastra/Utility/LZMA/LzmaHelper.cs`
  - TODO: PLACEHOLDER - LZMA support (lines 11-12)
  - TODO: STUB - LZMA decompression (line 21)
  - TODO: STUB - LZMA compression (line 27)

- **File**: `src/Andastra/Parsing/Resource/Formats/NCS/NCSBinaryReader.cs`
  - TODO: HACK - RESERVED opcode fallback (line 496)

---

## Phase 7: Non-Critical Systems (Can Defer)

**Priority**: LOWEST - Not required for playable demo  
**Goal**: Advanced features, other engine support

### 7.1 Other Engine Support (Aurora, Eclipse, Infinity)
- **File**: `src/Andastra/Runtime/Games/Aurora/EngineApi/AuroraEngineApi.cs`
  - TODO: STUB - Aurora engine API (lines 21, 34, 36, 41, 45, 57-58, 88)

- **File**: `src/Andastra/Runtime/Games/Eclipse/EngineApi/EclipseEngineApi.cs`
  - TODO: STUB - Eclipse engine API (multiple functions, lines 459-1333)

- **File**: `src/Andastra/Runtime/Games/Aurora/AuroraEngine.cs`
  - TODO: STUB - Aurora engine implementation

- **File**: `src/Andastra/Runtime/Games/Infinity/InfinityEngine.cs`
  - TODO: STUB - Infinity engine implementation

### 7.2 Stride Backend (Alternative to MonoGame)
- **File**: `src/Andastra/Runtime/Graphics/Stride/Graphics/StrideBasicEffect.cs`
  - TODO: STUB - Stride basic effect
  - TODO: PLACEHOLDER - Stride shader system

- **File**: `src/Andastra/Runtime/Graphics/Stride/Graphics/StrideRenderState.cs`
  - TODO: STUB - Stride render state
  - TODO: PLACEHOLDER - Stride state management

### 7.3 GUI Systems
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/GUI/MyraMenuRenderer.cs`
  - TODO: STUB - Myra menu rendering
  - TODO: STUB - GUI system integration

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/GUI/KotorGuiManager.cs`
  - TODO: STUB - KOTOR GUI manager
  - TODO: STUB - GUI widget system

### 7.4 Advanced Scene Systems
- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Scene/SceneBuilder.cs`
  - TODO: STUB - Scene building
  - TODO: STUB - Scene graph
  - TODO: PLACEHOLDER - Scene optimization

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Rendering/RenderTargetManager.cs`
  - TODO: STUB - Render target management
  - TODO: STUB - Render target pooling

- **File**: `src/Andastra/Runtime/Graphics/MonoGame/Graphics/MonoGameDepthStencilBuffer.cs`
  - TODO: STUB - Depth stencil buffer

---

## Implementation Strategy

### Phase Order (Critical Path)
1. **Phase 0** → **Phase 1** → **Phase 2** → **Phase 3** → **Phase 4** → **Phase 5**
2. **Phase 6** and **Phase 7** can be done in parallel or deferred

### Dependency Graph
```
Phase 0 (Foundation)
    ↓
Phase 1 (Rendering) ──→ Phase 2 (Movement)
    ↓                      ↓
Phase 3 (Interaction) ←───┘
    ↓
Phase 4 (Gameplay)
    ↓
Phase 5 (Save/Load)
```

### Critical Path TODOs (Minimum for Playable Demo)
1. Game initialization (Phase 0.1)
2. Resource loading (Phase 0.2)
3. Area rendering (Phase 1.1)
4. Player movement (Phase 2.1)
5. Pathfinding (Phase 2.2)
6. Entity spawning (Phase 3.1)
7. Basic script execution (Phase 3.4)
8. Critical engine API functions (Phase 3.5)

### Estimated TODO Count by Phase
- **Phase 0**: ~45 TODOs (Critical Foundation)
- **Phase 1**: ~35 TODOs (Basic Rendering)
- **Phase 2**: ~15 TODOs (Movement & Navigation)
- **Phase 3**: ~50 TODOs (Basic Interaction)
- **Phase 4**: ~30 TODOs (Core Gameplay)
- **Phase 5**: ~15 TODOs (Save/Load)
- **Phase 6**: ~40 TODOs (Polish & Optimization)
- **Phase 7**: ~82 TODOs (Non-Critical Systems)

**Total**: 312 TODOs

---

## Success Criteria for Playable Demo

### Minimum Viable Demo
- [ ] Game launches without crashes
- [ ] Module loads (area visible)
- [ ] Player character spawns
- [ ] Player can move via click-to-move
- [ ] Pathfinding works (can navigate around obstacles)
- [ ] Basic entities visible (NPCs, doors, placeables)
- [ ] Camera follows player
- [ ] Basic scripts execute (area entry, triggers)
- [ ] Can transition between areas
- [ ] Game can be saved and loaded

### Enhanced Demo (Stretch Goals)
- [ ] Combat system functional
- [ ] Dialogue system works
- [ ] Inventory accessible
- [ ] Party members follow
- [ ] Basic AI behavior
- [ ] Sound effects play
- [ ] Voice-over in dialogue

---

## Notes

- **Focus**: KOTOR2 (The Sith Lords) only - other engines can be deferred
- **Backend**: MonoGame is primary - Stride backend can be deferred
- **Performance**: Basic optimization only - advanced features in Phase 6
- **Testing**: Each TODO resolution should include appropriate testing
- **Documentation**: Follow .cursorrules for Ghidra documentation and code comments

---

## Progress Tracking

As TODOs are resolved, update this document to track progress:
- Mark completed phases with [x]
- Update TODO counts
- Note any blockers or dependencies discovered

