# Odyssey Engine Roadmap

A clean-room reimplementation of BioWare's Odyssey Engine (KOTOR 1/2) using Stride Game Engine and C#.

## Project Vision

Create a 100% faithful recreation of the Odyssey engine capable of running KOTOR 1 and KOTOR 2, with an architecture that supports future expansion to other Aurora/Eclipse family engines (NWN, Jade Empire, Dragon Age).

## Architecture Overview

### Project Structure

```
src/OdysseyRuntime/
├── Odyssey.Core/       # Pure domain logic, no rendering dependencies
├── Odyssey.Content/    # Asset conversion/caching pipeline
├── Odyssey.Scripting/  # NCS VM + NWScript engine API
├── Odyssey.Kotor/      # K1/K2-specific rules and data
├── Odyssey.Stride/     # Stride rendering/audio/input adapters
├── Odyssey.Game/       # Executable launcher
├── Odyssey.Tooling/    # CLI tools for validation/import
└── Odyssey.Tests/      # Unit and integration tests
```

### Engine Abstraction (Future Aurora/Eclipse Support)

The architecture separates engine-agnostic systems from game-specific implementations:

| Layer | Description | Examples |
|-------|-------------|----------|
| `Odyssey.Core` | Abstract engine interfaces | `IWorld`, `IEntity`, `INcsVm`, `IResourceProvider` |
| `Odyssey.Kotor` | KOTOR-specific implementations | Combat rules, 2DA bindings, game constants |
| `Odyssey.Nwn` (future) | NWN-specific implementations | Different combat rules, tileset system |
| `Odyssey.JadeEmpire` (future) | Jade Empire specifics | Real-time combat, chi system |

## Implementation Phases

### Phase 0: Foundation (Complete)
- [x] Project structure setup
- [x] Entity-component system (`World`, `Entity`, components)
- [x] NCS VM core (instruction dispatch, stack operations)
- [x] Navigation mesh with A* pathfinding
- [x] Resource provider interfaces
- [x] Action queue system

### Phase 1: Core Runtime
Current focus for achieving first playable module.

#### 1.1 Resource System
- [ ] `GameResourceProvider` with full precedence chain:
  - Override → Module → Save → TexturePacks → Chitin
- [ ] Archive caching (LRU eviction for ERF/RIM/BIF handles)
- [ ] Async resource streaming
- [ ] Save overlay integration

#### 1.2 Module Loading Pipeline
- [ ] IFO parser (module metadata)
- [ ] ARE parser (area properties, ambient settings)
- [ ] GIT parser (entity spawn lists)
- [ ] LYT parser (room layout, doorhooks)
- [ ] VIS parser (room visibility graph)
- [ ] PTH parser (path points)

#### 1.3 Entity Templates
- [ ] UTC → Creature template
- [ ] UTP → Placeable template
- [ ] UTD → Door template
- [ ] UTT → Trigger template
- [ ] UTW → Waypoint template
- [ ] UTS → Sound template
- [ ] UTE → Encounter template

#### 1.4 Scene Assembly
- [ ] Room instantiation from LYT
- [ ] Entity spawning from GIT
- [ ] Doorhook placement
- [ ] VIS culling groups
- [ ] Material assignment

### Phase 2: Player Interaction

#### 2.1 Character Controller
- [ ] Walkmesh projection and movement
- [ ] Click-to-move input handling
- [ ] Pathfinding integration
- [ ] Door/trigger intersection detection
- [ ] Ledge and ramp handling

#### 2.2 Camera System
- [ ] Chase camera (KOTOR-style)
- [ ] Free camera (debug)
- [ ] Dialogue camera
- [ ] Camera collision avoidance
- [ ] Cinematic camera scripting

#### 2.3 Object Interaction
- [ ] Click-to-select entities
- [ ] Use/interact actions
- [ ] Door open/close states
- [ ] Placeable activation
- [ ] Trigger enter/exit events

### Phase 3: Scripting Completion

#### 3.1 NCS VM Enhancements
- [ ] Full string handling (proper string pool)
- [ ] STORE_STATE for delayed actions
- [ ] Script debugging and tracing
- [ ] Error isolation (per-script)

