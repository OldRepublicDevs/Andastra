using BioWare.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare;
using BioWare.Resource;
using BioWare.Resource.Formats.TLK;
using OdyTools.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Dialogs
{
    public partial class LocalizedStringDialog : Window
    {
        private OdyInstallation _installation;
        private string _tlkPath;
        private string _femaleTlkPath;
        public LocalizedString LocString { get; private set; }

        // Public parameterless constructor for XAML
        public LocalizedStringDialog() : this(null, null, null)
        {
        }

        public LocalizedStringDialog(Window parent, OdyInstallation installation, LocalizedString locstring)
        {
            InitializeComponent();
            _installation = installation;
            _tlkPath = null;
            _femaleTlkPath = null;
            LocString = locstring ?? LocalizedString.FromInvalid();
            SetupUI();
        }

        /// <summary>
        /// Constructor for path-based TLK when no installation is set (DLG override paths).
        /// </summary>
        public LocalizedStringDialog(Window parent, string tlkPath, string femaleTlkPath, LocalizedString locstring)
        {
            InitializeComponent();
            _installation = null;
            _tlkPath = tlkPath;
            _femaleTlkPath = femaleTlkPath;
            LocString = locstring ?? LocalizedString.FromInvalid();
            SetupUI();
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

        private void SetupProgrammaticUI()
        {
            Title = "Localized String Editor";
            Width = 500;
            Height = 400;

            var panel = new StackPanel();
            var stringrefLabel = new TextBlock { Text = "StringRef:" };
            var stringrefSpin = new NumericUpDown { Minimum = -1, Maximum = 999999 };
            var stringEdit = new TextBox { AcceptsReturn = true, Watermark = "Text" };
            var okButton = new Button { Content = "OK" };
            okButton.Click += (s, e) => { LocString = LocString ?? LocalizedString.FromInvalid(); Close(true); };
            var cancelButton = new Button { Content = "Cancel" };
            cancelButton.Click += (s, e) => Close(false);

            panel.Children.Add(stringrefLabel);
            panel.Children.Add(stringrefSpin);
            panel.Children.Add(stringEdit);
            panel.Children.Add(okButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private NumericUpDown _stringrefSpin;
        private Button _stringrefNewButton;
        private Button _stringrefNoneButton;
        private ComboBox _languageSelect;
        private RadioButton _maleRadio;
        private RadioButton _femaleRadio;
        private TextBox _stringEdit;
        private Button _okButton;
        private Button _cancelButton;
        private List<Language> _orderedLanguages;

        private void SetupUI()
        {
            // Find controls from XAML
            _stringrefSpin = this.FindControl<NumericUpDown>("stringrefSpin");
            _stringrefNewButton = this.FindControl<Button>("stringrefNewButton");
            _stringrefNoneButton = this.FindControl<Button>("stringrefNoneButton");
            _languageSelect = this.FindControl<ComboBox>("languageSelect");
            _maleRadio = this.FindControl<RadioButton>("maleRadio");
            _femaleRadio = this.FindControl<RadioButton>("femaleRadio");
            _stringEdit = this.FindControl<TextBox>("stringEdit");
            _okButton = this.FindControl<Button>("okButton");
            _cancelButton = this.FindControl<Button>("cancelButton");

            if (_okButton != null)
            {
                _okButton.Click += (s, e) => Accept();
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close(false);
            }
            if (_stringrefNoneButton != null)
            {
                _stringrefNoneButton.Click += (s, e) => NoTlkString();
            }
            if (_stringrefNewButton != null)
            {
                _stringrefNewButton.Click += (s, e) => NewTlkString();
            }
            if (_stringrefSpin != null)
            {
                _stringrefSpin.ValueChanged += (s, e) => StringRefChanged((int)_stringrefSpin.Value);
            }
            if (_maleRadio != null)
            {
                _maleRadio.IsCheckedChanged += (s, e) => SubstringChanged();
            }
            if (_femaleRadio != null)
            {
                _femaleRadio.IsCheckedChanged += (s, e) => SubstringChanged();
            }
            if (_languageSelect != null)
            {
                _languageSelect.SelectionChanged += (s, e) => SubstringChanged();
            }
            if (_stringEdit != null)
            {
                _stringEdit.TextChanged += (s, e) => StringEdited();
            }

            // Populate language combo box with all Language enum values
            // The combo box index directly maps to the Language enum value
            if (_languageSelect != null)
            {
                _languageSelect.Items.Clear();
                // Get all Language enum values, excluding Unknown, and sort by their integer values
                // Store the ordered list for later lookup
                _orderedLanguages = Enum.GetValues(typeof(Language))
                    .Cast<Language>()
                    .Where(lang => lang != Language.Unknown)
                    .OrderBy(lang => (int)lang)
                    .ToList();

                foreach (var language in _orderedLanguages)
                {
                    _languageSelect.Items.Add(language.ToString());
                }

                // Set default selection to English (index 0)
                if (_languageSelect.Items.Count > 0)
                {
                    _languageSelect.SelectedIndex = 0;
                }
            }

            // Load current locstring values
            if (LocString != null && _stringrefSpin != null)
            {
                _stringrefSpin.Value = LocString.StringRef;
                StringRefChanged(LocString.StringRef);
            }
        }

        private void StringRefChanged(int stringref)
        {
            var substringFrame = this.FindControl<Control>("substringFrame");
            if (substringFrame != null)
            {
                substringFrame.IsVisible = stringref == -1;
            }

            if (LocString != null)
            {
                LocString.StringRef = stringref;
            }

            if (stringref == -1)
            {
                UpdateText();
            }
            else if (_stringEdit != null)
            {
                if (_installation != null)
                {
                    _stringEdit.Text = _installation.String(LocString);
                }
                else if (!string.IsNullOrWhiteSpace(_tlkPath) && File.Exists(_tlkPath))
                {
                    try
                    {
                        var talkTable = new TalkTable(_tlkPath);
                        _stringEdit.Text = talkTable.GetString(stringref) ?? "";
                    }
                    catch
                    {
                        _stringEdit.Text = "";
                    }
                }
                else
                {
                    _stringEdit.Text = "";
                }
            }
        }

        private void NewTlkString()
        {
            if (_stringrefSpin == null) return;
            try
            {
                if (_installation != null)
                {
                    var talkTable = _installation.TalkTable();
                    int size = talkTable.Size();
                    _stringrefSpin.Value = size;
                }
                else if (!string.IsNullOrWhiteSpace(_tlkPath) && File.Exists(_tlkPath))
                {
                    var talkTable = new TalkTable(_tlkPath);
                    int size = talkTable.Size();
                    _stringrefSpin.Value = size;
                }
                else
                {
                    _stringrefSpin.Value = 1000;
                }
            }
            catch
            {
                _stringrefSpin.Value = 1000;
            }
        }

        private void NoTlkString()
        {
            if (_stringrefSpin != null)
            {
                _stringrefSpin.Value = -1;
            }
        }

        private void SubstringChanged()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (LocString == null || _languageSelect == null || _stringEdit == null)
            {
                return;
            }

            // Get selected language from combo box index
            int languageIndex = _languageSelect.SelectedIndex;
            if (languageIndex < 0 || _orderedLanguages == null || languageIndex >= _orderedLanguages.Count)
            {
                return;
            }

            Language selectedLanguage = _orderedLanguages[languageIndex];

            // Get selected gender from radio buttons
            Gender selectedGender = Gender.Male;
            if (_femaleRadio != null && _femaleRadio.IsChecked == true)
            {
                selectedGender = Gender.Female;
            }

            // Get text from locstring for the selected language/gender combination
            string text = LocString.Get(selectedLanguage, selectedGender, false);
            if (text == null)
            {
                text = "";
            }

            _stringEdit.Text = text;
        }

        private void StringEdited()
        {
            if (LocString == null || LocString.StringRef != -1 || _stringEdit == null)
            {
                return;
            }

            // Get selected language from combo box index
            int languageIndex = _languageSelect != null ? _languageSelect.SelectedIndex : -1;
            if (languageIndex < 0 || _orderedLanguages == null || languageIndex >= _orderedLanguages.Count)
            {
                return;
            }

            Language selectedLanguage = _orderedLanguages[languageIndex];

            // Get selected gender from radio buttons
            Gender selectedGender = Gender.Male;
            if (_femaleRadio != null && _femaleRadio.IsChecked == true)
            {
                selectedGender = Gender.Female;
            }

            // Update locstring with edited text for the selected language/gender combination
            string editedText = _stringEdit.Text ?? "";
            LocString.SetData(selectedLanguage, selectedGender, editedText);
        }

        private void Accept()
        {
            string tlkPathToSave = _installation != null
                ? System.IO.Path.Combine(_installation.Path, "dialog.tlk")
                : _tlkPath;

            if (LocString != null && LocString.StringRef != -1 && _stringEdit != null && !string.IsNullOrWhiteSpace(tlkPathToSave))
            {
                try
                {
                    if (!File.Exists(tlkPathToSave))
                    {
                        var msgBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                            "Cannot save",
                            "dialog.tlk was not found at the specified path. Creating a new TLK file is not supported; ensure the path points to an existing dialog.tlk.",
                            MsBox.Avalonia.Enums.ButtonEnum.Ok,
                            MsBox.Avalonia.Enums.Icon.Warning);
                        msgBox.ShowWindowDialogAsync(this);
                        Close(false);
                        return;
                    }

                    TLK tlk = TLKAuto.ReadTlk(tlkPathToSave);

                    int stringref = LocString.StringRef;
                    if (tlk.Count <= stringref)
                    {
                        tlk.Resize(stringref + 1);
                    }

                    string text = _stringEdit.Text ?? "";
                    tlk[stringref].Text = text;

                    TLKAuto.WriteTlk(tlk, tlkPathToSave, ResourceType.TLK);
                }
                catch (Exception ex)
                {
                    var errorBox = MessageBoxManager.GetMessageBoxStandard(
                        "Error Saving TLK File",
                        $"Failed to save the TLK file: {ex.Message}",
                        ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    errorBox.ShowAsync();
                }
            }

            Close(true);
        }

        /// <summary>
        /// Shows the dialog modally. Returns true if accepted, false if cancelled.
        /// Call after showing the window (e.g. await window.ShowDialog(parent)) and read result from dialog state.
        /// </summary>
        public bool ShowDialog()
        {
            return true;
        }
    }
}
