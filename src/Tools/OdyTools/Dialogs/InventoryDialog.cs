using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BioWare.Common;
using BioWare.Extract.Capsule;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using OdyTools.Data;
using System.ComponentModel;
using BioWare.Resource;

namespace OdyTools.Dialogs
{
    public partial class InventoryDialog : Window
    {
        private Window _parentWindow;
        private OdyInstallation _installation;
        private List<InventoryItem> _inventory;
        private Dictionary<EquipmentSlot, InventoryItem> _equipment;
        private bool _droid;
        private bool _isStore;
        private Button _okButton;
        private Button _cancelButton;
        public bool DialogResult { get; private set; }

        // Public parameterless constructor for XAML
        public InventoryDialog() : this(null, null, null, null, null, null)
        {
        }

        // Note: PyKotor uses Sequence[LazyCapsule] but UTM/UTP editors pass list[Capsule], so we use List<Capsule> for compatibility
        public InventoryDialog(
            Window parent,
            OdyInstallation installation,
            List<Capsule> capsules,
            List<string> folders,
            List<InventoryItem> inventory,
            Dictionary<EquipmentSlot, InventoryItem> equipment,
            bool droid = false,
            bool hideEquipment = false,
            bool isStore = false)
        {
            InitializeComponent();
            _parentWindow = parent;
            _installation = installation;
            _inventory = inventory ?? new List<InventoryItem>();
            _equipment = equipment ?? new Dictionary<EquipmentSlot, InventoryItem>();
            _droid = droid;
            _isStore = isStore;
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
            Title = "Inventory Editor";
            Width = 800;
            Height = 600;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Inventory Editor",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            _okButton = new Button { Content = "OK" };
            _okButton.Click += (sender, e) =>
            {
                Accept();
                DialogResult = true;
                Close(true);
            };
            _cancelButton = new Button { Content = "Cancel" };
            _cancelButton.Click += (sender, e) =>
            {
                DialogResult = false;
                Close(false);
            };

            panel.Children.Add(titleLabel);
            panel.Children.Add(_okButton);
            panel.Children.Add(_cancelButton);
            Content = panel;
        }

        public InventoryDialogUi Ui { get; private set; }

        private DataGrid _contentsTable;

