using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OdyTools.Common
{
    // Simple CollectionViewSource implementation for Avalonia
    // Provides filtering functionality similar to WPF's CollectionViewSource
    public class CollectionViewSource : INotifyPropertyChanged
    {
        private IEnumerable _source;
        private ICollectionView _view;

        public CollectionViewSource()
        {
        }

        public IEnumerable Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    _view = new CollectionView(_source);
                    OnPropertyChanged(nameof(Source));
                    OnPropertyChanged(nameof(View));
                }
            }
        }

        public ICollectionView View => _view ?? (_view = new CollectionView(_source));

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private class CollectionView : ICollectionView, INotifyCollectionChanged, INotifyPropertyChanged, IList
        {
            private readonly IEnumerable _source;
            private Func<object, bool> _filter;
            private List<object> _filteredItems;

            public CollectionView(IEnumerable source)
            {
                _source = source;
                if (_source is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += OnSourceCollectionChanged;
                }
                Refresh();
            }

            private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                // Only do a full refresh (Reset) when filtering is active; otherwise forward the
                // original event so the DataGrid can handle it incrementally without clearing selection.
                if (_filter != null)
                {
                    Refresh();
                }
                else
                {
                    // Update our snapshot to match the source
                    if (_source != null)
                    {
                        _filteredItems = _source.Cast<object>().ToList();
                    }
                    // Forward the original event so DataGrid does incremental update
                    OnCollectionChanged(e);
                }
            }

            public Func<object, bool> Filter
            {
                get => _filter;
                set
                {
                    _filter = value;
                    Refresh();
                }
            }

            public void Refresh()
            {
                if (_source == null)
                {
                    _filteredItems = new List<object>();
                }
                else
                {
                    var items = _source.Cast<object>();
                    if (_filter != null)
                    {
                        items = items.Where(_filter);
                    }
                    _filteredItems = items.ToList();
                }

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            public IEnumerator GetEnumerator() => _filteredItems.GetEnumerator();

            // IList implementation - required by Avalonia DataGrid for item validation
            public bool IsFixedSize => false;
            public bool IsReadOnly => true;
            public int Count => _filteredItems.Count;
            public bool IsSynchronized => false;
            public object SyncRoot => _filteredItems;

            public object this[int index]
            {
                get => _filteredItems[index];
                set => throw new NotSupportedException();
            }

            public int Add(object value) => throw new NotSupportedException();
            public void Clear() => throw new NotSupportedException();
            public bool Contains(object value) => _filteredItems.Contains(value);
            public int IndexOf(object value) => _filteredItems.IndexOf(value);
            public void Insert(int index, object value) => throw new NotSupportedException();
            public void Remove(object value) => throw new NotSupportedException();
            public void RemoveAt(int index) => throw new NotSupportedException();
            public void CopyTo(Array array, int index) => ((ICollection)_filteredItems).CopyTo(array, index);

            public event NotifyCollectionChangedEventHandler CollectionChanged;
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                CollectionChanged?.Invoke(this, e);
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public interface ICollectionView : IEnumerable, INotifyCollectionChanged
    {
        Func<object, bool> Filter { get; set; }
        void Refresh();
    }
}
