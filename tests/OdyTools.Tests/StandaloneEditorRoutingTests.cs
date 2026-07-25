using BioWare.Common;
using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OdyTools.Editors;

namespace OdyTools.Tests
{
    public class StandaloneEditorRoutingTests
    {
        [TestCase("sample.2da", "2da")]
        [TestCase("sample.2da.csv", "2da")]
        [TestCase("sample.2da.json", "2da")]
        [TestCase("sample.are", "are")]
        [TestCase("sample.are.xml", "are")]
        [TestCase("sample.wok", "bwm")]
        [TestCase("sample.dwk", "bwm")]
        [TestCase("sample.pwk", "bwm")]
        [TestCase("sample.bwm", "bwm")]
        [TestCase("sample.dlg", "dlg")]
        [TestCase("sample.dlg.xml", "dlg")]
        [TestCase("sample.dlg.json", "dlg")]
        [TestCase("sample.twine.html", "dlg")]
        [TestCase("sample.twine.json", "dlg")]
        [TestCase("sample.cnv", "dlg")]
        [TestCase("sample.mod", "erf")]
        [TestCase("sample.rim", "erf")]
        [TestCase("sample.bif", "erf")]
        [TestCase("sample.hak", "erf")]
        [TestCase("sample.fac", "fac")]
        [TestCase("sample.fac.xml", "fac")]
        [TestCase("sample.gff", "gff")]
        [TestCase("sample.gff.xml", "gff")]
        [TestCase("sample.gff.json", "gff")]
        [TestCase("sample.res", "gff")]
        [TestCase("sample.res.xml", "gff")]
        [TestCase("sample.gam", "gff")]
        [TestCase("sample.gui", "gui")]
        [TestCase("sample.git", "git")]
        [TestCase("sample.git.xml", "git")]
        [TestCase("sample.ifo", "ifo")]
        [TestCase("sample.ifo.xml", "ifo")]
        [TestCase("sample.jrl", "jrl")]
        [TestCase("sample.lip", "lip")]
        [TestCase("sample.lip.xml", "lip")]
        [TestCase("sample.lip.json", "lip")]
        [TestCase("sample.ltr", "ltr")]
        [TestCase("sample.lyt", "lyt")]
        [TestCase("sample.mdl", "mdl")]
        [TestCase("sample.mdl.ascii", "mdl")]
        [TestCase("sample.mdx", "mdl")]
        [TestCase("sample.nss", "nss")]
        [TestCase("sample.ncs", "nss")]
        [TestCase("sample.pth", "pth")]
        [TestCase("sample.sav", "erf")]
        [TestCase("sample.ssf", "ssf")]
        [TestCase("sample.ssf.xml", "ssf")]
        [TestCase("sample.tlk", "tlk")]
        [TestCase("sample.tlk.xml", "tlk")]
        [TestCase("sample.tlk.json", "tlk")]
        [TestCase("sample.wav", "wav")]
        [TestCase("sample.mp3", "wav")]
        [TestCase("sample.ogg", "wav")]
        [TestCase("sample.wma", "wav")]
        [TestCase("sample.wmv", "wav")]
        [TestCase("sample.xmv", "wav")]
        [TestCase("sample.flac", "wav")]
        [TestCase("sample.bmu", "wav")]
        [TestCase("sample.tpc", "tpc")]
        [TestCase("sample.plt", "tpc")]
        [TestCase("sample.tga", "tpc")]
        [TestCase("sample.dds", "tpc")]
        [TestCase("sample.png", "tpc")]
        [TestCase("sample.jpg", "tpc")]
        [TestCase("sample.bmp", "tpc")]
        [TestCase("sample.txi", "txt")]
        [TestCase("sample.vis", "txt")]
        [TestCase("sample.utc", "utc")]
        [TestCase("sample.utc.xml", "utc")]
        [TestCase("sample.btc", "utc")]
        [TestCase("sample.bic", "utc")]
        [TestCase("sample.utd", "utd")]
        [TestCase("sample.utd.xml", "utd")]
        [TestCase("sample.btd", "utd")]
        [TestCase("sample.ute", "ute")]
        [TestCase("sample.ute.xml", "ute")]
        [TestCase("sample.bte", "ute")]
        [TestCase("sample.uti", "uti")]
        [TestCase("sample.uti.xml", "uti")]
        [TestCase("sample.bti", "uti")]
        [TestCase("sample.utm", "utm")]
        [TestCase("sample.utm.xml", "utm")]
        [TestCase("sample.utm.json", "utm")]
        [TestCase("sample.btm", "utm")]
        [TestCase("sample.utp", "utp")]
        [TestCase("sample.utp.xml", "utp")]
        [TestCase("sample.utp.json", "utp")]
        [TestCase("sample.btp", "utp")]
        [TestCase("sample.uts", "uts")]
        [TestCase("sample.uts.xml", "uts")]
        [TestCase("sample.utt", "utt")]
        [TestCase("sample.utt.xml", "utt")]
        [TestCase("sample.btt", "utt")]
        [TestCase("sample.utw", "utw")]
        [TestCase("sample.txt", "txt")]
        [TestCase("sample.ini", "txt")]
        [TestCase("sample.cfg", "txt")]
        [TestCase("sample.log", "txt")]
        [TestCase("sample.2da_bak", "txt")]
        public void GetEditorKeyFromPath_RoutesHolocronResourceExtensions(string path, string expectedKey)
        {
            Assert.That(StandaloneEditorRouting.GetEditorKeyFromPath(path), Is.EqualTo(expectedKey));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("sample")]
        [TestCase("sample.invalid")]
        public void GetEditorKeyFromPath_ReturnsNullForUnknownPaths(string path)
        {
            Assert.That(StandaloneEditorRouting.GetEditorKeyFromPath(path), Is.Null);
        }

