using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;

namespace OdyTools.Widgets.Settings
{
    public partial class InstallationsWidget : UserControl
    {
        // Event emitted when installations are edited
        public event EventHandler SettingsEdited;

        private ListBox _pathList;
        private Button _addPathButton;
        private Button _removePathButton;
        private Border _pathFrame;
        private TextBox _pathNameEdit;
        private TextBox _pathDirEdit;
        private CheckBox _pathTslCheckbox;
        private GlobalSettings _settings;
        
        // Store installation data: name -> {path, tsl}
        private Dictionary<string, Dictionary<string, object>> _installationData;

        public InstallationsWidget()
        {
            InitializeComponent();
            _settings = new GlobalSettings();
            _installationData = new Dictionary<string, Dictionary<string, object>>();
            SetupValues();
            SetupSignals();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InstallationsWidget: XAML load failed: {ex.Message}");
            }

            if (xamlLoaded)
            {
                _pathList = this.FindControl<ListBox>("pathList");
                _addPathButton = this.FindControl<Button>("addPathButton");
                _removePathButton = this.FindControl<Button>("removePathButton");
                _pathFrame = this.FindControl<Border>("pathFrame");
                _pathNameEdit = this.FindControl<TextBox>("pathNameEdit");
                _pathDirEdit = this.FindControl<TextBox>("pathDirEdit");
                _pathTslCheckbox = this.FindControl<CheckBox>("pathTslCheckbox");
            }
        }

        private void SetupValues()
        {
            if (_pathList != null)
            {
                _pathList.Items.Clear();
                _installationData.Clear();
                var installations = _settings.Installations();
                foreach (var kvp in installations)
                {
                    string name = kvp.Key;
                    var installData = kvp.Value;
                    _pathList.Items.Add(name);
                    // Store installation data
                    _installationData[name] = new Dictionary<string, object>
                    {
                        { "name", name },
                        { "path", installData.ContainsKey("path") ? installData["path"] : "" },
                        { "tsl", installData.ContainsKey("tsl") ? installData["tsl"] : false }
                    };
                }
            }
        }

        private void SetupSignals()
        {
            if (_addPathButton != null)
            {
                _addPathButton.Click += (s, e) => AddNewInstallation();
            }
            if (_removePathButton != null)
            {
                _removePathButton.Click += (s, e) => RemoveSelectedInstallation();
            }
            if (_pathNameEdit != null)
            {
                _pathNameEdit.TextChanged += (s, e) => UpdateInstallation();
            }
            if (_pathDirEdit != null)
            {
                _pathDirEdit.TextChanged += (s, e) => UpdateInstallation();
            }
            if (_pathTslCheckbox != null)
            {
                _pathTslCheckbox.IsCheckedChanged += (s, e) => UpdateInstallation();
            }
            if (_pathList != null)
            {
                _pathList.SelectionChanged += (s, e) => InstallationSelected();
            }
        }

        public void Save()
        {
            if (_pathList == null) return;

            Dictionary<string, Dictionary<string, object>> installations = new Dictionary<string, Dictionary<string, object>>();

            foreach (var item in _pathList.Items)
            {
                string itemText = item?.ToString() ?? "";
                if (string.IsNullOrEmpty(itemText))
                {
                    continue;
                }

                // Get installation data for this name
                if (_installationData.ContainsKey(itemText))
                {
                    var installData = new Dictionary<string, object>(_installationData[itemText]);
                    installData["name"] = itemText;
                    installations[itemText] = installData;
                }
                else
                {
                    // New installation without data - create default entry
                    installations[itemText] = new Dictionary<string, object>
                    {
                        { "name", itemText },
                        { "path", "" },
                        { "tsl", false }
                    };
                }
            }

            _settings.SetInstallations(installations);
        }

        private void AddNewInstallation()
        {
            if (_pathList != null)
            {
                string newName = "New";
                _pathList.Items.Add(newName);
                
                _installationData[newName] = new Dictionary<string, object>
                {
                    { "name", newName },
                    { "path", "" },
                    { "tsl", false }
                };
                
                SettingsEdited?.Invoke(this, EventArgs.Empty);
            }
        }