        private void SetupUI()
        {
            // Find controls from XAML and set up event handlers
            try
            {
                _contentsTable = this.FindControl<DataGrid>("contentsTable");
                _okButton = this.FindControl<Button>("okButton");
                _cancelButton = this.FindControl<Button>("cancelButton");

                // Set up OK and Cancel button handlers
                if (_okButton != null)
                {
                    _okButton.Click += (sender, e) =>
                    {
                        Accept();
                        DialogResult = true;
                        Close(true);
                    };
                }
                if (_cancelButton != null)
                {
                    _cancelButton.Click += (sender, e) =>
                    {
                        DialogResult = false;
                        Close(false);
                    };
                }
            }
            catch
            {
                // XAML not loaded or control not found - will use programmatic UI
                _contentsTable = null;
                _okButton = null;
                _cancelButton = null;
            }

            // Configure DataGrid if it exists
            if (_contentsTable != null)
            {
                // Set up DataGrid columns if not already configured
                if (_contentsTable.Columns.Count == 0)
                {
                    // Column 0: Icon (QTableWidgetItem with icon)
                    // Column 1: ResRef (InventoryTableResnameItem)
                    // Column 2: Name (QTableWidgetItem with name)
                    // For Avalonia DataGrid, we'll use bound properties from InventoryTableRowItem
                    _contentsTable.AutoGenerateColumns = false;
                    _contentsTable.CanUserReorderColumns = true;
                    _contentsTable.CanUserResizeColumns = true;
                    _contentsTable.CanUserSortColumns = true;
                    _contentsTable.GridLinesVisibility = DataGridGridLinesVisibility.All;
                    _contentsTable.SelectionMode = DataGridSelectionMode.Single;

                    // Column 0: ResRef (matching PyKotor column 1 - the ResRef column)
                    _contentsTable.Columns.Add(new DataGridTextColumn
                    {
                        Header = "ResRef",
                        Binding = new Avalonia.Data.Binding("ResRefString"),
                        Width = new DataGridLength(150),
                        IsReadOnly = false
                    });

                    // Column 1: Name (matching PyKotor column 2)
                    _contentsTable.Columns.Add(new DataGridTextColumn
                    {
                        Header = "Name",
                        Binding = new Avalonia.Data.Binding("Name"),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                        IsReadOnly = true
                    });

                    // Column 2: Droppable (for non-store inventories) or Infinite (for store inventories)
                    // In PyKotor, the context menu shows either "Droppable" or "Infinite" based on is_store flag
                    // In Avalonia DataGrid, columns don't have Visibility property, so we conditionally add columns
                    if (_isStore)
                    {
                        // For stores, show Infinite column
                        var infiniteColumn = new DataGridCheckBoxColumn
                        {
                            Header = "Infinite",
                            Binding = new Avalonia.Data.Binding("Infinite"),
                            Width = new DataGridLength(100),
                            IsReadOnly = false
                        };
                        _contentsTable.Columns.Add(infiniteColumn);
                    }
                    else
                    {
                        // For non-stores, show Droppable column
                        var droppableColumn = new DataGridCheckBoxColumn
                        {
                            Header = "Droppable",
                            Binding = new Avalonia.Data.Binding("Droppable"),
                            Width = new DataGridLength(100),
                            IsReadOnly = false
                        };
                        _contentsTable.Columns.Add(droppableColumn);
                    }
                }
            }

            // Create UI wrapper for testing
            Ui = new InventoryDialogUi
            {
                ContentsTable = _contentsTable,
                // Try to find equipment tabs from XAML if they exist
                StandardEquipmentTab = this.FindControl<Control>("standardEquipmentTab"),
                NaturalEquipmentTab = this.FindControl<Control>("naturalEquipmentTab")
            };

            // Populate DataGrid with initial inventory
            PopulateInventoryTable();
        }

        // Returns (filepath, name, uti) for the given ResRef, or (None, resname, None) if not found
        private (string filepath, string name, UTI uti) GetItem(string resname)
        {
            if (_installation == null || string.IsNullOrWhiteSpace(resname))
            {
                return (null, resname, null);
            }

            try
            {
                // Try to find the UTI resource
                var resRef = ResRef.FromString(resname);
                var resourceResult = _installation.Resource(resRef.ToString(), ResourceType.UTI);

                if (resourceResult == null || resourceResult.Data == null)
                {
                    return (null, resname, null);
                }

                // Parse the UTI data
                UTI uti = UTIHelpers.ReadUti(resourceResult.Data);

                // Get the display name from the UTI
                string displayName = uti.Name?.ToString() ?? resname;

                // Return filepath, name, and UTI object
                return (resourceResult.FilePath, displayName, uti);
            }
            catch (Exception)
            {
                // If anything fails, return the resname as fallback
                return (null, resname, null);
            }
        }

