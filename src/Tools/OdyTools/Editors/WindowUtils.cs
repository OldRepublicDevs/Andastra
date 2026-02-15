using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using OdyTools.Data;
using OdyTools.Editors;
#if !ARE_STANDALONE && !AUDIO_STANDALONE && !BWM_STANDALONE && !DLG_STANDALONE && !ERF_STANDALONE && !GFF_STANDALONE && !GIT_STANDALONE && !IFO_STANDALONE && !JRL_STANDALONE && !LIP_STANDALONE && !LTR_STANDALONE && !LYT_STANDALONE && !MDL_STANDALONE && !NSS_STANDALONE && !PTH_STANDALONE && !SAV_STANDALONE && !SSF_STANDALONE && !TPC_STANDALONE && !TLK_STANDALONE && !TXT_STANDALONE && !TWODA_STANDALONE && !UTC_STANDALONE && !UTD_STANDALONE && !UTE_STANDALONE && !UTI_STANDALONE && !UTM_STANDALONE && !UTP_STANDALONE && !UTS_STANDALONE && !UTT_STANDALONE && !UTW_STANDALONE
using OdyTools.Editors.DLG;
using OdyTools.Editors.GUI;
#endif
using BioWare.Common;
using BioWare.Resource;
using FileResource = BioWare.Extract.FileResource;
using JetBrains.Annotations;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:26
    // Original: TOOLSET_WINDOWS: list[QDialog | QMainWindow] = []
    public static class WindowUtils
    {
        private static readonly List<Window> ToolsetWindows = new List<Window>();
        private static readonly object UniqueSentinel = new object();

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:31-62
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:65-72
        // Original: def add_recent_file(file: Path):
        public static void AddRecentFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }

            var settings = new Settings("Global");
            var recentFiles = settings.GetValue("RecentFiles", new List<string>())
                .Where(fp => File.Exists(fp) && !string.Equals(fp, filePath, StringComparison.OrdinalIgnoreCase))
                .ToList();

            recentFiles.Insert(0, filePath);
            if (recentFiles.Count > 15)
            {
                recentFiles.RemoveAt(recentFiles.Count - 1);
            }

            settings.SetValue("RecentFiles", recentFiles);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:75-356
        // Original: def open_resource_editor(...):
        [CanBeNull]
        public static Tuple<string, Window> OpenResourceEditor(
            FileResource resource,
            OdyInstallation installation = null,
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
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:178
                // Original: QMessageBox(QMessageBox.Icon.Critical, tr("Failed to get the file data."), tr("An error occurred while attempting to read the data of the file.")).exec()
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Failed to get the file data.",
                    "An error occurred while attempting to read the data of the file.",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
                return null;
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:127-356
        // Original: def _open_resource_editor_impl(...):
        [CanBeNull]
        public static Tuple<string, Window> OpenResourceEditor(
            string filepath = null,
            string resname = null,
            ResourceType restype = null,
            byte[] data = null,
            OdyInstallation installation = null,
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

#if ARE_STANDALONE
            if (targetType == ResourceType.ARE)
                editor = new OdyToolARE(parentWindow, installation);
#elif AUDIO_STANDALONE
            if (restype.Category == "Audio")
                editor = new OdyToolWAV(parentWindow, installation);
#elif BWM_STANDALONE
            if (targetType.Category == "Walkmeshes")
                editor = new OdyToolBWM(parentWindow, installation);
#elif DLG_STANDALONE
            if (targetType == ResourceType.DLG)
                editor = new OdyTools.Editors.DLG.OdyToolDLG(parentWindow, installation);
#elif ERF_STANDALONE
            if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.RIM || restype == ResourceType.BIF)
                editor = new OdyToolERF(parentWindow, installation);
#elif GFF_STANDALONE
            if (targetType == ResourceType.GUI || targetType.Contents == "gff" || targetType == ResourceType.DLG || targetType == ResourceType.UTC || targetType == ResourceType.BTC || targetType == ResourceType.BIC
                || targetType == ResourceType.UTP || targetType == ResourceType.BTP || targetType == ResourceType.UTD || targetType == ResourceType.BTD
                || targetType == ResourceType.UTS || targetType == ResourceType.UTT || targetType == ResourceType.BTT || targetType == ResourceType.UTM || targetType == ResourceType.BTM
                || targetType == ResourceType.UTW || targetType == ResourceType.UTE || targetType == ResourceType.BTE || targetType == ResourceType.UTI || targetType == ResourceType.BTI
                || targetType == ResourceType.JRL || targetType == ResourceType.ARE || targetType == ResourceType.PTH || targetType == ResourceType.GIT)
                editor = new OdyToolGFF(parentWindow, installation);
#elif GIT_STANDALONE
            if (targetType == ResourceType.GIT)
                editor = new OdyToolGIT(parentWindow, installation);
#elif IFO_STANDALONE
            if (targetType == ResourceType.IFO)
                editor = new OdyToolIFO(parentWindow, installation);
#elif JRL_STANDALONE
            if (targetType == ResourceType.JRL)
                editor = new OdyToolJRL(parentWindow, installation);
#elif LIP_STANDALONE
            if (targetType == ResourceType.LIP)
                editor = new OdyToolLIP(parentWindow, installation);
#elif LTR_STANDALONE
            if (targetType == ResourceType.LTR)
                editor = new OdyToolLTR(parentWindow, installation);
#elif LYT_STANDALONE
            if (targetType == ResourceType.LYT)
                editor = new OdyToolLYT(parentWindow, installation);
#elif MDL_STANDALONE
            if (restype == ResourceType.MDL || restype == ResourceType.MDX)
                editor = new OdyToolMDL(parentWindow, installation);
#elif NSS_STANDALONE
            if (targetType == ResourceType.NSS || targetType == ResourceType.NCS)
            {
                if (installation == null && restype == ResourceType.NCS)
                {
                    var warningBox = MessageBoxManager.GetMessageBoxStandard(
                        "Cannot decompile NCS without an installation active",
                        "Please select an installation from the dropdown before loading an NCS.",
                        ButtonEnum.Ok,
                        Icon.Warning);
                    warningBox.ShowAsync();
                    return null;
                }
                editor = new OdyToolNSS(parentWindow, installation);
            }
#elif PTH_STANDALONE
            if (targetType == ResourceType.PTH)
                editor = new OdyToolPTH(parentWindow, installation);
#elif SAV_STANDALONE
            if (restype == ResourceType.SAV)
                editor = new OdyToolSAV(parentWindow, installation);
#elif SSF_STANDALONE
            if (targetType == ResourceType.SSF)
                editor = new OdyToolSSF(parentWindow, installation);
#elif TPC_STANDALONE
            if ((targetType.Category == "Images" || targetType.Category == "Textures") && targetType != ResourceType.TXI)
                editor = new OdyToolTPC(parentWindow, installation);
#elif TLK_STANDALONE
            if (targetType == ResourceType.TLK)
                editor = new OdyToolTLK(parentWindow, installation);
#elif TXT_STANDALONE
            if (restype.Contents == "plaintext")
                editor = new OdyToolTXT(parentWindow, installation);
#elif TWODA_STANDALONE
            if (targetType == ResourceType.TwoDA)
                editor = new OdyToolTwoDA(parentWindow, installation);
#elif UTC_STANDALONE
            if (targetType == ResourceType.UTC || targetType == ResourceType.BTC || targetType == ResourceType.BIC)
                editor = new OdyToolUTC(parentWindow, installation);
#elif UTD_STANDALONE
            if (targetType == ResourceType.UTD || targetType == ResourceType.BTD)
                editor = new OdyToolUTD(parentWindow, installation);
#elif UTE_STANDALONE
            if (targetType == ResourceType.UTE || targetType == ResourceType.BTE)
                editor = new OdyToolUTE(parentWindow, installation);
#elif UTI_STANDALONE
            if (targetType == ResourceType.UTI || targetType == ResourceType.BTI)
                editor = new OdyToolUTI(parentWindow, installation);
#elif UTM_STANDALONE
            if (targetType == ResourceType.UTM || targetType == ResourceType.BTM)
                editor = new OdyToolUTM(parentWindow, installation);
#elif UTP_STANDALONE
            if (targetType == ResourceType.UTP || targetType == ResourceType.BTP)
                editor = new OdyToolUTP(parentWindow, installation);
#elif UTS_STANDALONE
            if (targetType == ResourceType.UTS)
                editor = new OdyToolUTS(parentWindow, installation);
#elif UTT_STANDALONE
            if (targetType == ResourceType.UTT || targetType == ResourceType.BTT)
                editor = new OdyToolUTT(parentWindow, installation);
#elif UTW_STANDALONE
            if (targetType == ResourceType.UTW)
                editor = new OdyToolUTW(parentWindow, installation);
#else
            // Full OdyTools: route to appropriate editor based on resource type
            if (targetType == global::BioWare.Common.ResourceType.TwoDA)
            {
                editor = new OdyToolTwoDA(parentWindow, installation);
            }
            else if (targetType == global::BioWare.Common.ResourceType.SSF)
            {
                editor = new OdyToolSSF(parentWindow, installation);
            }
            else if (targetType == global::BioWare.Common.ResourceType.TLK)
            {
                editor = new OdyToolTLK(parentWindow, installation);
            }
            else if (targetType == global::BioWare.Common.ResourceType.LTR)
            {
                editor = new OdyToolLTR(parentWindow, installation);
            }
            else if (targetType == global::BioWare.Common.ResourceType.LIP)
            {
                editor = new OdyToolLIP(parentWindow, installation);
            }
            else if (restype.Category == "Walkmeshes")
            {
                editor = new OdyToolBWM(parentWindow, installation);
            }
            else if ((restype.Category == "Images" || restype.Category == "Textures") && restype != ResourceType.TXI)
            {
                editor = new OdyToolTPC(parentWindow, installation);
            }
            else if (targetType == ResourceType.GUI)
            {
                editor = new OdyToolGUI(parentWindow, installation);
            }
            else if (restype == ResourceType.NSS || restype == ResourceType.NCS)
            {
                if (installation == null && restype == ResourceType.NCS)
                {
                    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:215-219
                    // Original: QMessageBox.warning(parent_window_widget, tr("Cannot decompile NCS without an installation active"), tr("Please select an installation from the dropdown before loading an NCS."))
                    var warningBox = MessageBoxManager.GetMessageBoxStandard(
                        "Cannot decompile NCS without an installation active",
                        "Please select an installation from the dropdown before loading an NCS.",
                        ButtonEnum.Ok,
                        Icon.Warning);
                    warningBox.ShowAsync();
                    return null;
                }
                editor = new OdyToolNSS(parentWindow, installation);
            }
            else if (targetType == ResourceType.DLG)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyTools.Editors.DLG.OdyToolDLG(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTC || targetType == ResourceType.BTC || targetType == ResourceType.BIC)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTC(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTP || targetType == ResourceType.BTP)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTP(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTD || targetType == ResourceType.BTD)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTD(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.IFO)
            {
                editor = new OdyToolIFO(parentWindow, installation);
            }
            else if (targetType == ResourceType.UTS)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTS(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTT || targetType == ResourceType.BTT)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTT(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTM || targetType == ResourceType.BTM)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTM(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTW)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTW(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTE || targetType == ResourceType.BTE)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTE(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.UTI || targetType == ResourceType.BTI)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolUTI(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.JRL)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolJRL(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.ARE)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolARE(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.PTH)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolPTH(parentWindow, installation);
                }
            }
            else if (targetType == ResourceType.GIT)
            {
                if (installation == null || !gffSpecialized.Value)
                {
                    editor = new OdyToolGFF(parentWindow, installation);
                }
                else
                {
                    editor = new OdyToolGIT(parentWindow, installation);
                }
            }
            else if (restype.Category == "Audio")
            {
                editor = new OdyToolWAV(parentWindow, installation);
            }
            else if (restype == ResourceType.SAV)
            {
                editor = new OdyToolSAV(parentWindow, installation);
            }
            else if (restype == ResourceType.ERF || restype == ResourceType.MOD ||
                     restype == ResourceType.RIM || restype == ResourceType.BIF)
            {
                editor = new OdyToolERF(parentWindow, installation);
            }
            else if (targetType == ResourceType.LYT)
            {
                editor = new OdyToolLYT(parentWindow, installation);
            }
            else if (restype == ResourceType.MDL || restype == ResourceType.MDX)
            {
                editor = new OdyToolMDL(parentWindow, installation);
            }
            else if (targetType.Contents == "gff")
            {
                editor = new OdyToolGFF(parentWindow, installation);
            }
            else if (restype.Contents == "plaintext")
            {
                editor = new OdyToolTXT(parentWindow, installation);
            }
