using System;

namespace Andastra.Runtime.Core.Interfaces
{
    /// <summary>
    /// Interface for UI system that manages game UI screens and overlays.
    /// </summary>
    /// <remarks>
    /// UI System Interface:
    /// - Common interface for UI system functionality across all BioWare engines
    /// - Manages UI screen state, screen transitions, modal dialogs
    /// - UI screens: Upgrade screen, inventory screen, character screen, dialogue screen, etc.
    /// - Screen management: Push/pop screen stack, modal overlays, screen transitions
    /// - Engine-specific implementations provide concrete functionality for their respective engines
    ///
    /// Based on verified components of UI systems across engines:
    /// - Odyssey (swkotor.exe, swkotor2.exe): GUI panel-based UI system with upgrade screens
    /// - Aurora (nwmain.exe): Scene-based GUI system with multiple panel types
    /// - Eclipse (daorigins.exe, DragonAge2.exe): Advanced UI system with crafting and inventory screens
    /// - Infinity (, ): Modern UI system with cinematic overlays
    /// </remarks>
    public interface IUISystem
    {
        /// <summary>
        /// Shows the upgrade screen for item modification.
        /// </summary>
        /// <param name="item">Item to upgrade (OBJECT_INVALID for all items).</param>
        /// <param name="character">Character whose skills will be used (OBJECT_INVALID for player).</param>
        /// <param name="disableItemCreation">If true, disable item creation screen.</param>
        /// <param name="disableUpgrade">If true, force straight to item creation and disable upgrading.</param>
        /// <param name="override2DA">Override 2DA file name (empty string for default).</param>
        void ShowUpgradeScreen(uint item, uint character, bool disableItemCreation, bool disableUpgrade, string override2DA);

        /// <summary>
        /// Gets whether the upgrade screen is currently visible.
        /// </summary>
        bool IsUpgradeScreenVisible { get; }

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        void HideUpgradeScreen();
    }
}

