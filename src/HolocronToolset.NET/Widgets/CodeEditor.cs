using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.NET.Widgets
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/widgets/code_editor.py:80
    // Original: class CodeEditor(QPlainTextEdit):
    public class CodeEditor : TextBox
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/widgets/code_editor.py:95-121
        // Original: def __init__(self, parent: QWidget):
        public CodeEditor()
        {
            InitializeComponent();
            AcceptsReturn = true;
            AcceptsTab = true;
            TextWrapping = Avalonia.Media.TextWrapping.NoWrap;
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
                // XAML not available - will use defaults
            }
        }

        // Matching PyKotor implementation: QPlainTextEdit.toPlainText()
        // Original: Returns the plain text content
        public string ToPlainText()
        {
            return Text ?? "";
        }

        // Matching PyKotor implementation: QPlainTextEdit.setPlainText(text)
        // Original: Sets the plain text content
        public void SetPlainText(string text)
        {
            Text = text ?? "";
        }

        // Matching PyKotor implementation: QPlainTextEdit.document()
        // Original: Returns the QTextDocument
        // For Avalonia, we'll return null as TextBox doesn't have a separate document model
        public object Document()
        {
            return null;
        }
    }
}
