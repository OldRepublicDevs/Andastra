using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace HolocronToolset.Common
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

        private class CollectionView : ICollectionView, INotifyCollectionChanged, INotifyPropertyChanged
        {
            private readonly IEnumerable _source;
            private Func<object, bool> _filter;
            private List<object> _filteredItems;

            public CollectionView(IEnumerable source)
            {
                _source = source;
                Refresh();
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
