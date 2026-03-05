using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OdyPatch.UI.ViewModels;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyPatch.UI.Views
{
    public partial class MainWindow : Window
    {
        private ScrollViewer _logScrollViewer;
        private TextBlock _logTextBlock;
        private MainWindowViewModel _subscribedViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _logScrollViewer = this.FindControl<ScrollViewer>("LogScrollViewer");
            _logTextBlock = this.FindControl<TextBlock>("LogTextBlock");

            DataContextChanged += OnDataContextChanged;
            Opened += OnWindowOpened;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void OnWindowOpened(object sender, EventArgs e)
        {
            await ShowAlphaWarning();
        }

        private async Task ShowAlphaWarning()
        {
            MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null || !viewModel.IsAlphaVersion)
            {
                return;
            }


            await DialogHelper.ShowAsync("ALPHA VERSION WARNING", $"⚠️ WARNING: This is an ALPHA version ({Core.VersionLabel}) of OdyPatch\n\n" +
                "This version is for testing and demonstration purposes only.\n" +
                "It is NOT intended for production use.\n\n" +
                "Features may be incomplete, unstable, or contain bugs.\n" +
                "Use at your own risk.\n\n" +
                "For production use, please use the stable release.", ButtonEnum.Ok, IconType.Warning);
        }

        private void OnDataContextChanged(object sender, EventArgs e)
        {
            if (_subscribedViewModel != null)
            {
                _subscribedViewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _subscribedViewModel = null;
            }

            MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null)
            {
                return;
            }

            _subscribedViewModel = viewModel;
            _subscribedViewModel.PropertyChanged += ViewModel_PropertyChanged;
            RenderContent();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.LogText)
                || e.PropertyName == nameof(MainWindowViewModel.RtfContent)
                || e.PropertyName == nameof(MainWindowViewModel.IsRtfContent))
            {
                Dispatcher.UIThread.Post(() =>
                {
                    RenderContent();
                    _logScrollViewer?.ScrollToEnd();
                }, DispatcherPriority.Background);
            }
        }

        private void RenderContent()
        {
            MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null || _logTextBlock == null)
            {
                return;
            }

            if (viewModel.IsRtfContent && !string.IsNullOrEmpty(viewModel.RtfContent))
            {
                _logTextBlock.Text = viewModel.RtfContent;
            }
            else
            {
                _logTextBlock.Text = viewModel.LogText ?? string.Empty;
            }
        }
    }
}
