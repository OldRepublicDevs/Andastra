# Odyssey Engine Roadmap

A 100% faithful recreation of the Odyssey engine (KotOR 1/2), with future extensibility for Aurora/Eclipse engines (unified abstraction similar to xoreos).

## Architecture Overview

### Engine Family Abstraction

```
Odyssey Engine Family
├── Aurora Engine (NWN)
├── Odyssey Engine (KotOR 1/2)
├── Eclipse Engine (Jade Empire)
└── Unreal 3 Aurora (Mass Effect)
```

### Project Structure

```
src/OdysseyRuntime/
├── Odyssey.Core/          # Pure domain, no Stride dependency
│   ├── Entities/          # Entity/component system
│   ├── Actions/           # Action queue, delay scheduler
│   ├── Navigation/        # Walkmesh pathfinding
│   ├── Module/            # Runtime area/module abstractions
│   └── Interfaces/        # Core contracts
├── Odyssey.Content/       # Asset conversion/caching pipeline
│   ├── Cache/             # Content caching with hash keys
│   ├── Converters/        # TPC, MDL, BWM converters
│   └── ResourceProviders/ # Virtual file system
├── Odyssey.Scripting/     # NCS VM + NWScript engine API
│   ├── VM/                # Stack-based bytecode VM
│   ├── EngineApi/         # Engine function dispatch
│   └── Interfaces/        # Script contracts
├── Odyssey.Kotor/         # K1/K2 rule modules, gameplay systems
│   ├── Rules/             # D20 combat, feats, force powers
│   ├── Dialogue/          # DLG traversal, TLK lookup
│   └── Save/              # Save/load system
├── Odyssey.Stride/        # Stride adapters (rendering, physics, audio, UI)
│   ├── Rendering/         # Scene assembly, materials
│   ├── Materials/         # Lightmaps, transparency
│   └── Lighting/          # Dynamic lights, effects
├── Odyssey.Game/          # Stride executable/launcher
├── Odyssey.Tests/         # Deterministic tests
└── Odyssey.Tooling/       # Headless import/validation commands
```

### Layered Architecture

1. **Data/Formats Layer (CSharpKOTOR)**: File format parsing, installation scanning, resource management
2. **Runtime Domain Layer (Odyssey.Core)**: Game-agnostic runtime concepts (entities, components, world state, events)
3. **Stride Integration Layer (Odyssey.Stride)**: Rendering, physics, audio, UI adapters
4. **Game Rules Layer (Odyssey.Kotor)**: K1/K2-specific rulesets, 2DA-driven data

---

## Implementation Phases

### Phase 0: Foundation ✅ COMPLETE

- [x] Project structure created
- [x] C# 7.3 language version enforced
- [x] Core interfaces defined (IWorld, IEntity, INavigationMesh)
- [x] Entity/component system basics (Entity, World, EventBus)
- [x] Action system (ActionQueue, ActionBase, DelayScheduler)
- [x] Basic Stride project scaffolding

### Phase 1: NCS Virtual Machine ✅ COMPLETE

- [x] NCS bytecode parser with header validation (`"NCS V1.0"`, `0x42` marker)
- [x] Stack-based VM with 4-byte alignment
- [x] All core opcodes implemented:
  - Arithmetic (ADD/SUB/MUL/DIV for II/IF/FI/FF/VV/VF)
  - Comparisons (EQ/NEQ/GT/LT/GEQ/LEQ)
  - Logical (LOGAND/LOGOR/NOT)
  - Bitwise (INCOR/EXCOR/BOOLAND/SHLEFT/SHRIGHT)
  - Flow control (JMP/JSR/JZ/JNZ/RETN)
  - Stack operations (CPDOWNSP/CPTOPSP/MOVSP/DESTRUCT)
  - Base pointer (SAVEBP/RESTOREBP/CPTOPBP/CPDOWNBP)
  - Constants (CONSTI/CONSTF/CONSTS/CONSTO)
  - Reserve space (RSADDI/RSADDF/RSADDS/RSADDO)
- [x] Engine function dispatch interface (ACTION opcode)
- [x] Base engine API structure (K1EngineApi)
- [ ] Complete engine function surface (~850 K1, ~950 K2)
- [ ] Script globals/locals persistence
- [ ] Action queue integration with STORE_STATE

### Phase 2: Navigation & Walkmesh ✅ COMPLETE

- [x] NavigationMesh with full A* pathfinding
- [x] AABB tree for spatial queries
- [x] Adjacency-based pathfinding (face index * 3 + edge encoding)
- [x] Surface material walkability rules (from surfacemat.2da semantics)
- [x] Raycast for click-to-move
- [x] Line-of-sight testing
- [x] Surface projection (height interpolation)
- [x] Path smoothing
- [ ] Integration with CSharpKOTOR BWM parser

### Phase 3: Resource System 🔄 IN PROGRESS

- [x] Resource provider interface (IGameResourceProvider)
- [x] Resource identifier system
- [x] Content cache structure
- [ ] Full precedence chain: override → module → save → chitin
- [ ] Async resource streaming with cancellation
- [ ] Resource caching with LRU eviction
- [ ] Save overlay integration

