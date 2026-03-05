using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace OdyPatch.UI.Views
{
    public partial class RteEditorWindow : Window
    {
        public RteEditorWindow() : this(null) { }
        public RteEditorWindow(string initialDirectory = null)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnCloseEditor(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
