using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Editors.DLG;

namespace OdyTools.Dialogs.Edit
{
    public partial class DialogModelDialog : Window
    {
        private DLGStunt _stunt;
        private TextBox _participantEdit;
        private ComboBox _stuntModelCombo;
        private Button _okButton;
        private Button _cancelButton;
        private OdyToolDLG _editor;

        // Public parameterless constructor for XAML
        public DialogModelDialog() : this(null, null)
        {
        }

        public DialogModelDialog(Window parent, DLGStunt stunt = null)
        {
            _editor = parent as OdyToolDLG;
            InitializeComponent();
            _stunt = stunt ?? new DLGStunt();
            SetupUI();
            ApplyLocalization();
            LoadStuntData();
        }

        public DialogModelDialog(OdyToolDLG editor, DLGStunt stunt = null)
        {
            _editor = editor;
            InitializeComponent();
            _stunt = stunt ?? new DLGStunt();
            SetupUI();
            ApplyLocalization();
            LoadStuntData();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void ApplyLocalization()
        {
            Title = Localization.Tr("Edit Cutscene Model");
            if (_okButton != null) _okButton.Content = Localization.Tr("OK");
            if (_cancelButton != null) _cancelButton.Content = Localization.Tr("Cancel");
        }

        private void SetupProgrammaticUI()
        {
            Title = Localization.Tr("Edit Cutscene Model");
            Width = 400;
            Height = 200;

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            var participantLabel = new TextBlock { Text = Localization.Tr("Participant:") };
            _participantEdit = new TextBox();
            var stuntLabel = new TextBlock { Text = Localization.Tr("Stunt Model:") };
            _stuntModelCombo = new ComboBox { IsEditable = true };
            var okButton = new Button { Content = Localization.Tr("OK") };
            okButton.Click += (s, e) => Close(true);
            var cancelButton = new Button { Content = Localization.Tr("Cancel") };
            cancelButton.Click += (s, e) => Close(false);

            panel.Children.Add(participantLabel);
            panel.Children.Add(_participantEdit);
            panel.Children.Add(stuntLabel);
            panel.Children.Add(_stuntModelCombo);
            panel.Children.Add(okButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _participantEdit = this.FindControl<TextBox>("participantEdit");
            _stuntModelCombo = this.FindControl<ComboBox>("stuntModelCombo");
            _okButton = this.FindControl<Button>("okButton");
            _cancelButton = this.FindControl<Button>("cancelButton");

            if (_okButton != null)
            {
                _okButton.Click += (s, e) => Close(true);
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close(false);
            }
        }

        private void LoadStuntData()
        {
            if (_participantEdit != null)
            {
                _participantEdit.Text = _stunt.Participant ?? "";
            }
            if (_stuntModelCombo != null)
            {
                PopulateStuntModelCombo();
                _stuntModelCombo.Text = _stunt.StuntModel?.ToString() ?? "";
            }
        }

        /// <summary>
        /// Pre-fills the stunt model combo with ResourceType.MDL resrefs scoped to the open DLG:
        /// if the DLG is in a module, only override + same module + chitin; if in override/chitin, only override + chitin.
        /// </summary>
        private void PopulateStuntModelCombo()
        {
            if (_stuntModelCombo == null) return;

            var resnames = new List<string>();
            var editorBase = _editor as Editor;
            OdyInstallation installation = editorBase?.Installation;
            string filepath = editorBase?.FilepathPublic;

            if (installation != null)
            {
                var relevant = installation.GetRelevantResources(ResourceType.MDL, filepath);
                foreach (var res in relevant)
                {
                    if (res?.ResName != null && !string.IsNullOrWhiteSpace(res.ResName))
                    {
                        resnames.Add(res.ResName.Trim().ToLowerInvariant());
                    }
                }
            }

            resnames = resnames.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
            _stuntModelCombo.ItemsSource = resnames;
        }

        public DLGStunt GetStunt()
        {
            var stunt = new DLGStunt();
            if (_participantEdit != null)
            {
                stunt.Participant = _participantEdit.Text ?? "";
            }
            if (_stuntModelCombo != null)
            {
                string stuntText = _stuntModelCombo.Text?.Trim() ?? "";
                if (ResRef.IsValid(stuntText))
                {
                    stunt.StuntModel = new ResRef(stuntText);
                }
            }
            return stunt;
        }
    }
}