        //          try:
        //              self.ui.contentsTable.add_item(str(item.resref), droppable=item.droppable, infinite=item.infinite)
        //          except FileNotFoundError:
        //              RobustLogger().error(f"{item.resref}.uti did not exist in the installation", exc_info=True)
        //          except (OSError, ValueError):
        //              RobustLogger().error(f"{item.resref}.uti is corrupted", exc_info=True)
        // Populates the contents table DataGrid with items from the initial inventory list
        private void PopulateInventoryTable()
        {
            if (_contentsTable == null || _inventory == null)
            {
                return;
            }

            var rowItems = new List<InventoryTableRowItem>();

            foreach (var item in _inventory)
            {
                try
                {
                    // Get item information (filepath, name, uti) from installation if available
                    string filePath = "";
                    string name = item.ResRef?.ToString() ?? "";
                    UTI uti = null;

                    if (_installation != null && item.ResRef != null)
                    {
                        // Use get_item method to retrieve UTI data
                        var (utiFilePath, utiName, utiObject) = GetItem(item.ResRef.ToString());
                        filePath = utiFilePath ?? "";
                        name = utiName ?? item.ResRef.ToString();
                        uti = utiObject;
                    }

                    // Create row item matching PyKotor: InventoryTableResnameItem(resname, filepath, name, droppable=droppable, infinite=infinite)
                    var rowItem = new InventoryTableRowItem(
                        item.ResRef ?? ResRef.FromBlank(),
                        filePath,
                        name,
                        item.Droppable,
                        item.Infinite);

                    rowItems.Add(rowItem);
                }
                catch (FileNotFoundException ex)
                {
                    // Log error with exception information (matching exc_info=True in PyKotor)
                    string resrefStr = item.ResRef?.ToString() ?? "unknown";
                    System.Console.WriteLine($"[ERROR] {resrefStr}.uti did not exist in the installation");
                    System.Console.WriteLine($"[ERROR] Exception details: {ex.GetType().Name}: {ex.Message}");
                    if (ex.StackTrace != null)
                    {
                        System.Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                    }
                    // Skip items that don't exist (matching PyKotor behavior - continues after logging)
                    continue;
                }
                catch (Exception ex)
                {
                    // Log error with exception information (matching exc_info=True in PyKotor)
                    string resrefStr = item.ResRef?.ToString() ?? "unknown";
                    System.Console.WriteLine($"[ERROR] {resrefStr}.uti is corrupted");
                    System.Console.WriteLine($"[ERROR] Exception details: {ex.GetType().Name}: {ex.Message}");
                    if (ex.StackTrace != null)
                    {
                        System.Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                    }
                    // Skip corrupted items (matching PyKotor behavior - continues after logging)
                    continue;
                }
            }

            // Set ItemsSource to populate the DataGrid
            _contentsTable.ItemsSource = rowItems;
        }

        public List<InventoryItem> Inventory => _inventory;
        public Dictionary<EquipmentSlot, InventoryItem> Equipment => _equipment;

        // Updates inventory and equipment from UI before dialog closes with OK
        private void Accept()
        {
            // Clear existing inventory and rebuild from contents table
            _inventory.Clear();
            if (_contentsTable != null && _contentsTable.ItemsSource != null)
            {
                //          table_item: QTableWidgetItem | None = self.ui.contentsTable.item(i, 1)
                //          if not isinstance(table_item, ItemContainer):
                //              continue
                //          self.inventory.append(InventoryItem(ResRef(table_item.resname), table_item.droppable, table_item.infinite))
                // In PyKotor, column 1 (index 1) contains the InventoryTableResnameItem which extends ItemContainer
                // and has resname, droppable, and infinite properties.
                // In Avalonia DataGrid, each item in ItemsSource represents a row, so we iterate through ItemsSource
                // and extract ResRef, Droppable, and Infinite from each row item.
                var itemsSource = _contentsTable.ItemsSource as System.Collections.IEnumerable;
                if (itemsSource != null)
                {
                    foreach (var rowItem in itemsSource.OfType<object>())
                    {
                        // Try to extract from InventoryTableRowItem (our custom row item class)
                        if (rowItem is InventoryTableRowItem tableRowItem)
                        {
                            // Extract ResRef, Droppable, and Infinite from the row item
                            if (tableRowItem.ResRef != null && !string.IsNullOrEmpty(tableRowItem.ResRef.ToString()))
                            {
                                _inventory.Add(new InventoryItem(tableRowItem.ResRef, tableRowItem.Droppable, tableRowItem.Infinite));
                            }
                        }
                        // Fallback: Try to extract using reflection for compatibility with other row item types
                        else if (rowItem != null)
                        {
                            var rowType = rowItem.GetType();
                            var resRefProperty = rowType.GetProperty("ResRef");
                            var droppableProperty = rowType.GetProperty("Droppable");
                            var infiniteProperty = rowType.GetProperty("Infinite");

                            if (resRefProperty != null)
                            {
                                var resRefValue = resRefProperty.GetValue(rowItem) as ResRef;
                                if (resRefValue != null && !string.IsNullOrEmpty(resRefValue.ToString()))
                                {
                                    bool droppable = false;
                                    bool infinite = false;

                                    if (droppableProperty != null)
                                    {
                                        var droppableValue = droppableProperty.GetValue(rowItem);
                                        if (droppableValue is bool droppableBool)
                                        {
                                            droppable = droppableBool;
                                        }
                                    }

                                    if (infiniteProperty != null)
                                    {
                                        var infiniteValue = infiniteProperty.GetValue(rowItem);
                                        if (infiniteValue is bool infiniteBool)
                                        {
                                            infinite = infiniteBool;
                                        }
                                    }

                                    _inventory.Add(new InventoryItem(resRefValue, droppable, infinite));
                                }
                            }
                        }
                    }
                }
            }

            // Clear existing equipment and rebuild from equipment frames
            _equipment.Clear();
            //          widget: DropFrame | QObject
            //          for widget in self.ui.standardEquipmentTab.children() + self.ui.naturalEquipmentTab.children():
            //              if "DropFrame" in widget.__class__.__name__ and getattr(widget, "resname", None):
            //                  casted_widget: DropFrame = cast("DropFrame", widget)
            //                  self.equipment[casted_widget.slot] = InventoryItem(ResRef(casted_widget.resname), casted_widget.droppable, casted_widget.infinite)
            ExtractEquipmentFromFrames();
        }

