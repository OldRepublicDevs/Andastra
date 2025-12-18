# Andastra Codebase Audit

**Status**: 🔴 CRITICAL ISSUES FOUND
**Date**: 2025-01-16
**Purpose**: Comprehensive audit of all Runtime/Games implementations to ensure proper cross-engine support

## Critical Issues Found

### 1. ⚠️ DUPLICATE EngineApi Files (URGENT)

**Problem**: 4 EngineApi implementations for Odyssey, unclear which are canonical

**Files**:

- `Odyssey/EngineApi/K1EngineApi.cs` (7053 lines)
- `Odyssey/EngineApi/K2EngineApi.cs`
- `Odyssey/EngineApi/OdysseyK1EngineApi.cs`
- `Odyssey/EngineApi/OdysseyK2EngineApi.cs`

**Analysis Required**:

- Are K1/K2 old versions and OdysseyK1/OdysseyK2 new versions?
- Which should be kept?
- Need to consolidate and delete duplicates

**Action**: Determine canonical files and delete duplicates

### 2. ⚠️ MASSIVE Odyssey Implementation vs Empty Eclipse/Aurora

**Odyssey Has** (100+ files):

- ✅ Combat system (4 files: CombatManager, CombatRound, DamageCalculator, WeaponDamageCalculator)
- ✅ Components (19 files: ActionQueue, Animation, Creature, Door, Encounter, Faction, Inventory, Item, Perception, Placeable, QuickSlot, Renderable, ScriptHooks, Sound, Stats, Store, Transform, Trigger, Waypoint)
- ✅ Dialogue system (5 files: ConversationContext, DialogueManager, DialogueState, KotorDialogueLoader, KotorLipDataLoader)
- ✅ Data management (2 files: GameDataManager, TwoDATableManager)
- ✅ Game systems (5 files: GameSession, ModuleLoader, ModuleTransitionSystem, PlayerController, ScriptExecutor)
- ✅ Loading system (4 files: EntityFactory, KotorModuleLoader, ModuleLoader, NavigationMeshFactory)
- ✅ Systems (10 files: AIController, ComponentInitializer, EncounterSystem, FactionManager, HeartbeatSystem, ModelResolver, PartyManager, PerceptionManager, StoreSystem, TriggerSystem)
- ✅ Templates (18 files: UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW + Helpers)
- ✅ Profiles (6 files: GameProfileFactory, IGameProfile, K1GameProfile, K2GameProfile, OdysseyK1GameProfile, OdysseyK2GameProfile)
- ✅ Save system (1 file: SaveGameManager)

**Eclipse Has** (12 files):

- ✅ Engine base (EclipseEngine, EclipseGameSession, EclipseModuleLoader)
- ✅ Game-specific engines (4 subdirectories: DragonAgeOrigins, DragonAge2, MassEffect, MassEffect2)
- ✅ Save serializer base (EclipseSaveSerializer)
- ❌ NO Combat system
- ❌ NO Components
- ❌ NO Dialogue system  
- ❌ NO Data management
- ❌ NO Systems (AI, Encounter, Faction, Party, Perception, etc.)
- ❌ NO Templates
- ❌ NO Profiles
- ❌ NO Loading utilities

**Aurora Has** (1 file):

- ✅ Engine base (AuroraEngine)
- ❌ NO anything else

**Infinity Has** (1 file):

- ✅ Engine base (InfinityEngine)
- ❌ NO anything else

### 3. ⚠️ No Base Classes in Common

**Problem**: Odyssey implementations don't have proper base classes in Common for Eclipse/Aurora to inherit from

**Missing Base Classes**:

- ❌ BaseDialogueSystem (Odyssey has DialogueManager, but no base class)
- ❌ BaseCombatSystem (Odyssey has CombatManager, but no base class)
- ❌ BasePartyManager
- ❌ BasePerceptionManager
- ❌ BaseFactionManager
- ❌ BaseEncounterSystem
- ❌ BaseAIController
- ❌ BaseStoreSystem
- ❌ BaseTriggerSystem
- ❌ BaseHeartbeatSystem
- ❌ BaseEntityFactory
- ❌ BaseNavigationMeshFactory
- ❌ BaseGameDataManager
- ❌ BaseTwoDATableManager
- ❌ All component base classes

**Existing Base Classes** (Good):

