using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AmColor = Avalonia.Media.Color;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Common.Logger;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using BioWare.Resource.Formats.GFF.Generics.DLG.IO;
using BioWare.Resource.Formats.TwoDA;
using DLGType = BioWare.Resource.Formats.GFF.Generics.DLG.DLG;
using DLGHelper = BioWare.Resource.Formats.GFF.Generics.DLG.DLGHelper;
using CNVHelper = BioWare.Resource.Formats.GFF.Generics.CNV.CNVHelper;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Widgets;
using OdyTools.Widgets.Edit;
using OdyTools.Dialogs.Edit;
using OdyTools.Editors;
using OdyTools.Editors.Actions;
using Avalonia.Controls.Documents;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using Avalonia.Controls.Primitives;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Editors.DLG
{
    // DLG (Dialogue) format is Aurora Engine format used by:
    // - Neverwinter Nights: Enhanced Edition (Aurora) - nwmain.exe: Uses base DLG format
    // - KotOR 1 (Odyssey) - k1_win_gog_swkotor.exe: Uses base DLG format
    // - KotOR 2 (Odyssey) - k2_win_gog_aspyr_swkotor2.exe: Uses extended DLG format with K2-specific fields
    //   K2-specific root fields: AlienRaceOwner, PostProcOwner, RecordNoVO, NextNodeID
    //   K2-specific node fields: ActionParam1-5, Script2, AlienRaceNode, NodeID, Emotion, FacialAnim, etc.
    //   K2-specific link fields: Active2, Logic, Not, Not2, Param1-5, ParamStrA/B, etc.
    // - Eclipse Engine games (Dragon Age Origins, Dragon Age 2, Mass Effect 1/2):
    //   Eclipse games primarily use .cnv "conversation" format, but DLG files follow K1-style base format
    //   (no K2-specific fields). This editor supports both DLG and CNV files for Eclipse games.
    //   CNV files are automatically converted to DLG for editing, and can be saved back as CNV.
    //   Ghidra analysis: daorigins.exe, DragonAge2.exe, MassEffect.exe use "conversation" strings
    public partial class OdyToolDLG : Editor
    {
        private DLGType _coreDlg;
        private DLGModel _model;
        private DLGActionHistory _actionHistory;

        private HashSet<Key> _keysDown = new HashSet<Key>();

        private DLGLink _copy;

        /// <summary>
        /// Sets the copy link (for internal use and testing).
        /// </summary>
        internal void SetCopyLink(DLGLink link)
        {
            _copy = link;
        }

        /// <summary>
        /// Gets the copy link (for testing).
        /// </summary>
        public DLGLink GetCopyLink()
        {
            return _copy;
        }

        // UI Controls - Animations
        private ListBox _animsList;
        private Button _addAnimButton;
        private Button _removeAnimButton;
        private Button _editAnimButton;
        private ListBox _stuntList;
        private Button _addStuntButton;
        private Button _editStuntButton;
        private Button _removeStuntButton;

        // Public property for testing
        public ListBox StuntList => _stuntList;

        // Public properties for menu actions (for testing)
        public MenuItem ActionReloadTree => _actionReloadTree;
        private TextBox _commentsEdit;
        private Border _nodeTextPreviewBorder;
        private TextBlock _nodeTextPreviewLabel;
        private Control _leftDockWidget;
        private Control _topDockWidget;
        private Control _rightDockWidget;
        private Control _dockWidget; // bottom
        private Border _topDockContent;
        private Border _leftDockContent;
        private Border _rightDockContent;
        private Border _bottomDockContent;
        private readonly WindowHolder _topDockFloatWindow = new WindowHolder();
        private readonly WindowHolder _leftDockFloatWindow = new WindowHolder();
        private readonly WindowHolder _rightDockFloatWindow = new WindowHolder();
        private readonly WindowHolder _bottomDockFloatWindow = new WindowHolder();
        private DLGListWidget _orphanedNodesList;
        private DLGListWidget _pinnedItemsList;

        // Menu Actions - matching PyKotor UI actions
        // File menu actions
        private MenuItem _actionNew;
        private MenuItem _actionOpen;
        private MenuItem _actionSave;
        private MenuItem _actionSaveAs;
        private MenuItem _actionRevert;
        private MenuItem _actionDLGSettings;
        private MenuItem _actionExit;

        // Edit menu actions (undo/redo)
        private MenuItem _actionUndo;
        private MenuItem _actionRedo;

        // Tools menu actions
        private MenuItem _actionFind;
        private MenuItem _actionReloadTree;
        private MenuItem _actionUnfocus;
        private MenuItem _viewFileGlobals;
        private MenuItem _viewNodeFields;
        private MenuItem _viewOrphanedNodes;
        private MenuItem _viewJournalNode;

        private int _currentResultIndex = 0;
        private List<DLGStandardItem> _searchResults = new List<DLGStandardItem>();
        private string _currentSearchText = "";

        private const string DlgMimeFormat = "application/x-odytools-dlg-mime";
        private static readonly DataFormat<string> DlgMimeDataFormat = DataFormat.CreateStringPlatformFormat(DlgMimeFormat);

        // Search UI Controls
        private Panel _findBar;
        private TextBox _findInput;
        private Button _findButton;
        private Button _backButton;
        private TextBlock _resultsLabel;
        private Panel _findSuggestionsPanel;
        private Panel _goToBar;
        private TextBox _goToInput;
        private Button _goToButton;
        private List<string> _findSuggestions = new List<string>();
        private TextBlock _tipLabel;
        private DispatcherTimer _statusBarTipTimer;
        private static readonly string[] _tips = new[]
        {
            "Use the 'View' and 'Settings' menu to customize dlg editor settings. All of your changes will be saved for next time you load the editor.",
            "Tip: Drag and Drop is supported, even between different DLGs!",
            "Tip: Accidentally closed a side widget? Right click the Menu to reopen the dock panels.",
            "Tip: Hold CTRL and scroll to change the text size.",
            "Tip: Hold ALT and scroll to change the indentation.",
            "Tip: Hold CTRL+SHIFT and scroll to change the vertical spacing.",
            "Tip: 'Delete all references' will delete all EntriesList/RepliesList/StartingList links to the node, leaving it orphaned.",
            "Tip: Drag any item to the left dockpanel to pin it for easy access",
            "Tip: Orphaned Nodes will automatically be added to the top left list, drag back in to reintegrate.",
            "Tip: Use ':' after an attribute name in the search bar to filter items by specific properties, e.g., 'is_child:1'.",
            "Tip: Combine keywords with AND/OR in the search bar to refine your search results, such as 'script1:k_swg AND listener:PLAYER'",
            "Tip: Use double quotes to search for exact phrases in item descriptions, such as '\"urgent task\"'.",
            "Tip: Search for attributes without a value after ':' to find items where any non-null property exists, e.g., 'assigned:'.",
            "Tip: Double-click me to view all tips."
        };
        private Point? _dragStartPosition;
        private bool _dragStarted;

        private sealed class FlatNodeRow
        {
            public DLGLink Link { get; set; }
            public DLGNode Node { get; set; }
            public string Display { get; set; }
            public Control Visual { get; set; }
            public override string ToString() => Display ?? string.Empty;
        }

        // UI Controls - Link widgets
        private ComboBox _condition1ResrefEdit;
        private ComboBox _condition2ResrefEdit;
        private NumericUpDown _logicSpin;
        private TreeView _dialogTree;
        private TabControl _dialogViewTabs;
        private ListBox _flatStartingList;
        private ListBox _flatEntryList;
        private ListBox _flatReplyList;
        private Button _graphFitButton;
        private Button _graphAutoLayoutButton;
        private Button _graphZoomInButton;
        private Button _graphZoomOutButton;
        private TextBlock _graphZoomLabel;
        private TextBlock _graphStatusText;
        private Panel _graphCanvas;
        private DlgGraphScene _graphScene;
        private bool _isSyncingViewSelection;
        private bool _suppressTreeSelectionHandler;
        private DLGLink _selectedLink;
        private Dictionary<string, Point> _graphManualPositions = new Dictionary<string, Point>(StringComparer.OrdinalIgnoreCase);

        // Condition parameter widgets (K2-specific, but available in UI for all games)
        private NumericUpDown _condition1Param1Spin;
        private NumericUpDown _condition1Param2Spin;
        private NumericUpDown _condition1Param3Spin;
        private NumericUpDown _condition1Param4Spin;
        private NumericUpDown _condition1Param5Spin;
        private TextBox _condition1Param6Edit;
        private CheckBox _condition1NotCheckbox;
        private NumericUpDown _condition2Param1Spin;
        private NumericUpDown _condition2Param2Spin;
        private NumericUpDown _condition2Param3Spin;
        private NumericUpDown _condition2Param4Spin;
        private NumericUpDown _condition2Param5Spin;
        private TextBox _condition2Param6Edit;
        private CheckBox _condition2NotCheckbox;

        // UI Controls - Node widgets (Quest/Plot)
        private TextBox _questEdit;
        private NumericUpDown _questEntrySpin;
        private ComboBox2DA _plotIndexCombo;
        private NumericUpDown _plotXpSpin;

        // UI Controls - Speaker widgets
        private TextBox _speakerEdit;
        private TextBlock _speakerEditLabel;

        // UI Controls - Listener widget
        private TextBox _listenerEdit;

        // UI Controls - Script widgets
        private ComboBox _script1ResrefEdit;
        private ComboBox _script2ResrefEdit;
        private NumericUpDown _script1Param1Spin;
        private NumericUpDown _script1Param2Spin;
        private NumericUpDown _script1Param3Spin;
        private NumericUpDown _script1Param4Spin;
        private NumericUpDown _script1Param5Spin;
        private StackPanel _script1Param1Panel;
        private StackPanel _script2Panel;
        private StackPanel _condition2Panel;
        private Control _emotionExpressionPanel;
        private Control _nodeIdLabel;
        private Control _alienRaceNodeLabel;
        private Control _postProcLabel;
        private Control _logicLabel;
        private NumericUpDown _script2Param1Spin;
        private NumericUpDown _script2Param2Spin;
        private NumericUpDown _script2Param3Spin;
        private NumericUpDown _script2Param4Spin;
        private NumericUpDown _script2Param5Spin;
        private TextBox _script1Param6Edit;
        private TextBox _script2Param6Edit;
        private NumericUpDown _waitFlagSpin;
        private NumericUpDown _fadeTypeSpin;
        private ComboBox _soundComboBox;
        private ComboBox _voiceComboBox;
        private Button _soundButton;
        private Button _voiceButton;

        // UI Controls - Node timing widgets
        private NumericUpDown _delaySpin;
        private TextBox _voIdEdit;

        // UI Controls - Camera widgets
        private NumericUpDown _cameraIdSpin;
        private NumericUpDown _cameraAnimSpin;
        private NumericUpDown _nodeIdSpin;
        private NumericUpDown _alienRaceNodeSpin;
        private NumericUpDown _postProcSpin;
        private ComboBox _cameraAngleSelect;
        private ComboBox2DA _cameraEffectSelect;
        private ComboBox2DA _emotionSelect;
        private ComboBox2DA _expressionSelect;
        private ComboBox _ambientTrackCombo;

        // UI Controls - File-level checkboxes
        private CheckBox _unequipHandsCheckbox;
        private CheckBox _unequipAllCheckbox;
        private CheckBox _skippableCheckbox;
        private CheckBox _animatedCutCheckbox;
        private CheckBox _oldHitCheckbox;
        private CheckBox _soundCheckbox;
        private CheckBox _nodeUnskippableCheckbox;

        // UI Controls - File-level properties (conversation type, computer type, delays, scripts, camera)
        private ComboBox _conversationSelect;
        private ComboBox _computerSelect;
        private NumericUpDown _entryDelaySpin;
        private NumericUpDown _replyDelaySpin;
        private ComboBox _onAbortCombo;
        private ComboBox _onEndEdit;
        private ComboBox _cameraModelSelect;

        // Flag to track if node is loaded into UI (prevents updates during loading)
        private bool _nodeLoadedIntoUi = false;

        /// <summary>DLGStandardItem for the node currently shown in the right panel (Node Fields). Used by Add/Remove/Edit Animation when tree selection is unavailable.</summary>
        private DLGStandardItem _currentNodeItem = null;

        // Flag to track if editor is in focus mode (showing only a specific node and its children)
        private bool _focused = false;

        // Cross-platform sound player for WAV playback (replaces Windows-only SoundPlayer)
        private NAudioMediaPlayer _soundPlayer;

        // Reference history for navigation
        private ReferenceChooserDialog _dialogReferences;
        private List<Tuple<List<WeakReference<DLGLink>>, string>> _referenceHistory = new List<Tuple<List<WeakReference<DLGLink>>, string>>();
        private int _currentReferenceIndex = -1;

        /// <summary>Parameterless constructor for Avalonia XAML runtime loader (e.g. standalone avares).</summary>
        public OdyToolDLG() : this(null, null) { }

        public OdyToolDLG(Window parent = null, OdyInstallation installation = null)
            : base(parent, Localization.Tr("DLG Editor"), "dialog",
                new[] { ResourceType.DLG, ResourceType.CNV },
                new[] { ResourceType.DLG, ResourceType.CNV },
                installation)
        {
            _coreDlg = new DLGType();
            _model = new DLGModel(this);
            _actionHistory = new DLGActionHistory(this);
            _soundPlayer = new NAudioMediaPlayer();
            InitializeComponent();
            ApplyInstallationFromDLGSettings();
            UpdateUIForGame(); // Update UI visibility based on game type
            UpdateTreeView();
            New();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            if (EnsureBindingsFromXaml())
            {
                WireEventHandlersFromXaml();
                SetupStatusBarTips();
                InitializeCameraWidgets();
                SetupTslEmotionsAndExpressions();
            }
            else
            {
                SetupUI();
                SetupStatusBarTips();
                InitializeCameraWidgets();
                SetupTslEmotionsAndExpressions();
            }
        }

        /// <summary>
        /// Binds to controls defined in OdyToolDLG.axaml (matching PyKotor dlg.ui names). Returns true if XAML content was found.
        /// </summary>
        private bool EnsureBindingsFromXaml()
        {
            _dialogTree = EditorHelpers.FindControlSafe<TreeView>(this, "dialogTree");
            if (_dialogTree == null)
                return false;
            _dialogViewTabs = EditorHelpers.FindControlSafe<TabControl>(this, "dialogViewTabs");
            _flatStartingList = EditorHelpers.FindControlSafe<ListBox>(this, "flatStartingList");
            _flatEntryList = EditorHelpers.FindControlSafe<ListBox>(this, "flatEntryList");
            _flatReplyList = EditorHelpers.FindControlSafe<ListBox>(this, "flatReplyList");
            _graphFitButton = EditorHelpers.FindControlSafe<Button>(this, "graphFitButton");
            _graphAutoLayoutButton = EditorHelpers.FindControlSafe<Button>(this, "graphAutoLayoutButton");
            _graphZoomInButton = EditorHelpers.FindControlSafe<Button>(this, "graphZoomInButton");
            _graphZoomOutButton = EditorHelpers.FindControlSafe<Button>(this, "graphZoomOutButton");
            _graphZoomLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "graphZoomLabel");
            _graphStatusText = EditorHelpers.FindControlSafe<TextBlock>(this, "graphStatusText");
            _graphCanvas = EditorHelpers.FindControlSafe<Panel>(this, "graphCanvas");

            _findBar = EditorHelpers.FindControlSafe<Panel>(this, "findBar");
            _findInput = EditorHelpers.FindControlSafe<TextBox>(this, "findInput");
            _goToBar = EditorHelpers.FindControlSafe<Panel>(this, "goToBar");
            _goToInput = EditorHelpers.FindControlSafe<TextBox>(this, "goToInput");
            _goToButton = EditorHelpers.FindControlSafe<Button>(this, "goToButton");
            _findButton = EditorHelpers.FindControlSafe<Button>(this, "findButton");
            _backButton = EditorHelpers.FindControlSafe<Button>(this, "backButton");
            _resultsLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "resultsLabel");
            _findSuggestionsPanel = EditorHelpers.FindControlSafe<Panel>(this, "findSuggestionsPanel");
            _tipLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "tipLabel");

            _leftDockWidget = EditorHelpers.FindControlSafe<Control>(this, "leftDockWidget");
            _topDockWidget = EditorHelpers.FindControlSafe<Control>(this, "topDockWidget");
            _rightDockWidget = EditorHelpers.FindControlSafe<Control>(this, "rightDockWidget");
            _dockWidget = EditorHelpers.FindControlSafe<Control>(this, "dockWidget");
            _topDockContent = EditorHelpers.FindControlSafe<Border>(this, "topDockContent");
            _leftDockContent = EditorHelpers.FindControlSafe<Border>(this, "leftDockContent");
            _rightDockContent = EditorHelpers.FindControlSafe<Border>(this, "rightDockContent");
            _bottomDockContent = EditorHelpers.FindControlSafe<Border>(this, "bottomDockContent");
            _orphanedNodesList = EditorHelpers.FindControlSafe<DLGListWidget>(this, "orphanedNodesList");
            _pinnedItemsList = EditorHelpers.FindControlSafe<DLGListWidget>(this, "pinnedItemsList");
            if (_orphanedNodesList != null)
            {
                _orphanedNodesList.Editor = this;
                _orphanedNodesList.UseHoverText = false;
                _orphanedNodesList.UseWordWrap = true;
            }
            if (_pinnedItemsList != null)
            {
                _pinnedItemsList.Editor = this;
                _pinnedItemsList.SelectionMode = SelectionMode.Multiple;
                _pinnedItemsList.UseWordWrap = true;
            }

            _actionNew = EditorHelpers.FindControlSafe<MenuItem>(this, "actionNew");
            _actionOpen = EditorHelpers.FindControlSafe<MenuItem>(this, "actionOpen");
            _actionSave = EditorHelpers.FindControlSafe<MenuItem>(this, "actionSave");
            _actionSaveAs = EditorHelpers.FindControlSafe<MenuItem>(this, "actionSaveAs");
            _actionRevert = EditorHelpers.FindControlSafe<MenuItem>(this, "actionRevert");
            _actionDLGSettings = EditorHelpers.FindControlSafe<MenuItem>(this, "actionDLGSettings");
            _actionExit = EditorHelpers.FindControlSafe<MenuItem>(this, "actionExit");
            _actionUndo = EditorHelpers.FindControlSafe<MenuItem>(this, "actionUndo");
            _actionRedo = EditorHelpers.FindControlSafe<MenuItem>(this, "actionRedo");
            _actionFind = EditorHelpers.FindControlSafe<MenuItem>(this, "actionFind");
            _actionReloadTree = EditorHelpers.FindControlSafe<MenuItem>(this, "actionReloadTree");
            _actionUnfocus = EditorHelpers.FindControlSafe<MenuItem>(this, "actionUnfocus");
            _viewFileGlobals = EditorHelpers.FindControlSafe<MenuItem>(this, "viewFileGlobals");
            _viewNodeFields = EditorHelpers.FindControlSafe<MenuItem>(this, "viewNodeFields");
            _viewOrphanedNodes = EditorHelpers.FindControlSafe<MenuItem>(this, "viewOrphanedNodes");
            _viewJournalNode = EditorHelpers.FindControlSafe<MenuItem>(this, "viewJournalNode");

            _voIdEdit = EditorHelpers.FindControlSafe<TextBox>(this, "voIdEdit");
            _ambientTrackCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "ambientTrackCombo");
            _conversationSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "conversationSelect");
            _computerSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "computerSelect");
            _entryDelaySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "entryDelaySpin");
            _replyDelaySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "replyDelaySpin");
            _onAbortCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "onAbortCombo");
            _onEndEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "onEndEdit");
            _cameraModelSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "cameraModelSelect");
            _unequipHandsCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "unequipHandsCheckbox");
            _unequipAllCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "unequipAllCheckbox");
            _skippableCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "skippableCheckbox");
            _animatedCutCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "animatedCutCheckbox");
            _oldHitCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "oldHitCheckbox");
            _stuntList = EditorHelpers.FindControlSafe<ListBox>(this, "stuntList");
            _addStuntButton = EditorHelpers.FindControlSafe<Button>(this, "addStuntButton");
            _editStuntButton = EditorHelpers.FindControlSafe<Button>(this, "editStuntButton");
            _removeStuntButton = EditorHelpers.FindControlSafe<Button>(this, "removeStuntButton");

            _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");
            _nodeTextPreviewBorder = EditorHelpers.FindControlSafe<Border>(this, "nodeTextPreviewBorder");
            _nodeTextPreviewLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "nodeTextPreviewLabel");
            _script1ResrefEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "script1ResrefEdit");
            _script1Param1Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param1Spin");
            _script1Param2Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param2Spin");
            _script1Param3Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param3Spin");
            _script1Param4Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param4Spin");
            _script1Param5Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param5Spin");
            _script1Param1Panel = EditorHelpers.FindControlSafe<StackPanel>(this, "script1Param1Panel");
            _script2Panel = EditorHelpers.FindControlSafe<StackPanel>(this, "script2Panel");
            _condition2Panel = EditorHelpers.FindControlSafe<StackPanel>(this, "condition2Panel");
            _emotionExpressionPanel = EditorHelpers.FindControlSafe<Control>(this, "emotionExpressionPanel");
            _nodeIdLabel = EditorHelpers.FindControlSafe<Control>(this, "nodeIdLabel");
            _alienRaceNodeLabel = EditorHelpers.FindControlSafe<Control>(this, "alienRaceNodeLabel");
            _postProcLabel = EditorHelpers.FindControlSafe<Control>(this, "postProcLabel");
            _logicLabel = EditorHelpers.FindControlSafe<Control>(this, "logicLabel");
            _script2ResrefEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "script2ResrefEdit");
            _script2Param1Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script2Param1Spin");
            _script2Param2Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script2Param2Spin");
            _script2Param3Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script2Param3Spin");
            _script2Param4Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script2Param4Spin");
            _script2Param5Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "script2Param5Spin");
            _script1Param6Edit = EditorHelpers.FindControlSafe<TextBox>(this, "script1Param6Edit");
            _script2Param6Edit = EditorHelpers.FindControlSafe<TextBox>(this, "script2Param6Edit");
            _condition1ResrefEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "condition1ResrefEdit");
            _condition1NotCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "condition1NotCheckbox");
            _condition1Param1Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition1Param1Spin");
            _condition1Param2Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition1Param2Spin");
            _condition1Param3Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition1Param3Spin");
            _condition1Param4Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition1Param4Spin");
            _condition1Param5Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition1Param5Spin");
            _condition1Param6Edit = EditorHelpers.FindControlSafe<TextBox>(this, "condition1Param6Edit");
            _condition2ResrefEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "condition2ResrefEdit");
            _condition2NotCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "condition2NotCheckbox");
            _condition2Param1Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition2Param1Spin");
            _condition2Param2Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition2Param2Spin");
            _condition2Param3Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition2Param3Spin");
            _condition2Param4Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition2Param4Spin");
            _condition2Param5Spin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "condition2Param5Spin");
            _condition2Param6Edit = EditorHelpers.FindControlSafe<TextBox>(this, "condition2Param6Edit");
            _animsList = EditorHelpers.FindControlSafe<ListBox>(this, "animsList");
            _addAnimButton = EditorHelpers.FindControlSafe<Button>(this, "addAnimButton");
            _removeAnimButton = EditorHelpers.FindControlSafe<Button>(this, "removeAnimButton");
            _editAnimButton = EditorHelpers.FindControlSafe<Button>(this, "editAnimButton");
            _emotionSelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "emotionSelect");
            _expressionSelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "expressionSelect");
            _soundCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "soundCheckbox");
            _soundComboBox = EditorHelpers.FindControlSafe<ComboBox>(this, "soundComboBox");
            _voiceComboBox = EditorHelpers.FindControlSafe<ComboBox>(this, "voiceComboBox");
            _soundButton = EditorHelpers.FindControlSafe<Button>(this, "soundButton");
            _voiceButton = EditorHelpers.FindControlSafe<Button>(this, "voiceButton");
            _cameraIdSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "cameraIdSpin");
            _cameraAnimSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "cameraAnimSpin");
            _cameraAngleSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "cameraAngleSelect");
            _cameraEffectSelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "cameraEffectSelect");
            _nodeUnskippableCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "nodeUnskippableCheckbox");
            _nodeIdSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "nodeIdSpin");
            _alienRaceNodeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "alienRaceNodeSpin");
            _postProcSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "postProcSpin");
            _delaySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "delaySpin");
            _logicSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "logicSpin");
            _waitFlagSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "waitFlagSpin");
            _fadeTypeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "fadeTypeSpin");
            _speakerEdit = EditorHelpers.FindControlSafe<TextBox>(this, "speakerEdit");
            _speakerEditLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "speakerEditLabel");
            _listenerEdit = EditorHelpers.FindControlSafe<TextBox>(this, "listenerEdit");
            _questEdit = EditorHelpers.FindControlSafe<TextBox>(this, "questEdit");
            _questEntrySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "questEntrySpin");
            _plotIndexCombo = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "plotIndexCombo");
            _plotXpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "plotXpSpin");
            return true;
        }
        private static readonly string[] allCameraEffects = new[] { "None (0)", "Effect 1", "Effect 2", "Effect 3" };

        /// <summary>
        /// Wires event handlers when UI is loaded from XAML. Does not create controls.
        /// </summary>
        private void WireEventHandlersFromXaml()
        {
            SetupMenuActionHandlers();
            if (_findInput != null)
            {
                _findInput.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter || e.Key == Key.Return) { HandleFind(); e.Handled = true; }
                    else if (e.Key == Key.Escape) { HideFindBar(); e.Handled = true; }
                };
            }
            if (_findButton != null) _findButton.Click += (s, e) => HandleFind();
            if (_backButton != null) _backButton.Click += (s, e) => HandleBack();
            if (_goToInput != null)
            {
                _goToInput.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter || e.Key == Key.Return) { HandleGoTo(); e.Handled = true; }
                    else if (e.Key == Key.Escape) { HideGoToBar(); e.Handled = true; }
                };
            }
            if (_goToButton != null) _goToButton.Click += (s, e) => HandleGoTo();
            SetupCompleter();
            PopulateFindSuggestionChips();

            if (_orphanedNodesList != null) _orphanedNodesList.ContextRequested += OnListWidgetContextRequested;
            if (_pinnedItemsList != null) _pinnedItemsList.ContextRequested += OnListWidgetContextRequested;

            SetupPinnedListDragDrop();
            SetupTreeViewDragSource();
            SetupGraphView();
            WireFlatViewHandlers();

            if (_dialogTree != null)
            {
                _dialogTree.SelectionChanged += (s, e) =>
                {
                    if (_suppressTreeSelectionHandler)
                    {
                        return;
                    }
                    SyncSelectionFromTree();
                };
                _dialogTree.KeyDown += (s, e) => OnKeyDownFromTreeView(e);
                _dialogTree.DoubleTapped += OnDialogTreeDoubleTapped;
            }
            SetupDialogTreeContextMenu();

            WireFilePropertyHandlers();
            WireNodeUpdateHandlers();
            WireButtonHandlers();
            SetupDockablePanels();
            if (_installation != null)
            {
                SetupFileContextMenus();
            }
            SetupDockablePanels();
        }

        /// <summary>
        /// Sets up View menu toggles and float/dock buttons for panels.
        /// </summary>
        private void SetupDockablePanels()
        {
            if (_viewFileGlobals != null && _topDockWidget != null)
            {
                _viewFileGlobals.Click += (s, e) =>
                {
                    _topDockWidget.IsVisible = !_topDockWidget.IsVisible;
                    _viewFileGlobals.IsChecked = _topDockWidget.IsVisible;
                };
            }
            if (_viewNodeFields != null && _rightDockWidget != null)
            {
                _viewNodeFields.Click += (s, e) =>
                {
                    _rightDockWidget.IsVisible = !_rightDockWidget.IsVisible;
                    _viewNodeFields.IsChecked = _rightDockWidget.IsVisible;
                };
            }
            if (_viewOrphanedNodes != null && _leftDockWidget != null)
            {
                _viewOrphanedNodes.Click += (s, e) =>
                {
                    _leftDockWidget.IsVisible = !_leftDockWidget.IsVisible;
                    _viewOrphanedNodes.IsChecked = _leftDockWidget.IsVisible;
                };
            }
            if (_viewJournalNode != null && _dockWidget != null)
            {
                _viewJournalNode.Click += (s, e) =>
                {
                    _dockWidget.IsVisible = !_dockWidget.IsVisible;
                    _viewJournalNode.IsChecked = _dockWidget.IsVisible;
                };
            }

            var topFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "topDockFloatBtn");
            var leftFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "leftDockFloatBtn");
            var rightFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "rightDockFloatBtn");
            var bottomFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "bottomDockFloatBtn");

            if (topFloatBtn != null && _topDockContent != null)
                topFloatBtn.Click += (s, e) => ToggleFloatPanel("top", _topDockContent, _topDockWidget, _topDockFloatWindow, topFloatBtn, Localization.Tr("File Globals"));
            if (leftFloatBtn != null && _leftDockContent != null)
                leftFloatBtn.Click += (s, e) => ToggleFloatPanel("left", _leftDockContent, _leftDockWidget, _leftDockFloatWindow, leftFloatBtn, Localization.Tr("Orphaned Nodes & Pinned Items"));
            if (rightFloatBtn != null && _rightDockContent != null)
                rightFloatBtn.Click += (s, e) => ToggleFloatPanel("right", _rightDockContent, _rightDockWidget, _rightDockFloatWindow, rightFloatBtn, Localization.Tr("Node Fields"));
            if (bottomFloatBtn != null && _bottomDockContent != null)
                bottomFloatBtn.Click += (s, e) => ToggleFloatPanel("bottom", _bottomDockContent, _dockWidget, _bottomDockFloatWindow, bottomFloatBtn, Localization.Tr("Journal / Node"));

            ApplyDLGLocalization();
        }

        /// <summary>
        /// Applies localized strings to all DLG editor UI (menus, labels, buttons, tooltips, watermarks).
        /// Call after controls are loaded so nothing is hardcoded at runtime.
        /// </summary>
        private void ApplyDLGLocalization()
        {
            Title = Localization.Tr("DLG Editor");
            var menuFile = EditorHelpers.FindControlSafe<MenuItem>(this, "menuFile");
            var menuEdit = EditorHelpers.FindControlSafe<MenuItem>(this, "menuEdit");
            var menuTools = EditorHelpers.FindControlSafe<MenuItem>(this, "menuTools");
            var menuView = EditorHelpers.FindControlSafe<MenuItem>(this, "menuView");
            if (menuFile != null) menuFile.Header = Localization.Tr("File");
            if (menuEdit != null) menuEdit.Header = Localization.Tr("Edit");
            if (_actionUndo != null) _actionUndo.Header = Localization.Tr("Undo");
            if (_actionRedo != null) _actionRedo.Header = Localization.Tr("Redo");
            if (menuTools != null) menuTools.Header = Localization.Tr("Tools");
            if (menuView != null) menuView.Header = Localization.Tr("View");
            if (_actionNew != null) _actionNew.Header = Localization.Tr("New");
            if (_actionOpen != null) _actionOpen.Header = Localization.Tr("Open");
            if (_actionSave != null) _actionSave.Header = Localization.Tr("Save");
            if (_actionSaveAs != null) _actionSaveAs.Header = Localization.Tr("Save As");
            if (_actionRevert != null) _actionRevert.Header = Localization.Tr("Revert");
            var menuRecentFiles = EditorHelpers.FindControlSafe<MenuItem>(this, "menuRecentFiles");
            if (menuRecentFiles != null) menuRecentFiles.Header = Localization.Tr("_Recent Files");
            if (_actionDLGSettings != null) _actionDLGSettings.Header = Localization.Tr("DLG Settings...");
            if (_actionExit != null) _actionExit.Header = Localization.Tr("Exit");
            if (_actionReloadTree != null) _actionReloadTree.Header = Localization.Tr("Reload Tree");
            if (_actionUnfocus != null) _actionUnfocus.Header = Localization.Tr("Unfocus Tree");
            if (_viewFileGlobals != null) _viewFileGlobals.Header = Localization.Tr("File Globals");
            if (_viewNodeFields != null) _viewNodeFields.Header = Localization.Tr("Node Fields");
            if (_viewOrphanedNodes != null) _viewOrphanedNodes.Header = Localization.Tr("Orphaned Nodes");
            if (_viewJournalNode != null) _viewJournalNode.Header = Localization.Tr("Journal / Node");
            if (_goToInput != null) { _goToInput.Watermark = Localization.Tr("Go to node ID..."); ToolTip.SetTip(_goToInput, Localization.Tr("Enter node ID to jump. Ctrl+G toggles. Escape to close.")); }
            if (_goToButton != null) _goToButton.Content = Localization.Tr("Go");
            if (_findInput != null) { _findInput.Watermark = Localization.Tr("Find... (e.g. text, speaker:tag, strref:123, AND/OR)"); ToolTip.SetTip(_findInput, Localization.Tr("Query: plain text, or speaker:value, listener:value, strref:123, quest:value, AND/OR. Ctrl+F toggles bar. Escape to close.")); }
            if (_findButton != null) _findButton.Content = Localization.Tr("Find");
            if (_backButton != null) _backButton.Content = Localization.Tr("Back");
            TrySetTextBlockInParent(_topDockWidget, "File Globals", Localization.Tr("File Globals"));
            if (_unequipHandsCheckbox != null) _unequipHandsCheckbox.Content = Localization.Tr("Unequip Hands");
            if (_unequipAllCheckbox != null) _unequipAllCheckbox.Content = Localization.Tr("Unequip All");
            if (_skippableCheckbox != null) { _skippableCheckbox.Content = Localization.Tr("Skippable"); ToolTip.SetTip(_skippableCheckbox, Localization.Tr("Skippable (GFF: Skippable): BYTE. When checked, the player can skip dialogue lines. Default 1. When unchecked, dialogue is unskippable (e.g. cutscenes).")); }
            if (_animatedCutCheckbox != null) _animatedCutCheckbox.Content = Localization.Tr("Animated Cut");
            if (_oldHitCheckbox != null) _oldHitCheckbox.Content = Localization.Tr("Old Hit Check");
            TrySetTextBlockInParent(_topDockWidget, "Conversation Type:", Localization.Tr("Conversation Type:"));
            TrySetTextBlockInParent(_topDockWidget, "Computer Type:", Localization.Tr("Computer Type:"));
            TrySetTextBlockInParent(_topDockWidget, "Delay before Reply:", Localization.Tr("Delay before Reply:"));
            TrySetTextBlockInParent(_topDockWidget, "Delay before Entry:", Localization.Tr("Delay before Entry:"));
            TrySetTextBlockInParent(_topDockWidget, "Voiceover ID:", Localization.Tr("Voiceover ID:"));
            TrySetTextBlockInParent(_topDockWidget, "Conversation Aborts:", Localization.Tr("Conversation Aborts:"));
            TrySetTextBlockInParent(_topDockWidget, "Conversation Ends:", Localization.Tr("Conversation Ends:"));
            TrySetTextBlockInParent(_topDockWidget, "Camera Model:", Localization.Tr("Camera Model:"));
            TrySetTextBlockInParent(_topDockWidget, "Ambient Track:", Localization.Tr("Ambient Track:"));
            TrySetTextBlockInParent(_topDockWidget, "Cutscene Model", Localization.Tr("Cutscene Model"));
            // Localize Conversation Type and Computer Type combo items (order matches AXAML)
            if (_conversationSelect != null)
            {
                var convKeys = new[] { "Human", "Computer", "Type 3", "Type 4", "Type 5" };
                for (int i = 0; i < _conversationSelect.Items.Count && i < convKeys.Length; i++)
                {
                    if (_conversationSelect.Items[i] is ComboBoxItem cbi)
                        cbi.Content = Localization.Tr(convKeys[i]);
                }
                ToolTip.SetTip(_conversationSelect, Localization.Tr("ConversationType: 0=Human (cinematic, voice), 1=Computer (terminal UI), 2=Other (bark strings). Engine uses ReadFieldINT."));
            }
            if (_computerSelect != null)
            {
                var compKeys = new[] { "Modern", "Ancient" };
                for (int i = 0; i < _computerSelect.Items.Count && i < compKeys.Length; i++)
                {
                    if (_computerSelect.Items[i] is ComboBoxItem cbi)
                        cbi.Content = Localization.Tr(compKeys[i]);
                }
                ToolTip.SetTip(_computerSelect, Localization.Tr("ComputerType (GFF: ComputerType): BYTE. 0=Modern (green terminal), 1=Ancient (orange/red). Used when ConversationType is Computer."));
            }
            if (_addStuntButton != null) _addStuntButton.Content = Localization.Tr("Add");
            if (_editStuntButton != null) _editStuntButton.Content = Localization.Tr("Edit");
            if (_removeStuntButton != null) _removeStuntButton.Content = Localization.Tr("Remove");
            TrySetTextBlockInParent(_leftDockWidget, "Orphaned Nodes & Pinned Items", Localization.Tr("Orphaned Nodes & Pinned Items"));
            TrySetTextBlockInParent(_leftDockWidget, "Orphaned Nodes", Localization.Tr("Orphaned Nodes"));
            TrySetTextBlockInParent(_leftDockWidget, "Pinned Items", Localization.Tr("Pinned Items"));
            TrySetTextBlockInParent(_rightDockWidget, "Node Fields", Localization.Tr("Node Fields"));
            if (_commentsEdit != null) { _commentsEdit.Watermark = Localization.Tr("Comments"); ToolTip.SetTip(_commentsEdit, Localization.Tr("Comment (GFF: Comment): Developer-only notes stored in the node. The game ignores this field entirely. Use it to document your dialogue structure, tag nodes for scripting, or leave notes for other modders. Comments are preserved when saving the DLG.")); }
            TrySetTextBlockInParent(_rightDockWidget, "Script #1:", Localization.Tr("Script #1:"));
            TrySetTextBlockInParent(_rightDockWidget, "Script #2:", Localization.Tr("Script #2:"));
            TrySetTextBlockInParent(_rightDockWidget, "Conditional #1:", Localization.Tr("Conditional #1:"));
            if (_condition1NotCheckbox != null) { _condition1NotCheckbox.Content = Localization.Tr("Not"); ToolTip.SetTip(_condition1NotCheckbox, Localization.Tr("Not (GFF: Not): Inverts the Active condition result. When checked, the link is shown only when the script returns FALSE. Use for hide-if-met logic.")); }
            TrySetTextBlockInParent(_rightDockWidget, "Conditional #2:", Localization.Tr("Conditional #2:"));
            if (_condition2NotCheckbox != null) { _condition2NotCheckbox.Content = Localization.Tr("Not"); ToolTip.SetTip(_condition2NotCheckbox, Localization.Tr("Not (GFF: Not): Inverts the Active2 condition result. When checked, the link is shown only when the script returns FALSE. Same as Condition #1 Not.")); }
            TrySetTextBlockInParent(_rightDockWidget, "Current Animations", Localization.Tr("Current Animations"));
            if (_addAnimButton != null) _addAnimButton.Content = Localization.Tr("Add");
            if (_removeAnimButton != null) _removeAnimButton.Content = Localization.Tr("Remove");
            if (_editAnimButton != null) _editAnimButton.Content = Localization.Tr("Edit");
            TrySetTextBlockInParent(_rightDockWidget, "Emotion:", Localization.Tr("Emotion:"));
            TrySetTextBlockInParent(_rightDockWidget, "Expression:", Localization.Tr("Expression:"));
            TrySetTextBlockInParent(_rightDockWidget, "Sound:", Localization.Tr("Sound:"));
            if (_soundCheckbox != null) { _soundCheckbox.Content = Localization.Tr("Exists"); ToolTip.SetTip(_soundCheckbox, Localization.Tr("SoundExists (GFF: SoundExists): BYTE. When checked (0x80), the engine treats the Sound/VO as present and waits for playback. Default 0x80. Affects WaitFlags and timing.")); }
            if (_soundButton != null) _soundButton.Content = Localization.Tr("Play");
            TrySetTextBlockInParent(_rightDockWidget, "Voice:", Localization.Tr("Voice:"));
            if (_voiceButton != null) _voiceButton.Content = Localization.Tr("Play");
            TrySetTextBlockInParent(_rightDockWidget, "Camera ID:", Localization.Tr("Camera ID:"));
            TrySetTextBlockInParent(_rightDockWidget, "Camera Animation:", Localization.Tr("Camera Animation:"));
            TrySetTextBlockInParent(_rightDockWidget, "Camera Angle:", Localization.Tr("Camera Angle:"));
            TrySetTextBlockInParent(_rightDockWidget, "Camera Video Effect:", Localization.Tr("Camera Video Effect:"));
            if (_nodeUnskippableCheckbox != null) { _nodeUnskippableCheckbox.Content = Localization.Tr("Node Unskippable"); ToolTip.SetTip(_nodeUnskippableCheckbox, Localization.Tr("NodeUnskippable: When checked, the player cannot skip this node's voice/text. Use for critical story moments. Overrides file-level Skippable.")); }
            TrySetTextBlockInParent(_rightDockWidget, "Node ID:", Localization.Tr("Node ID:"));
            TrySetTextBlockInParent(_rightDockWidget, "Alien Race Node:", Localization.Tr("Alien Race Node:"));
            TrySetTextBlockInParent(_rightDockWidget, "Post Proc Node:", Localization.Tr("Post Proc Node:"));
            TrySetTextBlockInParent(_rightDockWidget, "Delay:", Localization.Tr("Delay:"));
            TrySetTextBlockInParent(_rightDockWidget, "Logic:", Localization.Tr("Logic:"));
            TrySetTextBlockInParent(_rightDockWidget, "Wait Flags:", Localization.Tr("Wait Flags:"));
            TrySetTextBlockInParent(_rightDockWidget, "Fade Type:", Localization.Tr("Fade Type:"));
            TrySetTextBlockInParent(_dockWidget, "Journal / Node", Localization.Tr("Journal / Node"));
            TrySetTextBlockInParent(_dockWidget, "Listener Tag:", Localization.Tr("Listener Tag:"));
            TrySetTextBlockInParent(_dockWidget, "Quest:", Localization.Tr("Quest:"));
            TrySetTextBlockInParent(_dockWidget, "Speaker Tag:", Localization.Tr("Speaker Tag:"));
            TrySetTextBlockInParent(_dockWidget, "Quest Entry:", Localization.Tr("Quest Entry:"));
            TrySetTextBlockInParent(_dockWidget, "Plot XP Percentage", Localization.Tr("Plot XP Percentage"));
            TrySetTextBlockInParent(_dockWidget, "Plot Index:", Localization.Tr("Plot Index:"));
            // Exhaustive tooltips for File Globals and Node Fields (localized; AXAML strings are overwritten at runtime)
            if (_replyDelaySpin != null) ToolTip.SetTip(_replyDelaySpin, Localization.Tr("DelayReply: Default delay in ms before player reply options appear. DWORD. Used when node Delay is 0xFFFFFFFF."));
            if (_entryDelaySpin != null) ToolTip.SetTip(_entryDelaySpin, Localization.Tr("DelayEntry: Default delay in ms before NPC entry lines appear. DWORD. Used when node Delay is 0xFFFFFFFF."));
            if (_voIdEdit != null) ToolTip.SetTip(_voIdEdit, Localization.Tr("VoiceoverID: Optional ID for voice-over tracking. Leave blank for default."));
            if (_onAbortCombo != null) ToolTip.SetTip(_onAbortCombo, Localization.Tr("EndConverAbort (GFF: EndConverAbort): Script run when the conversation is aborted (player exits early). ResRef of .ncs."));
            if (_onEndEdit != null) ToolTip.SetTip(_onEndEdit, Localization.Tr("EndConversation (GFF: EndConversation): Script run when the conversation ends normally. ResRef of .ncs."));
            if (_cameraModelSelect != null) ToolTip.SetTip(_cameraModelSelect, Localization.Tr("CameraModel (GFF: CameraModel): ResRef of MDL defining camera positions for cinematic dialogue. Required for Animated/Static camera angles."));
            if (_ambientTrackCombo != null) ToolTip.SetTip(_ambientTrackCombo, Localization.Tr("AmbientTrack (GFF: AmbientTrack): ResRef of background music loop during dialogue. Leave blank for default."));
            if (_stuntList != null) ToolTip.SetTip(_stuntList, Localization.Tr("StuntList: Custom models for dialogue participants. Each stunt maps Participant tag to StuntModel ResRef. Used for cutscenes with non-standard character models (e.g. droids, creatures). Participant must match a creature/npc tag in the dialogue."));
            if (_script1ResrefEdit != null) ToolTip.SetTip(_script1ResrefEdit, Localization.Tr("Script (GFF: Script): Action script run when this node is reached. Executes after text/voice plays. Use for granting items, updating variables, or triggering cutscenes. ResRef must be a valid .ncs script. Params below are passed as (int int int int int string)."));
            if (_script2ResrefEdit != null) ToolTip.SetTip(_script2ResrefEdit, Localization.Tr("Script2 (GFF: Script2): Secondary action script. KotOR supports two scripts per node; both run when the node is reached. Same param structure as Script. Use for modular or conditional logic."));
            if (_condition1ResrefEdit != null) ToolTip.SetTip(_condition1ResrefEdit, Localization.Tr("Active (GFF: Active): Condition script that determines if this link is shown. Must return TRUE (non-zero) for the link to appear. Empty = always show. Params below are passed to the script. Logic combines with Condition #2 (AND/OR)."));
            if (_condition2ResrefEdit != null) ToolTip.SetTip(_condition2ResrefEdit, Localization.Tr("Active2 (GFF: Active2): Second condition script for links. Combined with Condition #1 via Logic (AND/OR). KotOR 2 extension. Params use Param1b-5b, ParamStrB."));
            SetToolTipSafe(_script1Param1Spin, "ActionParam1: First integer for action script. INT32 range.");
            SetToolTipSafe(EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param2Spin"), "ActionParam2: Second integer for action script. INT32 range.");
            SetToolTipSafe(EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param3Spin"), "ActionParam3: Third integer for action script. INT32 range.");
            SetToolTipSafe(EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param4Spin"), "ActionParam4: Fourth integer for action script. INT32 range.");
            SetToolTipSafe(EditorHelpers.FindControlSafe<NumericUpDown>(this, "script1Param5Spin"), "ActionParam5: Fifth integer for action script. INT32 range.");
            if (_script1Param6Edit != null) ToolTip.SetTip(_script1Param6Edit, Localization.Tr("ActionParamStrA: String argument for action script. Use for ResRefs, tags, or custom text."));
            SetToolTipSafe(_script2Param1Spin, "ActionParam1b: First integer for Script2. INT32 range.");
            SetToolTipSafe(_script2Param2Spin, "ActionParam2b: Second integer for Script2. INT32 range.");
            SetToolTipSafe(_script2Param3Spin, "ActionParam3b: Third integer for Script2. INT32 range.");
            SetToolTipSafe(_script2Param4Spin, "ActionParam4b: Fourth integer for Script2. INT32 range.");
            SetToolTipSafe(_script2Param5Spin, "ActionParam5b: Fifth integer for Script2. INT32 range.");
            if (_script2Param6Edit != null) ToolTip.SetTip(_script2Param6Edit, Localization.Tr("ActionParamStrB: String argument for Script2."));
            SetToolTipSafe(_condition1Param1Spin, "Param1: First integer for Active condition script. INT32 range.");
            SetToolTipSafe(_condition1Param2Spin, "Param2: Second integer for Active condition script. INT32 range.");
            SetToolTipSafe(_condition1Param3Spin, "Param3: Third integer for Active condition script. INT32 range.");
            SetToolTipSafe(_condition1Param4Spin, "Param4: Fourth integer for Active condition script. INT32 range.");
            SetToolTipSafe(_condition1Param5Spin, "Param5: Fifth integer for Active condition script. INT32 range.");
            if (_condition1Param6Edit != null) ToolTip.SetTip(_condition1Param6Edit, Localization.Tr("ParamStrA: String argument for Active condition script."));
            SetToolTipSafe(_condition2Param1Spin, "Param1b: First integer for Active2 condition script. INT32 range.");
            SetToolTipSafe(_condition2Param2Spin, "Param2b: Second integer for Active2. INT32 range.");
            SetToolTipSafe(_condition2Param3Spin, "Param3b: Third integer for Active2. INT32 range.");
            SetToolTipSafe(_condition2Param4Spin, "Param4b: Fourth integer for Active2. INT32 range.");
            SetToolTipSafe(_condition2Param5Spin, "Param5b: Fifth integer for Active2. INT32 range.");
            if (_condition2Param6Edit != null) ToolTip.SetTip(_condition2Param6Edit, Localization.Tr("ParamStrB: String argument for Active2 condition script."));
            if (_emotionSelect != null) ToolTip.SetTip(_emotionSelect, Localization.Tr("Emotion (GFF: Emotion): Emotion ID from emotions.2da. Plays on the speaker. Used for gesture/emotion animations during dialogue."));
            if (_expressionSelect != null) ToolTip.SetTip(_expressionSelect, Localization.Tr("FacialAnim (GFF: FacialAnim): Expression ID from expressions.2da. Plays on the speaker during this node. Index into the 2DA row."));
            if (_soundComboBox != null) ToolTip.SetTip(_soundComboBox, Localization.Tr("Sound (GFF: Sound): ResRef of a WAV played during this node. Overrides VO_ResRef if both exist. Used for ambient or non-voice sounds."));
            if (_voiceComboBox != null) ToolTip.SetTip(_voiceComboBox, Localization.Tr("VO_ResRef (GFF: VO_ResRef): Voice-over WAV ResRef. Plays when this node is reached. Used if Sound is empty."));
            if (_cameraEffectSelect != null) ToolTip.SetTip(_cameraEffectSelect, Localization.Tr("CamVidEffect (GFF: CamVidEffect): Video effect ID from videoeffects.2da. INT32. -1 = no effect. Applied during this node's camera shot."));
            if (_nodeIdSpin != null) ToolTip.SetTip(_nodeIdSpin, Localization.Tr("NodeID (GFF: NodeID): Unique identifier for this node. Used by scripts and external references. INT32. Modders can use this to target specific nodes in scripts."));
            if (_alienRaceNodeSpin != null) ToolTip.SetTip(_alienRaceNodeSpin, Localization.Tr("AlienRaceNode (GFF: AlienRaceNode): Index for alien-race-specific dialogue variants. Used with alien language/translation systems. INT32."));
            if (_postProcSpin != null) ToolTip.SetTip(_postProcSpin, Localization.Tr("PostProcNode (GFF: PostProcNode): Post-processing node index for special dialogue effects. INT32. See PostProcOwner at file level."));
            if (_delaySpin != null) ToolTip.SetTip(_delaySpin, Localization.Tr("Delay (GFF: Delay): Milliseconds before text appears. DWORD. 0xFFFFFFFF (-1) = auto-calculated from voice/sound or DelayEntry/DelayReply."));
            if (_logicSpin != null) ToolTip.SetTip(_logicSpin, Localization.Tr("Logic (GFF: Logic): For links, combines Condition #1 and #2: 0=AND (both must pass), 1=OR (either passes). KotOR 2 extension. INT32."));
            if (_waitFlagSpin != null) ToolTip.SetTip(_waitFlagSpin, Localization.Tr("WaitFlags (GFF: WaitFlags): Bitmask controlling when the dialogue advances. DWORD. Bit 2=wait for sound/VO; bit 4=use explicit Delay; bit 0x10=Delay is set."));
            if (_fadeTypeSpin != null) ToolTip.SetTip(_fadeTypeSpin, Localization.Tr("FadeType (GFF: FadeType): Screen fade for this node. BYTE 0-255. 0=None, 1=FadeIn, 2=FadeOut. Values 1-2 clear FadeDelay/FadeLength."));
            if (_plotXpSpin != null) ToolTip.SetTip(_plotXpSpin, Localization.Tr("PlotXPPercentage: Float 0-100. Percentage of plot XP granted when this node is reached."));
            if (_plotIndexCombo != null) ToolTip.SetTip(_plotIndexCombo, Localization.Tr("PlotIndex: Index into plot.2da. -1 = None."));
            var topFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "topDockFloatBtn");
            var leftFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "leftDockFloatBtn");
            var rightFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "rightDockFloatBtn");
            var bottomFloatBtn = EditorHelpers.FindControlSafe<Button>(this, "bottomDockFloatBtn");
            if (topFloatBtn != null) ToolTip.SetTip(topFloatBtn, Localization.Tr("Float (detach) panel"));
            if (leftFloatBtn != null) ToolTip.SetTip(leftFloatBtn, Localization.Tr("Float (detach) panel"));
            if (rightFloatBtn != null) ToolTip.SetTip(rightFloatBtn, Localization.Tr("Float (detach) panel"));
            if (bottomFloatBtn != null) ToolTip.SetTip(bottomFloatBtn, Localization.Tr("Float (detach) panel"));
            if (_tipLabel != null) ToolTip.SetTip(_tipLabel, Localization.Tr("Double-click to view all tips."));
        }

        private static void TrySetTextBlockInParent(Control parent, string currentText, string newText)
        {
            if (parent == null || string.IsNullOrEmpty(currentText)) return;
            var tb = parent.GetVisualDescendants().OfType<TextBlock>().FirstOrDefault(t => t.Text == currentText);
            if (tb != null) tb.Text = newText;
        }

        private static void SetToolTipSafe(Control control, string localizationKey)
        {
            if (control != null) ToolTip.SetTip(control, Localization.Tr(localizationKey));
        }

        /// <summary>
        /// Toggles a panel between docked and floating. Vendor: QDockWidget float/dock behavior.
        /// </summary>
        private void ToggleFloatPanel(string id, Border contentHost, Control dockHost, WindowHolder floatWindowHolder, Button floatBtn, string title)
        {
            if (contentHost == null || dockHost == null) return;
            if (floatWindowHolder.Value != null && floatWindowHolder.Value.IsVisible)
            {
                DockPanelBack(contentHost, dockHost, floatWindowHolder);
                floatBtn.Content = "\u25A1"; // empty square = float
                ToolTip.SetTip(floatBtn, Localization.Tr("Float (detach) panel"));
            }
            else
            {
                FloatPanel(contentHost, dockHost, floatWindowHolder, floatBtn, title);
                floatBtn.Content = "\u25A3"; // filled square = dock
                ToolTip.SetTip(floatBtn, Localization.Tr("Dock (reattach) panel"));
            }
        }

        private void FloatPanel(Border contentHost, Control dockHost, WindowHolder floatWindowHolder, Button floatBtn, string title)
        {
            var child = contentHost.Child;
            if (child == null) return;
            contentHost.Child = null;
            double w = 320;
            double h = 200;
            if (dockHost != null)
            {
                w = Math.Max(320, dockHost.Bounds.Width);
                h = Math.Max(200, dockHost.Bounds.Height);
                dockHost.IsVisible = false;
            }
            var floatWindow = new Window
            {
                Title = title,
                Width = w,
                Height = h,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Content = new Border { Child = child, Padding = new Thickness(8) }
            };
            floatWindow.Closed += (s, e) =>
            {
                DockPanelBack(contentHost, dockHost, floatWindowHolder);
                floatBtn.Content = "\u25A1";
                ToolTip.SetTip(floatBtn, Localization.Tr("Float (detach) panel"));
            };
            floatWindowHolder.Value = floatWindow;

            // Defer Show so the window opens after the button click is fully processed.
            // Showing immediately can cause the window to close instantly (focus/owner handling).
            var ownerWindow = this;
            Dispatcher.UIThread.Post(() =>
            {
                if (floatWindowHolder.Value != floatWindow) return;
                try
                {
                    // Position near the owner so the window doesn't appear at (0,0)
                    if (ownerWindow is Window owner)
                    {
                        var pos = owner.Position;
                        floatWindow.Position = new PixelPoint(pos.X + 40, pos.Y + 40);
                    }
                    floatWindow.Show(ownerWindow);
                }
                catch (InvalidOperationException)
                {
                    // Owner may have been closed; show without owner
                    floatWindow.Show();
                }
            }, DispatcherPriority.Loaded);
        }

        private void DockPanelBack(Border contentHost, Control dockHost, WindowHolder floatWindowHolder)
        {
            var floatWindow = floatWindowHolder.Value;
            if (floatWindow == null) return;
            var winContent = floatWindow.Content as Border;
            var child = winContent?.Child;
            if (child != null)
            {
                winContent.Child = null;
                contentHost.Child = child;
            }
            if (dockHost != null)
                dockHost.IsVisible = true;
            // Clear holder before Close() so the Closed handler doesn't re-enter
            floatWindowHolder.Value = null;
            floatWindow.Close();
        }

        private sealed class WindowHolder
        {
            public Window Value;
        }

        /// <summary>
        /// Sets up right-click file context menus on script/condition/sound/voice/onEnd/onAbort/camera/ambient controls.
        /// </summary>
        private void SetupFileContextMenus()
        {
            var searchOrder = new[] { SearchLocation.CHITIN, SearchLocation.OVERRIDE, SearchLocation.MODULES, SearchLocation.RIMS };
            SetupFileContextMenuForControl(_script1ResrefEdit, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_script2ResrefEdit, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_condition1ResrefEdit, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_condition2ResrefEdit, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_onAbortCombo, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_onEndEdit, new[] { ResourceType.NSS, ResourceType.NCS }, searchOrder, true, "script");
            SetupFileContextMenuForControl(_cameraModelSelect, new[] { ResourceType.MDL }, new[] { SearchLocation.CHITIN, SearchLocation.OVERRIDE }, false, null);
            SetupFileContextMenuForControl(_ambientTrackCombo, new[] { ResourceType.WAV, ResourceType.MP3 }, new[] { SearchLocation.MUSIC }, false, null);
            SetupFileContextMenuForControl(_soundComboBox, new[] { ResourceType.WAV, ResourceType.MP3 }, new[] { SearchLocation.SOUND, SearchLocation.VOICE }, false, null);
            SetupFileContextMenuForControl(_voiceComboBox, new[] { ResourceType.WAV, ResourceType.MP3 }, new[] { SearchLocation.VOICE }, false, null);
        }

        private void SetupFileContextMenuForControl(Control control, ResourceType[] resourceTypes, SearchLocation[] order, bool enableFindRefs, string referenceSearchType)
        {
            if (control == null || _installation == null || resourceTypes == null || resourceTypes.Length == 0)
            {
                return;
            }
            control.ContextRequested += (sender, e) =>
            {
                string widgetText = (control as ComboBox)?.Text?.Trim() ?? (control as TextBox)?.Text?.Trim() ?? "";
                var menu = new ContextMenu();
                var fileSubMenu = new MenuItem { Header = Localization.Tr("File...") };
                int locationCount = 0;
                var flatLocations = new List<LocationResult>();
                foreach (var restype in resourceTypes)
                {
                    var query = new ResourceIdentifier(widgetText, restype);
                    var locations = _installation.Locations(new List<ResourceIdentifier> { query }, order);
                    if (locations != null && locations.ContainsKey(query) && locations[query].Count > 0)
                    {
                        foreach (var loc in locations[query])
                        {
                            locationCount++;
                            flatLocations.Add(loc);
                            var displayPath = System.IO.Path.GetFileName(loc.FilePath) ?? loc.FilePath;
                            var subItem = new MenuItem { Header = displayPath };
                            subItem.Click += (s, ev) =>
                            {
                                try
                                {
                                    var fr = loc.FileResource ?? new FileResource(widgetText, restype, loc.Size, loc.Offset, loc.FilePath);
                                    var dialog = new LoadFromLocationResultDialog(this, new List<FileResource> { fr }, _installation);
                                    dialog.Title = displayPath;
                                    dialog.Show();
                                }
                                catch (Exception ex)
                                {
                                    System.Console.WriteLine($"Open location: {ex.Message}");
                                }
                            };
                            fileSubMenu.Items.Add(subItem);
                        }
                    }
                }
                if (locationCount > 0)
                {
                    var detailsItem = new MenuItem { Header = Localization.Tr("Details...") };
                    detailsItem.Click += (s, ev) =>
                    {
                        var resources = flatLocations.Select(loc => loc.FileResource ?? new FileResource(widgetText, resourceTypes[0], loc.Size, loc.Offset, loc.FilePath)).ToList();
                        var dialog = new LoadFromLocationResultDialog(this, resources, _installation);
                        dialog.Title = string.Format(Localization.Tr("{0} file(s) located"), locationCount);
                        dialog.Show();
                    };
                    fileSubMenu.Items.Add(new Separator());
                    fileSubMenu.Items.Add(detailsItem);
                }
                else
                {
                    fileSubMenu.Header = Localization.Tr("0 file(s) located");
                    fileSubMenu.IsEnabled = false;
                }
                menu.Items.Add(fileSubMenu);
                if (locationCount > 0 && fileSubMenu.Header is string h && h == Localization.Tr("File..."))
                {
                    fileSubMenu.Header = string.Format(Localization.Tr("{0} file(s) located"), locationCount);
                }
                if (enableFindRefs && !string.IsNullOrWhiteSpace(widgetText))
                {
                    menu.Items.Add(new Separator());
                    var findRefItem = new MenuItem { Header = Localization.Tr("Find References...") };
                    findRefItem.Click += (s, ev) => FindReferencesToResref(widgetText.Trim(), referenceSearchType ?? "resref");
                    menu.Items.Add(findRefItem);
                }
                menu.Open(control);
                e.Handled = true;
            };
        }

        private void FindReferencesToResref(string searchText, string searchType)
        {
            if (_installation == null || string.IsNullOrWhiteSpace(searchText))
            {
                return;
            }
            var results = new List<FileResource>();
            var overrideList = _installation.OverrideResources() ?? new List<FileResource>();
            foreach (var fileRes in overrideList.Where(r => r != null && (r.ResType == ResourceType.NSS || r.ResType == ResourceType.NCS)))
            {
                try
                {
                    var rr = _installation.Resource(fileRes.ResName, fileRes.ResType, new[] { SearchLocation.OVERRIDE });
                    if (rr?.Data == null)
                    {
                        continue;
                    }
                    string content = System.Text.Encoding.UTF8.GetString(rr.Data);
                    if (content.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(fileRes);
                    }
                }
                catch
                {
                    // Skip
                }
            }
            if (results.Count == 0)
            {
                MessageBoxManager.GetMessageBoxStandard(Localization.Tr("No references found"), string.Format(Localization.Tr("No references found for '{0}'."), searchText),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowAsync();
                return;
            }
            var dialog = new LoadFromLocationResultDialog(this, results, _installation);
            dialog.Title = string.Format(Localization.Tr("{0} reference(s) found for '{1}'"), results.Count, searchText);
            dialog.Show();
        }

        private void WireFilePropertyHandlers()
        {
            if (_voIdEdit != null) _voIdEdit.LostFocus += (s, e) => OnFilePropertyChanged();
            if (_ambientTrackCombo != null) _ambientTrackCombo.LostFocus += (s, e) => OnFilePropertyChanged();
            if (_conversationSelect != null) _conversationSelect.SelectionChanged += (s, e) => OnFilePropertyChanged();
            if (_computerSelect != null) _computerSelect.SelectionChanged += (s, e) => OnFilePropertyChanged();
            if (_entryDelaySpin != null) _entryDelaySpin.ValueChanged += (s, e) => OnFilePropertyChanged();
            if (_replyDelaySpin != null) _replyDelaySpin.ValueChanged += (s, e) => OnFilePropertyChanged();
            if (_onAbortCombo != null) _onAbortCombo.LostFocus += (s, e) => OnFilePropertyChanged();
            if (_onEndEdit != null) _onEndEdit.LostFocus += (s, e) => OnFilePropertyChanged();
            if (_cameraModelSelect != null)
            {
                _cameraModelSelect.LostFocus += (s, e) => { OnFilePropertyChanged(); UpdateCameraWidgetState(); };
            }
            if (_unequipHandsCheckbox != null) { _unequipHandsCheckbox.Checked += (s, e) => OnFilePropertyChanged(); _unequipHandsCheckbox.Unchecked += (s, e) => OnFilePropertyChanged(); }
            if (_unequipAllCheckbox != null) { _unequipAllCheckbox.Checked += (s, e) => OnFilePropertyChanged(); _unequipAllCheckbox.Unchecked += (s, e) => OnFilePropertyChanged(); }
            if (_skippableCheckbox != null) { _skippableCheckbox.Checked += (s, e) => OnFilePropertyChanged(); _skippableCheckbox.Unchecked += (s, e) => OnFilePropertyChanged(); }
            if (_animatedCutCheckbox != null) { _animatedCutCheckbox.Checked += (s, e) => OnFilePropertyChanged(); _animatedCutCheckbox.Unchecked += (s, e) => OnFilePropertyChanged(); }
            if (_oldHitCheckbox != null) { _oldHitCheckbox.Checked += (s, e) => OnFilePropertyChanged(); _oldHitCheckbox.Unchecked += (s, e) => OnFilePropertyChanged(); }
        }

        private void WireNodeUpdateHandlers()
        {
            void WireCombo(ComboBox c) { if (c != null) c.LostFocus += (s, e) => OnNodeUpdate(); }
            void WireSpin(NumericUpDown n) { if (n != null) n.ValueChanged += (s, e) => OnNodeUpdate(); }
            void WireCheck(CheckBox c) { if (c != null) { c.Checked += (s, e) => OnNodeUpdate(); c.Unchecked += (s, e) => OnNodeUpdate(); } }
            WireCombo(_condition1ResrefEdit); WireCombo(_condition2ResrefEdit); WireSpin(_logicSpin);
            WireSpin(_condition1Param1Spin); WireSpin(_condition1Param2Spin); WireSpin(_condition1Param3Spin); WireSpin(_condition1Param4Spin); WireSpin(_condition1Param5Spin);
            if (_condition1Param6Edit != null) _condition1Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            WireCheck(_condition1NotCheckbox);
            WireSpin(_condition2Param1Spin); WireSpin(_condition2Param2Spin); WireSpin(_condition2Param3Spin); WireSpin(_condition2Param4Spin); WireSpin(_condition2Param5Spin);
            if (_condition2Param6Edit != null) _condition2Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            WireCheck(_condition2NotCheckbox);
            WireCombo(_script1ResrefEdit); WireCombo(_script2ResrefEdit);
            WireSpin(_script1Param1Spin); WireSpin(_script1Param2Spin); WireSpin(_script1Param3Spin); WireSpin(_script1Param4Spin); WireSpin(_script1Param5Spin);
            WireSpin(_script2Param1Spin); WireSpin(_script2Param2Spin); WireSpin(_script2Param3Spin); WireSpin(_script2Param4Spin); WireSpin(_script2Param5Spin);
            if (_script1Param6Edit != null) _script1Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            if (_script2Param6Edit != null) _script2Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            WireSpin(_cameraIdSpin); WireSpin(_cameraAnimSpin);
            if (_cameraAngleSelect != null) _cameraAngleSelect.SelectionChanged += (s, e) => OnNodeUpdate();
            if (_cameraEffectSelect != null) _cameraEffectSelect.SelectionChanged += (s, e) => OnNodeUpdate();
            if (_emotionSelect != null) _emotionSelect.SelectionChanged += (s, e) => OnNodeUpdate();
            if (_expressionSelect != null) _expressionSelect.SelectionChanged += (s, e) => OnNodeUpdate();
            WireSpin(_nodeIdSpin); WireSpin(_alienRaceNodeSpin); WireSpin(_postProcSpin); WireSpin(_delaySpin); WireSpin(_waitFlagSpin); WireSpin(_fadeTypeSpin);
            WireCheck(_nodeUnskippableCheckbox);
            WireCheck(_soundCheckbox);
            if (_speakerEdit != null) _speakerEdit.LostFocus += (s, e) => OnNodeUpdate();
            if (_listenerEdit != null) _listenerEdit.LostFocus += (s, e) => OnNodeUpdate();
            if (_questEdit != null) _questEdit.LostFocus += (s, e) => OnNodeUpdate();
            WireSpin(_questEntrySpin);
            if (_plotIndexCombo != null) _plotIndexCombo.SelectionChanged += (s, e) => OnNodeUpdate();
            WireSpin(_plotXpSpin);
        }

        private void WireButtonHandlers()
        {
            if (_addAnimButton != null) _addAnimButton.Click += (s, e) => OnAddAnimClicked();
            if (_removeAnimButton != null) _removeAnimButton.Click += (s, e) => OnRemoveAnimClicked();
            if (_editAnimButton != null) _editAnimButton.Click += (s, e) => OnEditAnimClicked();
            if (_soundButton != null) _soundButton.Click += (s, e) => { var t = _soundComboBox?.Text?.Trim(); if (!string.IsNullOrEmpty(t)) PlaySound(t, new[] { SearchLocation.SOUND, SearchLocation.VOICE }); else BlinkWindow(); };
            if (_voiceButton != null) _voiceButton.Click += (s, e) => { var t = _voiceComboBox?.Text?.Trim(); if (!string.IsNullOrEmpty(t)) PlaySound(t, new[] { SearchLocation.VOICE }); else BlinkWindow(); };
            if (_addStuntButton != null) _addStuntButton.Click += (s, e) => OnAddStuntClicked();
            if (_editStuntButton != null) _editStuntButton.Click += (s, e) => OnEditStuntClicked();
            if (_removeStuntButton != null) _removeStuntButton.Click += (s, e) => OnRemoveStuntClicked();
            if (_stuntList != null) _stuntList.DoubleTapped += OnStuntListDoubleTapped;
        }

        private void InitializeCameraWidgets()
        {
            // When loaded from XAML, controls already exist; only populate combo items and wire events.
            if (_cameraIdSpin != null)
            {
                PopulateCameraAndEmotionComboItems();
                return;
            }
            // Initialize camera widgets (code path when XAML not used)
            _cameraIdSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = -1 };
            _cameraIdSpin.ValueChanged += (s, e) => OnNodeUpdate();

            _cameraAnimSpin = new NumericUpDown { Minimum = 1200, Maximum = 65535, Value = 1200 };
            _cameraAnimSpin.ValueChanged += (s, e) => OnNodeUpdate();

            _cameraAngleSelect = new ComboBox();
            // Camera angle options: 0-6 (localized)
            // Vendor dlg.ui: Auto(0), Face(1), Shoulder(2), Wide Shot(3), Animated Camera(4), (DO NOT USE)(5), Static Camera(6)
            _cameraAngleSelect.Items.Add(Localization.Tr("Auto"));
            _cameraAngleSelect.Items.Add(Localization.Tr("Face"));
            _cameraAngleSelect.Items.Add(Localization.Tr("Shoulder"));
            _cameraAngleSelect.Items.Add(Localization.Tr("Wide Shot"));
            _cameraAngleSelect.Items.Add(Localization.Tr("Animated Camera"));
            _cameraAngleSelect.Items.Add(Localization.Tr("(DO NOT USE THIS ENTRY)"));
            _cameraAngleSelect.Items.Add(Localization.Tr("Static Camera"));
            _cameraAngleSelect.SelectedIndex = 0;
            _cameraAngleSelect.SelectionChanged += (s, e) => OnNodeUpdate();

            _cameraEffectSelect = new ComboBox2DA();
            _cameraEffectSelect.SelectionChanged += (s, e) => OnNodeUpdate();

            _emotionSelect = new ComboBox2DA();
            _emotionSelect.SelectionChanged += (s, e) => OnNodeUpdate();

            _expressionSelect = new ComboBox2DA();
            _expressionSelect.SelectionChanged += (s, e) => OnNodeUpdate();
        }

        /// <summary>
        /// Populates camera/emotion/expression combo boxes when UI is loaded from XAML.
        /// </summary>
        private void PopulateCameraAndEmotionComboItems()
        {
            // Vendor dlg.ui: Auto, Face, Shoulder, Wide Shot, Animated Camera, (DO NOT USE THIS ENTRY), Static Camera (localized)
            if (_cameraAngleSelect != null && _cameraAngleSelect.Items.Count == 0)
            {
                _cameraAngleSelect.Items.Add(Localization.Tr("Auto"));
                _cameraAngleSelect.Items.Add(Localization.Tr("Face"));
                _cameraAngleSelect.Items.Add(Localization.Tr("Shoulder"));
                _cameraAngleSelect.Items.Add(Localization.Tr("Wide Shot"));
                _cameraAngleSelect.Items.Add(Localization.Tr("Animated Camera"));
                _cameraAngleSelect.Items.Add(Localization.Tr("(DO NOT USE THIS ENTRY)"));
                _cameraAngleSelect.Items.Add(Localization.Tr("Static Camera"));
                _cameraAngleSelect.SelectedIndex = 0;
            }
            // emotion, expression, cameraEffect, plotIndex are populated by SetupTslEmotionsAndExpressions
        }

        /// <summary>
        /// True when an installation is selected in File → DLG Settings and we have that installation; otherwise we rely only on manual paths from DLG Settings.
        /// </summary>
        private bool UseInstallationForResources()
        {
            var dlgSettings = new DLGSettings();
            return dlgSettings.UseInstallation(true) && _installation != null;
        }

        /// <summary>
        /// Populates emotion, expression, cameraEffect, plotIndex ComboBox2DAs from 2DA files when installation available.
        /// Matching vendor: editor.py _setup_tsl_emotions_and_expressions, _setup_installation (vid_effects, plot2DA).
        /// </summary>
        private void SetupTslEmotionsAndExpressions()
        {
            bool useInstallation = UseInstallationForResources();
            var installation = useInstallation ? Installation : null;
            var dlgSettings = new DLGSettings();
            var customFolders = dlgSettings.GetCustom2DAFolders();
            if (_emotionSelect != null)
            {
                _emotionSelect.Items.Clear();
                if (installation != null)
                {
                    var emotions = installation.Get2DAWithCustomFolders(OdyInstallation.TwoDAEmotions, customFolders);
                    if (emotions != null)
                    {
                        _emotionSelect.SetContext(emotions, installation, OdyInstallation.TwoDAEmotions);
                        var labels = emotions.GetColumn("label");
                        if (labels != null) _emotionSelect.SetItems(labels, sortAlphabetically: false);
                    }
                }
                else
                {
                    var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDAEmotions);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var emotions = TwoDAFileHelper.LoadFromPath(path);
                        if (emotions != null)
                        {
                            _emotionSelect.SetContext(emotions, null, OdyInstallation.TwoDAEmotions);
                            var labels = emotions.GetColumn("label");
                            if (labels != null) _emotionSelect.SetItems(labels, sortAlphabetically: false);
                        }
                    }
                }
                if (_emotionSelect.Items.Count == 0)
                {
                    _emotionSelect.SetItems(new[] { "0 (None)", "1 (Happy)", "2 (Sad)", "3 (Angry)", "4 (Surprised)", "5 (Fear)", "6 (Disgust)", "7 (Neutral)" }, sortAlphabetically: false, cleanupStrings: false);
                }
            }
            if (_expressionSelect != null)
            {
                _expressionSelect.Items.Clear();
                if (installation != null)
                {
                    var expressions = installation.Get2DAWithCustomFolders(OdyInstallation.TwoDAExpressions, customFolders);
                    if (expressions != null)
                    {
                        _expressionSelect.SetContext(expressions, installation, OdyInstallation.TwoDAExpressions);
                        var labels = expressions.GetColumn("label");
                        if (labels != null) _expressionSelect.SetItems(labels, sortAlphabetically: false);
                    }
                }
                else
                {
                    var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDAExpressions);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var expressions = TwoDAFileHelper.LoadFromPath(path);
                        if (expressions != null)
                        {
                            _expressionSelect.SetContext(expressions, null, OdyInstallation.TwoDAExpressions);
                            var labels = expressions.GetColumn("label");
                            if (labels != null) _expressionSelect.SetItems(labels, sortAlphabetically: false);
                        }
                    }
                }
                if (_expressionSelect.Items.Count == 0)
                {
                    _expressionSelect.SetItems(new[] { "0 (None)", "1 (Smile)", "2 (Frown)", "3 (Scowl)", "4 (Shock)", "5 (Terror)", "6 (Wince)", "7 (Blink)" }, sortAlphabetically: false, cleanupStrings: false);
                }
            }
            if (_cameraEffectSelect != null)
            {
                _cameraEffectSelect.Items.Clear();
                if (installation != null)
                {
                    var vidEffects = installation.Get2DAWithCustomFolders(OdyInstallation.TwoDAVideoEffects, customFolders);
                    if (vidEffects != null)
                    {
                        _cameraEffectSelect.SetContext(vidEffects, installation, OdyInstallation.TwoDAVideoEffects);
                        _cameraEffectSelect.AddItem("[Unset]", -1);
                        var labels = vidEffects.GetColumn("label");
                        if (labels != null)
                        {
                            for (int i = 0; i < labels.Count; i++)
                            {
                                var l = (labels[i] ?? "").Replace("VIDEO_EFFECT_", "").Replace("_", " ");
                                if (!string.IsNullOrWhiteSpace(l))
                                    _cameraEffectSelect.AddItem(l, i);
                            }
                        }
                    }
                }
                else
                {
                    var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDAVideoEffects);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var vidEffects = TwoDAFileHelper.LoadFromPath(path);
                        if (vidEffects != null)
                        {
                            _cameraEffectSelect.SetContext(vidEffects, null, OdyInstallation.TwoDAVideoEffects);
                            _cameraEffectSelect.AddItem("[Unset]", -1);
                            var labels = vidEffects.GetColumn("label");
                            if (labels != null)
                            {
                                for (int i = 0; i < labels.Count; i++)
                                {
                                    var l = (labels[i] ?? "").Replace("VIDEO_EFFECT_", "").Replace("_", " ");
                                    if (!string.IsNullOrWhiteSpace(l))
                                        _cameraEffectSelect.AddItem(l, i);
                                }
                            }
                        }
                    }
                }
                if (_cameraEffectSelect.Items.Count == 0)
                {
                    _cameraEffectSelect.AddItem("[Unset]", -1);
                    _cameraEffectSelect.SetItems(allCameraEffects, sortAlphabetically: false, cleanupStrings: false);
                }
            }
            if (_plotIndexCombo != null)
            {
                _plotIndexCombo.Items.Clear();
                if (installation != null)
                {
                    var plot2da = installation.Get2DAWithCustomFolders(OdyInstallation.TwoDAPlot, customFolders);
                    if (plot2da != null)
                    {
                        _plotIndexCombo.SetContext(plot2da, installation, OdyInstallation.TwoDAPlot);
                        _plotIndexCombo.AddItem("[None]", -1);
                        var labels = plot2da.GetColumn("label");
                        if (labels != null)
                        {
                            for (int i = 0; i < labels.Count; i++)
                            {
                                var s = (labels[i] ?? "").ToLowerInvariant();
                                var titled = string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);
                                _plotIndexCombo.AddItem(titled, i);
                            }
                        }
                    }
                }
                else
                {
                    var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDAPlot);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var plot2da = TwoDAFileHelper.LoadFromPath(path);
                        if (plot2da != null)
                        {
                            _plotIndexCombo.SetContext(plot2da, null, OdyInstallation.TwoDAPlot);
                            _plotIndexCombo.AddItem("[None]", -1);
                            var labels = plot2da.GetColumn("label");
                            if (labels != null)
                            {
                                for (int idx = 0; idx < labels.Count; idx++)
                                {
                                    var s = (labels[idx]?.ToString() ?? "").ToLowerInvariant();
                                    var titled = string.IsNullOrEmpty(s) ? s : char.ToUpperInvariant(s[0]) + s.Substring(1);
                                    _plotIndexCombo.AddItem(titled, idx);
                                }
                            }
                        }
                    }
                }
                if (_plotIndexCombo.Items.Count == 0)
                {
                    _plotIndexCombo.AddItem("[None]", -1);
                    for (int i = 0; i <= 5; i++)
                        _plotIndexCombo.AddItem(i == 0 ? "No Plot" : $"Plot {i}", i);
                }
            }
        }

        private void SetupUI()
        {
            // Create main dock panel to support menu bar
            var dockPanel = new DockPanel();
            SetContentOrInject(dockPanel);

            // Setup menu bar - matching PyKotor implementation
            SetupMenuBar(dockPanel);

            // Create main content panel
            var panel = new StackPanel();
            dockPanel.Children.Add(panel);
            DockPanel.SetDock(panel, Dock.Bottom);

            // Find bar - must be added first (before dialog tree) to match PyKotor layout
            SetupGoToBar();
            if (_goToBar != null)
            {
                panel.Children.Insert(0, _goToBar);
            }
            SetupFindBar();
            if (_findBar != null)
            {
                panel.Children.Insert(0, _findBar);
            }

            // Initialize file-level properties (root DLG fields)
            _voIdEdit = new TextBox();
            _voIdEdit.LostFocus += (s, e) => OnFilePropertyChanged();
            _ambientTrackCombo = new ComboBox { IsEditable = true };
            _ambientTrackCombo.LostFocus += (s, e) => OnFilePropertyChanged();
            var filePropertiesPanel = new StackPanel();
            filePropertiesPanel.Children.Add(new TextBlock { Text = "File Properties" });
            var voIdPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            voIdPanel.Children.Add(new TextBlock { Text = "Voiceover ID:", Width = 120 });
            voIdPanel.Children.Add(_voIdEdit);
            filePropertiesPanel.Children.Add(voIdPanel);
            var ambientTrackPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            ambientTrackPanel.Children.Add(new TextBlock { Text = "Ambient Track:", Width = 120 });
            ambientTrackPanel.Children.Add(_ambientTrackCombo);
            filePropertiesPanel.Children.Add(ambientTrackPanel);

            // Initialize conversation type combo box
            _conversationSelect = new ComboBox();
            _conversationSelect.Items.Add("Human");
            _conversationSelect.Items.Add("Computer");
            _conversationSelect.Items.Add("Other");
            _conversationSelect.SelectedIndex = 0;
            _conversationSelect.SelectionChanged += (s, e) => OnFilePropertyChanged();
            var conversationPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            conversationPanel.Children.Add(new TextBlock { Text = "Conversation Type:", Width = 120 });
            conversationPanel.Children.Add(_conversationSelect);
            filePropertiesPanel.Children.Add(conversationPanel);

            // Initialize computer type combo box
            _computerSelect = new ComboBox();
            _computerSelect.Items.Add("Modern");
            _computerSelect.Items.Add("Ancient");
            _computerSelect.SelectedIndex = 0;
            _computerSelect.SelectionChanged += (s, e) => OnFilePropertyChanged();
            var computerPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            computerPanel.Children.Add(new TextBlock { Text = "Computer Type:", Width = 120 });
            computerPanel.Children.Add(_computerSelect);
            filePropertiesPanel.Children.Add(computerPanel);

            // Initialize entry delay spin box
            _entryDelaySpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Width = 120 };
            _entryDelaySpin.ValueChanged += (s, e) => OnFilePropertyChanged();
            var entryDelayPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            entryDelayPanel.Children.Add(new TextBlock { Text = "Entry Delay:", Width = 120 });
            entryDelayPanel.Children.Add(_entryDelaySpin);
            filePropertiesPanel.Children.Add(entryDelayPanel);

            // Initialize reply delay spin box
            _replyDelaySpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Width = 120 };
            _replyDelaySpin.ValueChanged += (s, e) => OnFilePropertyChanged();
            var replyDelayPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            replyDelayPanel.Children.Add(new TextBlock { Text = "Reply Delay:", Width = 120 });
            replyDelayPanel.Children.Add(_replyDelaySpin);
            filePropertiesPanel.Children.Add(replyDelayPanel);

            // Initialize on abort combo box (ResRef)
            _onAbortCombo = new ComboBox { IsEditable = true };
            _onAbortCombo.LostFocus += (s, e) => OnFilePropertyChanged();
            var onAbortPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            onAbortPanel.Children.Add(new TextBlock { Text = "On Abort Script:", Width = 120 });
            onAbortPanel.Children.Add(_onAbortCombo);
            filePropertiesPanel.Children.Add(onAbortPanel);

            // Initialize on end combo box (ResRef)
            _onEndEdit = new ComboBox { IsEditable = true };
            _onEndEdit.LostFocus += (s, e) => OnFilePropertyChanged();
            var onEndPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            onEndPanel.Children.Add(new TextBlock { Text = "On End Script:", Width = 120 });
            onEndPanel.Children.Add(_onEndEdit);
            filePropertiesPanel.Children.Add(onEndPanel);

            // Initialize camera model combo box (ResRef)
            _cameraModelSelect = new ComboBox { IsEditable = true };
            _cameraModelSelect.LostFocus += (s, e) => OnFilePropertyChanged();
            var cameraModelPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            cameraModelPanel.Children.Add(new TextBlock { Text = "Camera Model:", Width = 120 });
            cameraModelPanel.Children.Add(_cameraModelSelect);
            filePropertiesPanel.Children.Add(cameraModelPanel);

            // Initialize file-level checkboxes
            _unequipHandsCheckbox = new CheckBox { Content = "Unequip Hands" };
            _unequipHandsCheckbox.Checked += (s, e) => OnFilePropertyChanged();
            _unequipHandsCheckbox.Unchecked += (s, e) => OnFilePropertyChanged();
            filePropertiesPanel.Children.Add(_unequipHandsCheckbox);

            _unequipAllCheckbox = new CheckBox { Content = "Unequip All" };
            _unequipAllCheckbox.Checked += (s, e) => OnFilePropertyChanged();
            _unequipAllCheckbox.Unchecked += (s, e) => OnFilePropertyChanged();
            filePropertiesPanel.Children.Add(_unequipAllCheckbox);

            _skippableCheckbox = new CheckBox { Content = "Skippable" };
            _skippableCheckbox.Checked += (s, e) => OnFilePropertyChanged();
            _skippableCheckbox.Unchecked += (s, e) => OnFilePropertyChanged();
            filePropertiesPanel.Children.Add(_skippableCheckbox);

            _animatedCutCheckbox = new CheckBox { Content = "Animated Cut" };
            _animatedCutCheckbox.Checked += (s, e) => OnFilePropertyChanged();
            _animatedCutCheckbox.Unchecked += (s, e) => OnFilePropertyChanged();
            filePropertiesPanel.Children.Add(_animatedCutCheckbox);

            _oldHitCheckbox = new CheckBox { Content = "Old Hit Check" };
            _oldHitCheckbox.Checked += (s, e) => OnFilePropertyChanged();
            _oldHitCheckbox.Unchecked += (s, e) => OnFilePropertyChanged();
            filePropertiesPanel.Children.Add(_oldHitCheckbox);

            panel.Children.Add(filePropertiesPanel);

            // Initialize dialog tree view
            _dialogTree = new TreeView();
            _dialogTree.SelectionChanged += (s, e) => SyncSelectionFromTree();
            _dialogTree.KeyDown += (s, e) => OnKeyDownFromTreeView(e);
            _dialogTree.DoubleTapped += OnDialogTreeDoubleTapped;

            // Setup context menu for dialog tree
            SetupDialogTreeContextMenu();

            panel.Children.Add(_dialogTree);

            // Setup left dock widget (orphaned nodes and pinned items lists)
            SetupLeftDockWidget();
            if (_leftDockWidget != null)
            {
                panel.Children.Add(_leftDockWidget);
            }

            // Initialize link condition widgets
            _condition1ResrefEdit = new ComboBox { IsEditable = true };
            _condition1ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            _condition2ResrefEdit = new ComboBox { IsEditable = true };
            _condition2ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            _logicSpin = new NumericUpDown { Minimum = 0, Maximum = 1, Value = 0 };
            _logicSpin.ValueChanged += (s, e) => OnNodeUpdate();

            // Initialize condition parameter widgets (K2-specific fields, but available in UI)
            _condition1Param1Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition1Param1Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition1Param2Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition1Param2Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition1Param3Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition1Param3Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition1Param4Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition1Param4Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition1Param5Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition1Param5Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition1Param6Edit = new TextBox();
            _condition1Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            _condition1NotCheckbox = new CheckBox();
            _condition1NotCheckbox.Checked += (s, e) => OnNodeUpdate();
            _condition1NotCheckbox.Unchecked += (s, e) => OnNodeUpdate();

            _condition2Param1Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition2Param1Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition2Param2Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition2Param2Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition2Param3Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition2Param3Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition2Param4Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition2Param4Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition2Param5Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _condition2Param5Spin.ValueChanged += (s, e) => OnNodeUpdate();
            _condition2Param6Edit = new TextBox();
            _condition2Param6Edit.LostFocus += (s, e) => OnNodeUpdate();
            _condition2NotCheckbox = new CheckBox();
            _condition2NotCheckbox.Checked += (s, e) => OnNodeUpdate();
            _condition2NotCheckbox.Unchecked += (s, e) => OnNodeUpdate();

            var linkPanel = new StackPanel();
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 ResRef:" });
            linkPanel.Children.Add(_condition1ResrefEdit);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param1:" });
            linkPanel.Children.Add(_condition1Param1Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param2:" });
            linkPanel.Children.Add(_condition1Param2Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param3:" });
            linkPanel.Children.Add(_condition1Param3Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param4:" });
            linkPanel.Children.Add(_condition1Param4Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param5:" });
            linkPanel.Children.Add(_condition1Param5Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Param6:" });
            linkPanel.Children.Add(_condition1Param6Edit);
            linkPanel.Children.Add(_condition1NotCheckbox);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 Not" });

            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 ResRef:" });
            linkPanel.Children.Add(_condition2ResrefEdit);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param1:" });
            linkPanel.Children.Add(_condition2Param1Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param2:" });
            linkPanel.Children.Add(_condition2Param2Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param3:" });
            linkPanel.Children.Add(_condition2Param3Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param4:" });
            linkPanel.Children.Add(_condition2Param4Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param5:" });
            linkPanel.Children.Add(_condition2Param5Spin);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Param6:" });
            linkPanel.Children.Add(_condition2Param6Edit);
            linkPanel.Children.Add(_condition2NotCheckbox);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 Not" });

            linkPanel.Children.Add(new TextBlock { Text = "Logic:" });
            linkPanel.Children.Add(_logicSpin);
            panel.Children.Add(linkPanel);

            // Initialize script parameter widgets (K2-specific)
            // K2-specific: ActionParam1 field only exists in KotOR 2 (k2_win_gog_aspyr_swkotor2.exe: 0x005ea880)
            // Aurora (NWN) and Eclipse (DA/ME) use base DLG format without K2 extensions
            _script1Param1Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script1Param1Spin.ValueChanged += (s, e) => OnNodeUpdate();

            _script1Param1Panel = new StackPanel();
            _script1Param1Panel.Children.Add(new TextBlock { Text = Localization.Tr("Script1 Param1 (K2 only):") });
            _script1Param1Panel.Children.Add(_script1Param1Spin);
            panel.Children.Add(_script1Param1Panel);

            // Initialize script2 parameter widgets (K2-specific)
            // K2-specific: ActionParam1b, ActionParam2b, ActionParam3b, ActionParam4b, ActionParam5b fields only exist in KotOR 2 (k2_win_gog_aspyr_swkotor2.exe: 0x005ea880)
            // Aurora (NWN) and Eclipse (DA/ME) use base DLG format without K2 extensions
            _script2Param1Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script2Param1Spin.ValueChanged += (s, e) => OnNodeUpdate();

            _script2Param2Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script2Param2Spin.ValueChanged += (s, e) => OnNodeUpdate();

            _script2Param3Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script2Param3Spin.ValueChanged += (s, e) => OnNodeUpdate();

            _script2Param4Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script2Param4Spin.ValueChanged += (s, e) => OnNodeUpdate();

            _script2Param5Spin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _script2Param5Spin.ValueChanged += (s, e) => OnNodeUpdate();

            var script2ParamPanel = new StackPanel();
            script2ParamPanel.Children.Add(new TextBlock { Text = "Script2 Param1 (K2 only):" });
            script2ParamPanel.Children.Add(_script2Param1Spin);
            script2ParamPanel.Children.Add(new TextBlock { Text = "Script2 Param2 (K2 only):" });
            script2ParamPanel.Children.Add(_script2Param2Spin);
            script2ParamPanel.Children.Add(new TextBlock { Text = "Script2 Param3 (K2 only):" });
            script2ParamPanel.Children.Add(_script2Param3Spin);
            script2ParamPanel.Children.Add(new TextBlock { Text = "Script2 Param4 (K2 only):" });
            script2ParamPanel.Children.Add(_script2Param4Spin);
            script2ParamPanel.Children.Add(new TextBlock { Text = "Script2 Param5 (K2 only):" });
            script2ParamPanel.Children.Add(_script2Param5Spin);
            panel.Children.Add(script2ParamPanel);

            // Initialize node timing widgets
            _delaySpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = -1 };
            _delaySpin.ValueChanged += (s, e) => OnNodeUpdate();

            _waitFlagSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _waitFlagSpin.ValueChanged += (s, e) => OnNodeUpdate();

            _fadeTypeSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue, Value = 0 };
            _fadeTypeSpin.ValueChanged += (s, e) => OnNodeUpdate();

            var timingPanel = new StackPanel();
            timingPanel.Children.Add(new TextBlock { Text = "Delay:" });
            timingPanel.Children.Add(_delaySpin);
            timingPanel.Children.Add(new TextBlock { Text = "Wait Flags:" });
            timingPanel.Children.Add(_waitFlagSpin);
            timingPanel.Children.Add(new TextBlock { Text = "Fade Type:" });
            timingPanel.Children.Add(_fadeTypeSpin);
            panel.Children.Add(timingPanel);

            // Initialize camera widgets
            InitializeCameraWidgets();

            var cameraPanel = new StackPanel();
            cameraPanel.Children.Add(new TextBlock { Text = "Camera ID:" });
            cameraPanel.Children.Add(_cameraIdSpin);
            cameraPanel.Children.Add(new TextBlock { Text = "Camera Animation:" });
            cameraPanel.Children.Add(_cameraAnimSpin);
            cameraPanel.Children.Add(new TextBlock { Text = "Camera Angle:" });
            cameraPanel.Children.Add(_cameraAngleSelect);
            cameraPanel.Children.Add(new TextBlock { Text = "Camera Effect:" });
            cameraPanel.Children.Add(_cameraEffectSelect);
            cameraPanel.Children.Add(new TextBlock { Text = "Emotion:" });
            cameraPanel.Children.Add(_emotionSelect);
            cameraPanel.Children.Add(new TextBlock { Text = "Expression:" });
            cameraPanel.Children.Add(_expressionSelect);
            panel.Children.Add(cameraPanel);

            // Initialize sound combo box
            _soundComboBox = new ComboBox { IsEditable = true };
            _soundComboBox.LostFocus += (s, e) => OnNodeUpdate();

            _soundCheckbox = new CheckBox { Content = Localization.Tr("Exists") };
            _soundCheckbox.Checked += (s, e) => OnNodeUpdate();
            _soundCheckbox.Unchecked += (s, e) => OnNodeUpdate();

            var soundPanel = new StackPanel();
            soundPanel.Children.Add(new TextBlock { Text = "Sound ResRef:" });
            soundPanel.Children.Add(_soundComboBox);
            soundPanel.Children.Add(_soundCheckbox);
            panel.Children.Add(soundPanel);

            // Initialize voice combo box
            _voiceComboBox = new ComboBox { IsEditable = true };
            _voiceComboBox.LostFocus += (s, e) => OnNodeUpdate();
            var voicePanel = new StackPanel();
            voicePanel.Children.Add(new TextBlock { Text = "Voice ResRef:" });
            voicePanel.Children.Add(_voiceComboBox);
            panel.Children.Add(voicePanel);

            // Initialize listener text box
            _listenerEdit = new TextBox();
            _listenerEdit.LostFocus += (s, e) => OnNodeUpdate();
            var listenerPanel = new StackPanel();
            listenerPanel.Children.Add(new TextBlock { Text = "Listener:" });
            listenerPanel.Children.Add(_listenerEdit);
            panel.Children.Add(listenerPanel);

            // Initialize quest widgets
            _questEdit = new TextBox();
            _questEdit.LostFocus += (s, e) => OnNodeUpdate();
            var questPanel = new StackPanel();
            questPanel.Children.Add(new TextBlock { Text = "Quest:" });
            questPanel.Children.Add(_questEdit);
            panel.Children.Add(questPanel);

            _questEntrySpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Value = 0 };
            _questEntrySpin.ValueChanged += (s, e) => OnNodeUpdate();
            var questEntryPanel = new StackPanel();
            questEntryPanel.Children.Add(new TextBlock { Text = "Quest Entry:" });
            questEntryPanel.Children.Add(_questEntrySpin);
            panel.Children.Add(questEntryPanel);

            _nodeIdSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Value = 0 };
            _nodeIdSpin.ValueChanged += (s, e) => OnNodeUpdate();
            var nodeIdPanel = new StackPanel();
            nodeIdPanel.Children.Add(new TextBlock { Text = "Node ID:" });
            nodeIdPanel.Children.Add(_nodeIdSpin);
            panel.Children.Add(nodeIdPanel);

            _alienRaceNodeSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Value = 0 };
            _alienRaceNodeSpin.ValueChanged += (s, e) => OnNodeUpdate();
            var alienRacePanel = new StackPanel();
            alienRacePanel.Children.Add(new TextBlock { Text = "Alien Race Node:" });
            alienRacePanel.Children.Add(_alienRaceNodeSpin);
            panel.Children.Add(alienRacePanel);

            _postProcSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue, Value = 0 };
            _postProcSpin.ValueChanged += (s, e) => OnNodeUpdate();
            var postProcPanel = new StackPanel();
            postProcPanel.Children.Add(new TextBlock { Text = "Post Proc Node:" });
            postProcPanel.Children.Add(_postProcSpin);
            panel.Children.Add(postProcPanel);

            _nodeUnskippableCheckbox = new CheckBox { Content = "Unskippable" };
            _nodeUnskippableCheckbox.Checked += (s, e) => OnNodeUpdate();
            _nodeUnskippableCheckbox.Unchecked += (s, e) => OnNodeUpdate();
            var unskippablePanel = new StackPanel();
            unskippablePanel.Children.Add(_nodeUnskippableCheckbox);
            panel.Children.Add(unskippablePanel);

            // Initialize plot widgets
            // VERIFIED: PlotXpPercentage field aligns with k2_win_gog_aspyr_swkotor2.exe DLG format
            // - Field name "PlotXPPercentage" confirmed in k2_win_gog_aspyr_swkotor2.exe string table @ 0x007c35cc (DialogueManager.cs:1098)
            // - Field type: float (matches GFF Single type, default 1.0f in DLGNode.cs)
            // - GFF I/O: DLGHelper.cs reads as Acquire("PlotXPPercentage", 0.0f), writes conditionally when != 0.0f
            // - UI: NumericUpDown (0-100 int) converted to float for storage
            // - Round-trip tested: TestDlgEditorManipulatePlotXpRoundtrip verifies 0,25,50,75,100 values
            // - Cross-format consistency: Also implemented in CNV format (CNVHelper.cs)
            // Ghidra project: C:\Users\boden\Andastra Ghidra Project.gpr
            _plotIndexCombo = new ComboBox2DA();
            _plotIndexCombo.SelectionChanged += (s, e) => OnNodeUpdate();
            var plotIndexPanel = new StackPanel();
            plotIndexPanel.Children.Add(new TextBlock { Text = "Plot Index:" });
            plotIndexPanel.Children.Add(_plotIndexCombo);
            panel.Children.Add(plotIndexPanel);

            _plotXpSpin = new NumericUpDown { Minimum = 0, Maximum = 100, Value = 0, Increment = 1 };
            _plotXpSpin.ValueChanged += (s, e) => OnNodeUpdate();
            var plotXpPanel = new StackPanel();
            plotXpPanel.Children.Add(new TextBlock { Text = "Plot XP %:" });
            plotXpPanel.Children.Add(_plotXpSpin);
            panel.Children.Add(plotXpPanel);

            // Initialize script1 and script2 combo boxes
            _script1ResrefEdit = new ComboBox { IsEditable = true };
            _script1ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            var script1Panel = new StackPanel();
            script1Panel.Children.Add(new TextBlock { Text = "Script1 ResRef:" });
            script1Panel.Children.Add(_script1ResrefEdit);
            panel.Children.Add(script1Panel);

            _script2ResrefEdit = new ComboBox { IsEditable = true };
            _script2ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            var script2Panel = new StackPanel();
            script2Panel.Children.Add(new TextBlock { Text = "Script2 ResRef:" });
            script2Panel.Children.Add(_script2ResrefEdit);
            panel.Children.Add(script2Panel);

            // Initialize animation UI controls
            _animsList = new ListBox
            {
                Height = 150,
                Margin = new Thickness(0, 5, 0, 5)
            };

            _addAnimButton = new Button { Content = "Add" };
            _addAnimButton.Click += (s, e) => OnAddAnimClicked();

            _removeAnimButton = new Button { Content = "Remove" };
            _removeAnimButton.Click += (s, e) => OnRemoveAnimClicked();

            _editAnimButton = new Button { Content = "Edit" };
            _editAnimButton.Click += (s, e) => OnEditAnimClicked();

            var animPanel = new StackPanel
            {
                Margin = new Thickness(0, 10, 0, 0),
                Spacing = 5
            };

            // Add label matching PyKotor: curAnimsLabel "Current Animations"
            var animLabel = new TextBlock
            {
                Text = "Current Animations",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5)
            };
            animPanel.Children.Add(animLabel);

            animPanel.Children.Add(_animsList);

            // Button panel matching PyKotor: horizontalLayout_animsButtons
            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 5,
                Margin = new Thickness(0, 5, 0, 0)
            };
            buttonPanel.Children.Add(_addAnimButton);
            buttonPanel.Children.Add(_removeAnimButton);
            buttonPanel.Children.Add(_editAnimButton);
            animPanel.Children.Add(buttonPanel);

            panel.Children.Add(animPanel);
        }

        /// <summary>
        /// Sets up the context menu for the dialog tree.
        /// </summary>
        private void SetupDialogTreeContextMenu()
        {
            if (_dialogTree == null)
            {
                return;
            }

            // In Avalonia, we handle context menu requests via the ContextRequested event
            // This allows us to create dynamic context menus based on the selected item
            _dialogTree.ContextRequested += (sender, e) =>
            {
                // Get the item at the pointer position
                Point? point = e.TryGetPosition(_dialogTree, out Point pos) ? pos : (Point?)null;
                if (!point.HasValue)
                {
                    return;
                }

                // Find the item at the pointer position
                DLGStandardItem item = null;
                var selectedItem = _dialogTree.SelectedItem;
                if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                {
                    item = dlgItem;
                }
                else if (selectedItem is DLGStandardItem dlgItemDirect)
                {
                    item = dlgItemDirect;
                }

                if (item != null)
                {
                    var contextMenu = GetLinkContextMenu(_dialogTree, item);
                    if (contextMenu != null)
                    {
                        contextMenu.Open(_dialogTree);
                        e.Handled = true;
                    }
                }
                else
                {
                    // No item selected - show Add Entry or Reset Tree (matching PyKotor: if not self._focused: add_entry else: reset_tree)
                    var contextMenu = new ContextMenu();
                    if (!_focused)
                    {
                        var addEntryItem = new MenuItem { Header = Localization.Tr("Add Entry") };
                        addEntryItem.Click += (s, args) => AddRootNode();
                        contextMenu.Items.Add(addEntryItem);
                    }
                    else
                    {
                        var resetTreeItem = new MenuItem { Header = Localization.Tr("Reset Tree") };
                        resetTreeItem.Click += (s, args) => LoadDLG(_coreDlg);
                        contextMenu.Items.Add(resetTreeItem);
                    }
                    contextMenu.Open(_dialogTree);
                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// Gets the context menu for a dialog tree item.
        /// </summary>
        /// <param name="sourceWidget">The source widget (TreeView or DLGListWidget).</param>
        /// <param name="item">The item to get the context menu for.</param>
        /// <returns>The context menu for the item.</returns>
        public ContextMenu GetLinkContextMenu(Control sourceWidget, DLGStandardItem item)
        {
            if (item?.Link == null)
            {
                return null;
            }
            return GetLinkContextMenuCore(sourceWidget, item.Link, item);
        }

        /// <summary>
        /// Handles context menu request for orphaned/pinned list widgets. Shows same menu as tree when an item is selected,
        /// plus list-specific items: Jump to Tree, Insert Orphan at Selected Point (orphaned only), Unpin, Clear List.
        /// </summary>
        private void OnListWidgetContextRequested(object sender, ContextRequestedEventArgs e)
        {
            var list = sender as DLGListWidget;
            if (list == null)
            {
                return;
            }
            var item = list.SelectedItem as DLGListWidgetItem;
            if (item?.Link == null)
            {
                return;
            }
            var menu = GetLinkContextMenu(list, item);
            if (menu == null)
            {
                return;
            }
            menu.Items.Add(new Separator());
            var jumpToTreeItem = new MenuItem { Header = Localization.Tr("Jump to Tree") };
            jumpToTreeItem.Click += (s, ev) => JumpToNode(item.Link);
            menu.Items.Add(jumpToTreeItem);
            bool isOrphanedList = list == _orphanedNodesList;
            bool treeHasSelection = _dialogTree?.SelectedItem != null;
            if (isOrphanedList && treeHasSelection)
            {
                var insertOrphanItem = new MenuItem { Header = Localization.Tr("Insert Orphan at Selected Point") };
                insertOrphanItem.Click += (s, ev) => RestoreOrphanedNode(item);
                menu.Items.Add(insertOrphanItem);
            }
            menu.Items.Add(new Separator());
            var unpinItem = new MenuItem { Header = Localization.Tr("Unpin") };
            unpinItem.Click += (s, ev) =>
            {
                list.RemoveItem(item);
            };
            menu.Items.Add(unpinItem);
            menu.Items.Add(new Separator());
            var clearListItem = new MenuItem { Header = Localization.Tr("Clear List") };
            clearListItem.Click += (s, ev) => list.Clear();
            menu.Items.Add(clearListItem);
            menu.Open(list);
            e.Handled = true;
        }

        /// <summary>
        /// Gets the context menu for a list widget item (orphaned or pinned list).
        /// </summary>
        public ContextMenu GetLinkContextMenu(Control sourceWidget, DLGListWidgetItem listItem)
        {
            if (listItem?.Link == null)
            {
                return null;
            }
            return GetLinkContextMenuCore(sourceWidget, listItem.Link, null);
        }

        /// <summary>
        /// Core context menu builder. Tree-only actions use treeItem when non-null (tree context); list context passes null.
        /// </summary>
        private ContextMenu GetLinkContextMenuCore(Control sourceWidget, DLGLink link, DLGStandardItem treeItem)
        {
            if (link == null)
            {
                return null;
            }

            _ = CheckClipboardForJsonNodeAsync();

            bool notAnOrphan = sourceWidget != _orphanedNodesList;
            string nodeType = link.Node is DLGEntry ? "Entry" : "Reply";
            string otherNodeType = link.Node is DLGEntry ? "Reply" : "Entry";

            var menu = new ContextMenu();
            var menuItems = new List<MenuItem>();

            var editTextItem = new MenuItem { Header = Localization.Tr("Edit Text") };
            editTextItem.Click += (s, e) =>
            {
                Control sourceWidgetForEdit = sourceWidget ?? _dialogTree;
                List<object> indexes = null;
                if (sourceWidgetForEdit == _dialogTree)
                {
                    indexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
                }
                else if (sourceWidgetForEdit is DLGListWidget listWidget)
                {
                    indexes = GetSelectedIndexesFromListWidget(listWidget);
                }
                EditText(null, indexes, sourceWidgetForEdit);
            };
            menuItems.Add(editTextItem);

            var focusItem = new MenuItem { Header = Localization.Tr("Focus") };
            focusItem.Click += (s, e) => FocusOnNode(link);
            focusItem.IsEnabled = link.Node?.Links != null && link.Node.Links.Count > 0;
            focusItem.IsVisible = notAnOrphan;
            menuItems.Add(focusItem);

            var findReferencesItem = new MenuItem { Header = Localization.Tr("Find References") };
            findReferencesItem.Click += (s, e) => FindReferences(link);
            findReferencesItem.IsVisible = notAnOrphan;
            menuItems.Add(findReferencesItem);

            // Find References in Installation (dialog resref) - matching PyKotor: find_installation_refs_action
            string dialogResref = _resname?.Trim();
            var findInstallationRefsItem = new MenuItem { Header = Localization.Tr("Find References in Installation...") };
            findInstallationRefsItem.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(dialogResref))
                {
                    FindDialogReferencesInInstallation(dialogResref);
                }
            };
            findInstallationRefsItem.IsVisible = notAnOrphan && _installation != null && !string.IsNullOrEmpty(dialogResref);
            menuItems.Add(findInstallationRefsItem);

            int refCount = CountItemRefs(link?.Node);
            bool isCopy = refCount > 1;
            var jumpToOriginalItem = new MenuItem { Header = Localization.Tr("Jump to Original") };
            jumpToOriginalItem.Click += (s, e) => JumpToOriginal();
            jumpToOriginalItem.IsVisible = notAnOrphan && sourceWidget is TreeView && isCopy;
            menuItems.Add(jumpToOriginalItem);

            bool pinned = IsPinned(link);
            var pinItem = new MenuItem { Header = Localization.Tr("Pin") };
            pinItem.Click += (s, e) => PinItem(link);
            pinItem.IsVisible = !pinned;
            menuItems.Add(pinItem);
            var unpinItem = new MenuItem { Header = Localization.Tr("Unpin") };
            unpinItem.Click += (s, e) => UnpinItem(link);
            unpinItem.IsVisible = pinned;
            menuItems.Add(unpinItem);

            var playMenu = new MenuItem { Header = Localization.Tr("Play") };
            var playSubMenuItems = new List<MenuItem>();
            string soundResrefForEnable = link?.Node?.Sound?.ToString() ?? "";
            var playSoundItem = new MenuItem { Header = Localization.Tr("Play Sound") };
            playSoundItem.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(soundResrefForEnable))
                {
                    PlaySound(soundResrefForEnable, new[] { SearchLocation.SOUND, SearchLocation.VOICE });
                }
            };
            playSoundItem.IsEnabled = !string.IsNullOrWhiteSpace(soundResrefForEnable);
            playSubMenuItems.Add(playSoundItem);
            string voiceResrefForEnable = link?.Node?.VoResRef?.ToString() ?? "";
            var playVoiceItem = new MenuItem { Header = Localization.Tr("Play Voice") };
            playVoiceItem.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(voiceResrefForEnable))
                {
                    PlaySound(voiceResrefForEnable, new[] { SearchLocation.VOICE });
                }
            };
            playVoiceItem.IsEnabled = !string.IsNullOrWhiteSpace(voiceResrefForEnable);
            playSubMenuItems.Add(playVoiceItem);
            playMenu.IsEnabled = !string.IsNullOrWhiteSpace(soundResrefForEnable) || !string.IsNullOrWhiteSpace(voiceResrefForEnable);
            foreach (var subItem in playSubMenuItems)
            {
                playMenu.Items.Add(subItem);
            }
            menuItems.Add(playMenu);

            menuItems.Add(new MenuItem { Header = "-" });

            var copyNodeItem = new MenuItem { Header = $"Copy {nodeType} to Clipboard" };
            copyNodeItem.Click += async (s, e) =>
            {
                if (link != null)
                {
                    await _model.CopyLinkAndNode(link, this);
                }
            };
            menuItems.Add(copyNodeItem);

            var copyGffPathItem = new MenuItem { Header = "Copy GFF Path" };
            copyGffPathItem.Click += async (s, e) =>
            {
                if (link?.Node != null)
                {
                    await CopyPath(link.Node);
                }
            };
            copyGffPathItem.IsVisible = notAnOrphan;
            menuItems.Add(copyGffPathItem);

            menuItems.Add(new MenuItem { Header = "-" });

            if (sourceWidget is TreeView && treeItem != null)
            {
                var expandToRootItem = new MenuItem { Header = Localization.Tr("Expand to Root") };
                expandToRootItem.Click += (s, e) => ExpandToRoot(treeItem);
                menuItems.Add(expandToRootItem);

                var expandAllChildrenItem = new MenuItem { Header = Localization.Tr("Expand All Children") };
                expandAllChildrenItem.Click += (s, e) =>
                {
                    var tvi = FindTreeViewItem(_dialogTree.ItemsSource, treeItem);
                    if (tvi != null)
                    {
                        SetExpandRecursivelyInternal(treeItem, tvi, new HashSet<DLGNode>(), true, 11, 0, true);
                    }
                };
                menuItems.Add(expandAllChildrenItem);

                var collapseAllChildrenItem = new MenuItem { Header = Localization.Tr("Collapse All Children") };
                collapseAllChildrenItem.Click += (s, e) =>
                {
                    var tvi = FindTreeViewItem(_dialogTree.ItemsSource, treeItem);
                    if (tvi != null)
                    {
                        SetExpandRecursivelyInternal(treeItem, tvi, new HashSet<DLGNode>(), false, 11, 0, true);
                    }
                };
                menuItems.Add(collapseAllChildrenItem);
                menuItems.Add(new MenuItem { Header = "-" });

                var pasteLinkItem = new MenuItem { Header = $"Paste {otherNodeType} from Clipboard as Link" };
                var pasteNewItem = new MenuItem { Header = $"Paste {otherNodeType} from Clipboard as Deep Copy" };
                if (_copy == null)
                {
                    pasteLinkItem.IsEnabled = false;
                    pasteNewItem.IsEnabled = false;
                }
                else
                {
                    pasteLinkItem.Header = _copy.Node is DLGEntry ? Localization.Tr("Paste Entry from Clipboard as Link") : Localization.Tr("Paste Reply from Clipboard as Link");
                    pasteNewItem.Header = _copy.Node is DLGEntry ? Localization.Tr("Paste Entry from Clipboard as Deep Copy") : Localization.Tr("Paste Reply from Clipboard as Deep Copy");
                    if (nodeType == _copy.Node?.GetType().Name)
                    {
                        pasteLinkItem.IsEnabled = false;
                        pasteNewItem.IsEnabled = false;
                    }
                }
                pasteLinkItem.Click += (s, e) => _actionHistory.Apply(new PasteItemAction(treeItem, null, _copy, false));
                pasteNewItem.Click += (s, e) => _actionHistory.Apply(new PasteItemAction(treeItem, null, _copy, true));
                menuItems.Add(pasteLinkItem);
                menuItems.Add(pasteNewItem);
                menuItems.Add(new MenuItem { Header = "-" });

                var addNodeItem = new MenuItem { Header = otherNodeType == "Entry" ? Localization.Tr("Add Entry") : Localization.Tr("Add Reply") };
                addNodeItem.Click += (s, e) => AddChildToParentItem(treeItem);
                menuItems.Add(addNodeItem);
                menuItems.Add(new MenuItem { Header = "-" });

                var moveUpItem = new MenuItem { Header = Localization.Tr("Move Up") };
                moveUpItem.Click += (s, e) => { _model.ShiftItem(treeItem, -1); UpdateTreeView(); };
                menuItems.Add(moveUpItem);
                var moveDownItem = new MenuItem { Header = Localization.Tr("Move Down") };
                moveDownItem.Click += (s, e) => { _model.ShiftItem(treeItem, 1); UpdateTreeView(); };
                menuItems.Add(moveDownItem);
                menuItems.Add(new MenuItem { Header = "-" });

                var removeLinkItem = new MenuItem { Header = nodeType == "Entry" ? Localization.Tr("Remove Entry") : Localization.Tr("Remove Reply") };
                removeLinkItem.Click += (s, e) => RemoveLink(treeItem);  // undoable via RemoveLinkAction
                menuItems.Add(removeLinkItem);
                menuItems.Add(new MenuItem { Header = "-" });
            }

            var deleteAllReferencesItem = new MenuItem
            {
                Header = nodeType == "Entry" ? Localization.Tr("Delete ALL References to Entry") : Localization.Tr("Delete ALL References to Reply")
            };
            deleteAllReferencesItem.Click += (s, e) =>
            {
                if (link?.Node != null)
                {
                    _model.DeleteNodeEverywhere(link.Node);
                    UpdateTreeView();
                }
            };
            deleteAllReferencesItem.IsVisible = notAnOrphan;
            menuItems.Add(deleteAllReferencesItem);

            foreach (var mi in menuItems)
            {
                menu.Items.Add(mi);
            }
            return menu;
        }

        /// <summary>
        /// Checks the clipboard for a JSON node and sets _copy if found.
        /// </summary>
        private async Task CheckClipboardForJsonNodeAsync()
        {
            try
            {
                var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
                if (topLevel?.Clipboard == null)
                {
                    return;
                }

                // Avalonia clipboard access is async, so we use GetTextAsync
                string clipboardText = await topLevel.Clipboard.GetTextAsync();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    return;
                }

                Dictionary<string, object> nodeData = JsonSerializer.Deserialize<Dictionary<string, object>>(clipboardText);
                if (nodeData != null && nodeData.ContainsKey("type"))
                {
                    // Parse the JSON data into a DLGLink
                    Dictionary<string, object> nodeMap = new Dictionary<string, object>();
                    _copy = DLGLink.FromDict(nodeData, nodeMap);
                }
            }
            catch (JsonException)
            {
                // Silently ignore JSON decode errors (clipboard doesn't contain valid JSON)
            }
            catch (Exception)
            {
                // Silently ignore clipboard errors
            }
        }

        /// <summary>
        /// Removes a link from the parent node and records the action for undo/redo.
        /// </summary>
        /// <param name="item">The item whose link should be removed.</param>
        private void RemoveLink(DLGStandardItem item)
        {
            if (item == null || item.Link == null)
                return;
            var action = new RemoveLinkAction(this, item);
            _actionHistory.Apply(action);
        }

        /// <summary>
        /// Performs the actual removal of a link (used by RemoveLinkAction.Apply and internally). Does not record undo.
        /// </summary>
        internal void RemoveLinkInternal(DLGStandardItem item)
        {
            if (item == null || item.Link == null)
                return;
            var parent = item.Parent;
            if (parent == null)
            {
                _coreDlg?.Starters?.Remove(item.Link);
                _coreDlg?.Touch();
                _model.RemoveStarter(item.Link);
            }
            else
            {
                if (parent.Link?.Node != null)
                {
                    parent.Link.Node.Links.Remove(item.Link);
                    _coreDlg?.Touch();
                    parent.RemoveChild(item);
                }
                if (_model.LinkToItems != null && _model.LinkToItems.ContainsKey(item.Link))
                    _model.LinkToItems.Remove(item.Link);
                if (item.Link.Node != null && _model.NodeToItems != null && _model.NodeToItems.ContainsKey(item.Link.Node))
                {
                    var list = _model.NodeToItems[item.Link.Node];
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i] == item)
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            UpdateTreeView();
        }

        /// <summary>
        /// Restores an orphaned node by inserting it at the currently selected tree position.
        /// </summary>
        /// <param name="orphanListItem">The list item from the orphaned nodes list (its Link is the orphan to restore).</param>
        public void RestoreOrphanedNode(DLGListWidgetItem orphanListItem)
        {
            if (orphanListItem?.Link == null)
            {
                return;
            }
            DLGStandardItem selectedTreeItem = GetSelectedTreeItem();
            if (selectedTreeItem == null)
            {
                MessageBoxManager.GetMessageBoxStandard(
                    Localization.Tr("No target specified"),
                    Localization.Tr("Select a position in the tree to insert this orphan at then try again."),
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Info).ShowWindowAsync();
                return;
            }
            DLGLink oldLink = orphanListItem.Link;
            DLGStandardItem targetParent;
            int intendedRow;
            if (oldLink.Node.GetType() == selectedTreeItem.Link?.Node?.GetType())
            {
                targetParent = selectedTreeItem.Parent;
                intendedRow = selectedTreeItem.GetIndex();
                if (intendedRow < 0)
                {
                    intendedRow = 0;
                }
            }
            else
            {
                targetParent = selectedTreeItem;
                intendedRow = 0;
            }
            string newLinkPath = targetParent == null
                ? $"StartingList\\{intendedRow}"
                : (targetParent.Link?.Node?.Path() ?? "?");
            Tuple<string, string, string> paths = GetItemDlgPaths(orphanListItem);
            string linkParentPath = paths?.Item1 ?? "";
            string linkPartialPath = paths?.Item2 ?? "";
            string linkedToPath = paths?.Item3 ?? "";
            string linkFullPath = string.IsNullOrEmpty(linkParentPath) ? linkPartialPath : $"{linkParentPath}\\{linkPartialPath}";
            string confirmMessage = string.Format(Localization.Tr("The orphan '{0}' (originally linked from {1}) will be newly linked from {2} with this action. Continue?"), linkedToPath, linkFullPath, newLinkPath);
            var confirm = MessageBoxManager.GetMessageBoxStandard(
                Localization.Tr("Restore Orphaned Node"),
                confirmMessage,
                ButtonEnum.YesNo,
                MsBox.Avalonia.Enums.Icon.Question);
            async void ShowAndHandle()
            {
                var result = await confirm.ShowWindowAsync();
                if (result != ButtonResult.Yes)
                {
                    return;
                }
                var nodeMap = new Dictionary<string, object>();
                Dictionary<string, object> linkDict = oldLink.ToDict(nodeMap);
                DLGLink newLink = DLGLink.FromDict(linkDict, nodeMap);
                _actionHistory.Apply(new RestoreOrphanAction(targetParent, intendedRow, newLink, orphanListItem));
            }
            ShowAndHandle();
        }

        /// <summary>
        /// Finds references to the given dialog resref in the installation (UTC, UTP, UTD that reference this DLG).
        /// </summary>
        public void FindDialogReferencesInInstallation(string dialogResref)
        {
            if (_installation == null || string.IsNullOrWhiteSpace(dialogResref))
            {
                return;
            }
            var results = new List<FileResource>();
            string resrefLower = dialogResref.Trim().ToLowerInvariant();
            var typesToSearch = new[] { ResourceType.UTC, ResourceType.UTP, ResourceType.UTD };
            List<FileResource> overrideList = null;
            try
            {
                overrideList = _installation.OverrideResources();
            }
            catch
            {
                overrideList = new List<FileResource>();
            }
            foreach (var restype in typesToSearch)
            {
                foreach (var fileRes in overrideList.Where(r => r != null && r.ResType == restype))
                {
                    try
                    {
                        var rr = _installation.Resource(fileRes.ResName, restype, new[] { SearchLocation.OVERRIDE });
                        byte[] data = rr?.Data;
                        if (data == null || data.Length == 0)
                        {
                            continue;
                        }
                        string convResref = null;
                        if (restype == ResourceType.UTC)
                        {
                            var utc = ResourceAutoHelpers.ReadUtc(data);
                            convResref = utc?.Conversation?.ToString()?.Trim().ToLowerInvariant();
                        }
                        else if (restype == ResourceType.UTP)
                        {
                            var utp = ResourceAutoHelpers.ReadUtp(data);
                            convResref = utp?.Conversation?.ToString()?.Trim().ToLowerInvariant();
                        }
                        else if (restype == ResourceType.UTD)
                        {
                            var utd = ResourceAutoHelpers.ReadUtd(data);
                            convResref = utd?.Conversation?.ToString()?.Trim().ToLowerInvariant();
                        }
                        if (convResref == resrefLower && fileRes != null)
                        {
                            results.Add(fileRes);
                        }
                    }
                    catch
                    {
                        // Skip malformed resources
                    }
                }
            }
            if (results.Count == 0)
            {
                MessageBoxManager.GetMessageBoxStandard(
                    Localization.Tr("No references found"),
                    string.Format(Localization.Tr("No references found for dialog '{0}'."), dialogResref),
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            var dialog = new LoadFromLocationResultDialog(this, results, _installation);
            dialog.Title = string.Format(Localization.Tr("{0} reference(s) found for dialog '{1}'"), results.Count, dialogResref);
            dialog.Show();
        }

        /// <summary>
        /// Gets the currently selected tree item (DLGStandardItem) from the dialog tree, or null.
        /// </summary>
        private DLGStandardItem GetSelectedTreeItem()
        {
            if (_dialogTree?.SelectedItem == null)
            {
                return null;
            }
            if (_dialogTree.SelectedItem is TreeViewItem tvi && tvi.Tag is DLGStandardItem dlgItem)
            {
                return dlgItem;
            }
            if (_dialogTree.SelectedItem is DLGStandardItem direct)
            {
                return direct;
            }
            return null;
        }

        /// <summary>
        /// Sets up the left dock widget containing orphaned nodes and pinned items lists.
        /// </summary>
        private void SetupLeftDockWidget()
        {
            // Create the left dock widget container (only used when content is built in code, not AXAML)
            // In Avalonia, we use a StackPanel instead of QDockWidget
            var leftPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Vertical
            };

            // Orphaned Nodes List: word wrap, custom item template, and drag source (drag to tree to restore)
            _orphanedNodesList = new DLGListWidget(this)
            {
                UseHoverText = false,
                UseWordWrap = true
            };

            // Pinned Items List: same features, multi-selection, drop target for pinning from tree
            _pinnedItemsList = new DLGListWidget(this)
            {
                UseWordWrap = true
            };
            _pinnedItemsList.SelectionMode = SelectionMode.Multiple;

            _orphanedNodesList.ContextRequested += OnListWidgetContextRequested;
            _pinnedItemsList.ContextRequested += OnListWidgetContextRequested;

            SetupPinnedListDragDrop();
            SetupTreeViewDragSource();

            // Add labels and lists to the layout
            leftPanel.Children.Add(new TextBlock { Text = "Orphaned Nodes" });
            leftPanel.Children.Add(_orphanedNodesList);
            leftPanel.Children.Add(new TextBlock { Text = "Pinned Items" });
            leftPanel.Children.Add(_pinnedItemsList);

            _leftDockWidget = leftPanel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // Handle CNV format by converting to DLG for editing
            if (restype == ResourceType.CNV)
            {
                var cnv = CNVHelper.ReadCnv(data);
                _coreDlg = CNVHelper.ToDlg(cnv);
            }
            else if (restype == ResourceType.DLG_TWINE_HTML || restype == ResourceType.DLG_TWINE_JSON)
            {
                string content = Encoding.UTF8.GetString(data);
                _coreDlg = Twine.ReadTwineFromContent(content);
            }
            else
            {
                // DLG, DLG_XML, or DLG_JSON (plaintext); pass restype so XML/JSON are read correctly
                _coreDlg = DLGHelper.ReadDlg(data, 0, -1, restype);
            }
            LoadDLG(_coreDlg);
            RefreshStuntList();
            UpdateUIForGame(); // Update UI visibility after loading (game may have changed)
        }

        /// <summary>
        /// Loads a dialog tree into the UI view.
        /// </summary>
        public void LoadDLG(DLGType dlg)
        {
            if (_dialogTree != null)
            {
                _dialogTree.Background = null; // Reset background color
            }
            _focused = false;

            _coreDlg = dlg;
            _model.ResetModel();

            foreach (DLGLink start in dlg.Starters)
            {
                _model.AddStarter(start);
            }

            // Load file-level properties (root DLG fields)
            if (_voIdEdit != null)
            {
                _voIdEdit.Text = dlg.VoId ?? string.Empty;
            }
            if (_ambientTrackCombo != null)
            {
                string ambientTrackText = dlg.AmbientTrack.ToString();
                _ambientTrackCombo.Text = ambientTrackText;
            }
            if (_cameraModelSelect != null)
            {
                _cameraModelSelect.Text = dlg.CameraModel?.ToString() ?? string.Empty;
            }

            if (_skippableCheckbox != null)
            {
                _skippableCheckbox.IsChecked = dlg.Skippable;
            }
            if (_animatedCutCheckbox != null)
            {
                _animatedCutCheckbox.IsChecked = dlg.AnimatedCut != 0;
            }
            if (_oldHitCheckbox != null)
            {
                _oldHitCheckbox.IsChecked = dlg.OldHitCheck;
            }
            if (_unequipHandsCheckbox != null)
            {
                _unequipHandsCheckbox.IsChecked = dlg.UnequipHands;
            }
            if (_unequipAllCheckbox != null)
            {
                _unequipAllCheckbox.IsChecked = dlg.UnequipItems;
            }

            // Clear undo/redo history when loading a dialog
            _actionHistory.Clear();
            _graphManualPositions = LoadGraphLayout();
            UpdateTreeView();
            RefreshStuntList();
            // Populate orphaned nodes list (nodes not reachable from Starters).
            PopulateOrphanedNodesList();
            // Pre-fill Script #1/2 and Conditional #1/2 with NSS resources scoped to this DLG (override + same module + chitin).
            PopulateScriptAndConditionCombos();
        }

        /// <summary>
        /// Populates Script #1, Script #2, Conditional #1, and Conditional #2 combos with ResourceType.NSS resrefs
        /// from the installation, scoped to the open DLG (override + same module + chitin, or override + chitin if DLG is in override/chitin).
        /// </summary>
        private void PopulateScriptAndConditionCombos()
        {
            if (_installation == null) return;

            var relevant = _installation.GetRelevantResources(ResourceType.NSS, FilepathPublic);
            var resnames = relevant
                .Select(r => r?.ResName?.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.ToLowerInvariant())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (_script1ResrefEdit != null) _script1ResrefEdit.ItemsSource = resnames;
            if (_script2ResrefEdit != null) _script2ResrefEdit.ItemsSource = resnames;
            if (_condition1ResrefEdit != null) _condition1ResrefEdit.ItemsSource = resnames;
            if (_condition2ResrefEdit != null) _condition2ResrefEdit.ItemsSource = resnames;
        }

        /// <summary>
        /// Populates the orphaned nodes list with links whose target node is not reachable from Starters.
        /// </summary>
        private void PopulateOrphanedNodesList()
        {
            if (_orphanedNodesList == null || _coreDlg == null)
            {
                return;
            }

            _orphanedNodesList.Clear();

            // Build set of reachable nodes (BFS from Starters)
            var reachable = new HashSet<DLGNode>();
            var queue = new Queue<DLGLink>();
            foreach (var link in _coreDlg.Starters)
            {
                if (link != null)
                {
                    queue.Enqueue(link);
                }
            }
            while (queue.Count > 0)
            {
                var link = queue.Dequeue();
                if (link?.Node == null || reachable.Contains(link.Node))
                {
                    continue;
                }
                reachable.Add(link.Node);
                if (link.Node.Links != null)
                {
                    foreach (var child in link.Node.Links)
                    {
                        if (child != null)
                        {
                            queue.Enqueue(child);
                        }
                    }
                }
            }

            // Collect all links in the DLG
            var allLinks = new List<DLGLink>();
            if (_coreDlg.Starters != null)
                allLinks.AddRange(_coreDlg.Starters);
            foreach (var entry in _coreDlg.EntryList)
            {
                if (entry?.Links != null)
                {
                    allLinks.AddRange(entry.Links);
                }
            }
            foreach (var reply in _coreDlg.ReplyList)
            {
                if (reply?.Links != null)
                {
                    allLinks.AddRange(reply.Links);
                }
            }

            // Orphan links = links whose target node is not reachable
            foreach (var link in allLinks)
            {
                if (link?.Node == null)
                {
                    continue;
                }
                if (!reachable.Contains(link.Node))
                {
                    var listItem = new DLGListWidgetItem(link);
                    listItem.IsOrphaned = true;
                    _orphanedNodesList.AddItem(listItem);
                    _orphanedNodesList.UpdateItem(listItem);
                }
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // Sync file-level properties (root DLG fields) from UI to CoreDlg before writing
            if (_voIdEdit != null)
            {
                _coreDlg.VoId = _voIdEdit.Text ?? string.Empty;
            }
            if (_cameraModelSelect != null)
            {
                string cameraText = _cameraModelSelect.Text?.Trim() ?? string.Empty;
                _coreDlg.CameraModel = ResRef.IsValid(cameraText) ? new ResRef(cameraText) : ResRef.FromBlank();
            }

            // Handle CNV format by converting DLG to CNV
            if (_restype == ResourceType.CNV)
            {
                // CNV format is only used by Eclipse Engine games
                BioWareGame gameToUse = _installation?.Game ?? BioWareGame.DA;
                if (!gameToUse.IsEclipse())
                {
                    // Default to DA if not Eclipse game
                    gameToUse = BioWareGame.DA;
                }
                var cnv = DLGHelper.ToCnv(_coreDlg);
                byte[] cnvData = CNVHelper.BytesCnv(cnv, gameToUse, ResourceType.CNV);
                return Tuple.Create(cnvData, new byte[0]);
            }

            // Handle Twine format (HTML or JSON)
            if (_restype == ResourceType.DLG_TWINE_HTML || _restype == ResourceType.DLG_TWINE_JSON)
            {
                string format = _restype == ResourceType.DLG_TWINE_JSON ? "json" : "html";
                byte[] twineData = Twine.BytesTwine(_coreDlg, format);
                return Tuple.Create(twineData, new byte[0]);
            }

            // Detect game from installation - supports all engines (Odyssey K1/K2, Aurora NWN, Eclipse DA/DA2/ME)
            // BioWareGame-specific format handling:
            // - K2 (TSL): Extended DLG format with K2-specific fields (ActionParam1-5, Script2, etc.)
            // - K1, NWN, Eclipse (DA/DA2/ME): Base DLG format (no K2-specific fields)
            //   Eclipse games use K1-style DLG format (no K2 extensions)
            //   Note: Eclipse games may also use .cnv format, but DLG files follow K1 format
            BioWareGame gameToUseDlg = _installation?.Game ?? BioWareGame.K2;

            // For Eclipse games, use K1 format (no K2-specific fields)
            if (gameToUseDlg.IsEclipse())
            {
                gameToUseDlg = BioWareGame.K1; // Use K1 format for Eclipse (no K2-specific fields)
            }
            // For Aurora (NWN), use K1 format (base DLG, no K2 extensions)
            else if (gameToUseDlg.IsAurora())
            {
                gameToUseDlg = BioWareGame.K1; // Use K1 format for Aurora (base DLG, no K2 extensions)
            }

            ResourceType outputFormat = (_restype == ResourceType.DLG_XML || _restype == ResourceType.DLG_JSON) ? _restype : ResourceType.DLG;
            byte[] data = DLGHelper.BytesDlg(_coreDlg, gameToUseDlg, outputFormat);
            return Tuple.Create(data, new byte[0]);
        }

        /// <summary>
        /// Updates UI visibility based on game type.
        /// K2/TSL-specific controls are only shown for KotOR 2 (TSL).
        /// Aurora (NWN), K1, and Eclipse (DA/ME) use base DLG format without K2 extensions.
        /// </summary>
        private void UpdateUIForGame()
        {
            BioWareGame currentGame = _installation?.Game ?? BioWareGame.K2;
            bool isK2 = currentGame.IsK2();

            // Script #1: only ActionParam1 row is K2-specific (Param2–5 and ParamStrA exist in K1)
            if (_script1Param1Panel != null)
                _script1Param1Panel.IsVisible = isK2;

            // Script #2: entire block (Script2, ActionParam1b–5b, ParamStrB) is K2-only
            if (_script2Panel != null)
                _script2Panel.IsVisible = isK2;

            // Conditional #2: entire block (Active2, Not2, Param1b–5b, ParamStrB, Logic) is K2-only
            if (_condition2Panel != null)
                _condition2Panel.IsVisible = isK2;

            // Emotion / Expression: emotions.2da and expressions.2da are TSL-specific
            if (_emotionExpressionPanel != null)
                _emotionExpressionPanel.IsVisible = isK2;

            // Node ID, Alien Race Node, Post Proc Node, Logic: K2-only node/link fields
            if (_nodeIdLabel != null) _nodeIdLabel.IsVisible = isK2;
            if (_nodeIdSpin != null) _nodeIdSpin.IsVisible = isK2;
            if (_alienRaceNodeLabel != null) _alienRaceNodeLabel.IsVisible = isK2;
            if (_alienRaceNodeSpin != null) _alienRaceNodeSpin.IsVisible = isK2;
            if (_postProcLabel != null) _postProcLabel.IsVisible = isK2;
            if (_postProcSpin != null) _postProcSpin.IsVisible = isK2;
            if (_logicLabel != null) _logicLabel.IsVisible = isK2;
            if (_logicSpin != null) _logicSpin.IsVisible = isK2;
        }

        public override void New()
        {
            base.New();
            _coreDlg = new DLGType();
            _model.ResetModel();
            // Clear undo/redo history when creating new dialog
            _actionHistory.Clear();
            UpdateTreeView();
            RefreshStuntList();
            PopulateOrphanedNodesList();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        /// <summary>Returns the default file extension for the given dialogue resource type (e.g. .dlg, .dlg.xml, .cnv).</summary>
        private static string GetDefaultExtensionForRestype(ResourceType restype)
        {
            if (restype == ResourceType.CNV) return ".cnv";
            if (restype == ResourceType.DLG_XML) return ".dlg.xml";
            if (restype == ResourceType.DLG_JSON) return ".dlg.json";
            if (restype == ResourceType.DLG_TWINE_HTML) return ".twine.html";
            if (restype == ResourceType.DLG_TWINE_JSON) return ".twine.json";
            return ".dlg";
        }

        /// <summary>Parses a dialogue file path into resname and ResourceType. Used for Open, Save As, and Revert.</summary>
        private static (string resname, ResourceType restype) GetResnameAndRestypeFromPath(string filePath)
        {
            string baseName = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(baseName)) return (Path.GetFileNameWithoutExtension(filePath), ResourceType.DLG);
            if (baseName.EndsWith(".dlg.xml", StringComparison.OrdinalIgnoreCase))
                return (baseName.Substring(0, baseName.Length - 9), ResourceType.DLG_XML);
            if (baseName.EndsWith(".dlg.json", StringComparison.OrdinalIgnoreCase))
                return (baseName.Substring(0, baseName.Length - 10), ResourceType.DLG_JSON);
            if (baseName.EndsWith(".twine.html", StringComparison.OrdinalIgnoreCase))
                return (baseName.Substring(0, baseName.Length - 11), ResourceType.DLG_TWINE_HTML);
            if (baseName.EndsWith(".twine.json", StringComparison.OrdinalIgnoreCase))
                return (baseName.Substring(0, baseName.Length - 11), ResourceType.DLG_TWINE_JSON);
            string resname = Path.GetFileNameWithoutExtension(filePath);
            string ext = (Path.GetExtension(filePath) ?? "").TrimStart('.').ToLowerInvariant();
            ResourceType restype = ext == "cnv" ? ResourceType.CNV : ResourceType.DLG;
            return (resname, restype);
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "dialog" : _resname;
            string defaultExt = GetDefaultExtensionForRestype(_restype ?? ResourceType.DLG);
            var options = new FilePickerSaveOptions
            {
                Title = Localization.Tr("Save Dialogue As"),
                SuggestedFileName = suggestedName + defaultExt,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType(Localization.Tr("All dialogue formats (DLG, XML, JSON, CNV, Twine)"))
                    {
                        Patterns = new[] { "*.dlg", "*.dlg.xml", "*.dlg.json", "*.cnv", "*.twine.html", "*.twine.json" }
                    },
                    new FilePickerFileType(Localization.Tr("DLG Files")) { Patterns = new[] { "*.dlg" } },
                    new FilePickerFileType(Localization.Tr("DLG XML (plaintext)")) { Patterns = new[] { "*.dlg.xml" } },
                    new FilePickerFileType(Localization.Tr("DLG JSON (plaintext)")) { Patterns = new[] { "*.dlg.json" } },
                    new FilePickerFileType(Localization.Tr("CNV Files")) { Patterns = new[] { "*.cnv" } },
                    new FilePickerFileType(Localization.Tr("Twine HTML")) { Patterns = new[] { "*.twine.html" } },
                    new FilePickerFileType(Localization.Tr("Twine JSON")) { Patterns = new[] { "*.twine.json" } },
                    new FilePickerFileType(Localization.Tr("All files")) { Patterns = new[] { "*.*" } }
                }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            var (resname, restype) = GetResnameAndRestypeFromPath(path);
            _resname = resname;
            _restype = restype;
            RefreshWindowTitle();
            Save();
        }

        /// <summary>Override for custom DLG open (file picker only; module browser is separate).</summary>
        protected override async Task RunOpenAsync()
        {
            if (!await ConfirmDiscardUnsavedChangesAsync()) return;
            await OpenFileAsync();
        }

        /// <summary>
        /// Opens a file dialog to select and load a DLG file.
        /// </summary>
        private async Task OpenFileAsync()
        {
            var topLevel = GetTopLevel(this);
            if (topLevel == null) return;

            var options = new FilePickerOpenOptions
            {
                Title = Localization.Tr("Open Dialogue File"),
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new FilePickerFileType(Localization.Tr("All dialogue formats (DLG, XML, JSON, CNV, Twine)"))
                    {
                        Patterns = new List<string> { "*.dlg", "*.dlg.xml", "*.dlg.json", "*.cnv", "*.twine.html", "*.twine.json" }
                    },
                    new FilePickerFileType(Localization.Tr("DLG Files"))
                    {
                        Patterns = new List<string> { "*.dlg" },
                        MimeTypes = new List<string> { "application/octet-stream" }
                    },
                    new FilePickerFileType(Localization.Tr("DLG XML (plaintext)"))
                    {
                        Patterns = new List<string> { "*.dlg.xml" }
                    },
                    new FilePickerFileType(Localization.Tr("DLG JSON (plaintext)"))
                    {
                        Patterns = new List<string> { "*.dlg.json" }
                    },
                    new FilePickerFileType(Localization.Tr("CNV Files"))
                    {
                        Patterns = new List<string> { "*.cnv" }
                    },
                    new FilePickerFileType(Localization.Tr("Twine HTML"))
                    {
                        Patterns = new List<string> { "*.twine.html" }
                    },
                    new FilePickerFileType(Localization.Tr("Twine JSON"))
                    {
                        Patterns = new List<string> { "*.twine.json" }
                    },
                    new FilePickerFileType(Localization.Tr("All Files"))
                    {
                        Patterns = new List<string> { "*" },
                        MimeTypes = new List<string> { "application/octet-stream" }
                    }
                }
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0)
            {
                return;
            }

            var filePath = files[0].Path.LocalPath;
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                var data = File.ReadAllBytes(filePath);
                var (resname, restype) = GetResnameAndRestypeFromPath(filePath);
                Load(filePath, resname, restype, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open DLG file: {ex.Message}");
            }
        }

        /// <summary>Override: revert by re-reading from disk (or New if no file).</summary>
        public override void Revert()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                // No file to revert to, create new instead
                New();
                return;
            }

            try
            {
                if (File.Exists(_filepath))
                {
                    var data = File.ReadAllBytes(_filepath);
                    var (resname, restype) = GetResnameAndRestypeFromPath(_filepath);
                    Load(_filepath, resname, restype, data);
                }
                else
                {
                    // File no longer exists, create new
                    New();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to revert dialogue file: {ex.Message}");
                New();
            }
        }

        /// <summary>
        /// Refreshes the stunt list UI from the core DLG stunts.
        /// </summary>
        public void RefreshStuntList()
        {
            if (_stuntList == null)
            {
                return;
            }

            _stuntList.Items.Clear();

            foreach (DLGStunt stunt in _coreDlg.Stunts)
            {
                string text = $"{stunt.StuntModel} ({stunt.Participant})";
                var item = new ListBoxItem { Content = text, Tag = stunt };
                _stuntList.Items.Add(item);
            }
        }

        private async void OnAddStuntClicked()
        {
            if (_coreDlg == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Cutscene Model"),
                    Localization.Tr("Open a DLG file first."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            var dialog = new DialogModelDialog(this, null);
            bool result = await dialog.ShowDialog<bool>(this);
            if (result)
            {
                _actionHistory.Apply(new AddStuntAction(dialog.GetStunt()));
            }
        }

        private async void OnEditStuntClicked()
        {
            if (_coreDlg == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Cutscene Model"),
                    Localization.Tr("Open a DLG file first."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            if (!(_stuntList?.SelectedItem is ListBoxItem selItem) || !(selItem.Tag is DLGStunt stunt))
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Cutscene Model"),
                    Localization.Tr("Select a stunt from the list first, or use Add to create one."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            var dialog = new DialogModelDialog(this, stunt);
            string oldParticipant = stunt.Participant ?? "";
            string oldStuntModelStr = stunt.StuntModel?.ToString() ?? "";
            bool result = await dialog.ShowDialog<bool>(this);
            if (result)
            {
                DLGStunt updated = dialog.GetStunt();
                _actionHistory.Apply(new EditStuntAction(stunt, oldParticipant, oldStuntModelStr, updated.Participant ?? "", updated.StuntModel?.ToString() ?? ""));
            }
        }

        private async void OnRemoveStuntClicked()
        {
            if (_coreDlg == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Cutscene Model"),
                    Localization.Tr("Open a DLG file first."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            if (_stuntList?.SelectedItem is ListBoxItem item && item.Tag is DLGStunt stunt)
            {
                _actionHistory.Apply(new RemoveStuntAction(this, stunt));
            }
            else
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Cutscene Model"),
                    Localization.Tr("Select a stunt from the list first, or use Add to create one."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
            }
        }

        private void OnStuntListDoubleTapped(object sender, TappedEventArgs e)
        {
            if (_stuntList?.SelectedItem != null)
            {
                OnEditStuntClicked();
            }
        }

        /// <summary>
        /// Refreshes the animations list based on the currently selected node.
        /// </summary>
        public void RefreshAnimList()
        {
            if (_animsList == null)
            {
                return;
            }

            _animsList.Items.Clear();

            // Resolve dialoganimations.2da: installation (CHITIN, OVERRIDE) + custom folders from File → DLG Settings, then manual path fallback
            TwoDA animations2da = null;
            var dlgSettings = new DLGSettings();
            var customFolders = dlgSettings.GetCustom2DAFolders();
            if (UseInstallationForResources())
            {
                animations2da = _installation.Get2DAWithCustomFolders(OdyInstallation.TwoDADialogAnims, customFolders);
            }
            if (animations2da == null)
            {
                var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDADialogAnims);
                if (!string.IsNullOrEmpty(path))
                    animations2da = TwoDAFileHelper.LoadFromPath(path);
            }

            if (animations2da == null)
            {
                System.Console.WriteLine($"RefreshAnimList: {OdyInstallation.TwoDADialogAnims} not found. In File → DLG Settings choose an installation and/or set the 2DA directory or dialoganimations.2da path under Manual paths.");
                return;
            }

            // Get selected item from dialog tree
            var selectedItem = _dialogTree?.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            // Get DLGStandardItem from selected item
            DLGStandardItem dlgItem = null;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItemFromTag)
            {
                dlgItem = dlgItemFromTag;
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                dlgItem = dlgItemDirect;
            }

            if (dlgItem?.Link == null || dlgItem.Link.Node == null)
            {
                return;
            }

            foreach (DLGAnimation anim in dlgItem.Link.Node.Animations)
            {
                string name = anim.AnimationId.ToString();
                if (animations2da.GetHeight() > anim.AnimationId)
                {
                    var nameColumn = animations2da.GetColumn("name");
                    if (nameColumn != null && anim.AnimationId < nameColumn.Count)
                    {
                        name = nameColumn[anim.AnimationId] ?? anim.AnimationId.ToString();
                    }
                }
                string text = $"{name} ({anim.Participant})";
                var item = new ListBoxItem { Content = text, Tag = anim };
                _animsList.Items.Add(item);
            }
        }

        /// <summary>
        /// Handles the Add Animation button click.
        /// </summary>
        private async void OnAddAnimClicked()
        {
            DLGStandardItem selectedItem = GetSelectedItemFromTreeView() ?? _currentNodeItem;
            if (selectedItem?.Link?.Node == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Current Animations"),
                    Localization.Tr("Select a dialogue node in the tree first, or ensure the node whose fields are shown in the right panel has a valid link."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }

            var dialog = new DialogAnimationDialog(this, _installation, null);
            bool result = await dialog.ShowDialog<bool>(this);
            if (result)
            {
                var newAnim = dialog.GetAnimation();
                if (newAnim != null)
                {
                    selectedItem.Link.Node.Animations.Add(newAnim);
                    RefreshAnimList();
                    OnNodeUpdate();
                }
            }
        }

        /// <summary>
        /// Handles the Remove Animation button click.
        /// </summary>
        private async void OnRemoveAnimClicked()
        {
            DLGStandardItem selectedItem = GetSelectedItemFromTreeView() ?? _currentNodeItem;
            if (selectedItem?.Link?.Node == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Current Animations"),
                    Localization.Tr("Select a dialogue node in the tree first."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            if (_animsList?.SelectedItem == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Current Animations"),
                    Localization.Tr("Select an animation in the list to remove."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }

            if (_animsList.SelectedItem is ListBoxItem item && item.Tag is DLGAnimation anim)
            {
                selectedItem.Link.Node.Animations.Remove(anim);
                RefreshAnimList();
                OnNodeUpdate();
            }
        }

        /// <summary>
        /// Handles the Edit Animation button click.
        /// </summary>
        private async void OnEditAnimClicked()
        {
            DLGStandardItem selectedItem = GetSelectedItemFromTreeView() ?? _currentNodeItem;
            if (selectedItem?.Link?.Node == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Current Animations"),
                    Localization.Tr("Select a dialogue node in the tree first."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }
            if (_animsList?.SelectedItem == null)
            {
                await MessageBoxManager.GetMessageBoxStandard(Localization.Tr("Current Animations"),
                    Localization.Tr("Select an animation in the list to edit."),
                    ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this);
                return;
            }

            if (_animsList.SelectedItem is ListBoxItem item && item.Tag is DLGAnimation anim)
            {
                var dialog = new DialogAnimationDialog(this, _installation, anim);
                bool result = await dialog.ShowDialog<bool>(this);
                if (result)
                {
                    // Animation is updated in-place by the dialog
                    RefreshAnimList();
                    OnNodeUpdate();
                }
            }
        }

        /// <summary>
        /// Finds a DLGStandardItem that corresponds to the given link.
        /// </summary>
        private DLGStandardItem FindItemForLink(DLGLink link)
        {
            if (link == null || _model == null || _model.LinkToItems == null)
            {
                return null;
            }

            if (_model.LinkToItems.ContainsKey(link) && _model.LinkToItems[link] != null && _model.LinkToItems[link].Count > 0)
            {
                return _model.LinkToItems[link][0];
            }

            return _model.MaterializeItemForLink(link);
        }

        // Properties for tests
        public DLGType CoreDlg => _coreDlg;
        public DLGModel Model => _model;

        // Undo/redo functionality for DLG editor
        // Based on QUndoStack pattern from PyKotor implementation

        // --- Undo/Redo (Edit menu, Ctrl+Z / Ctrl+Y) ---
        // Undoable: Add/remove starter, move starter, add root node, add child node, remove link (tree or starter),
        // paste (link or deep copy), delete node everywhere, restore orphan, stunts add/remove/edit.
        // Not undoable: File globals or node field edits (LostFocus/ValueChanged sync).
        /// <summary>
        /// Gets whether undo is available.
        /// </summary>
        public bool CanUndo => _actionHistory.CanUndo;

        /// <summary>
        /// Gets whether redo is available.
        /// </summary>
        public bool CanRedo => _actionHistory.CanRedo;

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        public void Undo()
        {
            _actionHistory.Undo();
        }

        /// <summary>
        /// Redoes the last undone action.
        /// </summary>
        public void Redo()
        {
            _actionHistory.Redo();
        }

        /// <summary>
        /// Adds a starter link to the dialog and records it in the action history for undo/redo.
        /// </summary>
        /// <param name="link">The link to add.</param>
        public void AddStarter(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            int index = _coreDlg.Starters.Count;
            var action = new AddStarterAction(link, index);
            _actionHistory.Apply((IDLGAction)action);
        }

        /// <summary>
        /// Removes a starter link from the dialog and records it in the action history for undo/redo.
        /// </summary>
        /// <param name="link">The link to remove.</param>
        public void RemoveStarter(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            int index = _coreDlg.Starters.IndexOf(link);
            if (index < 0)
            {
                return; // Link not found, nothing to remove
            }

            var action = new RemoveStarterAction(link, index);
            _actionHistory.Apply(action);
        }

        /// <summary>
        /// Moves the selected item down in the starter list and records it in the action history for undo/redo.
        /// </summary>
        public void MoveItemDown()
        {
            int selectedIndex = _model.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= _coreDlg.Starters.Count - 1)
            {
                return; // No selection or already at bottom
            }

            int newIndex = selectedIndex + 1;
            DLGLink link = _coreDlg.Starters[selectedIndex];
            var action = new MoveStarterAction(link, selectedIndex, newIndex);
            _actionHistory.Apply(action);

            // Update selected index to track the moved item
            _model.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Moves the selected item up in the starter list and records it in the action history for undo/redo.
        /// </summary>
        public void MoveItemUp()
        {
            int selectedIndex = _model.SelectedIndex;
            if (selectedIndex <= 0 || selectedIndex >= _coreDlg.Starters.Count)
            {
                return; // No selection or already at top
            }

            int newIndex = selectedIndex - 1;
            DLGLink link = _coreDlg.Starters[selectedIndex];
            var action = new MoveStarterAction(link, selectedIndex, newIndex);
            _actionHistory.Apply(action);

            // Update selected index to track the moved item
            _model.SelectedIndex = newIndex;
        }

        public ListBox AnimsList => _animsList;
        public Button AddAnimButton => _addAnimButton;
        public Button RemoveAnimButton => _removeAnimButton;
        public Button EditAnimButton => _editAnimButton;

        // Expose link widgets for testing
        public ComboBox Condition1ResrefEdit => _condition1ResrefEdit;
        public ComboBox Condition2ResrefEdit => _condition2ResrefEdit;
        public ComboBox Script1ResrefEdit => _script1ResrefEdit;
        public ComboBox Script2ResrefEdit => _script2ResrefEdit;
        public NumericUpDown LogicSpin => _logicSpin;
        public TreeView DialogTree => _dialogTree;

        // Expose condition parameter widgets for testing
        public NumericUpDown Condition1Param1Spin => _condition1Param1Spin;
        public NumericUpDown Condition1Param2Spin => _condition1Param2Spin;
        public NumericUpDown Condition1Param3Spin => _condition1Param3Spin;
        public NumericUpDown Condition1Param4Spin => _condition1Param4Spin;
        public NumericUpDown Condition1Param5Spin => _condition1Param5Spin;
        public TextBox Condition1Param6Edit => _condition1Param6Edit;
        public CheckBox Condition1NotCheckbox => _condition1NotCheckbox;
        public NumericUpDown Condition2Param1Spin => _condition2Param1Spin;
        public NumericUpDown Condition2Param2Spin => _condition2Param2Spin;
        public NumericUpDown Condition2Param3Spin => _condition2Param3Spin;
        public NumericUpDown Condition2Param4Spin => _condition2Param4Spin;
        public NumericUpDown Condition2Param5Spin => _condition2Param5Spin;
        public TextBox Condition2Param6Edit => _condition2Param6Edit;
        public CheckBox Condition2NotCheckbox => _condition2NotCheckbox;

        // Expose quest widgets for testing
        public TextBox QuestEdit => _questEdit;
        public NumericUpDown QuestEntrySpin => _questEntrySpin;
        public ComboBox PlotIndexCombo => _plotIndexCombo;
        public NumericUpDown PlotXpSpin => _plotXpSpin;
        public NumericUpDown DelaySpin => _delaySpin;
        public NumericUpDown WaitFlagSpin => _waitFlagSpin;
        public NumericUpDown FadeTypeSpin => _fadeTypeSpin;

        // Expose camera widgets for testing
        public NumericUpDown CameraIdSpin => _cameraIdSpin;
        public NumericUpDown CameraAnimSpin => _cameraAnimSpin;
        public ComboBox CameraAngleSelect => _cameraAngleSelect;
        public ComboBox CameraEffectSelect => _cameraEffectSelect;
        public ComboBox EmotionSelect => _emotionSelect;
        public ComboBox ExpressionSelect => _expressionSelect;

        // Expose speaker widgets for testing
        public TextBox SpeakerEdit => _speakerEdit;
        public TextBlock SpeakerEditLabel => _speakerEditLabel;

        // Expose listener widget for testing
        // Expose comments widget for testing
        public TextBox CommentsEdit => _commentsEdit;

        public TextBox ListenerEdit => _listenerEdit;

        // Expose find/search widgets for testing
        public TextBox FindInput => _findInput;
        public Button FindButton => _findButton;
        public TextBlock ResultsLabel => _resultsLabel;


        // File-level properties exposed for testing
        public ComboBox ConversationSelect => _conversationSelect;
        public ComboBox ComputerSelect => _computerSelect;
        public NumericUpDown EntryDelaySpin => _entryDelaySpin;
        public NumericUpDown ReplyDelaySpin => _replyDelaySpin;
        public ComboBox OnAbortCombo => _onAbortCombo;
        public ComboBox OnEndEdit => _onEndEdit;
        public ComboBox CameraModelSelect => _cameraModelSelect;
        public TextBox VoIdEdit => _voIdEdit;
        public ComboBox AmbientTrackCombo => _ambientTrackCombo;
        public CheckBox SkippableCheckbox => _skippableCheckbox;
        public CheckBox AnimatedCutCheckbox => _animatedCutCheckbox;
        public CheckBox OldHitCheckbox => _oldHitCheckbox;
        public CheckBox UnequipHandsCheckbox => _unequipHandsCheckbox;
        public CheckBox UnequipAllCheckbox => _unequipAllCheckbox;

        // Expose left dock widget for testing
        public Control LeftDockWidget => _leftDockWidget;
        public DLGListWidget OrphanedNodesList => _orphanedNodesList;
        public DLGListWidget PinnedItemsList => _pinnedItemsList;

        // Expose script widgets for testing
        public NumericUpDown Script1Param1Spin => _script1Param1Spin;

        // Expose script2 parameter widgets for testing
        public NumericUpDown Script2Param1Spin => _script2Param1Spin;
        public NumericUpDown Script2Param2Spin => _script2Param2Spin;
        public NumericUpDown Script2Param3Spin => _script2Param3Spin;
        public NumericUpDown Script2Param4Spin => _script2Param4Spin;
        public NumericUpDown Script2Param5Spin => _script2Param5Spin;

        // Expose sound widget for testing
        public ComboBox SoundComboBox => _soundComboBox;

        // Expose voice widget for testing
        public ComboBox VoiceComboBox => _voiceComboBox;
        public CheckBox SoundCheckbox => _soundCheckbox;
        public NumericUpDown NodeIdSpin => _nodeIdSpin;
        public NumericUpDown AlienRaceNodeSpin => _alienRaceNodeSpin;
        public NumericUpDown PostProcSpin => _postProcSpin;
        public CheckBox NodeUnskippableCheckbox => _nodeUnskippableCheckbox;

        // Expose VO ID widget for testing

        /// </summary>
        private void OnSelectionChanged()
        {
            _nodeLoadedIntoUi = false;

            _currentNodeItem = null;
            if (_dialogTree?.SelectedItem == null)
            {
                _selectedLink = null;
                // Clear UI when nothing is selected
                if (_condition1ResrefEdit != null)
                {
                    _condition1ResrefEdit.Text = string.Empty;
                }
                if (_condition1Param1Spin != null)
                {
                    _condition1Param1Spin.Value = 0;
                }
                if (_condition1Param2Spin != null)
                {
                    _condition1Param2Spin.Value = 0;
                }
                if (_condition1Param3Spin != null)
                {
                    _condition1Param3Spin.Value = 0;
                }
                if (_condition1Param4Spin != null)
                {
                    _condition1Param4Spin.Value = 0;
                }
                if (_condition1Param5Spin != null)
                {
                    _condition1Param5Spin.Value = 0;
                }
                if (_condition1Param6Edit != null)
                {
                    _condition1Param6Edit.Text = string.Empty;
                }
                if (_condition1NotCheckbox != null)
                {
                    _condition1NotCheckbox.IsChecked = false;
                }
                if (_condition2ResrefEdit != null)
                {
                    _condition2ResrefEdit.Text = string.Empty;
                }
                if (_condition2Param1Spin != null)
                {
                    _condition2Param1Spin.Value = 0;
                }
                if (_condition2Param2Spin != null)
                {
                    _condition2Param2Spin.Value = 0;
                }
                if (_condition2Param3Spin != null)
                {
                    _condition2Param3Spin.Value = 0;
                }
                if (_condition2Param4Spin != null)
                {
                    _condition2Param4Spin.Value = 0;
                }
                if (_condition2Param5Spin != null)
                {
                    _condition2Param5Spin.Value = 0;
                }
                if (_condition2Param6Edit != null)
                {
                    _condition2Param6Edit.Text = string.Empty;
                }
                if (_condition2NotCheckbox != null)
                {
                    _condition2NotCheckbox.IsChecked = false;
                }
                if (_logicSpin != null)
                {
                    _logicSpin.Value = 0;
                }
                if (_questEdit != null)
                {
                    _questEdit.Text = string.Empty;
                }
                if (_questEntrySpin != null)
                {
                    _questEntrySpin.Value = 0;
                }
                if (_plotIndexCombo != null)
                {
                    _plotIndexCombo.SetSelectedIndex(0);
                }
                if (_plotXpSpin != null)
                {
                    _plotXpSpin.Value = 0;
                }
                if (_script1ResrefEdit != null)
                {
                    _script1ResrefEdit.Text = string.Empty;
                }
                if (_script2ResrefEdit != null)
                {
                    _script2ResrefEdit.Text = string.Empty;
                }
                if (_script1Param1Spin != null) _script1Param1Spin.Value = 0;
                if (_script1Param2Spin != null) _script1Param2Spin.Value = 0;
                if (_script1Param3Spin != null) _script1Param3Spin.Value = 0;
                if (_script1Param4Spin != null) _script1Param4Spin.Value = 0;
                if (_script1Param5Spin != null) _script1Param5Spin.Value = 0;
                if (_script1Param6Edit != null) _script1Param6Edit.Text = string.Empty;
                if (_script2Param1Spin != null) _script2Param1Spin.Value = 0;
                if (_script2Param2Spin != null) _script2Param2Spin.Value = 0;
                if (_script2Param3Spin != null) _script2Param3Spin.Value = 0;
                if (_script2Param4Spin != null) _script2Param4Spin.Value = 0;
                if (_script2Param5Spin != null) _script2Param5Spin.Value = 0;
                if (_script2Param6Edit != null) _script2Param6Edit.Text = string.Empty;
                if (_speakerEdit != null)
                {
                    _speakerEdit.Text = string.Empty;
                    _speakerEdit.IsVisible = false;
                }
                if (_speakerEditLabel != null)
                {
                    _speakerEditLabel.IsVisible = false;
                }
                UpdateNodeTextPreview();
            }

            // Get selected item from tree
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                _selectedLink = dlgItem?.Link;
                _currentNodeItem = dlgItem;
                LoadLinkIntoUI(dlgItem);
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                _selectedLink = dlgItemDirect?.Link;
                _currentNodeItem = dlgItemDirect;
                LoadLinkIntoUI(dlgItemDirect);
            }

            UpdateNodeTextPreview();
            _nodeLoadedIntoUi = true;
        }

        private void UpdateNodeTextPreview()
        {
            if (_nodeTextPreviewBorder == null || _nodeTextPreviewLabel == null)
                return;

            var node = _selectedLink?.Node;
            if (node == null)
            {
                _nodeTextPreviewBorder.IsVisible = false;
                return;
            }

            string resolved = GetResolvedNodeText(node);
            if (string.IsNullOrWhiteSpace(resolved))
            {
                _nodeTextPreviewLabel.Text = "<no text>";
                _nodeTextPreviewLabel.Foreground = new SolidColorBrush(AmColor.Parse("#B0BEC5"));
                _nodeTextPreviewLabel.FontStyle = FontStyle.Italic;
            }
            else
            {
                _nodeTextPreviewLabel.Text = resolved;
                _nodeTextPreviewLabel.Foreground = new SolidColorBrush(AmColor.Parse("#212121"));
                _nodeTextPreviewLabel.FontStyle = FontStyle.Normal;
            }
            _nodeTextPreviewBorder.IsVisible = true;
        }

        /// <summary>
        /// Syncs selection from the tree view to _selectedLink, flat lists, and graph.
        /// Called when the tree selection changes.
        /// </summary>
        private void SyncSelectionFromTree()
        {
            if (_isSyncingViewSelection) return;
            _isSyncingViewSelection = true;
            try
            {
                OnSelectionChanged();
                SyncFlatSelection(_selectedLink);
                SyncGraphSelection(_selectedLink);
            }
            finally
            {
                _isSyncingViewSelection = false;
            }
        }

        /// <summary>
        /// Loads link properties into UI controls.
        /// </summary>
        private void LoadLinkIntoUI(DLGStandardItem item)
        {
            if (item?.Link == null)
            {
                return;
            }

            var link = item.Link;
            var node = link.Node;

            // Load condition1
            if (_condition1ResrefEdit != null)
            {
                _condition1ResrefEdit.Text = link.Active1?.ToString() ?? string.Empty;
            }
            if (_condition1Param1Spin != null)
            {
                _condition1Param1Spin.Value = link.Active1Param1;
            }
            if (_condition1Param2Spin != null)
            {
                _condition1Param2Spin.Value = link.Active1Param2;
            }
            if (_condition1Param3Spin != null)
            {
                _condition1Param3Spin.Value = link.Active1Param3;
            }
            if (_condition1Param4Spin != null)
            {
                _condition1Param4Spin.Value = link.Active1Param4;
            }
            if (_condition1Param5Spin != null)
            {
                _condition1Param5Spin.Value = link.Active1Param5;
            }
            if (_condition1Param6Edit != null)
            {
                _condition1Param6Edit.Text = link.Active1Param6 ?? string.Empty;
            }
            if (_condition1NotCheckbox != null)
            {
                _condition1NotCheckbox.IsChecked = link.Active1Not;
            }

            // Load condition2
            if (_condition2ResrefEdit != null)
            {
                _condition2ResrefEdit.Text = link.Active2?.ToString() ?? string.Empty;
            }
            if (_condition2Param1Spin != null)
            {
                _condition2Param1Spin.Value = link.Active2Param1;
            }
            if (_condition2Param2Spin != null)
            {
                _condition2Param2Spin.Value = link.Active2Param2;
            }
            if (_condition2Param3Spin != null)
            {
                _condition2Param3Spin.Value = link.Active2Param3;
            }
            if (_condition2Param4Spin != null)
            {
                _condition2Param4Spin.Value = link.Active2Param4;
            }
            if (_condition2Param5Spin != null)
            {
                _condition2Param5Spin.Value = link.Active2Param5;
            }
            if (_condition2Param6Edit != null)
            {
                _condition2Param6Edit.Text = link.Active2Param6 ?? string.Empty;
            }
            if (_condition2NotCheckbox != null)
            {
                _condition2NotCheckbox.IsChecked = link.Active2Not;
            }

            // Load logic (0 = AND/false, 1 = OR/true)
            if (_logicSpin != null)
            {
                _logicSpin.Value = link.Logic ? 1 : 0;
            }

            // Load speaker field from node (only for Entry nodes)
            if (node is DLGEntry entry)
            {
                if (_speakerEditLabel != null)
                {
                    _speakerEditLabel.IsVisible = true;
                }
                if (_speakerEdit != null)
                {
                    _speakerEdit.IsVisible = true;
                    _speakerEdit.Text = entry.Speaker ?? string.Empty;
                }
            }
            else if (node is DLGReply)
            {
                if (_speakerEditLabel != null)
                {
                    _speakerEditLabel.IsVisible = false;
                }
                if (_speakerEdit != null)
                {
                    _speakerEdit.IsVisible = false;
                }
            }

            // Load listener field from node
            if (_listenerEdit != null && node != null)
            {
                _listenerEdit.Text = node.Listener ?? string.Empty;
            }

            // Load quest fields from node
            if (_questEdit != null && node != null)
            {
                _questEdit.Text = node.Quest ?? string.Empty;
            }

            if (_questEntrySpin != null && node != null)
            {
                _questEntrySpin.Value = node.QuestEntry ?? 0;
            }

            // Load plot fields from node (ComboBox2DA: use SetSelectedIndex)
            if (_plotIndexCombo != null && node != null)
            {
                _plotIndexCombo.SetSelectedIndex(node.PlotIndex);
            }

            if (_plotXpSpin != null && node != null)
            {
                _plotXpSpin.Value = (decimal)node.PlotXpPercentage;
            }

            // Load script1 and script2 ResRefs
            if (_script1ResrefEdit != null && node != null)
            {
                _script1ResrefEdit.Text = node.Script1?.ToString() ?? string.Empty;
            }
            if (_script2ResrefEdit != null && node != null)
            {
                _script2ResrefEdit.Text = node.Script2?.ToString() ?? string.Empty;
            }

            // Load script1 params (GFF: Script, ActionParam1-5, ActionParamStrA)
            if (_script1Param1Spin != null && node != null)
            {
                _script1Param1Spin.Value = node.Script1Param1;
            }
            if (_script1Param2Spin != null && node != null)
            {
                _script1Param2Spin.Value = node.Script1Param2;
            }
            if (_script1Param3Spin != null && node != null)
            {
                _script1Param3Spin.Value = node.Script1Param3;
            }
            if (_script1Param4Spin != null && node != null)
            {
                _script1Param4Spin.Value = node.Script1Param4;
            }
            if (_script1Param5Spin != null && node != null)
            {
                _script1Param5Spin.Value = node.Script1Param5;
            }
            if (_script1Param6Edit != null && node != null)
            {
                _script1Param6Edit.Text = node.Script1Param6 ?? string.Empty;
            }

            // Load script2 params (GFF: Script2, ActionParam1b-5b, ActionParamStrB)
            if (_script2Param1Spin != null && node != null)
            {
                _script2Param1Spin.Value = node.Script2Param1;
            }
            if (_script2Param2Spin != null && node != null)
            {
                _script2Param2Spin.Value = node.Script2Param2;
            }
            if (_script2Param3Spin != null && node != null)
            {
                _script2Param3Spin.Value = node.Script2Param3;
            }
            if (_script2Param4Spin != null && node != null)
            {
                _script2Param4Spin.Value = node.Script2Param4;
            }
            if (_script2Param5Spin != null && node != null)
            {
                _script2Param5Spin.Value = node.Script2Param5;
            }
            if (_script2Param6Edit != null && node != null)
            {
                _script2Param6Edit.Text = node.Script2Param6 ?? string.Empty;
            }

            // Load delay, wait flags, and fade type from node
            if (_delaySpin != null && node != null)
            {
                _delaySpin.Value = node.Delay;
            }

            if (_waitFlagSpin != null && node != null)
            {
                _waitFlagSpin.Value = node.WaitFlags;
            }

            if (_fadeTypeSpin != null && node != null)
            {
                _fadeTypeSpin.Value = node.FadeType;
            }

            // Load sound ResRef from node
            if (_soundComboBox != null && node != null)
            {
                _soundComboBox.Text = node.Sound?.ToString() ?? string.Empty;
            }

            if (_soundCheckbox != null && node != null)
            {
                _soundCheckbox.IsChecked = node.SoundExists != 0;
            }

            if (_emotionSelect != null && node != null)
            {
                _emotionSelect.SetSelectedIndex(node.EmotionId);
            }

            if (_expressionSelect != null && node != null)
            {
                _expressionSelect.SetSelectedIndex(node.FacialId);
            }

            // Load camera fields from node
            if (_cameraIdSpin != null && node != null)
            {
                _cameraIdSpin.Value = node.CameraId ?? -1;
            }
            if (_cameraAnimSpin != null && node != null)
            {
                _cameraAnimSpin.Value = node.CameraAnim ?? 0;
            }
            if (_cameraAngleSelect != null && node != null)
            {
                _cameraAngleSelect.SelectedIndex = Math.Max(0, Math.Min(6, node.CameraAngle));
            }
            if (_cameraEffectSelect != null && node != null)
            {
                _cameraEffectSelect.SetSelectedIndex(node.CameraEffect ?? -1);
            }

            RefreshAnimList();

            if (_nodeIdSpin != null && node != null)
            {
                _nodeIdSpin.Value = node.NodeId;
            }

            if (_alienRaceNodeSpin != null && node != null)
            {
                _alienRaceNodeSpin.Value = node.AlienRaceNode;
            }

            if (_postProcSpin != null && node != null)
            {
                _postProcSpin.Value = node.PostProcNode;
            }

            if (_nodeUnskippableCheckbox != null && node != null)
            {
                _nodeUnskippableCheckbox.IsChecked = node.Unskippable;
            }

            // Load voice ResRef from node
            if (_voiceComboBox != null && node != null)
            {
                _voiceComboBox.Text = node.VoResRef?.ToString() ?? string.Empty;
            }

            HandleSoundChecked();
            UpdateCameraWidgetState();
        }

        /// <summary>
        /// Updates node properties based on UI selections.
        /// </summary>
        public void OnNodeUpdate()
        {
            if (!_nodeLoadedIntoUi)
            {
                return;
            }

            if (_dialogTree?.SelectedItem == null)
            {
                return;
            }

            // Get selected item from tree
            DLGStandardItem item = null;
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                item = dlgItem;
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                item = dlgItemDirect;
            }

            if (item?.Link == null)
            {
                return;
            }

            var link = item.Link;
            var node = link.Node;

            // Update condition1
            if (_condition1ResrefEdit != null)
            {
                string text = _condition1ResrefEdit.Text ?? string.Empty;
                link.Active1 = string.IsNullOrEmpty(text) ? ResRef.FromBlank() : new ResRef(text);
            }
            if (_condition1Param1Spin != null)
            {
                link.Active1Param1 = _condition1Param1Spin.Value.HasValue ? (int)_condition1Param1Spin.Value.Value : 0;
            }
            if (_condition1Param2Spin != null)
            {
                link.Active1Param2 = _condition1Param2Spin.Value.HasValue ? (int)_condition1Param2Spin.Value.Value : 0;
            }
            if (_condition1Param3Spin != null)
            {
                link.Active1Param3 = _condition1Param3Spin.Value.HasValue ? (int)_condition1Param3Spin.Value.Value : 0;
            }
            if (_condition1Param4Spin != null)
            {
                link.Active1Param4 = _condition1Param4Spin.Value.HasValue ? (int)_condition1Param4Spin.Value.Value : 0;
            }
            if (_condition1Param5Spin != null)
            {
                link.Active1Param5 = _condition1Param5Spin.Value.HasValue ? (int)_condition1Param5Spin.Value.Value : 0;
            }
            if (_condition1Param6Edit != null)
            {
                link.Active1Param6 = _condition1Param6Edit.Text ?? string.Empty;
            }
            if (_condition1NotCheckbox != null)
            {
                link.Active1Not = _condition1NotCheckbox.IsChecked.HasValue && _condition1NotCheckbox.IsChecked.Value;
            }

            // Update condition2
            if (_condition2ResrefEdit != null)
            {
                string text = _condition2ResrefEdit.Text ?? string.Empty;
                link.Active2 = string.IsNullOrEmpty(text) ? ResRef.FromBlank() : new ResRef(text);
            }
            if (_condition2Param1Spin != null)
            {
                link.Active2Param1 = _condition2Param1Spin.Value.HasValue ? (int)_condition2Param1Spin.Value.Value : 0;
            }
            if (_condition2Param2Spin != null)
            {
                link.Active2Param2 = _condition2Param2Spin.Value.HasValue ? (int)_condition2Param2Spin.Value.Value : 0;
            }
            if (_condition2Param3Spin != null)
            {
                link.Active2Param3 = _condition2Param3Spin.Value.HasValue ? (int)_condition2Param3Spin.Value.Value : 0;
            }
            if (_condition2Param4Spin != null)
            {
                link.Active2Param4 = _condition2Param4Spin.Value.HasValue ? (int)_condition2Param4Spin.Value.Value : 0;
            }
            if (_condition2Param5Spin != null)
            {
                link.Active2Param5 = _condition2Param5Spin.Value.HasValue ? (int)_condition2Param5Spin.Value.Value : 0;
            }
            if (_condition2Param6Edit != null)
            {
                link.Active2Param6 = _condition2Param6Edit.Text ?? string.Empty;
            }
            if (_condition2NotCheckbox != null)
            {
                link.Active2Not = _condition2NotCheckbox.IsChecked.HasValue && _condition2NotCheckbox.IsChecked.Value;
            }

            // Update logic (0 = AND/false, 1 = OR/true)
            if (_logicSpin != null)
            {
                link.Logic = _logicSpin.Value.HasValue && _logicSpin.Value.Value != 0;
            }

            // Update speaker field in node (only for Entry nodes)
            if (_speakerEdit != null && node is DLGEntry entry)
            {
                entry.Speaker = _speakerEdit.Text ?? string.Empty;
            }

            // Update listener field in node
            if (_listenerEdit != null && node != null)
            {
                node.Listener = _listenerEdit.Text ?? string.Empty;
            }

            // Update quest fields in node
            if (_questEdit != null && node != null)
            {
                node.Quest = _questEdit.Text ?? string.Empty;
            }

            if (_questEntrySpin != null && node != null)
            {
                node.QuestEntry = _questEntrySpin.Value.HasValue ? (int)_questEntrySpin.Value.Value : 0;
            }

            // Update plot fields in node
            if (_plotIndexCombo != null && node != null)
            {
                node.PlotIndex = _plotIndexCombo.SelectedIndex;
            }

            if (_plotXpSpin != null && node != null)
            {
                node.PlotXpPercentage = _plotXpSpin.Value.HasValue ? (float)_plotXpSpin.Value.Value : 0f;
            }

            // Update script1 ResRef and params (GFF: Script, ActionParam1-5, ActionParamStrA)
            if (_script1ResrefEdit != null && node != null)
            {
                string script1Text = _script1ResrefEdit.Text?.Trim() ?? string.Empty;
                node.Script1 = string.IsNullOrEmpty(script1Text) ? ResRef.FromBlank() : new ResRef(script1Text);
            }
            if (_script1Param1Spin != null && node != null)
            {
                node.Script1Param1 = _script1Param1Spin.Value.HasValue ? (int)_script1Param1Spin.Value.Value : 0;
            }
            if (_script1Param2Spin != null && node != null)
            {
                node.Script1Param2 = _script1Param2Spin.Value.HasValue ? (int)_script1Param2Spin.Value.Value : 0;
            }
            if (_script1Param3Spin != null && node != null)
            {
                node.Script1Param3 = _script1Param3Spin.Value.HasValue ? (int)_script1Param3Spin.Value.Value : 0;
            }
            if (_script1Param4Spin != null && node != null)
            {
                node.Script1Param4 = _script1Param4Spin.Value.HasValue ? (int)_script1Param4Spin.Value.Value : 0;
            }
            if (_script1Param5Spin != null && node != null)
            {
                node.Script1Param5 = _script1Param5Spin.Value.HasValue ? (int)_script1Param5Spin.Value.Value : 0;
            }
            if (_script1Param6Edit != null && node != null)
            {
                node.Script1Param6 = _script1Param6Edit.Text ?? string.Empty;
            }

            // Update script2 ResRef and params (GFF: Script2, ActionParam1b-5b, ActionParamStrB)
            if (_script2ResrefEdit != null && node != null)
            {
                string script2Text = _script2ResrefEdit.Text?.Trim() ?? string.Empty;
                node.Script2 = string.IsNullOrEmpty(script2Text) ? ResRef.FromBlank() : new ResRef(script2Text);
            }
            if (_script2Param1Spin != null && node != null)
            {
                node.Script2Param1 = _script2Param1Spin.Value.HasValue ? (int)_script2Param1Spin.Value.Value : 0;
            }
            if (_script2Param2Spin != null && node != null)
            {
                node.Script2Param2 = _script2Param2Spin.Value.HasValue ? (int)_script2Param2Spin.Value.Value : 0;
            }
            if (_script2Param3Spin != null && node != null)
            {
                node.Script2Param3 = _script2Param3Spin.Value.HasValue ? (int)_script2Param3Spin.Value.Value : 0;
            }
            if (_script2Param4Spin != null && node != null)
            {
                node.Script2Param4 = _script2Param4Spin.Value.HasValue ? (int)_script2Param4Spin.Value.Value : 0;
            }
            if (_script2Param5Spin != null && node != null)
            {
                node.Script2Param5 = _script2Param5Spin.Value.HasValue ? (int)_script2Param5Spin.Value.Value : 0;
            }
            if (_script2Param6Edit != null && node != null)
            {
                node.Script2Param6 = _script2Param6Edit.Text ?? string.Empty;
            }

            // Update delay, wait flags, and fade type in node
            if (_delaySpin != null && node != null)
            {
                node.Delay = _delaySpin.Value.HasValue ? (int)_delaySpin.Value.Value : -1;
            }

            if (_waitFlagSpin != null && node != null)
            {
                node.WaitFlags = _waitFlagSpin.Value.HasValue ? (int)_waitFlagSpin.Value.Value : 0;
            }

            if (_fadeTypeSpin != null && node != null)
            {
                node.FadeType = _fadeTypeSpin.Value.HasValue ? (int)_fadeTypeSpin.Value.Value : 0;
            }

            // Update sound ResRef in node
            if (_soundComboBox != null && node != null)
            {
                string soundText = _soundComboBox.Text ?? string.Empty;
                node.Sound = string.IsNullOrEmpty(soundText) ? ResRef.FromBlank() : new ResRef(soundText);
            }

            if (_soundCheckbox != null && node != null)
            {
                node.SoundExists = _soundCheckbox.IsChecked == true ? 1 : 0;
            }

            if (_emotionSelect != null && node != null)
            {
                node.EmotionId = _emotionSelect?.SelectedIndex ?? 0;
            }

            if (_expressionSelect != null && node != null)
            {
                node.FacialId = _expressionSelect?.SelectedIndex ?? 0;
            }

            if (_nodeIdSpin != null && node != null)
            {
                node.NodeId = (int)(_nodeIdSpin.Value ?? 0);
            }

            if (_alienRaceNodeSpin != null && node != null)
            {
                node.AlienRaceNode = (int)(_alienRaceNodeSpin.Value ?? 0);
            }

            if (_postProcSpin != null && node != null)
            {
                node.PostProcNode = (int)(_postProcSpin.Value ?? 0);
            }

            if (_nodeUnskippableCheckbox != null && node != null)
            {
                node.Unskippable = _nodeUnskippableCheckbox.IsChecked == true;
            }

            // Update voice ResRef in node
            if (_voiceComboBox != null && node != null)
            {
                string voiceText = _voiceComboBox.Text ?? string.Empty;
                node.VoResRef = string.IsNullOrEmpty(voiceText) ? ResRef.FromBlank() : new ResRef(voiceText);
            }

            // Update camera properties in node
            if (_cameraIdSpin != null && node != null)
            {
                node.CameraId = _cameraIdSpin.Value.HasValue ? (int?)_cameraIdSpin.Value.Value : null;
            }

            if (_cameraAnimSpin != null && node != null)
            {
                node.CameraAnim = _cameraAnimSpin.Value.HasValue ? (int?)_cameraAnimSpin.Value.Value : null;
            }

            if (_cameraAngleSelect != null && node != null)
            {
                node.CameraAngle = _cameraAngleSelect.SelectedIndex >= 0 ? _cameraAngleSelect.SelectedIndex : 0;
            }

            if (_cameraEffectSelect != null && node != null)
            {
                // ComboBox2DA.SelectedIndex returns row index (can be -1 for [Unset])
                int idx = _cameraEffectSelect.SelectedIndex;
                node.CameraEffect = idx;
            }

            // Handle camera ID and angle interaction (matching Python logic)
            if (_cameraIdSpin != null && _cameraAngleSelect != null && node != null)
            {
                int? cameraId = _cameraIdSpin.Value.HasValue ? (int?)_cameraIdSpin.Value.Value : null;
                int cameraAngle = _cameraAngleSelect.SelectedIndex >= 0 ? _cameraAngleSelect.SelectedIndex : 0;
                if (cameraId.HasValue && cameraId.Value >= 0 && cameraAngle == 0)
                {
                    _cameraAngleSelect.SelectedIndex = 6;
                    node.CameraAngle = 6;
                }
                else if (cameraId.HasValue && cameraId.Value == -1 && cameraAngle == 7)
                {
                    _cameraAngleSelect.SelectedIndex = 0;
                    node.CameraAngle = 0;
                }
            }

            // Vendor parity: Update dependent UI state (handle_sound_checked, cameraAnimSpin enable/disable, Static Camera error)
            HandleSoundChecked();
            UpdateCameraWidgetState();
            _selectedLink = item.Link;
            RefreshAllViews();
        }

        /// <summary>Vendor: handle_sound_checked. Disables sound button when Exists is not checked.</summary>
        private void HandleSoundChecked()
        {
            if (_soundButton == null || _soundCheckbox == null) return;
            bool exists = _soundCheckbox.IsChecked == true;
            _soundButton.IsEnabled = exists;
            ToolTip.SetTip(_soundButton, exists ? "" : Localization.Tr("Exists must be checked."));
        }

        /// <summary>Creates a styled ToolTip for the Camera ID field with proper wrapping and formatting.</summary>
        private static TextBlock CreateCameraIdStyledTooltip()
        {
            var tb = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 340
            };
            tb.Inlines.Add(new Run(Localization.Tr("CameraID (GFF: CameraID):")) { FontWeight = FontWeight.Bold });
            tb.Inlines.Add(new Run(" " + Localization.Tr("Index of a static camera in the CameraModel. Only used when Camera Angle is Static Camera (6). Engine ignores this when angle is not 6. INT32.")));
            return tb;
        }

        /// <summary>Creates a styled ToolTip for the Camera Angle field with proper wrapping and formatting.</summary>
        private static TextBlock CreateCameraAngleStyledTooltip()
        {
            var tb = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 340
            };
            tb.Inlines.Add(new Run(Localization.Tr("CameraAngle (GFF: CameraAngle):")) { FontWeight = FontWeight.Bold });
            tb.Inlines.Add(new Run(" " + Localization.Tr("DWORD")));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run(Localization.Tr("0=Auto, 1=Face, 2=Shoulder, 3=Wide Shot, 4=Animated Camera, 5=unused, 6=Static Camera.")));
            tb.Inlines.Add(new LineBreak());
            tb.Inlines.Add(new Run(Localization.Tr("Only angle 6 uses CameraID.")));
            return tb;
        }

        /// <summary>Vendor parity: cameraAnimSpin enable/disable based on CameraModel and CameraAngle; Static Camera + CameraID -1 error styling. Camera ID enabled only when angle is Static Camera (6).</summary>
        private void UpdateCameraWidgetState()
        {
            string cameraModelText = _cameraModelSelect?.Text?.Trim() ?? "";
            int cameraAngleIndex = _cameraAngleSelect?.SelectedIndex ?? 0;
            int cameraId = (int)(_cameraIdSpin?.Value ?? -1);
            // Vendor: 0=Auto, 1=Face, 2=Shoulder, 3=Wide Shot, 4=Animated Camera, 5=DO NOT USE, 6=Static Camera
            const int AnimatedCameraIndex = 4;
            const int StaticCameraIndex = 6;

            // Camera ID: only enabled when Camera Angle is Static Camera (6); show tooltip when disabled
            bool cameraIdEnabled = cameraAngleIndex == StaticCameraIndex;
            if (_cameraIdSpin != null)
            {
                _cameraIdSpin.IsEnabled = cameraIdEnabled;
            }

            // cameraAnimSpin: disable when CameraModel empty or CameraAngle not "Animated Camera"
            if (_cameraAnimSpin != null)
            {
                if (string.IsNullOrEmpty(cameraModelText))
                {
                    _cameraAnimSpin.IsEnabled = false;
                    ToolTip.SetTip(_cameraAnimSpin, Localization.Tr("You must setup your custom `CameraModel` first (in the 'File Globals' dockpanel at the top.)"));
                }
                else if (cameraAngleIndex != AnimatedCameraIndex)
                {
                    _cameraAnimSpin.IsEnabled = false;
                    ToolTip.SetTip(_cameraAnimSpin, Localization.Tr("CameraAngle must be set to 'Animated' to use this feature."));
                }
                else
                {
                    _cameraAnimSpin.IsEnabled = true;
                    ToolTip.SetTip(_cameraAnimSpin, Localization.Tr("CameraAnimation: Index into the CameraModel for animated cutscene cameras. WORD 0-65535. Used when CameraAngle is Animated Camera (4). Leave 0 for default."));
                }
            }

            // Static Camera + CameraID -1: show error tooltip. When Camera ID is disabled, show why-it's-greyed-out tooltip.
            bool staticCameraError = cameraId == -1 && cameraAngleIndex == StaticCameraIndex;
            if (_cameraIdSpin != null)
            {
                object cameraIdTip;
                if (!cameraIdEnabled)
                    cameraIdTip = Localization.Tr("Camera ID is only used when Camera Angle is set to Static Camera (6).");
                else if (staticCameraError)
                    cameraIdTip = Localization.Tr("A Camera ID must be defined for Static Cameras.");
                else
                    cameraIdTip = CreateCameraIdStyledTooltip();
                ToolTip.SetTip(_cameraIdSpin, cameraIdTip);
            }
            if (_cameraAngleSelect != null)
            {
                object cameraAngleTip = staticCameraError
                    ? (object)Localization.Tr("A Camera ID must be defined for Static Cameras.")
                    : CreateCameraAngleStyledTooltip();
                ToolTip.SetTip(_cameraAngleSelect, cameraAngleTip);
            }
            // Avalonia: use Classes for error styling (requires .error style in XAML)
            if (_cameraIdSpin != null)
                _cameraIdSpin.Classes.Set("error", staticCameraError);
            if (_cameraAngleSelect != null)
                _cameraAngleSelect.Classes.Set("error", staticCameraError);
        }

        /// <summary>
        /// Updates file-level properties (root DLG fields) based on UI changes.
        /// </summary>
        private void OnFilePropertyChanged()
        {
            if (_coreDlg == null)
            {
                return;
            }

            // Update file-level (root) fields from UI
            if (_voIdEdit != null)
            {
                _coreDlg.VoId = _voIdEdit.Text ?? string.Empty;
            }
            if (_cameraModelSelect != null)
            {
                string cameraText = _cameraModelSelect.Text?.Trim() ?? string.Empty;
                _coreDlg.CameraModel = ResRef.IsValid(cameraText) ? new ResRef(cameraText) : ResRef.FromBlank();
            }
        }

        /// <summary>
        /// Updates the tree view with the current model data.
        /// </summary>
        public void UpdateTreeView()
        {
            if (_dialogTree == null || _model == null)
            {
                return;
            }

            _suppressTreeSelectionHandler = true;
            try
            {
                var treeItems = new List<TreeViewItem>();
                foreach (var rootView in DlgTreeItemView.CreateRoots(_coreDlg))
                {
                    var treeItem = CreateTreeViewItem(rootView);
                    treeItems.Add(treeItem);
                }
                _dialogTree.ItemsSource = treeItems;
                // Refresh orphaned nodes list when tree structure changes (paste, delete, undo, etc.).
                PopulateOrphanedNodesList();
                PopulateFlatLists();
                RefreshGraphView();
                if (_selectedLink != null)
                {
                    var item = FindItemForLink(_selectedLink);
                    if (item != null)
                    {
                        SelectTreeViewItem(item);
                    }
                    SyncFlatSelection(_selectedLink);
                    SyncGraphSelection(_selectedLink);
                }
            }
            finally
            {
                _suppressTreeSelectionHandler = false;
            }
        }

        /// <summary>
        /// Refreshes all center views (Tree, Flat, Graph) while preserving current selection.
        /// </summary>
        public void RefreshAllViews()
        {
            UpdateTreeView();
            OnSelectionChanged();
        }

        private void SetupGraphView()
        {
            if (_graphCanvas == null)
            {
                return;
            }
            _graphScene = new DlgGraphScene();
            _graphCanvas.Children.Add(_graphScene);
            _graphScene.NodeSelected += tag =>
            {
                if (_isSyncingViewSelection) return;
                if (tag is DLGLink link)
                {
                    _isSyncingViewSelection = true;
                    try
                    {
                        _selectedLink = link;
                        _suppressTreeSelectionHandler = true;
                        try
                        {
                            var item = FindItemForLink(link);
                            if (item != null) SelectTreeViewItem(item);
                        }
                        finally
                        {
                            _suppressTreeSelectionHandler = false;
                        }
                        SyncFlatSelection(link);
                        OnSelectionChanged();
                    }
                    finally
                    {
                        _isSyncingViewSelection = false;
                    }
                }
            };
            _graphScene.NodePositionCommitted += (key, point) =>
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _graphManualPositions[key] = point;
                    SaveGraphLayout();
                }
            };
            _graphScene.ContextRequested += (s, e) =>
            {
                if (!e.TryGetPosition(_graphScene, out Point pos))
                {
                    return;
                }
                var tag = _graphScene.GetNodeTagAt(pos);
                if (tag is DLGLink link)
                {
                    _isSyncingViewSelection = true;
                    try
                    {
                        _selectedLink = link;
                        _suppressTreeSelectionHandler = true;
                        try
                        {
                            var treeItem = FindItemForLink(link);
                            if (treeItem != null) SelectTreeViewItem(treeItem);
                        }
                        finally
                        {
                            _suppressTreeSelectionHandler = false;
                        }
                        SyncFlatSelection(link);
                        OnSelectionChanged();
                    }
                    finally
                    {
                        _isSyncingViewSelection = false;
                    }
                    var menu = GetLinkContextMenuCore(_graphScene, link, FindItemForLink(link));
                    if (menu != null)
                    {
                        menu.Open(_graphScene);
                        e.Handled = true;
                    }
                }
                else
                {
                    // Right-click on empty area: unselect any node and show add-node context menu
                    _suppressTreeSelectionHandler = true;
                    try
                    {
                        _selectedLink = null;
                        if (_dialogTree != null) _dialogTree.SelectedItem = null;
                        if (_flatStartingList != null) _flatStartingList.SelectedItem = null;
                        if (_flatEntryList != null) _flatEntryList.SelectedItem = null;
                        if (_flatReplyList != null) _flatReplyList.SelectedItem = null;
                        if (_graphScene != null) _graphScene.SetSelectedNodeKey(null);
                        OnSelectionChanged();
                    }
                    finally
                    {
                        _suppressTreeSelectionHandler = false;
                    }

                    var contextMenu = new ContextMenu();
                    var addStartingItem = new MenuItem { Header = Localization.Tr("Add Starting Node") };
                    addStartingItem.Click += (_, __) => AddRootNode();
                    contextMenu.Items.Add(addStartingItem);
                    var addEntryItem = new MenuItem { Header = Localization.Tr("Add Entry Node") };
                    addEntryItem.Click += (_, __) => AddRootNode();
                    contextMenu.Items.Add(addEntryItem);
                    var addReplyItem = new MenuItem { Header = Localization.Tr("Add Reply Node") };
                    addReplyItem.Click += (_, __) =>
                    {
                        AddRootNode();
                        var roots = _model?.GetRootItems();
                        if (roots != null && roots.Count > 0)
                        {
                            var lastRoot = roots[roots.Count - 1];
                            AddChildToParentItem(lastRoot);
                        }
                    };
                    contextMenu.Items.Add(addReplyItem);
                    contextMenu.Open(_graphScene);
                    e.Handled = true;
                }
            };
            _graphScene.KeyDown += (s, e) => HandleKeyDown(e, isTreeViewCall: true);
            _graphScene.DoubleTapped += (s, e) =>
            {
                Point pos = e.GetPosition(_graphScene);
                var tag = _graphScene.GetNodeTagAt(pos);
                if (tag is DLGLink link)
                {
                    _selectedLink = link;
                    _isSyncingViewSelection = true;
                    try
                    {
                        _suppressTreeSelectionHandler = true;
                        try
                        {
                            var treeItem = FindItemForLink(link);
                            if (treeItem != null) SelectTreeViewItem(treeItem);
                        }
                        finally { _suppressTreeSelectionHandler = false; }
                        SyncFlatSelection(link);
                        OnSelectionChanged();
                    }
                    finally { _isSyncingViewSelection = false; }
                    var indexes = GetSelectedIndexesFromCurrentLink();
                    if (indexes.Count > 0)
                        EditText(null, indexes, _graphScene);
                }
            };
            if (_graphFitButton != null)
            {
                _graphFitButton.Click += (s, e) => _graphScene?.FitToContent();
            }
            if (_graphAutoLayoutButton != null)
            {
                _graphAutoLayoutButton.Click += (s, e) =>
                {
                    if (_graphScene == null) return;
                    _graphScene.AutoLayout(keepPinned: false);
                    _graphManualPositions.Clear();
                    foreach (var kvp in _graphScene.ExportNodePositions(pinnedOnly: false))
                    {
                        _graphManualPositions[kvp.Key] = kvp.Value;
                    }
                    SaveGraphLayout();
                };
            }
            _graphScene.LinkDragCompletedOnNode += (sourceTag, targetTag, screenPos) =>
            {
                OnGraphLinkDragToNode(sourceTag, targetTag, screenPos);
            };
            _graphScene.LinkDragCompletedOnEmpty += (sourceTag, worldPos) =>
            {
                OnGraphLinkDragToEmpty(sourceTag, worldPos);
            };
            _graphScene.ZoomChanged += () => UpdateGraphZoomLabel();
            if (_graphZoomInButton != null)
                _graphZoomInButton.Click += (s, e) => { _graphScene?.ZoomIn(); UpdateGraphZoomLabel(); };
            if (_graphZoomOutButton != null)
                _graphZoomOutButton.Click += (s, e) => { _graphScene?.ZoomOut(); UpdateGraphZoomLabel(); };
        }

        private void UpdateGraphZoomLabel()
        {
            if (_graphZoomLabel != null && _graphScene != null)
                _graphZoomLabel.Text = (int)(_graphScene.Zoom * 100) + "%";
        }

        private void OnGraphLinkDragToNode(object sourceTag, object targetTag, Point screenPos)
        {
            var sourceLink = sourceTag as DLGLink;
            var targetLink = targetTag as DLGLink;
            if (sourceLink?.Node == null || targetLink?.Node == null)
                return;

            bool sourceIsEntry = sourceLink.Node is DLGEntry;
            bool targetIsEntry = targetLink.Node is DLGEntry;
            string sourceType = sourceIsEntry ? "Entry" : "Reply";
            string targetType = targetIsEntry ? "Entry" : "Reply";

            if (sourceIsEntry == targetIsEntry)
            {
                string msg = $"Cannot link two {sourceType} nodes. {sourceType} nodes can only connect to {(sourceIsEntry ? "Reply" : "Entry")} nodes.";
                _ = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                    Localization.Tr("Invalid Link"),
                    msg,
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Warning).ShowWindowDialogAsync(this);
                return;
            }

            var sourceItem = FindItemForLink(sourceLink);
            if (sourceItem == null) return;

            var newLink = new DLGLink(targetLink.Node);
            newLink.ListIndex = sourceItem.Link.Node.Links.Count;
            sourceItem.Link.Node.Links.Add(newLink);
            _coreDlg?.Touch();
            var newItem = new DLGStandardItem(newLink);
            sourceItem.AddChild(newItem);
            if (_model != null)
            {
                if (!_model.LinkToItems.ContainsKey(newLink))
                    _model.LinkToItems[newLink] = new List<DLGStandardItem>();
                if (!_model.LinkToItems[newLink].Contains(newItem))
                    _model.LinkToItems[newLink].Add(newItem);
                if (newLink.Node != null)
                {
                    if (!_model.NodeToItems.ContainsKey(newLink.Node))
                        _model.NodeToItems[newLink.Node] = new List<DLGStandardItem>();
                    if (!_model.NodeToItems[newLink.Node].Contains(newItem))
                        _model.NodeToItems[newLink.Node].Add(newItem);
                }
            }
            UpdateTreeView();
            SelectTreeViewItem(newItem);
        }

        private void OnGraphLinkDragToEmpty(object sourceTag, Point worldPos)
        {
            var sourceLink = sourceTag as DLGLink;
            if (sourceLink?.Node == null) return;

            bool sourceIsEntry = sourceLink.Node is DLGEntry;
            string childType = sourceIsEntry ? "Reply" : "Entry";

            var contextMenu = new ContextMenu();
            var newLinkedItem = new MenuItem { Header = Localization.Tr($"New Linked {childType} Here") };
            newLinkedItem.Click += (_, __) =>
            {
                var sourceItem = FindItemForLink(sourceLink);
                if (sourceItem == null) return;
                AddChildToParentItem(sourceItem);

                var lastChild = sourceItem.Children?.LastOrDefault();
                if (lastChild?.Link?.Node != null && _graphScene != null)
                {
                    string key = GetNodeKey(lastChild.Link.Node);
                    _graphScene.SetNodePosition(key, worldPos);
                    _graphManualPositions[key] = worldPos;
                    SaveGraphLayout();
                    RefreshGraphView();
                }
            };
            contextMenu.Items.Add(newLinkedItem);
            contextMenu.Open(_graphScene);
        }

        private void WireFlatViewHandlers()
        {
            var flatTemplate = new FuncDataTemplate<FlatNodeRow>((row, _) =>
            {
                if (row?.Visual != null)
                    return row.Visual;
                return new TextBlock { Text = row?.Display ?? "" };
            }, true);
            if (_flatStartingList != null) _flatStartingList.ItemTemplate = flatTemplate;
            if (_flatEntryList != null) _flatEntryList.ItemTemplate = flatTemplate;
            if (_flatReplyList != null) _flatReplyList.ItemTemplate = flatTemplate;

            void OnFlatSelectionChanged(object s, EventArgs e)
            {
                if (_isSyncingViewSelection) return;
                var list = s as ListBox;
                var row = list?.SelectedItem as FlatNodeRow;
                if (row?.Link == null) return;
                _isSyncingViewSelection = true;
                try
                {
                    _selectedLink = row.Link;
                    _suppressTreeSelectionHandler = true;
                    try
                    {
                        var item = FindItemForLink(row.Link);
                        if (item != null) SelectTreeViewItem(item);
                    }
                    finally
                    {
                        _suppressTreeSelectionHandler = false;
                    }
                    SyncGraphSelection(row.Link);
                    OnSelectionChanged();
                }
                finally
                {
                    _isSyncingViewSelection = false;
                }
            }
            if (_flatStartingList != null)
            {
                _flatStartingList.SelectionChanged += OnFlatSelectionChanged;
                _flatStartingList.ContextRequested += OnFlatListContextRequested;
                _flatStartingList.KeyDown += (s, e) => HandleKeyDown(e, isTreeViewCall: true);
                _flatStartingList.DoubleTapped += OnFlatListDoubleTapped;
            }
            if (_flatEntryList != null)
            {
                _flatEntryList.SelectionChanged += OnFlatSelectionChanged;
                _flatEntryList.ContextRequested += OnFlatListContextRequested;
                _flatEntryList.KeyDown += (s, e) => HandleKeyDown(e, isTreeViewCall: true);
                _flatEntryList.DoubleTapped += OnFlatListDoubleTapped;
            }
            if (_flatReplyList != null)
            {
                _flatReplyList.SelectionChanged += OnFlatSelectionChanged;
                _flatReplyList.ContextRequested += OnFlatListContextRequested;
                _flatReplyList.KeyDown += (s, e) => HandleKeyDown(e, isTreeViewCall: true);
                _flatReplyList.DoubleTapped += OnFlatListDoubleTapped;
            }
        }

        private void OnFlatListDoubleTapped(object sender, TappedEventArgs e)
        {
            var list = sender as ListBox;
            if (list == null) return;
            var row = list.SelectedItem as FlatNodeRow;
            if (row?.Link == null) return;
            _selectedLink = row.Link;
            _isSyncingViewSelection = true;
            try
            {
                _suppressTreeSelectionHandler = true;
                try
                {
                    var treeItem = FindItemForLink(row.Link);
                    if (treeItem != null) SelectTreeViewItem(treeItem);
                }
                finally { _suppressTreeSelectionHandler = false; }
                SyncGraphSelection(row.Link);
                OnSelectionChanged();
            }
            finally { _isSyncingViewSelection = false; }
            var indexes = GetSelectedIndexesFromCurrentLink();
            if (indexes.Count > 0)
                EditText(null, indexes, list);
        }

        private void OnFlatListContextRequested(object sender, ContextRequestedEventArgs e)
        {
            var list = sender as ListBox;
            if (list == null) return;
            var row = list.SelectedItem as FlatNodeRow;
            if (row?.Link == null)
            {
                e.Handled = true;
                return;
            }
            _isSyncingViewSelection = true;
            try
            {
                _selectedLink = row.Link;
                _suppressTreeSelectionHandler = true;
                try
                {
                    var treeItem = FindItemForLink(row.Link);
                    if (treeItem != null) SelectTreeViewItem(treeItem);
                }
                finally
                {
                    _suppressTreeSelectionHandler = false;
                }
                SyncGraphSelection(row.Link);
                OnSelectionChanged();
            }
            finally
            {
                _isSyncingViewSelection = false;
            }
            var menuItem = FindItemForLink(row.Link);
            var menu = GetLinkContextMenuCore(list, row.Link, menuItem);
            if (menu != null)
            {
                menu.Open(list);
                e.Handled = true;
            }
        }

        private void PopulateFlatLists()
        {
            if (_coreDlg == null)
            {
                return;
            }

            _isSyncingViewSelection = true;
            try
            {
                var nodeLinkMap = BuildNodeToLinkMap();

                var starters = new List<FlatNodeRow>();
                if (_coreDlg.Starters != null)
                {
                    foreach (var link in _coreDlg.Starters)
                    {
                        if (link?.Node == null) continue;
                        starters.Add(BuildFlatRow(link.Node, link, "S", AmColor.Parse(DlgGraphColors.StarterHex)));
                    }
                }
                if (_flatStartingList != null) _flatStartingList.ItemsSource = starters;

                var entries = new List<FlatNodeRow>();
                foreach (var entry in _coreDlg.EntryList)
                {
                    if (entry == null) continue;
                    nodeLinkMap.TryGetValue(entry, out DLGLink entryLink);
                    entries.Add(BuildFlatRow(entry, entryLink, "E", AmColor.Parse(DlgGraphColors.EntryHex)));
                }
                if (_flatEntryList != null) _flatEntryList.ItemsSource = entries;

                var replies = new List<FlatNodeRow>();
                foreach (var reply in _coreDlg.ReplyList)
                {
                    if (reply == null) continue;
                    nodeLinkMap.TryGetValue(reply, out DLGLink replyLink);
                    replies.Add(BuildFlatRow(reply, replyLink, "R", AmColor.Parse(DlgGraphColors.ReplyHex)));
                }
                if (_flatReplyList != null) _flatReplyList.ItemsSource = replies;
            }
            finally
            {
                _isSyncingViewSelection = false;
            }
        }

        private FlatNodeRow BuildFlatRow(DLGNode node, DLGLink link, string typePrefix, AmColor pillColor)
        {
            string idx = node.ListIndex >= 0 ? node.ListIndex.ToString() : "?";
            string text = GetNodePreviewText(node);
            bool isEmpty = string.IsNullOrWhiteSpace(text);
            if (isEmpty) text = "<empty>";
            string display = typePrefix + idx + ": " + text;

            var pill = new Border
            {
                Background = new SolidColorBrush(pillColor),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(4, 1, 4, 1),
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock
                {
                    Text = typePrefix + idx,
                    FontSize = 10,
                    FontWeight = FontWeight.SemiBold,
                    Foreground = Brushes.White
                }
            };

            var textBlock = new TextBlock
            {
                Text = text,
                TextTrimming = TextTrimming.CharacterEllipsis,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = isEmpty
                    ? new SolidColorBrush(AmColor.Parse("#B0BEC5"))
                    : new SolidColorBrush(AmColor.Parse("#212121")),
                FontStyle = isEmpty ? FontStyle.Italic : FontStyle.Normal
            };

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6
            };
            panel.Children.Add(pill);

            bool hasScript = (node.Script1 != null && !node.Script1.IsBlank())
                          || (node.Script2 != null && !node.Script2.IsBlank());
            bool hasCond = (link?.Active1 != null && !link.Active1.IsBlank())
                        || (link?.Active2 != null && !link.Active2.IsBlank());
            if (hasScript)
            {
                var badge = new TextBlock
                {
                    Text = "S",
                    FontSize = 9,
                    FontWeight = FontWeight.Bold,
                    Foreground = new SolidColorBrush(AmColor.Parse("#6A1B9A")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 2, 0)
                };
                ToolTip.SetTip(badge, "Has script");
                panel.Children.Add(badge);
            }
            if (hasCond)
            {
                var badge = new TextBlock
                {
                    Text = "?",
                    FontSize = 9,
                    FontWeight = FontWeight.Bold,
                    Foreground = new SolidColorBrush(AmColor.Parse("#E65100")),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 2, 0)
                };
                ToolTip.SetTip(badge, "Has condition");
                panel.Children.Add(badge);
            }

            panel.Children.Add(textBlock);

            return new FlatNodeRow
            {
                Link = link,
                Node = node,
                Display = display,
                Visual = panel
            };
        }

        private Dictionary<DLGNode, DLGLink> BuildNodeToLinkMap()
        {
            var map = new Dictionary<DLGNode, DLGLink>();
            if (_coreDlg?.Starters == null) return map;
            var seen = new HashSet<DLGLink>();
            var queue = new Queue<DLGLink>();
            foreach (var s in _coreDlg.Starters)
            {
                if (s != null && !seen.Contains(s))
                {
                    seen.Add(s);
                    queue.Enqueue(s);
                }
            }
            while (queue.Count > 0)
            {
                var link = queue.Dequeue();
                if (link?.Node != null && !map.ContainsKey(link.Node))
                {
                    map[link.Node] = link;
                }
                if (link?.Node?.Links != null)
                {
                    foreach (var child in link.Node.Links)
                    {
                        if (child != null && !seen.Contains(child))
                        {
                            seen.Add(child);
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            return map;
        }

        private void RefreshGraphView()
        {
            if (_graphScene == null || _coreDlg == null)
            {
                return;
            }

            if (_graphManualPositions == null || _graphManualPositions.Count == 0)
            {
                _graphManualPositions = LoadGraphLayout();
            }

            var nodes = new Dictionary<string, DlgGraphNodeData>(StringComparer.OrdinalIgnoreCase);
            var edges = new List<DlgGraphEdgeData>();

            if (_coreDlg.Starters != null)
            {
                foreach (var starter in _coreDlg.Starters)
                {
                    BuildGraphNodesAndEdges(starter, nodes, edges);
                    if (starter?.Node != null)
                    {
                        string k = GetNodeKey(starter.Node);
                        if (!string.IsNullOrEmpty(k) && nodes.ContainsKey(k))
                            nodes[k].Kind = DlgGraphNodeKind.Starter;
                    }
                }
            }

            foreach (var entry in _coreDlg.EntryList)
            {
                EnsureGraphNode(nodes, entry);
            }
            foreach (var reply in _coreDlg.ReplyList)
            {
                EnsureGraphNode(nodes, reply);
            }

            _graphScene.SetGraph(nodes.Values.ToList(), edges, _graphManualPositions);
            _graphScene.SetSelectedNodeKey(GetNodeKey((_selectedLink ?? GetSelectedItemFromTreeView()?.Link)?.Node));
            UpdateGraphZoomLabel();
            if (_graphStatusText != null)
            {
                int entries = 0, replies = 0, starters = 0;
                foreach (var n in nodes.Values)
                {
                    switch (n.Kind)
                    {
                        case DlgGraphNodeKind.Starter: starters++; break;
                        case DlgGraphNodeKind.Reply: replies++; break;
                        default: entries++; break;
                    }
                }
                int conditionalEdges = 0;
                foreach (var e in edges)
                    if (e.HasCondition) conditionalEdges++;
                string status = $"{starters} Starters, {entries} Entries, {replies} Replies  |  {edges.Count} Edges ({conditionalEdges} conditional)";
                if (_graphScene.OrphanCount > 0)
                    status += $"  |  {_graphScene.OrphanCount} orphan(s)";
                _graphStatusText.Text = status;
            }
        }

        private void SyncGraphSelection(DLGLink link)
        {
            if (_graphScene != null && link?.Node != null)
            {
                string key = GetNodeKey(link.Node);
                _graphScene.SetSelectedNodeKey(key);
                _graphScene.CenterOnNode(key);
            }
        }

        private void SyncFlatSelection(DLGLink link)
        {
            if (link?.Node == null) return;
            bool wasAlreadySyncing = _isSyncingViewSelection;
            _isSyncingViewSelection = true;
            try
            {
                if (_flatStartingList?.ItemsSource is System.Collections.IEnumerable startItems)
                {
                    foreach (FlatNodeRow row in startItems)
                    {
                        if (row?.Link == link)
                        {
                            _flatStartingList.SelectedItem = row;
                            if (_flatEntryList != null) _flatEntryList.SelectedItem = null;
                            if (_flatReplyList != null) _flatReplyList.SelectedItem = null;
                            return;
                        }
                    }
                }
                if (_flatEntryList?.ItemsSource is System.Collections.IEnumerable entryItems)
                {
                    foreach (FlatNodeRow row in entryItems)
                    {
                        if (row?.Node == link.Node)
                        {
                            if (_flatStartingList != null) _flatStartingList.SelectedItem = null;
                            _flatEntryList.SelectedItem = row;
                            if (_flatReplyList != null) _flatReplyList.SelectedItem = null;
                            return;
                        }
                    }
                }
                if (_flatReplyList?.ItemsSource is System.Collections.IEnumerable replyItems)
                {
                    foreach (FlatNodeRow row in replyItems)
                    {
                        if (row?.Node == link.Node)
                        {
                            if (_flatStartingList != null) _flatStartingList.SelectedItem = null;
                            if (_flatEntryList != null) _flatEntryList.SelectedItem = null;
                            _flatReplyList.SelectedItem = row;
                            return;
                        }
                    }
                }
                if (_flatStartingList != null) _flatStartingList.SelectedItem = null;
                if (_flatEntryList != null) _flatEntryList.SelectedItem = null;
                if (_flatReplyList != null) _flatReplyList.SelectedItem = null;
            }
            finally
            {
                _isSyncingViewSelection = wasAlreadySyncing;
            }
        }

        private void BuildGraphNodesAndEdges(
            DLGLink start,
            Dictionary<string, DlgGraphNodeData> nodes,
            List<DlgGraphEdgeData> edges)
        {
            if (start?.Node == null)
            {
                return;
            }

            var queue = new Queue<DLGLink>();
            var seenLinks = new HashSet<DLGLink>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var link = queue.Dequeue();
                if (link?.Node == null || seenLinks.Contains(link))
                {
                    continue;
                }
                seenLinks.Add(link);

                EnsureGraphNode(nodes, link.Node, link);
                string fromKey = GetNodeKey(link.Node);
                if (link.Node.Links == null)
                {
                    continue;
                }

                foreach (var child in link.Node.Links)
                {
                    if (child?.Node == null)
                    {
                        continue;
                    }
                    EnsureGraphNode(nodes, child.Node, child);
                    string toKey = GetNodeKey(child.Node);
                    bool hasCond = (child.Active1 != null && !child.Active1.IsBlank())
                                || (child.Active2 != null && !child.Active2.IsBlank());
                    edges.Add(new DlgGraphEdgeData { FromKey = fromKey, ToKey = toKey, HasCondition = hasCond });
                    queue.Enqueue(child);
                }
            }
        }

        private void EnsureGraphNode(
            Dictionary<string, DlgGraphNodeData> nodes,
            DLGNode node,
            DLGLink preferredLink = null)
        {
            if (node == null)
            {
                return;
            }
            string key = GetNodeKey(node);
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            if (nodes.ContainsKey(key))
            {
                return;
            }

            var link = preferredLink ?? ResolveRepresentativeLink(node);
            var badges = DlgGraphNodeBadges.None;
            if (node.Script1 != null && !node.Script1.IsBlank()) badges |= DlgGraphNodeBadges.HasScript;
            if (node.Script2 != null && !node.Script2.IsBlank()) badges |= DlgGraphNodeBadges.HasScript;
            if (link != null && link.Active1 != null && !link.Active1.IsBlank()) badges |= DlgGraphNodeBadges.HasCondition;
            if (link != null && link.Active2 != null && !link.Active2.IsBlank()) badges |= DlgGraphNodeBadges.HasCondition;
            if (node.Sound != null && !node.Sound.IsBlank()) badges |= DlgGraphNodeBadges.HasSound;
            if (node.VoResRef != null && !node.VoResRef.IsBlank()) badges |= DlgGraphNodeBadges.HasVoice;

            nodes[key] = new DlgGraphNodeData
            {
                Key = key,
                Title = GetNodeTitle(node),
                Subtitle = GetNodePreviewText(node),
                Tag = link,
                Kind = node is DLGReply ? DlgGraphNodeKind.Reply : DlgGraphNodeKind.Entry,
                Badges = badges,
                ChildCount = node.Links?.Count ?? 0
            };
        }

        private static string GetNodeKey(DLGNode node)
        {
            if (node == null) return "";
            if (node is DLGEntry) return "E" + node.ListIndex;
            if (node is DLGReply) return "R" + node.ListIndex;
            return "";
        }

        /// <summary>
        /// Resolves the display string for a node: TLK text when StringRef != -1 (and Installation available),
        /// otherwise custom substring when StringRef == -1. Used so tree/flat/graph show the correct text.
        /// </summary>
        public string GetResolvedNodeText(DLGNode node)
        {
            if (node?.Text == null) return "";
            if (Installation != null)
            {
                return Installation.String(node.Text, "") ?? "";
            }
            if (node.Text.StringRef == -1)
            {
                string custom = node.Text.GetString(0, Gender.Male) ?? "";
                return custom;
            }
            return $"(strref: {node.Text.StringRef})";
        }

        private string GetNodePreviewText(DLGNode node)
        {
            string text = GetResolvedNodeText(node);
            return string.IsNullOrEmpty(text) ? "<empty>" : text;
        }

        private static string GetNodeTitle(DLGNode node)
        {
            if (node == null) return "";
            return GetNodeKey(node);
        }

        private DLGLink ResolveRepresentativeLink(DLGNode node)
        {
            if (node == null || _coreDlg?.Starters == null) return null;
            var seen = new HashSet<DLGLink>();
            var queue = new Queue<DLGLink>();
            foreach (var s in _coreDlg.Starters)
            {
                if (s != null && !seen.Contains(s))
                {
                    seen.Add(s);
                    queue.Enqueue(s);
                }
            }
            while (queue.Count > 0)
            {
                var link = queue.Dequeue();
                if (link?.Node == node) return link;
                if (link?.Node?.Links != null)
                {
                    foreach (var child in link.Node.Links)
                    {
                        if (child != null && !seen.Contains(child))
                        {
                            seen.Add(child);
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            return null;
        }

        private string BuildGraphLayoutSettingsKey()
        {
            string raw = FilepathPublic ?? "unsaved";
            raw = raw.ToLowerInvariant();
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                raw = raw.Replace(ch, '_');
            }
            raw = raw.Replace('\\', '_').Replace('/', '_').Replace(':', '_');
            return "graph_layouts." + raw;
        }

        private Dictionary<string, Point> LoadGraphLayout()
        {
            var result = new Dictionary<string, Point>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var settings = new DLGSettings();
                string raw = settings.Get(BuildGraphLayoutSettingsKey(), "");
                if (string.IsNullOrWhiteSpace(raw))
                {
                    return result;
                }
                var dto = JsonSerializer.Deserialize<Dictionary<string, GraphPoint>>(raw);
                if (dto == null)
                {
                    return result;
                }
                foreach (var kvp in dto)
                {
                    if (kvp.Value == null)
                    {
                        continue;
                    }
                    result[kvp.Key] = new Point(kvp.Value.X, kvp.Value.Y);
                }
            }
            catch
            {
                // Ignore malformed persisted layout.
            }
            return result;
        }

        private void SaveGraphLayout()
        {
            try
            {
                var settings = new DLGSettings();
                var dto = new Dictionary<string, GraphPoint>(StringComparer.OrdinalIgnoreCase);
                foreach (var kvp in _graphManualPositions)
                {
                    dto[kvp.Key] = new GraphPoint { X = kvp.Value.X, Y = kvp.Value.Y };
                }
                settings.Set(BuildGraphLayoutSettingsKey(), JsonSerializer.Serialize(dto));
            }
            catch
            {
                // Ignore persistence errors.
            }
        }

        private sealed class GraphPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
        }

        /// <summary>
        /// Updates a specific tree view item's header without rebuilding the entire tree.
        /// This is an optimized version that only updates the specified item.
        /// </summary>
        /// <param name="item">The DLGStandardItem to update.</param>
        /// <param name="formattedText">The formatted HTML text to display.</param>
        /// <param name="tooltipText">Optional tooltip text to display.</param>
        public void UpdateTreeViewItemHeader(DLGStandardItem item, string formattedText, string tooltipText = null)
        {
            if (_dialogTree == null || item == null || string.IsNullOrEmpty(formattedText))
            {
                return;
            }

            // Find the corresponding TreeViewItem
            TreeViewItem treeItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, item);
            if (treeItem != null)
            {
                // Update the header with formatted text
                // Note: Avalonia TreeViewItem.Header can accept string or object
                // For HTML formatting, we'll use a TextBlock with Inlines or just set the text
                // Since Avalonia doesn't natively support HTML in TreeViewItem headers,
                // we'll strip HTML tags for now and use plain text
                // In a full implementation, we would use a custom DataTemplate with TextBlock and Inlines
                string plainText = System.Text.RegularExpressions.Regex.Replace(formattedText, "<.*?>", "");
                treeItem.Header = plainText;

                // Set tooltip if provided
                if (!string.IsNullOrEmpty(tooltipText))
                {
                    // Strip HTML from tooltip for plain text display
                    string plainTooltip = System.Text.RegularExpressions.Regex.Replace(tooltipText, "<.*?>", "");
                    ToolTip.SetTip(treeItem, plainTooltip);
                }

                // Force the tree item and tree to re-render so the new header is visible (Avalonia may not refresh otherwise)
                treeItem.InvalidateMeasure();
                treeItem.InvalidateVisual();
                _dialogTree?.InvalidateMeasure();
                _dialogTree?.InvalidateVisual();
            }
            else
            {
                // If item not found in tree, fall back to full tree update
                // This can happen if the tree hasn't been built yet or the item was removed
                UpdateTreeView();
            }
        }

        /// <summary>
        /// Updates item presentation across all center views (tree, flat, graph).
        /// </summary>
        public void UpdateItemPresentation(DLGStandardItem item, string formattedText, string tooltipText = null)
        {
            UpdateTreeViewItemHeader(item, formattedText, tooltipText);
            PopulateFlatLists();
            RefreshGraphView();
        }

        /// <summary>
        /// Selects the specified DLGStandardItem in the tree view.
        /// </summary>
        /// <param name="item">The DLGStandardItem to select.</param>
        public void SelectTreeItem(DLGStandardItem item)
        {
            if (_dialogTree == null || _dialogTree.ItemsSource == null || item == null)
            {
                return;
            }

            // Recursively search for the tree view item matching the DLGStandardItem
            TreeViewItem foundItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, item);
            if (foundItem != null)
            {
                _dialogTree.SelectedItem = foundItem;
                // Expand parent items to ensure the selected item is visible
                ExpandParentItems(foundItem);
            }
        }

        /// <summary>
        /// Creates a TreeViewItem from a DLGStandardItem, recursively creating children.
        /// </summary>
        private static readonly string LazyChildPlaceholder = "\u2026";

        private TreeViewItem CreateTreeViewItem(DLGStandardItem item)
        {
            var treeItem = new TreeViewItem
            {
                Header = BuildRichTreeHeader(item),
                Tag = item,
                IsExpanded = false
            };

            // Lazy tree expansion: do not recursively materialize entire tree up front.
            if (item != null && item.RowCount > 0)
            {
                treeItem.ItemsSource = new List<TreeViewItem>
                {
                    new TreeViewItem { Header = LazyChildPlaceholder }
                };
                treeItem.Expanded += (_, __) => EnsureTreeChildrenMaterialized(treeItem, item);
            }
            return treeItem;
        }

        private TreeViewItem CreateTreeViewItem(DlgTreeItemView itemView)
        {
            var link = itemView?.Link;
            DLGStandardItem backingItem = FindItemForLink(link);
            if (backingItem == null && link != null)
            {
                backingItem = new DLGStandardItem(link);
            }

            var treeItem = new TreeViewItem
            {
                Header = BuildRichTreeHeader(backingItem),
                Tag = backingItem,
                IsExpanded = false
            };

            if (itemView != null && itemView.Children.Count > 0)
            {
                treeItem.ItemsSource = new List<TreeViewItem>
                {
                    new TreeViewItem { Header = LazyChildPlaceholder }
                };
                treeItem.Expanded += (_, __) => EnsureTreeChildrenMaterialized(treeItem, itemView);
            }

            return treeItem;
        }

        private void EnsureTreeChildrenMaterialized(TreeViewItem treeItem, DLGStandardItem item)
        {
            if (treeItem == null || item == null)
            {
                return;
            }
            if (!(treeItem.ItemsSource is List<TreeViewItem> existing))
            {
                return;
            }
            bool isPlaceholderState = existing.Count == 1
                && existing[0] != null
                && string.Equals(existing[0].Header as string, LazyChildPlaceholder, StringComparison.Ordinal);
            if (!isPlaceholderState)
            {
                return;
            }

            var childItems = new List<TreeViewItem>();
            foreach (var child in item.Children)
            {
                childItems.Add(CreateTreeViewItem(child));
            }
            treeItem.ItemsSource = childItems;
        }

        private void EnsureTreeChildrenMaterialized(TreeViewItem treeItem, DlgTreeItemView itemView)
        {
            if (treeItem == null || itemView == null)
            {
                return;
            }
            if (!(treeItem.ItemsSource is List<TreeViewItem> existing))
            {
                return;
            }
            bool isPlaceholderState = existing.Count == 1
                && existing[0] != null
                && string.Equals(existing[0].Header as string, LazyChildPlaceholder, StringComparison.Ordinal);
            if (!isPlaceholderState)
            {
                return;
            }

            var childItems = new List<TreeViewItem>();
            foreach (var child in itemView.Children)
            {
                childItems.Add(CreateTreeViewItem(child));
            }
            treeItem.ItemsSource = childItems;
        }

        private Control BuildRichTreeHeader(DLGStandardItem item)
        {
            var link = item?.Link;
            var node = link?.Node;
            if (node == null)
                return new TextBlock { Text = "Unknown" };

            bool isEntry = node is DLGEntry;
            bool isStarter = false;
            if (_coreDlg?.Starters != null)
            {
                foreach (var s in _coreDlg.Starters)
                    if (s?.Node == node) { isStarter = true; break; }
            }

            string typeLabel = isStarter ? "S" : isEntry ? "E" : "R";
            string indexLabel = node.ListIndex >= 0 ? node.ListIndex.ToString() : "?";
            AmColor pillBg, pillFg;
            if (isStarter)
            {
                pillBg = AmColor.Parse(DlgGraphColors.StarterHex);
                pillFg = Colors.White;
            }
            else if (isEntry)
            {
                pillBg = AmColor.Parse(DlgGraphColors.EntryHex);
                pillFg = Colors.White;
            }
            else
            {
                pillBg = AmColor.Parse(DlgGraphColors.ReplyHex);
                pillFg = Colors.White;
            }

            var pill = new Border
            {
                Background = new SolidColorBrush(pillBg),
                CornerRadius = new CornerRadius(3),
                Padding = new Thickness(5, 1, 5, 1),
                VerticalAlignment = VerticalAlignment.Center,
                Child = new TextBlock
                {
                    Text = typeLabel + indexLabel,
                    FontSize = 11,
                    FontWeight = FontWeight.SemiBold,
                    Foreground = new SolidColorBrush(pillFg)
                }
            };

            string text = GetResolvedNodeText(node);
            bool isEmpty = string.IsNullOrWhiteSpace(text);
            if (isEmpty) text = "<empty>";
            if (text.Length > 90) text = text.Substring(0, 87) + "...";

            var textBlock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                MaxWidth = 600,
                Foreground = isEmpty
                    ? new SolidColorBrush(AmColor.Parse("#B0BEC5"))
                    : new SolidColorBrush(AmColor.Parse("#212121")),
                FontStyle = isEmpty ? FontStyle.Italic : FontStyle.Normal
            };

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6
            };
            panel.Children.Add(pill);

            var badges = BuildTreeBadgePanel(node, link);
            if (badges != null)
                panel.Children.Add(badges);

            panel.Children.Add(textBlock);

            if (isEmpty)
            {
                var warnBorder = new Border
                {
                    Background = new SolidColorBrush(AmColor.Parse("#FFF8E1")),
                    CornerRadius = new CornerRadius(2),
                    Padding = new Thickness(2, 0),
                    Child = panel
                };
                return warnBorder;
            }

            return panel;
        }

        private StackPanel BuildTreeBadgePanel(DLGNode node, DLGLink link)
        {
            var icons = new List<(string symbol, AmColor color, string tip)>();

            bool hasScript = (node.Script1 != null && !node.Script1.IsBlank())
                          || (node.Script2 != null && !node.Script2.IsBlank());
            bool hasCond = (link?.Active1 != null && !link.Active1.IsBlank())
                        || (link?.Active2 != null && !link.Active2.IsBlank());
            bool hasSound = node.Sound != null && !node.Sound.IsBlank();
            bool hasVoice = node.VoResRef != null && !node.VoResRef.IsBlank();

            if (hasScript) icons.Add(("S", AmColor.Parse("#6A1B9A"), "Has script"));
            if (hasCond)   icons.Add(("?", AmColor.Parse("#E65100"), "Has condition"));
            if (hasSound)  icons.Add(("\u266A", AmColor.Parse("#00695C"), "Has sound"));
            if (hasVoice)  icons.Add(("\u25B6", AmColor.Parse("#0277BD"), "Has voice"));

            if (icons.Count == 0) return null;

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 2,
                VerticalAlignment = VerticalAlignment.Center
            };
            foreach (var (symbol, color, tip) in icons)
            {
                var badge = new Border
                {
                    Width = 16,
                    Height = 16,
                    CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush(AmColor.FromArgb(30, color.R, color.G, color.B)),
                    BorderBrush = new SolidColorBrush(color),
                    BorderThickness = new Thickness(1),
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = new TextBlock
                    {
                        Text = symbol,
                        FontSize = 9,
                        FontWeight = FontWeight.Bold,
                        Foreground = new SolidColorBrush(color),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };
                ToolTip.SetTip(badge, tip);
                panel.Children.Add(badge);
            }
            return panel;
        }

        /// <summary>
        /// Gets the display text for an item.
        /// </summary>
        private string GetItemDisplayText(DLGStandardItem item)
        {
            if (item?.Link?.Node == null)
            {
                return "Unknown";
            }
            return GetItemDisplayTextFromLink(item.Link);
        }

        /// <summary>
        /// Gets display text for a link (used for list widget context menu and Find References).
        /// </summary>
        private string GetItemDisplayTextFromLink(DLGLink link)
        {
            if (link?.Node == null)
            {
                return "Unknown";
            }
            var node = link.Node;
            string nodeType = node is DLGEntry ? "Entry" : "Reply";
            string text = GetResolvedNodeText(node);
            if (string.IsNullOrEmpty(text))
            {
                text = "<empty>";
            }
            return $"{nodeType}: {text}";
        }

        /// <summary>
        /// Handles key press events from the dialog tree view.
        /// </summary>
        private void OnKeyDownFromTreeView(KeyEventArgs e)
        {
            HandleKeyDown(e, isTreeViewCall: true);
        }

        /// <summary>
        /// Handles double-click on the dialog tree. Vendor: doubleClicked -> edit_text.
        /// Opens LocalizedStringDialog to edit the selected node's text.
        /// </summary>
        private void OnDialogTreeDoubleTapped(object sender, TappedEventArgs e)
        {
            if (_dialogTree == null) return;
            var indexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
            EditText(null, indexes, _dialogTree);
        }

        /// <summary>
        /// Handles key press events for the DLG editor.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            HandleKeyDown(e, isTreeViewCall: false);
        }

        /// <summary>
        /// Internal key handling method that implements the full PyKotor keyPressEvent logic.
        /// </summary>
        /// <param name="e">The key event arguments.</param>
        /// <param name="isTreeViewCall">True if this call originated from the tree view's key handler.</param>
        private void HandleKeyDown(KeyEventArgs e, bool isTreeViewCall)
        {
            Key key = e.Key;
            // Avalonia doesn't have IsRepeat property - track manually via _keysDown
            bool isAutoRepeat = _keysDown.Contains(key);

            if (!isTreeViewCall)
            {
                // Only handle when a node view has focus (tree, flat lists, or graph)
                bool nodeViewFocused = (_dialogTree != null && _dialogTree.IsFocused)
                    || (_flatStartingList != null && _flatStartingList.IsFocused)
                    || (_flatEntryList != null && _flatEntryList.IsFocused)
                    || (_flatReplyList != null && _flatReplyList.IsFocused)
                    || (_graphScene != null && _graphScene.IsFocused);
                if (!nodeViewFocused)
                {
                    return;
                }
                // If dialog tree has focus, the tree view's key handler will call us back with isTreeViewCall=true
                if (_dialogTree != null && _dialogTree.IsFocused)
                {
                    return;
                }
            }

            // Get the selected item from the tree view
            DLGStandardItem selectedItem = GetSelectedItemFromTreeView();

            if (selectedItem == null)
            {
                // If no valid selection but tree has focus, allow Insert key to add root node
                if (key == Key.Insert)
                {
                    AddRootNode();
                    e.Handled = true;
                }
                return;
            }

            if (isAutoRepeat || _keysDown.Contains(key))
            {
                if (key == Key.Up || key == Key.Down)
                {
                    _keysDown.Add(key);
                    HandleShiftItemKeybind(selectedItem, key);
                }
                e.Handled = true;
                return; // Ignore auto-repeat events and prevent multiple executions on single key
            }

            if (_keysDown.Count == 0)
            {
                _keysDown.Add(key);

                if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && key == Key.Z)
                {
                    Undo();
                    e.Handled = true;
                    return;
                }
                if (e.KeyModifiers.HasFlag(KeyModifiers.Control) && (key == Key.Y || (key == Key.Z && e.KeyModifiers.HasFlag(KeyModifiers.Shift))))
                {
                    Redo();
                    e.Handled = true;
                    return;
                }
                if (key == Key.Delete || key == Key.Back)
                {
                    // When tree has a selection, remove that link; otherwise remove selected starter (file globals list)
                    if (selectedItem != null)
                        RemoveLink(selectedItem);
                    else
                        RemoveSelectedLink();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.Enter || key == Key.Return)
                {

                    // Check which widget has focus and call edit_text with appropriate parameters
                    if (_dialogTree != null && _dialogTree.IsFocused)
                    {
                        var selectedIndexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
                        EditText(e, selectedIndexes, _dialogTree);
                    }
                    else if (_orphanedNodesList != null && _orphanedNodesList.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromListWidget(_orphanedNodesList);
                        EditText(e, selectedIndexes, _orphanedNodesList);
                    }
                    else if (_pinnedItemsList != null && _pinnedItemsList.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromListWidget(_pinnedItemsList);
                        EditText(e, selectedIndexes, _pinnedItemsList);
                    }
                    else if (_flatStartingList != null && _flatStartingList.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromCurrentLink();
                        EditText(e, selectedIndexes, _flatStartingList);
                    }
                    else if (_flatEntryList != null && _flatEntryList.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromCurrentLink();
                        EditText(e, selectedIndexes, _flatEntryList);
                    }
                    else if (_flatReplyList != null && _flatReplyList.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromCurrentLink();
                        EditText(e, selectedIndexes, _flatReplyList);
                    }
                    else if (_graphScene != null && _graphScene.IsFocused)
                    {
                        var selectedIndexes = GetSelectedIndexesFromCurrentLink();
                        EditText(e, selectedIndexes, _graphScene);
                    }
                    else if ((_findBar != null && _findBar.IsFocused) || (_findInput != null && _findInput.IsFocused))
                    {
                        HandleFind();
                    }
                    else
                    {
                        var selectedIndexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
                        if (selectedIndexes.Count == 0) selectedIndexes = GetSelectedIndexesFromCurrentLink();
                        EditText(e, selectedIndexes, _dialogTree);
                    }
                    e.Handled = true;
                    return;
                }
                else if (key == Key.F)
                {
                    FocusOnSelectedNode();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.Insert)
                {
                    AddChildToSelectedItem();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.P)
                {
                    PlaySoundOrBlink();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.Home)
                {
                    _graphScene?.CenterOnStarters();
                    UpdateGraphZoomLabel();
                    e.Handled = true;
                    return;
                }
                return;
            }

            _keysDown.Add(key);

            HandleShiftItemKeybind(selectedItem, key);

            if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                if (key == Key.G)
                {
                    ShowGoToBar();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.F)
                {
                    ShowFindBar();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.C)
                {
                    if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
                    {
                        CopyPath();
                    }
                    else
                    {
                        CopyLinkAndNode();
                    }
                    e.Handled = true;
                    return;
                }
                else if (key == Key.Enter || key == Key.Return)
                {
                    JumpToOriginal();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.V)
                {
                    PasteItem(e.KeyModifiers.HasFlag(KeyModifiers.Alt));
                    e.Handled = true;
                    return;
                }
                else if (key == Key.Delete)
                {
                    if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
                    {
                        DeleteNodeEverywhere();
                    }
                    else
                    {
                        DeleteSelectedNode();
                    }
                    e.Handled = true;
                    return;
                }
                else if (key == Key.OemPlus || key == Key.Add)
                {
                    _graphScene?.ZoomIn();
                    UpdateGraphZoomLabel();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.OemMinus || key == Key.Subtract)
                {
                    _graphScene?.ZoomOut();
                    UpdateGraphZoomLabel();
                    e.Handled = true;
                    return;
                }
                else if (key == Key.D0 || key == Key.NumPad0)
                {
                    _graphScene?.FitToContent();
                    UpdateGraphZoomLabel();
                    e.Handled = true;
                    return;
                }
            }

            if (e.KeyModifiers.HasFlag(KeyModifiers.Shift) && (key == Key.Enter || key == Key.Return))
            {
                if (e.KeyModifiers.HasFlag(KeyModifiers.Alt))
                {
                    SetExpandRecursively(false, -1);
                }
                else
                {
                    SetExpandRecursively(true, 0);
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles key release events for the DLG editor.
        /// </summary>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            Key key = e.Key;

            if (_keysDown.Contains(key))
            {
                _keysDown.Remove(key);
            }
        }

        /// <summary>
        /// Handles shift+arrow key combinations for moving items.
        /// </summary>
        /// <param name="selectedItem">The currently selected DLGStandardItem.</param>
        /// <param name="key">The key that was pressed.</param>
        private void HandleShiftItemKeybind(DLGStandardItem selectedItem, Key key)
        {
            if (selectedItem == null || selectedItem.Link == null)
            {
                return;
            }

            // Note: The method checks keys_down set to determine if Shift is held
            if (key == Key.Up && (_keysDown.Contains(Key.LeftShift) || _keysDown.Contains(Key.RightShift)))
            {
                MoveItemUp();
            }
            else if (key == Key.Down && (_keysDown.Contains(Key.LeftShift) || _keysDown.Contains(Key.RightShift)))
            {
                MoveItemDown();
            }
            // Note: Avalonia TreeView doesn't have direct indexAbove/indexBelow methods, so we handle scrolling via selection changes
            // The tree view will automatically scroll to show the selected item
        }

        // Helper methods for key press actions - these will be fully implemented as the UI is completed

        /// <summary>
        /// Adds a root node to the dialog.
        /// Creates a new DLGEntry node, wraps it in a DLGLink, adds it as a starter, and selects it in the tree view.
        /// The operation is recorded in the action history for undo/redo support.
        /// </summary>
        private void AddRootNode()
        {
            // Create and apply the action (this performs the operation and records it for undo/redo)
            var action = new AddRootNodeAction();
            _actionHistory.Apply(action);

            // Get the newly created item from the action
            DLGStandardItem newItem = action.Item;

            if (newItem != null)
            {
                // Select the newly added root node in the tree view
                SelectTreeViewItem(newItem);

                // Update the model's selected index to track the new selection
                var rootItems = _model.GetRootItems();
                int newIndex = -1;
                for (int i = 0; i < rootItems.Count; i++)
                {
                    if (rootItems[i] == newItem)
                    {
                        newIndex = i;
                        break;
                    }
                }
                if (newIndex >= 0)
                {
                    _model.SelectedIndex = newIndex;
                }
            }
        }

        /// <summary>
        /// Removes the selected link.
        /// </summary>
        private void RemoveSelectedLink()
        {
            if (_model.SelectedIndex >= 0 && _model.SelectedIndex < _model.RowCount)
            {
                DLGLink link = _model.GetStarterAt(_model.SelectedIndex);
                if (link != null)
                {
                    RemoveStarter(link);
                }
            }
        }

        /// <summary>
        /// Edits the text of the selected dialog node(s).
        /// </summary>
        /// <param name="e">The key or mouse event that triggered the edit (optional).</param>
        /// <param name="indexes">List of selected indexes (optional, will be determined from sourceWidget if not provided).</param>
        /// <param name="sourceWidget">The widget that triggered the edit (optional, defaults to dialogTree).</param>
        private async void EditText(KeyEventArgs e = null, List<object> indexes = null, Control sourceWidget = null)
        {
            // If no indexes provided, try to get them from sourceWidget
            if (indexes == null || indexes.Count == 0)
            {
                if (sourceWidget != null)
                {
                    if (sourceWidget == _dialogTree)
                    {
                        indexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
                    }
                    else if (sourceWidget is DLGListWidget listWidget)
                    {
                        indexes = GetSelectedIndexesFromListWidget(listWidget);
                    }
                }

                // If still no indexes, try to get from dialogTree as fallback
                if (indexes == null || indexes.Count == 0)
                {
                    if (_dialogTree != null)
                    {
                        indexes = OdyToolDLG.GetSelectedIndexesFromTreeView(_dialogTree);
                    }
                }
            }

            if (indexes == null || indexes.Count == 0)
            {
                BlinkWindow();
                return;
            }

            // When no installation is selected we need a TLK path from File → DLG Settings (Manual paths).
            string tlkPathOverride = null;
            string femaleTlkPathOverride = null;
            if (!UseInstallationForResources())
            {
                var dlgSettings = new DLGSettings();
                tlkPathOverride = dlgSettings.TlkPath("")?.Trim();
                if (string.IsNullOrWhiteSpace(tlkPathOverride))
                {
                    new RobustLogger().Error(Localization.Tr("Cannot edit text: set the TLK path in File → DLG Settings (Manual paths), or choose an installation that provides dialog.tlk."));
                    return;
                }
                femaleTlkPathOverride = dlgSettings.FemaleTlkPath("")?.Trim();
            }

            // Process each selected index
            foreach (var indexObj in indexes)
            {
                DLGStandardItem item = null;

                // Determine the model and item based on source widget
                if (sourceWidget is DLGListWidget listWidget)
                {
                    // Get item from list widget
                    if (indexObj is DLGListWidgetItem listItem)
                    {
                        // DLGListWidgetItem wraps a DLGLink, need to get the associated DLGStandardItem
                        // For now, we'll need to find the item from the link
                        DLGLink link = listItem.Link;
                        if (link != null && link.Node != null)
                        {
                            // Find the DLGStandardItem that corresponds to this link/node
                            item = FindItemForLink(link);
                        }
                    }
                    else if (indexObj is DLGStandardItem dlgItem)
                    {
                        item = dlgItem;
                    }
                }
                else if (sourceWidget == _dialogTree || sourceWidget == null)
                {
                    // Get item from tree view
                    if (indexObj is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                    {
                        item = dlgItem;
                    }
                    else if (indexObj is DLGStandardItem dlgItemDirect)
                    {
                        item = dlgItemDirect;
                    }
                    else if (_dialogTree?.SelectedItem is TreeViewItem selectedTreeItem && selectedTreeItem.Tag is DLGStandardItem selectedDlgItem)
                    {
                        item = selectedDlgItem;
                    }
                    else if (_dialogTree?.SelectedItem is DLGStandardItem selectedDlgItemDirect)
                    {
                        item = selectedDlgItemDirect;
                    }
                }
                else
                {
                    // Flat list or graph: indexes are DLGStandardItem from GetSelectedIndexesFromCurrentLink
                    item = indexObj as DLGStandardItem;
                }

                if (item == null)
                {
                    continue;
                }

                if (item.Link == null)
                {
                    continue;
                }

                try
                {
                    // Get parent window for dialog
                    Window parentWindow = this;
                    try
                    {
                        // Check if window is valid (not null, not being destroyed)
                        if (parentWindow != null)
                        {
                            // Try to access a property to ensure window is valid
                            var _ = parentWindow.IsVisible;
                            // If we get here, window is likely valid
                            if (!parentWindow.IsVisible || !parentWindow.IsEnabled)
                            {
                                // Use active window as fallback if parent is not in a good state
                                var activeWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                                    ? desktop.MainWindow
                                    : null;
                                if (activeWindow != null)
                                {
                                    parentWindow = activeWindow;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Window is being destroyed or invalid, use active window or this window as fallback
                        var activeWindow = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                            ? desktop.MainWindow
                            : null;
                        if (activeWindow != null)
                        {
                            parentWindow = activeWindow;
                        }
                    }

                    LocalizedStringDialog dialog = UseInstallationForResources()
                        ? new LocalizedStringDialog(parentWindow, _installation, item.Link.Node.Text)
                        : new LocalizedStringDialog(parentWindow, tlkPathOverride, femaleTlkPathOverride, item.Link.Node.Text);

                    bool dialogResult = false;
                    try
                    {
                        // In Avalonia, we use ShowDialogAsync to show the dialog modally
                        dialogResult = await dialog.ShowDialog<bool>(parentWindow);
                    }
                    catch (Exception exc)
                    {
                        new RobustLogger().Exception($"Error executing LocalizedStringDialog: {exc.GetType().Name}: {exc}", exc);
                        continue; // Continue to next item
                    }

                    if (!dialogResult)
                    {
                        // User cancelled the dialog
                        continue; // Continue to next item
                    }

                    // Access dialog.LocString before cleanup
                    item.Link.Node.Text = dialog.LocString;

                    if (item is DLGStandardItem standardItem)
                    {
                        _model.UpdateItemDisplayText(standardItem);
                        // Rebuild tree so the node header shows the new text (in-place header update may not refresh in Avalonia)
                        if (sourceWidget == _dialogTree || sourceWidget == null)
                        {
                            UpdateTreeView();
                            SelectTreeViewItem(standardItem);
                        }
                    }
                    else if (sourceWidget is DLGListWidget listWidgetForUpdate && indexObj is DLGListWidgetItem listWidgetItem)
                    {
                        listWidgetForUpdate.UpdateItem(listWidgetItem);
                    }
                    else if (sourceWidget == _dialogTree || sourceWidget == null)
                    {
                        SelectTreeViewItem(item);
                    }
                }
                catch (Exception exc)
                {
                    new RobustLogger().Exception($"Error creating LocalizedStringDialog: {exc.GetType().Name}: {exc}", exc);
                    continue; // Continue to next item
                }
            } // End of foreach loop
        }

        /// <summary>
        /// Gets selected indexes from a TreeView.
        /// Helper method to extract DLGStandardItem objects from TreeView selection.
        /// </summary>
        /// <summary>
        /// Gets the DLGStandardItem for a link (from tree item tag). Used when selection comes from flat list or graph.
        /// </summary>
        private DLGStandardItem GetStandardItemForLink(DLGLink link)
        {
            return FindItemForLink(link);
        }

        /// <summary>
        /// Gets selected indexes (one DLGStandardItem) from current _selectedLink for use when focus is on flat list or graph.
        /// </summary>
        private List<object> GetSelectedIndexesFromCurrentLink()
        {
            var list = new List<object>();
            var item = GetStandardItemForLink(_selectedLink);
            if (item != null) list.Add(item);
            return list;
        }

        /// <summary>
        /// Gets the selected DLGStandardItem from the dialog tree view.
        /// </summary>
        /// <returns>The selected DLGStandardItem, or null if no valid selection.</returns>
        private DLGStandardItem GetSelectedItemFromTreeView()
        {
            if (_dialogTree != null)
            {
                var selectedItem = _dialogTree.SelectedItem;
                if (selectedItem != null)
                {
                    if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                        return dlgItem;
                    if (selectedItem is DLGStandardItem dlgItemDirect)
                        return dlgItemDirect;
                }
            }
            // Fallback when selection came from flat list or graph
            return GetStandardItemForLink(_selectedLink);
        }

        private static List<object> GetSelectedIndexesFromTreeView(TreeView treeView)
        {
            var indexes = new List<object>();
            if (treeView == null)
            {
                return indexes;
            }

            // Get selected item from tree view
            var selectedItem = treeView.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                {
                    indexes.Add(dlgItem);
                }
                else if (selectedItem is DLGStandardItem dlgItemDirect)
                {
                    indexes.Add(dlgItemDirect);
                }
                else
                {
                    indexes.Add(selectedItem);
                }
            }

            return indexes;
        }

        /// <summary>
        /// Gets selected indexes from a DLGListWidget.
        /// Helper method to extract DLGStandardItem objects from DLGListWidget selection.
        /// </summary>
        private List<object> GetSelectedIndexesFromListWidget(DLGListWidget listWidget)
        {
            var indexes = new List<object>();
            if (listWidget == null)
            {
                return indexes;
            }

            // Get selected items from list widget
            // In Avalonia, we get selected items directly
            var selectedItems = listWidget.SelectedItems;
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    if (item is DLGListWidgetItem listItem)
                    {
                        indexes.Add(listItem);
                    }
                    else if (item is DLGStandardItem dlgItem)
                    {
                        indexes.Add(dlgItem);
                    }
                    else
                    {
                        indexes.Add(item);
                    }
                }
            }

            return indexes;
        }

        /// <summary>
        /// Focuses on the selected node.
        /// </summary>
        private void FocusOnSelectedNode()
        {
            if (_dialogTree?.SelectedItem == null)
            {
                return;
            }

            // Get selected item from tree
            DLGLink link = null;
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                link = dlgItem?.Link;
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                link = dlgItemDirect?.Link;
            }

            if (link != null)
            {
                FocusOnNode(link);
            }
        }

        /// <summary>
        /// Focuses the dialog tree on a specific link node.
        /// </summary>
        /// <param name="link">The link to focus on, or null to clear focus.</param>
        /// <returns>The focused item, or null if link is null.</returns>
        public DLGStandardItem FocusOnNode(DLGLink link)
        {
            if (link == null)
            {
                return null;
            }

            if (_dialogTree != null)
            {
                var settings = new GlobalSettings();
                if (settings.SelectedTheme.Contains("Light") || settings.SelectedTheme == "Native")
                {
                    // Set light yellow background (#FFFFEE) for focus mode
                    _dialogTree.Background = new SolidColorBrush(
                        Avalonia.Media.Color.FromRgb(0xFF, 0xFF, 0xEE));
                }
            }

            // Clear the model and set focus state
            _model.ResetModel();
            _focused = true;

            // Create item for the focused link
            var item = new DLGStandardItem(link);

            // Add the item to the model's root items directly
            // We need to access the private _rootItems, so we'll use InsertStarter which adds it
            // But we need to ensure the item we created is the one used
            _model.InsertStarter(0, link);

            // Get the item that was actually added to the model
            var rootItems = _model.GetRootItems();
            DLGStandardItem focusedItem = null;
            if (rootItems.Count > 0)
            {
                focusedItem = rootItems[0];
            }

            // Update the tree view
            UpdateTreeView();

            // Select the focused item in the tree
            if (_dialogTree != null && _dialogTree.ItemsSource != null && focusedItem != null)
            {
                var treeItems = _dialogTree.ItemsSource as System.Collections.IEnumerable;
                if (treeItems != null)
                {
                    foreach (TreeViewItem treeItem in treeItems)
                    {
                        if (treeItem.Tag == focusedItem)
                        {
                            _dialogTree.SelectedItem = treeItem;
                            break;
                        }
                    }
                }
            }

            return focusedItem ?? item;
        }

        /// <summary>
        /// Adds a child node to the given parent item and records the action for undo/redo. Used by context menu "Add Entry/Reply" and Insert key.
        /// </summary>
        private void AddChildToParentItem(DLGStandardItem parentItem)
        {
            if (parentItem == null || parentItem.Link == null)
                return;
            int childIndex = parentItem.Link.Node != null ? parentItem.Link.Node.Links.Count : -1;
            var action = new AddChildToItemAction(parentItem, childIndex);
            _actionHistory.Apply(action);
            if (action.ChildItem != null)
                SelectTreeViewItem(action.ChildItem);
        }

        /// <summary>
        /// Adds a child to the selected item (used by Insert key).
        /// </summary>
        private void AddChildToSelectedItem()
        {
            if (_dialogTree?.SelectedItem == null)
                return;
            DLGStandardItem selectedItem = null;
            var treeSelectedItem = _dialogTree.SelectedItem;
            if (treeSelectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                selectedItem = dlgItem;
            else if (treeSelectedItem is DLGStandardItem dlgItemDirect)
                selectedItem = dlgItemDirect;
            if (selectedItem == null || selectedItem.Link == null)
                return;
            AddChildToParentItem(selectedItem);
        }

        /// <summary>
        /// Blinks the window to indicate an error or invalid action.
        /// </summary>
        /// <param name="sound">Whether to play a sound effect when blinking. Defaults to true.</param>
        private void BlinkWindow(bool sound = true)
        {
            if (sound)
            {
                try
                {
                    PlaySound("dr_metal_lock", new[] { SearchLocation.SOUND, SearchLocation.VOICE });
                }
                catch
                {
                    // Suppress exceptions when playing sound fails
                }
            }

            double originalOpacity = Opacity;
            Opacity = 0.7;

            // Restore opacity after 125ms
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(125)
            };
            timer.Tick += (s, e) =>
            {
                Opacity = 1.0;
                timer.Stop();
            };
            timer.Start();
        }

        /// <summary>
        /// Plays a sound resource.
        /// </summary>
        /// <param name="resname">The resource name of the sound to play (without extension).</param>
        /// <param name="searchOrder">The ordered list of locations to search for the sound. If null, uses default order.</param>
        /// <returns>True if the sound was played successfully, false otherwise.</returns>
        private bool PlaySound(string resname, SearchLocation[] searchOrder = null)
        {
            if (string.IsNullOrWhiteSpace(resname) || _installation == null)
            {
                BlinkWindow(sound: false);
                return false;
            }

            _soundPlayer?.Stop();

            if (searchOrder == null || searchOrder.Length == 0)
            {
                searchOrder = new[]
                {
                    SearchLocation.MUSIC,
                    SearchLocation.VOICE,
                    SearchLocation.SOUND,
                    SearchLocation.OVERRIDE,
                    SearchLocation.CHITIN
                };
            }

            byte[] soundData = _installation.Sound(resname.Trim(), searchOrder);

            if (soundData == null || soundData.Length == 0)
            {
                BlinkWindow(sound: false);
                return false;
            }

            return PlayByteSourceMedia(soundData);
        }

        /// <summary>
        /// Plays audio from byte array data.
        /// </summary>
        /// <param name="data">The audio data bytes (WAV format).</param>
        /// <returns>True if playback started successfully, false otherwise.</returns>
        private bool PlayByteSourceMedia(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                BlinkWindow();
                return false;
            }

            try
            {
                // Stop any currently playing sound
                _soundPlayer?.Stop();

                // Set source from WAV bytes (cross-platform; NAudio owns the buffer)
                _soundPlayer.SetSourceFromBytes(data);
                _soundPlayer.Play();

                return true;
            }
            catch (Exception)
            {
                BlinkWindow();
                return false;
            }
        }

        /// <summary>
        /// Plays sound or blinks window.
        /// </summary>
        private void PlaySoundOrBlink()
        {
            // Get sound and voice resource names from UI combo boxes
            string soundResname = _soundComboBox?.Text?.Trim() ?? string.Empty;
            string voiceResname = _voiceComboBox?.Text?.Trim() ?? string.Empty;

            // If sound combo box has text, play sound with SOUND and VOICE search locations
            if (!string.IsNullOrEmpty(soundResname))
            {
                PlaySound(soundResname, new[] { SearchLocation.SOUND, SearchLocation.VOICE });
            }
            // Else if voice combo box has text, play voice with VOICE search location
            else if (!string.IsNullOrEmpty(voiceResname))
            {
                PlaySound(voiceResname, new[] { SearchLocation.VOICE });
            }
            // Else blink window to indicate no playable sound
            else
            {
                BlinkWindow();
            }
        }

        /// <summary>
        /// Cleans up resources when the window is closed.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            // Clean up sound player resources
            try
            {
                _soundPlayer?.Stop();
                _soundPlayer?.Dispose();
            }
            catch
            {
                // Ignore errors during cleanup
            }

            base.OnClosed(e);
        }

        /// <summary>
        /// Sets up the menu bar with File and Tools menus.
        /// </summary>
        private void SetupMenuBar(DockPanel dockPanel)
        {
            var menuBar = new Menu();
            dockPanel.Children.Add(menuBar);
            DockPanel.SetDock(menuBar, Dock.Top);

            // File menu
            var fileMenu = new MenuItem { Header = Localization.Tr("File"), Name = "menuFile" };
            menuBar.Items.Add(fileMenu);

            // File menu actions (Name set so base Editor can wire New/Open/Save/SaveAs/Revert/Exit)
            _actionNew = new MenuItem { Header = Localization.Tr("New"), Name = "actionNew" };
            _actionOpen = new MenuItem { Header = Localization.Tr("Open"), Name = "actionOpen" };
            _actionSave = new MenuItem { Header = Localization.Tr("Save"), Name = "actionSave" };
            _actionSaveAs = new MenuItem { Header = Localization.Tr("Save As"), Name = "actionSaveAs" };
            _actionRevert = new MenuItem { Header = Localization.Tr("Revert"), Name = "actionRevert" };
            _actionDLGSettings = new MenuItem { Header = Localization.Tr("DLG Settings..."), Name = "actionDLGSettings" };
            _actionExit = new MenuItem { Header = Localization.Tr("Exit"), Name = "actionExit" };

            fileMenu.Items.Add(_actionNew);
            fileMenu.Items.Add(_actionOpen);
            fileMenu.Items.Add(_actionSave);
            fileMenu.Items.Add(_actionSaveAs);
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(_actionRevert);
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(_actionDLGSettings);
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(_actionExit);

            // Edit menu (undo/redo)
            var editMenu = new MenuItem { Header = Localization.Tr("Edit"), Name = "menuEdit" };
            menuBar.Items.Add(editMenu);
            _actionUndo = new MenuItem { Header = Localization.Tr("Undo"), Name = "actionUndo" };
            _actionRedo = new MenuItem { Header = Localization.Tr("Redo"), Name = "actionRedo" };
            editMenu.Items.Add(_actionUndo);
            editMenu.Items.Add(_actionRedo);

            // Tools menu
            var toolsMenu = new MenuItem { Header = Localization.Tr("Tools"), Name = "menuTools" };
            menuBar.Items.Add(toolsMenu);

            // Tools menu actions
            _actionFind = new MenuItem { Header = Localization.Tr("Find"), Name = "actionFind" };
            _actionReloadTree = new MenuItem { Header = Localization.Tr("Reload Tree"), Name = "actionReloadTree" };
            _actionUnfocus = new MenuItem { Header = Localization.Tr("Unfocus Tree"), Name = "actionUnfocus" };
            _actionUnfocus.IsEnabled = false;

            toolsMenu.Items.Add(_actionFind);
            toolsMenu.Items.Add(_actionReloadTree);
            toolsMenu.Items.Add(_actionUnfocus);

            // Connect action events
            SetupMenuActionHandlers();
        }

        /// <summary>
        /// Sets up event handlers for menu actions. File New/Open/Save/SaveAs/Revert/Settings/Exit wired by base Editor.
        /// </summary>
        private void SetupMenuActionHandlers()
        {
            // Edit menu actions (undo/redo)
            if (_actionUndo != null) _actionUndo.Click += (s, e) => Undo();
            if (_actionRedo != null) _actionRedo.Click += (s, e) => Redo();
            RefreshEditMenuState();

            // Tools menu actions
            if (_actionFind != null) _actionFind.Click += (s, e) => ShowFindBar();
            _actionReloadTree.Click += (s, e) => ReloadTree();
            _actionUnfocus.Click += (s, e) => UnfocusTree();
        }

        /// <summary>Called when undo/redo stack changes so Edit menu items can update IsEnabled.</summary>
        internal void NotifyActionHistoryChanged()
        {
            RefreshEditMenuState();
        }

        private void RefreshEditMenuState()
        {
            if (_actionUndo != null) _actionUndo.IsEnabled = CanUndo;
            if (_actionRedo != null) _actionRedo.IsEnabled = CanRedo;
        }

        /// <summary>Name of the Settings menu action for base Editor to wire.</summary>
        protected override string SettingsMenuActionName => "actionDLGSettings";

        /// <summary>
        /// Opens the DLG Settings dialog (File -> DLG Settings). Persists installation vs manual paths and all manual path fields.
        /// </summary>
        protected override async Task ShowSettingsDialogAsync()
        {
            var dialog = new Dialogs.DLGSettingsDialog();
            var result = await dialog.ShowDialog<bool?>(this);
            if (result == true)
            {
                ApplyInstallationFromDLGSettings();
                SetupTslEmotionsAndExpressions();
                RefreshAnimList();
            }
        }

        /// <summary>
        /// Resolves installation from File → DLG Settings and sets _installation.
        /// When no installation is selected, sets _installation to null (manual paths only).
        /// When an installation is selected, creates OdyInstallation from GlobalSettings; if none is selected but the combo is "use installation", preserves the installation passed from the app (e.g. when opening DLG from main window).
        /// </summary>
        private void ApplyInstallationFromDLGSettings()
        {
            var dlgSettings = new DLGSettings();
            if (!dlgSettings.UseInstallation(true))
            {
                _installation = null;
                return;
            }
            string name = dlgSettings.SelectedInstallationName("")?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                // Preserve installation passed from constructor (e.g. main app active installation)
                // so 2DA/TLK lookup works without requiring user to open DLG Settings first.
                return;
            }
            try
            {
                var installations = new GlobalSettings().Installations();
                if (installations == null || !installations.ContainsKey(name))
                {
                    _installation = null;
                    return;
                }
                var installData = installations[name];
                string path = installData != null && installData.ContainsKey("path") ? installData["path"]?.ToString()?.Trim() : null;
                bool tsl = installData != null && installData.ContainsKey("tsl") && installData["tsl"] is bool tslVal && tslVal;
                if (string.IsNullOrEmpty(path) || !System.IO.Directory.Exists(path))
                {
                    _installation = null;
                    return;
                }
                _installation = new OdyInstallation(path, name, tsl);
            }
            catch
            {
                _installation = null;
            }
        }

        /// <summary>
        /// Reloads the dialog tree from the current core DLG.
        /// </summary>
        private void ReloadTree()
        {
            LoadDLG(_coreDlg);
        }

        /// <summary>
        /// Unfocuses the current tree selection.
        /// </summary>
        private void UnfocusTree()
        {
            // Clear selection in the dialog tree
            if (_dialogTree != null)
            {
                _dialogTree.SelectedItem = null;
            }
            _focused = false;
        }

        /// <summary>
        /// Sets up the go-to bar UI controls. Matching vendor: go_to_bar, goToInput, goToButton.
        /// </summary>
        private void SetupGoToBar()
        {
            _goToBar = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                IsVisible = false,
                Margin = new Thickness(2),
                Spacing = 4
            };
            _goToInput = new TextBox
            {
                Watermark = "Go to node ID...",
                MinWidth = 150
            };
            _goToInput.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Return) { HandleGoTo(); e.Handled = true; }
                else if (e.Key == Key.Escape) { HideGoToBar(); e.Handled = true; }
            };
            _goToButton = new Button { Content = "Go" };
            _goToButton.Click += (s, e) => HandleGoTo();
            _goToBar.Children.Add(_goToInput);
            _goToBar.Children.Add(_goToButton);
        }

        /// <summary>
        /// Sets up the find bar UI controls.
        /// </summary>
        private void SetupFindBar()
        {
            _findBar = new Panel
            {
                IsVisible = false
            };

            var findLayout = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal
            };
            _findBar.Children.Add(findLayout);

            _findInput = new TextBox
            {
                Watermark = "Find in dialog..."
            };
            _findInput.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Return) { HandleFind(); e.Handled = true; }
                else if (e.Key == Key.Escape) { HideFindBar(); e.Handled = true; }
            };
            findLayout.Children.Add(_findInput);

            _backButton = new Button
            {
                Content = "←"
            };
            _backButton.Click += (s, e) => HandleBack();
            findLayout.Children.Add(_backButton);

            _findButton = new Button
            {
                Content = "→"
            };
            _findButton.Click += (s, e) => HandleFind();
            findLayout.Children.Add(_findButton);

            _resultsLabel = new TextBlock
            {
                Text = "",
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0)
            };
            findLayout.Children.Add(_resultsLabel);

            SetupCompleter();
            _findSuggestionsPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 0) };
            _findBar.Children.Add(_findSuggestionsPanel);
            PopulateFindSuggestionChips();
        }

        /// <summary>
        /// Sets up the autocompleter for the find input.
        /// </summary>
        private void SetupCompleter()
        {
            if (_findInput == null)
            {
                return;
            }

            var tempEntry = new DLGEntry();
            var tempLink = new DLGLink(tempEntry);

            var entryAttributes = new HashSet<string>();
            var entryType = typeof(DLGEntry);
            foreach (var prop in entryType.GetProperties())
            {
                if (!prop.Name.StartsWith("_") && prop.CanRead)
                {
                    entryAttributes.Add(prop.Name);
                }
            }

            var linkAttributes = new HashSet<string>();
            var linkType = typeof(DLGLink);
            foreach (var prop in linkType.GetProperties())
            {
                if (!prop.Name.StartsWith("_") && prop.CanRead)
                {
                    var propType = prop.PropertyType;
                    if (propType != typeof(DLGEntry) && propType != typeof(DLGReply))
                    {
                        linkAttributes.Add(prop.Name);
                    }
                }
            }

            var suggestions = new List<string>();
            foreach (var attr in entryAttributes)
            {
                suggestions.Add($"{attr}:");
            }
            foreach (var attr in linkAttributes)
            {
                suggestions.Add($"{attr}:");
            }
            suggestions.Add("stringref:");
            suggestions.Add("strref:");
            suggestions.Add("AND");
            suggestions.Add("OR");
            _findSuggestions = suggestions;
        }

        /// <summary>
        /// Hides the find bar.
        /// </summary>
        public void HideFindBar()
        {
            if (_findBar != null)
            {
                _findBar.IsVisible = false;
            }
        }

        /// <summary>
        /// Populates the find suggestion chips (speaker:, listener:, strref:, AND, OR, etc.) so user can click to append to find input.
        /// </summary>
        private void PopulateFindSuggestionChips()
        {
            if (_findSuggestionsPanel == null || _findSuggestions == null || _findSuggestions.Count == 0)
            {
                return;
            }
            foreach (string suggestion in _findSuggestions)
            {
                var btn = new Button
                {
                    Content = suggestion,
                    Padding = new Thickness(6, 2),
                    Margin = new Thickness(0, 0, 4, 0)
                };
                string toAppend = suggestion;
                btn.Click += (s, e) =>
                {
                    if (_findInput != null)
                    {
                        string t = _findInput.Text ?? "";
                        _findInput.Text = string.IsNullOrEmpty(t) ? toAppend : t + " " + toAppend;
                        _findInput.Focus();
                    }
                };
                _findSuggestionsPanel.Children.Add(btn);
            }
        }

        /// <summary>
        /// Shows the find bar.
        /// </summary>
        public void ShowFindBar()
        {
            if (_findBar != null)
            {
                _findBar.IsVisible = true;
            }

            if (_findInput != null)
            {
                _findInput.Focus();
            }
        }

        /// <summary>Shows the go-to bar. Matching vendor: show_go_to_bar().</summary>
        public void ShowGoToBar()
        {
            if (_goToBar != null) _goToBar.IsVisible = true;
            if (_goToInput != null) _goToInput.Focus();
        }

        /// <summary>Hides the go-to bar.</summary>
        public void HideGoToBar()
        {
            if (_goToBar != null) _goToBar.IsVisible = false;
        }

        /// <summary>Sets up status bar tips. Matching vendor: status_bar, tip_label, status_bar_anim_timer (30s).</summary>
        private void SetupStatusBarTips()
        {
            if (_tipLabel == null)
            {
                if (EnsureBindingsFromXaml())
                {
                    _tipLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "tipLabel");
                }
                if (_tipLabel == null)
                {
                    var statusBar = new Border
                    {
                        BorderThickness = new Thickness(0, 1, 0, 0),
                        Background = new SolidColorBrush(Avalonia.Media.Color.Parse("#E8EAED")),
                        Padding = new Thickness(6, 4),
                        Height = 24,
                        Child = _tipLabel = new TextBlock { VerticalAlignment = VerticalAlignment.Center, FontSize = 11 }
                    };
                    DockPanel.SetDock(statusBar, Dock.Bottom);
                    var content = Content as ContentControl;
                    var dock = content?.Content as DockPanel ?? Content as DockPanel;
                    if (dock != null)
                        dock.Children.Add(statusBar);
                }
            }
            if (_tipLabel != null)
            {
                ToolTip.SetTip(_tipLabel, Localization.Tr("Double-click to view all tips."));
                _tipLabel.DoubleTapped += (s, e) => ShowAllTipsDialog();
                ShowScrollingTip();
                _statusBarTipTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(30) };
                _statusBarTipTimer.Tick += (s, e) => ShowScrollingTip();
                _statusBarTipTimer.Start();
            }
        }

        /// <summary>Shows a random tip. Matching vendor: show_scrolling_tip().</summary>
        private void ShowScrollingTip()
        {
            if (_tipLabel != null && _tips != null && _tips.Length > 0)
            {
                var r = new Random();
                _tipLabel.Text = Localization.Tr(_tips[r.Next(_tips.Length)]);
            }
        }

        /// <summary>Shows dialog with all tips. Matching vendor: show_all_tips(). Theme matches OdyToolDLG.axaml (light).</summary>
        private void ShowAllTipsDialog()
        {
            if (_tips == null || _tips.Length == 0) return;
            var bgBrush = new SolidColorBrush(Avalonia.Media.Color.Parse("#F3F5F9"));
            var fgBrush = new SolidColorBrush(Avalonia.Media.Color.Parse("#202124"));
            var content = new TextBlock
            {
                Text = string.Join("\n• ", _tips),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(12),
                FontSize = 12,
                Foreground = fgBrush
            };
            var dlg = new Window
            {
                Title = Localization.Tr("All Tips"),
                Width = 700,
                Height = 400,
                RequestedThemeVariant = Avalonia.Styling.ThemeVariant.Light,
                Background = bgBrush,
                Content = new ScrollViewer
                {
                    Content = content,
                    HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                    VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                    Background = bgBrush
                }
            };
            dlg.Show(this);
        }

        /// <summary>Handles go-to: jumps to node by NodeID. Matching vendor: handle_go_to().</summary>
        public void HandleGoTo()
        {
            string inputText = _goToInput?.Text?.Trim() ?? "";
            CustomGoToFunction(inputText);
            HideGoToBar();
        }

        /// <summary>Jumps to the node with the given NodeID. Vendor: custom_go_to_function.</summary>
        private void CustomGoToFunction(string inputText)
        {
            if (string.IsNullOrEmpty(inputText) || _coreDlg == null || _model == null) return;
            if (!int.TryParse(inputText, out int nodeId)) return;
            DLGNode targetNode = null;
            foreach (var entry in _coreDlg.EntryList)
            {
                if (entry.NodeId == nodeId) { targetNode = entry; break; }
            }
            if (targetNode == null)
            {
                foreach (var reply in _coreDlg.ReplyList)
                {
                    if (reply.NodeId == nodeId) { targetNode = reply; break; }
                }
            }
            if (targetNode != null && _model.NodeToItems.TryGetValue(targetNode, out var items) && items.Count > 0)
            {
                SelectTreeItem(items[0]);
            }
        }

        /// <summary>
        /// Handles the find button click or Enter key press in the find input.
        /// </summary>
        public void HandleFind()
        {
            if (_findInput == null)
            {
                return;
            }

            string inputText = _findInput.Text ?? "";

            if (_searchResults == null || _searchResults.Count == 0 || inputText != _currentSearchText)
            {
                _searchResults = FindItemMatchingDisplayText(inputText);
                _currentSearchText = inputText;
                _currentResultIndex = 0;
            }

            if (_searchResults == null || _searchResults.Count == 0)
            {
                if (_resultsLabel != null)
                {
                    _resultsLabel.Text = Localization.Tr("No results found");
                }
                return;
            }

            _currentResultIndex = (_currentResultIndex + 1) % _searchResults.Count;
            HighlightResult(_searchResults[_currentResultIndex]);
            UpdateResultsLabel();
        }

        /// <summary>
        /// Handles the back button click to navigate to previous search result.
        /// </summary>
        private void HandleBack()
        {
            if (_searchResults == null || _searchResults.Count == 0)
            {
                return;
            }

            _currentResultIndex = (_currentResultIndex - 1 + _searchResults.Count) % _searchResults.Count;

            HighlightResult(_searchResults[_currentResultIndex]);

            UpdateResultsLabel();
        }

        /// <summary>
        /// Parses a search query string into conditions with operators.
        /// Supports attribute searches (e.g., "speaker:TestSpeaker"), text searches, and AND/OR operators.
        /// </summary>
        private List<Tuple<string, string, string>> ParseQuery(string inputText)
        {
            var conditions = new List<Tuple<string, string, string>>();

            if (string.IsNullOrEmpty(inputText))
            {
                return conditions;
            }

            // Pattern to match quoted strings or whitespace-separated tokens
            var quotedStringPattern = new Regex(@"""[^""]*""");
            var tokens = new List<string>();

            // Extract quoted strings first
            var quotedMatches = quotedStringPattern.Matches(inputText);
            int lastIndex = 0;
            foreach (Match match in quotedMatches)
            {
                // Add text before the quoted string
                if (match.Index > lastIndex)
                {
                    string before = inputText.Substring(lastIndex, match.Index - lastIndex);
                    var beforeTokens = Regex.Split(before, @"\s+");
                    foreach (var token in beforeTokens)
                    {
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            tokens.Add(token);
                        }
                    }
                }

                // Add the quoted string (without quotes)
                tokens.Add(match.Value.Substring(1, match.Value.Length - 2));
                lastIndex = match.Index + match.Length;
            }

            // Add remaining text after last quoted string
            if (lastIndex < inputText.Length)
            {
                string remaining = inputText.Substring(lastIndex);
                var remainingTokens = Regex.Split(remaining, @"\s+");
                foreach (var token in remainingTokens)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokens.Add(token);
                    }
                }
            }

            string logicalOperator = null;
            int i = 0;
            while (i < tokens.Count)
            {
                string token = tokens[i].ToUpperInvariant();
                if (token == "AND" || token == "OR")
                {
                    logicalOperator = token;
                    i++;
                    continue;
                }

                int? nextIndex = i + 1 < tokens.Count ? (int?)(i + 1) : null;
                if (tokens[i].Contains(":"))
                {
                    // Attribute search: "key:value"
                    var parts = tokens[i].Split(new[] { ':' }, 2);
                    string key = parts[0].Trim().ToLowerInvariant();
                    string value = parts.Length > 1 ? parts[1].Trim().ToLowerInvariant() : null;
                    conditions.Add(Tuple.Create(key, value ?? "", logicalOperator));
                    logicalOperator = null;
                }
                else if (nextIndex.HasValue && (tokens[nextIndex.Value].Equals("AND", StringComparison.InvariantCultureIgnoreCase) || tokens[nextIndex.Value].ToUpperInvariant() == "OR"))
                {
                    // Text search with operator
                    conditions.Add(Tuple.Create(tokens[i], "", logicalOperator));
                    logicalOperator = null;
                }
                else if (!nextIndex.HasValue)
                {
                    // Last token
                    conditions.Add(Tuple.Create(tokens[i], "", logicalOperator));
                    logicalOperator = null;
                }
                else
                {
                    // Text search without operator
                    conditions.Add(Tuple.Create(tokens[i], "", logicalOperator));
                    logicalOperator = null;
                }

                i++;
            }

            return conditions;
        }

        /// <summary>
        /// Finds all items matching the search text with full query parsing and attribute search support.
        /// Supports attribute searches (e.g., "speaker:TestSpeaker"), text searches, and AND/OR operators.
        /// </summary>
        private List<DLGStandardItem> FindItemMatchingDisplayText(string inputText)
        {
            var matchingItems = new List<DLGStandardItem>();

            if (string.IsNullOrEmpty(inputText))
            {
                return matchingItems;
            }

            // Parse query into conditions
            var conditions = ParseQuery(inputText);
            string searchTextLower = inputText.ToLowerInvariant();

            // Helper to check if a condition matches an item
            bool ConditionMatches(string key, string value, string op, DLGStandardItem item)
            {
                if (item?.Link?.Node == null)
                {
                    return false;
                }

                var link = item.Link;
                var node = item.Link.Node;
                object sentinel = new object();

                // Get attribute value from link or node using reflection
                object linkValue = GetAttributeValue(link, key, sentinel);
                object nodeValue = GetAttributeValue(node, key, sentinel);

                // Helper to check value match
                bool CheckValue(object attrValue, string searchValue)
                {
                    if (ReferenceEquals(attrValue, sentinel))
                    {
                        return false;
                    }

                    // Truthiness check (value is None or empty)
                    if (string.IsNullOrEmpty(searchValue))
                    {
                        if (attrValue is bool boolVal)
                        {
                            return boolVal;
                        }
                        if (attrValue is int intVal)
                        {
                            // C# warning CS0652: The constant 0xFFFFFFFF is outside the range of 'int'
                            // Since 0xFFFFFFFF as uint is -1 as int, just compare 0 and -1
                            return intVal != 0 && intVal != -1;
                        }
                        return attrValue != null;
                    }

                    // Type-specific matching
                    if (attrValue is int intAttr)
                    {
                        if (int.TryParse(searchValue, out int searchInt))
                        {
                            return intAttr == searchInt;
                        }
                        return false;
                    }

                    if (attrValue is bool boolAttr)
                    {
                        if (searchValue == "true" || searchValue == "1")
                        {
                            return boolAttr == true;
                        }
                        if (searchValue == "false" || searchValue == "0")
                        {
                            return boolAttr == false;
                        }
                        return false;
                    }

                    // String/substring matching
                    string attrStr = attrValue?.ToString() ?? "";
                    return attrStr.ToLowerInvariant().Contains(searchValue.ToLowerInvariant());
                }

                // Check link or node value
                if (CheckValue(linkValue, value) || CheckValue(nodeValue, value))
                {
                    return true;
                }

                // Special handling for strref/stringref (match by StringRef or substring IDs)
                if (key == "strref" || key == "stringref")
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        if (int.TryParse(value, out int strref))
                        {
                            if (node.Text != null)
                            {
                                if (node.Text.StringRef == strref)
                                    return true;
                                try
                                {
                                    var dict = node.Text.ToDictionary();
                                    if (dict != null && dict.TryGetValue("substrings", out object subs) && subs is Dictionary<int, string> subDict)
                                        return subDict.ContainsKey(strref);
                                }
                                catch { /* fallback: no substring match */ }
                                return false;
                            }
                        }
                        return false;
                    }
                    return node.Text != null && !string.IsNullOrEmpty(GetResolvedNodeText(node));
                }

                return false;
            }

            // Helper to evaluate all conditions for an item
            bool EvaluateConditions(DLGStandardItem item)
            {
                // Always check text match first
                string itemText = GetItemDisplayText(item).ToLowerInvariant();
                if (itemText.Contains(searchTextLower))
                {
                    return true;
                }

                // If no conditions, return false (text didn't match)
                if (conditions.Count == 0)
                {
                    return false;
                }

                // Evaluate conditions with AND/OR logic
                bool result = conditions.Count == 0;
                foreach (var condition in conditions)
                {
                    string key = condition.Item1;
                    string value = condition.Item2;
                    string op = condition.Item3;

                    bool matches = ConditionMatches(key, value, op, item);
                    if (op == "AND")
                    {
                        result = result && matches;
                    }
                    else if (op == "OR")
                    {
                        result = result || matches;
                    }
                    else
                    {
                        // First condition or condition without operator - set result directly
                        result = matches;
                    }
                }

                return result;
            }

            // Recursive search function
            void SearchItem(DLGStandardItem item)
            {
                if (item == null)
                {
                    return;
                }

                if (EvaluateConditions(item))
                {
                    matchingItems.Add(item);
                }

                // Search children
                foreach (var child in item.Children)
                {
                    SearchItem(child);
                }
            }

            // Search all root items
            var rootItems = _model.GetRootItems();
            foreach (var rootItem in rootItems)
            {
                SearchItem(rootItem);
            }

            return new List<DLGStandardItem>(new HashSet<DLGStandardItem>(matchingItems));
        }

        // Helper method to get attribute value using reflection
        // Handles case-insensitive property/field lookup and converts ResRef to string
        private object GetAttributeValue(object obj, string key, object sentinel)
        {
            if (obj == null || string.IsNullOrEmpty(key))
            {
                return sentinel;
            }

            try
            {
                // Convert snake_case to PascalCase for C# property names
                // e.g., "speaker" -> "Speaker", "is_child" -> "IsChild", "active1" -> "Active1"
                string pascalKey = ToPascalCase(key);

                // Try property first (case-insensitive)
                var properties = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var property in properties)
                {
                    if (property.Name.Equals(pascalKey, StringComparison.OrdinalIgnoreCase) ||
                        property.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        object value = property.GetValue(obj);

                        // Convert ResRef to string for comparison
                        if (value != null && value.GetType().Name == "ResRef")
                        {
                            return value.ToString();
                        }

                        return value;
                    }
                }

                // Try fields if property not found
                var fields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.Name.Equals(pascalKey, StringComparison.OrdinalIgnoreCase) ||
                        field.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    {
                        object value = field.GetValue(obj);

                        // Convert ResRef to string for comparison
                        if (value != null && value.GetType().Name == "ResRef")
                        {
                            return value.ToString();
                        }

                        return value;
                    }
                }

                return sentinel;
            }
            catch
            {
                return sentinel;
            }
        }

        // Helper to convert snake_case to PascalCase
        // e.g., "speaker" -> "Speaker", "is_child" -> "IsChild", "active1_param1" -> "Active1Param1"
        private string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Handle snake_case
            if (input.Contains("_"))
            {
                var parts = input.Split('_');
                var result = new System.Text.StringBuilder();
                foreach (var part in parts)
                {
                    if (part.Length > 0)
                    {
                        result.Append(char.ToUpperInvariant(part[0]));
                        if (part.Length > 1)
                        {
                            result.Append(part.Substring(1));
                        }
                    }
                }
                return result.ToString();
            }

            // Simple camelCase/PascalCase conversion
            if (input.Length > 0)
            {
                return char.ToUpperInvariant(input[0]) + (input.Length > 1 ? input.Substring(1) : "");
            }

            return input;
        }

        /// <summary>
        /// Highlights and scrolls to the specified search result item.
        /// </summary>
        private void HighlightResult(DLGStandardItem item)
        {
            if (item == null || _dialogTree == null)
            {
                return;
            }

            // Expand all parents to make the item visible
            ExpandParents(item);

            // Select the item in the tree
            _dialogTree.SelectedItem = item;
            _dialogTree.Focus();

            // Scroll to the item (Avalonia TreeView handles this automatically when selecting)
            // Note: Avalonia doesn't have explicit scrollTo, but selection should scroll into view
        }

        // Helper method to expand all parent items
        private void ExpandParents(DLGStandardItem item)
        {
            if (item == null || _dialogTree == null)
            {
                return;
            }

            // Find the TreeViewItem corresponding to this DLGStandardItem
            TreeViewItem treeItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, item);
            if (treeItem != null)
            {
                // Expand all parent items in the visual tree to ensure visibility
                ExpandParentItems(treeItem);
            }
        }


        /// <summary>
        /// Updates the results label to show current position in search results.
        /// </summary>
        private void UpdateResultsLabel()
        {
            if (_resultsLabel == null)
            {
                return;
            }

            if (_searchResults == null || _searchResults.Count == 0)
            {
                _resultsLabel.Text = "";
            }
            else
            {
                _resultsLabel.Text = $"{_currentResultIndex + 1} / {_searchResults.Count}";
            }
        }

        /// <summary>
        /// Copies the path of the selected node.
        /// </summary>
        private async void CopyPath()
        {
            DLGStandardItem selectedItem = null;
            var treeSelectedItem = _dialogTree?.SelectedItem;
            if (treeSelectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                selectedItem = dlgItem;
            }
            else if (treeSelectedItem is DLGStandardItem dlgItemDirect)
            {
                selectedItem = dlgItemDirect;
            }

            if (selectedItem?.Link?.Node != null)
            {
                await CopyPath(selectedItem.Link.Node);
            }
        }

        /// <summary>
        /// Copies GFF path(s) for the given node to clipboard. Used from context menu when node is known (e.g. list widget).
        /// </summary>
        private async Task CopyPath(DLGNode targetNode)
        {
            if (targetNode == null)
            {
                return;
            }

            // Find all paths to the target node
            List<string> paths;
            try
            {
                paths = _coreDlg.FindPaths(targetNode);
            }
            catch (Exception ex)
            {
                new RobustLogger().Error($"Failed to find paths for node: {ex.Message}", true, ex);
                BlinkWindow();
                return;
            }

            if (paths == null || paths.Count == 0)
            {
                new RobustLogger().Error("No paths available.");
                BlinkWindow();
                return;
            }

            // Format the path(s) for clipboard
            string pathText;
            if (paths.Count == 1)
            {
                pathText = paths[0];
            }
            else
            {
                // Format multiple paths as numbered list
                var pathLines = new List<string>();
                for (int i = 0; i < paths.Count; i++)
                {
                    pathLines.Add($"  {i + 1}. {paths[i]}");
                }
                pathText = string.Join("\n", pathLines);
            }

            // Copy to clipboard
            // Note: PyKotor doesn't catch clipboard errors, but in C# we should handle them gracefully
            // Matching OdyToolTPC pattern: Show error message when clipboard copy fails
            try
            {
                var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
                if (topLevel?.Clipboard != null)
                {
                    await topLevel.Clipboard.SetTextAsync(pathText);
                }
                else
                {
                    // Clipboard not available - show error message
                    // Matching OdyToolTPC pattern: QMessageBox.critical when clipboard is unavailable
                    var msgBox = MessageBoxManager.GetMessageBoxStandard(
                        Localization.Tr("Copy Failed"),
                        Localization.Tr("Clipboard is not available. Unable to copy path to clipboard."),
                        ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    await msgBox.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Matching OdyToolTPC pattern: QMessageBox.critical when clipboard copy fails
                // Show error message to user when clipboard operation fails
                var msgBox = MessageBoxManager.GetMessageBoxStandard(
                    Localization.Tr("Copy Failed"),
                    string.Format(Localization.Tr("Failed to copy path to clipboard:\n{0}"), ex.Message),
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await msgBox.ShowAsync();
            }
        }

        /// <summary>
        /// Copies the link and node.
        /// </summary>
        private async void CopyLinkAndNode()
        {
            if (_model.SelectedIndex >= 0 && _model.SelectedIndex < _model.RowCount)
            {
                DLGLink link = _model.GetStarterAt(_model.SelectedIndex);
                if (link != null)
                {
                    // Note: CopyLinkAndNode on the model will also set _copy via SetCopyLink
                    await _model.CopyLinkAndNode(link, this);
                }
            }
        }

        /// <summary>
        /// Returns how many tree items reference the given node (1 = only one reference, &gt;1 = "copy").
        /// </summary>
        private int CountItemRefs(DLGNode node)
        {
            if (node == null || _model?.NodeToItems == null)
            {
                return 0;
            }
            return _model.NodeToItems.TryGetValue(node, out var list) ? list.Count : 0;
        }

        /// <summary>
        /// Returns true if the given link is currently in the pinned items list.
        /// </summary>
        private bool IsPinned(DLGLink link)
        {
            if (link == null || _pinnedItemsList == null)
            {
                return false;
            }
            for (int i = 0; i < _pinnedItemsList.Count; i++)
            {
                var listItem = _pinnedItemsList.GetItem(i);
                if (listItem?.Link == link)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// </summary>
        private void PinItem(DLGLink link)
        {
            if (link == null || _pinnedItemsList == null || IsPinned(link))
            {
                return;
            }
            var listItem = new DLGListWidgetItem(link);
            _pinnedItemsList.AddItem(listItem);
            _pinnedItemsList.UpdateItem(listItem);
        }

        /// <summary>
        /// Removes the given link from the pinned items list.
        /// </summary>
        private void UnpinItem(DLGLink link)
        {
            if (link == null || _pinnedItemsList == null)
            {
                return;
            }
            for (int i = _pinnedItemsList.Count - 1; i >= 0; i--)
            {
                var listItem = _pinnedItemsList.GetItem(i);
                if (listItem?.Link == link)
                {
                    _pinnedItemsList.RemoveItem(listItem);
                    return;
                }
            }
        }

        /// <summary>
        /// Finds a DLGLink in the current dialog by its hash (from MIME serialization key "link-{hash}").
        /// Used when dropping dragged tree item onto pinned list to resolve the link instance.
        /// </summary>
        private DLGLink FindLinkByHashInDlg(int linkHash)
        {
            if (_coreDlg == null)
            {
                return null;
            }
            var seen = new HashSet<DLGLink>();
            var queue = new Queue<DLGLink>();
            foreach (var s in _coreDlg.Starters)
            {
                if (s != null && !seen.Contains(s))
                {
                    seen.Add(s);
                    queue.Enqueue(s);
                }
            }
            while (queue.Count > 0)
            {
                var link = queue.Dequeue();
                if (link.GetHashCode() == linkHash)
                {
                    return link;
                }
                if (link?.Node?.Links != null)
                {
                    foreach (var child in link.Node.Links)
                    {
                        if (child != null && !seen.Contains(child))
                        {
                            seen.Add(child);
                            queue.Enqueue(child);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Dropped data (from tree drag or clipboard-style MIME) is parsed and the link is pinned.
        /// </summary>
        private void SetupPinnedListDragDrop()
        {
            if (_pinnedItemsList == null)
            {
                return;
            }
            DragDrop.SetAllowDrop(_pinnedItemsList, true);
            _pinnedItemsList.AddHandler(DragDrop.DropEvent, OnPinnedListDrop);
            _pinnedItemsList.AddHandler(DragDrop.DragOverEvent, OnPinnedListDragOver);
        }

        private void OnPinnedListDragOver(object sender, DragEventArgs e)
        {
            if (e.DataTransfer != null && e.DataTransfer.Contains(DlgMimeDataFormat))
            {
                e.DragEffects = DragDropEffects.Copy | DragDropEffects.Move;
            }
        }

        private void OnPinnedListDrop(object sender, DragEventArgs e)
        {
            if (e.DataTransfer == null || !e.DataTransfer.Contains(DlgMimeDataFormat))
            {
                return;
            }
            string json = null;
            try
            {
                json = e.DataTransfer.TryGetValue(DlgMimeDataFormat);
            }
            catch
            {
                // ignore
            }
            if (string.IsNullOrEmpty(json) || _model == null)
            {
                return;
            }
            var itemDataList = _model.ParseMimeData(json);
            if (itemDataList == null || itemDataList.Count == 0)
            {
                return;
            }
            var first = itemDataList[0];
            if (!(first.TryGetValue("roles", out object rolesObj) && rolesObj is Dictionary<string, object> roles))
            {
                return;
            }
            if (!roles.TryGetValue("261", out object linkJsonObj))
            {
                return;
            }
            string linkJson = linkJsonObj?.ToString();
            if (string.IsNullOrEmpty(linkJson))
            {
                return;
            }
            try
            {
                using (var doc = JsonDocument.Parse(linkJson))
                {
                    var root = doc.RootElement;
                    if (!root.TryGetProperty("key", out var keyEl))
                    {
                        return;
                    }
                    string key = keyEl.GetString();
                    if (string.IsNullOrEmpty(key) || !key.StartsWith("link-", StringComparison.Ordinal))
                    {
                        return;
                    }
                    string hashStr = key.Substring(5);
                    if (!int.TryParse(hashStr, out int linkHash))
                    {
                        return;
                    }
                    var link = FindLinkByHashInDlg(linkHash);
                    if (link != null)
                    {
                        PinItem(link);
                    }
                }
            }
            catch
            {
                // ignore parse errors
            }
        }

        /// <summary>
        /// </summary>
        private void SetupTreeViewDragSource()
        {
            if (_dialogTree == null)
            {
                return;
            }
            _dialogTree.AddHandler(PointerPressedEvent, OnTreeViewPointerPressed, Avalonia.Interactivity.RoutingStrategies.Tunnel);
            _dialogTree.AddHandler(PointerMovedEvent, OnTreeViewPointerMoved, Avalonia.Interactivity.RoutingStrategies.Tunnel);
            _dialogTree.AddHandler(PointerReleasedEvent, OnTreeViewPointerReleased, Avalonia.Interactivity.RoutingStrategies.Tunnel);
            SetupTreeViewDropTarget();
        }

        /// <summary>Vendor: DropPosition - above, below, or on top of target.</summary>
        private enum DropPosition { Above, Below, OnTopOf, Invalid }

        /// <summary>Vendor: DropTarget.determine_drop_target + is_valid_drop.</summary>
        private (TreeViewItem targetItem, DropPosition position, int row, DLGStandardItem parentItem) DetermineDropTarget(Point pos)
        {
            // Smaller leniency = larger OnTopOf zone (drop as child). 0.1 = top/bottom 10% for Above/Below, middle 80% for OnTopOf.
            // Cross-type drops (Reply into Entry, Entry into Reply) require OnTopOf; same-type reorder uses Above/Below.
            const double leniency = 0.1;
            var hit = FindTreeViewItemAtPoint(_dialogTree.ItemsSource as System.Collections.IEnumerable, pos);
            if (hit.targetItem == null || hit.dlgItem == null)
            {
                return (null, DropPosition.Invalid, -1, null);
            }
            var treeItem = hit.targetItem;
            var dlgItem = hit.dlgItem;
            double relY = hit.relativeY;
            double itemHeight = hit.itemHeight;
            double leniencyHeight = itemHeight * leniency;
            double upperThreshold = leniencyHeight;
            double lowerThreshold = itemHeight - leniencyHeight;

            var parentTreeItem = treeItem.Parent as TreeViewItem;
            var parentItems = parentTreeItem?.ItemsSource as System.Collections.IEnumerable ?? _dialogTree?.ItemsSource as System.Collections.IEnumerable;
            var parentDlg = parentTreeItem?.Tag as DLGStandardItem;
            int row = GetRowIndex(parentItems, treeItem);

            if (relY <= upperThreshold)
            {
                return (treeItem, DropPosition.Above, row, parentDlg);
            }
            if (relY >= lowerThreshold)
            {
                return (treeItem, DropPosition.Below, row + 1, parentDlg);
            }
            return (treeItem, DropPosition.OnTopOf, 0, dlgItem);
        }

        private (TreeViewItem targetItem, DLGStandardItem dlgItem, double relativeY, double itemHeight) FindTreeViewItemAtPoint(System.Collections.IEnumerable items, Point pointRelativeToTree)
        {
            if (items == null || _dialogTree == null) return (null, null, 0, 0);
            TreeViewItem deepest = null;
            DLGStandardItem deepestDlg = null;
            double deepestRelY = 0;
            double deepestHeight = 0;

            void Search(System.Collections.IEnumerable list)
            {
                if (list == null) return;
                foreach (TreeViewItem child in list)
                {
                    if (child == null) continue;
                    var topLeft = child.TranslatePoint(new Point(0, 0), _dialogTree);
                    if (topLeft == null) continue;
                    double w = child.Bounds.Width;
                    double h = Math.Max(1, child.Bounds.Height);
                    var rect = new Rect(topLeft.Value.X, topLeft.Value.Y, w, h);
                    if (!rect.Contains(pointRelativeToTree)) continue;

                    double relY = pointRelativeToTree.Y - topLeft.Value.Y;
                    var saved = deepest;
                    Search(child.ItemsSource as System.Collections.IEnumerable);
                    if (deepest == saved && child.Tag is DLGStandardItem dlg)
                    {
                        deepest = child;
                        deepestDlg = dlg;
                        deepestRelY = relY;
                        deepestHeight = h;
                    }
                    return;
                }
            }
            Search(items);
            return (deepest, deepestDlg, deepestRelY, deepestHeight);
        }

        private int GetRowIndex(System.Collections.IEnumerable items, TreeViewItem target)
        {
            if (items == null || target == null) return -1;
            int i = 0;
            foreach (TreeViewItem t in items)
            {
                if (t == target) return i;
                i++;
            }
            return -1;
        }

        private bool IsValidDrop(DLGLink draggedLink, DropPosition position, DLGStandardItem targetParentOrItem)
        {
            if (draggedLink?.Node == null || targetParentOrItem?.Link?.Node == null) return false;
            var draggedNode = draggedLink.Node;
            var targetNode = targetParentOrItem.Link.Node;
            bool sameType = (draggedNode is DLGReply && targetNode is DLGReply) || (draggedNode is DLGEntry && targetNode is DLGEntry);
            if (position == DropPosition.OnTopOf)
            {
                return !sameType; // Drop as child: Reply into Entry or Entry into Reply
            }
            if (!sameType)
            {
                return true; // Above/Below with different types: treat as drop-as-child (first/last)
            }
            return true; // Same type: reorder between siblings
        }

        private void SetupTreeViewDropTarget()
        {
            if (_dialogTree == null) return;
            DragDrop.SetAllowDrop(_dialogTree, true);
            _dialogTree.AddHandler(DragDrop.DragOverEvent, OnTreeViewDragOver, Avalonia.Interactivity.RoutingStrategies.Bubble);
            _dialogTree.AddHandler(DragDrop.DropEvent, OnTreeViewDrop, Avalonia.Interactivity.RoutingStrategies.Bubble);
        }

        private void OnTreeViewDragOver(object sender, DragEventArgs e)
        {
            if (e.DataTransfer == null || !e.DataTransfer.Contains(DlgMimeDataFormat) || _model == null)
            {
                e.DragEffects = DragDropEffects.None;
                return;
            }
            string json = null;
            try { json = e.DataTransfer.TryGetValue(DlgMimeDataFormat); } catch { }
            if (string.IsNullOrEmpty(json))
            {
                e.DragEffects = DragDropEffects.None;
                return;
            }
            var pos = e.GetPosition(_dialogTree);
            var (targetItem, position, row, parentItem) = DetermineDropTarget(pos);
            if (position == DropPosition.Invalid || targetItem?.Tag == null)
            {
                e.DragEffects = DragDropEffects.None;
                return;
            }
            DLGLink draggedLink = null;
            try
            {
                var parsed = _model.ParseMimeData(json);
                if (parsed != null && parsed.Count > 0 && parsed[0].TryGetValue("roles", out object ro) && ro is Dictionary<string, object> roles)
                {
                    if (roles.TryGetValue("261", out object linkJson) && linkJson != null)
                    {
                        var linkEl = JsonDocument.Parse(linkJson.ToString()).RootElement;
                        if (linkEl.TryGetProperty("key", out var k) && k.GetString()?.StartsWith("link-") == true)
                        {
                            var hashStr = k.GetString().Substring(5);
                            if (int.TryParse(hashStr, out int hash))
                            {
                                draggedLink = FindLinkByHashInDlg(hash);
                            }
                        }
                    }
                }
            }
            catch { }
            if (draggedLink == null)
            {
                e.DragEffects = DragDropEffects.None;
                return;
            }
            DLGStandardItem validationTarget = position == DropPosition.OnTopOf ? parentItem : targetItem?.Tag as DLGStandardItem;
            if (!IsValidDrop(draggedLink, position, validationTarget))
            {
                e.DragEffects = DragDropEffects.None;
                return;
            }
            e.DragEffects = DragDropEffects.Move | DragDropEffects.Copy;
        }

        private void OnTreeViewDrop(object sender, DragEventArgs e)
        {
            if (e.DataTransfer == null || !e.DataTransfer.Contains(DlgMimeDataFormat) || _model == null)
                return;
            string json = null;
            try { json = e.DataTransfer.TryGetValue(DlgMimeDataFormat); } catch { }
            if (string.IsNullOrEmpty(json)) return;
            var pos = e.GetPosition(_dialogTree);
            var (targetItem, position, row, parentItem) = DetermineDropTarget(pos);
            if (position == DropPosition.Invalid || targetItem?.Tag == null) return;

            DLGLink draggedLink = null;
            DLGStandardItem draggedItem = null;
            try
            {
                var parsed = _model.ParseMimeData(json);
                if (parsed != null && parsed.Count > 0 && parsed[0].TryGetValue("roles", out object ro) && ro is Dictionary<string, object> roles)
                {
                    if (roles.TryGetValue("261", out object linkJson) && linkJson != null)
                    {
                        var linkEl = JsonDocument.Parse(linkJson.ToString()).RootElement;
                        if (linkEl.TryGetProperty("key", out var k) && k.GetString()?.StartsWith("link-") == true)
                        {
                            var hashStr = k.GetString().Substring(5);
                            if (int.TryParse(hashStr, out int hash))
                            {
                                draggedLink = FindLinkByHashInDlg(hash);
                            }
                        }
                    }
                }
            }
            catch { }
            if (draggedLink == null) return;

            draggedItem = _model.LinkToItems.TryGetValue(draggedLink, out var itemList) && itemList?.Count > 0 ? itemList[0] : null;
            DLGStandardItem dropParent;
            int insertRow;
            var targetDlg = targetItem?.Tag as DLGStandardItem;
            bool crossType = targetDlg?.Link?.Node != null && draggedLink?.Node != null &&
                !((draggedLink.Node is DLGReply && targetDlg.Link.Node is DLGReply) || (draggedLink.Node is DLGEntry && targetDlg.Link.Node is DLGEntry));
            if (position == DropPosition.OnTopOf)
            {
                dropParent = parentItem;
                insertRow = 0;
            }
            else if (crossType)
            {
                // Cross-type (Reply into Entry or vice versa): Above = first child, Below = last child
                dropParent = targetDlg;
                insertRow = position == DropPosition.Above ? 0 : (targetDlg?.RowCount ?? 0);
            }
            else
            {
                dropParent = parentItem;
                insertRow = row;
            }

            if (draggedItem != null)
            {
                _model.MoveItemToIndex(draggedItem, insertRow, dropParent);
            }
            else
            {
                _actionHistory.Apply(new PasteItemAction(dropParent, insertRow, draggedLink, false));
            }
            SelectTreeViewItem(draggedItem ?? (_model.LinkToItems.TryGetValue(draggedLink, out var lst) && lst?.Count > 0 ? lst[0] : null));
        }

        /// <summary>Returns true if the pointer event source is the scrollbar (or a descendant of it), so we do not start a drag or change selection when the user is interacting with the scrollbar.</summary>
        private static bool IsPointerOverScrollBar(Visual source, Visual treeView)
        {
            for (var v = source as Visual; v != null && v != treeView; v = v.GetVisualParent())
            {
                if (v is ScrollBar)
                    return true;
            }
            return false;
        }

        private void OnTreeViewPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (IsPointerOverScrollBar(e.Source as Visual, _dialogTree))
            {
                _dragStartPosition = null;
                return;
            }
            _dragStartPosition = e.GetPosition(_dialogTree);
            _dragStarted = false;
        }

        private void OnTreeViewPointerMoved(object sender, PointerEventArgs e)
        {
            if (_dragStartPosition == null || _dragStarted || _model == null)
            {
                return;
            }
            var pos = e.GetPosition(_dialogTree);
            double dx = pos.X - _dragStartPosition.Value.X;
            double dy = pos.Y - _dragStartPosition.Value.Y;
            if (Math.Abs(dx) < 4 && Math.Abs(dy) < 4)
            {
                return;
            }
            var item = GetSelectedItemFromTreeView();
            if (item == null)
            {
                return;
            }
            _dragStarted = true;
            _dragStartPosition = null;
            string mime = _model.MimeData(new[] { item });
            var dataTransfer = new DataTransfer();
            dataTransfer.Add(DataTransferItem.Create(DlgMimeDataFormat, mime));
            _ = DragDrop.DoDragDropAsync(e, dataTransfer, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void OnTreeViewPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _dragStartPosition = null;
            _dragStarted = false;
        }

        /// <summary>
        /// Starts a drag operation from a list widget (orphaned or pinned list). Uses the same MIME format as tree drag so drops on tree or pinned list work.
        /// </summary>
        public void StartDragFromListWidget(DLGListWidgetItem listItem, PointerEventArgs e)
        {
            if (listItem?.Link == null || _model == null)
            {
                return;
            }
            var tempItem = new DLGStandardItem(listItem.Link);
            string mime = _model.MimeData(new[] { tempItem });
            var dataTransfer = new DataTransfer();
            dataTransfer.Add(DataTransferItem.Create(DlgMimeDataFormat, mime));
            _ = DragDrop.DoDragDropAsync(e, dataTransfer, DragDropEffects.Copy | DragDropEffects.Move);
        }

        /// <summary>
        /// Jumps to the original node of a copied item.
        /// Searches through the entire dialog tree to find the original node that matches the copied item's node,
        /// then expands the tree and selects the original item.
        /// </summary>
        private void JumpToOriginal()
        {
            // Get the currently selected item
            DLGStandardItem selectedItem = null;
            var treeSelectedItem = _dialogTree?.SelectedItem;
            if (treeSelectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                selectedItem = dlgItem;
            }
            else if (treeSelectedItem is DLGStandardItem dlgItemDirect)
            {
                selectedItem = dlgItemDirect;
            }

            if (selectedItem?.Link == null)
            {
                return;
            }

            // Get the source node from the selected item's link
            DLGNode sourceNode = selectedItem.Link.Node;
            if (sourceNode == null)
            {
                return;
            }

            // Get the link from the selected item to avoid finding the same item
            DLGLink selectedLink = selectedItem.Link;

            // Perform breadth-first search through the entire dialog tree to find the original node
            var items = new Queue<DLGStandardItem>();
            var rootItems = _model.GetRootItems();
            foreach (var rootItem in rootItems)
            {
                items.Enqueue(rootItem);
            }

            DLGStandardItem foundItem = null;
            while (items.Count > 0)
            {
                DLGStandardItem item = items.Dequeue();
                if (item?.Link == null)
                {
                    continue;
                }

                // Check if this item's node matches the source node
                // Also check that this is a different link (not the same one we started with)
                // This ensures we find a different reference to the same node (the "original")
                if (item.Link.Node == sourceNode && item.Link != selectedLink)
                {
                    foundItem = item;
                    break;
                }

                // Add all children to the queue for breadth-first search
                foreach (var child in item.Children)
                {
                    items.Enqueue(child);
                }
            }

            // If we didn't find a different link, try finding any link (fallback behavior)
            // This handles the case where there's only one link pointing to the node
            if (foundItem == null)
            {
                // Reset the queue
                items.Clear();
                foreach (var rootItem in rootItems)
                {
                    items.Enqueue(rootItem);
                }

                while (items.Count > 0)
                {
                    DLGStandardItem item = items.Dequeue();
                    if (item?.Link == null)
                    {
                        continue;
                    }

                    if (item.Link.Node == sourceNode)
                    {
                        foundItem = item;
                        break;
                    }

                    foreach (var child in item.Children)
                    {
                        items.Enqueue(child);
                    }
                }
            }

            if (foundItem != null)
            {
                // Expand to root and select the found item
                ExpandToRoot(foundItem);
                HighlightResult(foundItem);
            }
            else
            {
                new RobustLogger().Error($"Failed to find original node for node {sourceNode}");
            }
        }

        /// <summary>
        /// Expands all parent items to make the specified item visible.
        /// </summary>
        /// <param name="item">The item whose parents should be expanded.</param>
        private void ExpandToRoot(DLGStandardItem item)
        {
            if (item == null || _dialogTree == null)
            {
                return;
            }

            // Find the TreeViewItem corresponding to this DLGStandardItem
            TreeViewItem treeItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, item);
            if (treeItem != null)
            {
                // Expand all parent items
                ExpandParentItems(treeItem);
            }
        }

        /// <summary>
        /// Finds references to the specified item.
        /// </summary>
        public void FindReferences(DLGStandardItem item)
        {
            if (item?.Link == null)
            {
                return;
            }
            FindReferences(item.Link);
        }

        /// <summary>
        /// Finds references to the specified link (used when context menu is from list widget).
        /// </summary>
        public void FindReferences(DLGLink link)
        {
            if (link == null)
            {
                return;
            }

            if (_currentReferenceIndex >= 0 && _currentReferenceIndex < _referenceHistory.Count - 1)
            {
                _referenceHistory.RemoveRange(_currentReferenceIndex + 1, _referenceHistory.Count - _currentReferenceIndex - 1);
            }

            string itemHtml = GetItemDisplayTextFromLink(link);

            // Increment reference index
            _currentReferenceIndex++;

            // Find all items that link to the same node as link
            var references = new List<WeakReference<DLGLink>>();
            foreach (var kvp in _model.LinkToItems)
            {
                foreach (var thisItem in kvp.Value)
                {
                    if (thisItem?.Link?.Node != null && thisItem.Link.Node.Links != null && thisItem.Link.Node.Links.Contains(link))
                    {
                        // Create weak reference to the link
                        var linkRef = new WeakReference<DLGLink>(thisItem.Link);
                        references.Add(linkRef);
                    }
                }
            }

            // Add to history and show dialog
            _referenceHistory.Add(Tuple.Create(references, itemHtml));
            ShowReferenceDialog(references, itemHtml);
        }

        /// <summary>
        /// Pastes an item.
        /// </summary>
        private void PasteItem(bool asNewBranches)
        {
            if (_copy == null)
            {
                // Show message to user when MessageBox is available
                return;
            }

            // Get the selected item from the tree view
            DLGStandardItem selectedItem = null;
            if (_dialogTree?.SelectedItem != null)
            {
                var treeSelectedItem = _dialogTree.SelectedItem;
                if (treeSelectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
                {
                    selectedItem = dlgItem;
                }
                else if (treeSelectedItem is DLGStandardItem dlgItemDirect)
                {
                    selectedItem = dlgItemDirect;
                }
            }

            // If no selected item, try to get from model's selected index
            if (selectedItem == null && _model.SelectedIndex >= 0 && _model.SelectedIndex < _model.RowCount)
            {
                DLGLink selectedLink = _model.GetStarterAt(_model.SelectedIndex);
                if (selectedLink != null)
                {
                    // Find the item in the model that corresponds to this link
                    var rootItems = _model.GetRootItems();
                    if (_model.SelectedIndex < rootItems.Count)
                    {
                        selectedItem = rootItems[_model.SelectedIndex];
                    }
                }
            }

            _actionHistory.Apply(new PasteItemAction(selectedItem, null, _copy, asNewBranches));

            // Select the newly pasted item if possible
            if (selectedItem != null && selectedItem.Children.Count > 0)
            {
                var pastedChild = selectedItem.Children[selectedItem.Children.Count - 1];
                SelectTreeViewItem(pastedChild);
            }
        }

        /// <summary>
        /// Deletes the selected node.
        /// </summary>
        private void DeleteSelectedNode()
        {
            if (_model.SelectedIndex >= 0 && _model.SelectedIndex < _model.RowCount)
            {
                DLGLink link = _model.GetStarterAt(_model.SelectedIndex);
                if (link != null)
                {
                    RemoveStarter(link);
                }
            }
        }

        /// <summary>
        /// Deletes the node everywhere.
        /// </summary>
        private void DeleteNodeEverywhere()
        {
            if (_dialogTree?.SelectedItem == null)
            {
                return;
            }

            // Get selected item from tree
            DLGLink link = null;
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                link = dlgItem?.Link;
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                link = dlgItemDirect?.Link;
            }

            if (link?.Node == null)
            {
                return;
            }

            _actionHistory.Apply(new DeleteNodeEverywhereAction(this, link.Node));
        }

        /// <summary>
        /// Sets expand recursively for tree items.
        /// </summary>
        /// <param name="expand">True to expand all items, false to collapse all items.</param>
        /// <param name="maxDepth">Maximum depth to expand/collapse. Use -1 for unlimited depth.</param>
        private void SetExpandRecursively(bool expand, int maxDepth)
        {
            if (_dialogTree == null || _dialogTree.ItemsSource == null)
            {
                return;
            }

            // Get the selected item from the tree
            DLGStandardItem selectedItem = null;
            var treeSelectedItem = _dialogTree.SelectedItem;
            if (treeSelectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                selectedItem = dlgItem;
            }
            else if (treeSelectedItem is DLGStandardItem dlgItemDirect)
            {
                selectedItem = dlgItemDirect;
            }

            // If no item is selected, operate on all root items
            if (selectedItem == null)
            {
                var rootItems = _model?.GetRootItems();
                if (rootItems != null)
                {
                    var rootSeenNodes = new HashSet<DLGNode>();
                    foreach (var rootItem in rootItems)
                    {
                        TreeViewItem rootTreeItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, rootItem);
                        if (rootTreeItem != null)
                        {
                            SetExpandRecursivelyInternal(rootItem, rootTreeItem, rootSeenNodes, expand, maxDepth, 0, true);
                        }
                    }
                }
                return;
            }

            // Find the TreeViewItem corresponding to the selected DLGStandardItem
            TreeViewItem selectedTreeItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, selectedItem);
            if (selectedTreeItem == null)
            {
                return;
            }

            // Recursively expand/collapse starting from the selected item
            var seenNodes = new HashSet<DLGNode>();
            SetExpandRecursivelyInternal(selectedItem, selectedTreeItem, seenNodes, expand, maxDepth, 0, true);
        }

        /// <summary>
        /// Internal recursive method to expand/collapse tree items.
        /// </summary>
        /// <param name="item">The DLGStandardItem to process.</param>
        /// <param name="treeItem">The corresponding TreeViewItem.</param>
        /// <param name="seenNodes">Set of nodes already processed (prevents infinite loops).</param>
        /// <param name="expand">True to expand, false to collapse.</param>
        /// <param name="maxDepth">Maximum depth. Use -1 for unlimited.</param>
        /// <param name="depth">Current depth in the tree.</param>
        /// <param name="isRoot">True if this is the root item being processed.</param>
        private void SetExpandRecursivelyInternal(
            DLGStandardItem item,
            TreeViewItem treeItem,
            HashSet<DLGNode> seenNodes,
            bool expand,
            int maxDepth,
            int depth,
            bool isRoot)
        {
            if (maxDepth >= 0 && depth > maxDepth)
            {
                return;
            }

            if (item == null)
            {
                return;
            }

            if (item.Link == null)
            {
                return;
            }

            DLGLink link = item.Link;

            if (link.Node != null && seenNodes.Contains(link.Node))
            {
                return;
            }

            if (link.Node != null)
            {
                seenNodes.Add(link.Node);
            }

            if (expand)
            {
                treeItem.IsExpanded = true;
            }
            else if (!isRoot)
            {
                treeItem.IsExpanded = false;
            }

            if (treeItem.ItemsSource != null)
            {
                foreach (TreeViewItem childTreeItem in treeItem.ItemsSource as System.Collections.IEnumerable)
                {
                    if (childTreeItem == null)
                    {
                        continue;
                    }

                    DLGStandardItem childItem = childTreeItem.Tag as DLGStandardItem;
                    if (childItem == null)
                    {
                        continue;
                    }

                    // Recursively process child
                    SetExpandRecursivelyInternal(childItem, childTreeItem, seenNodes, expand, maxDepth, depth + 1, false);
                }
            }
        }

        /// <summary>
        /// Selects a tree view item by its DLGStandardItem.
        /// Helper method for programmatically selecting items in the tree view.
        /// </summary>
        /// <param name="item">The DLGStandardItem to select.</param>
        private void SelectTreeViewItem(DLGStandardItem item)
        {
            if (_dialogTree == null || item == null || _dialogTree.ItemsSource == null)
            {
                return;
            }

            // Recursively search for the tree view item matching the DLGStandardItem
            TreeViewItem foundItem = FindTreeViewItem(_dialogTree.ItemsSource as System.Collections.IEnumerable, item);
            if (foundItem != null)
            {
                _dialogTree.SelectedItem = foundItem;
                // Expand parent items to ensure the selected item is visible
                ExpandParentItems(foundItem);
            }
        }

        /// <summary>
        /// Recursively finds a TreeViewItem by its Tag (DLGStandardItem).
        /// </summary>
        /// <param name="items">The items collection to search.</param>
        /// <param name="targetItem">The DLGStandardItem to find.</param>
        /// <returns>The TreeViewItem if found, null otherwise.</returns>
        private TreeViewItem FindTreeViewItem(System.Collections.IEnumerable items, DLGStandardItem targetItem)
        {
            if (items == null || targetItem == null)
            {
                return null;
            }

            foreach (TreeViewItem treeItem in items)
            {
                if (treeItem == null)
                {
                    continue;
                }

                // Check if this tree item matches the target
                if (treeItem.Tag == targetItem)
                {
                    return treeItem;
                }

                // Recursively search children
                if (treeItem.ItemsSource != null)
                {
                    TreeViewItem found = FindTreeViewItem(treeItem.ItemsSource as System.Collections.IEnumerable, targetItem);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Expands all parent items of the specified tree view item to ensure it's visible.
        /// </summary>
        /// <param name="item">The tree view item whose parents should be expanded.</param>
        private void ExpandParentItems(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }

            // Get parent container (TreeViewItem or TreeView)
            var parent = item.Parent;
            while (parent != null)
            {
                if (parent is TreeViewItem parentTreeItem)
                {
                    parentTreeItem.IsExpanded = true;
                    parent = parentTreeItem.Parent;
                }
                else
                {
                    break;
                }
            }
        }

        // Expose for testing
        public HashSet<Key> KeysDown => _keysDown;

        /// <summary>
        /// Gets whether the editor is in focus mode (showing only a specific node and its children).
        /// </summary>
        public bool Focused => _focused;

        /// <summary>
        /// Gets the current reference index for navigation.
        /// </summary>
        public int CurrentReferenceIndex => _currentReferenceIndex;

        /// <summary>
        /// Gets the count of items in the reference history.
        /// </summary>
        public int ReferenceHistoryCount => _referenceHistory.Count;

        /// <summary>
        /// Gets the dialog paths for an item (link parent path, link path, linked to path).
        /// </summary>
        /// <param name="item">The item to get paths for (DLGStandardItem or DLGListWidgetItem).</param>
        /// <returns>Tuple of (link_parent_path, link_path, linked_to_path).</returns>
        public Tuple<string, string, string> GetItemDlgPaths(object item)
        {
            string linkParentPath = "";
            string linkPath = "";
            string linkedToPath = "";

            DLGLink link = null;
            if (item is DLGStandardItem standardItem)
            {
                link = standardItem.Link;
                // In PyKotor, link_parent_path comes from item.data(_LINK_PARENT_NODE_PATH_ROLE); we derive from parent node path
                if (standardItem.Parent != null && standardItem.Parent.Link != null && standardItem.Parent.Link.Node != null)
                {
                    linkParentPath = standardItem.Parent.Link.Node.Path();
                }
            }
            else if (item is DLGListWidgetItem listItem)
            {
                link = listItem.Link;
            }

            if (link != null)
            {
                // Determine if link is a starter
                bool isStarter = _coreDlg != null && _coreDlg.Starters != null && _coreDlg.Starters.Contains(link);
                linkPath = link.PartialPath(isStarter);

                if (link.Node != null)
                {
                    linkedToPath = link.Node.Path();
                }
            }

            return Tuple.Create(linkParentPath, linkPath, linkedToPath);
        }

        /// <summary>
        /// Shows the reference dialog with the specified references.
        /// Creates a new dialog each time the previous one was closed (Avalonia does not allow re-showing a closed window).
        /// </summary>
        /// <param name="references">List of weak references to DLG links.</param>
        /// <param name="itemHtml">HTML text describing the item being referenced.</param>
        public void ShowReferenceDialog(List<WeakReference<DLGLink>> references, string itemHtml)
        {
            if (_dialogReferences == null)
            {
                _dialogReferences = new ReferenceChooserDialog(references, this, itemHtml);
                _dialogReferences.ItemChosen += OnReferenceChosen;
                _dialogReferences.Closed += (s, __) => _dialogReferences = null;
            }
            else
            {
                _dialogReferences.UpdateReferences(references, itemHtml);
            }

            if (!_dialogReferences.IsVisible)
            {
                _dialogReferences.Show();
            }
        }

        /// <summary>
        /// Handles when a reference is chosen from the dialog.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="item">The selected DLG list widget item.</param>
        private void OnReferenceChosen(object sender, DLGListWidgetItem item)
        {
            if (item?.Link != null)
            {
                JumpToNode(item.Link);
            }
        }

        /// <summary>
        /// Jumps to the specified node by highlighting it in the tree.
        /// </summary>
        /// <param name="link">The link to jump to.</param>
        public void JumpToNode(DLGLink link)
        {
            if (link == null || _model == null || _model.LinkToItems == null)
            {
                return;
            }

            if (!_model.LinkToItems.ContainsKey(link))
            {
                return;
            }

            var items = _model.LinkToItems[link];
            if (items != null && items.Count > 0)
            {
                HighlightResult(items[0]);
            }
        }

        /// <summary>
        /// Navigates back in the reference history.
        /// </summary>
        public void NavigateBack()
        {
            if (_currentReferenceIndex > 0)
            {
                _currentReferenceIndex--;
                var historyItem = _referenceHistory[_currentReferenceIndex];
                ShowReferenceDialog(historyItem.Item1, historyItem.Item2);
            }
        }

        /// <summary>
        /// Navigates forward in the reference history.
        /// </summary>
        public void NavigateForward()
        {
            if (_currentReferenceIndex < _referenceHistory.Count - 1)
            {
                _currentReferenceIndex++;
                var historyItem = _referenceHistory[_currentReferenceIndex];
                ShowReferenceDialog(historyItem.Item1, historyItem.Item2);
            }
        }
    }
}
