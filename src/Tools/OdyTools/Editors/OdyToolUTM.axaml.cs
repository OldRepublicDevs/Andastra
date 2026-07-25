using BioWare.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using BioWare;
using BioWare.Resource.Formats.GFF;
using BioWare.Extract.Capsule;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;
using InventoryItem = BioWare.Common.InventoryItem;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using UTM = BioWare.Resource.Formats.GFF.Generics.UTM.UTM;
using UTMHelpers = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers;
using UTMItem = BioWare.Resource.Formats.GFF.Generics.UTM.UTMItem;

namespace OdyTools.Editors
{
    public partial class OdyToolUTM : Editor
    {
        private UTM _utm;

        // UI Controls - Basic
        private LocalizedStringEdit _nameEdit;
        public LocalizedStringEdit NameEdit => _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        public TextBox TagEdit => _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        public TextBox ResrefEdit => _resrefEdit;
        private Button _resrefGenerateBtn;
        private NumericUpDown _idSpin;
        public NumericUpDown IdSpin => _idSpin;
        private Button _inventoryButton;

        // UI Controls - Pricing
        private NumericUpDown _markUpSpin;
        public NumericUpDown MarkUpSpin => _markUpSpin;
        private NumericUpDown _markDownSpin;
        public NumericUpDown MarkDownSpin => _markDownSpin;

        // UI Controls - Store (editable combo with prefilled script resnames, matching vendor utm.py)
        private ComboBox _onOpenEdit;
        public ComboBox OnOpenEdit => _onOpenEdit;
        private ComboBox _storeFlagSelect;
        public ComboBox StoreFlagSelect => _storeFlagSelect;

        // UI Controls - Comments
        private TextBox _commentsEdit;
        public TextBox CommentsEdit => _commentsEdit;
        private TabControl _editorSurface;
        internal bool HasStructuredEditorSurface => _editorSurface != null && _nameEdit != null && _markUpSpin != null && _onOpenEdit != null && _commentsEdit != null;
        private bool _loadingUtm;
        private bool _clearInitialDirtyOnOpen = true;

