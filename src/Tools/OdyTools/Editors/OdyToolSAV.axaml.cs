using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using Avalonia.VisualTree;
using BioWare.Common;
using BioWare.Extract.Capsule;
using BioWare.Extract.SaveData;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using UTIHelpers = BioWare.Resource.Formats.GFF.Generics.UTI.UTIHelpers;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.TPC;
using BioWare.Resource.Formats.TwoDA;
using Gender = BioWare.Common.Gender;
using JetBrains.Annotations;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using MsBox.Avalonia;
using IconType = MsBox.Avalonia.Enums.Icon;
using MsBox.Avalonia.Enums;
using ResourceAutoHelpers = BioWare.Resource.ResourceAutoHelpers;
using UTCHelpers = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers;

namespace OdyTools.Editors
{
    public partial class OdyToolSAV : Editor
    {
        private const int MinEditorWidth = 900;
        private const int MinEditorHeight = 620;
        private static readonly string[] SkillNames = {
            "Computer Use", "Demolitions", "Stealth", "Awareness",
            "Persuade", "Repair", "Security", "Treat Injury"
        };

        private SaveFolderEntry _saveFolder;
        private SaveInfo _saveInfo;
        private PartyTable _partyTable;
        private GlobalVars _globalVars;
        private SaveNestedCapsule _nestedCapsule;
        private UTC _currentCharacter;
        private Dictionary<ResourceIdentifier, UTC> _parsedCharacters = new Dictionary<ResourceIdentifier, UTC>();
        private List<SaveInventoryItem> _inventoryItems = new List<SaveInventoryItem>();

        // UI Controls - Save Info
        private TextBox _lineEditSaveName;
        private TextBox _lineEditAreaName;
        private TextBox _lineEditLastModule;
        private NumericUpDown _spinBoxTimePlayed;
        private TextBox _lineEditPCName;
        private TextBox _lineEditPortrait0;
        private TextBox _lineEditPortrait1;
        private TextBox _lineEditPortrait2;
        private CheckBox _checkBoxCheatUsed;
        private NumericUpDown _spinBoxGameplayHint;
        private NumericUpDown _spinBoxStoryHint;
        private TextBox _lineEditLive1;
        private TextBox _lineEditLive2;
        private TextBox _lineEditLive3;
        private TextBox _lineEditLive4;
        private TextBox _lineEditLive5;
        private TextBox _lineEditLive6;
        private NumericUpDown _spinBoxLiveContent;
        private TextBox _lineEditTimestamp;
        private Image _screenshotPreview;
        private TextBlock _screenshotPlaceholder;

        // UI Controls - Party Table
        private NumericUpDown _spinBoxGold;
        private NumericUpDown _spinBoxXPPool;
        private NumericUpDown _spinBoxComponents;
        private NumericUpDown _spinBoxChemicals;
        private NumericUpDown _spinBoxTimePlayedPT;
        private CheckBox _checkBoxCheatUsedPT;
        private NumericUpDown _spinBoxControlledNPC;
        private NumericUpDown _spinBoxAIState;
        private NumericUpDown _spinBoxFollowState;
        private CheckBox _checkBoxSoloMode;
        private NumericUpDown _spinBoxLastGUIPanel;
        private NumericUpDown _spinBoxJournalSortOrder;
        private ListBox _listWidgetPartyMembers;
        private DataGrid _gridAvailableNPCs;
        private DataGrid _gridInfluence;

        // UI Controls - Global Vars
        private DataGrid _gridBooleans;
        private DataGrid _gridNumbers;
        private DataGrid _gridStrings;
        private DataGrid _gridLocations;

        // UI Controls - Characters
        private ListBox _listWidgetCharacters;
        private TextBox _lineEditCharName;
        private TextBox _lineEditCharTag;
        private TextBox _lineEditCharResRef;
        private NumericUpDown _spinBoxCharHP;
        private NumericUpDown _spinBoxCharMaxHP;
        private NumericUpDown _spinBoxCharFP;
        private NumericUpDown _spinBoxCharMaxFP;
        private NumericUpDown _spinBoxCharXP;
        private CheckBox _checkBoxCharMin1HP;
        private NumericUpDown _spinBoxCharGoodEvil;
        private NumericUpDown _spinBoxCharSTR;
        private NumericUpDown _spinBoxCharDEX;
        private NumericUpDown _spinBoxCharCON;
        private NumericUpDown _spinBoxCharINT;
        private NumericUpDown _spinBoxCharWIS;
        private NumericUpDown _spinBoxCharCHA;
        private NumericUpDown _spinBoxCharPortraitId;
        private NumericUpDown _spinBoxCharAppearanceType;
        private ComboBox _comboCharGender;
        private NumericUpDown _spinBoxCharSoundset;
        private DataGrid _gridSkills;
        private DataGrid _gridCharClasses;
        private ListBox _listWidgetCharFeats;
        private ComboBox _comboAddFeat;
        private ComboBox _comboAddClass;
        private ListBox _listWidgetCharPowers;
        private ComboBox _comboAddPower;
        private ListBox _listWidgetEquipment;

        // UI Controls - Inventory
        private DataGrid _gridInventory;

        // UI Controls - Journal
        private DataGrid _gridJournal;
        private ComboBox _comboAddJournalPlot;

        // UI Controls - Cached Modules
        private TreeView _treeCachedModules;

        // UI Controls - Reputation
        private DataGrid _gridReputation;

        // UI Controls - Area/Doors
        private DataGrid _gridDoors;
        private GFF _doorsGff;
        private BioWare.Resource.Formats.ERF.ERF _doorsModuleErf;
        private string _doorsGitResName;
        private bool _suppressSaveInfoDirty;
        private bool _suppressPartyTableDirty;

        // UI Controls - Advanced
        private ListBox _listAdvancedResources;

        private TabControl _tabControl;

        private class SaveInventoryItem
        {
            public string ResRef { get; set; }
            public int StackSize { get; set; }
            public int Charges { get; set; }
            public int MaxCharges { get; set; }
            public int UpgradeLevel { get; set; }
            /// <summary>New item flag (bit 7) - shown as "new" in inventory UI.</summary>
            public bool NewItem { get; set; }
            /// <summary>K1 upgrade bitfield (Upgrades Dword).</summary>
            public int Upgrades { get; set; }
            /// <summary>K2 weapon/armor upgrade slot references (UpgradeSlot0-5). -1 = no upgrade.</summary>
            public int UpgradeSlot0 { get; set; } = -1;
            public int UpgradeSlot1 { get; set; } = -1;
            public int UpgradeSlot2 { get; set; } = -1;
            public int UpgradeSlot3 { get; set; } = -1;
            public int UpgradeSlot4 { get; set; } = -1;
            public int UpgradeSlot5 { get; set; } = -1;
        }

        private class InventoryGridRow
        {
            public string Name { get; set; }
            public string ResRef { get; set; }
            public int StackSize { get; set; }
            public string ChargesStr { get; set; }
            public int UpgradeLevel { get; set; }
            public bool NewItem { get; set; }
            public int Upgrades { get; set; }
            public int UpgradeSlot0 { get; set; } = -1;
            public int UpgradeSlot1 { get; set; } = -1;
            public int UpgradeSlot2 { get; set; } = -1;
            public int UpgradeSlot3 { get; set; } = -1;
            public int UpgradeSlot4 { get; set; } = -1;
            public int UpgradeSlot5 { get; set; } = -1;
        }

        private class EquipmentDisplayItem
        {
            public EquipmentSlot Slot { get; set; }
            public InventoryItem Item { get; set; }
            public string Display => $"{Slot}: {(Item?.ResRef?.ToString() ?? "")}";
        }

        private class PartyMemberDisplayItem
        {
            public string Display { get; set; }
            public string ToolTip { get; set; }
            public PartyMemberEntry Member { get; set; }
        }

        private class ClassGridRow
        {
            public string Name { get; set; }
            public int ClassId { get; set; }
            public string Level { get; set; }
            public override string ToString() => Name ?? $"Class {ClassId}";
        }

        private class FeatDisplayItem
        {
            public int FeatId { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display ?? $"Feat {FeatId}";
        }

        private class PowerDisplayItem
        {
            public int PowerId { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display ?? $"Power {PowerId}";
        }

        private class DoorGridRow
        {
            public GFFStruct DoorStruct { get; set; }
            public string Tag { get; set; }
            public bool Locked { get; set; }
            public string OpenStateStr { get; set; }
        }

        private class GlobalBoolRow { public string Name { get; set; } public bool Value { get; set; } }
        private class GlobalNumberRow { public string Name { get; set; } public int Value { get; set; } }
        private class GlobalStringRow { public string Name { get; set; } public string Value { get; set; } }
        private class GlobalLocationRow { public string Name { get; set; } public float X { get; set; } public float Y { get; set; } public float Z { get; set; } public float Orientation { get; set; } }

        public OdyToolSAV() : this(null, null) { }
        public OdyToolSAV(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolSAV", "savegame",
                new[] { ResourceType.SAV },
                new[] { ResourceType.SAV },
                installation)
        {
            InitializeComponent();
            Width = 1200;
            Height = 800;
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            New();
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                SetContentOrInject(new Grid());
            }
            SetupUI();
            SetupMenuHandlers();
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyModifiers & KeyModifiers.Control) != KeyModifiers.Control) return;
            if (e.Key == Key.S) { e.Handled = true; Save(); }
            else if (e.Key == Key.R) { e.Handled = true; Revert(); }
            else if (e.Key == Key.E) { e.Handled = true; CloseSave(); }
        }

