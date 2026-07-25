using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using BioWare.Extract;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Widgets;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using Window = Avalonia.Controls.Window;
using TextBlock = Avalonia.Controls.TextBlock;
using UTS = BioWare.Resource.Formats.GFF.Generics.UTS;
using UTSHelpers = BioWare.Resource.Formats.GFF.Generics.UTSHelpers;
using NumericUpDown = Avalonia.Controls.NumericUpDown;
using Slider = Avalonia.Controls.Slider;
using RadioButton = Avalonia.Controls.RadioButton;
using CheckBox = Avalonia.Controls.CheckBox;
using Button = Avalonia.Controls.Button;
using TextBox = Avalonia.Controls.TextBox;
using ListBox = Avalonia.Controls.ListBox;
using ScrollViewer = Avalonia.Controls.ScrollViewer;
using StackPanel = Avalonia.Controls.StackPanel;
using Expander = Avalonia.Controls.Expander;
using ResourceType = BioWare.Common.ResourceType;
using GFF = BioWare.Resource.Formats.GFF.GFF;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Editors
{
    public partial class OdyToolUTS : Editor
    {
        private UTS _uts;

        // UI Controls - Basic
        private TextBox _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private Button _resrefGenerateBtn;
        private Slider _volumeSlider;
        private CheckBox _activeCheckbox;

        // UI Controls - Advanced
        private RadioButton _playRandomRadio;
        private RadioButton _playSpecificRadio;
        private RadioButton _playEverywhereRadio;
        private RadioButton _orderSequentialRadio;
        private RadioButton _orderRandomRadio;
        private NumericUpDown _intervalSpin;
        private NumericUpDown _intervalVariationSpin;
        private Slider _volumeVariationSlider;
        private Slider _pitchVariationSlider;

        // UI Controls - Sounds
        private ListBox _soundList;
        private TextBox _soundEdit;
        private Button _addSoundBtn;
        private Button _removeSoundBtn;
        private Button _playSoundBtn;
        private Button _stopSoundBtn;
        private Button _moveUpBtn;
        private Button _moveDownBtn;

        // UI Controls - Positioning
        private RadioButton _styleOnceRadio;
        private RadioButton _styleSeamlessRadio;
        private RadioButton _styleRepeatRadio;
        private NumericUpDown _cutoffSpin;
        private NumericUpDown _maxVolumeDistanceSpin;
        private NumericUpDown _heightSpin;
        private NumericUpDown _northRandomSpin;
        private NumericUpDown _eastRandomSpin;

        // UI Controls - Comments
        private TextBox _commentsEdit;
        private TabControl _editorSurface;

        private NAudioMediaPlayer _soundPlayer;
        private bool _loadingSoundSelection;
        private bool _loadingUts;
        private bool _clearInitialDirtyOnOpen = true;

        public TextBox TagEdit => _tagEdit;
        public TextBox ResrefEdit => _resrefEdit;
        public Slider VolumeSlider => _volumeSlider;
        public CheckBox ActiveCheckbox => _activeCheckbox;
        public RadioButton PlayRandomRadio => _playRandomRadio;
        public RadioButton PlaySpecificRadio => _playSpecificRadio;
        public RadioButton PlayEverywhereRadio => _playEverywhereRadio;
        public RadioButton OrderSequentialRadio => _orderSequentialRadio;
        public RadioButton OrderRandomRadio => _orderRandomRadio;
        public NumericUpDown IntervalSpin => _intervalSpin;
        public NumericUpDown IntervalVariationSpin => _intervalVariationSpin;
        public Slider VolumeVariationSlider => _volumeVariationSlider;
        public Slider PitchVariationSlider => _pitchVariationSlider;
        public ListBox SoundList => _soundList;
        public TextBox SoundEdit => _soundEdit;
        public RadioButton StyleOnceRadio => _styleOnceRadio;
        public RadioButton StyleSeamlessRadio => _styleSeamlessRadio;
        public RadioButton StyleRepeatRadio => _styleRepeatRadio;
        public NumericUpDown CutoffSpin => _cutoffSpin;
        public NumericUpDown MaxVolumeDistanceSpin => _maxVolumeDistanceSpin;
        public NumericUpDown HeightSpin => _heightSpin;
        public NumericUpDown NorthRandomSpin => _northRandomSpin;
        public NumericUpDown EastRandomSpin => _eastRandomSpin;
        public TextBox CommentsEdit => _commentsEdit;
        internal bool HasStructuredEditorSurface => _editorSurface != null && _soundList != null && _commentsEdit != null;

        public OdyToolUTS() : this(null, null) { }
        public OdyToolUTS(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTS", "sound",
                new[] { ResourceType.UTS, ResourceType.UTS_XML },
                new[] { ResourceType.UTS, ResourceType.UTS_XML },
                installation)
        {
            _installation = installation;
            _uts = new UTS();

            // Initialize cross-platform sound player (replaces Windows-only SoundPlayer)
            _soundPlayer = new NAudioMediaPlayer();

            InitializeComponent();
            SetupUI();
            New();
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
            else
            {
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _nameEdit = EditorHelpers.FindControlSafe<TextBox>(this, "nameEdit");
                _tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateBtn");
                _resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateBtn");
                _nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "nameEditBtn");
                _volumeSlider = EditorHelpers.FindControlSafe<Slider>(this, "volumeSlider");
                _activeCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "activeCheckbox");
                _playRandomRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "playRandomRadio");
                _playSpecificRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "playSpecificRadio");
                _playEverywhereRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "playEverywhereRadio");
                _orderSequentialRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "orderSequentialRadio");
                _orderRandomRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "orderRandomRadio");
                _intervalSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "intervalSpin");
                _intervalVariationSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "intervalVariationSpin");
                _volumeVariationSlider = EditorHelpers.FindControlSafe<Slider>(this, "volumeVariationSlider");
                _pitchVariationSlider = EditorHelpers.FindControlSafe<Slider>(this, "pitchVariationSlider");
                _soundList = EditorHelpers.FindControlSafe<ListBox>(this, "soundList");
                _soundEdit = EditorHelpers.FindControlSafe<TextBox>(this, "soundEdit");
                _addSoundBtn = EditorHelpers.FindControlSafe<Button>(this, "addSoundBtn");
                _removeSoundBtn = EditorHelpers.FindControlSafe<Button>(this, "removeSoundBtn");
                _playSoundBtn = EditorHelpers.FindControlSafe<Button>(this, "playSoundBtn");
                _stopSoundBtn = EditorHelpers.FindControlSafe<Button>(this, "stopSoundBtn");
                _moveUpBtn = EditorHelpers.FindControlSafe<Button>(this, "moveUpBtn");
                _moveDownBtn = EditorHelpers.FindControlSafe<Button>(this, "moveDownBtn");
                _styleOnceRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "styleOnceRadio");
                _styleSeamlessRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "styleSeamlessRadio");
                _styleRepeatRadio = EditorHelpers.FindControlSafe<RadioButton>(this, "styleRepeatRadio");
                _cutoffSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "cutoffSpin");
                _maxVolumeDistanceSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "maxVolumeDistanceSpin");
                _heightSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "heightSpin");
                _northRandomSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "northRandomSpin");
                _eastRandomSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "eastRandomSpin");
                _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");
                _editorSurface = EditorHelpers.FindControlSafe<TabControl>(this, "editorSurface");

                if (_tagEdit == null || _resrefEdit == null || _soundList == null || _commentsEdit == null)
                {
                    SetupProgrammaticUI();
                }
                else
                {
                    BindLoadedControlEvents();
                    BindDirtyTracking();
                    AttachReferenceSearchMenus();
                }
            }
        }

        private void BindLoadedControlEvents()
        {
            EditorHelpers.BindClick(_nameEditBtn, EditName);
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            EditorHelpers.BindClick(_addSoundBtn, AddSound);
            EditorHelpers.BindClick(_removeSoundBtn, RemoveSound);
            EditorHelpers.BindClick(_playSoundBtn, PlaySound);
            EditorHelpers.BindClick(_stopSoundBtn, StopSound);
            EditorHelpers.BindClick(_moveUpBtn, MoveSoundUp);
            EditorHelpers.BindClick(_moveDownBtn, MoveSoundDown);
            if (_soundList != null)
            {
                _soundList.SelectionChanged += (s, e) => LoadSelectedSoundIntoEdit();
            }
            if (_soundEdit != null)
            {
                _soundEdit.LostFocus += (s, e) => CommitSoundEdit();
            }

            EditorHelpers.BindRadioChecked(_playRandomRadio, ChangePlay);
            EditorHelpers.BindRadioChecked(_playSpecificRadio, ChangePlay);
            EditorHelpers.BindRadioChecked(_playEverywhereRadio, ChangePlay);
            EditorHelpers.BindRadioChecked(_styleOnceRadio, ChangeStyle);
            EditorHelpers.BindRadioChecked(_styleSeamlessRadio, ChangeStyle);
            EditorHelpers.BindRadioChecked(_styleRepeatRadio, ChangeStyle);
        }

        private void BindDirtyTracking()
        {
            if (_tagEdit != null) _tagEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_resrefEdit != null) _resrefEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_volumeSlider != null) _volumeSlider.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_activeCheckbox != null) _activeCheckbox.IsCheckedChanged += (s, e) => MarkDirtyAfterLoad();
            if (_orderSequentialRadio != null) _orderSequentialRadio.Checked += (s, e) => MarkDirtyAfterLoad();
            if (_orderRandomRadio != null) _orderRandomRadio.Checked += (s, e) => MarkDirtyAfterLoad();
            if (_intervalSpin != null) _intervalSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_intervalVariationSpin != null) _intervalVariationSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_volumeVariationSlider != null) _volumeVariationSlider.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_pitchVariationSlider != null) _pitchVariationSlider.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_cutoffSpin != null) _cutoffSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_maxVolumeDistanceSpin != null) _maxVolumeDistanceSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_heightSpin != null) _heightSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_northRandomSpin != null) _northRandomSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_eastRandomSpin != null) _eastRandomSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_commentsEdit != null) _commentsEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
        }

        private void MarkDirtyAfterLoad()
        {
            if (!_loadingUts)
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

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Basic Group
            var basicGroup = new Expander { Header = "Basic", IsExpanded = true };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Name
            var nameLabel = new TextBlock { Text = "Name:" };
            _nameEdit = new TextBox { IsReadOnly = true };
            _nameEditBtn = new Button { Content = "Edit Name" };
            EditorHelpers.BindClick(_nameEditBtn, EditName);
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEdit);
            basicPanel.Children.Add(_nameEditBtn);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            _tagEdit = new TextBox();
            _tagGenerateBtn = new Button { Content = "Generate" };
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(_tagEdit);
            basicPanel.Children.Add(_tagGenerateBtn);

            // ResRef
            var resrefLabel = new TextBlock { Text = "ResRef:" };
            _resrefEdit = new TextBox();
            _resrefGenerateBtn = new Button { Content = "Generate" };
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            basicPanel.Children.Add(resrefLabel);
            basicPanel.Children.Add(_resrefEdit);
            basicPanel.Children.Add(_resrefGenerateBtn);

            AttachReferenceSearchMenus();

            // Volume
            var volumeLabel = new TextBlock { Text = "Volume:" };
            _volumeSlider = new Slider { Minimum = 0, Maximum = 255, Value = 127 };
            basicPanel.Children.Add(volumeLabel);
            basicPanel.Children.Add(_volumeSlider);

            // Active
            _activeCheckbox = new CheckBox { Content = "Active" };
            basicPanel.Children.Add(_activeCheckbox);

            basicGroup.Content = basicPanel;
            mainPanel.Children.Add(basicGroup);

            // Advanced Group
            var advancedGroup = new Expander { Header = "Advanced", IsExpanded = false };
            var advancedPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Play Mode
            var playModeLabel = new TextBlock { Text = "Play Mode:" };
            _playRandomRadio = new RadioButton { Content = "Random Position", GroupName = "PlayMode" };
            _playSpecificRadio = new RadioButton { Content = "Specific Position", GroupName = "PlayMode" };
            _playEverywhereRadio = new RadioButton { Content = "Everywhere", GroupName = "PlayMode", IsChecked = true };
            EditorHelpers.BindRadioChecked(_playRandomRadio, ChangePlay);
            EditorHelpers.BindRadioChecked(_playSpecificRadio, ChangePlay);
            EditorHelpers.BindRadioChecked(_playEverywhereRadio, ChangePlay);

            // Order
            var orderLabel = new TextBlock { Text = "Order:" };
            _orderSequentialRadio = new RadioButton { Content = "Sequential", GroupName = "Order", IsChecked = true };
            _orderRandomRadio = new RadioButton { Content = "Random", GroupName = "Order" };

            // Interval
            var intervalLabel = new TextBlock { Text = "Interval:" };
            _intervalSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };
            var intervalVariationLabel = new TextBlock { Text = "Interval Variation:" };
            _intervalVariationSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };

            // Variation
            var volumeVariationLabel = new TextBlock { Text = "Volume Variation:" };
            _volumeVariationSlider = new Slider { Minimum = 0, Maximum = 255 };
            var pitchVariationLabel = new TextBlock { Text = "Pitch Variation:" };
            _pitchVariationSlider = new Slider { Minimum = 0, Maximum = 100 };

            advancedPanel.Children.Add(playModeLabel);
            advancedPanel.Children.Add(_playRandomRadio);
            advancedPanel.Children.Add(_playSpecificRadio);
            advancedPanel.Children.Add(_playEverywhereRadio);
            advancedPanel.Children.Add(orderLabel);
            advancedPanel.Children.Add(_orderSequentialRadio);
            advancedPanel.Children.Add(_orderRandomRadio);
            advancedPanel.Children.Add(intervalLabel);
            advancedPanel.Children.Add(_intervalSpin);
            advancedPanel.Children.Add(intervalVariationLabel);
            advancedPanel.Children.Add(_intervalVariationSpin);
            advancedPanel.Children.Add(volumeVariationLabel);
            advancedPanel.Children.Add(_volumeVariationSlider);
            advancedPanel.Children.Add(pitchVariationLabel);
            advancedPanel.Children.Add(_pitchVariationSlider);

            advancedGroup.Content = advancedPanel;
            mainPanel.Children.Add(advancedGroup);

            // Sounds Group
            var soundsGroup = new Expander { Header = "Sounds", IsExpanded = false };
            var soundsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var soundsLabel = new TextBlock { Text = "Sound List:" };
            _soundList = new ListBox();
            _soundEdit = new TextBox { Watermark = "Selected sound resref" };
            _soundList.SelectionChanged += (s, e) => LoadSelectedSoundIntoEdit();
            _soundEdit.LostFocus += (s, e) => CommitSoundEdit();
            var soundButtonsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _addSoundBtn = new Button { Content = "Add" };
            EditorHelpers.BindClick(_addSoundBtn, AddSound);
            _removeSoundBtn = new Button { Content = "Remove" };
            EditorHelpers.BindClick(_removeSoundBtn, RemoveSound);
            _playSoundBtn = new Button { Content = "Play" };
            EditorHelpers.BindClick(_playSoundBtn, PlaySound);
            _stopSoundBtn = new Button { Content = "Stop" };
            EditorHelpers.BindClick(_stopSoundBtn, StopSound);
            _moveUpBtn = new Button { Content = "Up" };
            EditorHelpers.BindClick(_moveUpBtn, MoveSoundUp);
            _moveDownBtn = new Button { Content = "Down" };
            EditorHelpers.BindClick(_moveDownBtn, MoveSoundDown);
            soundButtonsPanel.Children.Add(_addSoundBtn);
            soundButtonsPanel.Children.Add(_removeSoundBtn);
            soundButtonsPanel.Children.Add(_playSoundBtn);
            soundButtonsPanel.Children.Add(_stopSoundBtn);
            soundButtonsPanel.Children.Add(_moveUpBtn);
            soundButtonsPanel.Children.Add(_moveDownBtn);
            soundsPanel.Children.Add(soundsLabel);
            soundsPanel.Children.Add(_soundList);
            soundsPanel.Children.Add(_soundEdit);
            soundsPanel.Children.Add(soundButtonsPanel);
            soundsGroup.Content = soundsPanel;
            mainPanel.Children.Add(soundsGroup);

            // Positioning Group
            var positioningGroup = new Expander { Header = "Positioning", IsExpanded = false };
            var positioningPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Style
            var styleLabel = new TextBlock { Text = "Style:" };
            _styleOnceRadio = new RadioButton { Content = "Once", GroupName = "Style", IsChecked = true };
            _styleSeamlessRadio = new RadioButton { Content = "Seamless", GroupName = "Style" };
            _styleRepeatRadio = new RadioButton { Content = "Repeat", GroupName = "Style" };
            EditorHelpers.BindRadioChecked(_styleOnceRadio, ChangeStyle);
            EditorHelpers.BindRadioChecked(_styleSeamlessRadio, ChangeStyle);
            EditorHelpers.BindRadioChecked(_styleRepeatRadio, ChangeStyle);

            // Distances
            var cutoffLabel = new TextBlock { Text = "Cutoff Distance:" };
            _cutoffSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue };
            var maxVolumeDistanceLabel = new TextBlock { Text = "Max Volume Distance:" };
            _maxVolumeDistanceSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue };
            var heightLabel = new TextBlock { Text = "Height:" };
            _heightSpin = new NumericUpDown { Minimum = decimal.MinValue, Maximum = decimal.MaxValue };
            var northRandomLabel = new TextBlock { Text = "North Random:" };
            _northRandomSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue };
            var eastRandomLabel = new TextBlock { Text = "East Random:" };
            _eastRandomSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue };

            positioningPanel.Children.Add(styleLabel);
            positioningPanel.Children.Add(_styleOnceRadio);
            positioningPanel.Children.Add(_styleSeamlessRadio);
            positioningPanel.Children.Add(_styleRepeatRadio);
            positioningPanel.Children.Add(cutoffLabel);
            positioningPanel.Children.Add(_cutoffSpin);
            positioningPanel.Children.Add(maxVolumeDistanceLabel);
            positioningPanel.Children.Add(_maxVolumeDistanceSpin);
            positioningPanel.Children.Add(heightLabel);
            positioningPanel.Children.Add(_heightSpin);
            positioningPanel.Children.Add(northRandomLabel);
            positioningPanel.Children.Add(_northRandomSpin);
            positioningPanel.Children.Add(eastRandomLabel);
            positioningPanel.Children.Add(_eastRandomSpin);

            positioningGroup.Content = positioningPanel;
            mainPanel.Children.Add(positioningGroup);

            // Comments Group
            var commentsGroup = new Expander { Header = "Comments", IsExpanded = false };
            var commentsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var commentsLabel = new TextBlock { Text = "Comment:" };
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            commentsPanel.Children.Add(commentsLabel);
            commentsPanel.Children.Add(_commentsEdit);
            commentsGroup.Content = commentsPanel;
            mainPanel.Children.Add(commentsGroup);

            BindDirtyTracking();

            scrollViewer.Content = mainPanel;
            var contentRoot = EditorHelpers.FindControlSafe<Avalonia.Controls.ContentControl>(this, "contentRoot");
            if (contentRoot != null)
            {
                contentRoot.Content = scrollViewer;
            }
            else
            {
                Content = scrollViewer;
            }
        }

        private void SetupUI()
        {
            var contentRoot = EditorHelpers.FindControlSafe<Avalonia.Controls.ContentControl>(this, "contentRoot");
            if (contentRoot != null && contentRoot.Content == null)
            {
                SetupProgrammaticUI();
            }
            else if (contentRoot == null && Content == null)
            {
                SetupProgrammaticUI();
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("The UTS file data is empty or invalid.");
            }

            var gff = GFFAuto.ReadGff(data, fileFormat: restype);
            _uts = UTSHelpers.ConstructUts(gff);
            LoadUTS(_uts);
        }

        private void LoadUTS(UTS uts)
        {
            _uts = uts;
            _loadingUts = true;
            try
            {

                // Basic
                if (_nameEdit != null)
                {
                    _nameEdit.Text = _installation != null ? _installation.String(uts.Name) : uts.Name.StringRef.ToString();
                }
                if (_tagEdit != null)
                {
                    _tagEdit.Text = uts.Tag;
                }
                if (_resrefEdit != null)
                {
                    _resrefEdit.Text = uts.ResRef.ToString();
                }
                if (_volumeSlider != null)
                {
                    _volumeSlider.Value = uts.Volume;
                }
                if (_activeCheckbox != null)
                {
                    _activeCheckbox.IsChecked = uts.Active;
                }

                // Advanced
                if (uts.RandomRangeX != 0 && uts.RandomRangeY != 0)
                {
                    if (_playRandomRadio != null) _playRandomRadio.IsChecked = true;
                }
                else if (uts.Positional)
                {
                    if (_playSpecificRadio != null) _playSpecificRadio.IsChecked = true;
                }
                else
                {
                    if (_playEverywhereRadio != null) _playEverywhereRadio.IsChecked = true;
                }

                if (_orderSequentialRadio != null) _orderSequentialRadio.IsChecked = !uts.Random;
                if (_orderRandomRadio != null) _orderRandomRadio.IsChecked = uts.Random;
                if (_intervalSpin != null) _intervalSpin.Value = uts.Interval;
                if (_intervalVariationSpin != null) _intervalVariationSpin.Value = uts.IntervalVariance;
                if (_volumeVariationSlider != null) _volumeVariationSlider.Value = uts.VolumeVariance;
                if (_pitchVariationSlider != null) _pitchVariationSlider.Value = (int)(uts.PitchVariance * 100);

                // Sounds
                if (_soundList != null)
                {
                    _soundList.Items.Clear();
                    if (uts.Sounds != null)
                    {
                        foreach (var sound in uts.Sounds)
                        {
                            _soundList.Items.Add(sound.ToString());
                        }
                    }
                    LoadSelectedSoundIntoEdit();
                }

                // Positioning
                if (uts.Continuous && uts.Looping)
                {
                    if (_styleSeamlessRadio != null) _styleSeamlessRadio.IsChecked = true;
                }
                else if (uts.Looping)
                {
                    if (_styleRepeatRadio != null) _styleRepeatRadio.IsChecked = true;
                }
                else
                {
                    if (_styleOnceRadio != null) _styleOnceRadio.IsChecked = true;
                }

                if (_cutoffSpin != null) _cutoffSpin.Value = (decimal?)uts.MinDistance;
                if (_maxVolumeDistanceSpin != null) _maxVolumeDistanceSpin.Value = (decimal?)uts.MaxDistance;
                if (_heightSpin != null) _heightSpin.Value = (decimal?)uts.Elevation;
                if (_northRandomSpin != null) _northRandomSpin.Value = (decimal?)uts.RandomRangeY;
                if (_eastRandomSpin != null) _eastRandomSpin.Value = (decimal?)uts.RandomRangeX;

                // Comments
                if (_commentsEdit != null) _commentsEdit.Text = uts.Comment;
            }
            finally
            {
                _loadingUts = false;
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            CommitSoundEdit();

            // Matching Python: uts: UTS = deepcopy(self._uts)
            var uts = CopyUts(_uts);

            // Basic - read from UI controls (matching Python which always reads from UI)
            // Python: uts.name = self.ui.nameEdit.locstring()
            // In C#, nameEdit is TextBox (read-only), LocalizedString is stored in _uts.Name and updated via EditName()
            // So we use uts.Name from the copy (which preserves the value set by EditName())
            // Note: This matches Python behavior where locstring() returns the stored LocalizedString
            uts.Name = uts.Name ?? LocalizedString.FromInvalid();
            uts.Tag = _tagEdit?.Text ?? "";
            uts.ResRef = ResRefFromText(_resrefEdit?.Text);
            uts.Volume = (int)(_volumeSlider?.Value ?? 127);
            uts.Active = _activeCheckbox?.IsChecked == true;

            // Advanced - read from UI controls
            uts.Positional = _playSpecificRadio?.IsChecked == true;
            uts.Random = _orderRandomRadio?.IsChecked == true;
            uts.Interval = (int)(_intervalSpin?.Value ?? 0);
            uts.IntervalVariance = (int)(_intervalVariationSpin?.Value ?? 0);
            uts.VolumeVariance = (int)(_volumeVariationSlider?.Value ?? 0);
            uts.PitchVariance = (float)((_pitchVariationSlider?.Value ?? 0) / 100.0);

            // Sounds - read from UI controls
            uts.Sounds.Clear();
            if (_soundList?.Items != null)
            {
                foreach (string item in _soundList.Items)
                {
                    string soundResRef = (item ?? string.Empty).Trim();
                    if (!string.IsNullOrEmpty(soundResRef))
                    {
                        uts.Sounds.Add(new ResRef(soundResRef));
                    }
                }
            }

            // Positioning - read from UI controls
            uts.Continuous = _styleSeamlessRadio?.IsChecked == true;
            uts.Looping = (_styleSeamlessRadio?.IsChecked == true) || (_styleRepeatRadio?.IsChecked == true);
            uts.MaxDistance = (float)(_maxVolumeDistanceSpin?.Value ?? 0);
            uts.MinDistance = (float)(_cutoffSpin?.Value ?? 0);
            uts.Elevation = (float)(_heightSpin?.Value ?? 0);
            uts.RandomRangeY = (float)(_northRandomSpin?.Value ?? 0);
            uts.RandomRangeX = (float)(_eastRandomSpin?.Value ?? 0);

            // Comments - read from UI controls
            uts.Comment = _commentsEdit?.Text ?? "";

            // Matching Python: gff: GFF = dismantle_uts(uts); write_gff(gff, data)
            BioWareGame game = _installation?.Game ?? BioWareGame.K2;
            var gff = UTSHelpers.DismantleUts(uts, game);
            ResourceType outputType = _restype == ResourceType.UTS_XML ? ResourceType.UTS_XML : ResourceType.UTS;
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        private static ResRef ResRefFromText(string text)
        {
            string value = (text ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(value) ? new ResRef(value) : ResRef.FromBlank();
        }

        // Matching Python: deepcopy(self._uts)
        private static UTS CopyUts(UTS source)
        {
            // Use Dismantle/Construct pattern for reliable deep copy (matching Python deepcopy behavior)
            BioWareGame game = BioWareGame.K2; // Default game for serialization
            var gff = UTSHelpers.DismantleUts(source, game);
            return UTSHelpers.ConstructUts(gff);
        }

        public override void New()
        {
            base.New();
            _uts = new UTS();
            LoadUTS(_uts);
        }

        private void EditName()
        {
            if (_installation == null) return;
            var dialog = new LocalizedStringDialog(this, _installation, _uts.Name);
            if (dialog.ShowDialog())
            {
                _uts.Name = dialog.LocString;
                if (_nameEdit != null)
                {
                    _nameEdit.Text = _installation.String(_uts.Name);
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
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_trg_000";
            }
            MarkDocumentDirty();
        }

        private void ChangeStyle()
        {
            // Enable/disable interval and variation groups based on style
            bool enableGroups = !(_styleSeamlessRadio?.IsChecked ?? false);
            if (_intervalSpin != null) _intervalSpin.IsEnabled = enableGroups;
            if (_intervalVariationSpin != null) _intervalVariationSpin.IsEnabled = enableGroups;
            if (_volumeVariationSlider != null) _volumeVariationSlider.IsEnabled = enableGroups;
            if (_pitchVariationSlider != null) _pitchVariationSlider.IsEnabled = enableGroups;
            if (_orderSequentialRadio != null) _orderSequentialRadio.IsEnabled = enableGroups;
            if (_orderRandomRadio != null) _orderRandomRadio.IsEnabled = enableGroups;

            if (_styleOnceRadio?.IsChecked ?? false)
            {
                if (_intervalSpin != null) _intervalSpin.IsEnabled = false;
            }
            MarkDirtyAfterLoad();
        }

        private void ChangePlay()
        {
            // Enable/disable range and distance groups based on play mode
            bool enableGroups = !(_playEverywhereRadio?.IsChecked ?? false);
            if (_cutoffSpin != null) _cutoffSpin.IsEnabled = enableGroups;
            if (_maxVolumeDistanceSpin != null) _maxVolumeDistanceSpin.IsEnabled = enableGroups;
            if (_heightSpin != null) _heightSpin.IsEnabled = enableGroups;
            if (_northRandomSpin != null) _northRandomSpin.IsEnabled = enableGroups;
            if (_eastRandomSpin != null) _eastRandomSpin.IsEnabled = enableGroups;

            if (_playSpecificRadio?.IsChecked ?? false)
            {
                if (_northRandomSpin != null) _northRandomSpin.Value = 0;
                if (_eastRandomSpin != null) _eastRandomSpin.Value = 0;
            }
            MarkDirtyAfterLoad();
        }

        private void PlaySound()
        {
            _soundPlayer?.Stop();

            if (_soundList?.SelectedItem == null)
            {
                return;
            }

            string resname = _soundList.SelectedItem.ToString();
            if (string.IsNullOrWhiteSpace(resname))
            {
                return;
            }

            if (_installation == null)
            {
                return;
            }

            // Default search order: SOUND, VOICE, OVERRIDE, CHITIN (matching PyKotor's default for UTS editor)
            byte[] soundData = _installation.Sound(resname.Trim(), new[]
            {
                SearchLocation.SOUND,
                SearchLocation.VOICE,
                SearchLocation.OVERRIDE,
                SearchLocation.CHITIN
            });

            if (soundData != null && soundData.Length > 0)
            {
                PlayByteSourceMedia(soundData);
            }
            else
            {
                _ = DialogHelper.ShowAsync("Could not find audio file", $"Could not find audio resource '{resname}'.", ButtonEnum.Ok, IconType.Error);
            }
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
            catch (Exception ex)
            {
                // Log error and show message box
                System.Console.WriteLine($"Failed to play sound: {ex}");
                _ = DialogHelper.ShowAsync("Error Playing Sound", $"Failed to play sound:\n{ex.Message}", ButtonEnum.Ok, IconType.Error);
                return false;
            }
        }

        private void StopSound()
        {
            _soundPlayer?.Stop();
        }

        private void AddSound()
        {
            if (_soundList != null)
            {
                _soundList.Items.Add(CreateUniqueSoundResRef());
                _soundList.SelectedIndex = _soundList.Items.Count - 1;
                LoadSelectedSoundIntoEdit();
                MarkDocumentDirty();
            }
        }

        private string CreateUniqueSoundResRef()
        {
            const string baseName = "new_sound";
            var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (_soundList?.Items != null)
            {
                foreach (var item in _soundList.Items)
                {
                    string value = item?.ToString();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        existing.Add(value.Trim());
                    }
                }
            }

            if (!existing.Contains(baseName))
            {
                return baseName;
            }

            for (int i = 1; i < 1000; i++)
            {
                string candidate = $"{baseName}{i}";
                if (candidate.Length > 16)
                {
                    candidate = candidate.Substring(0, 16);
                }
                if (!existing.Contains(candidate))
                {
                    return candidate;
                }
            }

            return baseName;
        }

        private void RemoveSound()
        {
            if (_soundList?.SelectedItem != null)
            {
                _soundList.Items.Remove(_soundList.SelectedItem);
                LoadSelectedSoundIntoEdit();
                MarkDocumentDirty();
            }
        }

        private void MoveSoundUp()
        {
            if (_soundList?.SelectedIndex > 0 && _soundList?.SelectedIndex < _soundList.Items.Count)
            {
                int index = _soundList.SelectedIndex;
                var item = _soundList.Items[index];
                _soundList.Items.RemoveAt(index);
                _soundList.Items.Insert(index - 1, item);
                _soundList.SelectedIndex = index - 1;
                MarkDocumentDirty();
            }
        }

        private void LoadSelectedSoundIntoEdit()
        {
            if (_soundEdit == null)
            {
                return;
            }

            _loadingSoundSelection = true;
            try
            {
                _soundEdit.Text = _soundList?.SelectedItem?.ToString() ?? string.Empty;
                _soundEdit.IsEnabled = _soundList?.SelectedItem != null;
            }
            finally
            {
                _loadingSoundSelection = false;
            }
        }

        private void CommitSoundEdit()
        {
            if (_loadingSoundSelection || _soundList == null || _soundEdit == null)
            {
                return;
            }

            int index = _soundList.SelectedIndex;
            if (index < 0 || index >= _soundList.Items.Count)
            {
                return;
            }

            string value = (_soundEdit.Text ?? string.Empty).Trim();
            if ((_soundList.Items[index]?.ToString() ?? string.Empty) == value)
            {
                return;
            }

            _loadingSoundSelection = true;
            try
            {
                _soundList.Items.RemoveAt(index);
                _soundList.Items.Insert(index, value);
                _soundList.SelectedIndex = index;
            }
            finally
            {
                _loadingSoundSelection = false;
            }
            MarkDocumentDirty();
        }

        internal void AddSoundForTest() => AddSound();
        internal void RemoveSoundForTest() => RemoveSound();
        internal void MoveSoundUpForTest() => MoveSoundUp();
        internal void MoveSoundDownForTest() => MoveSoundDown();

        private void MoveSoundDown()
        {
            if (_soundList?.SelectedIndex >= 0 && _soundList.SelectedIndex < _soundList.Items.Count - 1)
            {
                int index = _soundList.SelectedIndex;
                var item = _soundList.Items[index];
                _soundList.Items.RemoveAt(index);
                _soundList.Items.Insert(index + 1, item);
                _soundList.SelectedIndex = index + 1;
                MarkDocumentDirty();
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            await base.RunSaveAsAsync();
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
    }
}
