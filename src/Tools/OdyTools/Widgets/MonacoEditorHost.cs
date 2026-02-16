using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Monaco/VS Code-style editor host: line numbers margin + code editor with VS Code Dark+ theme.
    /// Wraps CodeEditor with 1:1 Monaco/VS Code visuals.
    /// </summary>
    public class MonacoEditorHost : Panel
    {
        private MonacoLineNumbersMargin _lineNumbersMargin;
        private CodeEditor _codeEditor;
        private Border _lineNumbersBorder;
        private Grid _grid;

        public CodeEditor CodeEditor => _codeEditor;

        public MonacoEditorHost()
        {
            ClipToBounds = true;
            Background = OdyTools.Themes.MonacoColors.EditorBackgroundBrush;

            _lineNumbersMargin = new MonacoLineNumbersMargin
            {
                MinWidth = 50,
                Width = 50
            };
            _lineNumbersMargin.Foreground = OdyTools.Themes.MonacoColors.LineNumbersForegroundBrush;
            _lineNumbersMargin.FontFamily = new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, 'Courier New', monospace");
            _lineNumbersMargin.FontSize = 14;

            _lineNumbersBorder = new Border
            {
                Child = _lineNumbersMargin,
                Background = OdyTools.Themes.MonacoColors.EditorBackgroundBrush,
                BorderBrush = new SolidColorBrush(OdyTools.Themes.MonacoColors.EditorIndentGuideBackground),
                BorderThickness = new Thickness(0, 0, 1, 0),
                MinWidth = 50,
                Width = 50
            };

            _codeEditor = new CodeEditor
            {
                Background = OdyTools.Themes.MonacoColors.EditorBackgroundBrush,
                Foreground = OdyTools.Themes.MonacoColors.EditorForegroundBrush,
                FontFamily = new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, 'Courier New', monospace"),
                FontSize = 14,
                Padding = new Thickness(8, 4)
            };

            _grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*"),
                Children =
                {
                    _lineNumbersBorder,
                    _codeEditor
                }
            };

            Grid.SetColumn(_lineNumbersBorder, 0);
            Grid.SetColumn(_codeEditor, 1);

            Children.Add(_grid);

            _codeEditor.TextChanged += OnCodeEditorTextChanged;
            _codeEditor.GetObservable(TextBox.SelectionStartProperty).Subscribe(_ => UpdateLineNumbersFromEditor());
            _codeEditor.GetObservable(TextBox.CaretIndexProperty).Subscribe(_ => UpdateLineNumbersFromEditor());
        }

        private void OnCodeEditorTextChanged(object sender, EventArgs e)
        {
            UpdateLineNumbersFromEditor();
        }

        private void UpdateLineNumbersFromEditor()
        {
            if (_codeEditor == null || _lineNumbersMargin == null) return;

            var text = _codeEditor.Text ?? "";
            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int lineCount = Math.Max(1, lines.Length);

            _lineNumbersMargin.LineCount = lineCount;

            var typeface = new Typeface(
                _codeEditor.FontFamily ?? FontFamily.Default,
                _codeEditor.FontStyle,
                _codeEditor.FontWeight);
            var ft = new FormattedText("A", System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, typeface, _codeEditor.FontSize, Brushes.Black);
            _lineNumbersMargin.LineHeight = ft.Height * 1.2;

            int caretLine = GetLineFromPosition(_codeEditor.CaretIndex, text);
            _lineNumbersMargin.ActiveLineNumber = caretLine;
        }

        private static int GetLineFromPosition(int position, string text)
        {
            if (string.IsNullOrEmpty(text) || position <= 0) return 1;
            if (position >= text.Length) position = text.Length;
            int line = 1;
            for (int i = 0; i < position && i < text.Length; i++)
            {
                if (text[i] == '\n' || (text[i] == '\r' && (i + 1 >= text.Length || text[i + 1] != '\n')))
                    line++;
                else if (text[i] == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                {
                    line++;
                    i++;
                }
            }
            return line;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _grid.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _grid.Measure(availableSize);
            return _grid.DesiredSize;
        }
    }
}