        // and extract equipment information (slot, resname, droppable, infinite)
        private void ExtractEquipmentFromFrames()
        {
            // Try to find equipment tabs from UI
            var equipmentTabWidgets = new List<Control>();

            // Try to find standardEquipmentTab
            var standardEquipmentTab = Ui?.StandardEquipmentTab ?? this.FindControl<Control>("standardEquipmentTab");
            if (standardEquipmentTab != null)
            {
                equipmentTabWidgets.AddRange(GetAllChildControls(standardEquipmentTab));
            }

            // Try to find naturalEquipmentTab
            var naturalEquipmentTab = Ui?.NaturalEquipmentTab ?? this.FindControl<Control>("naturalEquipmentTab");
            if (naturalEquipmentTab != null)
            {
                equipmentTabWidgets.AddRange(GetAllChildControls(naturalEquipmentTab));
            }

            // Iterate through all widgets found in equipment tabs
            foreach (var widget in equipmentTabWidgets)
            {
                // Check if widget has DropFrame-like properties using reflection
                // This works with both actual DropFrame implementations and any widget with the required properties
                var widgetType = widget.GetType();
                string typeName = widgetType.Name;

                // Check if this looks like a DropFrame (has "DropFrame" in class name or has required properties)
                bool isDropFrameLike = typeName.Contains("DropFrame") || HasDropFrameProperties(widgetType);

                if (isDropFrameLike)
                {
                    // Try to get resname property (must be non-null/non-empty to add to equipment)
                    var resnameProperty = widgetType.GetProperty("resname") ?? widgetType.GetProperty("Resname") ?? widgetType.GetProperty("ResName");
                    if (resnameProperty != null)
                    {
                        var resnameValue = resnameProperty.GetValue(widget);
                        string resname = resnameValue?.ToString() ?? "";

                        // Only add to equipment if resname is not null/empty
                        if (!string.IsNullOrEmpty(resname))
                        {
                            // Get slot property
                            var slotProperty = widgetType.GetProperty("slot") ?? widgetType.GetProperty("Slot");
                            EquipmentSlot slot = EquipmentSlot.INVALID;
                            if (slotProperty != null)
                            {
                                var slotValue = slotProperty.GetValue(widget);
                                if (slotValue is EquipmentSlot equipmentSlot)
                                {
                                    slot = equipmentSlot;
                                }
                                else if (slotValue != null)
                                {
                                    // Try to convert from int or other types
                                    if (Enum.TryParse(slotValue.ToString(), out EquipmentSlot parsedSlot))
                                    {
                                        slot = parsedSlot;
                                    }
                                }
                            }

                            // Get droppable property
                            bool droppable = false;
                            var droppableProperty = widgetType.GetProperty("droppable") ?? widgetType.GetProperty("Droppable");
                            if (droppableProperty != null)
                            {
                                var droppableValue = droppableProperty.GetValue(widget);
                                if (droppableValue is bool droppableBool)
                                {
                                    droppable = droppableBool;
                                }
                            }

                            // Get infinite property
                            bool infinite = false;
                            var infiniteProperty = widgetType.GetProperty("infinite") ?? widgetType.GetProperty("Infinite");
                            if (infiniteProperty != null)
                            {
                                var infiniteValue = infiniteProperty.GetValue(widget);
                                if (infiniteValue is bool infiniteBool)
                                {
                                    infinite = infiniteBool;
                                }
                            }

                            // Only add to equipment if slot is valid (matching PyKotor behavior)
                            if (slot != EquipmentSlot.INVALID)
                            {
                                var resRef = ResRef.FromString(resname);
                                _equipment[slot] = new InventoryItem(resRef, droppable, infinite);
                            }
                        }
                    }
                }
            }
        }

