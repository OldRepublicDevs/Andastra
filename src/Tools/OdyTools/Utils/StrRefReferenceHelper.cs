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
    /// Holocron-style StrRef reference search for TLK and related editors.
    /// </summary>
    public static class StrRefReferenceHelper
    {
        public static void FindAndShowStrRefReferences(
            Window parent,
            int strref,
            OdyInstallation installation)
        {
            if (installation?.Installation == null || strref < 0)
            {
                return;
            }

            try
            {
                List<StrRefSearchResult> strrefResults = ReferenceCacheHelpers.FindStrRefReferences(
                    installation.Installation,
                    strref,
                    null,
                    null);

                List<ReferenceSearchResult> results = ReferenceCacheHelpers.ConvertToReferenceSearchResults(
                    strrefResults,
                    strref);

                if (results.Count == 0)
                {
                    _ = DialogHelper.ShowAsync(
                        "No references found",
                        "No references found for StrRef " + strref + ".",
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
                    "Find StrRef References Failed",
                    ex.Message,
                    ButtonEnum.Ok,
                    IconType.Error);
            }
        }
    }
}
