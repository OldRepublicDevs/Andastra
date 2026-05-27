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
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            if (installation?.Installation == null || strref < 0)
            {
                return;
            }

            try
            {
                ReferenceSearchOptions options = new ReferenceSearchOptions
                {
                    SearchChitin = true,
                    SearchModules = true,
                    SearchOverride = true
                };

                if (showOptionsDialog)
                {
                    ReferenceSearchOptions chosen = ReferenceSearchHelper.PromptSearchOptions(parent, options);
                    if (chosen == null)
                    {
                        return;
                    }

                    options = chosen;
                }

                List<ReferenceSearchResult> results = CollectStrRefReferences(
                    strref,
                    installation,
                    options);

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

        public static List<ReferenceSearchResult> CollectStrRefReferences(
            int strref,
            OdyInstallation installation,
            ReferenceSearchOptions options = null)
        {
            if (installation?.Installation == null || strref < 0)
            {
                return new List<ReferenceSearchResult>();
            }

            options = options ?? new ReferenceSearchOptions
            {
                SearchChitin = true,
                SearchModules = true,
                SearchOverride = true
            };

            List<StrRefSearchResult> strrefResults = ReferenceCacheHelpers.FindStrRefReferences(
                installation.Installation,
                strref,
                null,
                null,
                options);

            return ReferenceCacheHelpers.ConvertToReferenceSearchResults(strrefResults, strref);
        }
    }
}
