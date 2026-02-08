using System;
using Andastra.Game.Games.Odyssey;
using Andastra.Game.Games.Odyssey.Game;
using Andastra.Game.Graphics.MonoGame.Camera;
using Andastra.Game.Graphics.MonoGame.Graphics;
using Andastra.Game.Graphics.MonoGame.GUI;
using Andastra.Game.Graphics.MonoGame.Rendering;
using Andastra.Game.Graphics.MonoGame.UI;
using Andastra.Game.Graphics.MonoGame.UI.MainMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Keys = Andastra.Runtime.Graphics.Keys;
using ButtonState = Andastra.Runtime.Graphics.ButtonState;
using Andastra.Game.Scripting.Interfaces;
using Andastra.Game.Scripting.VM;
using Andastra.Runtime.Content.ResourceProviders;
using Andastra.Runtime.Core;
using Andastra.Runtime.Core.Audio;
using Andastra.Runtime.Core.Entities;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.Graphics.Common.GUI;
using BioWare.Common;
using BioWare.Extract;

namespace Andastra.Game.Core
{
    /// <summary>
    /// Game flow state - 1:1 with Reva CSWGuiManager panel stack.
    /// MainMenu -> [CharacterCreation | LoadGameMenu | Options | Movies] -> InGame.
    /// </summary>
    internal enum OdysseyGameState
    {
        MainMenu,
        CharacterCreation,
        LoadGameMenu,
        Movies,
        Options,
        InGame
    }

    /// <summary>
    /// Main game wrapper for Odyssey Engine games (KOTOR 1/2).
    /// Coordinates graphics backend, engine initialization, and game session management.
    /// </summary>
    /// <remarks>
    /// Odyssey Game Wrapper:
    /// - [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): 0x00404250 @ 0x00404250 (WinMain equivalent, initializes game)
    /// - Original implementation: Initializes engine, creates game session, runs game loop
    /// - Graphics backend: MonoGame (exclusive)
    /// - Game session: Manages all game systems (combat, dialogue, AI, scripts, etc.)
    /// - Game loop: Coordinates update/draw callbacks with graphics backend
    /// </remarks>
    public class OdysseyGame : IDisposable
    {
        private readonly GameSettings _settings;
        private readonly IGraphicsBackend _graphicsBackend;
        private GameSession _gameSession;
        private World _world;
        private NcsVm _vm;
        private IScriptGlobals _globals;
        private Installation _installation;
        private KotorGuiManager _mainMenuGui;
        private MainMenuScreen _mainMenuScreen;
        private bool _useFallbackMainMenu;
        private bool _isInMainMenu = true;
        private bool _disposed;
        private bool _initialized;
        private IMusicPlayer _musicPlayer;
        private bool _mainMenuMusicStarted;
        private OdysseyGameState _gameState = OdysseyGameState.MainMenu;
        private CharacterCreationScreen _characterCreationScreen;
        private LoadGameScreen _loadGameScreen;
        private Games.Common.BaseGuiManager _chargenGuiManager;
        private IRoomMeshRenderer _cachedRoomMeshRenderer;
        private IBasicEffect _cachedBasicEffect;
        private OdysseyAreaRenderContext _areaRenderContext;
        private PauseMenu _pauseMenu;
        private SaveGameScreen _saveGameScreen;
        private MoviesScreen _moviesScreen;
        private OptionsScreen _optionsScreen;
        private bool _pauseMenuVisible;
        private bool _loadScreenFromPause;
        private bool _escapeWasDown;
        private ChaseCamera _chaseCamera;
        private BaseMenuRenderer _fallbackMenuRenderer;

        /// <summary>
        /// Initializes a new instance of OdysseyGame.
        /// </summary>
        /// <param name="settings">Game settings including game path and configuration.</param>
        /// <param name="graphicsBackend">Graphics backend to use for rendering.</param>
        public OdysseyGame(GameSettings settings, IGraphicsBackend graphicsBackend)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (graphicsBackend == null)
            {
                throw new ArgumentNullException(nameof(graphicsBackend));
            }

            if (string.IsNullOrEmpty(settings.GamePath))
            {
                throw new ArgumentException("Game path cannot be null or empty", nameof(settings));
            }

            _settings = settings;
            _graphicsBackend = graphicsBackend;
        }

        /// <summary>
        /// Initializes the game systems and prepares for execution.
        /// </summary>
        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            // Create engine-specific time manager
            var timeManager = new Games.Common.BaseTimeManager();

            // Create world instance with engine-specific time manager
            _world = new World(timeManager);

            // Create NCS VM for script execution
            _vm = new NcsVm();

            // Create script globals using factory pattern based on game type
            // Based on k1_win_gog_swkotor.exe and k2_win_gog_aspyr_swkotor2.exe: Script globals system initializes global variables
            // Original implementation: Global variables initialized at game start based on game type
            // Factory pattern ensures correct globals instance is created (K1ScriptGlobals for K1, K2ScriptGlobals for K2)
            _globals = ScriptGlobalsFactory.Create(_settings.Game);

            // Create game session with all required dependencies
            _gameSession = new GameSession(_settings, _world, _vm, _globals);

            // Initialize graphics backend with settings
            string gameTitle = _settings.Game == KotorGame.K1
                ? "Star Wars: Knights of the Old Republic"
                : "Star Wars: Knights of the Old Republic II - The Sith Lords";

            // Set game path before initialization so content manager can use it
            // Note: Odyssey graphics backend is accessed via IGraphicsBackend interface
            // Game path is set through the backend initialization, not directly here

