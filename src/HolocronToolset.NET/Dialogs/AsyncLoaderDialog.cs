using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/asyncloader.py:112
    // Original: class AsyncLoader(QDialog, Generic[T]):
    public class AsyncLoaderDialog : Window
    {
        private string _title;
        private Func<object> _task;
        private string _errorTitle;
        private object _result;
        private Exception _error;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/asyncloader.py:116-184
        // Original: def __init__(self, parent, title, task, error_title=None, ...):
        public AsyncLoaderDialog(Window parent = null, string title = "Loading...", Func<object> task = null, string errorTitle = null)
        {
            InitializeComponent();
            _title = title;
            _task = task;
            _errorTitle = errorTitle ?? "Error";
            _result = null;
            _error = null;
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
            Title = _title;
            Width = 300;
            Height = 100;

            var panel = new StackPanel();
            var statusLabel = new TextBlock
            {
                Text = "Loading...",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(statusLabel);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/asyncloader.py:359-400
        // Original: def exec(self) -> bool:
        public bool Exec()
        {
            try
            {
                if (_task != null)
                {
                    _result = _task();
                }
                Close();
                return true;
            }
            catch (Exception ex)
            {
                _error = ex;
                // Show error - will be implemented when MessageBox is available
                System.Console.WriteLine($"Error: {_errorTitle}: {ex}");
                Close();
                return false;
            }
        }

        public object Result => _result;
        public Exception Error => _error;
    }
}
