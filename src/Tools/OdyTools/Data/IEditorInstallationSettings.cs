namespace OdyTools.Data
{
    /// <summary>
    /// Interface for editor settings that provide installation selection (UseInstallation + SelectedInstallationName).
    /// Used by base Editor.ApplyInstallationFromSettings to resolve _installation from GlobalSettings.
    /// </summary>
    public interface IEditorInstallationSettings
    {
        /// <summary>True = use game installation when available; False = use manual paths / no installation.</summary>
        bool UseInstallation(bool defaultValue = true);

        /// <summary>When UseInstallation is true, which installation name to use (from GlobalSettings.Installations keys).</summary>
        string SelectedInstallationName(string defaultValue = "");
    }
}