- ✅ BaseEngine
- ✅ BaseEngineGame
- ✅ BaseEngineModule
- ✅ BaseEngineProfile

## Complete File Inventory

### Runtime/Games/Odyssey (100+ files)

#### Combat (4 files)

- CombatManager.cs
- CombatRound.cs
- DamageCalculator.cs
- WeaponDamageCalculator.cs

#### Components (19 files)

- ActionQueueComponent.cs
- AnimationComponent.cs
- CreatureComponent.cs
- DoorComponent.cs
- EncounterComponent.cs
- FactionComponent.cs
- InventoryComponent.cs
- ItemComponent.cs
- PerceptionComponent.cs
- PlaceableComponent.cs
- QuickSlotComponent.cs
- RenderableComponent.cs
- ScriptHooksComponent.cs
- SoundComponent.cs
- StatsComponent.cs
- StoreComponent.cs
- TransformComponent.cs
- TriggerComponent.cs
- WaypointComponent.cs

#### Dialogue (5 files)

- ConversationContext.cs
- DialogueManager.cs
- DialogueState.cs
- KotorDialogueLoader.cs
- KotorLipDataLoader.cs

#### EngineApi (4 files - DUPLICATES)

- K1EngineApi.cs (7053 lines)
- K2EngineApi.cs
- OdysseyK1EngineApi.cs
- OdysseyK2EngineApi.cs

#### Data (2 files)

- GameDataManager.cs
- TwoDATableManager.cs

#### Game (5 files)

- GameSession.cs
- ModuleLoader.cs
- ModuleTransitionSystem.cs
- PlayerController.cs
- ScriptExecutor.cs

#### Input (1 file)

- PlayerController.cs

#### Loading (4 files)

- EntityFactory.cs
- KotorModuleLoader.cs
- ModuleLoader.cs
- NavigationMeshFactory.cs

#### Profiles (6 files - DUPLICATES?)

- GameProfileFactory.cs
- IGameProfile.cs
- K1GameProfile.cs
- K2GameProfile.cs
- OdysseyK1GameProfile.cs
- OdysseyK2GameProfile.cs

#### Save (1 file)

- SaveGameManager.cs

#### Systems (10 files)

- AIController.cs
- ComponentInitializer.cs
- EncounterSystem.cs
- FactionManager.cs
- HeartbeatSystem.cs
- ModelResolver.cs
- PartyManager.cs
- PerceptionManager.cs
- StoreSystem.cs
- TriggerSystem.cs

#### Templates (18 files)

- UTC.cs + UTCHelpers.cs (Creature templates)
- UTD.cs + UTDHelpers.cs (Door templates)
- UTE.cs + UTEHelpers.cs (Encounter templates)
- UTI.cs + UTIHelpers.cs (Item templates)
- UTM.cs + UTMHelpers.cs (Merchant templates)
- UTP.cs + UTPHelpers.cs (Placeable templates)
- UTS.cs + UTSHelpers.cs (Sound templates)
- UTT.cs + UTTHelpers.cs (Trigger templates)
- UTW.cs + UTWHelpers.cs (Waypoint templates)

#### Root (3 files)

- OdysseyEngine.cs
- OdysseyGameSession.cs
- OdysseyModuleLoader.cs

### Runtime/Games/Eclipse (12 files)

#### DragonAgeOrigins (3 files)

- DragonAgeOriginsEngine.cs
- DragonAgeOriginsGameSession.cs
- DragonAgeOriginsModuleLoader.cs

#### DragonAge2 (3 files)

- DragonAge2Engine.cs
- DragonAge2GameSession.cs
- DragonAge2ModuleLoader.cs

#### MassEffect (3 files)

- MassEffectEngine.cs
- MassEffectGameSession.cs
- MassEffectModuleLoader.cs

#### MassEffect2 (3 files)

- MassEffect2Engine.cs
- MassEffect2GameSession.cs
- MassEffect2ModuleLoader.cs

#### Save (1 file)

- EclipseSaveSerializer.cs

#### Root (3 files)

- EclipseEngine.cs
- EclipseGameSession.cs
- EclipseModuleLoader.cs

### Runtime/Games/Aurora (1 file)

- AuroraEngine.cs

### Runtime/Games/Infinity (1 file)

- InfinityEngine.cs

### Runtime/Games/Common (8 files)

