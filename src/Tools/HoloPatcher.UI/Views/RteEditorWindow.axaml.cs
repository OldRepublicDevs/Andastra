using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using AvRichTextBox;
using HoloPatcher.UI.Rte;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using AvaloniaTextElement = Avalonia.Controls.Documents.TextElement;
using KeyEventArgs = Avalonia.Input.KeyEventArgs;
using TextRange = AvRichTextBox.TextRange;
using TextAlignment = Avalonia.Media.TextAlignment;
using TextDecorationCollection = Avalonia.Media.TextDecorationCollection;
using TextDecoration = Avalonia.Media.TextDecoration;
using FontWeight = Avalonia.Media.FontWeight;
using FontStyle = Avalonia.Media.FontStyle;
using FontFamily = Avalonia.Media.FontFamily;
using SolidColorBrush = Avalonia.Media.SolidColorBrush;
using Color = Avalonia.Media.Color;
using Colors = Avalonia.Media.Colors;

namespace HoloPatcher.UI.Views
{
    public partial class RteEditorWindow : Window
    {
        private readonly string _initialDirectory;
        private string _currentFilePath;
        private bool _isDirty;
        private RichTextBox _editor;
        private ComboBox _fontSizeComboBox;
        private ComboBox _fontFamilyComboBox;
        private ComboBox _foregroundComboBox;
        private ComboBox _backgroundComboBox;

        public RteEditorWindow(string initialDirectory = null)
        {
            InitializeComponent();
            _initialDirectory = initialDirectory;

            // Get controls from XAML
            _editor = this.FindControl<RichTextBox>("Editor");
            _fontSizeComboBox = this.FindControl<ComboBox>("FontSizeComboBox");
            _fontFamilyComboBox = this.FindControl<ComboBox>("FontFamilyComboBox");
            _foregroundComboBox = this.FindControl<ComboBox>("ForegroundComboBox");
            _backgroundComboBox = this.FindControl<ComboBox>("BackgroundComboBox");

            if (_fontSizeComboBox != null) _fontSizeComboBox.SelectedIndex = 2;
            if (_foregroundComboBox != null) _foregroundComboBox.SelectedIndex = 0;
            if (_backgroundComboBox != null) _backgroundComboBox.SelectedIndex = 0;
            if (_editor != null)
            {
                _editor.FlowDocument.Selection_Changed += OnSelectionChanged;
                _editor.AddHandler(KeyUpEvent, OnEditorKeyUp, Avalonia.Interactivity.RoutingStrategies.Bubble);
            }

            PopulateFontSelector();
            _ = InitializeNewDocumentAsync();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PopulateFontSelector()
        {
            if (_fontFamilyComboBox == null) return;

            System.Collections.Generic.IEnumerable<string> fonts = FontManager.Current.SystemFonts
                .Select(f => f.Name)
                .OrderBy(name => name, StringComparer.CurrentCultureIgnoreCase)
                .Take(30); // keep list manageable

            foreach (string font in fonts)
            {
                _fontFamilyComboBox.Items.Add(new ComboBoxItem { Content = font });
            }

            _fontFamilyComboBox.SelectedIndex = 0;
        }

        private async Task InitializeNewDocumentAsync()
        {
            if (!await ConfirmDiscardChangesAsync())
            {
                return;
            }

            if (_editor != null) _editor.FlowDocument = new FlowDocument();
            _currentFilePath = null;
            _isDirty = false;
            UpdateTitle();
        }

        private void OnNewDocument(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _ = InitializeNewDocumentAsync();
        }

        private async void OnOpenDocument(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!await ConfirmDiscardChangesAsync())
            {
                return;
            }

            var options = new FilePickerOpenOptions
            {
                Title = "Open info.rte",
                SuggestedStartLocation = await GetStartLocationAsync(),
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Rich Text Editor (*.rte)") { Patterns = new[] { "*.rte" } }
                }
            };

            System.Collections.Generic.IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(options);
            if (files.Count == 0)
            {
                return;
            }

            string path = files[0].TryGetLocalPath();
            if (path is null)
            {
                return;
            }

            string json = await File.ReadAllTextAsync(path);
            var document = RteDocument.Parse(json);
            if (_editor != null) RteDocumentConverter.ApplyToRichTextBox(_editor, document);
            _currentFilePath = path;
            _isDirty = false;
            UpdateTitle();
        }

        private async void OnSaveDocument(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            await SaveDocumentAsync(false);
        }

        private async void OnSaveDocumentAs(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            await SaveDocumentAsync(true);
        }

