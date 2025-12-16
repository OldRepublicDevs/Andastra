using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.Widgets
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:17
    // Original: class SetBindWidget(QWidget):
    public partial class SetBindWidget : UserControl
    {
        private HashSet<Key> _keybind = new HashSet<Key>();
        private bool _recordBind = false;
        private ComboBox _mouseCombo;
        private TextBox _setKeysEdit;
        private Button _setButton;
        private Button _clearButton;

        // Public parameterless constructor for XAML
        public SetBindWidget()
        {
            InitializeComponent();
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
            var panel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            _mouseCombo = new ComboBox { MinWidth = 80 };
            _mouseCombo.Items.Add("Left");
            _mouseCombo.Items.Add("Middle");
            _mouseCombo.Items.Add("Right");
            _mouseCombo.Items.Add("Any");
            _mouseCombo.Items.Add("None");
            _setKeysEdit = new TextBox { IsReadOnly = true, Watermark = "none" };
            _setButton = new Button { Content = "Set", MaxWidth = 40 };
            _setButton.Click += (s, e) => StartRecording();
            _clearButton = new Button { Content = "Clear", MaxWidth = 40 };
            _clearButton.Click += (s, e) => ClearKeybind();
            panel.Children.Add(_mouseCombo);
            panel.Children.Add(_setKeysEdit);
            panel.Children.Add(_setButton);
            panel.Children.Add(_clearButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _mouseCombo = this.FindControl<ComboBox>("mouseCombo");
            _setKeysEdit = this.FindControl<TextBox>("setKeysEdit");
            _setButton = this.FindControl<Button>("setButton");
            _clearButton = this.FindControl<Button>("clearButton");

            if (_setButton != null)
            {
                _setButton.Click += (s, e) => StartRecording();
            }
            if (_clearButton != null)
            {
                _clearButton.Click += (s, e) => ClearKeybind();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:39-44
        // Original: def start_recording(self):
        private void StartRecording()
        {
            _recordBind = true;
            _keybind.Clear();
            UpdateKeybindText();
            if (_setKeysEdit != null)
            {
                _setKeysEdit.Watermark = "Enter a key...";
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:46-50
        // Original: def clear_keybind(self):
        private void ClearKeybind()
        {
            _keybind.Clear();
            if (_setKeysEdit != null)
            {
                _setKeysEdit.Watermark = "none";
            }
            UpdateKeybindText();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:52-57
        // Original: def keyPressEvent(self, a0: QKeyEvent):
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (_recordBind)
            {
                _keybind.Add(e.Key);
                UpdateKeybindText();
            }
            base.OnKeyDown(e);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:58-61
        // Original: def keyReleaseEvent(self, e: QKeyEvent):
        protected override void OnKeyUp(KeyEventArgs e)
        {
            _recordBind = false;
            System.Console.WriteLine($"Set keybind to {string.Join(",", _keybind)}");
            base.OnKeyUp(e);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:93-98
        // Original: def update_keybind_text(self):
        private void UpdateKeybindText()
        {
            if (_setKeysEdit == null)
            {
                return;
            }

            // TODO: Implement key string localization when available
            var sortedKeys = _keybind.OrderBy(k => k.ToString()).ToList();
            string text = string.Join("+", sortedKeys.Select(k => k.ToString().ToUpperInvariant()));
            _setKeysEdit.Text = text;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:63-85
        // Original: def set_mouse_and_key_binds(self, bind: Bind):
        public void SetMouseAndKeyBinds(Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> bind)
        {
            // TODO: Implement when Bind type is available
            // For now, just update keybind
            if (bind != null && bind.Item1 != null)
            {
                _keybind = bind.Item1;
                UpdateKeybindText();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/set_bind.py:87-91
        // Original: def get_mouse_and_key_binds(self) -> Bind:
        public Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> GetMouseAndKeyBinds()
        {
            // TODO: Extract mouse bind from combo box when available
            return Tuple.Create(_keybind, new HashSet<PointerUpdateKind>());
        }
    }
}
