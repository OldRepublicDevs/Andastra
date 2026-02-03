# MonoGame ↔ Reva (Original Engine) UI Conversion Mapping

## Status Overview

| Component | Status | Notes |
|-----------|--------|-------|
| Main Menu | ✅ Complete | KotorGuiManager (MAINMENU) or FallbackMainMenu, music |
| New Game | ✅ Complete | → CharacterCreationScreen (CSWGuiClassSelection) |
| Load Game | ✅ Complete | LoadGameScreen (CSWGuiSaveLoad load mode) |
| Character Creation | ✅ Complete | Class, attributes, skills, feats, portrait, name |
| Start New Game | ✅ Complete | StartNewGame(characterData) → Load module → InGame |
| In-Game Rendering | ✅ Complete | IAreaRenderContext + OdysseyArea.Render(), chase camera |
| In-Game Save | ✅ Complete | Pause (Escape) → Save Game → SaveGameScreen (CSWGuiSaveLoad save mode) |
| Options | ✅ Complete | OptionsScreen: Graphics, Sound, persists to andastra.ini; loaded on next launch |
| Movies | ✅ Complete | MoviesScreen: lists BIK files, play selected (CSWGuiTitleMovies) |

### In-Game Save Flow (1:1 with Reva)
- Reva: Escape → pause_p (pause panel), BTN_SAVEGAME opens CSWGuiSaveLoad(manager, 1, 1) save mode
- Andastra: Escape → PauseMenu (Resume, Save Game, Load Game, Options, Exit)
- Save Game → SaveGameScreen: text input for save name, Enter to save, Escape to cancel
- GameSession.BuildSaveGameData() exports current state; OdysseySaveGameManager.SaveGameAsync() writes ERF

---

This document describes the 1:1 mapping between Andastra's MonoGame implementation and the original KOTOR engine as reverse-engineered via Reva/Ghidra.

## Program Startup Flow

### Original (swkotor.exe WinMain @ 0x004041f0)

```
1. CreateMutexA("swkotor")
2. CoInitialize
3. dofile("config.txt")
4. CExoBase::CExoBase, LoadAliases("swKotor.ini")
5. CAppManager::CAppManager
6. InitGameApp (creates window)
7. Read INI "Disable Sound" from swkotor.ini
8. checkCommandLineArgs
9. AurRenderCallback = messagepump
10. CClientExoApp::StartServices
11. CClientExoApp::Initialize
12. dofile("startup.txt")
13. CClientExoApp::SetLoadStep (0-4)
14. CClientExoApp::BeginIntro (intro movie)
15. CClientExoApp::DisplayMainMenu  ← Main menu shown
16. Main loop: PeekMessage → CClientExoApp::MainLoop → CServerExoApp::MainLoop
```

### Andastra Implementation

- **Program.cs**: Avalonia launcher or `--no-launcher` → `OdysseyGame.Run()`
- **OdysseyGame.Run()**: `Initialize()` → `_graphicsBackend.Initialize()` → `EnsureMainMenuInitialized()` → `_graphicsBackend.Run(updateAction, drawAction)`
- Main menu: KotorGuiManager (MAINMENU GUI) or FallbackMainMenu
- Main menu music: `StartMainMenuMusic()` → `mus_theme_cult` (K1) / `mus_sion` (K2)

## Main Menu Button Handlers

| Reva Function | Address (K1) | Andastra Handler | MonoGame Component |
|---------------|--------------|------------------|---------------------|
| OnNewGamePicked | 0x0067afb0 | BTN_NEWGAME | → CharacterCreationScreen (CSWGuiClassSelection) |
| OnLoadSaveGame | 0x0067b1a0 | BTN_LOADGAME | → LoadGameScreen (CSWGuiSaveLoad load mode) |
| OnOptionsPicked | 0x0067b2f0 | BTN_OPTIONS | → OptionsScreen (CSWGuiOptionsMain) |
| OnMoviesPicked | 0x0067b250 | BTN_MOVIES | → MoviesScreen (CSWGuiTitleMovies) |
| OnQuitButtonPressed | 0x0067b4a0 | BTN_EXIT | → _graphicsBackend.Exit() |
| OnWarpButtonPressed | 0x0067c4b0 | BTN_WARP | → same as BTN_LOADGAME |

