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
    /// Holocron-style reference search for scripts, tags, and template ResRefs.
    /// </summary>
    public static class ReferenceSearchHelper
    {
        public static void AttachTagFindReferencesMenu(
            TextBox tagEdit,
            Window parent,
            OdyInstallation installation)
        {
            if (tagEdit == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var findReferencesItem = new MenuItem
            {
                Header = "Find Tag References",
                IsEnabled = false
            };
            findReferencesItem.Click += (sender, e) =>
                FindAndShowTagReferences(parent, tagEdit.Text, installation, showOptionsDialog: true);
            contextMenu.Items.Add(findReferencesItem);

            void UpdateEnabled(object s, EventArgs e)
            {
                findReferencesItem.IsEnabled = !string.IsNullOrWhiteSpace(tagEdit.Text) && installation?.Installation != null;
            }

            tagEdit.TextChanged += UpdateEnabled;
            contextMenu.Opened += UpdateEnabled;
            tagEdit.ContextMenu = contextMenu;
        }

        public static void AttachTemplateResRefFindReferencesMenu(
            TextBox resRefEdit,
            Window parent,
            OdyInstallation installation)
        {
            if (resRefEdit == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var findReferencesItem = new MenuItem
            {
                Header = "Find Template ResRef References",
                IsEnabled = false
            };
            findReferencesItem.Click += (sender, e) =>
                FindAndShowTemplateResRefReferences(parent, resRefEdit.Text, installation, showOptionsDialog: true);
            contextMenu.Items.Add(findReferencesItem);

            void UpdateEnabled(object s, EventArgs e)
            {
                findReferencesItem.IsEnabled = !string.IsNullOrWhiteSpace(resRefEdit.Text) && installation?.Installation != null;
            }

            resRefEdit.TextChanged += UpdateEnabled;
            contextMenu.Opened += UpdateEnabled;
            resRefEdit.ContextMenu = contextMenu;
        }

        public static ReferenceSearchOptions PromptSearchOptions(Window parent, ReferenceSearchOptions defaults)
        {
            return PromptSearchOptions(parent, defaults, showStrRefNcsOptions: false);
        }

        public static ReferenceSearchOptions PromptSearchOptions(
            Window parent,
            ReferenceSearchOptions defaults,
            bool showStrRefNcsOptions)
        {
            defaults = defaults ?? new ReferenceSearchOptions();
            var dialog = new ReferenceSearchOptionsDialog(parent, showStrRefNcsOptions);
            dialog.SetDefaults(defaults);

            if (!dialog.ShowDialogAndAccepted(parent))
            {
                return null;
            }

            return dialog.ToSearchOptions();
        }

        public static void FindAndShowTagReferences(
            Window parent,
            string tag,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            RunSearch(
                parent,
                tag,
                installation,
                showOptionsDialog,
                ReferenceFinder.FindTagReferences,
                "Find Tag References Failed");
        }

        public static void FindAndShowTemplateResRefReferences(
            Window parent,
            string templateResRef,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            RunSearch(
                parent,
                templateResRef,
                installation,
                showOptionsDialog,
                ReferenceFinder.FindTemplateResRefReferences,
                "Find Template ResRef References Failed");
        }

        public static void FindAndShowScriptReferences(
            Window parent,
            string scriptResRef,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            RunSearch(
                parent,
                scriptResRef,
                installation,
                showOptionsDialog,
                ReferenceFinder.FindScriptReferences,
                "Find References Failed");
        }

        public static void FindAndShowConversationReferences(
            Window parent,
            string conversationResRef,
            OdyInstallation installation,
            bool showOptionsDialog = false)
        {
            RunSearch(
                parent,
                conversationResRef,
                installation,
                showOptionsDialog,
                ReferenceFinder.FindConversationResRefReferences,
                "Find Conversation References Failed");
        }

        private static void RunSearch(
            Window parent,
            string needle,
            OdyInstallation installation,
            bool showOptionsDialog,
            Func<BioWare.Extract.Installation, string, ReferenceSearchOptions, List<ReferenceSearchResult>> searchFunc,
            string errorTitle)
        {
            if (installation?.Installation == null || string.IsNullOrWhiteSpace(needle))
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
                    ReferenceSearchOptions chosen = PromptSearchOptions(parent, options);
                    if (chosen == null)
                    {
                        return;
                    }

                    options = chosen;
                }

                List<ReferenceSearchResult> results = searchFunc(
                    installation.Installation,
                    needle.Trim(),
                    options);

                FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(parent, results, installation);
                dialog.Show();
            }
            catch (Exception ex)
            {
                _ = DialogHelper.ShowAsync(errorTitle, ex.Message, ButtonEnum.Ok, IconType.Error);
            }
        }
    }
}
