using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Utils;

namespace OdyTools.Widgets.Edit
{
    public partial class ComboBox2DA : ComboBox
    {
        // Item wrapper to store row index and display text
        private class ComboBoxItem
        {
            public string DisplayText { get; set; }
            public int RowIndex { get; set; }
            public string RealText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }

        private bool _sortAlphabetically = false;
        private TwoDA _this2DA; // Can be null (matching Python: 2DA | None)
        private OdyInstallation _installation;
        private string _resname;

        // Public parameterless constructor for XAML
        public ComboBox2DA()
        {
            InitializeComponent();
            AttachContextMenu();
        }

        private void AttachContextMenu()
        {
            var contextMenu = new ContextMenu();
            var findReferencesItem = new MenuItem
            {
                Header = "Find References...",
                IsEnabled = false
            };
            findReferencesItem.Click += (sender, e) =>
            {
                if (_installation?.Installation == null || string.IsNullOrEmpty(_resname))
                {
                    return;
                }

                int rowIndex = SelectedIndex;
                TwoDAMemoryReferenceHelper.FindAndShowTwoDAMemoryReferences(
                    TopLevel.GetTopLevel(this) as Window,
                    _resname,
                    rowIndex,
                    _installation);
            };
            contextMenu.Items.Add(findReferencesItem);

            void UpdateEnabled(object s, EventArgs e)
            {
                findReferencesItem.IsEnabled =
                    _installation?.Installation != null
                    && !string.IsNullOrEmpty(_resname)
                    && _this2DA != null
                    && SelectedIndex >= 0;
            }

            SelectionChanged += UpdateEnabled;
            contextMenu.Opened += UpdateEnabled;
            ContextMenu = contextMenu;
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }
        }

        public new int SelectedIndex
        {
            get
            {
                int currentIndex = base.SelectedIndex;
                if (currentIndex == -1)
                {
                    return 0;
                }
                // Get row index from item data (matching PyKotor: itemData(currentIndex))
                if (currentIndex >= 0 && currentIndex < Items.Count)
                {
                    object item = Items[currentIndex];
                    if (item is ComboBoxItem comboItem)
                    {
                        return comboItem.RowIndex;
                    }
                }
                return currentIndex;
            }
        }

        // Python implementation: Iterates through items, finds one with matching row index via itemData(), sets currentIndex
        public void SetSelectedIndex(int rowIn2DA)
        {
            // Find item with matching row index (matching PyKotor: searches items for matching itemData)
            for (int i = 0; i < Items.Count; i++)
            {
                object item = Items[i];
                if (item is ComboBoxItem comboItem && comboItem.RowIndex == rowIn2DA)
                {
                    base.SelectedIndex = i;
                    return;
                }
            }
            // If no match found and rowIn2DA is within valid range, set directly (fallback behavior)
            if (rowIn2DA >= 0 && rowIn2DA < Items.Count)
            {
                base.SelectedIndex = rowIn2DA;
            }
        }

        // Python implementation: Stores row index via setItemData(), stores real text separately
        public void AddItem(string text, int? row = null)
        {
            int rowIndex = row ?? Items.Count;
            string displayText = text.StartsWith("[Modded Entry #") ? text : $"{text} [{rowIndex}]";
            // Store row index and real text in item data (matching PyKotor: setItemData())
            ComboBoxItem item = new ComboBoxItem
            {
                DisplayText = displayText,
                RowIndex = rowIndex,
                RealText = text
            };
            Items.Add(item);
        }

        // Python implementation: Clears items, adds each value with cleanup/blank filtering, then sorts if enabled
        public void SetItems(IEnumerable<string> values, bool sortAlphabetically = true, bool cleanupStrings = true, bool ignoreBlanks = false)
        {
            _sortAlphabetically = sortAlphabetically;
            Items.Clear();

            int index = 0;
            foreach (string text in values)
            {
                string newText = text;
                if (cleanupStrings)
                {
                    newText = text.Replace("TRAP_", "");
                    newText = newText.Replace("GENDER_", "");
                    newText = newText.Replace("_", " ");
                }
                if (!ignoreBlanks || (!string.IsNullOrEmpty(newText) && !string.IsNullOrWhiteSpace(newText)))
                {
                    AddItem(newText, index);
                }
                index++;
            }

            // Sort items alphabetically by display text if enabled (matching PyKotor: model().sort(0) when sortAlphabetically is True)
            if (_sortAlphabetically && Items.Count > 0)
            {
                var itemsList = Items.Cast<ComboBoxItem>().ToList();
                itemsList.Sort((a, b) => string.Compare(a.DisplayText, b.DisplayText, StringComparison.OrdinalIgnoreCase));
                Items.Clear();
                foreach (var item in itemsList)
                {
                    Items.Add(item);
                }
            }
        }

        /// <param name="data">2DA data for the combo.</param>
        /// <param name="install">Installation (may be null when using DLG override paths).</param>
        /// <param name="resname">Resource name of the 2DA.</param>
        public void SetContext(TwoDA data, OdyInstallation install, string resname)
        {
            _this2DA = data;
            _installation = install;
            _resname = resname;
        }
    }
}