        private async Task SaveDocumentAsync(bool saveAs)
        {
            if (string.IsNullOrEmpty(_currentFilePath) || saveAs)
            {
                var options = new FilePickerSaveOptions
                {
                    Title = "Save info.rte",
                    SuggestedStartLocation = await GetStartLocationAsync(),
                    SuggestedFileName = string.IsNullOrEmpty(_currentFilePath) ? "info.rte" : Path.GetFileName(_currentFilePath),
                    DefaultExtension = "rte",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Rich Text Editor (*.rte)") { Patterns = new[] { "*.rte" } }
                    }
                };

                IStorageFile file = await StorageProvider.SaveFilePickerAsync(options);
                if (file is null)
                {
                    return;
                }
                _currentFilePath = file.Path.LocalPath;
            }

            if (_editor == null || _editor.FlowDocument == null) return;
            RteDocument rte = RteDocumentConverter.FromFlowDocument(_editor.FlowDocument);
            await File.WriteAllTextAsync(_currentFilePath, rte.ToJson());
            _isDirty = false;
            UpdateTitle();
        }

        private void OnCloseEditor(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close();
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            if (!await ConfirmDiscardChangesAsync())
            {
                e.Cancel = true;
                return;
            }
            base.OnClosing(e);
        }

        private async Task<bool> ConfirmDiscardChangesAsync()
        {
            if (!_isDirty)
            {
                return true;
            }

            MsBox.Avalonia.Base.IMsBox<ButtonResult> messageBox = MessageBoxManager.GetMessageBoxStandard(
                "Unsaved changes",
                "You have unsaved changes. Do you want to discard them?",
                ButtonEnum.YesNo,
                MsBox.Avalonia.Enums.Icon.Warning);

            return await messageBox.ShowAsync() == ButtonResult.Yes;
        }

        private async Task<IStorageFolder> GetStartLocationAsync()
        {
            if (!string.IsNullOrEmpty(_currentFilePath))
            {
                string folder = Path.GetDirectoryName(_currentFilePath);
                if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                {
                    return await StorageProvider.TryGetFolderFromPathAsync(folder);
                }
            }

            if (!string.IsNullOrEmpty(_initialDirectory) && Directory.Exists(_initialDirectory))
            {
                return await StorageProvider.TryGetFolderFromPathAsync(_initialDirectory);
            }

            return null;
        }

        private void UpdateTitle()
        {
            string name = string.IsNullOrEmpty(_currentFilePath) ? "Untitled" : Path.GetFileName(_currentFilePath);
            Title = _isDirty ? $"{name}* - RTE Editor" : $"{name} - RTE Editor";
        }

        private void MarkDirty()
        {
            _isDirty = true;
            UpdateTitle();
        }

        private void OnBoldClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ToggleFontWeight(FontWeight.Bold);
        }

        private void OnItalicClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ToggleFontStyle(FontStyle.Italic);
        }

        private void OnUnderlineClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ToggleTextDecoration(TextDecorationLocation.Underline);
        }

        private void OnStrikeClicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ToggleTextDecoration(TextDecorationLocation.Strikethrough);
        }

        private void ToggleFontWeight(FontWeight weight)
        {
            if (_editor == null || _editor.FlowDocument == null) return;
            TextRange selection = _editor.FlowDocument.Selection;
            FontWeight current = selection.GetFormatting(AvaloniaTextElement.FontWeightProperty) as FontWeight? ?? FontWeight.Normal;
            FontWeight newValue = current == weight ? FontWeight.Normal : weight;
            selection.ApplyFormatting(AvaloniaTextElement.FontWeightProperty, newValue);
            MarkDirty();
        }

        private void ToggleFontStyle(FontStyle style)
        {
            if (_editor == null || _editor.FlowDocument == null) return;
            TextRange selection = _editor.FlowDocument.Selection;
            FontStyle current = selection.GetFormatting(AvaloniaTextElement.FontStyleProperty) as FontStyle? ?? FontStyle.Normal;
            FontStyle newValue = current == style ? FontStyle.Normal : style;
            selection.ApplyFormatting(AvaloniaTextElement.FontStyleProperty, newValue);
            MarkDirty();
        }

        private void ToggleTextDecoration(TextDecorationLocation location)
        {
            if (_editor == null || _editor.FlowDocument == null) return;
            TextRange selection = _editor.FlowDocument.Selection;

            // Use Inline.TextDecorationsProperty directly (the correct property for text decorations)
            Avalonia.AvaloniaProperty textDecorationsProp = Avalonia.Controls.Documents.Inline.TextDecorationsProperty;

            try
            {
                var existing = selection.GetFormatting(textDecorationsProp) as TextDecorationCollection;
                var collection = new TextDecorationCollection(existing ?? new TextDecorationCollection());

                bool hasDecoration = collection.Any(dec => dec.Location == location);
                if (hasDecoration)
                {
                    collection = new TextDecorationCollection(collection.Where(dec => dec.Location != location));
                }
                else
                {
                    collection.Add(new TextDecoration { Location = location });
                }

                selection.ApplyFormatting(textDecorationsProp, collection);
                MarkDirty();
            }
            catch
            {
                // Fallback: work directly with inlines if ApplyFormatting fails
                // This handles edge cases where the formatting system might not work as expected
                ApplyTextDecorationDirectly(selection, location);
                MarkDirty();
            }
        }

        private void ApplyTextDecorationDirectly(TextRange selection, TextDecorationLocation location)
        {
            // Fallback method for applying text decorations when the main formatting approach fails.
            // Processes only the inlines that overlap with the selection range, not all inlines in selected paragraphs.
            // Get the paragraphs that contain the selection
            Paragraph startPar = selection.GetStartPar();
            Paragraph endPar = selection.GetEndPar();

            if (startPar == null || endPar == null)
            {
                return;
            }

            if (_editor == null || _editor.FlowDocument == null) return;

            int selectionStart = selection.Start;
            int selectionEnd = selection.End;

            // Get all paragraphs in the range
            var paragraphs = new List<Paragraph>();
            bool collecting = false;

            foreach (var block in _editor.FlowDocument.Blocks)
            {
                if (block is Paragraph par)
                {
                    if (par == startPar)
                    {
                        collecting = true;
                    }
                    if (collecting)
                    {
                        paragraphs.Add(par);
                        if (par == endPar)
                        {
                            break;
                        }
                    }
                }
            }

            // Calculate paragraph positions manually (Paragraph doesn't have StartInDoc property)
            var paragraphPositions = CalculateParagraphPositions(_editor.FlowDocument);

            // Process each paragraph's inlines that overlap with the selection range
            foreach (Paragraph par in paragraphs)
            {
                int paragraphStart = paragraphPositions.ContainsKey(par) ? paragraphPositions[par] : 0;

                // Process only inlines that overlap with the selection range
                foreach (IEditable inline in par.Inlines)
                {
                    // Calculate inline position within paragraph
                    int inlineOffsetInParagraph = GetInlineOffsetInParagraph(par, inline);
                    int inlineLength = GetInlineLength(inline);

                    // Calculate absolute position of inline in document
                    int absInlineStart = paragraphStart + inlineOffsetInParagraph;
                    int absInlineEnd = absInlineStart + inlineLength;

                    // Check if inline overlaps with selection range
                    // An inline overlaps if: absInlineEnd > selectionStart && absInlineStart < selectionEnd
                    bool inlineOverlapsSelection = absInlineEnd > selectionStart && absInlineStart < selectionEnd;

                    if (inlineOverlapsSelection && inline is EditableRun run)
                    {
                        // Get current text decorations
                        TextDecorationCollection currentDecs = run.TextDecorations ?? new TextDecorationCollection();
                        var newDecs = new TextDecorationCollection(currentDecs);

                        // Check if decoration at this location already exists
                        bool hasDecoration = newDecs.Any(dec => dec.Location == location);

                        if (hasDecoration)
                        {
                            // Remove decoration at this location
                            newDecs = new TextDecorationCollection(newDecs.Where(dec => dec.Location != location));
                        }
                        else
                        {
                            // Add decoration at this location
                            newDecs.Add(new TextDecoration { Location = location });
                        }

                        // Apply the new decorations
                        run.TextDecorations = newDecs.Count > 0 ? newDecs : null;
                    }
                }
            }
        }

        private void OnFontSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_fontSizeComboBox == null || _editor == null || _editor.FlowDocument == null) return;
            double size;
            var item = _fontSizeComboBox.SelectedItem as ComboBoxItem;
            if (item != null && double.TryParse(item.Content?.ToString(), out size))
            {
                _editor.FlowDocument.Selection.ApplyFormatting(AvaloniaTextElement.FontSizeProperty, size);
                MarkDirty();
            }
        }

        private void OnFontFamilyChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_fontFamilyComboBox == null || _editor == null || _editor.FlowDocument == null) return;
            var item = _fontFamilyComboBox.SelectedItem as ComboBoxItem;
            string familyName = item?.Content as string;
            if (item != null && familyName != null)
            {
                _editor.FlowDocument.Selection.ApplyFormatting(AvaloniaTextElement.FontFamilyProperty, new FontFamily(familyName));
                MarkDirty();
            }
        }

        private void OnForegroundChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_foregroundComboBox == null || _editor == null || _editor.FlowDocument == null) return;
            var item = _foregroundComboBox.SelectedItem as ComboBoxItem;
            string name = item?.Content as string;
            if (item != null && name != null)
            {
                var brush = new SolidColorBrush(ColorFromName(name));
                _editor.FlowDocument.Selection.ApplyFormatting(AvaloniaTextElement.ForegroundProperty, brush);
                MarkDirty();
            }
        }

        private void OnBackgroundChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_backgroundComboBox == null || _editor == null || _editor.FlowDocument == null) return;
            var item = _backgroundComboBox.SelectedItem as ComboBoxItem;
            string name = item?.Content as string;
            if (item != null && name != null)
            {
                IBrush brush = name.Equals("Transparent", StringComparison.OrdinalIgnoreCase)
                    ? (IBrush)Brushes.Transparent
                    : new SolidColorBrush(ColorFromName(name));
                _editor.FlowDocument.Selection.ApplyFormatting(AvaloniaTextElement.BackgroundProperty, brush);
                MarkDirty();
            }
        }

        private static Color ColorFromName(string name)
        {
            switch (name.ToLowerInvariant())
            {
                case "red": return Colors.Red;
                case "green": return Colors.Green;
                case "blue": return Colors.Blue;
                case "gray": return Colors.Gray;
                case "yellow": return Colors.Yellow;
                case "lightblue": return Colors.LightBlue;
                case "lightgreen": return Colors.LightGreen;
                default: return Colors.Black;
            }
        }

        private void OnAlignLeft(object sender, Avalonia.Interactivity.RoutedEventArgs e) { ApplyAlignment(TextAlignment.Left); }

        private void OnAlignCenter(object sender, Avalonia.Interactivity.RoutedEventArgs e) { ApplyAlignment(TextAlignment.Center); }

        private void OnAlignRight(object sender, Avalonia.Interactivity.RoutedEventArgs e) { ApplyAlignment(TextAlignment.Right); }

        private void ApplyAlignment(TextAlignment alignment)
        {
            if (_editor == null || _editor.FlowDocument == null) return;
            foreach (Paragraph paragraph in _editor.FlowDocument.GetSelectedParagraphs)
            {
                paragraph.TextAlignment = alignment;
            }
            MarkDirty();
        }

        private void OnSelectionChanged(TextRange range)
        {
            // Update toolbar state to reflect current selection
            if (range is null)
            {
                return;
            }

            var weight = range.GetFormatting(AvaloniaTextElement.FontWeightProperty) as FontWeight?;
            var style = range.GetFormatting(AvaloniaTextElement.FontStyleProperty) as FontStyle?;
            // TextDecorationsProperty may not be available
            TextDecorationCollection decorations = null;
            try
            {
                Type textElementType = typeof(AvaloniaTextElement);
                System.Reflection.PropertyInfo prop = textElementType.GetProperty("TextDecorationsProperty", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (prop != null)
                {
                    var textDecorationsProp = prop.GetValue(null) as Avalonia.AvaloniaProperty;
                    if (textDecorationsProp != null)
                    {
                        decorations = range.GetFormatting(textDecorationsProp) as TextDecorationCollection;
                    }
                }
            }
            catch
            {
                // Property not available
            }

            // Update toggle buttons appearance if desired in future.
        }

        private void OnEditorKeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Handled)
            {
                MarkDirty();
            }
        }

        // Helper methods to calculate paragraph and inline positions (Paragraph doesn't have StartInDoc property)
        private static Dictionary<Paragraph, int> CalculateParagraphPositions(FlowDocument document)
        {
            var positions = new Dictionary<Paragraph, int>();
            int currentOffset = 0;

            foreach (Block block in document.Blocks)
            {
                if (block is Paragraph paragraph)
                {
                    positions[paragraph] = currentOffset;
                    currentOffset += GetParagraphTextLength(paragraph);
                }
            }

            return positions;
        }

        private static int GetParagraphTextLength(Paragraph paragraph)
        {
            int length = 0;
            foreach (IEditable inline in paragraph.Inlines)
            {
                length += GetInlineLength(inline);
            }
            return length;
        }

        private static int GetInlineOffsetInParagraph(Paragraph paragraph, IEditable targetInline)
        {
            int offset = 0;
            foreach (IEditable inline in paragraph.Inlines)
            {
                if (inline == targetInline)
                {
                    break;
                }
                offset += GetInlineLength(inline);
            }
            return offset;
        }

        private static int GetInlineLength(IEditable inline)
        {
            if (inline is EditableRun run)
            {
                return run.InlineLength;
            }
            else if (inline is Avalonia.Controls.Documents.Run avRun)
            {
                return avRun.Text?.Length ?? 0;
            }
            return 0;
        }
    }
}

