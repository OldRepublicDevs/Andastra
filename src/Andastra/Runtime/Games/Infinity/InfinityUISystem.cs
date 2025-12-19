using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;
using Andastra.Parsing;
using Andastra.Parsing.Installation;

namespace Andastra.Runtime.Games.Infinity
{
    /// <summary>
    /// UI system implementation for Infinity engine (Baldur's Gate, Icewind Dale, Planescape: Torment).
    /// </summary>
    /// <remarks>
    /// UI System Implementation:
    /// - Infinity-specific UI system implementation inheriting from BaseUISystem
    /// - Based on Infinity engine UI systems (Baldur's Gate, Icewind Dale, Planescape: Torment)
    /// - Infinity engine uses traditional 2D UI system with character sheets, inventory, and dialogue
    /// - Screen management via UI panels and modal dialogs
    ///
    /// Based on reverse engineering:
    /// - Infinity engine: Traditional 2D UI system with character sheets, inventory, spellbooks
    /// - UI panels: Character sheet, inventory, spellbook, journal, etc.
    /// - Screen management via UI panel stack and modal dialogs
    /// - nwmain.exe (Aurora engine, similar to Infinity): No upgrade screen functions found
    ///   - Aurora engine uses enchant/identify systems instead of upgrade screens
    ///   - Infinity engine follows similar pattern - no upgrade screen system
    ///
    /// Note: Infinity engine does not have upgrade screens like Odyssey or Eclipse (Dragon Age).
    /// This implementation uses InfinityUpgradeScreen which provides graceful no-op behavior
    /// for upgrade screen operations, maintaining API consistency with other engines.
    /// </remarks>
    public class InfinityUISystem : BaseUISystem
    {
        private readonly IUpgradeScreen _upgradeScreen;

        /// <summary>
        /// Initializes a new instance of the UI system.
        /// </summary>
        /// <param name="installation">Game installation for accessing game data.</param>
        /// <param name="world">World context for entity access.</param>
        public InfinityUISystem(Installation installation, IWorld world)
            : base(world)
        {
            if (installation == null)
            {
                throw new ArgumentNullException("installation");
            }

            // Create Infinity upgrade screen
            // Infinity engine does not have upgrade screens, but we use InfinityUpgradeScreen
            // to maintain API consistency and provide graceful no-op behavior
            _upgradeScreen = new InfinityUpgradeScreen(installation, world);
        }

        /// <summary>
        /// Infinity-specific implementation of upgrade screen display.
        /// </summary>
        /// <param name="item">Item to upgrade (validated by base class).</param>
        /// <param name="character">Character whose skills will be used (validated by base class).</param>
        /// <param name="disableItemCreation">If true, disable item creation screen.</param>
        /// <param name="disableUpgrade">If true, force straight to item creation and disable upgrading.</param>
        /// <param name="override2DA">Override 2DA file name (empty string for default).</param>
        /// <remarks>
        /// Based on reverse engineering:
        /// - nwmain.exe (Aurora engine, similar architecture): No upgrade screen functions found
        /// - Infinity engine (Baldur's Gate, Icewind Dale): No upgrade screen system
        /// - Infinity engine uses character sheet and inventory UI for item management
        /// - This method delegates to InfinityUpgradeScreen which provides graceful no-op behavior
        /// </remarks>
        protected override void ShowUpgradeScreenImpl(uint item, uint character, bool disableItemCreation, bool disableUpgrade, string override2DA)
        {
            // OBJECT_INVALID = 0x7FFFFFFF (uint.MaxValue)
            const uint ObjectInvalid = 0x7FFFFFFF;

            // Resolve item entity (base class already validated, but we need the entity for the upgrade screen)
            IEntity itemEntity = null;
            if (item != 0 && item != ObjectInvalid)
            {
                itemEntity = _world.GetEntity(item);
            }

            // Resolve character entity
            IEntity characterEntity = null;
            if (character != 0 && character != ObjectInvalid)
            {
                characterEntity = _world.GetEntity(character);
            }

            // Configure upgrade screen
            // Infinity engine does not have upgrade screens, but we maintain API consistency
            // by delegating to InfinityUpgradeScreen which provides graceful no-op behavior
            _upgradeScreen.TargetItem = itemEntity;
            _upgradeScreen.Character = characterEntity;
            _upgradeScreen.DisableItemCreation = disableItemCreation;
            _upgradeScreen.DisableUpgrade = disableUpgrade;
            _upgradeScreen.Override2DA = override2DA;

            // Show upgrade screen (no-op for Infinity engine)
            // InfinityUpgradeScreen.Show() sets _isVisible = true but does not display any UI
            // This maintains API consistency while correctly reflecting that Infinity has no upgrade screens
            _upgradeScreen.Show();
        }

        /// <summary>
        /// Gets whether the upgrade screen is currently visible.
        /// </summary>
        /// <remarks>
        /// Infinity engine does not have upgrade screens, so this returns the visibility state
        /// from InfinityUpgradeScreen (which will always be false after Hide() is called).
        /// </remarks>
        public override bool IsUpgradeScreenVisible
        {
            get { return _upgradeScreen.IsVisible; }
        }

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        /// <remarks>
        /// Infinity engine does not have upgrade screens, but we delegate to InfinityUpgradeScreen
        /// to maintain API consistency. This is effectively a no-op but maintains proper state.
        /// </remarks>
        public override void HideUpgradeScreen()
        {
            _upgradeScreen.Hide();
        }
    }
}

