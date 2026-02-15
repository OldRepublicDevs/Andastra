using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Editors;
#if !ARE_STANDALONE && !AUDIO_STANDALONE && !BWM_STANDALONE && !DLG_STANDALONE && !ERF_STANDALONE && !GFF_STANDALONE && !GIT_STANDALONE && !IFO_STANDALONE && !JRL_STANDALONE && !LIP_STANDALONE && !LTR_STANDALONE && !LYT_STANDALONE && !MDL_STANDALONE && !NSS_STANDALONE && !PTH_STANDALONE && !SAV_STANDALONE && !SSF_STANDALONE && !TPC_STANDALONE && !TLK_STANDALONE && !TXT_STANDALONE && !TWODA_STANDALONE && !UTC_STANDALONE && !UTD_STANDALONE && !UTE_STANDALONE && !UTI_STANDALONE && !UTM_STANDALONE && !UTP_STANDALONE && !UTS_STANDALONE && !UTT_STANDALONE && !UTW_STANDALONE
using OdyTools.Editors.GUI;
#endif

namespace OdyTools.Editors.Standalone.EditorStandaloneHost
{
    /// <summary>
    /// Avalonia Application for the unified editor standalone. Shows the chosen editor as the main window.
    /// </summary>
    public partial class EditorStandaloneApp : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                string key = (EditorStandaloneProgram.StartupArgs != null && EditorStandaloneProgram.StartupArgs.Length > 0)
                    ? (EditorStandaloneProgram.StartupArgs[0] ?? "").Trim().ToLowerInvariant()
                    : "2da";
                Window mainWindow = CreateEditor(key);
                if (mainWindow == null)
                    mainWindow = CreateDefaultEditor();
                desktop.MainWindow = mainWindow;
                desktop.MainWindow.Show();
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static Window CreateDefaultEditor()
        {
            OdyInstallation installation = null;
            Window parent = null;
#if TWODA_STANDALONE
            return new OdyTools.Editors.OdyToolTwoDA(parent, installation);
#elif GFF_STANDALONE
            return new OdyTools.Editors.OdyToolGFF(parent, installation);
#elif GUI_STANDALONE
            return new OdyTools.Editors.GUI.OdyToolGUI(parent, installation);
#elif TLK_STANDALONE
            return new OdyTools.Editors.OdyToolTLK(parent, installation);
#elif DLG_STANDALONE
            return new OdyTools.Editors.DLG.OdyToolDLG(parent, installation);
#elif ARE_STANDALONE
            return new OdyTools.Editors.OdyToolARE(parent, installation);
#elif AUDIO_STANDALONE
            return new OdyTools.Editors.OdyToolWAV(parent, installation);
#elif BWM_STANDALONE
            return new OdyTools.Editors.OdyToolBWM(parent, installation);
#elif ERF_STANDALONE
            return new OdyTools.Editors.OdyToolERF(parent, installation);
#elif GIT_STANDALONE
            return new OdyTools.Editors.OdyToolGIT(parent, installation);
#elif IFO_STANDALONE
            return new OdyTools.Editors.OdyToolIFO(parent, installation);
#elif JRL_STANDALONE
            return new OdyTools.Editors.OdyToolJRL(parent, installation);
#elif LIP_STANDALONE
            return new OdyTools.Editors.OdyToolLIP(parent, installation);
#elif LTR_STANDALONE
            return new OdyTools.Editors.OdyToolLTR(parent, installation);
#elif LYT_STANDALONE
            return new OdyTools.Editors.OdyToolLYT(parent, installation);
#elif MDL_STANDALONE
            return new OdyTools.Editors.OdyToolMDL(parent, installation);
#elif NSS_STANDALONE
            return new OdyTools.Editors.OdyToolNSS(parent, installation);
#elif PTH_STANDALONE
            return new OdyTools.Editors.OdyToolPTH(parent, installation);
#elif SAV_STANDALONE
            return new OdyTools.Editors.OdyToolSAV(parent, installation);
#elif SSF_STANDALONE
            return new OdyTools.Editors.OdyToolSSF(parent, installation);
#elif TPC_STANDALONE
            return new OdyTools.Editors.OdyToolTPC(parent, installation);
#elif TXT_STANDALONE
            return new OdyTools.Editors.OdyToolTXT(parent, installation);
#elif UTC_STANDALONE
            return new OdyTools.Editors.OdyToolUTC(parent, installation);
#elif UTD_STANDALONE
            return new OdyTools.Editors.OdyToolUTD(parent, installation);
#elif UTE_STANDALONE
            return new OdyTools.Editors.OdyToolUTE(parent, installation);
#elif UTI_STANDALONE
            return new OdyTools.Editors.OdyToolUTI(parent, installation);
#elif UTM_STANDALONE
            return new OdyTools.Editors.OdyToolUTM(parent, installation);
#elif UTP_STANDALONE
            return new OdyTools.Editors.OdyToolUTP(parent, installation);
#elif UTS_STANDALONE
            return new OdyTools.Editors.OdyToolUTS(parent, installation);
#elif UTT_STANDALONE
            return new OdyTools.Editors.OdyToolUTT(parent, installation);
#elif UTW_STANDALONE
            return new OdyTools.Editors.OdyToolUTW(parent, installation);
#else
            return new OdyTools.Editors.OdyToolTwoDA(parent, installation);
#endif
        }