        // Helper method to check if a type has DropFrame-like properties (resname, slot, droppable, infinite)
        private bool HasDropFrameProperties(Type type)
        {
            var resnameProperty = type.GetProperty("resname") ?? type.GetProperty("Resname") ?? type.GetProperty("ResName");
            var slotProperty = type.GetProperty("slot") ?? type.GetProperty("Slot");
            var droppableProperty = type.GetProperty("droppable") ?? type.GetProperty("Droppable");
            var infiniteProperty = type.GetProperty("infinite") ?? type.GetProperty("Infinite");

            // Consider it DropFrame-like if it has at least resname and slot properties
            return resnameProperty != null && slotProperty != null;
        }

        // Helper method to recursively get all child controls from a parent control
        private List<Control> GetAllChildControls(Control parent)
        {
            var children = new List<Control>();
            if (parent == null)
            {
                return children;
            }

            // In Avalonia, controls can have children in different ways depending on the control type
            // Try to get children from common container types
            if (parent is Panel panel)
            {
                foreach (var child in panel.Children.OfType<Control>())
                {
                    children.Add(child);
                    // Recursively get children of children
                    children.AddRange(GetAllChildControls(child));
                }
            }
            else if (parent is Decorator decorator && decorator.Child is Control decoratorChild)
            {
                children.Add(decoratorChild);
                children.AddRange(GetAllChildControls(decoratorChild));
            }
            else if (parent is ContentControl contentControl && contentControl.Content is Control contentChild)
            {
                children.Add(contentChild);
                children.AddRange(GetAllChildControls(contentChild));
            }
            else
            {
                // Try to use reflection to find children property
                var childrenProperty = parent.GetType().GetProperty("Children");
                if (childrenProperty != null)
                {
                    var childrenValue = childrenProperty.GetValue(parent);
                    if (childrenValue is System.Collections.IEnumerable childrenEnumerable)
                    {
                        foreach (var child in childrenEnumerable.OfType<Control>())
                        {
                            children.Add(child);
                            children.AddRange(GetAllChildControls(child));
                        }
                    }
                }
            }

            return children;
        }

        // PyKotor's QDialog.exec() is a blocking modal dialog that returns QDialog.DialogCode.Accepted (true) or Rejected (false)
        // This synchronous method provides the same behavior for compatibility with existing code
        /// <summary>
        /// Shows the dialog modally and returns true if the user clicked OK, false if Cancel was clicked or the dialog was closed.
        /// This is a blocking synchronous method that matches PyKotor's QDialog.exec() behavior.
        /// </summary>
        /// <returns>True if OK was clicked, false if Cancel was clicked or the dialog was closed.</returns>
        public bool ShowDialog()
        {
            // Use ShowDialogAsync and block synchronously to match Qt's exec() behavior
            // This provides proper modal dialog behavior while maintaining compatibility with synchronous code
            Task<bool> dialogTask = ShowDialogAsync();
            return dialogTask.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Shows the dialog modally asynchronously and returns a Task that completes with true if the user clicked OK, false if Cancel was clicked or the dialog was closed.
        /// This is the recommended method for async/await code.
        /// </summary>
        /// <param name="parent">Optional parent window for the dialog. If null, uses the parent from constructor or finds the main window.</param>
        /// <returns>A Task that completes with true if OK was clicked, false if Cancel was clicked or the dialog was closed.</returns>
        public async Task<bool> ShowDialogAsync(Window parent = null)
        {
            // Use parent parameter if provided, otherwise use the parent from constructor
            Window dialogParent = parent ?? _parentWindow;

            // If we still don't have a parent, try to find the main window
            if (dialogParent == null)
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    dialogParent = desktop.MainWindow;
                }
            }

