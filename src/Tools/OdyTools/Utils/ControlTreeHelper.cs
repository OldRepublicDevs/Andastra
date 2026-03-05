using Avalonia.Controls;

namespace OdyTools.Utils
{
    /// <summary>
    /// Utility for traversing visual control trees in Avalonia applications.
    /// Eliminates duplicated parent-window discovery logic across multiple widgets.
    /// </summary>
    public static class ControlTreeHelper
    {
        /// <summary>
        /// Find the parent Window for a given Control by traversing up the visual tree.
        /// Uses traversal pattern: Control.Parent → cast to Control → iterate until Window found.
        /// </summary>
        /// <remarks>
        /// This method is used to center dialogs over their owning window, pass to dialog constructors,
        /// and determine the root window context for relative positioning of child controls.
        /// Common in settings widgets, editors, and dynamic UI creation scenarios.
        /// </remarks>
        /// <param name="control">The control whose parent window is to be found</param>
        /// <returns>The parent Window if found; null if control has no Window parent or is null</returns>
        public static Window GetParentWindow(Control control)
        {
            Control current = control;
            while (current != null)
            {
                if (current is Window window)
                {
                    return window;
                }

                current = current.Parent as Control;
            }

            return null;
        }
    }
}