### Phase 4: Module Loading 📋 PLANNED

- [ ] IFO parsing (module metadata, scripts, variables)
- [ ] ARE parsing (area properties, lighting, weather)
- [ ] GIT parsing (instance spawning: creatures, placeables, doors, triggers)
- [ ] LYT parsing (room layout, doorhooks)
- [ ] VIS parsing (room visibility culling)
- [ ] PTH parsing (path network for AI)
- [ ] Module transition system

### Phase 5: Entity System 📋 PLANNED

**Object Type Hierarchy**:
```
Object (abstract base)
├── Creature
│   ├── PC (player-controlled)
│   └── NPC (AI-controlled)
├── Door
├── Placeable
├── Trigger (invisible volume)
├── Waypoint (invisible marker)
├── Sound (ambient emitter)
├── Store (merchant)
├── Encounter (spawn point)
├── Item (world-dropped or inventory)
└── AreaOfEffect (spell zones)
```

**GFF Template Loading**:
- [ ] UTC → Creature (Appearance_Type, Faction, HP, Attributes, Feats, Scripts)
- [ ] UTP → Placeable (Appearance, Useable, Locked, OnUsed)
- [ ] UTD → Door (GenericType, Locked, OnOpen, OnClose)
- [ ] UTT → Trigger (Geometry polygon, OnEnter, OnExit)
- [ ] UTW → Waypoint (Tag, position)
- [ ] UTS → Sound (Active, Looping, Positional, ResRef)
- [ ] UTE → Encounter (Creature list, spawn conditions)
- [ ] UTI → Item (BaseItem, Properties, Charges)

### Phase 6: Rendering 📋 PLANNED

- [ ] MDL/MDX model loading and conversion to Stride
- [ ] TPC/TGA texture loading with alpha handling
- [ ] TXI material metadata parsing
- [ ] Room scene assembly from LYT
- [ ] VIS-based culling groups
- [ ] Material system (Opaque, AlphaCutout, AlphaBlend, Additive, Lightmapped)
- [ ] Transparency sorting
- [ ] Skeletal animation
- [ ] Attachment nodes (weapons, effects)
- [ ] Environment mapping

### Phase 7: NWScript Engine API 📋 PLANNED

**Coverage Tiers**:
- **Tier 0**: Boot + area + movement + interaction + dialogue entry
- **Tier 1**: Combat core + inventory + party management essentials
- **Tier 2**: Quests, journals, influence, minigames, full AI
- **Tier 3**: Edge features and obscure calls

**Core Function Groups** (implement in order):
1. Object functions (GetObjectByTag, GetNearestCreature, etc.)
2. Position/location functions (GetPosition, Location, etc.)
3. Area/module functions (GetArea, GetModule, etc.)
4. Action functions (ActionMoveToLocation, ActionAttack, etc.)
5. Effect functions (EffectDamage, ApplyEffectToObject, etc.)
6. Conversation functions (ActionStartConversation, etc.)
7. Combat functions (GetAttackTarget, etc.)
8. Variable functions (GetLocalInt, SetGlobalBoolean, etc.)
9. Party functions (GetPartyMemberByIndex, etc.)
10. Utility functions (PrintString, Random, etc.)

### Phase 8: Dialogue System 📋 PLANNED

- [ ] DLG file traversal (entries, replies, conditions)
- [ ] TLK localized text lookup
- [ ] Conditional script evaluation
- [ ] VO playback with timing
- [ ] LIP sync (phoneme shapes to facial animation)
- [ ] Dialogue camera positioning
- [ ] Script hooks (OnEntry, OnReply)

### Phase 9: Combat System 📋 PLANNED

**Round-Based Combat (~3 second rounds)**:
```
Starting     → 0.0s - init animations
FirstAttack  → ~0.5s - primary attack
SecondAttack → ~1.5s - offhand/counter (if duel)
Cooldown     → ~2.5s - return to ready
Finished     → 3.0s - complete
```

- [ ] D20 attack resolution (roll + modifiers vs AC)
- [ ] Damage calculation with resistances
- [ ] Critical hits (threat range, confirmation)
- [ ] Effect system (~60+ effect types)
- [ ] Perception system (sight/hearing ranges)
- [ ] Faction hostility checks
- [ ] AI behavior (action queue population)

### Phase 10: Save/Load System 📋 PLANNED

- [ ] SaveModel definition (globals, party, inventory, quests)
- [ ] Module state serialization (positions, door states, triggers)
- [ ] Save file packaging (ERF/SAV compatible)
- [ ] Load with state restoration
- [ ] Module transition state preservation

---

