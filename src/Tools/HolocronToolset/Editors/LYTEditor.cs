using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Andastra.Parsing;
using Andastra.Parsing.Formats.LYT;
using Andastra.Parsing.Formats.TPC;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Tools;
using HolocronToolset.Data;

namespace HolocronToolset.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:29
    // Original: class LYTEditor(Editor):
    public partial class LYTEditor : Editor
    {
        private LYT _lyt;
        private LYTEditorSettings _settings;
        private Dictionary<string, string> _importedTextures = new Dictionary<string, string>(); // Maps texture name to file path

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:32-73
        // Original: def __init__(self, parent, installation):
        public LYTEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "LYT Editor", "lyt",
                new[] { ResourceType.LYT },
                new[] { ResourceType.LYT },
                installation)
        {
            _lyt = new LYT();
            _settings = new LYTEditorSettings();

            InitializeComponent();
            SetupUI();
            New();
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
            var panel = new StackPanel();
            Content = panel;
        }

        private void SetupUI()
        {
            // UI setup - will be implemented when XAML is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:127-131
        // Original: def add_room(self):
        public void AddRoom()
        {
            var room = new LYTRoom("default_room", new Vector3(0, 0, 0));
            _lyt.Rooms.Add(room);
            UpdateScene();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:133-150
        // Original: def add_track(self):
        public void AddTrack()
        {
            if (_lyt.Rooms.Count < 2)
            {
                return;
            }

            var track = new LYTTrack("default_track", new Vector3(0, 0, 0));

            // Find path through connected rooms
            var startRoom = _lyt.Rooms[0];
            var endRoom = _lyt.Rooms.Count > 1 ? _lyt.Rooms[1] : startRoom;
            var path = FindPath(startRoom, endRoom);

            if (path != null && path.Count > 0)
            {
                _lyt.Tracks.Add(track);
            }

            UpdateScene();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:152-179
        // Original: def find_path(self, start: LYTRoom, end: LYTRoom) -> list[LYTRoom] | None:
        public List<LYTRoom> FindPath(LYTRoom start, LYTRoom end)
        {
            if (start == null || end == null)
            {
                return null;
            }

            if (start.Equals(end))
            {
                return new List<LYTRoom> { start };
            }

            // Simple pathfinding - check if rooms are connected
            if (start.Connections.Contains(end))
            {
                return new List<LYTRoom> { start, end };
            }

            // A* pathfinding implementation
            var queue = new List<Tuple<float, LYTRoom, List<LYTRoom>>>
            {
                Tuple.Create(0f, start, new List<LYTRoom> { start })
            };
            var visited = new HashSet<LYTRoom> { start };

            while (queue.Count > 0)
            {
                queue.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                var current = queue[0];
                queue.RemoveAt(0);

                var (_, currentRoom, path) = current;

                if (currentRoom.Equals(end))
                {
                    return path;
                }

                foreach (var nextRoom in currentRoom.Connections)
                {
                    if (visited.Contains(nextRoom))
                    {
                        continue;
                    }

                    visited.Add(nextRoom);
                    var newPath = new List<LYTRoom>(path) { nextRoom };
                    var priority = newPath.Count + (nextRoom.Position - end.Position).Length();
                    queue.Add(Tuple.Create(priority, nextRoom, newPath));
                }
            }

            return null;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:181-184
        // Original: def add_obstacle(self):
        public void AddObstacle()
        {
            var obstacle = new LYTObstacle("default_obstacle", new Vector3(0, 0, 0));
            _lyt.Obstacles.Add(obstacle);
            UpdateScene();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:186-210
        // Original: def add_door_hook(self):
        public void AddDoorHook()
        {
            if (_lyt.Rooms.Count == 0)
            {
                return;
            }

            var firstRoom = _lyt.Rooms[0];

            var doorhook = new LYTDoorHook(
                firstRoom.Model,
                "",
                new Vector3(0, 0, 0),
                new Vector4(0, 0, 0, 1)
            );

            _lyt.Doorhooks.Add(doorhook);
            UpdateScene();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:212-214
        // Original: def generate_walkmesh(self):
        public void GenerateWalkmesh()
        {
            // Implement walkmesh generation logic here
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:216-218
        // Original: def update_zoom(self, value: int):
        public void UpdateZoom(int value)
        {
            // Zoom functionality - will be implemented when graphics view is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:220-229
        // Original: def update_scene(self):
        public void UpdateScene()
        {
            // Scene update - will be implemented when graphics scene is available
            // For now, just ensure LYT data is consistent
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:231-235
        // Original: def import_texture(self):
        public async void ImportTexture()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Import Texture",
                    AllowMultiple = true,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Image Files")
                        {
                            Patterns = new[] { "*.tpc", "*.tga", "*.dds", "*.png", "*.jpg", "*.jpeg", "*.bmp" },
                            MimeTypes = new[] { "image/tga", "image/dds", "image/png", "image/jpeg", "image/bmp" }
                        },
                        new FilePickerFileType("TPC Files") { Patterns = new[] { "*.tpc" } },
                        new FilePickerFileType("TGA Files") { Patterns = new[] { "*.tga" } },
                        new FilePickerFileType("DDS Files") { Patterns = new[] { "*.dds" } },
                        new FilePickerFileType("PNG Files") { Patterns = new[] { "*.png" } },
                        new FilePickerFileType("JPEG Files") { Patterns = new[] { "*.jpg", "*.jpeg" } },
                        new FilePickerFileType("BMP Files") { Patterns = new[] { "*.bmp" } },
                        new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                    }
                };

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                foreach (var file in files)
                {
                    string filePath = file.Path.LocalPath;
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        continue;
                    }

                    await ImportTextureFile(filePath);
                }

                UpdateTextureBrowser();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing texture: {ex}");
            }
        }

        private async Task ImportTextureFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    System.Console.WriteLine($"Error: Texture file does not exist: {filePath}");
                    return;
                }

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                string targetResref = fileName;

                // Validate file extension
                string[] supportedExtensions = { ".tpc", ".tga", ".dds", ".png", ".jpg", ".jpeg", ".bmp" };
                bool isSupported = false;
                foreach (string ext in supportedExtensions)
                {
                    if (extension == ext)
                    {
                        isSupported = true;
                        break;
                    }
                }

                if (!isSupported)
                {
                    System.Console.WriteLine($"Error: Unsupported texture format: {extension}. Supported formats: TPC, TGA, DDS, PNG, JPG, BMP");
                    return;
                }

                // Determine if we need to convert the texture
                bool needsConversion = extension != ".tpc" && extension != ".tga" && extension != ".dds";

                string overridePath = GetOverrideDirectory();
                if (string.IsNullOrEmpty(overridePath))
                {
                    System.Console.WriteLine("Warning: Could not determine override directory. Texture will not be saved to installation.");
                    return;
                }

                // Ensure override/textures directory exists
                string texturesPath = Path.Combine(overridePath, "textures");
                if (!Directory.Exists(texturesPath))
                {
                    try
                    {
                        Directory.CreateDirectory(texturesPath);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Could not create textures directory at {texturesPath}: {ex}");
                        return;
                    }
                }

                string outputTpcPath = Path.Combine(texturesPath, $"{targetResref}.tpc");
                string txiPath = Path.ChangeExtension(filePath, ".txi");

                TPC tpc = null;

                // Read the texture based on its format
                // TPCAuto.ReadTpc can directly handle TPC, TGA, and DDS formats
                if (extension == ".tpc" || extension == ".tga" || extension == ".dds")
                {
                    try
                    {
                        // TPC, TGA, and DDS can be read directly by TPCAuto
                        tpc = TPCAuto.ReadTpc(filePath, txiSource: File.Exists(txiPath) ? txiPath : null);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Failed to read {extension.ToUpperInvariant()} file {filePath}: {ex}");
                        return;
                    }
                }
                else if (needsConversion)
                {
                    // PNG, JPG, BMP formats require conversion to TGA/TPC first
                    // TPCAuto does not directly support these formats
                    // In a full implementation, we would:
                    // 1. Load the image using an image library (e.g., System.Drawing, SkiaSharp, or ImageSharp)
                    // 2. Convert to RGBA format
                    // 3. Create a TPC object from the image data
                    // 4. Write as TPC
                    // 
                    // For now, we provide a clear error message indicating this limitation
                    System.Console.WriteLine($"Error: Direct import of {extension.ToUpperInvariant()} files is not yet supported.");
                    System.Console.WriteLine($"Please convert {Path.GetFileName(filePath)} to TGA or TPC format first, then import.");
                    System.Console.WriteLine($"You can use external tools to convert PNG/JPG/BMP to TGA, then import the TGA file.");
                    return;
                }

                if (tpc == null)
                {
                    System.Console.WriteLine($"Failed to load texture from {filePath}");
                    return;
                }

                // Check if output file already exists and handle overwrite
                if (File.Exists(outputTpcPath))
                {
                    System.Console.WriteLine($"Warning: Texture {targetResref}.tpc already exists in override directory. It will be overwritten.");
                }

                // Write as TPC to override directory
                try
                {
                    TPCAuto.WriteTpc(tpc, outputTpcPath, ResourceType.TPC);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error: Failed to write TPC file to {outputTpcPath}: {ex}");
                    return;
                }

                // Write TXI file if it exists
                if (!string.IsNullOrEmpty(tpc.Txi))
                {
                    try
                    {
                        string outputTxiPath = Path.ChangeExtension(outputTpcPath, ".txi");
                        File.WriteAllText(outputTxiPath, tpc.Txi, System.Text.Encoding.ASCII);
                        System.Console.WriteLine($"Also wrote TXI file: {Path.GetFileName(outputTxiPath)}");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Warning: Failed to write TXI file: {ex}");
                    }
                }

                // Store the imported texture reference
                _importedTextures[targetResref] = outputTpcPath;

                System.Console.WriteLine($"Successfully imported texture: {targetResref} -> {outputTpcPath}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing texture file {filePath}: {ex}");
            }
        }

        private string GetOverrideDirectory()
        {
            if (_installation == null)
            {
                return null;
            }

            try
            {
                string installPath = _installation.Path;
                if (string.IsNullOrEmpty(installPath) || !Directory.Exists(installPath))
                {
                    return null;
                }

                // Standard KOTOR override directory is at <installPath>/override
                string overridePath = Path.Combine(installPath, "override");
                return overridePath;
            }
            catch
            {
                return null;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:237-241
        // Original: def import_model(self):
        public void ImportModel()
        {
            // TODO: Implement model import logic
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:243-245
        // Original: def update_texture_browser(self):
        public void UpdateTextureBrowser()
        {
            // Update texture browser UI with imported textures
            // This method should refresh any texture browser widget in the UI
            // For now, we'll ensure the imported textures list is maintained
            
            // If there's a texture browser widget, it should be updated here
            // The actual UI update will depend on the specific texture browser implementation
            // This is a placeholder for the UI update logic
            
            System.Console.WriteLine($"Texture browser updated. {_importedTextures.Count} texture(s) available.");
            foreach (var kvp in _importedTextures)
            {
                System.Console.WriteLine($"  - {kvp.Key}: {kvp.Value}");
            }
        }

        public List<string> GetImportedTextures()
        {
            return new List<string>(_importedTextures.Keys);
        }

        public string GetImportedTexturePath(string textureName)
        {
            return _importedTextures.TryGetValue(textureName, out string path) ? path : null;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:247-264
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            try
            {
                _lyt = LYTAuto.ReadLyt(data);
                LoadLYT(_lyt);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load LYT: {ex}");
                New();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:220-229
        // Original: def update_scene(self):
        private void LoadLYT(LYT lyt)
        {
            _lyt = lyt;
            UpdateScene();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py:266-267
        // Original: def build(self) -> tuple[bytes, ResourceType]:
        public override Tuple<byte[], byte[]> Build()
        {
            byte[] data = LYTAuto.BytesLyt(_lyt);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/lyt.py
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _lyt = new LYT();
            UpdateScene();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