#endif

            if (editor == null)
            {
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:326-335
                // Original: QMessageBox(QMessageBox.Icon.Critical, tr("Failed to open file"), trf("The selected file format '{format}' is not yet supported.", format=str(restype)), ...).show()
                // Note: C# string.Format uses positional placeholders {0}, {1}, etc., so we convert the Python named placeholder {format} to {0}
                string message = string.Format("The selected file format '{0}' is not yet supported.", restype?.ToString() ?? "unknown");
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Failed to open file",
                    message,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
                return null;
            }

            try
            {
                editor.Load(filepath, resname, restype, data);
                AddWindow(editor, show: true);
                if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
                {
                    AddRecentFile(filepath);
                }
                return Tuple.Create(filepath, (Window)editor);
            }
            catch (Exception ex)
            {
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/utils/window.py:345-352
                // Original: QMessageBox(QMessageBox.Icon.Critical, tr("An unexpected error has occurred"), str(universal_simplify_exception(e)), ...).show()
                // Note: Using ex.Message for error details (similar to universal_simplify_exception in PyKotor)
                string errorMessage = ex.Message;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ex.ToString();
                }
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "An unexpected error has occurred",
                    errorMessage,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
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

        /// <summary>
        /// Gets all tracked toolset windows.
        /// Used by MiscUtils.GetTopLevel() to find an active window when MainWindow is not available.
        /// </summary>
        /// <returns>A copy of the list of tracked windows</returns>
        public static List<Window> GetTrackedWindows()
        {
            return new List<Window>(ToolsetWindows);
        }

        /// <summary>
        /// Gets the currently focused window from tracked windows, if any.
        /// </summary>
        /// <returns>The focused window, or null if none is focused</returns>
        public static Window GetFocusedWindow()
        {
            return ToolsetWindows.FirstOrDefault(w => w.IsFocused);
        }

        /// <summary>
        /// Gets the first visible window from tracked windows, if any.
        /// </summary>
        /// <returns>The first visible window, or null if none are visible</returns>
        public static Window GetVisibleWindow()
        {
            return ToolsetWindows.FirstOrDefault(w => w.IsVisible);
        }
    }
}
