using System.Collections.Generic;
using Avalonia.Controls;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Tools;
using MsBox.Avalonia.Enums;
using OdyTools.Data;
using OdyTools.Dialogs;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Utils
{
    /// <summary>
    /// Holocron-style 2DA row reference search (memory, label field values, row StrRefs).
    /// </summary>
    public static class TwoDAMemoryReferenceHelper
    {
        public static void FindAndShowTwoDAMemoryReferences(
            Window parent,
            string twodaFilename,
            int rowIndex,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            FindAndShowTwoDARowReferences(parent, twodaFilename, rowIndex, null, installation, showOptionsDialog);
        }

        public static void FindAndShowTwoDARowReferences(
            Window parent,
            string twodaFilename,
            int rowIndex,
            TwoDA twoDA,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            if (installation?.Installation == null || rowIndex < 0 || string.IsNullOrWhiteSpace(twodaFilename))
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

                List<ReferenceSearchResult> results = CollectTwoDARowReferences(
                    twodaFilename,
                    rowIndex,
                    twoDA,
                    installation,
                    options);

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

        public static List<ReferenceSearchResult> CollectTwoDARowReferences(
            string twodaFilename,
            int rowIndex,
            TwoDA twoDA,
            OdyInstallation installation,
            ReferenceSearchOptions options = null)
        {
            var results = new List<ReferenceSearchResult>();
            if (installation?.Installation == null || rowIndex < 0 || string.IsNullOrWhiteSpace(twodaFilename))
            {
                return results;
            }

            options = options ?? new ReferenceSearchOptions
            {
                SearchChitin = true,
                SearchModules = true,
                SearchOverride = true
            };

            results.AddRange(ReferenceCacheHelpers.Find2DAMemoryReferences(
                installation.Installation,
                twodaFilename,
                rowIndex,
                null,
                null,
                options));

            if (twoDA != null && rowIndex >= 0 && rowIndex < twoDA.GetHeight())
            {
                string rowLabel = twoDA.GetLabel(rowIndex);
                if (!string.IsNullOrWhiteSpace(rowLabel))
                {
                    results.AddRange(ReferenceFinder.FindFieldValueReferences(
                        installation.Installation,
                        rowLabel.Trim(),
                        null,
                        options));
                }

                foreach (string header in twoDA.GetHeaders())
                {
                    if (string.IsNullOrEmpty(header) || header == ">>##HEADER##<<")
                    {
                        continue;
                    }

                    string cellValue = twoDA.GetCellString(rowIndex, header);
                    if (string.IsNullOrWhiteSpace(cellValue))
                    {
                        continue;
                    }

                    string trimmed = cellValue.Trim();
                    int strref;
                    if (!int.TryParse(trimmed, out strref) || strref <= 0)
                    {
                        continue;
                    }

                    List<StrRefSearchResult> strrefResults = ReferenceCacheHelpers.FindStrRefReferences(
                        installation.Installation,
                        strref,
                        null,
                        null,
                        options);
                    results.AddRange(ReferenceCacheHelpers.ConvertToReferenceSearchResults(strrefResults, strref));
                }
            }

            return results;
        }
    }
}
