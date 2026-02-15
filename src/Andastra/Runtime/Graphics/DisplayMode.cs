namespace Andastra.Runtime.Graphics
{
    public enum DisplayModePreference
    {
        BorderlessFullscreen,
        Windowed,
        ExclusiveFullscreen
    }

    public static class DisplayModeContext
    {
        public static DisplayModePreference CurrentMode { get; set; } = DisplayModePreference.BorderlessFullscreen;
    }
}