        private static Window CreateEditor(string key)
        {
            OdyInstallation installation = null;
            Window parent = null;
#if TWODA_STANDALONE
            return new OdyTools.Editors.OdyToolTwoDA(parent, installation);
#elif GFF_STANDALONE
            return new OdyTools.Editors.OdyToolGFF(parent, installation);
#elif GUI_STANDALONE
            return new OdyTools.Editors.GUI.OdyToolGUI(parent, installation);
#elif TLK_STANDALONE
            return new OdyTools.Editors.OdyToolTLK(parent, installation);
#elif DLG_STANDALONE
            return new OdyTools.Editors.DLG.OdyToolDLG(parent, installation);
#elif ARE_STANDALONE
            return new OdyTools.Editors.OdyToolARE(parent, installation);
#elif AUDIO_STANDALONE
            return new OdyTools.Editors.OdyToolWAV(parent, installation);
#elif BWM_STANDALONE
            return new OdyTools.Editors.OdyToolBWM(parent, installation);
#elif ERF_STANDALONE
            return new OdyTools.Editors.OdyToolERF(parent, installation);
#elif GIT_STANDALONE
            return new OdyTools.Editors.OdyToolGIT(parent, installation);
#elif IFO_STANDALONE
            return new OdyTools.Editors.OdyToolIFO(parent, installation);
#elif JRL_STANDALONE
            return new OdyTools.Editors.OdyToolJRL(parent, installation);
#elif LIP_STANDALONE
            return new OdyTools.Editors.OdyToolLIP(parent, installation);
#elif LTR_STANDALONE
            return new OdyTools.Editors.OdyToolLTR(parent, installation);
#elif LYT_STANDALONE
            return new OdyTools.Editors.OdyToolLYT(parent, installation);
#elif MDL_STANDALONE
            return new OdyTools.Editors.OdyToolMDL(parent, installation);
#elif NSS_STANDALONE
            return new OdyTools.Editors.OdyToolNSS(parent, installation);
#elif PTH_STANDALONE
            return new OdyTools.Editors.OdyToolPTH(parent, installation);
#elif SAV_STANDALONE
            return new OdyTools.Editors.OdyToolSAV(parent, installation);
#elif SSF_STANDALONE
            return new OdyTools.Editors.OdyToolSSF(parent, installation);
#elif TPC_STANDALONE
            return new OdyTools.Editors.OdyToolTPC(parent, installation);
#elif TXT_STANDALONE
            return new OdyTools.Editors.OdyToolTXT(parent, installation);
#elif UTC_STANDALONE
            return new OdyTools.Editors.OdyToolUTC(parent, installation);
#elif UTD_STANDALONE
            return new OdyTools.Editors.OdyToolUTD(parent, installation);
#elif UTE_STANDALONE
            return new OdyTools.Editors.OdyToolUTE(parent, installation);
#elif UTI_STANDALONE
            return new OdyTools.Editors.OdyToolUTI(parent, installation);
#elif UTM_STANDALONE
            return new OdyTools.Editors.OdyToolUTM(parent, installation);
#elif UTP_STANDALONE
            return new OdyTools.Editors.OdyToolUTP(parent, installation);
#elif UTS_STANDALONE
            return new OdyTools.Editors.OdyToolUTS(parent, installation);
#elif UTT_STANDALONE
            return new OdyTools.Editors.OdyToolUTT(parent, installation);
#elif UTW_STANDALONE
            return new OdyTools.Editors.OdyToolUTW(parent, installation);
#else
            switch (key)
            {
                case "2da": return new OdyTools.Editors.OdyToolTwoDA(parent, installation);
                case "are": return new OdyTools.Editors.OdyToolARE(parent, installation);
                case "bwm": return new OdyTools.Editors.OdyToolBWM(parent, installation);
                case "dlg": return new OdyTools.Editors.DLG.OdyToolDLG(parent, installation);
                case "erf": return new OdyTools.Editors.OdyToolERF(parent, installation);
                case "gff": return new OdyTools.Editors.OdyToolGFF(parent, installation);
                case "git": return new OdyTools.Editors.OdyToolGIT(parent, installation);
                case "gui": return new OdyTools.Editors.GUI.OdyToolGUI(parent, installation);
                case "ifo": return new OdyTools.Editors.OdyToolIFO(parent, installation);
                case "jrl": return new OdyTools.Editors.OdyToolJRL(parent, installation);
                case "lip": return new OdyTools.Editors.OdyToolLIP(parent, installation);
                case "ltr": return new OdyTools.Editors.OdyToolLTR(parent, installation);
                case "lyt": return new OdyTools.Editors.OdyToolLYT(parent, installation);
                case "mdl": return new OdyTools.Editors.OdyToolMDL(parent, installation);
                case "nss": return new OdyTools.Editors.OdyToolNSS(parent, installation);
                case "pth": return new OdyTools.Editors.OdyToolPTH(parent, installation);
                case "sav": return new OdyTools.Editors.OdyToolSAV(parent, installation);
                case "ssf": return new OdyTools.Editors.OdyToolSSF(parent, installation);
                case "tlk": return new OdyTools.Editors.OdyToolTLK(parent, installation);
                case "tpc": return new OdyTools.Editors.OdyToolTPC(parent, installation);
                case "txt": return new OdyTools.Editors.OdyToolTXT(parent, installation);
                case "utc": return new OdyTools.Editors.OdyToolUTC(parent, installation);
                case "utd": return new OdyTools.Editors.OdyToolUTD(parent, installation);
                case "ute": return new OdyTools.Editors.OdyToolUTE(parent, installation);
                case "uti": return new OdyTools.Editors.OdyToolUTI(parent, installation);
                case "utm": return new OdyTools.Editors.OdyToolUTM(parent, installation);
                case "utp": return new OdyTools.Editors.OdyToolUTP(parent, installation);
                case "uts": return new OdyTools.Editors.OdyToolUTS(parent, installation);
                case "utt": return new OdyTools.Editors.OdyToolUTT(parent, installation);
                case "utw": return new OdyTools.Editors.OdyToolUTW(parent, installation);
                case "wav": return new OdyTools.Editors.OdyToolWAV(parent, installation);
                default: return null;
            }
#endif
        }
    }
}
