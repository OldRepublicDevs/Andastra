using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using HolocronToolset.Data;
using HolocronToolset.Editors;
using Andastra.Formats.Resources;
using FileResource = Andastra.Formats.Resources.FileResource;
using JetBrains.Annotations;
using NSSEditor = HolocronToolset.Editors.NSSEditor;

namespace HolocronToolset.Utils
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/window.py:26
    // Original: TOOLSET_WINDOWS: list[QDialog | QMainWindow] = []
    public static class WindowUtils
    {
        private static readonly List<Window> ToolsetWindows = new List<Window>();
        private static readonly object UniqueSentinel = new object();

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/window.py:31-62
        // Original: def add_window(window: QDialog | QMainWindow, *, show: bool = True):
        public static void AddWindow(Window window, bool show = true)
        {
            if (window == null)
            {
                return;
            }

            // Store original closing handler
            window.Closing += (sender, e) =>
            {
                if (sender is Window w && ToolsetWindows.Contains(w))
                {
                    ToolsetWindows.Remove(w);
                }
            };

            if (show)
            {
                window.Show();
            }
            ToolsetWindows.Add(window);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/window.py:65-72
        // Original: def add_recent_file(file: Path):
        public static void AddRecentFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }

            var settings = new Settings("Global");
            var recentFiles = settings.GetValue<List<string>>("RecentFiles", new List<string>())
                .Where(fp => File.Exists(fp))
                .ToList();

            recentFiles.Insert(0, filePath);
            if (recentFiles.Count > 15)
            {
                recentFiles.RemoveAt(recentFiles.Count - 1);
            }

            settings.SetValue("RecentFiles", recentFiles);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/window.py:75-356
        // Original: def open_resource_editor(...):
        [CanBeNull]
        public static Tuple<string, Window> OpenResourceEditor(
            FileResource resource,
            HTInstallation installation = null,
            Window parentWindow = null,
            bool? gffSpecialized = null)
        {
            if (resource == null)
            {
                return null;
            }

            try
            {
                byte[] data = resource.GetData();
                return OpenResourceEditor(
                    resource.FilePath,
                    resource.ResName,
                    resource.ResType,
                    data,
                    installation,
                    parentWindow,
                    gffSpecialized);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error getting resource data: {ex}");
                // TODO: Show MessageBox when MessageBox.Avalonia is available
                return null;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/window.py:127-356
        // Original: def _open_resource_editor_impl(...):
        [CanBeNull]
        public static Tuple<string, Window> OpenResourceEditor(
            string filepath = null,
            string resname = null,
            Andastra.Formats.Resources.ResourceType restype = null,
            byte[] data = null,
            HTInstallation installation = null,
            Window parentWindow = null,
            bool? gffSpecialized = null)
        {
            if (restype == null)
            {
                return null;
            }

            // Get GFF specialized setting if not provided
            if (gffSpecialized == null)
            {
                var settings = new GlobalSettings();
                gffSpecialized = settings.GetGffSpecializedEditors();
            }

            Editor editor = null;
            var targetType = restype.TargetType();

            // Route to appropriate editor based on resource type
            if (targetType == Andastra.Formats.Resources.ResourceType.TwoDA)
            {
                editor = new TwoDAEditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.SSF)
            {
                editor = new SSFEditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.TLK)
            {
                editor = new TLKEditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.LTR)
            {
                editor = new LTREditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.LIP)
            {
                editor = new LIPEditor(parentWindow, installation);
            }
            else if (restype.Category == "Walkmeshes")
            {
                editor = new BWMEditor(parentWindow, installation);
            }
            else if ((restype.Category == "Images" || restype.Category == "Textures") && restype != Andastra.Formats.Resources.ResourceType.TXI)
            {
                editor = new TPCEditor(parentWindow, installation);
            }
            else if (restype == Andastra.Formats.Resources.ResourceType.NSS || restype == Andastra.Formats.Resources.ResourceType.NCS)
            {
                if (installation == null && restype == Andastra.Formats.Resources.ResourceType.NCS)
                {
                    // Show warning for NCS without installation
                    // TODO: Show MessageBox when MessageBox.Avalonia is available
                    return null;
                }
                editor = new NSSEditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.DLG)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new DLGEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTC || targetType == Andastra.Formats.Resources.ResourceType.BTC || targetType == Andastra.Formats.Resources.ResourceType.BIC)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTCEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTP || targetType == Andastra.Formats.Resources.ResourceType.BTP)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTPEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTD || targetType == Andastra.Formats.Resources.ResourceType.BTD)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTDEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.IFO)
            {
                editor = new IFOEditor(parentWindow, installation);
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTS)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTSEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTT || targetType == Andastra.Formats.Resources.ResourceType.BTT)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTTEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTM || targetType == Andastra.Formats.Resources.ResourceType.BTM)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTMEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTW)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTWEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTE || targetType == Andastra.Formats.Resources.ResourceType.BTE)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTEEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.UTI || targetType == Andastra.Formats.Resources.ResourceType.BTI)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new UTIEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.JRL)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new JRLEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.ARE)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new AREEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.PTH)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new PTHEditor(parentWindow, installation);
                }
            }
            else if (targetType == Andastra.Formats.Resources.ResourceType.GIT)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new GFFEditor(parentWindow, installation);
                }
                else
                {
                    editor = new GITEditor(parentWindow, installation);
                }
            }
            else if (restype.Category == "Audio")
            {
                editor = new WAVEditor(parentWindow, installation);
            }
            else if (restype == Andastra.Formats.Resources.ResourceType.ERF || restype == Andastra.Formats.Resources.ResourceType.SAV ||
                     restype == Andastra.Formats.Resources.ResourceType.MOD || restype == Andastra.Formats.Resources.ResourceType.RIM ||
                     restype == Andastra.Formats.Resources.ResourceType.BIF)
            {
                editor = new ERFEditor(parentWindow, installation);
            }
            else if (restype == Andastra.Formats.Resources.ResourceType.MDL || restype == Andastra.Formats.Resources.ResourceType.MDX)
            {
                editor = new MDLEditor(parentWindow, installation);
            }
            else if (targetType.Contents == "gff")
            {
                editor = new GFFEditor(parentWindow, installation);
            }
            else if (restype.Contents == "plaintext")
            {
                editor = new TXTEditor(parentWindow, installation);
            }

            if (editor == null)
            {
                // TODO: Show error message when MessageBox.Avalonia is available
                return null;
            }

            try
            {
                editor.Load(filepath, resname, restype, data);
                AddWindow(editor, show: true);
                return Tuple.Create(filepath, (Window)editor);
            }
            catch (Exception ex)
            {
                // TODO: Show error message when MessageBox.Avalonia is available
                System.Console.WriteLine($"Error loading resource: {ex}");
                return null;
            }
        }

        public static void CloseAllWindows()
        {
            var windows = new List<Window>(ToolsetWindows);
            foreach (var window in windows)
            {
                try
                {
                    window.Close();
                }
                catch
                {
                    // Ignore errors when closing
                }
            }
            ToolsetWindows.Clear();
        }

        public static int WindowCount => ToolsetWindows.Count;
    }
}
