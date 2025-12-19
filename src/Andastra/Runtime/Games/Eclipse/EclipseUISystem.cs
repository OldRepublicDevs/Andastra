using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;

namespace Andastra.Runtime.Games.Eclipse
{
    /// <summary>
    /// UI system implementation for Eclipse engine (Dragon Age, Mass Effect).
    /// </summary>
    /// <remarks>
    /// UI System Implementation:
    /// - Eclipse-specific UI system implementation inheriting from BaseUISystem
    /// - Based on daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe UI systems
    /// - Eclipse engine uses advanced UI system with crafting, inventory, and character progression screens
    /// - Enhanced screen management with transitions and cinematic overlays
    ///
    /// Based on reverse engineering:
    /// - daorigins.exe: Advanced UI system with crafting screens and inventory management
    /// - DragonAge2.exe: Enhanced UI system with character progression and ability screens
    /// - MassEffect.exe: Modern UI system with cinematic overlays and dialogue system
    /// - MassEffect2.exe: Advanced UI system with inventory, character, and mission screens
    ///
    /// Note: Eclipse engine games may have crafting or modification screens but not upgrade screens
    /// in the same sense as Odyssey. This implementation provides a placeholder that throws
    /// NotImplementedException for upgrade screen functionality.
    /// </remarks>
    public class EclipseUISystem : BaseUISystem
    {
        /// <summary>
        /// Initializes a new instance of the UI system.
        /// </summary>
        /// <param name="world">World context for entity access.</param>
        public EclipseUISystem(IWorld world)
            : base(world)
        {
        }

        /// <summary>
        /// Eclipse-specific implementation of upgrade screen display.
        /// </summary>
        /// <param name="item">Item to upgrade (validated by base class).</param>
        /// <param name="character">Character whose skills will be used (validated by base class).</param>
        /// <param name="disableItemCreation">If true, disable item creation screen.</param>
        /// <param name="disableUpgrade">If true, force straight to item creation and disable upgrading.</param>
        /// <param name="override2DA">Override 2DA file name (empty string for default).</param>
        /// <remarks>
        /// Eclipse engine games may have crafting or modification screens but not upgrade screens
        /// in the same sense as Odyssey. This method throws NotImplementedException.
        /// If upgrade-like functionality is needed, it should be implemented via crafting screens or similar.
        /// </remarks>
        protected override void ShowUpgradeScreenImpl(uint item, uint character, bool disableItemCreation, bool disableUpgrade, string override2DA)
        {
            // TODO: STUB - Eclipse engine games do not have upgrade screens in the same sense as Odyssey
            // If upgrade-like functionality is needed, implement via crafting screens or item modification UI
            throw new NotImplementedException("Eclipse engine games do not support upgrade screens in the same sense as Odyssey. Use crafting screens or item modification UI instead.");
        }

        /// <summary>
        /// Gets whether the upgrade screen is currently visible.
        /// </summary>
        /// <remarks>
        /// Eclipse engine games do not have upgrade screens, so this always returns false.
        /// </remarks>
        public override bool IsUpgradeScreenVisible
        {
            get { return false; }
        }

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        /// <remarks>
        /// Eclipse engine games do not have upgrade screens, so this is a no-op.
        /// </remarks>
        public override void HideUpgradeScreen()
        {
            // No-op: Eclipse engine games do not have upgrade screens
        }
    }
}

