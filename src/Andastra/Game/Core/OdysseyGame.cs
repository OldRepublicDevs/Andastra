using System;
using Andastra.Runtime.Core;
using Andastra.Runtime.Core.Entities;
using Andastra.Runtime.Games.Common;
using Andastra.Game;
using Andastra.Game.Games.Odyssey.Game;
using Andastra.Game.Games.Odyssey;
using Andastra.Game.Games.Common;
using Andastra.Runtime.Graphics;
using Andastra.Game.Scripting.Interfaces;
using Andastra.Game.Scripting.VM;
using Andastra.Game.Graphics.MonoGame.GUI;
using BioWare.Extract;
using JetBrains.Annotations;
using Andastra.Game.Graphics.MonoGame.Graphics;

namespace Andastra.Game.Core
{
    /// <summary>
    /// Main game wrapper for Odyssey Engine games (KOTOR 1/2).
    /// Coordinates graphics backend, engine initialization, and game session management.
    /// </summary>
    /// <remarks>
    /// Odyssey Game Wrapper:
    /// - [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): 0x00404250 @ 0x00404250 (WinMain equivalent, initializes game)
    /// - Original implementation: Initializes engine, creates game session, runs game loop
    /// - Graphics backend: Provides cross-platform graphics abstraction (MonoGame, Stride)
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
        private bool _isInMainMenu = true;
        private bool _disposed;
        private bool _initialized;

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
            // Based on swkotor.exe and swkotor2.exe: Script globals system initializes global variables
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
                if (_mainMenuGui != null)
                {
                    return;
                }

                if (_graphicsBackend?.GraphicsDevice == null)
                {
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
                    Console.WriteLine($"[OdysseyGame] ERROR initializing installation: {ex}");
                    return;
                }

                // Get the raw MonoGame GraphicsDevice
                Microsoft.Xna.Framework.Graphics.GraphicsDevice rawDevice = null;
                if (_graphicsBackend.GraphicsDevice is MonoGameGraphicsDevice mgDevice)
                {
                    rawDevice = mgDevice.Device;
                }
                else
                {
                    Console.WriteLine("[OdysseyGame] ERROR: Graphics backend is not MonoGame, cannot create KotorGuiManager");
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
                string mainMenuResRef = "MAINMENU";
                bool loaded = _mainMenuGui.LoadGui(mainMenuResRef, _settings.Width, _settings.Height);
                
                if (!loaded)
                {
                    // Try alternate GUI files based on game type
                    mainMenuResRef = _settings.Game == KotorGame.K1 ? "mainmenu16x12" : "mainmenu8x6_p";
                    loaded = _mainMenuGui.LoadGui(mainMenuResRef, _settings.Width, _settings.Height);
                }

                if (loaded)
                {
                    _mainMenuGui.SetCurrentGui(mainMenuResRef);
                    Console.WriteLine($"[OdysseyGame] Loaded main menu GUI: {mainMenuResRef}");

                    // Wire up button click handlers
                    _mainMenuGui.OnButtonClicked += (sender, e) =>
                    {
                        try
                        {
                            Console.WriteLine($"[OdysseyGame] Main menu button clicked: {e.ButtonTag}");
                            
                            if (string.Equals(e.ButtonTag, "BTN_NEWGAME", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(e.ButtonTag, "BTN_LOADGAME", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("[OdysseyGame] Start/Load Game selected");
                                _isInMainMenu = false;
                                _gameSession?.StartNewGame();
                            }
                            else if (string.Equals(e.ButtonTag, "BTN_OPTIONS", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("[OdysseyGame] Options selected (not implemented yet)");
                            }
                            else if (string.Equals(e.ButtonTag, "BTN_EXIT", StringComparison.OrdinalIgnoreCase) ||
                                     string.Equals(e.ButtonTag, "BTN_WARP", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("[OdysseyGame] Exit selected");
                                _graphicsBackend?.Exit();
                            }
                            else if (string.Equals(e.ButtonTag, "BTN_MOVIES", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine("[OdysseyGame] Movies selected (not implemented yet)");
                            }
                            else
                            {
                                Console.WriteLine($"[OdysseyGame] Unknown button: {e.ButtonTag}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[OdysseyGame] ERROR handling button click: {ex}");
                        }
                    };
                }
                else
                {
                    Console.WriteLine($"[OdysseyGame] WARNING: Failed to load main menu GUI");
                }
            }

            // Set up update callback for game logic
            Action<float> updateAction = (deltaTime) =>
            {
                EnsureMainMenuInitialized();

                if (_isInMainMenu)
                {
                    _mainMenuGui?.Update(deltaTime);
                    return;
                }

                _gameSession?.Update(deltaTime);
            };

            // Set up draw callback for rendering
            Action drawAction = () =>
            {
                EnsureMainMenuInitialized();

                // MonoGame/Stride expect actual rendering to occur in Draw, not Update.
                // If Draw does nothing, you get a blank window.
                if (_graphicsBackend?.GraphicsDevice != null)
                {
                    var clearColor = _isInMainMenu
                        ? new Color(20, 30, 60, 255)
                        : new Color(0, 0, 0, 255);
                    _graphicsBackend.GraphicsDevice.Clear(clearColor);
                }

                if (_isInMainMenu)
                {
                    _mainMenuGui?.Draw(null);
                }
            };

            // Run game loop
            _graphicsBackend.Run(updateAction, drawAction);
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

                if (_mainMenuGui != null)
                {
                    _mainMenuGui.Dispose();
                    _mainMenuGui = null;
                }

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