## New Game Flow (1:1 Reva)

### Original (OnNewGamePicked)

1. `CSWPartyTable::ResetCurrentSessionStartTim()`
2. Add resource directory "MODULES:"
3. Check CExoResMan::Exists("END_M01AA", MOD) then RIM
4. Remove "MODULES:" directory
5. **CSWGuiClassSelection** constructor with module name
6. **CSWGuiManager::AddPanel**(manager, classSelectionPanel, 2, 1)
7. CExoSoundInternal::SetSoundMode
8. Set panel bit_flags |= 0x400

### Andastra

- `HandleMainMenuButtonClick("BTN_NEWGAME")` → `_gameState = CharacterCreation`, `ShowCharacterCreationScreen()`
- CharacterCreationScreen = CSWGuiClassSelection equivalent (class selection, attributes, skills, feats, portrait, name)
- On complete → `StartNewGame(characterData)` with module "end_m01aa" (K1) or "001ebo" (K2)

## Load Game Flow (1:1 Reva)

### Original (OnLoadSaveGame)

1. **CSWGuiSaveLoad**(manager, 0, 1) — param 0 = load mode, 1 = ?
2. **CSWGuiManager::AddPanel**(manager, saveLoadPanel, 2, 1)

### Andastra

- `HandleMainMenuButtonClick("BTN_LOADGAME")` → `_gameState = LoadGameMenu`, `ShowLoadGameScreen()`
- LoadGameScreen lists saves via OdysseySaveGameManager.ListSaves()
- On load → `GameSession.LoadGame(saveName)` → SaveGameManager.LoadGameAsync → LoadModule(saveData.CurrentModule)

## In-Game Rendering (1:1 with Reva)

When InGame, our draw callback:
1. Gets `World.CurrentArea` as `OdysseyArea`
2. Creates `OdysseyAreaRenderContext` with: GraphicsDevice, RoomMeshRenderer, BasicEffect, ViewMatrix, ProjectionMatrix, CameraPosition
3. Camera: ChaseCamera (Reva: swkotor.exe 0x004af630, swkotor2.exe 0x004dcfb0) with full controls:
   - Q/E: yaw, R/F: pitch
   - Right-mouse-drag: rotate
   - Mouse wheel: zoom
   - Navmesh raycast callback for collision avoidance
4. Calls `area.SetRenderContext(context)` then `area.Render()`
5. `OdysseyArea.Render()` uses VIS culling (FindCurrentRoom from camera position), room meshes, fog (Reva: CExo3DInternal)

### Controls (1:1 Reva)
- Left-click: move/attack/interact (cursor mode)
- Right-click: context (talk/attack)
- Tab: cycle party leader
- Space: pause
- 1–9: quick slots
- Camera: Q/E/R/F, right-drag, scroll (CameraRotate @ 0x007cb910, CameraViewAngle @ 0x007cb940)

### Perception
- PerceptionManager.Update() called each frame (Reva: 0x005fb0f0, PERCEPTIONDIST @ 0x007c4070)
- Line-of-sight via NavigationMesh.Raycast

## Graphics Conversion: DirectX → MonoGame