        private void RemoveSelectedInstallation()
        {
            if (_pathList?.SelectedItem != null)
            {
                string selectedName = _pathList.SelectedItem.ToString();
                _pathList.Items.Remove(_pathList.SelectedItem);
                
                // Remove from installation data
                if (_installationData.ContainsKey(selectedName))
                {
                    _installationData.Remove(selectedName);
                }
                
                SettingsEdited?.Invoke(this, EventArgs.Empty);
            }
            if (_pathList?.SelectedItem == null && _pathFrame != null)
            {
                _pathFrame.IsEnabled = false;
            }
        }

        private void UpdateInstallation()
        {
            if (_pathList?.SelectedItem == null)
            {
                return;
            }

            string selectedName = _pathList.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedName))
            {
                return;
            }

            // Get or create installation data
            if (!_installationData.ContainsKey(selectedName))
            {
                _installationData[selectedName] = new Dictionary<string, object>
                {
                    { "name", selectedName },
                    { "path", "" },
                    { "tsl", false }
                };
            }

            var data = _installationData[selectedName];
            
            if (_pathDirEdit != null)
            {
                data["path"] = _pathDirEdit.Text ?? "";
            }
            
            if (_pathTslCheckbox != null)
            {
                data["tsl"] = _pathTslCheckbox.IsChecked ?? false;
            }

            if (_pathNameEdit != null)
            {
                string newName = _pathNameEdit.Text ?? "";
                if (!string.IsNullOrEmpty(newName) && newName != selectedName)
                {
                    // Name changed - update the item in the list
                    int index = _pathList.Items.IndexOf(selectedName);
                    if (index >= 0)
                    {
                        // Rebuild list with updated name at same index (Items may not support indexer set)
                        var items = new List<object>();
                        foreach (var i in _pathList.Items) items.Add(i);
                        items[index] = newName;
                        _pathList.Items.Clear();
                        foreach (var i in items) _pathList.Items.Add(i);
                        _pathList.SelectedItem = newName;

                        // Update dictionary key
                        if (_installationData.ContainsKey(selectedName))
                        {
                            var oldData = _installationData[selectedName];
                            _installationData.Remove(selectedName);
                            oldData["name"] = newName;
                            _installationData[newName] = oldData;
                        }
                    }
                }
            }

            SettingsEdited?.Invoke(this, EventArgs.Empty);
        }

        private void InstallationSelected()
        {
            if (_pathList?.SelectedItem != null && _pathFrame != null)
            {
                _pathFrame.IsEnabled = true;
                
                string selectedName = _pathList.SelectedItem.ToString();
                
                if (_installationData.ContainsKey(selectedName))
                {
                    var itemData = _installationData[selectedName];
                    
                    if (_pathNameEdit != null)
                    {
                        _pathNameEdit.Text = selectedName;
                    }
                    
                    if (_pathDirEdit != null)
                    {
                        _pathDirEdit.Text = itemData.ContainsKey("path") ? itemData["path"]?.ToString() ?? "" : "";
                    }
                    
                    if (_pathTslCheckbox != null)
                    {
                        bool tslValue = false;
                        if (itemData.ContainsKey("tsl") && itemData["tsl"] is bool tsl)
                        {
                            tslValue = tsl;
                        }
                        else if (itemData.ContainsKey("tsl"))
                        {
                            bool.TryParse(itemData["tsl"]?.ToString(), out tslValue);
                        }
                        _pathTslCheckbox.IsChecked = tslValue;
                    }
                }
                else
                {
                    // No data for this installation - load defaults
                    if (_pathNameEdit != null)
                    {
                        _pathNameEdit.Text = selectedName;
                    }
                    if (_pathDirEdit != null)
                    {
                        _pathDirEdit.Text = "";
                    }
                    if (_pathTslCheckbox != null)
                    {
                        _pathTslCheckbox.IsChecked = false;
                    }
                }
            }
            else if (_pathFrame != null)
            {
                _pathFrame.IsEnabled = false;
            }
        }
    }
}
