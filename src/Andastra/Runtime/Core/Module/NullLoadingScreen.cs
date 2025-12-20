using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Core.Module
{
    /// <summary>
    /// Null implementation of ILoadingScreen that does nothing.
    /// Used as a fallback when no loading screen implementation is available.
    /// </summary>
    /// <remarks>
    /// Null Loading Screen:
    /// - Provides a no-op implementation of ILoadingScreen
    /// - Used when loading screen functionality is not available or not needed
    /// - All methods are no-ops that do nothing
    /// - IsVisible always returns false
    /// </remarks>
    public class NullLoadingScreen : ILoadingScreen
    {
        /// <summary>
        /// Gets whether the loading screen is currently visible.
        /// Always returns false for null implementation.
        /// </summary>
        public bool IsVisible
        {
            get { return false; }
        }

        /// <summary>
        /// Shows the loading screen with the specified image.
        /// No-op for null implementation.
        /// </summary>
        /// <param name="imageResRef">Resource reference for the loading screen image (ignored).</param>
        public void Show(string imageResRef)
        {
            // No-op: Null implementation does nothing
        }

        /// <summary>
        /// Hides the loading screen.
        /// No-op for null implementation.
        /// </summary>
        public void Hide()
        {
            // No-op: Null implementation does nothing
        }

        /// <summary>
        /// Updates the loading screen progress bar.
        /// No-op for null implementation.
        /// </summary>
        /// <param name="progress">Progress value (ignored).</param>
        public void SetProgress(int progress)
        {
            // No-op: Null implementation does nothing
        }
    }
}