- BaseEngine.cs
- BaseEngineGame.cs
- BaseEngineModule.cs
- BaseEngineProfile.cs
- IEngine.cs
- IEngineGame.cs
- IEngineModule.cs
- IEngineProfile.cs

## Required Actions

### Priority 1: Fix Duplication Issues

1. **Determine canonical EngineApi files**
   - Investigate K1EngineApi vs OdysseyK1EngineApi
   - Investigate K2EngineApi vs OdysseyK2EngineApi
   - Delete duplicates

2. **Determine canonical Profile files**
   - Investigate K1GameProfile vs OdysseyK1GameProfile
   - Investigate K2GameProfile vs OdysseyK2GameProfile
   - Delete duplicates

### Priority 2: Create Base Classes in Common

For EVERY Odyssey implementation, create corresponding base class in Common:

1. **Combat System**
   - BaseCombatManager
   - BaseCombatRound
   - BaseDamageCalculator
   - BaseWeaponDamageCalculator

2. **Dialogue System**
   - BaseDialogueManager
   - BaseDialogueState
   - BaseConversationContext

3. **Systems**
   - BaseAIController
   - BaseEncounterSystem
   - BaseFactionManager
   - BaseHeartbeatSystem
   - BasePartyManager
   - BasePerceptionManager
   - BaseStoreSystem
   - BaseTriggerSystem

4. **Data Management**
   - BaseGameDataManager
   - BaseTwoDATableManager

5. **Loading**
   - BaseEntityFactory
   - BaseNavigationMeshFactory

6. **Templates**
   - BaseCreatureTemplate
   - BaseDoorTemplate
   - BaseEncounterTemplate
   - BaseItemTemplate
   - BaseMerchantTemplate
   - BasePlaceableTemplate
   - BaseSoundTemplate
   - BaseTriggerTemplate
   - BaseWaypointTemplate

7. **Components**
   - BaseActionQueueComponent
   - BaseAnimationComponent
   - BaseCreatureComponent
   - BaseDoorComponent
   - BaseEncounterComponent
   - BaseFactionComponent
   - BaseInventoryComponent
   - BaseItemComponent
   - BasePerceptionComponent
   - BasePlaceableComponent
   - BaseQuickSlotComponent
   - BaseRenderableComponent
   - BaseScriptHooksComponent
   - BaseSoundComponent
   - BaseStatsComponent
   - BaseStoreComponent
   - BaseTransformComponent
   - BaseTriggerComponent
   - BaseWaypointComponent

### Priority 3: Create Eclipse Implementations

For EVERY Odyssey system, create Eclipse equivalent:

1. **Combat** - Eclipse/Combat/
   - EclipseCombatManager : BaseCombatManager
   - DragonAgeOriginsCombatManager : EclipseCombatManager
   - DragonAge2CombatManager : EclipseCombatManager
   - MassEffectCombatManager : EclipseCombatManager
   - MassEffect2CombatManager : EclipseCombatManager

2. **Dialogue** - Eclipse/Dialogue/
   - EclipseDialogueManager : BaseDialogueManager
   - (game-specific subclasses as needed)

3. **Systems** - Eclipse/Systems/
   - (all Eclipse equivalents)

4. **Components** - Eclipse/Components/
   - (all Eclipse equivalents)

5. **Templates** - Eclipse/Templates/
   - (all Eclipse equivalents)

6. **Data** - Eclipse/Data/
   - (all Eclipse equivalents)

7. **Loading** - Eclipse/Loading/
   - (all Eclipse equivalents)

### Priority 4: Create Aurora Implementations

Same structure as Eclipse.

### Priority 5: Update Roadmap

Document ALL systems in roadmap with:

- Odyssey status (complete)
- Eclipse status (to be created)
- Aurora status (to be created)
- Base class status
- Ghidra references for each

## Architecture Principles

1. **NO tight coupling** - All layers properly isolated
2. **Proper inheritance** - Base classes in Common, engine-specific in subfolders
3. **Modular design** - Each system is independent
4. **Cross-engine consistency** - Same structure for all engines
5. **Parsing layer independence** - Tools can use Parsing without Runtime

## Next Steps

1. Investigate and fix EngineApi duplication
2. Create base classes for all Odyssey systems
3. Refactor Odyssey to use base classes
4. Create Eclipse implementations
5. Create Aurora implementations
6. Update roadmap with complete inventory
7. Ensure all systems have Ghidra references
