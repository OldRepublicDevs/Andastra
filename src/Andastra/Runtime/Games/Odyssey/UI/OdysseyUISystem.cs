using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Parsing;
using Andastra.Parsing.Installation;

namespace Andastra.Runtime.Engines.Odyssey.UI
{
    /// <summary>
    /// UI system implementation for Odyssey engine (KOTOR/TSL).
    /// </summary>
    /// <remarks>
    /// UI System Implementation:
    /// - Based on swkotor2.exe UI system
    /// - Located via string references: GUI panels, UI screens, upgrade screens
    /// - Original implementation: Manages UI screen state, screen transitions, modal dialogs
    /// - UI screens: Upgrade screen, inventory screen, character screen, dialogue screen, etc.
    /// - Screen management: Push/pop screen stack, modal overlays, screen transitions
    /// - Based on swkotor2.exe: ShowUpgradeScreen @ 0x00680cb0 creates upgrade selection screen ("upgradesel_p") and upgrade items screen ("upgradeitems_p")
    /// - Original creates two GUI panels: upgrade selection screen for item type filtering, upgrade items screen for item modification
    /// </remarks>
    public class OdysseyUISystem : IUISystem
    {
        private readonly IUpgradeScreen _upgradeScreen;
        private readonly IWorld _world;

        /// <summary>
        /// Initializes a new instance of the UI system.
        /// </summary>
        /// <param name="installation">Game installation for accessing game data.</param>
        /// <param name="world">World context for entity access.</param>
        public OdysseyUISystem(Installation installation, IWorld world)
        {
            if (installation == null)
            {
                throw new ArgumentNullException("installation");
            }
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            _world = world;
            
            // Create appropriate upgrade screen based on game type
            // K1 uses K1UpgradeScreen (swkotor.exe), K2 uses K2UpgradeScreen (swkotor2.exe)
            if (installation.Game == Parsing.Game.K1)
            {
                _upgradeScreen = new K1UpgradeScreen(installation, world);
            }
            else if (installation.Game == Parsing.Game.TSL)
            {
                _upgradeScreen = new K2UpgradeScreen(installation, world);
            }
            else
            {
                // Fallback to K2 for unknown game types
                _upgradeScreen = new K2UpgradeScreen(installation, world);
            }
        }

        /// <summary>
        /// Shows the upgrade screen for item modification.
        /// </summary>
        /// <param name="item">Item to upgrade (OBJECT_INVALID for all items).</param>
        /// <param name="character">Character whose skills will be used (OBJECT_INVALID for player).</param>
        /// <param name="disableItemCreation">If true, disable item creation screen.</param>
        /// <param name="disableUpgrade">If true, force straight to item creation and disable upgrading.</param>
        /// <param name="override2DA">Override 2DA file name (empty string for default).</param>
        /// <remarks>
        /// Based on swkotor2.exe: ShowUpgradeScreen @ 0x00680cb0
        /// Original implementation:
        /// - Validates item exists if item != OBJECT_INVALID (0x7f000000)
        /// - Creates upgrade selection screen GUI ("upgradesel_p") with item type filters
        /// - Creates upgrade items screen GUI ("upgradeitems_p") with item list and upgrade buttons
        /// - Sets flags in GUI object: item ID (offset 0x629), character ID (offset 0x18a8), disableItemCreation (offset 0x18c8), disableUpgrade (offset 0x18cc)
        /// - Shows screen via GUI manager
        /// </remarks>
        public void ShowUpgradeScreen(uint item, uint character, bool disableItemCreation, bool disableUpgrade, string override2DA)
        {
            // OBJECT_INVALID = 0x7FFFFFFF (uint.MaxValue)
            const uint ObjectInvalid = 0x7FFFFFFF;

            // Resolve item entity (original validates via FUN_004dc020 if item != OBJECT_INVALID)
            IEntity itemEntity = null;
            if (item != 0 && item != ObjectInvalid)
            {
                itemEntity = _world.GetEntity(item);
                // Original returns early if item validation fails
                if (itemEntity == null)
                {
                    return;
                }
            }

            // Resolve character entity
            IEntity characterEntity = null;
            if (character != 0 && character != ObjectInvalid)
            {
                characterEntity = _world.GetEntity(character);
            }

            // Configure upgrade screen
            // Original creates two GUI panels: upgrade selection screen and upgrade items screen
            _upgradeScreen.TargetItem = itemEntity;
            _upgradeScreen.Character = characterEntity;
            _upgradeScreen.DisableItemCreation = disableItemCreation;
            _upgradeScreen.DisableUpgrade = disableUpgrade;
            _upgradeScreen.Override2DA = override2DA ?? string.Empty;

            // Show upgrade screen
            // Original shows screen via GUI manager (FUN_0040bf90 adds to GUI manager, FUN_00638bb0 sets screen mode)
            _upgradeScreen.Show();
        }

        /// <summary>
        /// Gets whether the upgrade screen is currently visible.
        /// </summary>
        public bool IsUpgradeScreenVisible
        {
            get { return _upgradeScreen.IsVisible; }
        }

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        public void HideUpgradeScreen()
        {
            _upgradeScreen.Hide();
        }
    }
}