| Original (DirectX) | MonoGame Equivalent |
|--------------------|---------------------|
| DirectX device, swap chain | Microsoft.Xna.Framework.Game, GraphicsDevice |
| Sprite/quad rendering | SpriteBatch.Draw |
| Bitmap fonts | SpriteFont, SpriteBatch.DrawString |
| CExoStreamingSoundSource (WAV) | SoundEffect, SoundEffectInstance (looping) |
| GUI panels (GFF + TPC) | KotorGuiManager loads GUI/TPC, renders via SpriteBatch |
| CSWGuiManager::AddPanel | OdysseyGameState (MainMenu/CharacterCreation/LoadGameMenu/InGame) |
| Area room/entity rendering | IRoomMeshRenderer, IBasicEffect, OdysseyArea.Render() |

## Key Reva Addresses (swkotor.exe)

- WinMain: 0x004041f0
- CClientExoApp::DisplayMainMenu: 0x005ed420
- CClientExoAppInternal::DisplayMainMenu: 0x005fca30
- CClientExoAppInternal::StartMenuMusic: 0x005f9af0 (mus_theme_cult param_1=1)
- OnNewGamePicked: 0x0067afb0
- OnLoadSaveGame: 0x0067b1a0
- CSWGuiMainMenu constructor: 0x0067c4c0
- CSWGuiClassSelection: 0x006dc3c0
- CSWGuiSaveLoad: 0x006cc680

## State Machine

```
MainMenu
   ├─ BTN_NEWGAME → CharacterCreation
   │                   └─ Complete → InGame (StartNewGame)
   │                   └─ Cancel  → MainMenu
   ├─ BTN_LOADGAME → LoadGameMenu
   │                   └─ Load    → InGame (LoadGame)
   │                   └─ Cancel  → MainMenu
   ├─ BTN_EXIT    → Exit
   ├─ BTN_OPTIONS → OptionsScreen
   └─ BTN_MOVIES  → MoviesScreen

InGame
   └─ Escape → PauseMenu (overlay)
                 ├─ Resume     → InGame
                 ├─ Save Game  → SaveGameScreen (overlay)
                 │                 └─ Save/Cancel → PauseMenu
                 ├─ Load Game  → LoadGameScreen (overlay)
                 │                 └─ Load/Cancel → InGame or PauseMenu
                 ├─ Options    → OptionsScreen (overlay)
                 └─ Exit       → MainMenu
```

---

## Full Explanation: How We Implement the UI and Convert to MonoGame While Preserving Reva Accuracy

### Design Principle

We treat the Reva decompiled C/C++ as the source of truth. Every UI transition, button handler, and panel flow maps to a specific function or address in the original binary. MonoGame is the *rendering substrate*—we translate DirectX/GUI concepts to MonoGame equivalents without changing the logical flow.

### 1. Panel Stack → State Machine

**Reva:** `CSWGuiManager::AddPanel(manager, panel, flags)` pushes panels; button handlers determine which panel is shown next.

**Andastra:** `OdysseyGameState` enum and `_gameState` variable implement the same flow:
- MainMenu = main menu panel visible
- CharacterCreation = CSWGuiClassSelection equivalent
- LoadGameMenu = CSWGuiSaveLoad load mode
- InGame = gameplay
- Pause/Save/Load = overlays on InGame (Reva: pause_p, BTN_SAVEGAME, BTN_SAVELOAD)

### 2. Graphics Conversion Table

| Original (swkotor.exe) | MonoGame Implementation |
|------------------------|--------------------------|
| DirectX device, D3DPRESENT_PARAMETERS | `Microsoft.Xna.Framework.Game`, `GraphicsDeviceManager` |
| CExoBitmapSurface (sprite quads) | `SpriteBatch.Draw()` with 1×1 pixel texture for rectangles |
| CExoFont / bitmap fonts | `SpriteFont` (Arial) or `OdysseyBitmapFont` (TPC→Texture2D) |
| CExoStreamingSoundSource (WAV loop) | `SoundEffect.CreateInstance()`, `IsLooping = true` |
| GUI GFF (controls, textures) | KotorGuiManager loads GFF, TPC→Texture2D, renders via SpriteBatch |
| Room/area 3D rendering | `IRoomMeshRenderer`, `IBasicEffect`, `OdysseyArea.Render()` |
| Camera (CExo3DInternal) | `OdysseyAreaRenderContext` with View/Projection matrices |