        private void SetupMenuHandlers()
        {
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("Save", Save),
                ("Revert", Revert),
                ("Close", CloseSave),
                ("Close Game", CloseSave),
            });
        }

        /// <summary>Unload the current save game (matches vendor Close Game).</summary>
        public void CloseSave()
        {
            if (IsDirty)
            {
                _ = RunCloseSaveAsync();
            }
            else
            {
                DoCloseSave();
            }
        }

        private async Task RunCloseSaveAsync()
        {
            var result = await DialogHelper.ShowWindowAsync(this, Localization.Tr("Unsaved Changes"), Localization.Tr("You have unsaved changes. Save before closing?"), ButtonEnum.YesNoCancel, IconType.Warning);
            if (result == ButtonResult.Yes) Save();
            if (result != ButtonResult.Cancel) DoCloseSave();
        }

        private void DoCloseSave()
        {
            _saveFolder = null;
            _saveInfo = null;
            _partyTable = null;
            _globalVars = null;
            _nestedCapsule = null;
            _currentCharacter = null;
            _parsedCharacters.Clear();
            _filepath = null;
            _resname = null;
            _restype = null;
            ClearDirty();
            ClearSaveInfo();
            ClearPartyTable();
            ClearGlobalVars();
            ClearCharacters();
            ClearInventory();
            ClearJournal();
            ClearCachedModules();
            ClearDoors();
            ClearReputation();
            ClearAdvancedFields();
            RefreshWindowTitle();
        }

        /// <summary>Revert unsaved changes by reloading the save from disk.</summary>
        public override void Revert()
        {
            if (_saveFolder == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("Load a save game first."), IconType.Warning);
                return;
            }
            try
            {
                LoadSaveGame(_saveFolder.FolderPath);
                ClearDirty();
                DialogHelper.ShowWindow(this, Localization.Tr("Reverted"), Localization.Tr("Save game reverted to last saved state."), IconType.Success);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowErrorFromException(this, ex);
            }
        }

        /// <summary>Rebuild cached modules by reloading the save from disk.</summary>
        public void RebuildCachedModules()
        {
            if (_saveFolder == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("Load a save game first."), IconType.Warning);
                return;
            }
            try
            {
                LoadSaveGame(_saveFolder.FolderPath);
                DialogHelper.ShowWindow(this, Localization.Tr("Rebuilt"), Localization.Tr("Cached modules have been reloaded from disk."), IconType.Success);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowErrorFromException(this, ex);
            }
        }

        /// <summary>Flush EventQueue corruption in cached modules (matches vendor fix savegame corruption).</summary>
        public void FlushEventQueue()
        {
            if (_saveFolder == null || _installation == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("Load a save game first."), IconType.Warning);
                return;
            }
            if (!_installation.IsSaveCorrupted(_saveFolder.FolderPath))
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Corruption"), Localization.Tr("This save does not appear to be corrupted."), IconType.Info);
                return;
            }
            if (_installation.FixSaveCorruption(_saveFolder.FolderPath))
            {
                DialogHelper.ShowWindow(this, Localization.Tr("Fixed"), Localization.Tr("EventQueue corruption was fixed. Reload the save to see changes."), IconType.Success);
                LoadSaveGame(_saveFolder.FolderPath);
            }
            else
            {
                DialogHelper.ShowWindow(this, Localization.Tr("Fix Failed"), Localization.Tr("Could not fix savegame corruption."), IconType.Error);
            }
        }

        private static MenuItem FindMenuItem(Visual parent, string header)
        {
            foreach (var c in parent.GetVisualDescendants())
            {
                if (c is MenuItem mi && mi.Header?.ToString()?.Replace("_", "")?.Equals(header, StringComparison.OrdinalIgnoreCase) == true)
                    return mi;
            }
            return null;
        }

        private void SetupUI()
        {
            _tabControl = new TabControl();
            _tabControl.Items.Add(CreateSaveInfoTab());
            _tabControl.Items.Add(CreatePartyTableTab());
            _tabControl.Items.Add(CreateGlobalVarsTab());
            _tabControl.Items.Add(CreateCharactersTab());
            _tabControl.Items.Add(CreateInventoryTab());
            _tabControl.Items.Add(CreateJournalTab());
            _tabControl.Items.Add(CreateCachedModulesTab());
            _tabControl.Items.Add(CreateAreaDoorsTab());
            _tabControl.Items.Add(CreateReputationTab());
            _tabControl.Items.Add(CreateAdvancedTab());
            WireSaveInfoChangeHandlers();
            WirePartyTableChangeHandlers();

            var scroll = new ScrollViewer { Content = _tabControl, HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            SetContentOrInject(scroll);
        }

        private void WireSaveInfoChangeHandlers()
        {
            TextBox[] textBoxes =
            {
                _lineEditSaveName,
                _lineEditAreaName,
                _lineEditLastModule,
                _lineEditPCName,
                _lineEditPortrait0,
                _lineEditPortrait1,
                _lineEditPortrait2,
                _lineEditLive1,
                _lineEditLive2,
                _lineEditLive3,
                _lineEditLive4,
                _lineEditLive5,
                _lineEditLive6
            };
            foreach (var box in textBoxes)
            {
                if (box != null)
                {
                    box.TextChanged += (s, e) => OnSaveInfoControlChanged();
                }
            }

            NumericUpDown[] spins =
            {
                _spinBoxTimePlayed,
                _spinBoxGameplayHint,
                _spinBoxStoryHint,
                _spinBoxLiveContent
            };
            foreach (var spin in spins)
            {
                if (spin != null)
                {
                    spin.ValueChanged += (s, e) => OnSaveInfoControlChanged();
                }
            }

            if (_checkBoxCheatUsed != null)
            {
                _checkBoxCheatUsed.IsCheckedChanged += (s, e) => OnSaveInfoControlChanged();
            }
        }

        private void OnSaveInfoControlChanged()
        {
            if (_suppressSaveInfoDirty || _saveInfo == null)
            {
                return;
            }

            UpdateSaveInfoFromUI();
            MarkDocumentDirty();
        }

        private void WirePartyTableChangeHandlers()
        {
            NumericUpDown[] spins =
            {
                _spinBoxGold,
                _spinBoxXPPool,
                _spinBoxComponents,
                _spinBoxChemicals,
                _spinBoxTimePlayedPT,
                _spinBoxControlledNPC,
                _spinBoxAIState,
                _spinBoxFollowState,
                _spinBoxLastGUIPanel,
                _spinBoxJournalSortOrder
            };
            foreach (var spin in spins)
            {
                if (spin != null)
                {
                    spin.ValueChanged += (s, e) => OnPartyTableControlChanged();
                }
            }

            if (_checkBoxCheatUsedPT != null)
            {
                _checkBoxCheatUsedPT.IsCheckedChanged += (s, e) => OnPartyTableControlChanged();
            }

            if (_checkBoxSoloMode != null)
            {
                _checkBoxSoloMode.IsCheckedChanged += (s, e) => OnPartyTableControlChanged();
            }

            if (_gridAvailableNPCs != null)
            {
                _gridAvailableNPCs.CellEditEnded += (s, e) => OnPartyTableControlChanged();
            }

            if (_gridInfluence != null)
            {
                _gridInfluence.CellEditEnded += (s, e) => OnPartyTableControlChanged();
            }
        }

        private void OnPartyTableControlChanged()
        {
            if (_suppressPartyTableDirty || _partyTable == null)
            {
                return;
            }

            UpdatePartyTableFromUI();
            MarkDocumentDirty();
        }

        private TabItem CreateSaveInfoTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Save Info") };
            var scroll = new ScrollViewer { VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            var panel = new StackPanel { Spacing = 8, Margin = new Thickness(8) };

            var basicGroup = new Expander { Header = Localization.Tr("Basic"), IsExpanded = true };
            var basicPanel = new StackPanel { Spacing = 4 };
            basicPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Save Name"), out _lineEditSaveName));
            basicPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Area Name"), out _lineEditAreaName));
            basicPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Last Module"), out _lineEditLastModule));
            basicPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Time Played (seconds)"), out _spinBoxTimePlayed, 0, int.MaxValue));
            basicPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("PC Name"), out _lineEditPCName));
            basicGroup.Content = basicPanel;
            panel.Children.Add(basicGroup);

            var portraitsGroup = new Expander { Header = Localization.Tr("Portraits"), IsExpanded = true };
            var portraitsPanel = new StackPanel { Spacing = 4 };
            portraitsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Portrait 0"), out _lineEditPortrait0));
            portraitsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Portrait 1"), out _lineEditPortrait1));
            portraitsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Portrait 2"), out _lineEditPortrait2));
            portraitsGroup.Content = portraitsPanel;
            panel.Children.Add(portraitsGroup);

            var miscGroup = new Expander { Header = Localization.Tr("Misc"), IsExpanded = true };
            var miscPanel = new StackPanel { Spacing = 4 };
            _checkBoxCheatUsed = new CheckBox { Content = Localization.Tr("Cheat Used") };
            miscPanel.Children.Add(_checkBoxCheatUsed);
            miscPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Gameplay Hint"), out _spinBoxGameplayHint, 0, 255));
            miscPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Story Hint"), out _spinBoxStoryHint, 0, 255));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Timestamp"), out _lineEditTimestamp));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 1"), out _lineEditLive1));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 2"), out _lineEditLive2));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 3"), out _lineEditLive3));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 4"), out _lineEditLive4));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 5"), out _lineEditLive5));
            miscPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Live 6"), out _lineEditLive6));
            miscPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Live Content"), out _spinBoxLiveContent, 0, 255));
            miscGroup.Content = miscPanel;
            panel.Children.Add(miscGroup);

            var screenshotGroup = new Expander { Header = Localization.Tr("Screenshot"), IsExpanded = true };
            var screenshotPanel = new StackPanel { Spacing = 4 };
            _screenshotPlaceholder = new TextBlock { Text = Localization.Tr("No screenshot available"), Margin = new Thickness(4) };
            _screenshotPreview = new Image { MaxWidth = 320, MaxHeight = 240, Stretch = Stretch.Uniform };
            var screenshotBorder = new Border { Child = _screenshotPreview, BorderThickness = new Thickness(1), BorderBrush = Brushes.Gray, Padding = new Thickness(4) };
            screenshotPanel.Children.Add(_screenshotPlaceholder);
            screenshotPanel.Children.Add(screenshotBorder);
            screenshotGroup.Content = screenshotPanel;
            panel.Children.Add(screenshotGroup);

            var fixBtn = new Button { Content = Localization.Tr("Fix savegame corruption (EventQueue)"), Margin = new Thickness(0, 8, 0, 0) };
            fixBtn.Click += (s, e) => FlushEventQueue();
            panel.Children.Add(fixBtn);
            var rebuildBtn = new Button { Content = Localization.Tr("Rebuild Cached Modules"), Margin = new Thickness(0, 4, 0, 0) };
            rebuildBtn.Click += (s, e) => RebuildCachedModules();
            panel.Children.Add(rebuildBtn);
            var revertBtn = new Button { Content = Localization.Tr("Revert"), Margin = new Thickness(0, 4, 0, 0) };
            revertBtn.Click += (s, e) => Revert();
            panel.Children.Add(revertBtn);
            var closeSaveBtn = new Button { Content = Localization.Tr("Close Game"), Margin = new Thickness(0, 4, 0, 0) };
            closeSaveBtn.Click += (s, e) => CloseSave();
            panel.Children.Add(closeSaveBtn);

            scroll.Content = panel;
            tab.Content = scroll;
            return tab;
        }

        private TabItem CreatePartyTableTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Party Table") };
            var scroll = new ScrollViewer { VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            var panel = new StackPanel { Spacing = 8, Margin = new Thickness(8) };

            var resourcesGroup = new Expander { Header = Localization.Tr("Resources"), IsExpanded = true };
            var resourcesPanel = new StackPanel { Spacing = 4 };
            resourcesPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Gold"), out _spinBoxGold, 0, int.MaxValue));
            resourcesPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("XP Pool"), out _spinBoxXPPool, 0, int.MaxValue));
            resourcesPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Components"), out _spinBoxComponents, 0, int.MaxValue));
            resourcesPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Chemicals"), out _spinBoxChemicals, 0, int.MaxValue));
            resourcesPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Time Played"), out _spinBoxTimePlayedPT, -1, int.MaxValue));
            _checkBoxCheatUsedPT = new CheckBox { Content = Localization.Tr("Cheat Used") };
            resourcesPanel.Children.Add(_checkBoxCheatUsedPT);
            resourcesGroup.Content = resourcesPanel;
            panel.Children.Add(resourcesGroup);

            var stateGroup = new Expander { Header = Localization.Tr("Party State"), IsExpanded = true };
            var statePanel = new StackPanel { Spacing = 4 };
            statePanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Controlled NPC"), out _spinBoxControlledNPC, -1, 20));
            statePanel.Children.Add(CreateLabelAndSpin(Localization.Tr("AI State"), out _spinBoxAIState, 0, 255));
            statePanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Follow State"), out _spinBoxFollowState, 0, 255));
            _checkBoxSoloMode = new CheckBox { Content = Localization.Tr("Solo Mode") };
            statePanel.Children.Add(_checkBoxSoloMode);
            statePanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Last GUI Panel"), out _spinBoxLastGUIPanel, 0, int.MaxValue));
            statePanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Journal Sort Order"), out _spinBoxJournalSortOrder, 0, int.MaxValue));
            stateGroup.Content = statePanel;
            panel.Children.Add(stateGroup);

            panel.Children.Add(new TextBlock { Text = Localization.Tr("Party Members"), FontWeight = FontWeight.SemiBold });
            _listWidgetPartyMembers = new ListBox { MinHeight = 80 };
            _listWidgetPartyMembers.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<PartyMemberDisplayItem>((item, _) =>
            {
                var tb = new TextBlock { Text = item?.Display ?? "", VerticalAlignment = VerticalAlignment.Center };
                if (item != null && !string.IsNullOrEmpty(item.ToolTip))
                    ToolTip.SetTip(tb, item.ToolTip);
                return tb;
            });
            panel.Children.Add(_listWidgetPartyMembers);

            panel.Children.Add(new TextBlock { Text = Localization.Tr("Available NPCs"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _gridAvailableNPCs = new DataGrid { AutoGenerateColumns = false, MinHeight = 120, IsReadOnly = false };
            _gridAvailableNPCs.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Index"), Binding = new Avalonia.Data.Binding("Index"), IsReadOnly = true });
            _gridAvailableNPCs.Columns.Add(new DataGridCheckBoxColumn { Header = Localization.Tr("Available"), Binding = new Avalonia.Data.Binding("Available") });
            _gridAvailableNPCs.Columns.Add(new DataGridCheckBoxColumn { Header = Localization.Tr("Selected"), Binding = new Avalonia.Data.Binding("Selected") });
            panel.Children.Add(_gridAvailableNPCs);

            panel.Children.Add(new TextBlock { Text = Localization.Tr("Influence (K2)"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _gridInfluence = new DataGrid { AutoGenerateColumns = false, MinHeight = 120, IsReadOnly = false };
            _gridInfluence.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("NPC"), Binding = new Avalonia.Data.Binding("Index"), IsReadOnly = true });
            _gridInfluence.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Value"), Binding = new Avalonia.Data.Binding("Value") });
            panel.Children.Add(_gridInfluence);

            var miscGroup = new Expander { Header = Localization.Tr("Pazaak & Misc (read-only)"), IsExpanded = false };
            var miscPanel = new StackPanel { Spacing = 4 };
            _textPazaakCards = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            _textPazaakDecks = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            _textFeedbackMessages = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            _textDialogMessages = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            _textTutorialShown = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            _textCostMultipliers = new TextBlock { Text = "", TextWrapping = TextWrapping.Wrap };
            miscPanel.Children.Add(_textPazaakCards);
            miscPanel.Children.Add(_textPazaakDecks);
            miscPanel.Children.Add(_textFeedbackMessages);
            miscPanel.Children.Add(_textDialogMessages);
            miscPanel.Children.Add(_textTutorialShown);
            miscPanel.Children.Add(_textCostMultipliers);
            miscGroup.Content = miscPanel;
            panel.Children.Add(miscGroup);

            scroll.Content = panel;
            tab.Content = scroll;
            return tab;
        }

        private TextBlock _textPazaakCards;
        private TextBlock _textPazaakDecks;
        private TextBlock _textFeedbackMessages;
        private TextBlock _textDialogMessages;
        private TextBlock _textTutorialShown;
        private TextBlock _textCostMultipliers;

        private TabItem CreateGlobalVarsTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Global Variables") };
            var subTabs = new TabControl();
            subTabs.Items.Add(CreateGlobalBoolsTab());
            subTabs.Items.Add(CreateGlobalNumbersTab());
            subTabs.Items.Add(CreateGlobalStringsTab());
            subTabs.Items.Add(CreateGlobalLocationsTab());
            tab.Content = subTabs;
            return tab;
        }

        private TabItem CreateGlobalBoolsTab()
        {
            _gridBooleans = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridBooleans.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Name"), Binding = new Avalonia.Data.Binding("Name") });
            _gridBooleans.Columns.Add(new DataGridCheckBoxColumn { Header = Localization.Tr("Value"), Binding = new Avalonia.Data.Binding("Value") });
            return CreateGlobalVarTab(Localization.Tr("Booleans"), _gridBooleans, () => AddGlobalVarRow(_gridBooleans, "bools"));
        }

        private TabItem CreateGlobalNumbersTab()
        {
            _gridNumbers = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridNumbers.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Name"), Binding = new Avalonia.Data.Binding("Name") });
            _gridNumbers.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Value"), Binding = new Avalonia.Data.Binding("Value") });
            return CreateGlobalVarTab(Localization.Tr("Numbers"), _gridNumbers, () => AddGlobalVarRow(_gridNumbers, "numbers"));
        }

        private TabItem CreateGlobalStringsTab()
        {
            _gridStrings = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridStrings.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Name"), Binding = new Avalonia.Data.Binding("Name") });
            _gridStrings.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Value"), Binding = new Avalonia.Data.Binding("Value") });
            return CreateGlobalVarTab(Localization.Tr("Strings"), _gridStrings, () => AddGlobalVarRow(_gridStrings, "strings"));
        }

        private TabItem CreateGlobalLocationsTab()
        {
            _gridLocations = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridLocations.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Name"), Binding = new Avalonia.Data.Binding("Name") });
            _gridLocations.Columns.Add(new DataGridTextColumn { Header = "X", Binding = new Avalonia.Data.Binding("X") });
            _gridLocations.Columns.Add(new DataGridTextColumn { Header = "Y", Binding = new Avalonia.Data.Binding("Y") });
            _gridLocations.Columns.Add(new DataGridTextColumn { Header = "Z", Binding = new Avalonia.Data.Binding("Z") });
            _gridLocations.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Orientation"), Binding = new Avalonia.Data.Binding("Orientation") });
            return CreateGlobalVarTab(Localization.Tr("Locations"), _gridLocations, () => AddGlobalVarRow(_gridLocations, "locations"));
        }

        /// <summary>
        /// Builds a global-variable editor tab with common add/remove controls.
        /// </summary>
        private TabItem CreateGlobalVarTab(string header, DataGrid grid, Action onAdd)
        {
            grid.CellEditEnded += (s, e) => SyncGlobalVarsFromGridAndMarkDirty();
            var addBtn = CreateActionButton(Localization.Tr("Add"), () => onAdd?.Invoke());
            var removeBtn = CreateActionButton(Localization.Tr("Remove"), () => RemoveGlobalVarRow(grid));
            var btnPanel = CreateButtonRow(addBtn, removeBtn);
            var panel = new StackPanel();
            panel.Children.Add(grid);
            panel.Children.Add(btnPanel);
            return new TabItem { Header = header, Content = panel };
        }

        private Button CreateActionButton(string label, Action onClick)
        {
            var button = new Button { Content = label };
            button.Click += (s, e) => onClick?.Invoke();
            return button;
        }

        private StackPanel CreateButtonRow(params Control[] controls)
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 4, 0, 0) };
            if (controls != null)
            {
                foreach (Control control in controls)
                {
                    if (control != null)
                    {
                        panel.Children.Add(control);
                    }
                }
            }

            return panel;
        }

        private void AddGlobalVarRow(DataGrid grid, string kind)
        {
            if (_globalVars == null) return;
            if (grid == _gridBooleans)
            {
                var rows = (_gridBooleans.ItemsSource as IEnumerable<GlobalBoolRow>)?.ToList() ?? new List<GlobalBoolRow>();
                rows.Add(new GlobalBoolRow { Name = NextGlobalVarName(rows.Select(row => row.Name)), Value = false });
                _gridBooleans.ItemsSource = rows;
            }
            else if (grid == _gridNumbers)
            {
                var rows = (_gridNumbers.ItemsSource as IEnumerable<GlobalNumberRow>)?.ToList() ?? new List<GlobalNumberRow>();
                rows.Add(new GlobalNumberRow { Name = NextGlobalVarName(rows.Select(row => row.Name)), Value = 0 });
                _gridNumbers.ItemsSource = rows;
            }
            else if (grid == _gridStrings)
            {
                var rows = (_gridStrings.ItemsSource as IEnumerable<GlobalStringRow>)?.ToList() ?? new List<GlobalStringRow>();
                rows.Add(new GlobalStringRow { Name = NextGlobalVarName(rows.Select(row => row.Name)), Value = "" });
                _gridStrings.ItemsSource = rows;
            }
            else if (grid == _gridLocations)
            {
                var rows = (_gridLocations.ItemsSource as IEnumerable<GlobalLocationRow>)?.ToList() ?? new List<GlobalLocationRow>();
                rows.Add(new GlobalLocationRow { Name = NextGlobalVarName(rows.Select(row => row.Name)), X = 0, Y = 0, Z = 0, Orientation = 0 });
                _gridLocations.ItemsSource = rows;
            }
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        private void RemoveGlobalVarRow(DataGrid grid)
        {
            int idx = grid.SelectedIndex;
            if (idx < 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a row to remove."), IconType.Info);
                return;
            }
            if (grid == _gridBooleans)
            {
                var rows = (_gridBooleans.ItemsSource as IEnumerable<GlobalBoolRow>)?.ToList() ?? new List<GlobalBoolRow>();
                if (idx < rows.Count) { rows.RemoveAt(idx); _gridBooleans.ItemsSource = rows; }
            }
            else if (grid == _gridNumbers)
            {
                var rows = (_gridNumbers.ItemsSource as IEnumerable<GlobalNumberRow>)?.ToList() ?? new List<GlobalNumberRow>();
                if (idx < rows.Count) { rows.RemoveAt(idx); _gridNumbers.ItemsSource = rows; }
            }
            else if (grid == _gridStrings)
            {
                var rows = (_gridStrings.ItemsSource as IEnumerable<GlobalStringRow>)?.ToList() ?? new List<GlobalStringRow>();
                if (idx < rows.Count) { rows.RemoveAt(idx); _gridStrings.ItemsSource = rows; }
            }
            else if (grid == _gridLocations)
            {
                var rows = (_gridLocations.ItemsSource as IEnumerable<GlobalLocationRow>)?.ToList() ?? new List<GlobalLocationRow>();
                if (idx < rows.Count) { rows.RemoveAt(idx); _gridLocations.ItemsSource = rows; }
            }
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        private void SyncGlobalVarsFromGridAndMarkDirty()
        {
            UpdateGlobalVarsFromUI();
            MarkDocumentDirty();
        }

        private static string NextGlobalVarName(IEnumerable<string> existingNames)
        {
            var existing = new HashSet<string>(existingNames.Where(name => !string.IsNullOrWhiteSpace(name)), StringComparer.OrdinalIgnoreCase);
            const string baseName = "NEW_VAR";
            if (!existing.Contains(baseName))
            {
                return baseName;
            }

            for (int i = 1; i < 10000; i++)
            {
                string candidate = baseName + "_" + i;
                if (!existing.Contains(candidate))
                {
                    return candidate;
                }
            }

            return baseName + "_" + Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        private TabItem CreateCharactersTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Characters") };
            var split = new Grid();
            split.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            split.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            _listWidgetCharacters = new ListBox { MinWidth = 180 };
            _listWidgetCharacters.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<object>((item, _) =>
            {
                var display = item?.ToString() ?? "";
                if (item != null)
                {
                    try
                    {
                        var d = (dynamic)item;
                        if (d.Display != null)
                            display = d.Display.ToString();
                    }
                    catch { }
                }
                return new TextBlock { Text = display, VerticalAlignment = VerticalAlignment.Center };
            });
            _listWidgetCharacters.SelectionChanged += (s, e) => OnCharacterSelected();
            Grid.SetColumn(_listWidgetCharacters, 0);
            split.Children.Add(_listWidgetCharacters);

            var detailsScroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var detailsPanel = new StackPanel { Spacing = 6, Margin = new Thickness(8) };

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Stats"), FontWeight = FontWeight.SemiBold });
            detailsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Name"), out _lineEditCharName));
            detailsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("Tag"), out _lineEditCharTag));
            detailsPanel.Children.Add(CreateLabelAndEdit(Localization.Tr("ResRef"), out _lineEditCharResRef));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("HP"), out _spinBoxCharHP, 0, 9999));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Max HP"), out _spinBoxCharMaxHP, 0, 9999));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("FP"), out _spinBoxCharFP, 0, 9999));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Max FP"), out _spinBoxCharMaxFP, 0, 9999));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("XP"), out _spinBoxCharXP, 0, int.MaxValue));
            _checkBoxCharMin1HP = new CheckBox { Content = Localization.Tr("Min 1 HP") };
            detailsPanel.Children.Add(_checkBoxCharMin1HP);
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Alignment"), out _spinBoxCharGoodEvil, 0, 100));

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Attributes"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            foreach (var abbr in new[] { "STR", "DEX", "CON", "INT", "WIS", "CHA" })
            {
                NumericUpDown spin;
                detailsPanel.Children.Add(CreateLabelAndSpin(abbr, out spin, 0, 30));
                if (abbr == "STR") _spinBoxCharSTR = spin;
                else if (abbr == "DEX") _spinBoxCharDEX = spin;
                else if (abbr == "CON") _spinBoxCharCON = spin;
                else if (abbr == "INT") _spinBoxCharINT = spin;
                else if (abbr == "WIS") _spinBoxCharWIS = spin;
                else _spinBoxCharCHA = spin;
            }
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Portrait ID"), out _spinBoxCharPortraitId, 0, 65535));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Appearance"), out _spinBoxCharAppearanceType, 0, 65535));
            detailsPanel.Children.Add(CreateLabelAndSpin(Localization.Tr("Soundset"), out _spinBoxCharSoundset, 0, 65535));
            _comboCharGender = new ComboBox { MinWidth = 120 };
            foreach (var g in new[] { "None", "Male", "Female", "Both", "Other" }) _comboCharGender.Items.Add(g);
            _comboCharGender.SelectedIndex = 0;
            var genderPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            genderPanel.Children.Add(new TextBlock { Text = Localization.Tr("Gender"), VerticalAlignment = VerticalAlignment.Center });
            genderPanel.Children.Add(_comboCharGender);
            detailsPanel.Children.Add(genderPanel);

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Skills"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _gridSkills = new DataGrid { AutoGenerateColumns = false, MinHeight = 150, IsReadOnly = false };
            _gridSkills.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Skill"), Binding = new Avalonia.Data.Binding("Name"), IsReadOnly = true });
            _gridSkills.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Rank"), Binding = new Avalonia.Data.Binding("Rank") });
            detailsPanel.Children.Add(_gridSkills);

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Classes"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _gridCharClasses = new DataGrid { AutoGenerateColumns = false, MinHeight = 60, IsReadOnly = false };
            _gridCharClasses.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Class"), Binding = new Avalonia.Data.Binding("Name"), IsReadOnly = true });
            _gridCharClasses.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Level"), Binding = new Avalonia.Data.Binding("Level") });
            _comboAddClass = new ComboBox { MinWidth = 160, PlaceholderText = Localization.Tr("Select class to add") };
            var addClassBtn = CreateActionButton(Localization.Tr("Add Class"), AddCharacterClass);
            var removeClassBtn = CreateActionButton(Localization.Tr("Remove Class"), RemoveCharacterClass);
            var classBtnPanel = CreateButtonRow(_comboAddClass, addClassBtn, removeClassBtn);
            detailsPanel.Children.Add(_gridCharClasses);
            detailsPanel.Children.Add(classBtnPanel);

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Feats"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _listWidgetCharFeats = new ListBox { MinHeight = 80 };
            _listWidgetCharFeats.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<FeatDisplayItem>((item, _) =>
                new TextBlock { Text = item?.Display ?? "", VerticalAlignment = VerticalAlignment.Center });
            _comboAddFeat = new ComboBox { MinWidth = 180, PlaceholderText = Localization.Tr("Select feat to add") };
            var addFeatBtn = CreateActionButton(Localization.Tr("Add Feat"), AddCharacterFeat);
            var removeFeatBtn = CreateActionButton(Localization.Tr("Remove Feat"), RemoveCharacterFeat);
            var featBtnPanel = CreateButtonRow(_comboAddFeat, addFeatBtn, removeFeatBtn);
            detailsPanel.Children.Add(_listWidgetCharFeats);
            detailsPanel.Children.Add(featBtnPanel);

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Force Powers"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _listWidgetCharPowers = new ListBox { MinHeight = 80 };
            _listWidgetCharPowers.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<PowerDisplayItem>((item, _) =>
                new TextBlock { Text = item?.Display ?? "", VerticalAlignment = VerticalAlignment.Center });
            _comboAddPower = new ComboBox { MinWidth = 180, PlaceholderText = Localization.Tr("Select power to add") };
            var addPowerBtn = CreateActionButton(Localization.Tr("Add Power"), AddCharacterPower);
            var removePowerBtn = CreateActionButton(Localization.Tr("Remove Power"), RemoveCharacterPower);
            var powerBtnPanel = CreateButtonRow(_comboAddPower, addPowerBtn, removePowerBtn);
            detailsPanel.Children.Add(_listWidgetCharPowers);
            detailsPanel.Children.Add(powerBtnPanel);

            detailsPanel.Children.Add(new TextBlock { Text = Localization.Tr("Equipment"), FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 8, 0, 0) });
            _listWidgetEquipment = new ListBox { MinHeight = 120 };
            _listWidgetEquipment.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<object>((item, _) =>
                new TextBlock { Text = (item as EquipmentDisplayItem)?.Display ?? "", VerticalAlignment = VerticalAlignment.Center });
            _listWidgetEquipment.DoubleTapped += (s, e) => OnEquipmentDoubleTapped();
            var equipContextMenu = new ContextMenu();
            var editEquipItem = new MenuItem { Header = Localization.Tr("Edit Item...") };
            editEquipItem.Click += (s, e) => OpenCharacterInventoryDialog();
            var removeEquipItem = new MenuItem { Header = Localization.Tr("Remove Item") };
            removeEquipItem.Click += (s, e) => OnEquipmentRemoveSelected();
            equipContextMenu.Items.Add(editEquipItem);
            equipContextMenu.Items.Add(removeEquipItem);
            _listWidgetEquipment.ContextMenu = equipContextMenu;
            var editEquipBtn = CreateActionButton(Localization.Tr("Edit Inventory / Equipment"), OpenCharacterInventoryDialog);
            detailsPanel.Children.Add(_listWidgetEquipment);
            detailsPanel.Children.Add(editEquipBtn);

            detailsScroll.Content = detailsPanel;
            Grid.SetColumn(detailsScroll, 1);
            split.Children.Add(detailsScroll);
            tab.Content = split;
            return tab;
        }

        private TabItem CreateInventoryTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Inventory") };
            _gridInventory = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridInventory.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Name"), Binding = new Avalonia.Data.Binding("Name"), IsReadOnly = true });
            _gridInventory.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("ResRef"), Binding = new Avalonia.Data.Binding("ResRef") });
            _gridInventory.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Stack"), Binding = new Avalonia.Data.Binding("StackSize") });
            _gridInventory.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Charges"), Binding = new Avalonia.Data.Binding("ChargesStr") });
            _gridInventory.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Upgrades"), Binding = new Avalonia.Data.Binding("UpgradeLevel") });
            _gridInventory.Columns.Add(new DataGridCheckBoxColumn { Header = Localization.Tr("New"), Binding = new Avalonia.Data.Binding("NewItem") });
            _gridInventory.CellEditEnded += (s, e) => MarkDocumentDirty();
            var addInvBtn = CreateActionButton(Localization.Tr("Add Item"), AddInventoryItem);
            var addFromTemplateBtn = new Button { Content = Localization.Tr("Add from template...") };
            addFromTemplateBtn.Click += async (s, e) => await AddInventoryItemFromTemplateAsync();
            var removeInvBtn = CreateActionButton(Localization.Tr("Remove Selected"), RemoveInventoryItem);
            var invPanel = new StackPanel();
            invPanel.Children.Add(_gridInventory);
            var btnPanel = CreateButtonRow(addInvBtn, addFromTemplateBtn, removeInvBtn);
            invPanel.Children.Add(btnPanel);
            tab.Content = invPanel;
            return tab;
        }

        private TabItem CreateJournalTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Journal") };
            _gridJournal = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridJournal.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Plot ID"), Binding = new Avalonia.Data.Binding("PlotIdRaw") });
            _gridJournal.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("State"), Binding = new Avalonia.Data.Binding("StateStr") });
            _gridJournal.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Date"), Binding = new Avalonia.Data.Binding("DateStr") });
            _gridJournal.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Time"), Binding = new Avalonia.Data.Binding("TimeStr") });
            _comboAddJournalPlot = new ComboBox { MinWidth = 200, PlaceholderText = Localization.Tr("Select plot to add") };
            var addJournalBtn = CreateActionButton(Localization.Tr("Add Journal Entry"), AddJournalEntry);
            var removeJournalBtn = CreateActionButton(Localization.Tr("Remove Selected"), RemoveJournalEntry);
            var journalBtnPanel = CreateButtonRow(_comboAddJournalPlot, addJournalBtn, removeJournalBtn);
            var journalPanel = new StackPanel { Spacing = 8, Margin = new Thickness(8) };
            journalPanel.Children.Add(_gridJournal);
            journalPanel.Children.Add(journalBtnPanel);
            tab.Content = journalPanel;
            return tab;
        }

        private TabItem CreateCachedModulesTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Cached Modules") };
            var openBtn = new Button { Content = Localization.Tr("Open Selected Resource") };
            openBtn.Click += (s, e) => OnCachedModuleDoubleTapped();
            _treeCachedModules = new TreeView { MinHeight = 200 };
            _treeCachedModules.DoubleTapped += (s, e) => OnCachedModuleDoubleTapped();
            var panel = new StackPanel { Spacing = 8 };
            panel.Children.Add(openBtn);
            panel.Children.Add(_treeCachedModules);
            tab.Content = panel;
            return tab;
        }

        private TabItem CreateAreaDoorsTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Area / Doors") };
            var label = new TextBlock
            {
                Text = Localization.Tr("Door states in the current area (from GIT in cached module). Locked and Open State (0=closed, 1=open1, 2=open2) can be edited."),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            };
            _gridDoors = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridDoors.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Tag"), Binding = new Avalonia.Data.Binding("Tag"), IsReadOnly = true });
            _gridDoors.Columns.Add(new DataGridCheckBoxColumn { Header = Localization.Tr("Locked"), Binding = new Avalonia.Data.Binding("Locked") });
            _gridDoors.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Open State (0–2)"), Binding = new Avalonia.Data.Binding("OpenStateStr") });
            _gridDoors.CellEditEnded += (s, e) => MarkDocumentDirty();
            var panel = new StackPanel { Spacing = 8, Margin = new Thickness(8) };
            panel.Children.Add(label);
            panel.Children.Add(_gridDoors);
            tab.Content = panel;
            return tab;
        }

        private TabItem CreateReputationTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Reputation") };
            _gridReputation = new DataGrid { AutoGenerateColumns = false, IsReadOnly = false };
            _gridReputation.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Faction"), Binding = new Avalonia.Data.Binding("Name"), IsReadOnly = true });
            _gridReputation.Columns.Add(new DataGridTextColumn { Header = Localization.Tr("Value"), Binding = new Avalonia.Data.Binding("Value") });
            tab.Content = _gridReputation;
            return tab;
        }

        private TabItem CreateAdvancedTab()
        {
            var tab = new TabItem { Header = Localization.Tr("Advanced / Raw") };
            var label = new TextBlock { Text = Localization.Tr("Other resources (double-click to open):"), Margin = new Thickness(0, 0, 0, 8) };
            _listAdvancedResources = new ListBox { MinHeight = 200 };
            _listAdvancedResources.DoubleTapped += (s, e) => OnAdvancedResourceDoubleTapped();
            _listAdvancedResources.ItemTemplate = new Avalonia.Controls.Templates.FuncDataTemplate<AdvancedResourceItem>((item, _) =>
                new TextBlock { Text = item?.Display ?? "", VerticalAlignment = VerticalAlignment.Center });
            var panel = new StackPanel { Spacing = 8, Margin = new Thickness(8) };
            panel.Children.Add(label);
            panel.Children.Add(_listAdvancedResources);
            tab.Content = panel;
            return tab;
        }

        private class AdvancedResourceItem
        {
            public string Display { get; set; }
            public ResourceIdentifier Ident { get; set; }
            public byte[] Data { get; set; }
        }

        private static StackPanel CreateLabelAndEdit(string label, out TextBox edit)
        {
            edit = new TextBox { MinWidth = 200 };
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = label + ":", MinWidth = 120, VerticalAlignment = VerticalAlignment.Center });
            panel.Children.Add(edit);
            return panel;
        }

        private static StackPanel CreateLabelAndSpin(string label, out NumericUpDown spin, decimal min, decimal max)
        {
            spin = new NumericUpDown { Minimum = min, Maximum = max, MinWidth = 100 };
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = label + ":", MinWidth = 120, VerticalAlignment = VerticalAlignment.Center });
            panel.Children.Add(spin);
            return panel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            bool isPathToFolder = !string.IsNullOrEmpty(filepath) && Directory.Exists(filepath);
            bool isPathToSaveFile = !string.IsNullOrEmpty(filepath) && File.Exists(filepath) &&
                string.Equals(Path.GetFileName(filepath), "SAVEGAME.SAV", StringComparison.OrdinalIgnoreCase);
            if (!isPathToFolder && !isPathToSaveFile && data != null && data.Length > 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("Save game"), Localization.Tr("Save games opened from an archive cannot be loaded here. Open the save folder from disk (e.g. the game's Saves directory) for full editing."), IconType.Info);
                New();
                return;
            }
            LoadSaveGame(filepath);
        }

        public override bool CanLoadPath(string filepath)
        {
            return IsSaveGameFolderPath(filepath) || IsSaveGameSavPath(filepath);
        }

        protected override bool TryLoadFromPath(string path)
        {
            if (!CanLoadPath(path))
            {
                return false;
            }

            Load(path, Path.GetFileNameWithoutExtension(path), ResourceType.SAV, null);
            return true;
        }

        protected override bool IsPathSupportedByEditor(string filepath)
        {
            return CanLoadPath(filepath);
        }

        private static bool IsSaveGameFolderPath(string filepath)
        {
            return !string.IsNullOrWhiteSpace(filepath)
                && Directory.Exists(filepath)
                && Directory.EnumerateFiles(filepath)
                    .Any(file => string.Equals(Path.GetFileName(file), "SAVEGAME.sav", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsSaveGameSavPath(string filepath)
        {
            return !string.IsNullOrWhiteSpace(filepath)
                && File.Exists(filepath)
                && string.Equals(Path.GetFileName(filepath), "SAVEGAME.sav", StringComparison.OrdinalIgnoreCase);
        }

        public override Tuple<byte[], byte[]> Build()
        {
            return Tuple.Create(new byte[0], new byte[0]);
        }

        private void LoadSaveGame(string filepath)
        {
            try
            {
                string saveFolder;
                if (File.Exists(filepath) && Path.GetFileName(filepath).ToUpperInvariant() == "SAVEGAME.SAV")
                {
                    saveFolder = Path.GetDirectoryName(filepath);
                    _filepath = saveFolder;
                }
                else
                {
                    saveFolder = filepath;
                }

                _saveFolder = new SaveFolderEntry(saveFolder);
                _saveFolder.Load();

                _saveInfo = _saveFolder.SaveInfo;
                _partyTable = _saveFolder.PartyTable;
                _globalVars = _saveFolder.GlobalVars;
                _nestedCapsule = _saveFolder.NestedCapsule;
                _parsedCharacters.Clear();
                _inventoryItems.Clear();

                PopulateSaveInfo();
                PopulatePartyTable();
                PopulateGlobalVars();
                PopulateCharacters();
                PopulateInventory();
                PopulateJournal();
                PopulateCachedModules();
                PopulateDoors();
                PopulateReputation();
                PopulateScreenshot();
                PopulateAdvancedFields();

                ClearDirty();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowErrorFromException(this, ex);
                New();
            }
        }

        public override void Save()
        {
            if (_saveFolder == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("No save game is currently loaded."), IconType.Warning);
                return;
            }
            try
            {
                UpdateSaveInfoFromUI();
                UpdatePartyTableFromUI();
                UpdateGlobalVarsFromUI();
                UpdateCharactersFromUI();
                UpdateInventoryFromUI();
                UpdateReputationFromUI();
                UpdateDoorsFromUI();
                UpdateDoorsToModule();
                UpdateCachedCharacters();

                _saveFolder.Save();
                ClearDirty();
                DialogHelper.ShowWindow(this, Localization.Tr("Saved"), Localization.Tr("Save game saved successfully."), IconType.Success);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowErrorFromException(this, ex);
            }
        }

        public override void New()
        {
            base.New();
            _saveFolder = null;
            _saveInfo = null;
            _partyTable = null;
            _globalVars = null;
            _nestedCapsule = null;
            _currentCharacter = null;
            _parsedCharacters.Clear();
            _inventoryItems.Clear();

            ClearSaveInfo();
            ClearPartyTable();
            ClearGlobalVars();
            ClearCharacters();
            ClearInventory();
            ClearJournal();
            ClearCachedModules();
            ClearDoors();
            ClearReputation();
            ClearAdvancedFields();
            ClearScreenshot();
        }

        #region Save Info
        private void PopulateSaveInfo()
        {
            if (_saveInfo == null) return;
            _suppressSaveInfoDirty = true;
            try
            {
                _lineEditSaveName.Text = _saveInfo.SavegameName ?? "";
                _lineEditAreaName.Text = _saveInfo.AreaName ?? "";
                _lineEditLastModule.Text = _saveInfo.LastModule ?? "";
                _spinBoxTimePlayed.Value = _saveInfo.TimePlayed;
                _lineEditPCName.Text = _saveInfo.PcName ?? "";
                _lineEditPortrait0.Text = _saveInfo.Portrait0?.ToString() ?? "";
                _lineEditPortrait1.Text = _saveInfo.Portrait1?.ToString() ?? "";
                _lineEditPortrait2.Text = _saveInfo.Portrait2?.ToString() ?? "";
                _checkBoxCheatUsed.IsChecked = _saveInfo.CheatUsed;
                _spinBoxGameplayHint.Value = _saveInfo.GameplayHint;
                _spinBoxStoryHint.Value = _saveInfo.StoryHint;
                _lineEditLive1.Text = _saveInfo.Live1 ?? "";
                _lineEditLive2.Text = _saveInfo.Live2 ?? "";
                _lineEditLive3.Text = _saveInfo.Live3 ?? "";
                _lineEditLive4.Text = _saveInfo.Live4 ?? "";
                _lineEditLive5.Text = _saveInfo.Live5 ?? "";
                _lineEditLive6.Text = _saveInfo.Live6 ?? "";
                _spinBoxLiveContent.Value = _saveInfo.LiveContent;
                if (_saveInfo.Timestamp.HasValue)
                {
                    try
                    {
                        var winEpoch = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        var dt = winEpoch.AddTicks((long)_saveInfo.Timestamp.Value);
                        _lineEditTimestamp.Text = dt.ToString("yyyy-MM-dd HH:mm:ss") + " UTC";
                    }
                    catch { _lineEditTimestamp.Text = _saveInfo.Timestamp.ToString(); }
                }
                else _lineEditTimestamp.Text = "";
            }
            finally
            {
                _suppressSaveInfoDirty = false;
            }
        }

        private void UpdateSaveInfoFromUI()
        {
            if (_saveInfo == null) return;
            _saveInfo.SavegameName = _lineEditSaveName?.Text ?? "";
            _saveInfo.AreaName = _lineEditAreaName?.Text ?? "";
            _saveInfo.LastModule = _lineEditLastModule?.Text ?? "";
            _saveInfo.TimePlayed = (int)(_spinBoxTimePlayed?.Value ?? 0);
            _saveInfo.PcName = _lineEditPCName?.Text ?? "";
            _saveInfo.Portrait0 = BioWare.Common.ResRef.FromString(_lineEditPortrait0?.Text?.Trim() ?? "");
            _saveInfo.Portrait1 = BioWare.Common.ResRef.FromString(_lineEditPortrait1?.Text?.Trim() ?? "");
            _saveInfo.Portrait2 = BioWare.Common.ResRef.FromString(_lineEditPortrait2?.Text?.Trim() ?? "");
            _saveInfo.CheatUsed = _checkBoxCheatUsed?.IsChecked ?? false;
            _saveInfo.GameplayHint = (byte)(_spinBoxGameplayHint?.Value ?? 0);
            _saveInfo.StoryHint = (byte)(_spinBoxStoryHint?.Value ?? 0);
            _saveInfo.Live1 = _lineEditLive1?.Text ?? "";
            _saveInfo.Live2 = _lineEditLive2?.Text ?? "";
            _saveInfo.Live3 = _lineEditLive3?.Text ?? "";
            _saveInfo.Live4 = _lineEditLive4?.Text ?? "";
            _saveInfo.Live5 = _lineEditLive5?.Text ?? "";
            _saveInfo.Live6 = _lineEditLive6?.Text ?? "";
            _saveInfo.LiveContent = (byte)(_spinBoxLiveContent?.Value ?? 0);
        }

        private void ClearSaveInfo()
        {
            _suppressSaveInfoDirty = true;
            try
            {
                _lineEditSaveName.Text = "";
                _lineEditAreaName.Text = "";
                _lineEditLastModule.Text = "";
                _spinBoxTimePlayed.Value = 0;
                _lineEditPCName.Text = "";
                _lineEditPortrait0.Text = "";
                _lineEditPortrait1.Text = "";
                _lineEditPortrait2.Text = "";
                _checkBoxCheatUsed.IsChecked = false;
                _spinBoxGameplayHint.Value = 0;
                _spinBoxStoryHint.Value = 0;
                _lineEditTimestamp.Text = "";
                _lineEditLive1.Text = "";
                _lineEditLive2.Text = "";
                _lineEditLive3.Text = "";
                _lineEditLive4.Text = "";
                _lineEditLive5.Text = "";
                _lineEditLive6.Text = "";
                _spinBoxLiveContent.Value = 0;
            }
            finally
            {
                _suppressSaveInfoDirty = false;
            }
        }
        #endregion

        #region Screenshot
        private void PopulateScreenshot()
        {
            _screenshotPreview.Source = null;
            _screenshotPlaceholder.IsVisible = true;
            if (_saveFolder == null) return;
            string path = Path.Combine(_saveFolder.FolderPath, "screen.tga");
            if (!File.Exists(path)) return;
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                if (bytes == null || bytes.Length < 18) return;
                TGAImage tga;
                using (var ms = new MemoryStream(bytes))
                    tga = TGA.ReadTga(ms);
                if (tga?.Data == null || tga.Width <= 0 || tga.Height <= 0) return;
                var bmp = new WriteableBitmap(new PixelSize(tga.Width, tga.Height), new Avalonia.Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888, Avalonia.Platform.AlphaFormat.Opaque);
                using (var fb = bmp.Lock())
                {
                    int len = Math.Min(tga.Data.Length, tga.Width * tga.Height * 4);
                    if (len > 0)
                        System.Runtime.InteropServices.Marshal.Copy(tga.Data, 0, fb.Address, len);
                }
                _screenshotPreview.Source = bmp;
                _screenshotPlaceholder.IsVisible = false;
            }
            catch { }
        }

        private void ClearScreenshot()
        {
            _screenshotPreview.Source = null;
            _screenshotPlaceholder.IsVisible = true;
        }
        #endregion

        #region Party Table
        private void PopulatePartyTable()
        {
            if (_partyTable == null) return;
            _suppressPartyTableDirty = true;
            try
            {
                _spinBoxGold.Value = _partyTable.Gold;
                _spinBoxXPPool.Value = _partyTable.XpPool;
                _spinBoxComponents.Value = _partyTable.ItemComponents;
                _spinBoxChemicals.Value = _partyTable.ItemChemicals;
                _spinBoxTimePlayedPT.Value = _partyTable.TimePlayed >= 0 ? _partyTable.TimePlayed : 0;
                _checkBoxCheatUsedPT.IsChecked = _partyTable.CheatUsed;
                _spinBoxControlledNPC.Value = _partyTable.ControlledNpc;
                _spinBoxAIState.Value = _partyTable.AiState;
                _spinBoxFollowState.Value = _partyTable.FollowState;
                _checkBoxSoloMode.IsChecked = _partyTable.SoloMode;
                _spinBoxLastGUIPanel.Value = _partyTable.LastGuiPanel;
                _spinBoxJournalSortOrder.Value = _partyTable.JournalSortOrder;

                _listWidgetPartyMembers.Items.Clear();
                foreach (var m in _partyTable.Members.OrderBy(x => !x.IsLeader).ThenBy(x => x.Index >= 0 ? x.Index : 999))
                {
                    var display = GetPartyMemberName(m) + (m.IsLeader ? " [Leader]" : "");
                    _listWidgetPartyMembers.Items.Add(new PartyMemberDisplayItem { Display = display, ToolTip = GetPartyMemberTooltip(m), Member = m });
                }

                var availList = new List<dynamic>();
                while (_partyTable.AvailableNpcs.Count < 12) _partyTable.AvailableNpcs.Add(new AvailableNPCEntry());
                for (int i = 0; i < 12; i++)
                {
                    var e = _partyTable.AvailableNpcs[i];
                    availList.Add(new { Index = i, Available = e.NpcAvailable, Selected = e.NpcSelected });
                }
                _gridAvailableNPCs.ItemsSource = availList;

                var infList = new List<dynamic>();
                for (int i = 0; i < 12; i++)
                    infList.Add(new { Index = i, Value = i < _partyTable.Influence.Count ? _partyTable.Influence[i].ToString() : "0" });
                _gridInfluence.ItemsSource = infList;
            }
            finally
            {
                _suppressPartyTableDirty = false;
            }

            PopulatePazaakAndMiscSummary();
        }

        private void PopulatePazaakAndMiscSummary()
        {
            if (_partyTable == null) return;
            int cards = _partyTable.PazaakCards?.Count ?? 0;
            int decks = _partyTable.PazaakDecks?.Count ?? 0;
            int fb = _partyTable.FeedbackMessages?.Count ?? 0;
            int dlg = _partyTable.DialogMessages?.Count ?? 0;
            int tut = _partyTable.TutorialWindowsShown?.Length ?? 0;
            int cost = _partyTable.CostMultiplierList?.Count ?? 0;
            if (_textPazaakCards != null) _textPazaakCards.Text = Localization.Tr("Pazaak Cards") + ": " + cards + " " + Localization.Tr("entries");
            if (_textPazaakDecks != null) _textPazaakDecks.Text = Localization.Tr("Pazaak Decks") + ": " + decks + " " + Localization.Tr("entries");
            if (_textFeedbackMessages != null) _textFeedbackMessages.Text = Localization.Tr("Feedback Messages") + ": " + fb + " " + Localization.Tr("entries");
            if (_textDialogMessages != null) _textDialogMessages.Text = Localization.Tr("Dialog Messages") + ": " + dlg + " " + Localization.Tr("entries");
            if (_textTutorialShown != null) _textTutorialShown.Text = Localization.Tr("Tutorial Windows Shown") + ": " + tut + " " + Localization.Tr("bytes");
            if (_textCostMultipliers != null) _textCostMultipliers.Text = Localization.Tr("Cost Multipliers") + ": " + cost + " " + Localization.Tr("entries");
        }

        private string GetPartyMemberName(PartyMemberEntry member)
        {
            if (member.Index == -1) return _saveInfo?.PcName ?? "PC";
            if (member.IsLeader && !string.IsNullOrWhiteSpace(_saveInfo?.PcName)) return _saveInfo.PcName;
            if (!string.IsNullOrWhiteSpace(_partyTable?.PcName) && member.Index == 0) return _partyTable.PcName;
            if (_nestedCapsule?.CachedCharacterIndices != null && _nestedCapsule.CachedCharacterIndices.TryGetValue(member.Index, out var ident))
                return ident.ResName ?? $"Member #{member.Index}";
            return $"Member #{member.Index}";
        }

        private string GetPartyMemberTooltip(PartyMemberEntry member)
        {
            var lines = new List<string>
            {
                "Party Member Information",
                "Index: " + member.Index,
                "Type: " + (member.Index == -1 ? "Player Character" : "Companion"),
                "Is Leader: " + (member.IsLeader ? "Yes" : "No")
            };
            UTC charUtc = null;
            if (member.Index == -1 && _nestedCapsule != null)
            {
                foreach (var kvp in _nestedCapsule.CachedCharacters)
                {
                    try
                    {
                        var utc = ResourceAutoHelpers.ReadUtc(kvp.Value);
                        if (utc != null && _saveInfo != null && !string.IsNullOrEmpty(_saveInfo.PcName))
                        {
                            var name = _installation?.String(utc.FirstName, "") ?? utc.FirstName?.GetString(0, Gender.Male) ?? "";
                            if (name.Trim() == _saveInfo.PcName.Trim()) { charUtc = utc; break; }
                        }
                    }
                    catch { }
                }
                if (charUtc == null && _nestedCapsule.CachedCharacters.Count > 0)
                    try { charUtc = ResourceAutoHelpers.ReadUtc(_nestedCapsule.CachedCharacters.Values.First()); } catch { }
            }
            else if (member.Index >= 0 && _nestedCapsule?.CachedCharacterIndices != null && _nestedCapsule.CachedCharacterIndices.TryGetValue(member.Index, out var ident) && _nestedCapsule.CachedCharacters.TryGetValue(ident, out var data))
            {
                try { charUtc = ResourceAutoHelpers.ReadUtc(data); } catch { }
            }
            if (charUtc != null)
            {
                lines.Add("---");
                lines.Add("Name: " + (_installation?.String(charUtc.FirstName, "") ?? charUtc.FirstName?.GetString(0, Gender.Male) ?? ""));
                lines.Add("Tag: " + (charUtc.Tag ?? "N/A"));
                lines.Add("ResRef: " + (charUtc.ResRef?.ToString() ?? "N/A"));
                lines.Add("HP: " + charUtc.CurrentHp + "/" + charUtc.MaxHp);
                lines.Add("FP: " + charUtc.Fp + "/" + charUtc.MaxFp);
                lines.Add("STR: " + charUtc.Strength + " DEX: " + charUtc.Dexterity + " CON: " + charUtc.Constitution);
                lines.Add("INT: " + charUtc.Intelligence + " WIS: " + charUtc.Wisdom + " CHA: " + charUtc.Charisma);
            }
            return string.Join("\n", lines);
        }

        private void UpdatePartyTableFromUI()
        {
            if (_partyTable == null) return;
            _partyTable.Gold = (int)(_spinBoxGold?.Value ?? 0);
            _partyTable.XpPool = (int)(_spinBoxXPPool?.Value ?? 0);
            _partyTable.ItemComponents = (int)(_spinBoxComponents?.Value ?? 0);
            _partyTable.ItemChemicals = (int)(_spinBoxChemicals?.Value ?? 0);
            _partyTable.TimePlayed = (int)(_spinBoxTimePlayedPT?.Value ?? -1);
            _partyTable.CheatUsed = _checkBoxCheatUsedPT?.IsChecked ?? false;
            _partyTable.PcName = _saveInfo?.PcName ?? "";
            _partyTable.ControlledNpc = (int)(_spinBoxControlledNPC?.Value ?? -1);
            _partyTable.AiState = (int)(_spinBoxAIState?.Value ?? 0);
            _partyTable.FollowState = (int)(_spinBoxFollowState?.Value ?? 0);
            _partyTable.SoloMode = _checkBoxSoloMode?.IsChecked ?? false;
            _partyTable.LastGuiPanel = (int)(_spinBoxLastGUIPanel?.Value ?? 0);
            _partyTable.JournalSortOrder = (int)(_spinBoxJournalSortOrder?.Value ?? 0);

            if (_gridAvailableNPCs?.ItemsSource is IEnumerable<dynamic> availSrc)
            {
                int idx = 0;
                foreach (var row in availSrc)
                {
                    while (_partyTable.AvailableNpcs.Count <= idx) _partyTable.AvailableNpcs.Add(new AvailableNPCEntry());
                    var e = _partyTable.AvailableNpcs[idx];
                    e.NpcAvailable = row.Available;
                    e.NpcSelected = row.Selected;
                    idx++;
                }
            }

            if (_gridInfluence?.ItemsSource is IEnumerable<dynamic> infSrc)
            {
                _partyTable.Influence.Clear();
                foreach (var row in infSrc)
                {
                    int v;
                    _partyTable.Influence.Add(int.TryParse(row.Value?.ToString(), out v) ? v : 0);
                }
            }

            UpdateJournalFromUI();
        }

        private void ClearPartyTable()
        {
            _spinBoxGold.Value = 0;
            _spinBoxXPPool.Value = 0;
            _spinBoxComponents.Value = 0;
            _spinBoxChemicals.Value = 0;
            _spinBoxTimePlayedPT.Value = 0;
            _checkBoxCheatUsedPT.IsChecked = false;
            _spinBoxControlledNPC.Value = -1;
            _spinBoxAIState.Value = 0;
            _spinBoxFollowState.Value = 0;
            _checkBoxSoloMode.IsChecked = false;
            _spinBoxLastGUIPanel.Value = 0;
            _spinBoxJournalSortOrder.Value = 0;
            if (_textPazaakCards != null) _textPazaakCards.Text = "";
            if (_textPazaakDecks != null) _textPazaakDecks.Text = "";
            if (_textFeedbackMessages != null) _textFeedbackMessages.Text = "";
            if (_textDialogMessages != null) _textDialogMessages.Text = "";
            if (_textTutorialShown != null) _textTutorialShown.Text = "";
            if (_textCostMultipliers != null) _textCostMultipliers.Text = "";
            _listWidgetPartyMembers.Items.Clear();
            _gridAvailableNPCs.ItemsSource = null;
            _gridInfluence.ItemsSource = null;
        }
        #endregion

        #region Global Vars
        private void PopulateGlobalVars()
        {
            if (_globalVars == null) return;
            _gridBooleans.ItemsSource = _globalVars.GlobalBools.Select(x => new GlobalBoolRow { Name = x.Item1, Value = x.Item2 }).ToList();
            _gridNumbers.ItemsSource = _globalVars.GlobalNumbers.Select(x => new GlobalNumberRow { Name = x.Item1, Value = x.Item2 }).ToList();
            _gridStrings.ItemsSource = _globalVars.GlobalStrings.Select(x => new GlobalStringRow { Name = x.Item1, Value = x.Item2 }).ToList();
            _gridLocations.ItemsSource = _globalVars.GlobalLocations.Select(x => new GlobalLocationRow { Name = x.Item1, X = x.Item2.X, Y = x.Item2.Y, Z = x.Item2.Z, Orientation = x.Item2.W }).ToList();
        }

        private void UpdateGlobalVarsFromUI()
        {
            if (_globalVars == null) return;
            if (_gridBooleans?.ItemsSource is IEnumerable<GlobalBoolRow> boolRows)
            {
                _globalVars.GlobalBools.Clear();
                foreach (var row in boolRows)
                    if (!string.IsNullOrWhiteSpace(row.Name))
                        _globalVars.GlobalBools.Add(Tuple.Create(row.Name.Trim(), row.Value));
            }
            if (_gridNumbers?.ItemsSource is IEnumerable<GlobalNumberRow> numRows)
            {
                _globalVars.GlobalNumbers.Clear();
                foreach (var row in numRows)
                    if (!string.IsNullOrWhiteSpace(row.Name))
                        _globalVars.GlobalNumbers.Add(Tuple.Create(row.Name.Trim(), Math.Max(0, Math.Min(255, row.Value))));
            }
            if (_gridStrings?.ItemsSource is IEnumerable<GlobalStringRow> strRows)
            {
                _globalVars.GlobalStrings.Clear();
                foreach (var row in strRows)
                    if (!string.IsNullOrWhiteSpace(row.Name))
                        _globalVars.GlobalStrings.Add(Tuple.Create(row.Name.Trim(), row.Value ?? ""));
            }
            if (_gridLocations?.ItemsSource is IEnumerable<GlobalLocationRow> locRows)
            {
                _globalVars.GlobalLocations.Clear();
                foreach (var row in locRows)
                    if (!string.IsNullOrWhiteSpace(row.Name))
                        _globalVars.GlobalLocations.Add(Tuple.Create(row.Name.Trim(), new Vector4(row.X, row.Y, row.Z, row.Orientation)));
            }
        }

        private void ClearGlobalVars()
        {
            _gridBooleans.ItemsSource = null;
            _gridNumbers.ItemsSource = null;
            _gridStrings.ItemsSource = null;
            _gridLocations.ItemsSource = null;
        }
        #endregion

        #region Characters
        private void PopulateCharacters()
        {
            _listWidgetCharacters.Items.Clear();
            _parsedCharacters.Clear();
            if (_nestedCapsule == null) return;
            foreach (var kvp in _nestedCapsule.CachedCharacters.OrderBy(x => x.Key.ResName))
            {
                try
                {
                    var utc = ResourceAutoHelpers.ReadUtc(kvp.Value);
                    if (utc == null) continue;
                    _parsedCharacters[kvp.Key] = utc;
                    string name = _installation?.String(utc.FirstName, "") ?? utc.FirstName?.GetString(0, Gender.Male) ?? utc.Tag ?? kvp.Key.ResName ?? "";
                    _listWidgetCharacters.Items.Add(new { Display = name, Ident = kvp.Key, Utc = utc });
                }
                catch { }
            }
        }

        private void OnCharacterSelected()
        {
            var sel = _listWidgetCharacters.SelectedItem;
            if (sel == null) { _currentCharacter = null; ClearCharacterDetails(); return; }
            dynamic d = sel;
            _currentCharacter = d.Utc as UTC;
            if (_currentCharacter != null) PopulateCharacterDetails();
            else ClearCharacterDetails();
        }

        private void PopulateCharacterDetails()
        {
            if (_currentCharacter == null) return;
            _lineEditCharName.Text = _installation?.String(_currentCharacter.FirstName, "") ?? _currentCharacter.FirstName?.GetString(0, Gender.Male) ?? "";
            _lineEditCharTag.Text = _currentCharacter.Tag ?? "";
            _lineEditCharResRef.Text = _currentCharacter.ResRef?.ToString() ?? "";
            _spinBoxCharHP.Value = _currentCharacter.CurrentHp;
            _spinBoxCharMaxHP.Value = _currentCharacter.MaxHp;
            _spinBoxCharFP.Value = _currentCharacter.Fp;
            _spinBoxCharMaxFP.Value = _currentCharacter.MaxFp;
            _spinBoxCharXP.Value = _currentCharacter.Experience;
            _checkBoxCharMin1HP.IsChecked = _currentCharacter.Min1Hp;
            _spinBoxCharGoodEvil.Value = _currentCharacter.Alignment;
            _spinBoxCharSTR.Value = _currentCharacter.Strength;
            _spinBoxCharDEX.Value = _currentCharacter.Dexterity;
            _spinBoxCharCON.Value = _currentCharacter.Constitution;
            _spinBoxCharINT.Value = _currentCharacter.Intelligence;
            _spinBoxCharWIS.Value = _currentCharacter.Wisdom;
            _spinBoxCharCHA.Value = _currentCharacter.Charisma;
            _spinBoxCharPortraitId.Value = _currentCharacter.PortraitId;
            _spinBoxCharAppearanceType.Value = _currentCharacter.AppearanceId;
            _spinBoxCharSoundset.Value = _currentCharacter.SoundsetId;
            _comboCharGender.SelectedIndex = Math.Min(_currentCharacter.GenderId, 4);

            var skills = new List<dynamic>();
            var names = new[] { "ComputerUse", "Demolitions", "Stealth", "Awareness", "Persuade", "Repair", "Security", "TreatInjury" };
            var vals = new[] { _currentCharacter.ComputerUse, _currentCharacter.Demolitions, _currentCharacter.Stealth, _currentCharacter.Awareness, _currentCharacter.Persuade, _currentCharacter.Repair, _currentCharacter.Security, _currentCharacter.TreatInjury };
            for (int i = 0; i < 8; i++) skills.Add(new { Name = SkillNames[i], Rank = vals[i].ToString() });
            _gridSkills.ItemsSource = skills;

            var classes = new List<ClassGridRow>();
            if (_currentCharacter.Classes != null)
                foreach (var c in _currentCharacter.Classes)
                    classes.Add(new ClassGridRow { Name = GetClassName(c.ClassId) ?? $"Class {c.ClassId}", ClassId = c.ClassId, Level = c.ClassLevel.ToString() });
            _gridCharClasses.ItemsSource = classes;

            var feats = new List<FeatDisplayItem>();
            if (_currentCharacter.Feats != null)
                foreach (var f in _currentCharacter.Feats)
                    feats.Add(new FeatDisplayItem { FeatId = f, Display = GetFeatName(f) ?? $"Feat {f}" });
            _listWidgetCharFeats.ItemsSource = feats;

            var powers = new List<PowerDisplayItem>();
            if (_currentCharacter.Classes != null)
            {
                var seen = new HashSet<int>();
                foreach (var c in _currentCharacter.Classes)
                {
                    if (c.Powers != null)
                    {
                        foreach (var p in c.Powers)
                        {
                            if (!seen.Contains(p))
                            {
                                seen.Add(p);
                                powers.Add(new PowerDisplayItem { PowerId = p, Display = GetPowerName(p) ?? $"Power {p}" });
                            }
                        }
                    }
                }
            }
            _listWidgetCharPowers.ItemsSource = powers;

            PopulateFeatComboBox();
            PopulateClassComboBox();
            PopulatePowerComboBox();

            _listWidgetEquipment.Items.Clear();
            if (_currentCharacter.Equipment != null)
                foreach (var kvp in _currentCharacter.Equipment)
                    _listWidgetEquipment.Items.Add(new EquipmentDisplayItem { Slot = kvp.Key, Item = kvp.Value });
        }

        private void UpdateCharactersFromUI()
        {
            if (_currentCharacter == null) return;
            var nameText = _lineEditCharName?.Text?.Trim() ?? "";
            _currentCharacter.FirstName = string.IsNullOrEmpty(nameText) ? LocalizedString.FromInvalid() : LocalizedString.FromEnglish(nameText);
            _currentCharacter.Tag = _lineEditCharTag?.Text?.Trim() ?? "";
            var resrefText = _lineEditCharResRef?.Text?.Trim() ?? "";
            _currentCharacter.ResRef = string.IsNullOrEmpty(resrefText) ? BioWare.Common.ResRef.FromBlank() : BioWare.Common.ResRef.FromString(resrefText);
            _currentCharacter.CurrentHp = (int)(_spinBoxCharHP?.Value ?? 0);
            _currentCharacter.MaxHp = (int)(_spinBoxCharMaxHP?.Value ?? 0);
            _currentCharacter.Fp = (int)(_spinBoxCharFP?.Value ?? 0);
            _currentCharacter.MaxFp = (int)(_spinBoxCharMaxFP?.Value ?? 0);
            _currentCharacter.Experience = (int)(_spinBoxCharXP?.Value ?? 0);
            _currentCharacter.Min1Hp = _checkBoxCharMin1HP?.IsChecked ?? false;
            _currentCharacter.Alignment = (int)(_spinBoxCharGoodEvil?.Value ?? 50);
            _currentCharacter.Strength = (int)(_spinBoxCharSTR?.Value ?? 10);
            _currentCharacter.Dexterity = (int)(_spinBoxCharDEX?.Value ?? 10);
            _currentCharacter.Constitution = (int)(_spinBoxCharCON?.Value ?? 10);
            _currentCharacter.Intelligence = (int)(_spinBoxCharINT?.Value ?? 10);
            _currentCharacter.Wisdom = (int)(_spinBoxCharWIS?.Value ?? 10);
            _currentCharacter.Charisma = (int)(_spinBoxCharCHA?.Value ?? 10);
            _currentCharacter.PortraitId = (int)(_spinBoxCharPortraitId?.Value ?? 0);
            _currentCharacter.AppearanceId = (int)(_spinBoxCharAppearanceType?.Value ?? 0);
            _currentCharacter.SoundsetId = (int)(_spinBoxCharSoundset?.Value ?? 0);
            _currentCharacter.GenderId = _comboCharGender?.SelectedIndex ?? 0;
            if (_gridSkills?.ItemsSource is IEnumerable<dynamic> skillRows)
            {
                var vals = new[] { _currentCharacter.ComputerUse, _currentCharacter.Demolitions, _currentCharacter.Stealth, _currentCharacter.Awareness, _currentCharacter.Persuade, _currentCharacter.Repair, _currentCharacter.Security, _currentCharacter.TreatInjury };
                int i = 0;
                foreach (var row in skillRows)
                {
                    int v;
                    if (int.TryParse(row.Rank?.ToString(), out v) && i < 8)
                    {
                        if (i == 0) _currentCharacter.ComputerUse = v;
                        else if (i == 1) _currentCharacter.Demolitions = v;
                        else if (i == 2) _currentCharacter.Stealth = v;
                        else if (i == 3) _currentCharacter.Awareness = v;
                        else if (i == 4) _currentCharacter.Persuade = v;
                        else if (i == 5) _currentCharacter.Repair = v;
                        else if (i == 6) _currentCharacter.Security = v;
                        else _currentCharacter.TreatInjury = v;
                    }
                    i++;
                }
            }
            if (_gridCharClasses?.ItemsSource is IEnumerable<ClassGridRow> classRows)
            {
                _currentCharacter.Classes.Clear();
                foreach (var row in classRows)
                {
                    int level;
                    if (int.TryParse(row.Level?.Trim(), out level) && level >= 0 && level <= 50)
                        _currentCharacter.Classes.Add(new UTCClass(row.ClassId, level));
                }
            }

            if (_listWidgetCharFeats?.ItemsSource is IEnumerable<FeatDisplayItem> featItems)
            {
                _currentCharacter.Feats.Clear();
                foreach (var item in featItems)
                    _currentCharacter.Feats.Add(item.FeatId);
            }
        }

        private void PopulateFeatComboBox()
        {
            _comboAddFeat?.Items.Clear();
            if (_installation == null || _comboAddFeat == null) return;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAFeats);
                if (twoda == null) return;
                var present = new HashSet<int>(_currentCharacter?.Feats ?? Enumerable.Empty<int>());
                for (int i = 0; i < twoda.GetHeight(); i++)
                {
                    if (present.Contains(i)) continue;
                    try
                    {
                        var label = twoda.GetCellString(i, "label");
                        if (!string.IsNullOrWhiteSpace(label))
                            _comboAddFeat.Items.Add(new FeatDisplayItem { FeatId = i, Display = $"{label} ({i})" });
                    }
                    catch { }
                }
                if (_comboAddFeat.Items.Count > 0)
                    _comboAddFeat.SelectedIndex = 0;
            }
            catch { }
        }

        private void AddCharacterFeat()
        {
            if (_currentCharacter == null) return;
            var sel = _comboAddFeat?.SelectedItem as FeatDisplayItem;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a feat to add."), IconType.Info);
                return;
            }
            if (_currentCharacter.Feats == null) _currentCharacter.Feats = new List<int>();
            if (_currentCharacter.Feats.Contains(sel.FeatId)) return;
            _currentCharacter.Feats.Add(sel.FeatId);
            PopulateCharacterDetails();
            MarkDocumentDirty();
        }

        private void RemoveCharacterFeat()
        {
            if (_currentCharacter == null) return;
            var sel = _listWidgetCharFeats?.SelectedItem as FeatDisplayItem;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a feat to remove."), IconType.Info);
                return;
            }
            _currentCharacter.Feats?.Remove(sel.FeatId);
            PopulateCharacterDetails();
            MarkDocumentDirty();
        }

        private void PopulateClassComboBox()
        {
            _comboAddClass?.Items.Clear();
            if (_installation == null || _comboAddClass == null) return;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAClasses);
                if (twoda == null) return;
                var present = new HashSet<int>((_currentCharacter?.Classes ?? Enumerable.Empty<UTCClass>()).Select(c => c.ClassId));
                for (int i = 0; i < twoda.GetHeight(); i++)
                {
                    if (present.Contains(i)) continue;
                    try
                    {
                        var label = twoda.GetCellString(i, "label");
                        if (!string.IsNullOrWhiteSpace(label))
                            _comboAddClass.Items.Add(new ClassGridRow { ClassId = i, Name = label, Level = "" });
                    }
                    catch { }
                }
                if (_comboAddClass.Items.Count > 0)
                    _comboAddClass.SelectedIndex = 0;
            }
            catch { }
        }

        private void AddCharacterClass()
        {
            if (_currentCharacter == null) return;
            var sel = _comboAddClass?.SelectedItem as ClassGridRow;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a class to add."), IconType.Info);
                return;
            }
            if (_currentCharacter.Classes == null) _currentCharacter.Classes = new List<UTCClass>();
            if (_currentCharacter.Classes.Any(c => c.ClassId == sel.ClassId))
            {
                DialogHelper.ShowWindow(this, Localization.Tr("Already Present"), Localization.Tr("Character already has this class."), IconType.Info);
                return;
            }
            _currentCharacter.Classes.Add(new UTCClass(sel.ClassId, 1));
            PopulateCharacterDetails();
            MarkDocumentDirty();
        }

        private void RemoveCharacterClass()
        {
            if (_currentCharacter == null) return;
            int idx = _gridCharClasses?.SelectedIndex ?? -1;
            if (idx < 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a class to remove."), IconType.Info);
                return;
            }
            if (idx < _currentCharacter.Classes?.Count)
            {
                _currentCharacter.Classes.RemoveAt(idx);
                PopulateCharacterDetails();
                MarkDocumentDirty();
            }
        }

        private void PopulatePowerComboBox()
        {
            _comboAddPower?.Items.Clear();
            if (_installation == null || _comboAddPower == null || _currentCharacter == null) return;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAPowers);
                if (twoda == null) return;
                var present = new HashSet<int>();
                if (_currentCharacter.Classes != null)
                {
                    foreach (var c in _currentCharacter.Classes)
                        if (c.Powers != null)
                            foreach (var p in c.Powers) present.Add(p);
                }
                for (int i = 0; i < twoda.GetHeight(); i++)
                {
                    if (present.Contains(i)) continue;
                    try
                    {
                        var label = twoda.GetCellString(i, "label");
                        var name = GetPowerName(i);
                        var display = !string.IsNullOrWhiteSpace(name) ? name : (label ?? $"Power {i}");
                        _comboAddPower.Items.Add(new PowerDisplayItem { PowerId = i, Display = $"{display} ({i})" });
                    }
                    catch { }
                }
                if (_comboAddPower.Items.Count > 0)
                    _comboAddPower.SelectedIndex = 0;
            }
            catch { }
        }

        private void AddCharacterPower()
        {
            if (_currentCharacter == null) return;
            var sel = _comboAddPower?.SelectedItem as PowerDisplayItem;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a power to add."), IconType.Info);
                return;
            }
            if (_currentCharacter.Classes == null || _currentCharacter.Classes.Count == 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Class"), Localization.Tr("Character must have at least one class to add powers."), IconType.Warning);
                return;
            }
            // Add to first Force-using class (Jedi/Sith: ClassId 2–7 in K1/K2), else first class
            var targetClass = _currentCharacter.Classes.FirstOrDefault(c => c.ClassId >= 2 && c.ClassId <= 7) ?? _currentCharacter.Classes[0];
            if (targetClass.Powers == null) targetClass.Powers = new List<int>();
            if (targetClass.Powers.Contains(sel.PowerId)) return;
            targetClass.Powers.Add(sel.PowerId);
            PopulateCharacterDetails();
            MarkDocumentDirty();
        }

        private void RemoveCharacterPower()
        {
            if (_currentCharacter == null) return;
            var sel = _listWidgetCharPowers?.SelectedItem as PowerDisplayItem;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a power to remove."), IconType.Info);
                return;
            }
            if (_currentCharacter.Classes != null)
            {
                foreach (var c in _currentCharacter.Classes)
                {
                    if (c.Powers != null && c.Powers.Remove(sel.PowerId))
                    {
                        PopulateCharacterDetails();
                        MarkDocumentDirty();
                        return;
                    }
                }
            }
        }

        private void UpdateCachedCharacters()
        {
            foreach (var kvp in _parsedCharacters)
            {
                try
                {
                    byte[] data = UTCHelpers.BytesUtc(kvp.Value, BioWareGame.K2);
                    _nestedCapsule.SetResource(kvp.Key, data);
                    _nestedCapsule.CachedCharacters[kvp.Key] = data;
                }
                catch { }
            }
        }

        private void ClearCharacterDetails()
        {
            _lineEditCharName.Text = "";
            _lineEditCharTag.Text = "";
            _lineEditCharResRef.Text = "";
            _spinBoxCharHP.Value = 0;
            _spinBoxCharMaxHP.Value = 0;
            _spinBoxCharFP.Value = 0;
            _spinBoxCharMaxFP.Value = 0;
            _spinBoxCharXP.Value = 0;
            _checkBoxCharMin1HP.IsChecked = false;
            _spinBoxCharGoodEvil.Value = 50;
            _spinBoxCharSTR.Value = 10;
            _spinBoxCharDEX.Value = 10;
            _spinBoxCharCON.Value = 10;
            _spinBoxCharINT.Value = 10;
            _spinBoxCharWIS.Value = 10;
            _spinBoxCharCHA.Value = 10;
            _spinBoxCharPortraitId.Value = 0;
            _spinBoxCharAppearanceType.Value = 0;
            _spinBoxCharSoundset.Value = 0;
            _comboCharGender.SelectedIndex = 0;
            _gridSkills.ItemsSource = null;
            _gridCharClasses.ItemsSource = null;
            _listWidgetCharFeats.ItemsSource = null;
            _listWidgetCharPowers.ItemsSource = null;
            _listWidgetEquipment.Items.Clear();
        }

        private void ClearCharacters()
        {
            _listWidgetCharacters.Items.Clear();
            ClearCharacterDetails();
        }

        [CanBeNull]
        private string GetClassName(int classId)
        {
            if (_installation == null) return null;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAClasses);
                if (twoda != null && classId >= 0 && classId < twoda.GetHeight())
                {
                    var headers = twoda.GetHeaders();
                    if (headers != null && headers.Contains("label"))
                        return twoda.GetCellString(classId, "label");
                }
            }
            catch { }
            return null;
        }

        [CanBeNull]
        private string GetFeatName(int featId)
        {
            if (_installation == null) return null;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAFeats);
                if (twoda != null && featId >= 0 && featId < twoda.GetHeight())
                {
                    var headers = twoda.GetHeaders();
                    if (headers != null && headers.Contains("label"))
                        return twoda.GetCellString(featId, "label");
                }
            }
            catch { }
            return null;
        }

        [CanBeNull]
        private string GetPowerName(int powerId)
        {
            if (_installation == null) return null;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAPowers);
                if (twoda != null && powerId >= 0 && powerId < twoda.GetHeight())
                {
                    var headers = twoda.GetHeaders();
                    if (headers != null && headers.Contains("name"))
                    {
                        var strRef = twoda.GetCellInt(powerId, "name", 0);
                        if (strRef.HasValue && strRef.Value != 0 && _installation.TalkTable() != null)
                        {
                            var s = _installation.TalkTable().GetString(strRef.Value);
                            if (!string.IsNullOrWhiteSpace(s)) return s.Replace("\n", " ").Trim();
                        }
                    }
                    if (headers != null && headers.Contains("label"))
                        return twoda.GetCellString(powerId, "label");
                }
            }
            catch { }
            return null;
        }

        [CanBeNull]
        private string GetPlotDisplayName(string plotId)
        {
            if (string.IsNullOrWhiteSpace(plotId)) return plotId;
            if (_installation != null)
            {
                try
                {
                    var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAPlot);
                    if (twoda != null)
                    {
                        for (int i = 0; i < twoda.GetHeight(); i++)
                        {
                            try
                            {
                                var label = twoda.GetCellString(i, "label");
                                if (string.Equals(label, plotId.Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        var name = twoda.GetCellString(i, "name");
                                        if (!string.IsNullOrWhiteSpace(name)) return name;
                                    }
                                    catch { }
                                    return label?.Replace("_", " ").Replace("-", " ");
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }
            return plotId?.Replace("_", " ").Replace("-", " ");
        }

        private void OnEquipmentDoubleTapped()
        {
            if (_currentCharacter != null && _listWidgetEquipment.SelectedItem is EquipmentDisplayItem)
                OpenCharacterInventoryDialog();
        }

        private void OnEquipmentRemoveSelected()
        {
            if (_currentCharacter == null) return;
            var sel = _listWidgetEquipment.SelectedItem as EquipmentDisplayItem;
            if (sel == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select an equipment slot to remove."), IconType.Info);
                return;
            }
            if (_currentCharacter.Equipment != null && _currentCharacter.Equipment.Remove(sel.Slot))
            {
                PopulateCharacterDetails();
                MarkDocumentDirty();
            }
        }

        private void OpenCharacterInventoryDialog()
        {
            if (_currentCharacter == null || _installation == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Character"), Localization.Tr("Select a character first, and ensure a game installation is configured."), IconType.Warning);
                return;
            }
            var capsules = GetCapsulesForSaveEditor();
            bool droid = (_currentCharacter.AppearanceId >= 100 && _currentCharacter.AppearanceId <= 199) || (_currentCharacter.Tag != null && _currentCharacter.Tag.IndexOf("droid", StringComparison.OrdinalIgnoreCase) >= 0);
            var inventoryDialog = new InventoryDialog(
                this,
                _installation,
                capsules,
                new List<string>(),
                _currentCharacter.Inventory != null ? new List<InventoryItem>(_currentCharacter.Inventory) : new List<InventoryItem>(),
                _currentCharacter.Equipment != null ? new Dictionary<EquipmentSlot, InventoryItem>(_currentCharacter.Equipment) : new Dictionary<EquipmentSlot, InventoryItem>(),
                droid: droid,
                hideEquipment: false,
                isStore: false);
            if (inventoryDialog.ShowDialog())
            {
                _currentCharacter.Inventory = inventoryDialog.Inventory ?? new List<InventoryItem>();
                _currentCharacter.Equipment = inventoryDialog.Equipment ?? new Dictionary<EquipmentSlot, InventoryItem>();
                PopulateCharacterDetails();
                MarkDocumentDirty();
            }
        }

        private List<Capsule> GetCapsulesForSaveEditor()
        {
            var capsules = new List<Capsule>();
            if (_installation == null) return capsules;
            try
            {
                string lastMod = _saveInfo?.LastModule?.Trim();
                var moduleNames = _installation.ModuleNames();
                string modulesPath = _installation.ModulePath();
                if (string.IsNullOrEmpty(modulesPath) || !Directory.Exists(modulesPath)) return capsules;
                var rootsToAdd = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (!string.IsNullOrEmpty(lastMod)) rootsToAdd.Add(lastMod.ToLowerInvariant());
                rootsToAdd.Add("003ebo");
                rootsToAdd.Add("002ebo");
                rootsToAdd.Add("modules");
                foreach (var kvp in moduleNames)
                {
                    string modFile = kvp.Key;
                    string modLower = modFile.ToLowerInvariant();
                    string root = Path.GetFileNameWithoutExtension(modFile).ToLowerInvariant();
                    if (root.Contains("_")) root = root.Substring(0, root.IndexOf('_'));
                    if (rootsToAdd.Contains(root) || rootsToAdd.Any(r => modLower.Contains(r)))
                    {
                        string fullPath = Path.Combine(modulesPath, modFile);
                        if (File.Exists(fullPath))
                        {
                            try
                            {
                                capsules.Add(new Capsule(fullPath, createIfNotExist: false));
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
            return capsules;
        }
        #endregion

        #region Inventory
        private string GetInventoryItemName(string resRef)
        {
            if (_installation == null || string.IsNullOrWhiteSpace(resRef)) return resRef ?? "";
            try
            {
                var rr = _installation.Resource(resRef.Trim(), ResourceType.UTI);
                if (rr?.Data != null)
                {
                    var uti = UTIHelpers.ReadUti(rr.Data);
                    return _installation.String(uti?.Name, resRef) ?? resRef;
                }
            }
            catch { }
            return resRef;
        }

        private void SyncInventoryFromGrid()
        {
            if (_gridInventory?.ItemsSource is IEnumerable<InventoryGridRow> rows)
            {
                _inventoryItems.Clear();
                foreach (var row in rows)
                {
                    int stack = 0, ch = 0, maxCh = 0, upgrades = 0;
                    int.TryParse(row.StackSize.ToString(), out stack);
                    var chStr = row.ChargesStr ?? "";
                    if (chStr.Contains("/")) { var p = chStr.Split('/'); int.TryParse(p[0], out ch); int.TryParse(p.Length > 1 ? p[1] : "0", out maxCh); }
                    else int.TryParse(chStr, out ch);
                    int.TryParse(row.UpgradeLevel.ToString(), out upgrades);
                    _inventoryItems.Add(new SaveInventoryItem
                    {
                        ResRef = row.ResRef ?? "",
                        StackSize = stack,
                        Charges = ch,
                        MaxCharges = maxCh,
                        UpgradeLevel = upgrades,
                        NewItem = row.NewItem,
                        Upgrades = row.Upgrades,
                        UpgradeSlot0 = row.UpgradeSlot0,
                        UpgradeSlot1 = row.UpgradeSlot1,
                        UpgradeSlot2 = row.UpgradeSlot2,
                        UpgradeSlot3 = row.UpgradeSlot3,
                        UpgradeSlot4 = row.UpgradeSlot4,
                        UpgradeSlot5 = row.UpgradeSlot5
                    });
                }
            }
        }

        private void RefreshInventoryGrid()
        {
            var rows = _inventoryItems.Select(x =>
            {
                var chStr = x.MaxCharges > 0 ? $"{x.Charges}/{x.MaxCharges}" : x.Charges.ToString();
                return new InventoryGridRow
                {
                    Name = GetInventoryItemName(x.ResRef),
                    ResRef = x.ResRef,
                    StackSize = x.StackSize,
                    ChargesStr = chStr,
                    UpgradeLevel = x.UpgradeLevel,
                    NewItem = x.NewItem,
                    Upgrades = x.Upgrades,
                    UpgradeSlot0 = x.UpgradeSlot0,
                    UpgradeSlot1 = x.UpgradeSlot1,
                    UpgradeSlot2 = x.UpgradeSlot2,
                    UpgradeSlot3 = x.UpgradeSlot3,
                    UpgradeSlot4 = x.UpgradeSlot4,
                    UpgradeSlot5 = x.UpgradeSlot5
                };
            }).ToList();
            _gridInventory.ItemsSource = rows;
        }

        private void AddInventoryItem()
        {
            if (_nestedCapsule?.InventoryGff == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("Load a save game first."), IconType.Warning);
                return;
            }
            SyncInventoryFromGrid();
            _inventoryItems.Add(new SaveInventoryItem { ResRef = "", StackSize = 1, Charges = 0, MaxCharges = 0, UpgradeLevel = 0, NewItem = true });
            RefreshInventoryGrid();
            MarkDocumentDirty();
        }

        private async Task AddInventoryItemFromTemplateAsync()
        {
            if (_nestedCapsule?.InventoryGff == null)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Save Loaded"), Localization.Tr("Load a save game first."), IconType.Warning);
                return;
            }
            var utiResRefs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (_nestedCapsule != null)
            {
                if (_nestedCapsule.CachedModules != null)
                {
                    foreach (var erf in _nestedCapsule.CachedModules.Values)
                    {
                        foreach (var res in erf)
                        {
                            if (res?.ResType == ResourceType.UTI && !string.IsNullOrWhiteSpace(res.ResRef?.ToString()))
                                utiResRefs.Add(res.ResRef.ToString().Trim());
                        }
                    }
                }
                if (_nestedCapsule.CachedRimModules != null)
                {
                    foreach (var rim in _nestedCapsule.CachedRimModules.Values)
                    {
                        foreach (var res in rim)
                        {
                            if (res?.ResType == ResourceType.UTI && !string.IsNullOrWhiteSpace(res.ResRef?.ToString()))
                                utiResRefs.Add(res.ResRef.ToString().Trim());
                        }
                    }
                }
            }
            if (_installation != null)
            {
                try
                {
                    var capsules = GetCapsulesForSaveEditor();
                    foreach (var cap in capsules)
                    {
                        if (cap == null) continue;
                        foreach (var res in cap)
                        {
                            if (res?.ResType == ResourceType.UTI && !string.IsNullOrWhiteSpace(res.ResName))
                                utiResRefs.Add(res.ResName.Trim());
                        }
                    }
                }
                catch { }
            }
            var sortedRefs = utiResRefs.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

            var dialog = new Window
            {
                Title = Localization.Tr("Add Item from Template"),
                Width = 420,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = this
            };
            var resRef = "";
            var stackPanel = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            stackPanel.Children.Add(new TextBlock { Text = Localization.Tr("Select or enter item ResRef (UTI template):") });
            var combo = new ComboBox
            {
                MinWidth = 300,
                PlaceholderText = Localization.Tr("Select item..."),
                ItemsSource = sortedRefs
            };
            var textBox = new TextBox
            {
                Watermark = Localization.Tr("Or type ResRef manually (e.g. g_w_lghtsbr01)"),
                MinWidth = 300
            };
            combo.SelectionChanged += (sender, args) =>
            {
                if (combo.SelectedItem is string sel)
                    textBox.Text = sel;
            };
            stackPanel.Children.Add(combo);
            stackPanel.Children.Add(textBox);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            var okBtn = new Button { Content = Localization.Tr("Add"), Width = 80 };
            var cancelBtn = new Button { Content = Localization.Tr("Cancel"), Width = 80 };
            okBtn.Click += async (s, e) =>
            {
                resRef = (textBox?.Text ?? "").Trim();
                if (string.IsNullOrEmpty(resRef) && combo?.SelectedItem is string sel)
                    resRef = sel;
                dialog.Close(true);
            };
            cancelBtn.Click += (s, e) => dialog.Close(false);
            btnPanel.Children.Add(okBtn);
            btnPanel.Children.Add(cancelBtn);
            stackPanel.Children.Add(btnPanel);
            dialog.Content = stackPanel;

            var result = await dialog.ShowDialog<bool?>(this);
            if (result == true && !string.IsNullOrWhiteSpace(resRef))
            {
                SyncInventoryFromGrid();
                _inventoryItems.Add(new SaveInventoryItem { ResRef = resRef.Trim(), StackSize = 1, Charges = 0, MaxCharges = 0, UpgradeLevel = 0, NewItem = true });
                RefreshInventoryGrid();
                MarkDocumentDirty();
            }
        }

        private void RefreshInventoryGridFromRows(IEnumerable<InventoryGridRow> rows)
        {
            _inventoryItems.Clear();
            foreach (var row in rows ?? Enumerable.Empty<InventoryGridRow>())
            {
                int stack = 0, ch = 0, maxCh = 0, upgrades = 0;
                int.TryParse(row.StackSize.ToString(), out stack);
                var chStr = row.ChargesStr ?? "";
                if (chStr.Contains("/")) { var p = chStr.Split('/'); int.TryParse(p[0], out ch); int.TryParse(p.Length > 1 ? p[1] : "0", out maxCh); }
                else int.TryParse(chStr, out ch);
                int.TryParse(row.UpgradeLevel.ToString(), out upgrades);
                _inventoryItems.Add(new SaveInventoryItem
                {
                    ResRef = row.ResRef ?? "",
                    StackSize = stack,
                    Charges = ch,
                    MaxCharges = maxCh,
                    UpgradeLevel = upgrades,
                    NewItem = row.NewItem,
                    Upgrades = row.Upgrades,
                    UpgradeSlot0 = row.UpgradeSlot0,
                    UpgradeSlot1 = row.UpgradeSlot1,
                    UpgradeSlot2 = row.UpgradeSlot2,
                    UpgradeSlot3 = row.UpgradeSlot3,
                    UpgradeSlot4 = row.UpgradeSlot4,
                    UpgradeSlot5 = row.UpgradeSlot5
                });
            }
            RefreshInventoryGrid();
        }

        private void RemoveInventoryItem()
        {
            var rows = (_gridInventory?.ItemsSource as IEnumerable<InventoryGridRow>)?.ToList();
            if (rows == null) return;
            int idx = _gridInventory.SelectedIndex;
            if (idx < 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select an item to remove."), IconType.Info);
                return;
            }
            if (idx < rows.Count)
            {
                rows.RemoveAt(idx);
                RefreshInventoryGridFromRows(rows);
                MarkDocumentDirty();
            }
        }

        private void PopulateInventory()
        {
            _inventoryItems.Clear();
            if (_nestedCapsule?.InventoryGff == null) return;
            var root = _nestedCapsule.InventoryGff.Root;
            var list = root?.GetList("ItemList");
            if (list == null) return;
            foreach (var s in list)
            {
                var resref = s.Acquire<BioWare.Common.ResRef>("InventoryRes", BioWare.Common.ResRef.FromBlank());
                int stack = s.Acquire("StackSize", 1);
                int charges = s.Acquire("Charges", 0);
                int maxCharges = s.Acquire("MaxCharges", 0);
                int upgradeLevel = s.Acquire("UpgradeLevel", 0);
                bool newItem = s.Exists("NewItem") ? s.GetUInt8("NewItem") != 0 : false;
                int upgrades = s.Acquire("Upgrades", 0);
                int slot0 = s.Exists("UpgradeSlot0") ? s.Acquire("UpgradeSlot0", -1) : -1;
                int slot1 = s.Exists("UpgradeSlot1") ? s.Acquire("UpgradeSlot1", -1) : -1;
                int slot2 = s.Exists("UpgradeSlot2") ? s.Acquire("UpgradeSlot2", -1) : -1;
                int slot3 = s.Exists("UpgradeSlot3") ? s.Acquire("UpgradeSlot3", -1) : -1;
                int slot4 = s.Exists("UpgradeSlot4") ? s.Acquire("UpgradeSlot4", -1) : -1;
                int slot5 = s.Exists("UpgradeSlot5") ? s.Acquire("UpgradeSlot5", -1) : -1;
                _inventoryItems.Add(new SaveInventoryItem
                {
                    ResRef = resref?.ToString() ?? "",
                    StackSize = stack,
                    Charges = charges,
                    MaxCharges = maxCharges,
                    UpgradeLevel = upgradeLevel,
                    NewItem = newItem,
                    Upgrades = upgrades,
                    UpgradeSlot0 = slot0,
                    UpgradeSlot1 = slot1,
                    UpgradeSlot2 = slot2,
                    UpgradeSlot3 = slot3,
                    UpgradeSlot4 = slot4,
                    UpgradeSlot5 = slot5
                });
            }
            RefreshInventoryGrid();
        }

        private void UpdateInventoryFromUI()
        {
            if (_nestedCapsule?.InventoryGff == null) return;
            if (_gridInventory?.ItemsSource is IEnumerable<InventoryGridRow> rows)
            {
                var newItems = new List<SaveInventoryItem>();
                foreach (var row in rows)
                {
                    int stack, ch = 0, maxCh = 0, upgrades = 0;
                    int.TryParse(row.StackSize.ToString(), out stack);
                    var chStr = row.ChargesStr ?? "";
                    if (chStr.Contains("/")) { var p = chStr.Split('/'); int.TryParse(p[0], out ch); int.TryParse(p.Length > 1 ? p[1] : "0", out maxCh); }
                    else int.TryParse(chStr, out ch);
                    int.TryParse(row.UpgradeLevel.ToString(), out upgrades);
                    newItems.Add(new SaveInventoryItem
                    {
                        ResRef = row.ResRef ?? "",
                        StackSize = stack,
                        Charges = ch,
                        MaxCharges = maxCh,
                        UpgradeLevel = upgrades,
                        NewItem = row.NewItem,
                        Upgrades = row.Upgrades,
                        UpgradeSlot0 = row.UpgradeSlot0,
                        UpgradeSlot1 = row.UpgradeSlot1,
                        UpgradeSlot2 = row.UpgradeSlot2,
                        UpgradeSlot3 = row.UpgradeSlot3,
                        UpgradeSlot4 = row.UpgradeSlot4,
                        UpgradeSlot5 = row.UpgradeSlot5
                    });
                }
                var root = _nestedCapsule.InventoryGff.Root;
                var gffList = new GFFList();
                foreach (var item in newItems)
                {
                    var st = gffList.Add();
                    st.SetResRef("InventoryRes", BioWare.Common.ResRef.FromString(item.ResRef?.Trim() ?? ""));
                    st.SetInt32("StackSize", item.StackSize);
                    st.SetInt32("Charges", item.Charges);
                    st.SetInt32("MaxCharges", item.MaxCharges);
                    st.SetInt32("UpgradeLevel", item.UpgradeLevel);
                    st.SetUInt8("NewItem", item.NewItem ? (byte)1 : (byte)0);
                    st.SetUInt32("Upgrades", (uint)item.Upgrades);
                    st.SetInt32("UpgradeSlot0", item.UpgradeSlot0);
                    st.SetInt32("UpgradeSlot1", item.UpgradeSlot1);
                    st.SetInt32("UpgradeSlot2", item.UpgradeSlot2);
                    st.SetInt32("UpgradeSlot3", item.UpgradeSlot3);
                    st.SetInt32("UpgradeSlot4", item.UpgradeSlot4);
                    st.SetInt32("UpgradeSlot5", item.UpgradeSlot5);
                }
                root.SetList("ItemList", gffList);
                var bytes = new GFFBinaryWriter(_nestedCapsule.InventoryGff).Write();
                _nestedCapsule.SetResource(_nestedCapsule.InventoryIdentifier, bytes);
            }
        }

        private void ClearInventory()
        {
            _inventoryItems.Clear();
            _gridInventory.ItemsSource = null;
        }
        #endregion

        private class JournalGridRow
        {
            public string PlotId { get; set; }
            public string PlotIdRaw { get; set; }
            public string StateStr { get; set; }
            public string DateStr { get; set; }
            public string TimeStr { get; set; }
            public int Date { get; set; }
            public int Time { get; set; }
        }

        #region Journal
        private void PopulateJournal()
        {
            if (_partyTable?.JournalEntries == null) return;
            PopulateJournalPlotComboBox();
            _gridJournal.ItemsSource = _partyTable.JournalEntries.Select(j => new JournalGridRow
            {
                PlotId = GetPlotDisplayName(j.PlotId) ?? j.PlotId,
                PlotIdRaw = j.PlotId ?? "",
                StateStr = (j.State >= 0 ? j.State : 0).ToString(),
                Date = j.Date,
                Time = j.Time,
                DateStr = j.Date >= 0 ? "Day " + j.Date : "N/A",
                TimeStr = j.Time >= 0 ? $"{j.Time / 3600:D2}:{(j.Time % 3600) / 60:D2}:{j.Time % 60:D2} ({j.Time}s)" : "N/A"
            }).ToList();
        }

        private void AddJournalEntry()
        {
            if (_partyTable?.JournalEntries == null) return;
            string plotId = (_comboAddJournalPlot?.SelectedItem as JournalPlotItem)?.PlotId ?? GetFirstPlotIdFrom2DA() ?? "NEW_PLOT";
            if (_partyTable.JournalEntries.Any(j => string.Equals(j.PlotId, plotId, StringComparison.OrdinalIgnoreCase)))
            {
                DialogHelper.ShowWindow(this, Localization.Tr("Already Present"), Localization.Tr("This plot is already in the journal."), IconType.Info);
                return;
            }
            var last = _partyTable.JournalEntries.LastOrDefault();
            int date = last?.Date ?? 0;
            int time = (last?.Time ?? 0) + 1;
            _partyTable.JournalEntries.Add(new JournalEntry { PlotId = plotId, State = 0, Date = date, Time = time });
            PopulateJournal();
            PopulateJournalPlotComboBox();
            MarkDocumentDirty();
        }

        private string GetFirstPlotIdFrom2DA()
        {
            if (_installation == null) return null;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAPlot);
                if (twoda != null && twoda.GetHeight() > 0)
                {
                    var label = twoda.GetCellString(0, "label");
                    if (!string.IsNullOrWhiteSpace(label)) return label;
                }
            }
            catch { }
            return null;
        }

        private void PopulateJournalPlotComboBox()
        {
            _comboAddJournalPlot?.Items.Clear();
            if (_installation == null || _comboAddJournalPlot == null || _partyTable?.JournalEntries == null) return;
            try
            {
                var twoda = _installation.HtGetCache2DA(OdyInstallation.TwoDAPlot);
                if (twoda == null) return;
                var present = new HashSet<string>(_partyTable.JournalEntries.Select(j => j.PlotId?.Trim().ToLowerInvariant()).Where(s => !string.IsNullOrEmpty(s)));
                for (int i = 0; i < twoda.GetHeight(); i++)
                {
                    try
                    {
                        var label = twoda.GetCellString(i, "label");
                        if (string.IsNullOrWhiteSpace(label)) continue;
                        if (present.Contains(label.Trim().ToLowerInvariant())) continue;
                        var name = twoda.GetCellString(i, "name");
                        var display = !string.IsNullOrWhiteSpace(name) ? $"{name} ({label})" : label;
                        _comboAddJournalPlot.Items.Add(new JournalPlotItem { PlotId = label, Display = display });
                    }
                    catch { }
                }
                if (_comboAddJournalPlot.Items.Count > 0)
                    _comboAddJournalPlot.SelectedIndex = 0;
            }
            catch { }
        }

        private class JournalPlotItem
        {
            public string PlotId { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display ?? PlotId ?? "";
        }

        private void RemoveJournalEntry()
        {
            if (_partyTable?.JournalEntries == null) return;
            int idx = _gridJournal?.SelectedIndex ?? -1;
            if (idx < 0)
            {
                DialogHelper.ShowWindow(this, Localization.Tr("No Selection"), Localization.Tr("Select a journal entry to remove."), IconType.Info);
                return;
            }
            if (idx < _partyTable.JournalEntries.Count)
            {
                _partyTable.JournalEntries.RemoveAt(idx);
                PopulateJournal();
                PopulateJournalPlotComboBox();
                MarkDocumentDirty();
            }
        }

        private void UpdateJournalFromUI()
        {
            if (_partyTable?.JournalEntries == null) return;
            if (_gridJournal?.ItemsSource is IEnumerable<JournalGridRow> rows)
            {
                _partyTable.JournalEntries.Clear();
                foreach (var row in rows)
                {
                    int date = row.Date;
                    int time = row.Time;
                    if (!string.IsNullOrWhiteSpace(row.DateStr))
                    {
                        var dStr = row.DateStr.Replace("Day ", "").Replace("N/A", "0").Trim();
                        int.TryParse(dStr, out date);
                    }
                    if (!string.IsNullOrWhiteSpace(row.TimeStr))
                    {
                        var timeParts = row.TimeStr.Split('(', ')');
                        if (timeParts.Length >= 2 && int.TryParse(timeParts[1].Replace("s", "").Trim(), out var parsedTime))
                            time = parsedTime;
                    }
                    int state = 0;
                    int.TryParse(row.StateStr?.Trim(), out state);
                    _partyTable.JournalEntries.Add(new JournalEntry
                    {
                        PlotId = row.PlotIdRaw ?? row.PlotId ?? "",
                        State = state,
                        Date = date,
                        Time = time
                    });
                }
            }
        }

        private void ClearJournal()
        {
            _gridJournal.ItemsSource = null;
        }
        #endregion

        #region Cached Modules
        private void PopulateCachedModules()
        {
            var items = new List<TreeViewItem>();
            var root = new TreeViewItem { Header = Localization.Tr("Resources") };
            if (_nestedCapsule != null)
            {
                foreach (var kvp in _nestedCapsule.CachedModules)
                {
                    try
                    {
                        byte[] data = ERFAuto.BytesErf(kvp.Value);
                        var modItem = new TreeViewItem { Header = kvp.Key.ResName + " (module)" };
                        modItem.Tag = Tuple.Create(kvp.Key, data);
                        root.Items.Add(modItem);
                    }
                    catch { /* skip on serialization error */ }
                }
                foreach (var kvp in _nestedCapsule.CachedCharacters)
                {
                    var charItem = new TreeViewItem { Header = kvp.Key.ResName + " (character)" };
                    charItem.Tag = Tuple.Create(kvp.Key, kvp.Value);
                    root.Items.Add(charItem);
                }
            }
            items.Add(root);
            _treeCachedModules.ItemsSource = items;
        }

        private void OnCachedModuleDoubleTapped()
        {
            var sel = _treeCachedModules?.SelectedItem as TreeViewItem;
            if (sel?.Tag is Tuple<ResourceIdentifier, byte[]> tagData)
            {
                var ident = tagData.Item1;
                var data = tagData.Item2;
                if (ident != null && ident.ResType != null && data != null)
                {
                    WindowUtils.OpenResourceEditor(null, ident.ResName, ident.ResType, data, _installation, this);
                }
            }
        }

        private void ClearCachedModules()
        {
            _treeCachedModules.ItemsSource = null;
        }
        #endregion

        #region Area / Doors
        private void PopulateDoors()
        {
            _doorsGff = null;
            _doorsModuleErf = null;
            _doorsGitResName = null;
            _gridDoors.ItemsSource = null;
            if (_nestedCapsule == null || _saveInfo == null) return;
            var lastMod = _saveInfo.LastModule?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(lastMod)) return;
            var moduleKey = _nestedCapsule.CachedModules?.Keys.FirstOrDefault(k =>
                string.Equals(k?.ResName, lastMod, StringComparison.OrdinalIgnoreCase));
            if (moduleKey == null || !_nestedCapsule.CachedModules.TryGetValue(moduleKey, out var erf)) return;
            byte[] gitData = null;
            string gitResName = null;
            foreach (var res in erf)
            {
                if (res?.ResType == ResourceType.GIT)
                {
                    gitData = res.Data;
                    gitResName = res.ResRef?.ToString();
                    break;
                }
            }
            if (gitData == null || string.IsNullOrEmpty(gitResName)) return;
            try
            {
                var gff = GFF.FromBytes(gitData);
                var root = gff?.Root;
                var doorList = root?.GetList("Door List");
                if (doorList == null || doorList.Count == 0)
                {
                    _doorsGff = gff;
                    _doorsModuleErf = erf;
                    _doorsGitResName = gitResName;
                    _gridDoors.ItemsSource = new List<DoorGridRow>();
                    return;
                }
                var rows = new List<DoorGridRow>();
                foreach (GFFStruct doorStruct in doorList)
                {
                    var tag = doorStruct.GetString("Tag");
                    var locked = doorStruct.GetUInt8("Locked") != 0;
                    var openState = doorStruct.GetUInt8("OpenState");
                    if (openState > 2) openState = 0;
                    rows.Add(new DoorGridRow { DoorStruct = doorStruct, Tag = tag, Locked = locked, OpenStateStr = openState.ToString() });
                }
                _doorsGff = gff;
                _doorsModuleErf = erf;
                _doorsGitResName = gitResName;
                _gridDoors.ItemsSource = rows;
            }
            catch { }
        }

        private void UpdateDoorsFromUI()
        {
            if (_doorsGff == null || _doorsModuleErf == null) return;
            if (_gridDoors?.ItemsSource is IEnumerable<DoorGridRow> rows)
            {
                foreach (var row in rows)
                {
                    if (row?.DoorStruct == null) continue;
                    row.DoorStruct.SetUInt8("Locked", (byte)(row.Locked ? 1 : 0));
                    int openState = 0;
                    int.TryParse(row.OpenStateStr?.Trim(), out openState);
                    openState = Math.Max(0, Math.Min(2, openState));
                    row.DoorStruct.SetUInt8("OpenState", (byte)openState);
                }
            }
        }

        private void UpdateDoorsToModule()
        {
            if (_doorsGff == null || _doorsModuleErf == null || string.IsNullOrEmpty(_doorsGitResName)) return;
            try
            {
                var bytes = _doorsGff.ToBytes();
                _doorsModuleErf.SetData(_doorsGitResName, ResourceType.GIT, bytes);
            }
            catch { }
        }

        private void ClearDoors()
        {
            _doorsGff = null;
            _doorsModuleErf = null;
            _doorsGitResName = null;
            _gridDoors.ItemsSource = null;
        }
        #endregion

        #region Reputation
        private void PopulateReputation()
        {
            if (_nestedCapsule?.ReputeGff == null) return;
            var root = _nestedCapsule.ReputeGff.Root;
            var list = root?.GetList("FactionList");
            if (list == null) return;
            var rows = new List<dynamic>();
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                var name = s.Acquire("FactionName", $"Faction {i}");
                var val = s.Acquire("Reputation", 50);
                rows.Add(new { Name = name.ToString(), Value = val.ToString() });
            }
            _gridReputation.ItemsSource = rows;
        }

        private void UpdateReputationFromUI()
        {
            if (_nestedCapsule?.ReputeGff == null) return;
            if (_gridReputation?.ItemsSource is IEnumerable<dynamic> rows)
            {
                var root = _nestedCapsule.ReputeGff.Root;
                var list = root?.GetList("FactionList");
                if (list == null) return;
                int i = 0;
                foreach (var row in rows)
                {
                    if (i >= list.Count) break;
                    int val;
                    if (int.TryParse(row.Value?.ToString(), out val))
                        list[i].SetInt32("Reputation", val);
                    i++;
                }
                var bytes = new GFFBinaryWriter(_nestedCapsule.ReputeGff).Write();
                _nestedCapsule.SetResource(_nestedCapsule.ReputeIdentifier, bytes);
            }
        }

        private void ClearReputation()
        {
            _gridReputation.ItemsSource = null;
        }
        #endregion

        #region Advanced
        private void PopulateAdvancedFields()
        {
            _listAdvancedResources?.Items.Clear();
            if (_nestedCapsule?.ResourceData == null) return;
            var known = new HashSet<ResourceIdentifier>();
            foreach (var k in _nestedCapsule.CachedModules.Keys) known.Add(k);
            foreach (var k in _nestedCapsule.CachedCharacters.Keys) known.Add(k);
            foreach (var k in _nestedCapsule.CachedRimModules.Keys) known.Add(k);
            if (_nestedCapsule.InventoryIdentifier != null) known.Add(_nestedCapsule.InventoryIdentifier);
            if (_nestedCapsule.ReputeIdentifier != null) known.Add(_nestedCapsule.ReputeIdentifier);
            var others = _nestedCapsule.ResourceData
                .Where(kvp => !known.Contains(kvp.Key))
                .OrderBy(kvp => kvp.Key.ResName?.ToLowerInvariant() ?? "")
                .Select(kvp => new AdvancedResourceItem
                {
                    Display = $"{kvp.Key.ResName} ({kvp.Key.ResType})",
                    Ident = kvp.Key,
                    Data = kvp.Value
                })
                .ToList();
            foreach (var item in others)
                _listAdvancedResources.Items.Add(item);
        }

        private void ClearAdvancedFields()
        {
            _listAdvancedResources?.Items.Clear();
        }

        private void OnAdvancedResourceDoubleTapped()
        {
            var sel = _listAdvancedResources?.SelectedItem as AdvancedResourceItem;
            if (sel?.Ident != null && sel.Data != null && sel.Ident.ResType != null)
            {
                WindowUtils.OpenResourceEditor(null, sel.Ident.ResName, sel.Ident.ResType, sel.Data, _installation, this);
            }
        }
        #endregion

        public SaveInfo SaveInfo => _saveInfo;
        public PartyTable PartyTable => _partyTable;
        public GlobalVars GlobalVars => _globalVars;
        public SaveNestedCapsule NestedCapsule => _nestedCapsule;

        internal void SetSaveInfoForTesting(SaveInfo saveInfo)
        {
            _saveInfo = saveInfo;
            PopulateSaveInfo();
            ClearDirty();
        }

        internal void SetPartyTableForTesting(PartyTable partyTable)
        {
            _partyTable = partyTable;
            PopulatePartyTable();
            ClearDirty();
        }

        internal void SetGlobalVarsForTesting(GlobalVars globalVars)
        {
            _globalVars = globalVars;
            PopulateGlobalVars();
            ClearDirty();
        }

        internal void AddGlobalBoolForTest(string name, bool value)
        {
            AddGlobalVarRow(_gridBooleans, "bools");
            var rows = (_gridBooleans.ItemsSource as IEnumerable<GlobalBoolRow>)?.ToList() ?? new List<GlobalBoolRow>();
            if (rows.Count == 0) return;
            rows[rows.Count - 1].Name = name;
            rows[rows.Count - 1].Value = value;
            _gridBooleans.ItemsSource = rows;
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        internal string AddDefaultGlobalBoolForTest()
        {
            AddGlobalVarRow(_gridBooleans, "bools");
            var rows = (_gridBooleans.ItemsSource as IEnumerable<GlobalBoolRow>)?.ToList() ?? new List<GlobalBoolRow>();
            return rows.LastOrDefault()?.Name;
        }

        internal void AddGlobalNumberForTest(string name, int value)
        {
            AddGlobalVarRow(_gridNumbers, "numbers");
            var rows = (_gridNumbers.ItemsSource as IEnumerable<GlobalNumberRow>)?.ToList() ?? new List<GlobalNumberRow>();
            if (rows.Count == 0) return;
            rows[rows.Count - 1].Name = name;
            rows[rows.Count - 1].Value = value;
            _gridNumbers.ItemsSource = rows;
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        internal void AddGlobalStringForTest(string name, string value)
        {
            AddGlobalVarRow(_gridStrings, "strings");
            var rows = (_gridStrings.ItemsSource as IEnumerable<GlobalStringRow>)?.ToList() ?? new List<GlobalStringRow>();
            if (rows.Count == 0) return;
            rows[rows.Count - 1].Name = name;
            rows[rows.Count - 1].Value = value;
            _gridStrings.ItemsSource = rows;
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        internal void AddGlobalLocationForTest(string name, Vector4 value)
        {
            AddGlobalVarRow(_gridLocations, "locations");
            var rows = (_gridLocations.ItemsSource as IEnumerable<GlobalLocationRow>)?.ToList() ?? new List<GlobalLocationRow>();
            if (rows.Count == 0) return;
            rows[rows.Count - 1].Name = name;
            rows[rows.Count - 1].X = value.X;
            rows[rows.Count - 1].Y = value.Y;
            rows[rows.Count - 1].Z = value.Z;
            rows[rows.Count - 1].Orientation = value.W;
            _gridLocations.ItemsSource = rows;
            SyncGlobalVarsFromGridAndMarkDirty();
        }

        internal void RemoveGlobalBoolAtForTest(int index)
        {
            _gridBooleans.SelectedIndex = index;
            RemoveGlobalVarRow(_gridBooleans);
        }

        internal void RemoveGlobalNumberAtForTest(int index)
        {
            _gridNumbers.SelectedIndex = index;
            RemoveGlobalVarRow(_gridNumbers);
        }

        internal void RemoveGlobalStringAtForTest(int index)
        {
            _gridStrings.SelectedIndex = index;
            RemoveGlobalVarRow(_gridStrings);
        }

        internal void RemoveGlobalLocationAtForTest(int index)
        {
            _gridLocations.SelectedIndex = index;
            RemoveGlobalVarRow(_gridLocations);
        }


        internal TextBox SaveNameEditForTest => _lineEditSaveName;
        internal TextBox AreaNameEditForTest => _lineEditAreaName;
        internal TextBox LastModuleEditForTest => _lineEditLastModule;
        internal NumericUpDown TimePlayedSpinForTest => _spinBoxTimePlayed;
        internal CheckBox CheatUsedCheckForTest => _checkBoxCheatUsed;
        internal NumericUpDown PartyGoldSpinForTest => _spinBoxGold;
        internal NumericUpDown PartyXpPoolSpinForTest => _spinBoxXPPool;
        internal NumericUpDown PartyComponentsSpinForTest => _spinBoxComponents;
        internal NumericUpDown PartyChemicalsSpinForTest => _spinBoxChemicals;
        internal CheckBox PartyCheatUsedCheckForTest => _checkBoxCheatUsedPT;
        internal CheckBox PartySoloModeCheckForTest => _checkBoxSoloMode;
        internal bool HasProgrammaticEditorSurfaceForTest =>
            _tabControl != null &&
            _tabControl.Items.Count >= 10 &&
            _lineEditSaveName != null &&
            _lineEditAreaName != null &&
            _spinBoxTimePlayed != null &&
            _checkBoxCheatUsed != null &&
            _spinBoxGold != null &&
            _gridBooleans != null &&
            _listWidgetCharacters != null &&
            _gridInventory != null &&
            _treeCachedModules != null &&
            _listAdvancedResources != null;

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = Localization.Tr("Select save folder"), AllowMultiple = false });
            if (folders == null || folders.Count == 0) return;
            string path = folders[0].Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            if (_saveFolder == null) return;
            try
            {
                foreach (var file in new[] { "savenfo.res", "partytable.res", "globalvars.res", "savegame.sav", "screen.tga" })
                {
                    var src = Path.Combine(_saveFolder.FolderPath, file);
                    var dst = Path.Combine(path, file);
                    if (File.Exists(src)) File.Copy(src, dst, true);
                }
                _filepath = path;
                _saveFolder = new SaveFolderEntry(path);
                _saveFolder.Load();
                _saveInfo = _saveFolder.SaveInfo;
                _partyTable = _saveFolder.PartyTable;
                _globalVars = _saveFolder.GlobalVars;
                _nestedCapsule = _saveFolder.NestedCapsule;
                _resname = Path.GetFileName(path);
                RefreshWindowTitle();
                PopulateSaveInfo();
                PopulatePartyTable();
                PopulateGlobalVars();
                PopulateCharacters();
                PopulateInventory();
                PopulateJournal();
                PopulateCachedModules();
                PopulateDoors();
                PopulateReputation();
                PopulateScreenshot();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowErrorFromException(this, ex);
            }
        }
    }
}