            _graphicsBackend.Initialize(
                _settings.Width,
                _settings.Height,
                gameTitle,
                _settings.Fullscreen);

            _initialized = true;
        }


        /// <summary>
        /// Runs the game loop (blocks until game exits).
        /// </summary>
        public void Run()
        {
            Initialize();

            void EnsureMainMenuInitialized()
            {
                // Already have KOTOR main menu or fallback menu (MonoGame or Stride)
                if (_mainMenuGui != null || _mainMenuScreen != null || _fallbackMenuRenderer != null)
                {
                    return;
                }

                if (_graphicsBackend?.GraphicsDevice == null)
                {
                    return;
                }

                // Stride backend: use MenuRendererFactory to create StrideMenuRenderer. Reva (k1_win_gog_swkotor.exe): DisplayMainMenu @ 0x005fca30, CSWGuiMainMenu @ 0x0067c4c0, LoadFromLayout @ 0x0067ace0.
                if (_graphicsBackend.BackendType == GraphicsBackendType.Stride)
                {
                    try
                    {
                        _fallbackMenuRenderer = MenuRendererFactory.CreateMenuRenderer(_graphicsBackend);
                        if (_fallbackMenuRenderer != null)
                        {
                            _fallbackMenuRenderer.IsVisible = true;
                            _useFallbackMainMenu = true;
                            try
                            {
                                _installation = new Installation(_settings.GamePath);
                            }
                            catch
                            {
                                // Installation optional for Stride fallback (music may not play)
                            }
                            StartMainMenuMusic();
                            Console.WriteLine("[OdysseyGame] Stride main menu (fallback) initialized");
                        }
                        else
                        {
                            Console.WriteLine("[OdysseyGame] ERROR: Failed to create Stride menu renderer");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[OdysseyGame] ERROR initializing Stride main menu: {ex.Message}");
                    }
                    return;
                }

                // Get the raw MonoGame GraphicsDevice first (needed for both KOTOR GUI and fallback)
                Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice = null;
                if (_graphicsBackend.GraphicsDevice is MonoGameGraphicsDevice mgDevice)
                {
                    rawDevice = mgDevice.Device;
                }
                else
                {
                    Console.WriteLine("[OdysseyGame] ERROR: Graphics backend is not MonoGame or Stride, cannot create main menu");
                    return;
                }

                // Initialize game installation for resource access
                try
                {
                    _installation = new Installation(_settings.GamePath);
                    Console.WriteLine($"[OdysseyGame] Initialized game installation from: {_settings.GamePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[OdysseyGame] ERROR initializing installation: {ex} - using fallback menu");
                    _useFallbackMainMenu = true;
                    _mainMenuScreen = CreateMainMenuScreen(rawDevice, null);
                    _mainMenuScreen.OnButtonClicked += (tag) => HandleMainMenuButtonClick(tag);
                    return;
                }

                // Create KotorGuiManager for proper GUI loading
                _mainMenuGui = new KotorGuiManager(
                    rawDevice,
                    _installation,
                    soundPlayer: null);

                // Load the main menu GUI
                // K1: "MAINMENU" or "mainmenu16x12"
                // K2: "MAINMENU" or "mainmenu8x6_p"
                string mainMenuResRef = _settings.Game == KotorGame.K1 ? "mainmenu16x12" : "mainmenu8x6_p";
                bool loaded = _mainMenuGui.LoadGui(mainMenuResRef, _settings.Width, _settings.Height);

                if (!loaded)
                {
                    // Try alternate GUI files based on game type
                    mainMenuResRef = "MAINMENU";
                    loaded = _mainMenuGui.LoadGui(mainMenuResRef, _settings.Width, _settings.Height);
                }

                if (loaded)
                {
                    _mainMenuGui.SetCurrentGui(mainMenuResRef);
                    Console.WriteLine($"[OdysseyGame] Loaded main menu GUI: {mainMenuResRef}");

                    // Wire up button click handlers
                    _mainMenuGui.OnButtonClicked += (sender, e) =>
                    {
                        HandleMainMenuButtonClick(e.ButtonTag);
                    };

                    // Start main menu music (1:1 with Reva k1_win_gog_swkotor.exe CClientExoAppInternal::DisplayMainMenu -> StartMenuMusic)
                    StartMainMenuMusic();
                }
                else
                {
                    Console.WriteLine($"[OdysseyGame] WARNING: Failed to load main menu GUI - using fallback menu");
                    _useFallbackMainMenu = true;
                    _mainMenuScreen = CreateMainMenuScreen(rawDevice, _installation);
                    _mainMenuScreen.OnButtonClicked += (tag) => HandleMainMenuButtonClick(tag);
                    _mainMenuScreen.OnModuleSelected += (moduleName) =>
                    {
                        Console.WriteLine($"[OdysseyGame] Warp to module: {moduleName}");
                        _graphicsBackend?.Exit();
                    };
                    StartMainMenuMusic();
                }
            }

            void StartMainMenuMusic()
            {
                if (_mainMenuMusicStarted)
                    return;

                try
                {
                    var resourceProvider = new GameResourceProvider(_installation);
                    _musicPlayer = _graphicsBackend?.CreateMusicPlayer(resourceProvider) as IMusicPlayer;
                    if (_musicPlayer != null && (_settings.Audio?.MusicEnabled ?? true))
                    {
                        // Reva k1_win_gog_swkotor.exe 0x005f9af0: param_1=1 -> mus_theme_cult (K1), K2 uses mus_sion
                        string mainMenuMusic = _settings.Game == KotorGame.K1 ? "mus_theme_cult" : "mus_sion";
                        float vol = _settings.Audio?.MusicVolume ?? 0.5f;
                        bool played = _musicPlayer.Play(mainMenuMusic, vol);
                        _mainMenuMusicStarted = played;
                        if (played)
                            Console.WriteLine($"[OdysseyGame] Started main menu music: {mainMenuMusic}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[OdysseyGame] WARNING: Could not start main menu music: {ex.Message}");
                }
            }

            MainMenuScreen CreateMainMenuScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice, Installation installation)
            {
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try
                    {
                        font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial");
                    }
                    catch (Exception)
                    {
                        // Font optional; fallback draws labels as rectangles only
                    }
                }
                int w = _settings.Width > 0 ? _settings.Width : rawDevice.Viewport.Width;
                int h = _settings.Height > 0 ? _settings.Height : rawDevice.Viewport.Height;
                bool isK2 = _settings.Game == KotorGame.K2;
                var screen = new MainMenuScreen(rawDevice, w, h, isK2, font, installation);
                screen.SetWarpButtonVisible(false);
                return screen;
            }

            void HandleMainMenuButtonClick(string buttonTag)
            {
                try
                {
                    Console.WriteLine($"[OdysseyGame] Main menu button clicked: {buttonTag}");

                    if (string.Equals(buttonTag, "BTN_NEWGAME", StringComparison.OrdinalIgnoreCase))
                    {
                        var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (dev != null)
                        {
                            Console.WriteLine("[OdysseyGame] New Game -> Character Creation (Reva: OnNewGamePicked -> CSWGuiClassSelection)");
                            _isInMainMenu = false;
                            StopMainMenuMusic();
                            _gameState = OdysseyGameState.CharacterCreation;
                            ShowCharacterCreationScreen(dev);
                        }
                        if (_characterCreationScreen == null)
                        {
                            _gameState = OdysseyGameState.MainMenu;
                            _isInMainMenu = true;
                            StartMainMenuMusic();
                        }
                    }
                    else if (string.Equals(buttonTag, "BTN_LOADGAME", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(buttonTag, "BTN_WARP", StringComparison.OrdinalIgnoreCase))
                    {
                        var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (dev != null)
                        {
                            Console.WriteLine("[OdysseyGame] Load Game (Reva: OnLoadSaveGame -> CSWGuiSaveLoad load mode)");
                            _isInMainMenu = false;
                            StopMainMenuMusic();
                            _gameState = OdysseyGameState.LoadGameMenu;
                            ShowLoadGameScreen(dev);
                        }
                        if (_loadGameScreen == null)
                        {
                            _gameState = OdysseyGameState.MainMenu;
                            _isInMainMenu = true;
                            StartMainMenuMusic();
                        }
                    }
                    else if (string.Equals(buttonTag, "BTN_OPTIONS", StringComparison.OrdinalIgnoreCase))
                    {
                        var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (dev != null)
                        {
                            _gameState = OdysseyGameState.Options;
                            ShowOptionsScreen(dev);
                        }
                    }
                    else if (string.Equals(buttonTag, "BTN_EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("[OdysseyGame] Exit selected (Reva: OnQuitButtonPressed -> ExitProgram)");
                        _graphicsBackend?.Exit();
                    }
                    else if (string.Equals(buttonTag, "BTN_MUSIC", StringComparison.OrdinalIgnoreCase))
                    {
                        var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (dev != null)
                        {
                            _gameState = OdysseyGameState.Options;
                            ShowOptionsScreen(dev);
                        }
                    }
                    else if (string.Equals(buttonTag, "BTN_MOVIES", StringComparison.OrdinalIgnoreCase))
                    {
                        var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (dev != null && _installation != null)
                        {
                            _gameState = OdysseyGameState.Movies;
                            ShowMoviesScreen(dev);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[OdysseyGame] Unknown button: {buttonTag}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[OdysseyGame] ERROR handling button click: {ex}");
                }
            }

            void ShowCharacterCreationScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice)
            {
                if (_characterCreationScreen != null) return;
                if (_installation == null) return;
                var mgDevice = _graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice;
                if (mgDevice == null) return;
                _chargenGuiManager = _mainMenuGui ?? new KotorGuiManager(rawDevice, _installation, null);
                var gameDataManager = new Games.Odyssey.Data.GameDataManager(_installation);
                _characterCreationScreen = new CharacterCreationScreen(
                    _graphicsBackend.GraphicsDevice,
                    _installation,
                    _settings.Game == KotorGame.K1 ? BioWareGame.K1 : BioWareGame.TSL,
                    _chargenGuiManager,
                    (data) =>
                    {
                        Console.WriteLine("[OdysseyGame] Character creation complete -> StartNewGame");
                        _chaseCamera = null;
                        _characterCreationScreen = null;
                        _gameState = OdysseyGameState.InGame;
                        var runtimeData = data != null ? ToRuntimeCharacterCreationData(data) : null;
                        _gameSession?.StartNewGame(runtimeData);
                    },
                    () =>
                    {
                        Console.WriteLine("[OdysseyGame] Character creation cancelled -> Main Menu");
                        _characterCreationScreen = null;
                        _gameState = OdysseyGameState.MainMenu;
                        _isInMainMenu = true;
                        StartMainMenuMusic();
                    },
                    _graphicsBackend);
            }

            void EnsurePauseMenuInitialized()
            {
                if (_pauseMenu != null) return;
                var dev = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                if (dev == null) return;
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try { font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial"); }
                    catch { }
                }
                _pauseMenu = new PauseMenu(dev, font);
                _pauseMenu.IsVisible = true;
                _pauseMenu.OnResume += () => { _pauseMenuVisible = false; _pauseMenu.IsVisible = false; _gameSession?.Resume(); };
                _pauseMenu.OnMenuClosed += () => { _pauseMenuVisible = false; _pauseMenu.IsVisible = false; _gameSession?.Resume(); };
                _pauseMenu.OnSaveGame += () =>
                {
                    _pauseMenuVisible = false;
                    _pauseMenu.IsVisible = false;
                    var d = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                    if (d != null) ShowSaveGameScreen(d);
                };
                _pauseMenu.OnLoadGame += () =>
                {
                    _pauseMenuVisible = false;
                    _pauseMenu.IsVisible = false;
                    _loadScreenFromPause = true;
                    var d = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                    if (d != null) ShowLoadGameScreen(d, fromPause: true);
                };
                _pauseMenu.OnOptions += () =>
                {
                    _pauseMenuVisible = false;
                    _pauseMenu.IsVisible = false;
                    var d = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                    if (d != null) ShowOptionsScreenFromPause(d);
                };
                _pauseMenu.OnExit += () =>
                {
                    _pauseMenuVisible = false;
                    _pauseMenu.IsVisible = false;
                    _gameSession?.Resume();
                    _gameState = OdysseyGameState.MainMenu;
                    _isInMainMenu = true;
                    StartMainMenuMusic();
                };
            }

            void ShowSaveGameScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice)
            {
                if (_saveGameScreen != null) return;
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try { font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial"); }
                    catch { }
                }
                _saveGameScreen = new SaveGameScreen(rawDevice, _settings.Width, _settings.Height, font);
                _saveGameScreen.OnSave += (saveName) =>
                {
                    bool ok = _gameSession?.SaveGame(saveName) ?? false;
                    _saveGameScreen?.Dispose();
                    _saveGameScreen = null;
                    if (ok) Console.WriteLine("[OdysseyGame] Game saved successfully");
                    _pauseMenuVisible = true;
                    EnsurePauseMenuInitialized();
                    _pauseMenu.IsVisible = true;
                };
                _saveGameScreen.OnCancel += () =>
                {
                    _saveGameScreen?.Dispose();
                    _saveGameScreen = null;
                    _pauseMenuVisible = true;
                    EnsurePauseMenuInitialized();
                    _pauseMenu.IsVisible = true;
                };
            }

            void ShowMoviesScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice)
            {
                if (_moviesScreen != null) return;
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try { font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial"); }
                    catch { }
                }
                _moviesScreen = new MoviesScreen(rawDevice, _settings.Width, _settings.Height, font, _installation);
                _moviesScreen.OnPlayMovie += (movieName) =>
                {
                    Console.WriteLine("[OdysseyGame] Play movie: " + movieName);
                    if (_installation != null && !string.IsNullOrEmpty(movieName))
                    {
                        try
                        {
                            string moviesPath = System.IO.Path.Combine(_installation.Path, "movies");
                            string bikPath = System.IO.Path.Combine(moviesPath, movieName + ".bik");
                            if (System.IO.File.Exists(bikPath))
                            {
                                string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "andastra_movie_" + movieName + ".bik");
                                System.IO.File.Copy(bikPath, tempPath, overwrite: true);
                                var psi = new System.Diagnostics.ProcessStartInfo
                                {
                                    FileName = tempPath,
                                    UseShellExecute = true
                                };
                                System.Diagnostics.Process.Start(psi);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[OdysseyGame] Movie playback: " + ex.Message);
                        }
                    }
                    _moviesScreen?.Dispose();
                    _moviesScreen = null;
                    _gameState = OdysseyGameState.MainMenu;
                    _isInMainMenu = true;
                    StartMainMenuMusic();
                };
                _moviesScreen.OnCancel += () =>
                {
                    _moviesScreen?.Dispose();
                    _moviesScreen = null;
                    _gameState = OdysseyGameState.MainMenu;
                    _isInMainMenu = true;
                    StartMainMenuMusic();
                };
            }

            void ShowOptionsScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice)
            {
                ShowOptionsScreenInternal(rawDevice, fromPause: false);
            }

            void ShowOptionsScreenFromPause(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice)
            {
                ShowOptionsScreenInternal(rawDevice, fromPause: true);
            }

            void ShowOptionsScreenInternal(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice, bool fromPause)
            {
                if (_optionsScreen != null) return;
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try { font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial"); }
                    catch { }
                }
                string configPath = System.IO.Path.Combine(_settings.GamePath, "andastra.ini");
                _optionsScreen = new OptionsScreen(rawDevice, _settings.Width, _settings.Height, font, configPath);
                _optionsScreen.OnApply += () =>
                {
                    // Apply options to settings and music player before disposing
                    if (_optionsScreen != null)
                    {
                        _settings.Width = _optionsScreen.ResolutionWidth;
                        _settings.Height = _optionsScreen.ResolutionHeight;
                        _settings.Fullscreen = _optionsScreen.Fullscreen;
                        if (_settings.Graphics != null)
                        {
                            _settings.Graphics.ResolutionWidth = _optionsScreen.ResolutionWidth;
                            _settings.Graphics.ResolutionHeight = _optionsScreen.ResolutionHeight;
                            _settings.Graphics.Fullscreen = _optionsScreen.Fullscreen;
                            _settings.Graphics.VSync = _optionsScreen.VSync;
                        }
                        if (_settings.Audio != null)
                        {
                            _settings.Audio.MusicVolume = _optionsScreen.MusicVolume;
                            _settings.Audio.SfxVolume = _optionsScreen.SoundVolume;
                            _settings.Audio.MusicEnabled = !_optionsScreen.DisableSound;
                        }
                        if (_musicPlayer != null)
                        {
                            if (_optionsScreen.DisableSound)
                                _musicPlayer.Stop();
                            else
                                _musicPlayer.Volume = _optionsScreen.MusicVolume;
                        }
                    }
                    _optionsScreen?.Dispose();
                    _optionsScreen = null;
                    if (fromPause)
                    {
                        _pauseMenuVisible = true;
                        EnsurePauseMenuInitialized();
                        _pauseMenu.IsVisible = true;
                    }
                    else
                        _gameState = OdysseyGameState.MainMenu;
                };
                _optionsScreen.OnCancel += () =>
                {
                    _optionsScreen?.Dispose();
                    _optionsScreen = null;
                    if (fromPause)
                    {
                        _pauseMenuVisible = true;
                        EnsurePauseMenuInitialized();
                        _pauseMenu.IsVisible = true;
                    }
                    else
                        _gameState = OdysseyGameState.MainMenu;
                };
            }

            void ShowLoadGameScreen(Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice, bool fromPause = false)
            {
                if (_loadGameScreen != null && !fromPause) return;
                if (fromPause && _loadGameScreen != null) return;
                if (_installation == null)
                {
                    Console.WriteLine("[OdysseyGame] Cannot show Load Game: installation not initialized");
                    return;
                }
                Microsoft.Xna.Framework.Graphics.SpriteFont font = null;
                if (_graphicsBackend?.ContentManager is MonoGameContentManager mgContent)
                {
                    try { font = mgContent.ContentManager.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Fonts/Arial"); }
                    catch { }
                }
                string savesDir = System.IO.Path.Combine(_settings.GamePath, "saves");
                if (!System.IO.Directory.Exists(savesDir))
                    savesDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), _settings.Game == KotorGame.K1 ? "SWKotOR" : "SWKotOR2", "saves");
                _loadGameScreen = new LoadGameScreen(rawDevice, _settings.Width, _settings.Height, font, savesDir, _installation);
                _loadGameScreen.OnLoad += (saveName) =>
                {
                    Console.WriteLine($"[OdysseyGame] Load selected: {saveName}");
                    _chaseCamera = null;
                    bool ok = _gameSession?.LoadGame(saveName) ?? false;
                    _loadGameScreen?.Dispose();
                    _loadGameScreen = null;
                    _loadScreenFromPause = false;
                    _gameState = ok ? OdysseyGameState.InGame : (fromPause ? OdysseyGameState.InGame : OdysseyGameState.LoadGameMenu);
                    if (!ok)
                    {
                        var d = (_graphicsBackend.GraphicsDevice as MonoGameGraphicsDevice)?.Device;
                        if (d != null) ShowLoadGameScreen(d, fromPause);
                    }
                };
                _loadGameScreen.OnCancel += () =>
                {
                    _loadGameScreen?.Dispose();
                    _loadGameScreen = null;
                    _loadScreenFromPause = false;
                    if (fromPause)
                    {
                        _pauseMenuVisible = true;
                        EnsurePauseMenuInitialized();
                        _pauseMenu.IsVisible = true;
                    }
                    else
                    {
                        Console.WriteLine("[OdysseyGame] Load cancelled -> Main Menu");
                        _gameState = OdysseyGameState.MainMenu;
                        _isInMainMenu = true;
                        StartMainMenuMusic();
                    }
                };
            }

            Runtime.Core.Game.CharacterCreationData ToRuntimeCharacterCreationData(CharacterCreationData data)
            {
                if (data == null) return null;
                Runtime.Core.Game.CharacterClass rc = MapCharacterClass(data.Class);
                return new Runtime.Core.Game.CharacterCreationData
                {
                    Class = rc,
                    Gender = data.Gender == Gender.Male ? Runtime.Core.Game.Gender.Male : Runtime.Core.Game.Gender.Female,
                    Appearance = data.Appearance,
                    Portrait = data.Portrait,
                    Name = data.Name,
                    Strength = data.Strength,
                    Dexterity = data.Dexterity,
                    Constitution = data.Constitution,
                    Intelligence = data.Intelligence,
                    Wisdom = data.Wisdom,
                    Charisma = data.Charisma,
                    SkillRanks = data.SkillRanks
                };
            }

            Runtime.Core.Game.CharacterClass MapCharacterClass(CharacterClass c)
            {
                switch (c)
                {
                    case CharacterClass.Soldier: return Runtime.Core.Game.CharacterClass.Soldier;
                    case CharacterClass.Scout: return Runtime.Core.Game.CharacterClass.Scout;
                    case CharacterClass.Scoundrel: return Runtime.Core.Game.CharacterClass.Scoundrel;
                    case CharacterClass.JediGuardian: return Runtime.Core.Game.CharacterClass.JediGuardian;
                    case CharacterClass.JediSentinel: return Runtime.Core.Game.CharacterClass.JediSentinel;
                    case CharacterClass.JediConsular: return Runtime.Core.Game.CharacterClass.JediConsular;
                    default: return Runtime.Core.Game.CharacterClass.Scout;
                }
            }

            // Set up update callback for game logic
            Action<float> updateAction = (deltaTime) =>
            {
                EnsureMainMenuInitialized();

                switch (_gameState)
                {
                    case OdysseyGameState.MainMenu:
                        if (_useFallbackMainMenu)
                        {
                            if (_mainMenuScreen != null)
                                _mainMenuScreen.Update(deltaTime);
                            else
                                _fallbackMenuRenderer?.Update(deltaTime);
                        }
                        else
                            _mainMenuGui?.Update(deltaTime);
                        return;

                    case OdysseyGameState.CharacterCreation:
                        if (_characterCreationScreen != null && _graphicsBackend?.InputManager != null)
                            _characterCreationScreen.Update(deltaTime, _graphicsBackend.InputManager.KeyboardState, _graphicsBackend.InputManager.MouseState);
                        return;

                    case OdysseyGameState.LoadGameMenu:
                        _loadGameScreen?.Update(deltaTime);
                        return;

                    case OdysseyGameState.Movies:
                        _moviesScreen?.Update(deltaTime);
                        return;

                    case OdysseyGameState.Options:
                        _optionsScreen?.Update(deltaTime);
                        return;

                    case OdysseyGameState.InGame:
                        if (_saveGameScreen != null)
                        {
                            _saveGameScreen.Update(deltaTime);
                            return;
                        }
                        if (_optionsScreen != null)
                        {
                            _optionsScreen.Update(deltaTime);
                            return;
                        }
                        if (_pauseMenuVisible)
                        {
                            EnsurePauseMenuInitialized();
                            var kb = _graphicsBackend?.InputManager?.KeyboardState;
                            var prevKb = _graphicsBackend?.InputManager?.PreviousKeyboardState;
                            bool up = (kb?.IsKeyDown(Keys.Up) ?? false) && (prevKb?.IsKeyUp(Keys.Up) ?? true);
                            bool down = (kb?.IsKeyDown(Keys.Down) ?? false) && (prevKb?.IsKeyUp(Keys.Down) ?? true);
                            bool select = (kb?.IsKeyDown(Keys.Enter) ?? false) && (prevKb?.IsKeyUp(Keys.Enter) ?? true);
                            bool cancel = (kb?.IsKeyDown(Keys.Escape) ?? false) && (prevKb?.IsKeyUp(Keys.Escape) ?? true);
                            _pauseMenu?.HandleInput(up, down, select, cancel);
                            return;
                        }
                        if (_loadGameScreen != null && _loadScreenFromPause)
                        {
                            _loadGameScreen.Update(deltaTime);
                            return;
                        }
                        bool escDown = _graphicsBackend?.InputManager?.KeyboardState?.IsKeyDown(Keys.Escape) ?? false;
                        if (escDown && !_escapeWasDown)
                        {
                            _pauseMenuVisible = true;
                            _gameSession?.Pause();
                        }
                        _escapeWasDown = escDown;

                        // Update chase camera and process player input - Reva: 0x004af630 (chase camera), CExoInputInternal
                        var input = _graphicsBackend?.InputManager;
                        if (_gameSession != null && input != null && _graphicsBackend?.GraphicsDevice != null)
                        {
                            int vw = _graphicsBackend.GraphicsDevice.Viewport.Width;
                            int vh = _graphicsBackend.GraphicsDevice.Viewport.Height;
                            int mx = input.MouseState?.X ?? 0;
                            int my = input.MouseState?.Y ?? 0;
                            bool leftClick = (input.MouseState?.LeftButton == ButtonState.Pressed) && (input.PreviousMouseState?.LeftButton == ButtonState.Released);
                            bool rightClick = (input.MouseState?.RightButton == ButtonState.Pressed) && (input.PreviousMouseState?.RightButton == ButtonState.Released);
                            var kb = input.KeyboardState;
                            var prevKb = input.PreviousKeyboardState;
                            bool tabPress = (kb?.IsKeyDown(Keys.Tab) ?? false) && (prevKb?.IsKeyUp(Keys.Tab) ?? true);
                            bool spacePress = (kb?.IsKeyDown(Keys.Space) ?? false) && (prevKb?.IsKeyUp(Keys.Space) ?? true);
                            int qs = -1;
                            for (int i = 0; i < 9; i++)
                            {
                                var k = (Keys)((int)Keys.D1 + i);
                                if ((kb?.IsKeyDown(k) ?? false) && (prevKb?.IsKeyUp(k) ?? true)) { qs = i; break; }
                            }

                            var player = _gameSession.PlayerEntity;
                            System.Numerics.Vector3 targetPos = System.Numerics.Vector3.Zero;
                            float targetFacing = 0f;
                            if (player != null)
                            {
                                var transform = player.GetComponent<Runtime.Core.Interfaces.Components.ITransformComponent>();
                                targetPos = transform != null ? transform.Position : System.Numerics.Vector3.Zero;
                                targetFacing = transform != null ? transform.Facing : 0f;
                            }

                            EnsureChaseCameraInitialized();
                            if (_chaseCamera != null)
                            {
                                var xnTarget = new Vector3(targetPos.X, targetPos.Y, targetPos.Z);
                                _chaseCamera.SetTarget(xnTarget, xnTarget);
                                _chaseCamera.Update(deltaTime, xnTarget, Keyboard.GetState(), Mouse.GetState());
                                var viewXna = _chaseCamera.ViewMatrix;
                                var camPosXna = _chaseCamera.Position;
                                float aspect = vw > 0 && vh > 0 ? (float)vw / vh : 16f / 9f;
                                var projXna = Matrix.CreatePerspectiveFieldOfView((float)(Math.PI / 4), aspect, 0.1f, 1000f);
                                var view = XnaToNumerics(viewXna);
                                var proj = XnaToNumerics(projXna);
                                _gameSession.ProcessPlayerInput(mx, my, vw, vh, view, proj, leftClick, rightClick, tabPress, spacePress, qs);
                            }
                            else
                            {
                                System.Numerics.Vector3 camPos = new System.Numerics.Vector3(0, 5, -10);
                                System.Numerics.Vector3 lookAt = targetPos;
                                float dist = 8f, height = 4f;
                                camPos = new System.Numerics.Vector3(targetPos.X - (float)Math.Sin(targetFacing) * dist, targetPos.Y + height, targetPos.Z - (float)Math.Cos(targetFacing) * dist);
                                float aspect = vw > 0 && vh > 0 ? (float)vw / vh : 16f / 9f;
                                var view = System.Numerics.Matrix4x4.CreateLookAt(camPos, lookAt, System.Numerics.Vector3.UnitY);
                                var proj = System.Numerics.Matrix4x4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), aspect, 0.1f, 1000f);
                                _gameSession.ProcessPlayerInput(mx, my, vw, vh, view, proj, leftClick, rightClick, tabPress, spacePress, qs);
                            }
                        }

                        _gameSession?.Update(deltaTime);
                        break;
                }
            };

            // Set up draw callback for rendering
            Action drawAction = () =>
            {
                EnsureMainMenuInitialized();

                if (_graphicsBackend?.GraphicsDevice != null)
                {
                    var clearColor = _gameState == OdysseyGameState.InGame
                        ? new Runtime.Graphics.Color(0, 0, 0, 255)
                        : new Runtime.Graphics.Color(20, 30, 60, 255);
                    _graphicsBackend.GraphicsDevice.Clear(clearColor);
                }

                switch (_gameState)
                {
                    case OdysseyGameState.MainMenu:
                        if (_useFallbackMainMenu)
                        {
                            if (_mainMenuScreen != null)
                                _mainMenuScreen.Draw();
                            else
                                _fallbackMenuRenderer?.Draw();
                        }
                        else
                            _mainMenuGui?.Draw(null);
                        break;

                    case OdysseyGameState.CharacterCreation:
                        if (_characterCreationScreen != null && _graphicsBackend?.GraphicsDevice != null)
                        {
                            using (var sb = _graphicsBackend.GraphicsDevice.CreateSpriteBatch())
                            {
                                IFont font = null;
                                if (_graphicsBackend?.ContentManager != null)
                                {
                                    try
                                    {
                                        font = _graphicsBackend.ContentManager.Load<IFont>("Fonts/Arial");
                                    }
                                    catch { }
                                }
                                _characterCreationScreen.Draw(sb, font);
                            }
                        }
                        break;

                    case OdysseyGameState.LoadGameMenu:
                        _loadGameScreen?.Draw();
                        break;

                    case OdysseyGameState.Movies:
                        _moviesScreen?.Draw();
                        break;

                    case OdysseyGameState.Options:
                        _optionsScreen?.Draw();
                        break;

                    case OdysseyGameState.InGame:
                        DrawInGameScene();
                        if (_saveGameScreen != null)
                            _saveGameScreen.Draw();
                        else if (_optionsScreen != null)
                            _optionsScreen.Draw();
                        else if (_pauseMenuVisible && _pauseMenu != null)
                            _pauseMenu.Draw(default);
                        else if (_loadGameScreen != null && _loadScreenFromPause)
                            _loadGameScreen.Draw();
                        break;
                }
            };

            void DrawInGameScene()
            {
                if (_gameSession?.World?.CurrentArea == null || _graphicsBackend?.GraphicsDevice == null)
                    return;

                var area = _gameSession.World.CurrentArea as OdysseyArea;
                if (area == null)
                    return;

                try
                {
                    if (_cachedRoomMeshRenderer == null)
                        _cachedRoomMeshRenderer = _graphicsBackend.CreateRoomMeshRenderer();
                    if (_cachedBasicEffect == null)
                        _cachedBasicEffect = _graphicsBackend.GraphicsDevice.CreateBasicEffect();
                    if (_areaRenderContext == null)
                        _areaRenderContext = new OdysseyAreaRenderContext();

                    int w = _graphicsBackend.GraphicsDevice.Viewport.Width;
                    int h = _graphicsBackend.GraphicsDevice.Viewport.Height;
                    float aspect = w > 0 && h > 0 ? (float)w / h : 16f / 9f;

                    System.Numerics.Matrix4x4 view;
                    System.Numerics.Matrix4x4 proj;
                    System.Numerics.Vector3 camPos;
                    if (_chaseCamera != null)
                    {
                        view = XnaToNumerics(_chaseCamera.ViewMatrix);
                        proj = System.Numerics.Matrix4x4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), aspect, 0.1f, 1000f);
                        var p = _chaseCamera.Position;
                        camPos = new System.Numerics.Vector3(p.X, p.Y, p.Z);
                    }
                    else
                    {
                        var player = _gameSession.PlayerEntity;
                        camPos = new System.Numerics.Vector3(0, 5, -10);
                        System.Numerics.Vector3 lookAt = System.Numerics.Vector3.Zero;
                        if (player != null)
                        {
                            var transform = player.GetComponent<Runtime.Core.Interfaces.Components.ITransformComponent>();
                            var pos = transform != null ? transform.Position : System.Numerics.Vector3.Zero;
                            float facing = transform != null ? transform.Facing : 0f;
                            lookAt = pos;
                            float dist = 8f, height = 4f;
                            camPos = new System.Numerics.Vector3(pos.X - (float)Math.Sin(facing) * dist, pos.Y + height, pos.Z - (float)Math.Cos(facing) * dist);
                        }
                        view = System.Numerics.Matrix4x4.CreateLookAt(camPos, lookAt, System.Numerics.Vector3.UnitY);
                        proj = System.Numerics.Matrix4x4.CreatePerspectiveFieldOfView((float)(Math.PI / 4), aspect, 0.1f, 1000f);
                    }

                    _areaRenderContext.GraphicsDevice = _graphicsBackend.GraphicsDevice;
                    _areaRenderContext.RoomMeshRenderer = _cachedRoomMeshRenderer;
                    _areaRenderContext.BasicEffect = _cachedBasicEffect;
                    _areaRenderContext.ViewMatrix = view;
                    _areaRenderContext.ProjectionMatrix = proj;
                    _areaRenderContext.CameraPosition = camPos;

                    area.SetRenderContext(_areaRenderContext);
                    area.Render();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[OdysseyGame] InGame render error: " + ex.Message);
                }
            }

            // Run game loop
            _graphicsBackend.Run(updateAction, drawAction);
        }

        /// <summary>
        /// Ensures chase camera is initialized when InGame. Reva: k1_win_gog_swkotor.exe 0x004af630, k2_win_gog_aspyr_swkotor2.exe 0x004dcfb0.
        /// </summary>
        private void EnsureChaseCameraInitialized()
        {
            if (_chaseCamera != null) return;
            if (_gameSession?.World?.CurrentArea == null || _gameSession?.PlayerEntity == null) return;
            _chaseCamera = new ChaseCamera();
            var area = _gameSession.World.CurrentArea;
            var navMesh = area?.NavigationMesh;
            if (navMesh != null)
            {
                _chaseCamera.SetRaycastCallback((targetXna, camXna) =>
                {
                    var from = new System.Numerics.Vector3(targetXna.X, targetXna.Y, targetXna.Z);
                    var to = new System.Numerics.Vector3(camXna.X, camXna.Y, camXna.Z);
                    var dir = System.Numerics.Vector3.Normalize(to - from);
                    float dist = System.Numerics.Vector3.Distance(from, to);
                    return navMesh.Raycast(from, dir, dist, out _, out _);
                });
            }
            var player = _gameSession.PlayerEntity;
            var t = player?.GetComponent<Runtime.Core.Interfaces.Components.ITransformComponent>();
            if (t != null)
                _chaseCamera.Yaw = (float)(t.Facing * 180.0 / Math.PI);
        }

        /// <summary>
        /// Converts XNA Matrix to System.Numerics.Matrix4x4.
        /// </summary>
        private static System.Numerics.Matrix4x4 XnaToNumerics(Matrix m)
        {
            return new System.Numerics.Matrix4x4(m.M11, m.M12, m.M13, m.M14, m.M21, m.M22, m.M23, m.M24, m.M31, m.M32, m.M33, m.M34, m.M41, m.M42, m.M43, m.M44);
        }

        /// <summary>
        /// Stops main menu music (Reva: StopMenuMusic). Called when leaving main menu or disposing.
        /// </summary>
        private void StopMainMenuMusic()
        {
            try
            {
                _musicPlayer?.Stop();
                _mainMenuMusicStarted = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OdysseyGame] WARNING: Could not stop main menu music: {ex.Message}");
            }
        }

        /// <summary>
        /// Disposes of resources used by the game.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_gameSession != null)
                {
                    // Game session cleanup (if it implements IDisposable)
                    // Note: Check if GameSession needs explicit cleanup
                    _gameSession = null;
                }

                StopMainMenuMusic();
                if (_musicPlayer is IDisposable disposableMusic)
                {
                    disposableMusic.Dispose();
                    _musicPlayer = null;
                }

                if (_mainMenuScreen != null)
                {
                    _mainMenuScreen.Dispose();
                    _mainMenuScreen = null;
                }

                if (_mainMenuGui != null)
                {
                    _mainMenuGui.Dispose();
                    _mainMenuGui = null;
                }

                if (_fallbackMenuRenderer != null)
                {
                    _fallbackMenuRenderer.Dispose();
                    _fallbackMenuRenderer = null;
                }

                _loadGameScreen?.Dispose();
                _loadGameScreen = null;
                _characterCreationScreen = null;
                _moviesScreen?.Dispose();
                _moviesScreen = null;
                _optionsScreen?.Dispose();
                _optionsScreen = null;

                _cachedRoomMeshRenderer?.Dispose();
                _cachedRoomMeshRenderer = null;
                _cachedBasicEffect = null;
                _areaRenderContext = null;
                _chaseCamera = null;

                if (_vm != null)
                {
                    // NcsVm does not implement IDisposable
                    _vm = null;
                }

                if (_world != null)
                {
                    _world = null;
                }

                if (_graphicsBackend != null)
                {
                    _graphicsBackend.Dispose();
                }

                _disposed = true;
            }
        }
    }
}

