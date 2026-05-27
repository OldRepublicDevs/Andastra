using System;
using Avalonia.Controls;
using OdyTools.Data;

namespace OdyTools.Utils
{
    /// <summary>
    /// Shared script reference search wiring for editor script combo context menus.
    /// </summary>
    public static class ScriptReferenceHelper
    {
        public static void FindAndShowScriptReferences(
            Window parent,
            ComboBox comboBox,
            OdyInstallation installation)
        {
            if (comboBox == null)
            {
                return;
            }

            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName))
            {
                scriptName = comboBox.SelectedItem?.ToString()?.Trim();
            }

            ReferenceSearchHelper.FindAndShowScriptReferences(
                parent,
                scriptName,
                installation,
                showOptionsDialog: true);
        }
    }
}