#### 3.2 Engine API Surface
- [ ] Core object functions (~100 functions)
- [ ] Area/module functions
- [ ] Effect system functions
- [ ] Combat functions
- [ ] Dialogue functions
- [ ] 2DA lookup functions
- [ ] K1 profile (~850 total)
- [ ] K2 profile (~950 total)

#### 3.3 Script Events
- [ ] OnSpawn
- [ ] OnHeartbeat
- [ ] OnPerception
- [ ] OnUserDefined
- [ ] OnDamaged
- [ ] OnDeath
- [ ] OnConversation
- [ ] OnUsed
- [ ] OnEnter/OnExit

### Phase 4: Dialogue System

#### 4.1 DLG Playback
- [ ] Conversation graph traversal
- [ ] Conditional script evaluation
- [ ] Entry/reply script hooks
- [ ] Node script execution

#### 4.2 Localization
- [ ] TLK string lookup
- [ ] StrRef resolution
- [ ] Language support

#### 4.3 Voice/Lipsync
- [ ] VO playback from StreamVoice/StreamWaves
- [ ] LIP keyframe interpolation
- [ ] Facial animation blending
- [ ] Camera transitions (speaker/listener)

### Phase 5: Combat System

#### 5.1 Combat Resolution
- [ ] D20 attack rolls
- [ ] Defense calculations
- [ ] Damage application
- [ ] Critical hits
- [ ] Saving throws

#### 5.2 Combat Round System
- [ ] 3-second round timing
- [ ] Attack queue management
- [ ] Animation synchronization
- [ ] Projectile handling

#### 5.3 Effect System
- [ ] Buff/debuff application
- [ ] Duration tracking
- [ ] Effect stacking rules
- [ ] Visual effect spawning

#### 5.4 AI System
- [ ] Perception (sight/hearing)
- [ ] Faction hostility checks
- [ ] Combat AI patterns
- [ ] Heartbeat AI decisions

### Phase 6: Party System

#### 6.1 Party Management
- [ ] Party member addition/removal
- [ ] Party member selection
- [ ] Leader switching
- [ ] Party inventory

#### 6.2 Follower AI
- [ ] Follow behavior
- [ ] Formation positioning
- [ ] Combat assistance
- [ ] Script hooks

### Phase 7: Save/Load System

#### 7.1 State Serialization
- [ ] Global variables
- [ ] Local variables per object
- [ ] Party state
- [ ] Inventory state
- [ ] Module state (entity positions, door states)

#### 7.2 SAV Format
- [ ] SAV file reading
- [ ] SAV file writing
- [ ] Resource overlay integration
- [ ] Autosave support

### Phase 8: Content Pipeline

#### 8.1 Texture Conversion
- [ ] TPC → Stride.Texture
- [ ] TGA → Stride.Texture
- [ ] TXI material metadata
- [ ] Mipmap generation
- [ ] Normal map handling

#### 8.2 Model Conversion
- [ ] MDL/MDX → Stride.Model
- [ ] Geometry and skinning
- [ ] Bone hierarchy
- [ ] Animation keyframes
- [ ] Attachment points

#### 8.3 Audio Conversion
- [ ] WAV decoding
- [ ] Spatial audio positioning
- [ ] Music streaming
- [ ] Ambient sound zones

#### 8.4 Caching
- [ ] Hash-keyed content cache
- [ ] Dependency tracking
- [ ] Cache invalidation
- [ ] Memory pressure management

### Phase 9: Rendering

#### 9.1 Material System
- [ ] Lightmap application
- [ ] Environment maps
- [ ] Alpha blending
- [ ] Additive transparency
- [ ] Two-sided materials
- [ ] Cutout/alpha test

#### 9.2 Scene Rendering
- [ ] Room mesh rendering
- [ ] VIS-based culling
- [ ] Transparency sorting
- [ ] Dynamic lighting
- [ ] Particle effects

#### 9.3 Character Rendering
- [ ] Skeletal animation
- [ ] Animation blending
- [ ] Equipment attachment
- [ ] Facial animation

### Phase 10: UI System

#### 10.1 In-Game UI
- [ ] Dialogue panel
- [ ] HUD (health, party)
- [ ] Action bar
- [ ] Target reticle