            if (dialogParent != null)
            {
                // ShowDialogAsync will handle setting the parent relationship
                // The result will be the value passed to Close() when the dialog closes (true for OK, false for Cancel)
                var resultObj = await ShowDialogAsync(dialogParent);
                bool result = resultObj is bool b && b == true;
                DialogResult = result;
                return result;
            }
            else
            {
                // Fallback: show non-modally and track result via Closed event
                // This should rarely happen, but provides a fallback
                bool result = false;
                void closedHandler(object s, EventArgs e)
                {
                    Closed -= closedHandler;
                    result = DialogResult;
                }

                Closed += closedHandler;
                Show();
                // Wait for dialog to close
                // Note: This is not ideal but provides fallback behavior
                while (IsVisible)
                {
                    await Task.Delay(10);
                }
                return result;
            }
        }
    }

    public class InventoryDialogUi
    {
        public DataGrid ContentsTable { get; set; }

        // These tabs contain DropFrame widgets for each equipment slot
        public Control StandardEquipmentTab { get; set; }
        public Control NaturalEquipmentTab { get; set; }
    }

    // This class represents a row item in the inventory DataGrid, containing ResRef, Droppable, and Infinite properties.
    // In PyKotor, InventoryTableResnameItem extends both ItemContainer (which has droppable and infinite) and QTableWidgetItem (which has resname).
    // In Avalonia, we use a simple class with properties that can be bound to DataGrid columns.
    public class InventoryTableRowItem : INotifyPropertyChanged
    {
        private ResRef _resRef;
        private bool _droppable;
        private bool _infinite;
        private string _name;
        private string _filePath;

        public ResRef ResRef
        {
            get => _resRef;
            set
            {
                if (_resRef != value)
                {
                    _resRef = value;
                    OnPropertyChanged(nameof(ResRef));
                    OnPropertyChanged(nameof(ResRefString));
                }
            }
        }

        // String representation of ResRef for display in DataGrid
        public string ResRefString
        {
            get => _resRef?.ToString() ?? "";
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ResRef = ResRef.FromString(value);
                }
                else
                {
                    ResRef = ResRef.FromBlank();
                }
            }
        }

        public bool Droppable
        {
            get => _droppable;
            set
            {
                if (_droppable != value)
                {
                    _droppable = value;
                    OnPropertyChanged(nameof(Droppable));
                }
            }
        }

        public bool Infinite
        {
            get => _infinite;
            set
            {
                if (_infinite != value)
                {
                    _infinite = value;
                    OnPropertyChanged(nameof(Infinite));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        // and InventoryTableResnameItem.__init__(self, resname, filepath, name, *, droppable, infinite)
        public InventoryTableRowItem(ResRef resRef, string filePath, string name, bool droppable = false, bool infinite = false)
        {
            _resRef = resRef ?? ResRef.FromBlank();
            _filePath = filePath ?? "";
            _name = name ?? "";
            _droppable = droppable;
            _infinite = infinite;
        }

        // Default constructor for XAML binding
        public InventoryTableRowItem()
        {
            _resRef = ResRef.FromBlank();
            _filePath = "";
            _name = "";
            _droppable = false;
            _infinite = false;
        }

        public void SetItem(ResRef resRef, string filePath, string name, bool droppable, bool infinite)
        {
            ResRef = resRef ?? ResRef.FromBlank();
            FilePath = filePath ?? "";
            Name = name ?? "";
            Droppable = droppable;
            Infinite = infinite;
        }

        public void ToggleDroppable()
        {
            Droppable = !Droppable;
        }

        public void ToggleInfinite()
        {
            Infinite = !Infinite;
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
