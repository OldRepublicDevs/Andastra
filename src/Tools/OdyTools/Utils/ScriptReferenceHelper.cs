using System;
using System.Collections.Generic;
using Avalonia.Controls;
using BioWare.Tools;
using MsBox.Avalonia.Enums;
using OdyTools.Data;
using OdyTools.Dialogs;
using IconType = MsBox.Avalonia.Enums.Icon;

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
            if (comboBox == null || installation?.Installation == null)
            {
                return;
            }

            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName))
            {
                return;
            }

            try
            {
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = true,
                    SearchModules = true,
                    SearchOverride = true
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindScriptReferences(
                    installation.Installation,
                    scriptName,
                    options);

                FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(parent, results, installation);
                dialog.Show();
            }
            catch (Exception ex)
            {
                _ = DialogHelper.ShowAsync(
                    "Find References Failed",
                    ex.Message,
                    ButtonEnum.Ok,
                    IconType.Error);
            }
        }
    }
}
