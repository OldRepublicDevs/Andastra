using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;

namespace OdyTools.Editors
{
    /// <summary>
    /// ViewModel for GFF tree nodes. Supports hierarchical binding in TreeView.
    /// Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/gff.py (QStandardItem + roles).
    /// </summary>
    public class GFFTreeNodeViewModel : INotifyPropertyChanged
    {
        private string _text;
        private string _label;
        private object _value;
        private string _typeDisplay;
        private string _valueSummary;
        private string _keyDisplay;

        public event PropertyChangedEventHandler PropertyChanged;

        public GFFFieldType FieldType { get; set; }
        public int StructId { get; set; }
        /// <summary>True only for the root struct node; used for display text (e.g. "[ROOT]" vs "Struct (ID: n)").</summary>
        public bool IsRoot { get; set; }
        public ObservableCollection<GFFTreeNodeViewModel> Children { get; }

        /// <summary>PropertyListEditor parity: key column (label or "[ROOT]" or "Struct (ID: n)").</summary>
        public string KeyDisplay
        {
            get => _keyDisplay;
            set
            {
                if (_keyDisplay == value) return;
                _keyDisplay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KeyDisplay)));
            }
        }

        /// <summary>PropertyListEditor parity: type column (e.g. "Struct", "List", "String").</summary>
        public string TypeDisplay
        {
            get => _typeDisplay;
            set
            {
                if (_typeDisplay == value) return;
                _typeDisplay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeDisplay)));
            }
        }

        /// <summary>PropertyListEditor parity: value column (leaf value or "N fields" / "N items").</summary>
        public string ValueSummary
        {
            get => _valueSummary;
            set
            {
                if (_valueSummary == value) return;
                _valueSummary = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueSummary)));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
            }
        }

        public string Label
        {
            get => _label;
            set
            {
                if (_label == value) return;
                _label = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Label)));
            }
        }

        public object Value
        {
            get => _value;
            set
            {
                if (ReferenceEquals(_value, value)) return;
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public GFFTreeNodeViewModel(string text, GFFFieldType fieldType, string label, object value)
        {
            _text = text;
            _label = label;
            _value = value;
            FieldType = fieldType;
            Children = new ObservableCollection<GFFTreeNodeViewModel>();
            StructId = -1;
        }
    }
}
