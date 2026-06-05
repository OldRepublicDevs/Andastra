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
    /// Holocron-style GFF string/ResRef field-value reference search.
    /// </summary>
    public static class FieldValueReferenceHelper
    {
        public static void AttachFieldValueFindReferencesMenu(
            TextBox valueEdit,
            Window parent,
            OdyInstallation installation,
            Func<string> getFieldNameFilter = null)
        {
            if (valueEdit == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var findReferencesItem = new MenuItem
            {
                Header = "Find Field Value References",
                IsEnabled = false
            };
            findReferencesItem.Click += (sender, e) =>
            {
                HashSet<string> fieldNames = BuildFieldNameFilter(getFieldNameFilter);
                FindAndShowFieldValueReferences(
                    parent,
                    valueEdit.Text,
                    installation,
                    fieldNames,
                    showOptionsDialog: true);
            };
            contextMenu.Items.Add(findReferencesItem);

            void UpdateEnabled(object s, EventArgs e)
            {
                findReferencesItem.IsEnabled =
                    !string.IsNullOrWhiteSpace(valueEdit.Text)
                    && installation?.Installation != null;
            }

            valueEdit.TextChanged += UpdateEnabled;
            contextMenu.Opened += UpdateEnabled;
            valueEdit.ContextMenu = contextMenu;
            UpdateEnabled(null, EventArgs.Empty);
        }

        public static void FindAndShowFieldValueReferences(
            Window parent,
            string value,
            OdyInstallation installation,
            HashSet<string> fieldNames = null,
            bool showOptionsDialog = false)
        {
            if (installation?.Installation == null || string.IsNullOrWhiteSpace(value))
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

                List<ReferenceSearchResult> results = CollectFieldValueReferences(
                    value,
                    installation,
                    fieldNames,
                    options);

                if (results.Count == 0)
                {
                    _ = DialogHelper.ShowAsync(
                        "No references found",
                        "No references found for field value \"" + value.Trim() + "\".",
                        ButtonEnum.Ok,
                        IconType.Info);
                    return;
                }

                FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(parent, results, installation);
                dialog.Show();
            }
            catch (Exception ex)
            {
                _ = DialogHelper.ShowAsync(
                    "Find Field Value References Failed",
                    ex.Message,
                    ButtonEnum.Ok,
                    IconType.Error);
            }
        }

        public static List<ReferenceSearchResult> CollectFieldValueReferences(
            string value,
            OdyInstallation installation,
            HashSet<string> fieldNames = null,
            ReferenceSearchOptions options = null)
        {
            if (installation?.Installation == null || string.IsNullOrWhiteSpace(value))
            {
                return new List<ReferenceSearchResult>();
            }

            options = options ?? new ReferenceSearchOptions
            {
                SearchChitin = true,
                SearchModules = true,
                SearchOverride = true
            };

            return ReferenceFinder.FindFieldValueReferences(
                installation.Installation,
                value.Trim(),
                fieldNames,
                options);
        }

        internal static HashSet<string> BuildFieldNameFilter(Func<string> getFieldNameFilter)
        {
            if (getFieldNameFilter == null)
            {
                return null;
            }

            string fieldName = getFieldNameFilter();
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return null;
            }

            return new HashSet<string> { fieldName.Trim() };
        }
    }
}
