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
    /// Holocron-style 2DA memory (GFF row index) reference search.
    /// </summary>
    public static class TwoDAMemoryReferenceHelper
    {
        public static void FindAndShowTwoDAMemoryReferences(
            Window parent,
            string twodaFilename,
            int rowIndex,
            OdyInstallation installation)
        {
            if (installation?.Installation == null || rowIndex < 0 || string.IsNullOrWhiteSpace(twodaFilename))
            {
                return;
            }

            try
            {
                List<ReferenceSearchResult> results = ReferenceCacheHelpers.Find2DAMemoryReferences(
                    installation.Installation,
                    twodaFilename,
                    rowIndex,
                    null,
                    null);

                if (results.Count == 0)
                {
                    string normalized = ReferenceCacheHelpers.NormalizeTwoDAFilename(twodaFilename);
                    _ = DialogHelper.ShowAsync(
                        "No references found",
                        "No references found for " + normalized + " row " + rowIndex + ".",
                        ButtonEnum.Ok,
                        IconType.Info);
                    return;
                }

                FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(parent, results, installation);
                dialog.Show();
            }
            catch (System.Exception ex)
            {
                _ = DialogHelper.ShowAsync(
                    "Find 2DA References Failed",
                    ex.Message,
                    ButtonEnum.Ok,
                    IconType.Error);
            }
        }
    }
}
