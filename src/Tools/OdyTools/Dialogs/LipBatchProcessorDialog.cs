using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using MsBox.Avalonia.Enums;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Batch WAV to LIP conversion dialog matching Holocron BatchLIPProcessor.
    /// Reference: vendor/src/toolset/gui/editors/lip/batch_processor.py
    /// </summary>
    public class LipBatchProcessorDialog : Window
    {
        private readonly List<string> _audioFiles = new List<string>();
        private string _outputDirectory;

        private ListBox _audioList;
        private TextBox _outputPathBox;
        private Button _addAudioButton;
        private Button _removeAudioButton;
        private Button _clearAudioButton;
        private Button _browseButton;
        private Button _processButton;

        public LipBatchProcessorDialog(Window parent)
        {
            _outputDirectory = null;
            Title = "Batch Process WAV to LIP";
            Width = 520;
            Height = 420;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            SetupProgrammaticUI();
        }

        private void SetupProgrammaticUI()
        {
            var root = new Grid
            {
                Margin = new Avalonia.Thickness(12),
                RowDefinitions = new RowDefinitions("Auto,*,Auto,Auto"),
                ColumnDefinitions = new ColumnDefinitions("*"),
            };

            var audioLabel = new TextBlock { Text = "Audio files (WAV):" };
            Grid.SetRow(audioLabel, 0);
            root.Children.Add(audioLabel);

            _audioList = new ListBox { MinHeight = 160 };
            Grid.SetRow(_audioList, 1);
            Grid.SetRowSpan(_audioList, 1);
            root.Children.Add(_audioList);

            var audioButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 6,
                Margin = new Avalonia.Thickness(0, 8, 0, 0),
            };
            _addAudioButton = new Button { Content = "Add..." };
            _addAudioButton.Click += async (s, e) => await AddAudioFilesAsync();
            _removeAudioButton = new Button { Content = "Remove" };
            _removeAudioButton.Click += (s, e) => RemoveSelectedAudioFiles();
            _clearAudioButton = new Button { Content = "Clear" };
            _clearAudioButton.Click += (s, e) => ClearAudioFiles();
            audioButtons.Children.Add(_addAudioButton);
            audioButtons.Children.Add(_removeAudioButton);
            audioButtons.Children.Add(_clearAudioButton);

            var outputPanel = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"),
                Margin = new Avalonia.Thickness(0, 12, 0, 0),
            };
            outputPanel.Children.Add(new TextBlock
            {
                Text = "Output directory:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
            });
            _outputPathBox = new TextBox { IsReadOnly = true, Watermark = "Select output directory..." };
            Grid.SetColumn(_outputPathBox, 1);
            outputPanel.Children.Add(_outputPathBox);
            _browseButton = new Button { Content = "Browse...", Margin = new Avalonia.Thickness(8, 0, 0, 0) };
            _browseButton.Click += async (s, e) => await BrowseOutputDirectoryAsync();
            Grid.SetColumn(_browseButton, 2);
            outputPanel.Children.Add(_browseButton);

            var bottomPanel = new StackPanel { Spacing = 8, Margin = new Avalonia.Thickness(0, 12, 0, 0) };
            bottomPanel.Children.Add(audioButtons);
            bottomPanel.Children.Add(outputPanel);

            _processButton = new Button
            {
                Content = "Process",
                HorizontalAlignment = HorizontalAlignment.Right,
                MinWidth = 100,
                Margin = new Avalonia.Thickness(0, 8, 0, 0),
            };
            _processButton.Click += async (s, e) => await ProcessFilesAsync();
            bottomPanel.Children.Add(_processButton);

            Grid.SetRow(bottomPanel, 3);
            root.Children.Add(bottomPanel);

            Content = root;
        }

        private async Task AddAudioFilesAsync()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel?.StorageProvider == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Select Audio Files",
                    AllowMultiple = true,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Audio Files") { Patterns = new[] { "*.wav" } },
                    },
                };

                if (!string.IsNullOrEmpty(_outputDirectory) && Directory.Exists(_outputDirectory))
                {
                    var folder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(_outputDirectory);
                    if (folder != null)
                    {
                        options.SuggestedStartLocation = folder;
                    }
                }

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                foreach (var file in files)
                {
                    string path = file.Path.LocalPath;
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        continue;
                    }

                    if (!_audioFiles.Contains(path, StringComparer.OrdinalIgnoreCase))
                    {
                        _audioFiles.Add(path);
                        _audioList.Items.Add(Path.GetFileName(path));
                    }
                }
            }
            catch
            {
                // User cancelled or picker failed.
            }
        }

        private void RemoveSelectedAudioFiles()
        {
            var selected = _audioList.SelectedItems;
            if (selected == null || selected.Count == 0)
            {
                return;
            }

            var indices = new List<int>();
            foreach (object item in selected)
            {
                int idx = _audioList.Items.IndexOf(item);
                if (idx >= 0)
                {
                    indices.Add(idx);
                }
            }

            indices.Sort();
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int idx = indices[i];
                if (idx >= 0 && idx < _audioFiles.Count)
                {
                    _audioFiles.RemoveAt(idx);
                    _audioList.Items.RemoveAt(idx);
                }
            }
        }

        private void ClearAudioFiles()
        {
            _audioFiles.Clear();
            _audioList.Items.Clear();
        }

        private async Task BrowseOutputDirectoryAsync()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel?.StorageProvider == null)
            {
                return;
            }

            try
            {
                var options = new FolderPickerOpenOptions
                {
                    Title = "Select Output Directory",
                    AllowMultiple = false,
                };

                string initialDirectory = _outputDirectory;
                if (string.IsNullOrEmpty(initialDirectory) && _audioFiles.Count > 0)
                {
                    initialDirectory = Path.GetDirectoryName(_audioFiles[0]);
                }

                if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
                {
                    var folder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                    if (folder != null)
                    {
                        options.SuggestedStartLocation = folder;
                    }
                }

                var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
                if (folders == null || folders.Count == 0)
                {
                    return;
                }

                _outputDirectory = folders[0].Path.LocalPath;
                _outputPathBox.Text = _outputDirectory;
            }
            catch
            {
                // User cancelled or picker failed.
            }
        }

        private async Task ProcessFilesAsync()
        {
            if (_audioFiles.Count == 0)
            {
                await DialogHelper.ShowWindowAsync(this, "Error", "No audio files selected", ButtonEnum.Ok, IconType.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_outputDirectory))
            {
                await DialogHelper.ShowWindowAsync(this, "Error", "No output directory selected", ButtonEnum.Ok, IconType.Warning);
                return;
            }

            LipBatchProcessor.LipBatchProcessResult result = LipBatchProcessor.ProcessFiles(_audioFiles, _outputDirectory);
            var errors = new List<string>();
            foreach (LipBatchProcessor.LipBatchFileResult fileResult in result.Files)
            {
                if (!fileResult.Success)
                {
                    string name = string.IsNullOrEmpty(fileResult.InputPath)
                        ? "(unknown)"
                        : Path.GetFileName(fileResult.InputPath);
                    errors.Add(name + ": " + fileResult.Error);
                }
            }

            if (errors.Count > 0)
            {
                string message = "The following errors occurred:\n\n" + string.Join("\n", errors);
                await DialogHelper.ShowWindowAsync(this, "Errors Occurred", message, ButtonEnum.Ok, IconType.Warning);
            }
            else
            {
                await DialogHelper.ShowWindowAsync(
                    this,
                    "Success",
                    "Successfully processed " + _audioFiles.Count + " files",
                    ButtonEnum.Ok,
                    IconType.Success);
            }
        }
    }
}