#### 10.2 Menus
- [ ] Pause menu
- [ ] Inventory
- [ ] Character sheet
- [ ] Journal
- [ ] Map
- [ ] Options

#### 10.3 Loading
- [ ] Loading screen
- [ ] Progress indication
- [ ] Area transition effects

## File Format Support

### Fully Implemented (in CSharpKOTOR)
| Format | Description | Status |
|--------|-------------|--------|
| GFF | Generic file format (templates) | ✅ Complete |
| ERF | Module/save archives | ✅ Complete |
| RIM | Read-only module archives | ✅ Complete |
| KEY/BIF | Installation archives | ✅ Complete |
| TLK | Talk table (localization) | ✅ Complete |
| 2DA | Data tables | ✅ Complete |
| TPC | Compressed textures | ✅ Complete |
| TGA | Uncompressed textures | ✅ Complete |
| TXI | Texture metadata | ✅ Complete |
| NCS | Compiled scripts | ✅ Complete |
| BWM | Walkmesh (binary) | ✅ Complete |
| LYT | Layout (ASCII) | ✅ Complete |
| VIS | Visibility (ASCII) | ✅ Complete |
| LIP | Lipsync animation | ✅ Complete |
| LTR | Letter combo probabilities | ✅ Complete |
| SSF | Sound set | ✅ Complete |
| WAV | Audio | ✅ Complete |

### Needs Runtime Integration
| Format | Description | Priority |
|--------|-------------|----------|
| MDL/MDX | Models/animations | High |
| DLG | Dialogue trees | High |
| UTC/UTP/UTD/etc | Entity templates | High |
| ARE | Area properties | High |
| GIT | Instance lists | High |
| IFO | Module info | High |
| PTH | Pathfinding points | Medium |
| GUI | User interface | Medium |
| JRL | Journal | Low |
| PTM/PTT | Plot manager | Low |

## Aurora/Eclipse Engine Family

### Supported (Current)
- **Odyssey Engine**: KOTOR 1 (2003), KOTOR 2 (2004)

### Planned (Future)
- **Aurora Engine**: Neverwinter Nights (2002)
- **Electron Engine**: Jade Empire (2005)
- **Eclipse Engine**: Dragon Age: Origins (2009), Dragon Age 2 (2011)

### Abstraction Points
| System | Odyssey | Aurora | Electron | Eclipse |
|--------|---------|--------|----------|---------|
| Combat | D20/turn-based | D20/real-time | Real-time martial | Real-time tactical |
| Scripting | NWScript | NWScript | NWScript | Lua-based |
| Resources | KEY/BIF/ERF | KEY/BIF/ERF | KEY/BIF/ERF | DADB |
| Models | MDL/MDX | MDL/MDX | MDL/MDX | MMH/MSH |

## Quality Targets

### Faithfulness
- Identical script behavior to original
- Matching resource precedence rules
- Compatible with existing mods
- Same visual appearance (materials, lighting)

### Performance
- 60 FPS on mid-range hardware
- Background asset streaming
- No loading stutters
- Memory-efficient caching

### Stability
- Graceful script error handling
- Save game integrity
- Clean shutdown/restart

## Reference Documentation

### Primary Sources
- `vendor/PyKotor/wiki/` - File format specifications
- `vendor/PyKotor/vendor/xoreos-docs/` - BioWare specifications
- In-game observation and testing

### Implementation References
- `.cursor/plans/stride_odyssey_engine_e8927e4a.plan.md` - Detailed implementation plan
- `src/CSharpKOTOR/` - Format readers/writers
- Original game behavior (via Ghidra analysis when needed)

## Development Guidelines

### Clean-Room Process
1. Derive behavior from specs and observation
2. Do not copy code from other implementations
3. Document behavioral decisions
4. Test against original game behavior

### Code Standards
- C# 7.3 compatible (no nullable reference types)
- Follow existing CSharpKOTOR conventions
- Add XML documentation for public APIs
- Unit test all critical paths

### Commit Guidelines
- Use conventional commits format
- Stage files individually (not `git add .`)
- Reference relevant spec sections in commits