## Game Loop Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                       Fixed-Timestep Game Loop                   │
├─────────────────────────────────────────────────────────────────┤
│ 1. Input Phase     │ Collect input, update camera, click-to-move │
│ 2. Script Phase    │ Process delay wheel, heartbeats, actions    │
│ 3. Simulation      │ Update positions, perception, combat rounds │
│ 4. Animation       │ Skeletal animations, particles, lip sync    │
│ 5. Scene Sync      │ Sync runtime transforms → Stride scene      │
│ 6. Render Phase    │ VIS culling, transparency sort, draw calls  │
│ 7. Audio Phase     │ Spatial audio, trigger one-shots            │
└─────────────────────────────────────────────────────────────────┘
```

---

## Key Specifications

### NCS Bytecode Format

- **Header**: 13 bytes (`"NCS V1.0"` + `0x42` marker + big-endian file size)
- **Instructions**: `<opcode:uint8><qualifier:uint8><args...>`
- **Stack**: 4-byte aligned, vectors = 12 bytes (3 floats)
- **Jump offsets**: Relative from instruction start (not next instruction)
- **Engine calls**: `ACTION` with `uint16 routineId` + `uint8 argCount`

### BWM Walkmesh Format

- **Types**: WOK (area), PWK (placeable), DWK (door)
- **Adjacency encoding**: `adjacency = faceIndex * 3 + edgeIndex`
- **Surface materials**: Walkability from surfacemat.2da
- **AABB tree**: 44-byte nodes for spatial acceleration

### Resource Precedence

```
Pre-save:  override → module archives → chitin (KEY/BIF) → defaults
Post-save: override → module archives → save overlay → chitin → defaults
```

---

## Design Principles

1. **100% Faithfulness**: Match original engine behavior exactly
2. **Clean-Room**: Behavioral specs + observation, no code copying
3. **Modularity**: Clear separation for Aurora/Eclipse extensibility
4. **C# 7.3 Compatibility**: Maintain .NET Framework 4.x support
5. **Commercial-Friendly**: Permissive dependencies only
6. **No Asset Shipping**: User-provided installation required

---

## 2DA Tables Reference

| Table | Purpose |
|-------|---------|
| `appearance.2da` | Model resref, walk/run speed, body type |
| `heads.2da` | Head model by race/gender |
| `baseitems.2da` | Item categories, damage, properties |
| `feat.2da` | Feat definitions, prerequisites |
| `spells.2da` | Force powers, ranges, effects |
| `classes.2da` | Class progression, hit dice, saves |
| `skills.2da` | Skill definitions |
| `surfacemat.2da` | Surface walkability, footstep sounds |
| `portraits.2da` | Portrait images |
| `placeables.2da` | Placeable appearance |
| `genericdoors.2da` | Door models |
| `ambientmusic.2da` | Music tracks |
| `ambientsound.2da` | Ambient sounds |

---

## Performance Budgets

- **Module load (warm)**: < 5 seconds
- **Module load (cold)**: Progress bar, cache building acceptable
- **Frame time**: Stable 60fps on midrange GPU
- **Script budget**: Capped per frame with spillover queue
- **Memory**: Bounded caches with LRU eviction

---

## Testing Strategy

### Unit Tests (Fast)
- NCS VM instruction semantics
- Engine function pure tests
- Resource precedence with synthetic fixtures

### Golden-File Tests (Content)
- Parse MDL/TPC/BWM/GFF and assert properties
- Synthetic assets (not proprietary)

### Integration Tests (Manual + Scripted)
- Module boot scripts
- Door transitions
- Dialogue flows
- Vertical slice checklist

---

## Future Engine Profiles

```csharp
// Placeholder structure for engine family
namespace Odyssey.Profiles
{
    public interface IGameProfile
    {
        string GameId { get; }
        int ScriptFunctionCount { get; }
        IResourceLayout ResourceLayout { get; }
        IRuleSet RuleSet { get; }
    }
    
    // Implemented
    public class Kotor1Profile : IGameProfile { }
    public class Kotor2Profile : IGameProfile { }
    
    // Future placeholders
    public class NwnProfile : IGameProfile { }        // Aurora
    public class JadeEmpireProfile : IGameProfile { } // Eclipse
    public class MassEffectProfile : IGameProfile { } // Unreal 3 Aurora
}
```

---

## Ghidra MCP Integration

Engine-related code uses Ghidra MCP server with `swkotor2.exe` loaded for:
- Understanding original engine mechanics
- Verifying faithful recreation
- Discovering undocumented behavior

Search patterns:
- File format strings: `"BWM "`, `"NCS "`, `"ERF "`
- Function names: `LoadModule`, `ExecuteScript`, `Pathfind`
- Error messages for behavior clues

---

## Definition of Done

The runtime is "playable KOTOR" when:

- [ ] **Boot**: User selects K1/K2 install; engine validates; loads module
- [ ] **Render**: Module rooms render with lightmaps/materials; 60fps
- [ ] **Move**: Player moves on walkmesh; blocked regions work; camera follows
- [ ] **Interact**: Click/use doors and placeables; triggers fire scripts
- [ ] **Script**: NWScript VM runs area scripts; major engine-call coverage
- [ ] **Dialogue**: Complete conversation with branching, VO, lip sync
- [ ] **Combat**: Basic combat encounter playable end-to-end
- [ ] **Save/Load**: Save and reload with core state preserved
- [ ] **Mod compatibility**: Override/module/chitin precedence correct

---

*Last updated: December 2024*