        [Test]
        public void GetEditorKeyFromPath_RoutesSaveGameFolderToSaveGameEditor()
        {
            string saveFolder = Path.Combine(Path.GetTempPath(), "odytools-routing-save-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(saveFolder);
            try
            {
                File.WriteAllBytes(Path.Combine(saveFolder, "savegame.sav"), new byte[] { 1, 2, 3 });

                Assert.That(StandaloneEditorRouting.GetEditorKeyFromPath(saveFolder), Is.EqualTo("savegame"));
            }
            finally
            {
                try { Directory.Delete(saveFolder, true); }
                catch { }
            }
        }

        [Test]
        public void GetEditorKeyFromPath_RoutesSaveGameSavFileToSaveGameEditor()
        {
            string saveFolder = Path.Combine(Path.GetTempPath(), "odytools-routing-save-file-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(saveFolder);
            try
            {
                string savePath = Path.Combine(saveFolder, "SAVEGAME.sav");
                File.WriteAllBytes(savePath, new byte[] { 1, 2, 3 });

                Assert.That(StandaloneEditorRouting.GetEditorKeyFromPath(savePath), Is.EqualTo("savegame"));
            }
            finally
            {
                try { Directory.Delete(saveFolder, true); }
                catch { }
            }
        }

        [TestCase("dlg", "dlg")]
        [TestCase("DLG", "dlg")]
        [TestCase("dlg-editor", "dlg")]
        [TestCase("gui-editor", "gui")]
        [TestCase("lyt-editor", "lyt")]
        [TestCase("twoda-editor", "2da")]
        [TestCase("save-editor", "savegame")]
        [TestCase("sav", "savegame")]
        [TestCase("sav-editor", "savegame")]
        [TestCase("savegame-editor", "savegame")]
        [TestCase("AREEditor", "are")]
        [TestCase("BWMEditor", "bwm")]
        [TestCase("DLGEditor", "dlg")]
        [TestCase("GFFEditor", "gff")]
        [TestCase("GITEditor", "git")]
        [TestCase("NSSEditor", "nss")]
        [TestCase("SaveGameEditor", "savegame")]
        [TestCase("TwoDAEditor", "2da")]
        [TestCase("2DA Table Editor", "2da")]
        [TestCase("Area Editor", "are")]
        [TestCase("Walkmesh Editor", "bwm")]
        [TestCase("Walkmesh Painter", "bwm")]
        [TestCase("Dialog Editor", "dlg")]
        [TestCase("Texture Editor", "tpc")]
        [TestCase("Texture Viewer", "tpc")]
        [TestCase("Audio Player", "wav")]
        [TestCase("Sound Editor", "uts")]
        [TestCase("Sound Set Editor", "ssf")]
        [TestCase("Talk Table Editor", "tlk")]
        [TestCase("Module Info Editor", "ifo")]
        [TestCase("Placeable Editor", "utp")]
        [TestCase("Waypoint Editor", "utw")]
        [TestCase("OdyTools.DLG", "dlg")]
        [TestCase("OdyTools.BWM", "bwm")]
        [TestCase("OdyTools.Save", "savegame")]
        [TestCase("OdyTools.SAV", "savegame")]
        [TestCase("OdyTools.SaveGame", "savegame")]
        [TestCase("OdyToolDLG.Standalone", "dlg")]
        [TestCase("OdyToolBWM.Standalone.exe", "bwm")]
        [TestCase("module-designer", "module-designer")]
        [TestCase("OdyTools.ModuleDesigner", "module-designer")]
        [TestCase("OdyTools.IndoorBuilder", "indoor-builder")]
        public void NormalizeEditorKey_AcceptsHolocronAndOdyToolsLaunchNames(string key, string expectedKey)
        {
            Assert.That(StandaloneEditorRouting.NormalizeEditorKey(key), Is.EqualTo(expectedKey));
        }

        [TestCase("sample.2da", "OdyTool2DA")]
        [TestCase("sample.dlg", "OdyToolDLG.Standalone")]
        [TestCase("sample.cnv", "OdyToolDLG.Standalone")]
        [TestCase("sample.dlg.json", "OdyToolDLG.Standalone")]
        [TestCase("sample.twine.html", "OdyToolDLG.Standalone")]
        [TestCase("sample.hak", "OdyToolERF.Standalone")]
        [TestCase("sample.sav", "OdyToolERF.Standalone")]
        [TestCase("sample.wok", "OdyToolBWM.Standalone")]
        [TestCase("sample.2da.json", "OdyTool2DA")]
        [TestCase("sample.tlk.xml", "OdyToolTLK.Standalone")]
        [TestCase("sample.plt", "OdyToolTPC.Standalone")]
        [TestCase("sample.txi", "OdyToolTXT.Standalone")]
        [TestCase("sample.vis", "OdyToolTXT.Standalone")]
        [TestCase("sample.ogg", "OdyToolWAV.Standalone")]
        [TestCase("sample.wma", "OdyToolWAV.Standalone")]
        [TestCase("sample.wmv", "OdyToolWAV.Standalone")]
        [TestCase("sample.xmv", "OdyToolWAV.Standalone")]
        [TestCase("sample.flac", "OdyToolWAV.Standalone")]
        [TestCase("sample.bmu", "OdyToolWAV.Standalone")]
        [TestCase("sample.mdx", "OdyToolMDL.Standalone")]
        public void GetStandaloneExecutableName_UsesStandaloneApphostNames(string path, string expectedName)
        {
            ResourceType restype = StandaloneEditorRouting.GetResourceTypeFromPath(path);

            Assert.That(StandaloneEditorRouting.GetStandaloneExecutableName(restype), Is.EqualTo(expectedName));
        }

        [TestCase("dlg-editor", "OdyToolDLG.Standalone")]
        [TestCase("gui-editor", "OdyToolGUI.Standalone")]
        [TestCase("lyt-editor", "OdyToolLYT.Standalone")]
        [TestCase("twoda-editor", "OdyTool2DA")]
        [TestCase("save-editor", "OdyToolSAV.Standalone")]
        [TestCase("sav", "OdyToolSAV.Standalone")]
        [TestCase("savegame-editor", "OdyToolSAV.Standalone")]
        [TestCase("SaveGameEditor", "OdyToolSAV.Standalone")]
        [TestCase("TwoDAEditor", "OdyTool2DA")]
        [TestCase("Dialog Editor", "OdyToolDLG.Standalone")]
        [TestCase("Texture Editor", "OdyToolTPC.Standalone")]
        [TestCase("Audio Player", "OdyToolWAV.Standalone")]
        [TestCase("Sound Editor", "OdyToolUTS.Standalone")]
        [TestCase("module-designer", "OdyTools.Standalone")]
        [TestCase("indoor-builder", "OdyTools.Standalone")]
        public void GetStandaloneExecutableNameForKey_UsesExistingStandaloneNames(string key, string expectedName)
        {
            Assert.That(StandaloneEditorRouting.GetStandaloneExecutableNameForKey(key), Is.EqualTo(expectedName));
        }

        [Test]
        public void KnownEditors_ContainsHolocronEditorSurfaceAndOdyToolsWindows()
        {
            var keys = StandaloneEditorRouting.KnownEditors().Select(editor => editor.Key).ToArray();

            Assert.That(keys, Does.Contain("2da"));
            Assert.That(keys, Does.Contain("are"));
            Assert.That(keys, Does.Contain("bwm"));
            Assert.That(keys, Does.Contain("dlg"));
            Assert.That(keys, Does.Contain("erf"));
            Assert.That(keys, Does.Contain("gff"));
            Assert.That(keys, Does.Contain("git"));
            Assert.That(keys, Does.Contain("ifo"));
            Assert.That(keys, Does.Contain("jrl"));
            Assert.That(keys, Does.Contain("lip"));
            Assert.That(keys, Does.Contain("ltr"));
            Assert.That(keys, Does.Contain("mdl"));
            Assert.That(keys, Does.Contain("nss"));
            Assert.That(keys, Does.Contain("pth"));
            Assert.That(keys, Does.Contain("savegame"));
            Assert.That(keys, Does.Contain("ssf"));
            Assert.That(keys, Does.Contain("tlk"));
            Assert.That(keys, Does.Contain("tpc"));
            Assert.That(keys, Does.Contain("txt"));
            Assert.That(keys, Does.Contain("utc"));
            Assert.That(keys, Does.Contain("utd"));
            Assert.That(keys, Does.Contain("ute"));
            Assert.That(keys, Does.Contain("uti"));
            Assert.That(keys, Does.Contain("utm"));
            Assert.That(keys, Does.Contain("utp"));
            Assert.That(keys, Does.Contain("uts"));
            Assert.That(keys, Does.Contain("utt"));
            Assert.That(keys, Does.Contain("utw"));
            Assert.That(keys, Does.Contain("wav"));
            Assert.That(keys, Does.Contain("module-designer"));
            Assert.That(keys, Does.Contain("indoor-builder"));
        }
    }
}
