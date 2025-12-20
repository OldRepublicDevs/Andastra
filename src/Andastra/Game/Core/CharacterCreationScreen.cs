using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Andastra.Parsing;
using Andastra.Parsing.Installation;
using Andastra.Runtime.Core;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Engines.Odyssey.Data;

namespace Andastra.Runtime.Game.Core
{
    /// <summary>
    /// Character creation screen for KOTOR 1 and KOTOR 2.
    /// </summary>
    /// <remarks>
    /// Character Creation Screen:
    /// - Based on swkotor.exe and swkotor2.exe character generation system
    /// - GUI Panel: "maincg" (character generation)
    /// - K1 Music: "mus_theme_rep", K2 Music: "mus_main"
    /// - Load Screen: K1 uses "load_chargen", K2 uses "load_default"
    /// - Flow: Main Menu → Character Creation → Module Load
    /// 
    /// Based on reverse engineering of:
    /// - swkotor.exe: Character generation functions
    /// - swkotor2.exe: Character generation functions
    /// - vendor/reone: CharacterGeneration class implementation
    /// 
    /// Character Creation Steps:
    /// 1. Class Selection (Scout, Soldier, Scoundrel for K1; Jedi Guardian, Jedi Sentinel, Jedi Consular for K2)
    /// 2. Quick or Custom (Quick uses defaults, Custom allows full customization)
    /// 3. Attributes (STR, DEX, CON, INT, WIS, CHA)
    /// 4. Skills (based on class and INT)
    /// 5. Feats (based on class)
    /// 6. Portrait Selection
    /// 7. Name Entry
    /// 8. Finish → Create Player Entity → Load Module
    /// </remarks>
    public class CharacterCreationScreen
    {
        private readonly IGraphicsDevice _graphicsDevice;
        private readonly Installation _installation;
        private readonly KotorGame _game;
        private readonly BaseGuiManager _guiManager;
        private readonly Action<CharacterCreationData> _onComplete;
        private readonly Action _onCancel;
        private readonly GameDataManager _gameDataManager;
        
        private CharacterCreationData _characterData;
        private CreationStep _currentStep = CreationStep.ClassSelection;
        private bool _isQuickMode = false;
        private int _selectedClassIndex = 0;
        private int _selectedAttributeIndex = 0;
        private int _previousAppearance = 1;
        private IKeyboardState _previousKeyboardState;
        private IMouseState _previousMouseState;
        private float _modelRotationAngle = 0f;
        private bool _needsModelUpdate = true;
        private bool _guiLoaded = false;
        private ITexture2D _pixelTexture;
        
        // Feat selection state
        private List<int> _availableFeatIds = new List<int>();
        private int _selectedFeatIndex = 0;
        private int _featScrollOffset = 0;
        
        /// <summary>
        /// Character creation steps.
        /// </summary>
        private enum CreationStep
        {
            ClassSelection,
            QuickOrCustom,
            Attributes,
            Skills,
            Feats,
            Portrait,
            Name,
            Summary
        }
        
        /// <summary>
        /// Available classes for the current game.
        /// </summary>
        private CharacterClass[] GetAvailableClasses()
        {
            if (_game == KotorGame.K1)
            {
                return new CharacterClass[] { CharacterClass.Scout, CharacterClass.Soldier, CharacterClass.Scoundrel };
            }
            else
            {
                return new CharacterClass[] { CharacterClass.JediGuardian, CharacterClass.JediSentinel, CharacterClass.JediConsular };
            }
        }
        
        /// <summary>
        /// Creates a new character creation screen.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device for rendering.</param>
        /// <param name="installation">Game installation for loading resources.</param>
        /// <param name="game">KOTOR game version (K1 or K2).</param>
        /// <param name="guiManager">GUI manager for loading and rendering GUI panels.</param>
        /// <param name="onComplete">Callback when character creation is complete.</param>
        /// <param name="onCancel">Callback when character creation is cancelled.</param>
        public CharacterCreationScreen(
            IGraphicsDevice graphicsDevice,
            Installation installation,
            KotorGame game,
            BaseGuiManager guiManager,
            Action<CharacterCreationData> onComplete,
            Action onCancel)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _installation = installation ?? throw new ArgumentNullException(nameof(installation));
            _game = game;
            _guiManager = guiManager ?? throw new ArgumentNullException(nameof(guiManager));
            _onComplete = onComplete ?? throw new ArgumentNullException(nameof(onComplete));
            _onCancel = onCancel ?? throw new ArgumentNullException(nameof(onCancel));
            _gameDataManager = new GameDataManager(installation);
            
            // Initialize character data with defaults
            _characterData = new CharacterCreationData
            {
                Game = game,
                Class = game == KotorGame.K1 ? CharacterClass.Scout : CharacterClass.JediGuardian,
                Gender = Gender.Male,
                Appearance = 1,
                Portrait = 0,
                Name = string.Empty,
                Strength = 14,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 12,
                Wisdom = 12,
                Charisma = 12,
                SelectedFeats = new List<int>()
            };
            
            // Initialize available feats for the default class
            UpdateAvailableFeats();
            
            // Load the maincg GUI panel
            // Based on swkotor.exe and swkotor2.exe: Character creation uses "maincg" GUI panel
            // Original implementation: GUI panel is loaded when character creation screen is initialized
            int screenWidth = _graphicsDevice.Viewport.Width > 0 ? _graphicsDevice.Viewport.Width : 800;
            int screenHeight = _graphicsDevice.Viewport.Height > 0 ? _graphicsDevice.Viewport.Height : 600;
            
            _guiLoaded = _guiManager.LoadGui("maincg", screenWidth, screenHeight);
            if (_guiLoaded)
            {
                _guiManager.SetCurrentGui("maincg");
            }
            else
            {
                System.Console.WriteLine("[CharacterCreationScreen] WARNING: Failed to load maincg GUI panel, character creation UI may not display correctly");
            }
            
            // Create a 1x1 pixel texture for drawing rectangles
            byte[] pixelData = new byte[] { 255, 255, 255, 255 }; // White pixel
            _pixelTexture = _graphicsDevice.CreateTexture2D(1, 1, pixelData);
        }
        