### 3. Button Tag Mapping

Reva uses string tags (e.g. `BTN_NEWGAME`, `BTN_SAVEGAME`). KotorGuiManager fires `OnButtonClicked(e.ButtonTag)`. We compare `buttonTag` in `HandleMainMenuButtonClick` and branch exactly as the original handlers do.

### 4. Save/Load Format Fidelity

`OdysseySaveGameManager` writes ERF archives with `savenfo.res`, `GLOBALVARS.res`, `PARTYTABLE.res`, `[module]_s.rim` per Reva 0x004eb750 and LoadSavegame 0x00708990. Directory format `%06d - %s` matches the original.

### 5. SynchronizationContext Reset

Reva's WinMain runs the game loop directly. When launching from Avalonia, `SynchronizationContext.SetSynchronizationContext(null)` is called before `OdysseyGame.Run()` to avoid UI-thread affinity conflicts with MonoGame/OpenGL—preserving the “direct loop” behavior.

### 6. Movies and Options (1:1 Reva)

- **Movies**: MoviesScreen lists BIK files from game movies folder. On selection, opens movie in system default player (shell execute).
- **Options**: OptionsScreen with Graphics (Width, Height, Fullscreen, VSync) and Sound (MusicVolume, SoundVolume, DisableSound). Persists to andastra.ini.

### 7. Full Save Format Serialization (Reva-complete)

- **savenfo.res**: NFO GFF (metadata: AREANAME, LASTMODULE, TIMEPLAYED, CHEATUSED, SAVEGAMENAME, TIMESTAMP, PCNAME, SAVENUMBER, STORYHINT0-9, LIVECONTENT, LIVE1-9)
- **GLOBALVARS.res**: GLOB GFF (booleans, ints, strings; KOTOR VariableList format does not include location type)
- **PARTYTABLE.res**: PT GFF (Reva 0x0057bd70 full format): PT_PCNAME, PT_GOLD, PT_ITEM_COMPONENT, PT_ITEM_CHEMICAL, PT_SWOOP1-3, PT_XP_POOL, PT_PLAYEDSECONDS, PT_CONTROLLED_NPC, PT_SOLOMODE, PT_CHEAT_USED, PT_NUM_MEMBERS, PT_MEMBERS, PT_NUM_PUPPETS, PT_PUPPETS, PT_AVAIL_PUPS, PT_AVAIL_NPCS, PT_INFLUENCE, PT_AISTATE, PT_FOLLOWSTATE, PT_PAZAAKCARDS, PT_PAZSIDELIST, PT_TUT_WND_SHOWN, PT_LAST_GUI_PNL, PT_FB_MSG_LIST, PT_DLG_MSG_LIST, PT_COM_MSG_LIST, PT_COST_MULT_LIST, PT_DISABLEMAP, PT_DISABLEREGEN, plus PartyList for loader
- **JOURNAL.res**: Journal GFF (quest states, journal entries with QuestTag, State, DateAdded, Text, XPReward)
- **REPUTE.res**: Faction reputation GFF (FactionID1, FactionID2, FactionRep)
- **[module]_s.rim**: Module state ERF (area states, creatures, doors, placeables)

### 7. Runtime Requirements

- **Valid KOTOR installation path** required (Program.Main validates via Installation; GameSession requires it for resource loading).
- **MonoGame backend**: Select MonoGame in launcher, or use `--no-launcher --backend monogame --path <KOTOR_PATH>`.
- **FallbackMainMenu** includes all Reva buttons: BTN_NEWGAME, BTN_LOADGAME, BTN_WARP, BTN_OPTIONS, BTN_MOVIES, BTN_EXIT (1:1 with KotorGuiManager MAINMENU).
