using System;
using Avalonia.Controls;

namespace HolocronToolset.Editors
{
    // Helper methods for editors
    public static class EditorHelpers
    {
        // Safe FindControl that handles missing name scope
        public static T FindControlSafe<T>(Control control, string name) where T : Control
        {
            try
            {
                return control.FindControl<T>(name);
            }
            catch
            {
                return null;
            }
        }
    }
}