        /// <summary>
        /// Updates the character creation screen.
        /// </summary>
        /// <remarks>
        /// Character Creation Update Implementation:
        /// - Based on swkotor.exe and swkotor2.exe character generation update logic
        /// - Handles input for current step (keyboard and mouse)
        /// - Updates character model preview when appearance changes
        /// - Handles button clicks (Next, Back, Cancel, Finish)
        /// - Manages step navigation and validation
        /// - Processes step-specific input (class selection, attributes, skills, feats, portrait, name)
        /// 
        /// Based on reverse engineering of:
        /// - swkotor.exe: Character generation input handling and update loop
        /// - swkotor2.exe: Character generation input handling and update loop
        /// - vendor/reone: CharacterGeneration::handle() and CharacterGeneration::update() methods
        /// </remarks>
        public void Update(float deltaTime, IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Initialize previous states on first call
            if (_previousKeyboardState == null)
            {
                _previousKeyboardState = keyboardState;
            }
            if (_previousMouseState == null)
            {
                _previousMouseState = mouseState;
            }
            
            // Update model rotation for preview animation
            _modelRotationAngle += deltaTime * 0.5f; // Slow rotation
            if (_modelRotationAngle > 2.0f * (float)Math.PI)
            {
                _modelRotationAngle -= 2.0f * (float)Math.PI;
            }
            
            // Check for appearance changes and update model if needed
            if (_characterData.Appearance != _previousAppearance)
            {
                _previousAppearance = _characterData.Appearance;
                _needsModelUpdate = true;
            }
            
            // Handle global input (Cancel/Escape)
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Escape))
            {
                Cancel();
                return;
            }
            
            // Handle step-specific input
            switch (_currentStep)
            {
                case CreationStep.ClassSelection:
                    HandleClassSelectionInput(keyboardState, mouseState);
                    break;
                case CreationStep.QuickOrCustom:
                    HandleQuickOrCustomInput(keyboardState, mouseState);
                    break;
                case CreationStep.Attributes:
                    HandleAttributesInput(keyboardState, mouseState);
                    break;
                case CreationStep.Skills:
                    HandleSkillsInput(keyboardState, mouseState);
                    break;
                case CreationStep.Feats:
                    HandleFeatsInput(keyboardState, mouseState);
                    break;
                case CreationStep.Portrait:
                    HandlePortraitInput(keyboardState, mouseState);
                    break;
                case CreationStep.Name:
                    HandleNameInput(keyboardState, mouseState);
                    break;
                case CreationStep.Summary:
                    HandleSummaryInput(keyboardState, mouseState);
                    break;
            }
            
            // Update previous states for next frame
            _previousKeyboardState = keyboardState;
            _previousMouseState = mouseState;
        }
        
        /// <summary>
        /// Checks if a key was just pressed (not held).
        /// </summary>
        private bool IsKeyPressed(IKeyboardState current, IKeyboardState previous, Keys key)
        {
            return current.IsKeyDown(key) && previous.IsKeyUp(key);
        }
        
        /// <summary>
        /// Checks if a mouse button was just clicked (not held).
        /// </summary>
        private bool IsMouseButtonClicked(IMouseState current, IMouseState previous, MouseButton button)
        {
            return current.IsButtonDown(button) && previous.IsButtonUp(button);
        }
        
        /// <summary>
        /// Handles input for class selection step.
        /// </summary>
        private void HandleClassSelectionInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            CharacterClass[] availableClasses = GetAvailableClasses();
            
            // Keyboard navigation
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Up) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Left))
            {
                _selectedClassIndex = (_selectedClassIndex - 1 + availableClasses.Length) % availableClasses.Length;
                _characterData.Class = availableClasses[_selectedClassIndex];
                UpdateAvailableFeats();
                _needsModelUpdate = true;
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Down) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Right))
            {
                _selectedClassIndex = (_selectedClassIndex + 1) % availableClasses.Length;
                _characterData.Class = availableClasses[_selectedClassIndex];
                UpdateAvailableFeats();
                _needsModelUpdate = true;
            }
            
            // Confirm selection
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                GoToNextStep();
            }
        }
        
        /// <summary>
        /// Handles input for Quick or Custom selection step.
        /// </summary>
        private void HandleQuickOrCustomInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Keyboard navigation
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Left) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Up))
            {
                _isQuickMode = true;
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Right) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Down))
            {
                _isQuickMode = false;
            }
            
            // Confirm selection
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                if (_isQuickMode)
                {
                    // Quick mode: skip to portrait/name, use defaults for attributes/skills/feats
                    _currentStep = CreationStep.Portrait;
                }
                else
                {
                    // Custom mode: go to attributes
                    _currentStep = CreationStep.Attributes;
                }
            }
            
            // Back button
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Handles input for attributes step.
        /// </summary>
        private void HandleAttributesInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Attribute navigation (6 attributes: STR, DEX, CON, INT, WIS, CHA)
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Up))
            {
                _selectedAttributeIndex = (_selectedAttributeIndex - 1 + 6) % 6;
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Down))
            {
                _selectedAttributeIndex = (_selectedAttributeIndex + 1) % 6;
            }
            
            // Attribute adjustment
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Left))
            {
                AdjustAttribute(_selectedAttributeIndex, -1);
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Right))
            {
                AdjustAttribute(_selectedAttributeIndex, 1);
            }
            
            // Next/Back
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                GoToNextStep();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Adjusts an attribute value with validation.
        /// </summary>
        private void AdjustAttribute(int attributeIndex, int delta)
        {
            // Calculate total points spent
            int totalPoints = _characterData.Strength + _characterData.Dexterity + _characterData.Constitution +
                            _characterData.Intelligence + _characterData.Wisdom + _characterData.Charisma;
            int basePoints = 6 * 8; // 6 attributes * 8 base = 48 points
            int pointsSpent = totalPoints - basePoints;
            int maxPoints = 30; // Maximum points that can be allocated
            
            // Get current attribute value
            int currentValue = GetAttributeValue(attributeIndex);
            int newValue = currentValue + delta;
            
            // Validate: attributes must be between 8 and 18 (or 20 with bonuses)
            if (newValue < 8 || newValue > 20)
            {
                return;
            }
            
            // Validate: check point allocation
            int newPointsSpent = pointsSpent + delta;
            if (newPointsSpent < 0 || newPointsSpent > maxPoints)
            {
                return;
            }
            
            // Apply change
            SetAttributeValue(attributeIndex, newValue);
        }
        
        /// <summary>
        /// Gets the value of an attribute by index.
        /// </summary>
        private int GetAttributeValue(int index)
        {
            switch (index)
            {
                case 0: return _characterData.Strength;
                case 1: return _characterData.Dexterity;
                case 2: return _characterData.Constitution;
                case 3: return _characterData.Intelligence;
                case 4: return _characterData.Wisdom;
                case 5: return _characterData.Charisma;
                default: return 12;
            }
        }
        
        /// <summary>
        /// Sets the value of an attribute by index.
        /// </summary>
        private void SetAttributeValue(int index, int value)
        {
            switch (index)
            {
                case 0: _characterData.Strength = value; break;
                case 1: _characterData.Dexterity = value; break;
                case 2: _characterData.Constitution = value; break;
                case 3: _characterData.Intelligence = value; break;
                case 4: _characterData.Wisdom = value; break;
                case 5: _characterData.Charisma = value; break;
            }
        }
        
        /// <summary>
        /// Handles input for skills step.
        /// </summary>
        private void HandleSkillsInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Skills are typically auto-calculated based on class and INT, but allow manual adjustment
            // For now, just allow navigation
            
            // Next/Back
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                GoToNextStep();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Handles input for feats step.
        /// Based on swkotor.exe and swkotor2.exe: Feat selection allows browsing available feats and selecting/deselecting them
        /// - Original implementation: Up/Down arrows navigate feat list, Enter/Space selects/deselects feat, Left/Right scrolls description
        /// - Feats are filtered by class and prerequisites
        /// </summary>
        private void HandleFeatsInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Feat list navigation
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Up))
            {
                if (_selectedFeatIndex > 0)
                {
                    _selectedFeatIndex--;
                    // Auto-scroll if selection is above visible area
                    if (_selectedFeatIndex < _featScrollOffset)
                    {
                        _featScrollOffset = _selectedFeatIndex;
                    }
                }
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Down))
            {
                if (_selectedFeatIndex < _availableFeatIds.Count - 1)
                {
                    _selectedFeatIndex++;
                    // Auto-scroll if selection is below visible area (assuming ~10 visible items)
                    int maxVisible = 10;
                    if (_selectedFeatIndex >= _featScrollOffset + maxVisible)
                    {
                        _featScrollOffset = _selectedFeatIndex - maxVisible + 1;
                    }
                }
            }
            
            // Select/Deselect feat
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                if (_selectedFeatIndex >= 0 && _selectedFeatIndex < _availableFeatIds.Count)
                {
                    int featId = _availableFeatIds[_selectedFeatIndex];
                    if (_characterData.SelectedFeats.Contains(featId))
                    {
                        _characterData.SelectedFeats.Remove(featId);
                    }
                    else
                    {
                        // Check if feat meets prerequisites
                        FeatData featData = _gameDataManager.GetFeat(featId);
                        if (featData != null && MeetsFeatPrerequisites(featData))
                        {
                            _characterData.SelectedFeats.Add(featId);
                        }
                    }
                }
            }
            
            // Next/Back
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Tab))
            {
                GoToNextStep();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Handles input for portrait selection step.
        /// </summary>
        private void HandlePortraitInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Portrait navigation
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Left) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Up))
            {
                _characterData.Portrait = Math.Max(0, _characterData.Portrait - 1);
                _needsModelUpdate = true;
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Right) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Down))
            {
                _characterData.Portrait = Math.Min(99, _characterData.Portrait + 1); // Assume max 100 portraits
                _needsModelUpdate = true;
            }
            
            // Next/Back
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                GoToNextStep();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Handles input for name entry step.
        /// </summary>
        private void HandleNameInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Name entry: handle text input
            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                if (IsKeyPressed(keyboardState, _previousKeyboardState, key))
                {
                    // Handle backspace
                    if (key == Keys.Back && _characterData.Name.Length > 0)
                    {
                        _characterData.Name = _characterData.Name.Substring(0, _characterData.Name.Length - 1);
                    }
                    // Handle printable characters
                    else if (key >= Keys.A && key <= Keys.Z)
                    {
                        // Check for shift to get uppercase
                        bool isShift = keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift);
                        char c = (char)((int)'a' + (int)(key - Keys.A));
                        if (isShift)
                        {
                            c = char.ToUpper(c);
                        }
                        _characterData.Name += c;
                    }
                    else if (key >= Keys.D0 && key <= Keys.D9)
                    {
                        char c = (char)((int)'0' + (int)(key - Keys.D0));
                        _characterData.Name += c;
                    }
                    else if (key == Keys.Space)
                    {
                        _characterData.Name += " ";
                    }
                }
            }
            
            // Limit name length
            if (_characterData.Name.Length > 32)
            {
                _characterData.Name = _characterData.Name.Substring(0, 32);
            }
            
            // Next/Back/Finish
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter))
            {
                if (string.IsNullOrWhiteSpace(_characterData.Name))
                {
                    _characterData.Name = "Player"; // Default name
                }
                GoToNextStep();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Handles input for summary step.
        /// </summary>
        private void HandleSummaryInput(IKeyboardState keyboardState, IMouseState mouseState)
        {
            // Finish or go back
            if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Enter) || IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Space))
            {
                Finish();
            }
            else if (IsKeyPressed(keyboardState, _previousKeyboardState, Keys.Back))
            {
                GoToPreviousStep();
            }
        }
        
        /// <summary>
        /// Advances to the next step in character creation.
        /// </summary>
        private void GoToNextStep()
        {
            switch (_currentStep)
            {
                case CreationStep.ClassSelection:
                    _currentStep = CreationStep.QuickOrCustom;
                    break;
                case CreationStep.QuickOrCustom:
                    if (_isQuickMode)
                    {
                        _currentStep = CreationStep.Portrait;
                    }
                    else
                    {
                        _currentStep = CreationStep.Attributes;
                    }
                    break;
                case CreationStep.Attributes:
                    _currentStep = CreationStep.Skills;
                    break;
                case CreationStep.Skills:
                    _currentStep = CreationStep.Feats;
                    break;
                case CreationStep.Feats:
                    _currentStep = CreationStep.Portrait;
                    break;
                case CreationStep.Portrait:
                    _currentStep = CreationStep.Name;
                    break;
                case CreationStep.Name:
                    _currentStep = CreationStep.Summary;
                    break;
                case CreationStep.Summary:
                    Finish();
                    break;
            }
        }
        
        /// <summary>
        /// Returns to the previous step in character creation.
        /// </summary>
        private void GoToPreviousStep()
        {
            switch (_currentStep)
            {
                case CreationStep.QuickOrCustom:
                    _currentStep = CreationStep.ClassSelection;
                    break;
                case CreationStep.Attributes:
                    _currentStep = CreationStep.QuickOrCustom;
                    break;
                case CreationStep.Skills:
                    _currentStep = CreationStep.Attributes;
                    break;
                case CreationStep.Feats:
                    _currentStep = CreationStep.Skills;
                    break;
                case CreationStep.Portrait:
                    if (_isQuickMode)
                    {
                        _currentStep = CreationStep.QuickOrCustom;
                    }
                    else
                    {
                        _currentStep = CreationStep.Feats;
                    }
                    break;
                case CreationStep.Name:
                    _currentStep = CreationStep.Portrait;
                    break;
                case CreationStep.Summary:
                    _currentStep = CreationStep.Name;
                    break;
            }
        }
        
        /// <summary>
        /// Draws the character creation screen.
        /// </summary>
        /// <remarks>
        /// Character Creation Rendering Implementation:
        /// - Based on swkotor.exe and swkotor2.exe character generation rendering
        /// - Renders "maincg" GUI panel as the base UI
        /// - Renders character model preview in 3D viewport
        /// - Renders step-specific UI elements (class selection, attributes, skills, feats, portrait, name, summary)
        /// - Renders navigation buttons (Next, Back, Cancel, Finish)
        /// - Updates GUI control states based on current step and selections
        /// 
        /// Based on reverse engineering of:
        /// - swkotor.exe: Character generation rendering functions
        /// - swkotor2.exe: Character generation rendering functions
        /// - vendor/reone: CharacterGeneration::draw() method
        /// </remarks>
        /// <param name="spriteBatch">Sprite batch for 2D rendering.</param>
        /// <param name="font">Font for text rendering.</param>
        public void Draw(ISpriteBatch spriteBatch, IFont font)
        {
            if (spriteBatch == null)
            {
                return;
            }
            
            // Render the maincg GUI panel
            // Based on swkotor.exe and swkotor2.exe: "maincg" GUI panel is the base UI for character creation
            // The GUI manager handles rendering of the GUI panel using its internal sprite batch
            // We render step-specific UI and buttons on top using the provided sprite batch
            if (_guiLoaded)
            {
                // Render GUI panel using GUI manager's Draw method
                // Note: GUI manager uses its own sprite batch, so we render it first
                // Then we render step-specific UI on top using the provided sprite batch
                _guiManager.Draw(null); // Pass null for gameTime as it's not used in base implementation
            }
            
            // Begin sprite batch for step-specific UI and buttons
            spriteBatch.Begin();
            
            try
            {
                // Render character model preview
                // Based on swkotor.exe and swkotor2.exe: Character model is rendered in a 3D viewport
                // Original implementation: 3D model is rendered with rotation animation
                // For now, render a placeholder until 3D rendering system is available
                RenderCharacterModelPreview(spriteBatch);
                
                // Render step-specific UI based on current step
                // Based on swkotor.exe and swkotor2.exe: Each step has specific UI elements
                switch (_currentStep)
                {
                    case CreationStep.ClassSelection:
                        RenderClassSelectionUI(spriteBatch, font);
                        break;
                    case CreationStep.QuickOrCustom:
                        RenderQuickOrCustomUI(spriteBatch, font);
                        break;
                    case CreationStep.Attributes:
                        RenderAttributesUI(spriteBatch, font);
                        break;
                    case CreationStep.Skills:
                        RenderSkillsUI(spriteBatch, font);
                        break;
                    case CreationStep.Feats:
                        RenderFeatsUI(spriteBatch, font);
                        break;
                    case CreationStep.Portrait:
                        RenderPortraitUI(spriteBatch, font);
                        break;
                    case CreationStep.Name:
                        RenderNameUI(spriteBatch, font);
                        break;
                    case CreationStep.Summary:
                        RenderSummaryUI(spriteBatch, font);
                        break;
                }
                
                // Render navigation buttons (Next, Back, Cancel, Finish)
                // Based on swkotor.exe and swkotor2.exe: Navigation buttons are always visible
                RenderNavigationButtons(spriteBatch, font);
            }
            finally
            {
                spriteBatch.End();
            }
        }
        
        /// <summary>
        /// Renders the character model preview.
        /// Based on swkotor.exe and swkotor2.exe: Character model is rendered in a 3D viewport with rotation
        /// - Original implementation: 3D model is rendered using DirectX with camera positioned for character preview
        /// - Model rotates slowly to show character from different angles
        /// - Model updates when appearance, gender, or class changes
        /// - Preview viewport is typically positioned on the right side of the screen
        /// </summary>
        private void RenderCharacterModelPreview(ISpriteBatch spriteBatch)
        {
            // TODO: PLACEHOLDER - Implement 3D character model rendering
            // This requires 3D rendering system integration
            // For now, render a placeholder rectangle indicating where the model preview would be
            // Based on swkotor.exe and swkotor2.exe: Model preview is typically at position (500, 100) with size (300, 400)
            
            int previewX = _graphicsDevice.Viewport.Width - 350;
            int previewY = 100;
            int previewWidth = 300;
            int previewHeight = 400;
            
            // Draw placeholder background
            Color previewBgColor = new Color(30, 30, 30, 200);
            DrawRectangle(spriteBatch, new Rectangle(previewX, previewY, previewWidth, previewHeight), previewBgColor);
            
            // Draw placeholder border
            Color previewBorderColor = new Color(100, 100, 100, 255);
            DrawRectangleOutline(spriteBatch, new Rectangle(previewX, previewY, previewWidth, previewHeight), previewBorderColor, 2);
            
            // Draw placeholder text (if font is available)
            // Note: This is a placeholder until 3D model rendering is implemented
        }
        
        /// <summary>
        /// Renders the class selection UI.
        /// Based on swkotor.exe and swkotor2.exe: Class selection displays available classes with descriptions
        /// - K1: Scout, Soldier, Scoundrel
        /// - K2: Jedi Guardian, Jedi Sentinel, Jedi Consular
        /// </summary>
        private void RenderClassSelectionUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            CharacterClass[] availableClasses = GetAvailableClasses();
            int startY = 150;
            int itemHeight = 40;
            int selectedY = startY + (_selectedClassIndex * itemHeight);
            
            // Render class options
            for (int i = 0; i < availableClasses.Length; i++)
            {
                CharacterClass characterClass = availableClasses[i];
                string className = GetClassName(characterClass);
                bool isSelected = (i == _selectedClassIndex);
                
                int y = startY + (i * itemHeight);
                Color textColor = isSelected ? Color.Yellow : Color.White;
                
                // Draw selection indicator
                if (isSelected)
                {
                    DrawRectangle(spriteBatch, new Rectangle(50, y - 2, 400, itemHeight), new Color(100, 100, 100, 100));
                }
                
                // Draw class name
                spriteBatch.DrawString(font, className, new Vector2(60, y), textColor);
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Select Class", new Vector2(50, 100), Color.White);
        }
        
        /// <summary>
        /// Renders the Quick or Custom selection UI.
        /// Based on swkotor.exe and swkotor2.exe: Player chooses between Quick (defaults) or Custom (full customization)
        /// </summary>
        private void RenderQuickOrCustomUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            int startY = 200;
            int itemHeight = 50;
            
            // Render Quick option
            Color quickColor = _isQuickMode ? Color.Yellow : Color.White;
            spriteBatch.DrawString(font, "Quick Character", new Vector2(50, startY), quickColor);
            if (_isQuickMode)
            {
                DrawRectangle(spriteBatch, new Rectangle(45, startY - 2, 300, itemHeight), new Color(100, 100, 100, 100));
            }
            
            // Render Custom option
            Color customColor = !_isQuickMode ? Color.Yellow : Color.White;
            spriteBatch.DrawString(font, "Custom Character", new Vector2(50, startY + itemHeight), customColor);
            if (!_isQuickMode)
            {
                DrawRectangle(spriteBatch, new Rectangle(45, startY + itemHeight - 2, 300, itemHeight), new Color(100, 100, 100, 100));
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Character Creation Mode", new Vector2(50, 100), Color.White);
        }
        
        /// <summary>
        /// Renders the attributes UI.
        /// Based on swkotor.exe and swkotor2.exe: Attributes are displayed with current values and adjustment controls
        /// - Attributes: STR, DEX, CON, INT, WIS, CHA
        /// - Each attribute can be adjusted within valid ranges (8-20)
        /// - Total points available for allocation are displayed
        /// </summary>
        private void RenderAttributesUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            string[] attributeNames = { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };
            int[] attributeValues = {
                _characterData.Strength,
                _characterData.Dexterity,
                _characterData.Constitution,
                _characterData.Intelligence,
                _characterData.Wisdom,
                _characterData.Charisma
            };
            
            int startY = 150;
            int itemHeight = 35;
            
            // Calculate points spent
            int totalPoints = _characterData.Strength + _characterData.Dexterity + _characterData.Constitution +
                            _characterData.Intelligence + _characterData.Wisdom + _characterData.Charisma;
            int basePoints = 6 * 8; // 6 attributes * 8 base = 48 points
            int pointsSpent = totalPoints - basePoints;
            int maxPoints = 30;
            int pointsRemaining = maxPoints - pointsSpent;
            
            // Render points remaining
            string pointsText = $"Points Remaining: {pointsRemaining} / {maxPoints}";
            spriteBatch.DrawString(font, pointsText, new Vector2(50, 100), Color.Cyan);
            
            // Render attributes
            for (int i = 0; i < attributeNames.Length; i++)
            {
                bool isSelected = (i == _selectedAttributeIndex);
                int y = startY + (i * itemHeight);
                Color textColor = isSelected ? Color.Yellow : Color.White;
                
                // Draw selection indicator
                if (isSelected)
                {
                    DrawRectangle(spriteBatch, new Rectangle(45, y - 2, 450, itemHeight), new Color(100, 100, 100, 100));
                }
                
                // Draw attribute name and value
                string attributeText = $"{attributeNames[i]}: {attributeValues[i]}";
                spriteBatch.DrawString(font, attributeText, new Vector2(60, y), textColor);
                
                // Draw adjustment indicators
                if (isSelected)
                {
                    spriteBatch.DrawString(font, "< - >", new Vector2(350, y), Color.Gray);
                }
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Adjust Attributes", new Vector2(50, 70), Color.White);
        }
        
        /// <summary>
        /// Renders the skills UI.
        /// Based on swkotor.exe and swkotor2.exe: Skills are displayed with current ranks and available points
        /// - Skills are based on class and Intelligence modifier
        /// - Available skill points are calculated and displayed
        /// </summary>
        private void RenderSkillsUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Skills", new Vector2(50, 100), Color.White);
            spriteBatch.DrawString(font, "Skills are calculated based on class and Intelligence.", new Vector2(50, 150), Color.Gray);
            
            // TODO: PLACEHOLDER - Implement full skills UI with skill list and ranks
            // This requires skill system integration
        }
        
        /// <summary>
        /// Renders the feats UI.
        /// Based on swkotor.exe and swkotor2.exe: Feats are displayed with available feats for selection
        /// - Feats are based on class (starting feats from featgain.2da)
        /// - Available feats are listed with names, descriptions, and selection status
        /// - Selected feats are highlighted
        /// - Original implementation: Feat list scrolls, descriptions shown in detail panel
        /// </summary>
        private void RenderFeatsUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Select Feats", new Vector2(50, 50), Color.White);
            
            // Render instruction text
            string instructionText = "Use Up/Down to navigate, Enter/Space to select/deselect, Tab to continue";
            spriteBatch.DrawString(font, instructionText, new Vector2(50, 80), Color.Gray);
            
            // Render selected feats count
            string selectedCountText = $"Selected: {_characterData.SelectedFeats.Count}";
            spriteBatch.DrawString(font, selectedCountText, new Vector2(50, 105), Color.Cyan);
            
            // Render feat list
            int listStartY = 135;
            int itemHeight = 30;
            int maxVisibleItems = 12;
            int listWidth = 450;
            
            // Calculate visible range
            int visibleStart = _featScrollOffset;
            int visibleEnd = Math.Min(visibleStart + maxVisibleItems, _availableFeatIds.Count);
            
            // Render scrollbar hint if needed
            if (_availableFeatIds.Count > maxVisibleItems)
            {
                string scrollHint = $"Showing {visibleStart + 1}-{visibleEnd} of {_availableFeatIds.Count}";
                spriteBatch.DrawString(font, scrollHint, new Vector2(50, listStartY + (maxVisibleItems * itemHeight) + 5), Color.DarkGray);
            }
            
            // Render visible feats
            for (int i = visibleStart; i < visibleEnd; i++)
            {
                int featId = _availableFeatIds[i];
                FeatData featData = _gameDataManager.GetFeat(featId);
                if (featData == null)
                {
                    continue;
                }
                
                bool isSelected = (i == _selectedFeatIndex);
                bool isTaken = _characterData.SelectedFeats.Contains(featId);
                bool meetsPrereqs = MeetsFeatPrerequisites(featData);
                
                int y = listStartY + ((i - visibleStart) * itemHeight);
                
                // Draw selection indicator background
                if (isSelected)
                {
                    DrawRectangle(spriteBatch, new Rectangle(45, y - 2, listWidth, itemHeight), new Color(100, 100, 100, 150));
                }
                
                // Draw taken indicator (different background for selected feats)
                if (isTaken)
                {
                    DrawRectangle(spriteBatch, new Rectangle(45, y - 2, 20, itemHeight), new Color(40, 120, 40, 200));
                }
                
                // Determine text color based on state
                Color textColor = Color.White;
                if (!meetsPrereqs)
                {
                    textColor = Color.DarkGray; // Gray out feats that don't meet prerequisites
                }
                else if (isTaken)
                {
                    textColor = new Color(144, 238, 144, 255); // Light green for selected feats
                }
                else if (isSelected)
                {
                    textColor = Color.Yellow; // Yellow for currently selected item
                }
                
                // Render feat name
                string featName = featData.Name ?? $"Feat {featId}";
                if (featName.Length > 40)
                {
                    featName = featName.Substring(0, 37) + "...";
                }
                
                // Render selection indicator
                string indicator = isTaken ? "[X]" : "[ ]";
                spriteBatch.DrawString(font, indicator, new Vector2(50, y), textColor);
                
                spriteBatch.DrawString(font, featName, new Vector2(90, y), textColor);
                
                // Render prerequisite warning if not met
                if (!meetsPrereqs && isSelected)
                {
                    spriteBatch.DrawString(font, " (Prerequisites not met)", new Vector2(90 + font.MeasureString(featName).X, y), Color.Red);
                }
            }
            
            // Render detailed description for selected feat
            if (_selectedFeatIndex >= 0 && _selectedFeatIndex < _availableFeatIds.Count)
            {
                int selectedFeatId = _availableFeatIds[_selectedFeatIndex];
                FeatData selectedFeatData = _gameDataManager.GetFeat(selectedFeatId);
                if (selectedFeatData != null)
                {
                    int descriptionX = 520;
                    int descriptionY = 135;
                    int descriptionWidth = 250;
                    
                    // Draw description panel background
                    DrawRectangle(spriteBatch, new Rectangle(descriptionX - 5, descriptionY - 5, descriptionWidth + 10, 250), new Color(30, 30, 30, 220));
                    DrawRectangleOutline(spriteBatch, new Rectangle(descriptionX - 5, descriptionY - 5, descriptionWidth + 10, 250), Color.White, 2);
                    
                    // Render feat name
                    string descFeatName = selectedFeatData.Name ?? $"Feat {selectedFeatId}";
                    spriteBatch.DrawString(font, descFeatName, new Vector2(descriptionX, descriptionY), Color.White);
                    descriptionY += 25;
                    
                    // Render description
                    string description = selectedFeatData.Description ?? "No description available.";
                    // Word wrap description (simple implementation)
                    string[] words = description.Split(' ');
                    string currentLine = string.Empty;
                    int lineHeight = 20;
                    int maxCharsPerLine = descriptionWidth / 8; // Rough estimate
                    
                    foreach (string word in words)
                    {
                        string testLine = currentLine + (currentLine.Length > 0 ? " " : "") + word;
                        if (testLine.Length > maxCharsPerLine && currentLine.Length > 0)
                        {
                            spriteBatch.DrawString(font, currentLine, new Vector2(descriptionX, descriptionY), new Color(211, 211, 211, 255)); // Light gray
                            descriptionY += lineHeight;
                            currentLine = word;
                        }
                        else
                        {
                            currentLine = testLine;
                        }
                        
                        if (descriptionY > 135 + 220) // Stop if we run out of space
                        {
                            break;
                        }
                    }
                    if (currentLine.Length > 0 && descriptionY <= 135 + 220)
                    {
                        spriteBatch.DrawString(font, currentLine, new Vector2(descriptionX, descriptionY), new Color(211, 211, 211, 255)); // Light gray
                    }
                    
                    // Render prerequisites
                    descriptionY += 30;
                    if (selectedFeatData.PrereqFeat1 >= 0)
                    {
                        FeatData prereqFeat = _gameDataManager.GetFeat(selectedFeatData.PrereqFeat1);
                        string prereqName = prereqFeat?.Name ?? $"Feat {selectedFeatData.PrereqFeat1}";
                        bool hasPrereq = _characterData.SelectedFeats.Contains(selectedFeatData.PrereqFeat1);
                        Color prereqColor = hasPrereq ? Color.Green : Color.Red;
                        spriteBatch.DrawString(font, $"Requires: {prereqName}", new Vector2(descriptionX, descriptionY), prereqColor);
                    }
                }
            }
        }
        
        /// <summary>
        /// Renders the portrait selection UI.
        /// Based on swkotor.exe and swkotor2.exe: Portrait selection displays available portraits
        /// - Portraits are loaded from game resources
        /// - Current portrait is highlighted
        /// </summary>
        private void RenderPortraitUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Select Portrait", new Vector2(50, 100), Color.White);
            
            // Render current portrait number
            string portraitText = $"Portrait: {_characterData.Portrait}";
            spriteBatch.DrawString(font, portraitText, new Vector2(50, 150), Color.White);
            
            // Render navigation hints
            spriteBatch.DrawString(font, "Use Left/Right arrows to change portrait", new Vector2(50, 200), Color.Gray);
            
            // TODO: PLACEHOLDER - Implement portrait thumbnail rendering
            // This requires portrait texture loading from game resources
        }
        
        /// <summary>
        /// Renders the name entry UI.
        /// Based on swkotor.exe and swkotor2.exe: Name entry displays text input field
        /// - Current name is displayed and can be edited
        /// - Text input is handled in Update method
        /// </summary>
        private void RenderNameUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Enter Character Name", new Vector2(50, 100), Color.White);
            
            // Render name input field background
            int inputX = 50;
            int inputY = 150;
            int inputWidth = 400;
            int inputHeight = 40;
            DrawRectangle(spriteBatch, new Rectangle(inputX, inputY, inputWidth, inputHeight), new Color(50, 50, 50, 200));
            DrawRectangleOutline(spriteBatch, new Rectangle(inputX, inputY, inputWidth, inputHeight), Color.White, 2);
            
            // Render current name
            string displayName = string.IsNullOrEmpty(_characterData.Name) ? "Enter name..." : _characterData.Name;
            Color nameColor = string.IsNullOrEmpty(_characterData.Name) ? Color.Gray : Color.White;
            spriteBatch.DrawString(font, displayName, new Vector2(inputX + 10, inputY + 10), nameColor);
            
            // Render cursor (blinking)
            float time = (float)(DateTime.Now.Millisecond % 1000) / 1000.0f;
            if (time < 0.5f)
            {
                int cursorX = inputX + 10 + (int)font.MeasureString(displayName).X;
                spriteBatch.DrawString(font, "|", new Vector2(cursorX, inputY + 10), Color.White);
            }
        }
        
        /// <summary>
        /// Renders the summary UI.
        /// Based on swkotor.exe and swkotor2.exe: Summary displays all character creation choices
        /// - Shows class, attributes, skills, feats, portrait, and name
        /// - Allows final review before completing character creation
        /// </summary>
        private void RenderSummaryUI(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            // Render step title
            spriteBatch.DrawString(font, "Character Summary", new Vector2(50, 50), Color.White);
            
            int y = 100;
            int lineHeight = 25;
            
            // Render character information
            spriteBatch.DrawString(font, $"Name: {_characterData.Name}", new Vector2(50, y), Color.White);
            y += lineHeight;
            
            spriteBatch.DrawString(font, $"Class: {GetClassName(_characterData.Class)}", new Vector2(50, y), Color.White);
            y += lineHeight;
            
            spriteBatch.DrawString(font, $"Gender: {_characterData.Gender}", new Vector2(50, y), Color.White);
            y += lineHeight;
            
            spriteBatch.DrawString(font, $"Portrait: {_characterData.Portrait}", new Vector2(50, y), Color.White);
            y += lineHeight * 2;
            
            // Render attributes
            spriteBatch.DrawString(font, "Attributes:", new Vector2(50, y), Color.Cyan);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  STR: {_characterData.Strength}", new Vector2(70, y), Color.White);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  DEX: {_characterData.Dexterity}", new Vector2(70, y), Color.White);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  CON: {_characterData.Constitution}", new Vector2(70, y), Color.White);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  INT: {_characterData.Intelligence}", new Vector2(70, y), Color.White);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  WIS: {_characterData.Wisdom}", new Vector2(70, y), Color.White);
            y += lineHeight;
            spriteBatch.DrawString(font, $"  CHA: {_characterData.Charisma}", new Vector2(70, y), Color.White);
            y += lineHeight * 2;
            
            // Render completion hint
            spriteBatch.DrawString(font, "Press Enter to finish character creation", new Vector2(50, y), Color.Yellow);
        }
        
        /// <summary>
        /// Renders navigation buttons (Next, Back, Cancel, Finish).
        /// Based on swkotor.exe and swkotor2.exe: Navigation buttons are always visible at the bottom of the screen
        /// - Next: Advances to next step (or finishes on summary step)
        /// - Back: Returns to previous step
        /// - Cancel: Cancels character creation and returns to main menu
        /// - Finish: Completes character creation (only on summary step)
        /// </summary>
        private void RenderNavigationButtons(ISpriteBatch spriteBatch, IFont font)
        {
            if (font == null)
            {
                return;
            }
            
            int buttonY = _graphicsDevice.Viewport.Height - 60;
            int buttonWidth = 100;
            int buttonHeight = 40;
            int buttonSpacing = 20;
            
            // Render Back button (if not on first step)
            if (_currentStep != CreationStep.ClassSelection)
            {
                int backX = 50;
                DrawRectangle(spriteBatch, new Rectangle(backX, buttonY, buttonWidth, buttonHeight), new Color(80, 80, 80, 200));
                DrawRectangleOutline(spriteBatch, new Rectangle(backX, buttonY, buttonWidth, buttonHeight), Color.White, 2);
                spriteBatch.DrawString(font, "Back", new Vector2(backX + 25, buttonY + 10), Color.White);
            }
            
            // Render Cancel button
            int cancelX = _currentStep != CreationStep.ClassSelection ? 170 : 50;
            DrawRectangle(spriteBatch, new Rectangle(cancelX, buttonY, buttonWidth, buttonHeight), new Color(120, 40, 40, 200));
            DrawRectangleOutline(spriteBatch, new Rectangle(cancelX, buttonY, buttonWidth, buttonHeight), Color.White, 2);
            spriteBatch.DrawString(font, "Cancel", new Vector2(cancelX + 15, buttonY + 10), Color.White);
            
            // Render Next/Finish button
            string nextButtonText = (_currentStep == CreationStep.Summary) ? "Finish" : "Next";
            int nextX = _graphicsDevice.Viewport.Width - buttonWidth - 50;
            Color nextButtonColor = (_currentStep == CreationStep.Summary) ? new Color(40, 120, 40, 200) : new Color(80, 80, 80, 200);
            DrawRectangle(spriteBatch, new Rectangle(nextX, buttonY, buttonWidth, buttonHeight), nextButtonColor);
            DrawRectangleOutline(spriteBatch, new Rectangle(nextX, buttonY, buttonWidth, buttonHeight), Color.White, 2);
            spriteBatch.DrawString(font, nextButtonText, new Vector2(nextX + 20, buttonY + 10), Color.White);
        }
        
        /// <summary>
        /// Draws a filled rectangle using a 1x1 pixel texture.
        /// </summary>
        private void DrawRectangle(ISpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            if (_pixelTexture == null)
            {
                return;
            }
            spriteBatch.Draw(_pixelTexture, rect, color);
        }
        
        /// <summary>
        /// Draws a rectangle outline using a 1x1 pixel texture.
        /// </summary>
        private void DrawRectangleOutline(ISpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            if (_pixelTexture == null)
            {
                return;
            }
            // Draw top edge
            spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // Draw bottom edge
            spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            // Draw left edge
            spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // Draw right edge
            spriteBatch.Draw(_pixelTexture, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
        
        /// <summary>
        /// Converts CharacterClass enum to class ID used in classes.2da.
        /// Based on nwscript.nss constants: CLASS_TYPE_SOLDIER=0, CLASS_TYPE_SCOUT=1, etc.
        /// </summary>
        private int GetClassId(CharacterClass characterClass)
        {
            switch (characterClass)
            {
                case CharacterClass.Soldier:
                    return 0;
                case CharacterClass.Scout:
                    return 1;
                case CharacterClass.Scoundrel:
                    return 2;
                case CharacterClass.JediGuardian:
                    return 3;
                case CharacterClass.JediConsular:
                    return 4;
                case CharacterClass.JediSentinel:
                    return 5;
                default:
                    return 0;
            }
        }
        
        /// <summary>
        /// Updates the available feats list based on current class.
        /// Based on swkotor.exe and swkotor2.exe: FUN_0060d1d0 (LoadFeatGain) loads starting feats from featgain.2da
        /// - Original implementation: Gets starting feats from featgain.2da _REG and _BON columns
        /// - Feats are filtered to only include selectable feats that meet prerequisites
        /// </summary>
        private void UpdateAvailableFeats()
        {
            _availableFeatIds.Clear();
            
            // Get starting feats for the current class
            int classId = GetClassId(_characterData.Class);
            List<int> startingFeats = _gameDataManager.GetStartingFeats(classId);
            
            // Add all starting feats that are selectable
            foreach (int featId in startingFeats)
            {
                FeatData featData = _gameDataManager.GetFeat(featId);
                if (featData != null && featData.Selectable)
                {
                    _availableFeatIds.Add(featId);
                }
            }
            
            // Sort by feat ID for consistent display
            _availableFeatIds.Sort();
            
            // Reset selection state
            _selectedFeatIndex = 0;
            _featScrollOffset = 0;
        }
        
        /// <summary>
        /// Checks if a feat's prerequisites are met.
        /// Based on swkotor.exe and swkotor2.exe: Feat prerequisite checking in character creation
        /// - Original implementation: Checks if prerequisite feats are in SelectedFeats list
        /// - Also checks attribute requirements (minlevel, minstr, mindex, etc.) based on current character attributes
        /// </summary>
        private bool MeetsFeatPrerequisites(FeatData featData)
        {
            if (featData == null)
            {
                return false;
            }
            
            // Check prerequisite feats
            if (featData.PrereqFeat1 >= 0 && !_characterData.SelectedFeats.Contains(featData.PrereqFeat1))
            {
                return false;
            }
            
            if (featData.PrereqFeat2 >= 0 && !_characterData.SelectedFeats.Contains(featData.PrereqFeat2))
            {
                return false;
            }
            
            // Check minimum level (character is level 1 during creation, so minlevel should be <= 1)
            if (featData.MinLevel > 1)
            {
                return false;
            }
            
            // TODO: Check attribute requirements (minstr, mindex, minint, minwis, mincon, mincha)
            // This would require reading these fields from feat.2da which may not be in FeatData structure yet
            // For now, we skip attribute requirement checking
            
            return true;
        }
        
        /// <summary>
        /// Gets the display name for a character class.
        /// </summary>
        private string GetClassName(CharacterClass characterClass)
        {
            switch (characterClass)
            {
                case CharacterClass.Scout:
                    return "Scout";
                case CharacterClass.Soldier:
                    return "Soldier";
                case CharacterClass.Scoundrel:
                    return "Scoundrel";
                case CharacterClass.JediGuardian:
                    return "Jedi Guardian";
                case CharacterClass.JediSentinel:
                    return "Jedi Sentinel";
                case CharacterClass.JediConsular:
                    return "Jedi Consular";
                default:
                    return "Unknown";
            }
        }
        
        /// <summary>
        /// Completes character creation and calls the completion callback.
        /// </summary>
        private void Finish()
        {
            // Validate character data
            if (string.IsNullOrWhiteSpace(_characterData.Name))
            {
                _characterData.Name = "Player"; // Default name
            }
            
            // Call completion callback
            _onComplete(_characterData);
        }
        
        /// <summary>
        /// Cancels character creation and returns to main menu.
        /// </summary>
        private void Cancel()
        {
            _onCancel();
        }
    }
    
    /// <summary>
    /// Data structure for character creation.
    /// </summary>
    public class CharacterCreationData
    {
        public KotorGame Game { get; set; }
        public CharacterClass Class { get; set; }
        public Gender Gender { get; set; }
        public int Appearance { get; set; }
        public int Portrait { get; set; }
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        /// <summary>
        /// List of selected feat IDs during character creation.
        /// Based on swkotor.exe and swkotor2.exe: Character creation stores selected feats in character data
        /// - Original implementation: Selected feats are stored as list of feat IDs from feat.2da
        /// - Feats are added to creature's FeatList when character is created
        /// </summary>
        public List<int> SelectedFeats { get; set; }
    }
    
    /// <summary>
    /// Character class enumeration.
    /// </summary>
    public enum CharacterClass
    {
        Scout,          // K1 only
        Soldier,        // K1 only
        Scoundrel,      // K1 only
        JediGuardian,   // K2 only
        JediSentinel,   // K2 only
        JediConsular    // K2 only
    }
    
    /// <summary>
    /// Gender enumeration.
    /// </summary>
    public enum Gender
    {
        Male,
        Female
    }
}