        public OdyToolUTM() : this(null, null) { }
        public OdyToolUTM(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTM", "merchant",
                new[] { ResourceType.UTM, ResourceType.BTM, ResourceType.UTM_XML, ResourceType.UTM_JSON },
                new[] { ResourceType.UTM, ResourceType.BTM, ResourceType.UTM_XML, ResourceType.UTM_JSON },
                installation)
        {
            _installation = installation;
            _utm = new UTM();

            InitializeComponent();
            if (installation != null)
            {
                SetupInstallation(installation);
            }
            New();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;

                // Try to find controls from XAML
                _editorSurface = EditorHelpers.FindControlSafe<TabControl>(this, "editorSurface");
                _nameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "nameEdit");
                _nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "nameEditBtn");
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateButton");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateButton");
                _idSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "idSpin");
                _inventoryButton = EditorHelpers.FindControlSafe<Button>(this, "inventoryButton");
                _markUpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "markUpSpin");
                _markDownSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "markDownSpin");
                _onOpenEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "onOpenEdit");
                if (_onOpenEdit != null) { _onOpenEdit.IsEditable = true; SetupScriptComboBoxContextMenu(_onOpenEdit, "OnOpenStore"); }
                _storeFlagSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "storeFlagSelect");
                // Ensure ComboBox has items populated (even if loaded from XAML)
                if (_storeFlagSelect != null && _storeFlagSelect.Items.Count == 0)
                {
                    _storeFlagSelect.Items.Add("Only Buy");
                    _storeFlagSelect.Items.Add("Only Sell");
                    _storeFlagSelect.Items.Add("Buy and Sell");
                }
                _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");

                // Check if all critical controls were found
                if (_nameEdit == null || _tagEdit == null || _resrefEdit == null || _idSpin == null)
                {
                    xamlLoaded = false; // Some controls missing, use programmatic UI
                }
            }
            catch
            {
                // XAML not available or controls not found - will use programmatic UI
                xamlLoaded = false;
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
                SetupSignals();
                BindDirtyTracking();
                AttachReferenceSearchMenus();
            }
            else
            {
                // XAML loaded, set up signals
                SetupSignals();
                BindDirtyTracking();
                AttachReferenceSearchMenus();
            }
        }

        private void AttachReferenceSearchMenus()
        {
            if (_tagEdit == null || _resrefEdit == null)
            {
                return;
            }

            ReferenceSearchHelper.AttachTagFindReferencesMenu(_tagEdit, this, _installation);
            FieldValueReferenceHelper.AppendFieldValueFindReferencesMenuItem(
                _tagEdit.ContextMenu,
                _tagEdit,
                this,
                _installation,
                () => "Tag");
            ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(_resrefEdit, this, _installation);
            FieldValueReferenceHelper.AppendFieldValueFindReferencesMenuItem(
                _resrefEdit.ContextMenu,
                _resrefEdit,
                this,
                _installation,
                () => "TemplateResRef");
        }

        private void SetupSignals()
        {
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            EditorHelpers.BindClick(_inventoryButton, OpenInventory);
            EditorHelpers.BindClick(_nameEditBtn, ChangeName);
        }

        private void BindDirtyTracking()
        {
            if (_tagEdit != null) _tagEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_resrefEdit != null) _resrefEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_idSpin != null) _idSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_markUpSpin != null) _markUpSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_markDownSpin != null) _markDownSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_onOpenEdit != null)
            {
                _onOpenEdit.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
                _onOpenEdit.PropertyChanged += (s, e) =>
                {
                    if (e.Property.Name == nameof(ComboBox.Text))
                    {
                        MarkDirtyAfterLoad();
                    }
                };
            }
            if (_storeFlagSelect != null) _storeFlagSelect.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
            if (_commentsEdit != null) _commentsEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
        }

        private void MarkDirtyAfterLoad()
        {
            if (!_loadingUtm)
            {
                MarkDocumentDirty();
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            if (!_clearInitialDirtyOnOpen)
            {
                return;
            }

            ClearDirty();
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (_clearInitialDirtyOnOpen)
                {
                    ClearDirty();
                    _clearInitialDirtyOnOpen = false;
                }
            }, Avalonia.Threading.DispatcherPriority.Background);
        }

        private void SetupInstallation(OdyInstallation installation)
        {
            _installation = installation;
            if (_nameEdit != null)
            {
                _nameEdit.SetInstallation(installation);
            }
        }

        protected override void OnInstallationChanged()
        {
            SetupInstallation(_installation);
            PopulateScriptComboBoxes();
        }

        private void PopulateScriptComboBoxes()
        {
            if (_installation == null || _onOpenEdit == null) return;
            try
            {
                var relevantResources = _installation.GetRelevantResources(ResourceType.NCS, FilepathPublic);
                var resnames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (relevantResources != null)
                {
                    foreach (var res in relevantResources)
                    {
                        if (res != null && !string.IsNullOrEmpty(res.ResName))
                            resnames.Add(res.ResName.ToLowerInvariant());
                    }
                }
                var sorted = resnames.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
                _onOpenEdit.Items.Clear();
                foreach (string r in sorted)
                    _onOpenEdit.Items.Add(r);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to populate script combo box: {ex.Message}");
            }
        }

        private void SetupScriptComboBoxContextMenu(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null) return;
            var contextMenu = new ContextMenu();
            var openInEditorItem = new MenuItem { Header = "Open in OdyToolNSS", IsEnabled = false };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(comboBox, scriptTypeName);
            contextMenu.Items.Add(openInEditorItem);

            var findReferencesItem = new MenuItem { Header = "Find References", IsEnabled = false };
            findReferencesItem.Click += (sender, e) => ScriptReferenceHelper.FindAndShowScriptReferences(this, comboBox, _installation);
            contextMenu.Items.Add(findReferencesItem);

            void UpdateOpenEnabled(object s, EventArgs e)
            {
                string text = comboBox.SelectedItem?.ToString() ?? comboBox.Text ?? string.Empty;
                bool hasScript = !string.IsNullOrWhiteSpace(text);
                openInEditorItem.IsEnabled = hasScript;
                findReferencesItem.IsEnabled = hasScript && _installation != null;
            }
            comboBox.SelectionChanged += UpdateOpenEnabled;
            contextMenu.Opened += (s, e) => UpdateOpenEnabled(s, e);
            comboBox.ContextMenu = contextMenu;
        }

        private void OpenScriptInEditor(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null || _installation == null) return;
            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName)) return;
            try
            {
                var resourceResult = _installation.Resource(scriptName, ResourceType.NSS, null);
                var resourceType = ResourceType.NSS;
                if (resourceResult == null)
                {
                    resourceResult = _installation.Resource(scriptName, ResourceType.NCS, null);
                    resourceType = ResourceType.NCS;
                }
                if (resourceResult == null)
                {
                    System.Console.WriteLine($"Script '{scriptName}' not found in installation.");
                    return;
                }
                byte[] data = resourceResult.Data;
                if (data == null && !string.IsNullOrEmpty(resourceResult.FilePath) && System.IO.File.Exists(resourceResult.FilePath))
                    data = System.IO.File.ReadAllBytes(resourceResult.FilePath);
                if (data == null)
                {
                    System.Console.WriteLine($"No data for script '{scriptName}'.");
                    return;
                }
                WindowUtils.OpenResourceEditor(resourceResult.FilePath, scriptName, resourceType, data, _installation, this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"OpenScriptInEditor failed: {ex.Message}");
            }
        }

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Basic Group
            var basicGroup = new Expander { Header = "Basic", IsExpanded = true };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Name
            var nameLabel = new TextBlock { Text = "Name:" };
            _nameEdit = new LocalizedStringEdit();
            if (_installation != null)
            {
                _nameEdit.SetInstallation(_installation);
            }
            _nameEditBtn = new Button { Content = "Edit Name" };
            EditorHelpers.BindClick(_nameEditBtn, ChangeName);
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEdit);
            basicPanel.Children.Add(_nameEditBtn);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            _tagEdit = new TextBox();
            _tagGenerateBtn = new Button { Content = "-" };
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            var tagPanel = new StackPanel { Orientation = Orientation.Horizontal };
            tagPanel.Children.Add(_tagEdit);
            tagPanel.Children.Add(_tagGenerateBtn);
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(tagPanel);

            // ResRef
            var resrefLabel = new TextBlock { Text = "ResRef:" };
            _resrefEdit = new TextBox { MaxLength = 16 };
            _resrefGenerateBtn = new Button { Content = "-" };
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            var resrefPanel = new StackPanel { Orientation = Orientation.Horizontal };
            resrefPanel.Children.Add(_resrefEdit);
            resrefPanel.Children.Add(_resrefGenerateBtn);
            basicPanel.Children.Add(resrefLabel);
            basicPanel.Children.Add(resrefPanel);

            // ID
            var idLabel = new TextBlock { Text = "ID:" };
            _idSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue };
            basicPanel.Children.Add(idLabel);
            basicPanel.Children.Add(_idSpin);

            // Inventory Button
            _inventoryButton = new Button { Content = "Edit Inventory" };
            EditorHelpers.BindClick(_inventoryButton, OpenInventory);
            basicPanel.Children.Add(_inventoryButton);

            basicGroup.Content = basicPanel;
            mainPanel.Children.Add(basicGroup);

            // Pricing Group
            var pricingGroup = new Expander { Header = "Pricing", IsExpanded = true };
            var pricingPanel = new StackPanel { Orientation = Orientation.Vertical };

            var markUpLabel = new TextBlock { Text = "Mark Up:" };
            _markUpSpin = new NumericUpDown { Minimum = 0, Maximum = 1000000 };
            var markDownLabel = new TextBlock { Text = "Mark Down:" };
            _markDownSpin = new NumericUpDown { Minimum = 0, Maximum = 1000000 };

            pricingPanel.Children.Add(markUpLabel);
            pricingPanel.Children.Add(_markUpSpin);
            pricingPanel.Children.Add(markDownLabel);
            pricingPanel.Children.Add(_markDownSpin);

            pricingGroup.Content = pricingPanel;
            mainPanel.Children.Add(pricingGroup);

            // Store Group
            var storeGroup = new Expander { Header = "Store", IsExpanded = true };
            var storePanel = new StackPanel { Orientation = Orientation.Vertical };

            var onOpenLabel = new TextBlock { Text = "OnOpenStore:" };
            _onOpenEdit = new ComboBox { IsEditable = true };
            SetupScriptComboBoxContextMenu(_onOpenEdit, "OnOpenStore");
            var storeLabel = new TextBlock { Text = "Store:" };
            _storeFlagSelect = new ComboBox();
            _storeFlagSelect.Items.Add("Only Buy");
            _storeFlagSelect.Items.Add("Only Sell");
            _storeFlagSelect.Items.Add("Buy and Sell");

            storePanel.Children.Add(onOpenLabel);
            storePanel.Children.Add(_onOpenEdit);
            storePanel.Children.Add(storeLabel);
            storePanel.Children.Add(_storeFlagSelect);

            storeGroup.Content = storePanel;
            mainPanel.Children.Add(storeGroup);

            // Comments Group
            var commentsGroup = new Expander { Header = "Comments", IsExpanded = false };
            var commentsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var commentsLabel = new TextBlock { Text = "Comment:" };
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            commentsPanel.Children.Add(commentsLabel);
            commentsPanel.Children.Add(_commentsEdit);
            commentsGroup.Content = commentsPanel;
            mainPanel.Children.Add(commentsGroup);

            scrollViewer.Content = mainPanel;
            Content = scrollViewer;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFFAuto.ReadGff(data, fileFormat: restype);
            _utm = UTMHelpers.ConstructUtm(gff);
            LoadUTM(_utm);
        }

        private void LoadUTM(UTM utm)
        {
            _utm = utm;
            _loadingUtm = true;
            try
            {

                // Basic
                if (_nameEdit != null)
                {
                    _nameEdit.SetLocString(utm.Name);
                }
                if (_tagEdit != null)
                {
                    _tagEdit.Text = utm.Tag;
                }
                if (_resrefEdit != null)
                {
                    _resrefEdit.Text = utm.ResRef.ToString();
                }
                if (_idSpin != null)
                {
                    _idSpin.Value = (decimal)utm.Id;
                }
                if (_markUpSpin != null)
                {
                    _markUpSpin.Value = (decimal)utm.MarkUp;
                }
                if (_markDownSpin != null)
                {
                    _markDownSpin.Value = (decimal)utm.MarkDown;
                }
                if (_onOpenEdit != null)
                {
                    _onOpenEdit.Text = utm.OnOpenScript.ToString();
                }
                if (_storeFlagSelect != null)
                {
                    int index = (utm.CanBuy ? 1 : 0) + (utm.CanSell ? 2 : 0) - 1;
                    if (index >= 0 && index < _storeFlagSelect.Items.Count)
                    {
                        _storeFlagSelect.SelectedIndex = index;
                    }
                }

                // Comments
                if (_commentsEdit != null)
                {
                    _commentsEdit.Text = utm.Comment;
                }
            }
            finally
            {
                _loadingUtm = false;
            }

            PopulateScriptComboBoxes();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var utm = CopyUTM(_utm);

            // Basic - read from UI controls (matching Python which always reads from UI)
            utm.Name = _nameEdit?.GetLocString() ?? utm.Name ?? LocalizedString.FromInvalid();
            utm.Tag = _tagEdit?.Text ?? utm.Tag ?? "";
            // Python always reads from UI, even if empty (creates blank ResRef)
            utm.ResRef = _resrefEdit != null ? new ResRef(_resrefEdit.Text ?? "") : utm.ResRef;
            // Python always reads from UI, even if 0
            // Note: NumericUpDown.Value is decimal?, but Python's QSpinBox.value() always returns an int
            // Python: utm.id = self.ui.idSpin.value() - always returns int, never None
            // Python: utm.id = self.ui.idSpin.value() - always returns int, never None
            if (_idSpin != null)
            {
                // Always read from UI (matching Python behavior)
                // Directly read from _idSpin to match Python's direct UI access
                // Python's QSpinBox.value() always returns an int, never None
                // NumericUpDown.Value is decimal?, so we need to handle null
                var value = _idSpin.Value;
                utm.Id = value.HasValue ? (int)value.Value : 0;
            }
            if (_markUpSpin != null && _markUpSpin.Value.HasValue)
            {
                utm.MarkUp = (int)_markUpSpin.Value.Value;
            }
            else if (_markUpSpin != null)
            {
                utm.MarkUp = 0;
            }
            if (_markDownSpin != null && _markDownSpin.Value.HasValue)
            {
                utm.MarkDown = (int)_markDownSpin.Value.Value;
            }
            else if (_markDownSpin != null)
            {
                utm.MarkDown = 0;
            }
            // Python always reads from UI, even if empty (creates blank ResRef)
            utm.OnOpenScript = _onOpenEdit != null ? ResRefFromText(_onOpenEdit.Text) : utm.OnOpenScript;

            // Python always reads from UI without null checks - currentIndex() returns -1 if nothing selected
            // In Avalonia, SelectedIndex can be 0 (first item), so we need to check for >= 0, not > 0
            int index = -1;
            if (_storeFlagSelect != null)
            {
                // SelectedIndex can be 0 (first item), so we check >= 0
                index = _storeFlagSelect.SelectedIndex >= 0 ? _storeFlagSelect.SelectedIndex : -1;
            }
            int flagValue = index + 1; // -1 + 1 = 0, 0 + 1 = 1, 1 + 1 = 2, 2 + 1 = 3
            utm.CanBuy = (flagValue & 1) != 0;
            utm.CanSell = (flagValue & 2) != 0;

            // Comments
            utm.Comment = _commentsEdit?.Text ?? utm.Comment ?? "";

            // Build GFF
            var game = _installation?.Game ?? Game.K2;
            var gff = UTMHelpers.DismantleUtm(utm, game);
            ResourceType outputType = _restype == ResourceType.UTM_XML || _restype == ResourceType.UTM_JSON
                ? _restype
                : (_restype == ResourceType.BTM ? ResourceType.BTM : ResourceType.UTM);
            if (outputType == ResourceType.BTM)
            {
                gff.Content = GFFContent.BTM;
            }
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        private static ResRef ResRefFromText(string text)
        {
            string value = (text ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(value) ? new ResRef(value) : new ResRef("");
        }

        private UTM CopyUTM(UTM source)
        {
            // Deep copy LocalizedString objects (they're reference types)
            LocalizedString copyName = source.Name != null
                ? new LocalizedString(source.Name.StringRef, new Dictionary<int, string>(GetSubstringsDict(source.Name)))
                : null;

            var copy = new UTM
            {
                ResRef = source.ResRef,
                Name = copyName,
                Tag = source.Tag,
                MarkUp = source.MarkUp,
                MarkDown = source.MarkDown,
                OnOpenScript = source.OnOpenScript,
                Comment = source.Comment,
                Id = source.Id,
                CanBuy = source.CanBuy,
                CanSell = source.CanSell
            };

            // Copy items
            foreach (var item in source.Items)
            {
                copy.Items.Add(new UTMItem
                {
                    ResRef = item.ResRef,
                    Infinite = item.Infinite,
                    Droppable = item.Droppable
                });
            }

            return copy;
        }

        // Helper to extract substrings dictionary from LocalizedString for copying
        private Dictionary<int, string> GetSubstringsDict(LocalizedString locString)
        {
            var dict = new Dictionary<int, string>();
            if (locString != null)
            {
                foreach ((Language lang, Gender gender, string text) in locString)
                {
                    int substringId = LocalizedString.SubstringId(lang, gender);
                    dict[substringId] = text;
                }
            }
            return dict;
        }

        public override void New()
        {
            base.New();
            _utm = new UTM();
            LoadUTM(_utm);
        }

        private void ChangeName()
        {
            if (_installation == null) return;
            var dialog = new LocalizedStringDialog(this, _installation, _utm.Name);
            if (dialog.ShowDialog())
            {
                _utm.Name = dialog.LocString;
                if (_nameEdit != null)
                {
                    _nameEdit.SetLocString(_utm.Name);
                }
                MarkDocumentDirty();
            }
        }

        private void GenerateTag()
        {
            if (string.IsNullOrEmpty(_resrefEdit?.Text))
            {
                GenerateResref();
            }
            if (_tagEdit != null && _resrefEdit != null)
            {
                _tagEdit.Text = _resrefEdit.Text;
            }
            MarkDocumentDirty();
        }

        private void GenerateResref()
        {
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_mer_000";
            }
            MarkDocumentDirty();
        }

        private void OpenInventory()
        {
            if (_utm == null) return;

            var inventoryEditor = CreateInventoryDialog(BuildInventoryCapsules());

            if (inventoryEditor.ShowDialog())
            {
                ApplyInventoryResult(inventoryEditor.Inventory);
            }
        }

        private List<Capsule> BuildInventoryCapsules()
        {
            var capsules = new List<Capsule>();
            if (_installation == null)
            {
                return capsules;
            }

            try
            {
                string root = null;
                if (!string.IsNullOrEmpty(base._filepath))
                {
                    root = Module.FilepathToRoot(base._filepath);
                }

                if (root != null)
                {
                    string caseRoot = root.ToLowerInvariant();
                    var moduleNames = _installation.ModuleNames();
                    string filepathStr = base._filepath ?? "";
                    string filepathFilename = !string.IsNullOrEmpty(filepathStr) ? System.IO.Path.GetFileName(filepathStr) : "";

                    foreach (var kvp in moduleNames)
                    {
                        // kvp.Key is the module filename (e.g., "danm13.rim"), kvp.Value is the area name
                        string moduleFilename = kvp.Key;
                        string moduleFilenameLower = moduleFilename.ToLowerInvariant();

                        // Check if root is contained in module filename and it's not the same as the current filepath
                        if (moduleFilenameLower.Contains(caseRoot) && moduleFilename != filepathFilename)
                        {
                            string fullModulePath = System.IO.Path.Combine(_installation.ModulePath(), moduleFilename);
                            if (File.Exists(fullModulePath))
                            {
                                try
                                {
                                    var capsule = new Capsule(fullModulePath, createIfNotExist: false);
                                    capsules.Add(capsule);
                                }
                                catch
                                {
                                    // Skip invalid capsule files
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception suppressed: {ex.Message}");
            }

            return capsules;
        }

        private InventoryDialog CreateInventoryDialog(List<Capsule> capsules)
        {
            // Convert UTMItem list to InventoryItem list for the dialog
            var inventoryItems = new List<InventoryItem>();
            foreach (var utmItem in _utm.Items)
            {
                inventoryItems.Add(new InventoryItem(
                    utmItem.ResRef,
                    droppable: utmItem.Droppable != 0,
                    infinite: utmItem.Infinite != 0));
            }

            return new InventoryDialog(
                this,
                _installation,
                capsules ?? new List<Capsule>(),
                new List<string>(), // folders parameter
                inventoryItems,
                new Dictionary<EquipmentSlot, InventoryItem>(), // equipment parameter
                droid: false,
                hideEquipment: true,
                isStore: true
            );
        }

        internal InventoryDialog CreateInventoryDialogForTest()
        {
            return CreateInventoryDialog(new List<Capsule>());
        }

        internal bool CanOpenInventoryWithoutInstallationForTest()
        {
            return _installation == null && _utm != null && BuildInventoryCapsules().Count == 0;
        }

        internal void ApplyInventoryResult(List<InventoryItem> inventory)
        {
            if (_utm == null)
            {
                return;
            }

            _utm.Items.Clear();
            if (inventory != null)
            {
                foreach (var invItem in inventory)
                {
                    _utm.Items.Add(new UTMItem
                    {
                        ResRef = invItem.ResRef,
                        Infinite = invItem.Infinite ? 1 : 0,
                        Droppable = invItem.Droppable ? 1 : 0
                    });
                }
            }

            MarkDocumentDirty();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            await base.RunSaveAsAsync();
        }
    }
}
