using Avalonia.Controls;

namespace HolocronToolset.Tests.TestHelpers
{
    // Extension methods to match PyKotor test patterns
    public static class ComboBoxExtensions
    {
        // Matching PyKotor: editor.ui.appearanceSelect.count() -> ItemCount
        // Provides ItemCount property for ComboBox to match Python test patterns
        public static int ItemCount(this ComboBox comboBox)
        {
            if (comboBox?.Items == null)
            {
                return 0;
            }

            // Count items in the Items collection
            int count = 0;
            foreach (var item in comboBox.Items)
            {
                count++;
            }

            return count;
        }
    }
}

