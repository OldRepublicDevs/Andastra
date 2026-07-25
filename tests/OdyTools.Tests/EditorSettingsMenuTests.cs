using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Threading;
using NUnit.Framework;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Editors.DLG;
using OdyTools.Editors.GUI;
using OdyTools.Editors.Standalone;

namespace OdyTools.Tests
{
    public class EditorSettingsMenuTests
    {
        private static IReadOnlyDictionary<string, Func<Editor>> CreateEditorFactories()
        {
            return new Dictionary<string, Func<Editor>>
            {
                { "2DA", () => new OdyTool2DA(null, null) },
                { "ARE", () => new OdyToolARE(null, null) },
                { "BWM", () => new OdyToolBWM(null, null) },
                { "DLG", () => new OdyToolDLG(null, null) },
                { "ERF", () => new OdyToolERF(null, null) },
                { "FAC", () => new OdyToolFAC(null, null) },
                { "GFF", () => new OdyToolGFF(null, null) },
                { "GIT", () => new OdyToolGIT(null, null) },
                { "GUI", () => new OdyToolGUI(null, null) },
                { "IFO", () => new OdyToolIFO(null, null) },
                { "JRL", () => new OdyToolJRL(null, null) },
                { "LIP", () => new OdyToolLIP(null, null) },
                { "LTR", () => new OdyToolLTR(null, null) },
                { "LYT", () => new OdyToolLYT(null, null) },
                { "MDL", () => new OdyToolMDL(null, null) },
                { "NSS", () => new OdyToolNSS(null, null) },
                { "PTH", () => new OdyToolPTH(null, null) },
                { "SAV", () => new OdyToolSAV(null, null) },
                { "SSF", () => new OdyToolSSF(null, null) },
                { "TLK", () => new OdyToolTLK(null, null) },
                { "TPC", () => new OdyToolTPC(null, null) },
                { "TXT", () => new OdyToolTXT(null, null) },
                { "UTC", () => new OdyToolUTC(null, null) },
                { "UTD", () => new OdyToolUTD(null, null) },
                { "UTE", () => new OdyToolUTE(null, null) },
                { "UTI", () => new OdyToolUTI(null, null) },
                { "UTM", () => new OdyToolUTM(null, null) },
                { "UTP", () => new OdyToolUTP(null, null) },
                { "UTS", () => new OdyToolUTS(null, null) },
                { "UTT", () => new OdyToolUTT(null, null) },
                { "UTW", () => new OdyToolUTW(null, null) },
                { "WAV", () => new OdyToolWAV(null, null) },
            };
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void StandaloneEditorsExposeSettingsFromFileMenu()
        {
            var editors = CreateEditorFactories();

            var missing = new List<string>();

            foreach (var entry in editors)
            {
                Editor editor = null;
                try
                {
                    editor = entry.Value();
                    editor.Show();
                    Dispatcher.UIThread.RunJobs();

                    var fileMenu = FindFileMenu(editor);
                    if (fileMenu == null || !ContainsSettingsItem(fileMenu))
                    {
                        missing.Add(entry.Key);
                    }
                }
                finally
                {
                    editor?.Close();
                }
            }

            Assert.That(missing, Is.Empty, "Editors missing File -> Settings: " + string.Join(", ", missing));
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void StandaloneEditorsExposeMappedDocumentationFromHelpMenu()
        {
            var missing = new List<string>();
            var duplicates = new List<string>();

            foreach (var entry in CreateEditorFactories())
            {
                Editor editor = null;
                try
                {
                    editor = entry.Value();
                    editor.Show();
                    Dispatcher.UIThread.RunJobs();

                    bool hasMappedDocs = EditorWikiMapping.GetWikiFiles(editor.GetType().Name)?.Length > 0;
                    var helpMenus = FindHelpMenus(editor).ToList();
                    var documentationItem = helpMenus
                        .SelectMany(menu => menu.Items.OfType<MenuItem>())
                        .FirstOrDefault(item => NormalizeHeader(item.Header) == "documentation");

                    if (hasMappedDocs && documentationItem == null)
                    {
                        missing.Add(entry.Key);
                    }

                    if (hasMappedDocs && helpMenus.Count != 1)
                    {
                        duplicates.Add(entry.Key + " (" + helpMenus.Count + ")");
                    }

                    if (documentationItem != null)
                    {
                        Assert.That(documentationItem.HotKey, Is.EqualTo(new KeyGesture(Key.F1)), entry.Key);
                    }
                }
                finally
                {
                    editor?.Close();
                }
            }

            Assert.That(missing, Is.Empty, "Editors missing Help -> Documentation: " + string.Join(", ", missing));
            Assert.That(duplicates, Is.Empty, "Editors with duplicate Help menus: " + string.Join(", ", duplicates));
        }

        [Test]
        public void EditorWikiMapping_ReferencesExistingHelpFiles()
        {
            string wikiDirectory = FindRepoWikiDirectory();
            var mappedFiles = EditorWikiMapping.GetAllWikiFilenames();
            var missing = mappedFiles
                .Where(file => !File.Exists(Path.Combine(wikiDirectory, file)) && !OdyTools.Dialogs.InlineEditorHelp.HasInlineContent(file))
                .ToList();

            Assert.That(missing, Is.Empty, "Missing mapped wiki files: " + string.Join(", ", missing));
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void StandaloneInstallationBarWrapsEveryEditorWithSelectableControls()
        {
            var missing = new List<string>();
            foreach (var entry in CreateEditorFactories())
            {
                Editor editor = null;
                try
                {
                    editor = entry.Value();
                    StandaloneInstallationBar.Attach(editor);
                    editor.Show();
                    Dispatcher.UIThread.RunJobs();

                    if (FindNamedControl<Control>(editor, "standaloneInstallationBar") == null ||
                        FindNamedControl<ComboBox>(editor, "standaloneEditorCombo") == null ||
                        FindNamedControl<ComboBox>(editor, "standaloneInstallationCombo") == null ||
                        FindNamedControl<Button>(editor, "standaloneBrowseInstallationButton") == null ||
                        FindNamedControl<Button>(editor, "standaloneManageInstallationsButton") == null ||
                        FindNamedControl<CheckBox>(editor, "standaloneTslInstallationCheck") == null)
                    {
                        missing.Add(entry.Key);
                    }
                }
                finally
                {
                    editor?.Close();
                }
            }

            Assert.That(missing, Is.Empty, "Editors missing standalone selectable installation controls: " + string.Join(", ", missing));
        }

        [Test]
        [AvaloniaTest]
        public void StandaloneInstallationBarWrapsEditorWithSelectableControls()
        {
            OdyToolPTH editor = null;
            try
            {
                editor = new OdyToolPTH(null, null);
                StandaloneInstallationBar.Attach(editor);
                editor.Show();
                Dispatcher.UIThread.RunJobs();

                Assert.That(FindNamedControl<Control>(editor, "standaloneInstallationBar"), Is.Not.Null);
                var editorCombo = FindNamedControl<ComboBox>(editor, "standaloneEditorCombo");
                Assert.That(editorCombo, Is.Not.Null);
                Assert.That(editorCombo.Items.Count, Is.GreaterThanOrEqualTo(30));
                Assert.That(editorCombo.SelectedItem?.ToString(), Is.EqualTo("Path"));
                Assert.That(FindNamedControl<ComboBox>(editor, "standaloneInstallationCombo"), Is.Not.Null);
                Assert.That(FindNamedControl<Button>(editor, "standaloneBrowseInstallationButton"), Is.Not.Null);
                Assert.That(FindNamedControl<Button>(editor, "standaloneManageInstallationsButton"), Is.Not.Null);
                Assert.That(FindNamedControl<CheckBox>(editor, "standaloneTslInstallationCheck"), Is.Not.Null);
                Assert.That(FindNamedControl<TextBlock>(editor, "standaloneInstallationPathText")?.Text, Is.EqualTo("No game path selected"));
            }
            finally
            {
                editor?.Close();
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void StandaloneInstallationSelectionRefreshesInstallationAwareEditors()
        {
            var installDir = CreateFakeK1Install();
            var installation = new OdyInstallation(installDir, "Selectable Game", false);
            var editors = new Dictionary<string, Func<Editor>>
            {
                { "ARE", () => new OdyToolARE(null, null) },
                { "UTC", () => new OdyToolUTC(null, null) },
                { "UTD", () => new OdyToolUTD(null, null) },
                { "UTE", () => new OdyToolUTE(null, null) },
                { "UTI", () => new OdyToolUTI(null, null) },
                { "UTM", () => new OdyToolUTM(null, null) },
                { "UTP", () => new OdyToolUTP(null, null) },
                { "UTT", () => new OdyToolUTT(null, null) },
                { "UTW", () => new OdyToolUTW(null, null) },
            };
            var failures = new List<string>();

            try
            {
                foreach (var entry in editors)
                {
                    Editor editor = null;
                    try
                    {
                        editor = entry.Value();
                        editor.Show();
                        Dispatcher.UIThread.RunJobs();

                        editor.SetStandaloneInstallation(installation);
                        Dispatcher.UIThread.RunJobs();

                        Assert.That(editor.Installation, Is.SameAs(installation), entry.Key);

                        editor.SetStandaloneInstallation(null);
                        Dispatcher.UIThread.RunJobs();

                        Assert.That(editor.Installation, Is.Null, entry.Key);
                    }
                    catch (Exception ex)
                    {
                        failures.Add(entry.Key + ": " + ex.GetType().Name + " - " + ex.Message);
                    }
                    finally
                    {
                        editor?.Close();
                    }
                }

                Assert.That(failures, Is.Empty, "Editors failed standalone installation refresh: " + string.Join("; ", failures));
            }
            finally
            {
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        [Test]
        public void StandaloneEditorSwitchArguments_PreserveCurrentOpenFile()
        {
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".pth");
            try
            {
                File.WriteAllText(tempFile, string.Empty);

                var args = StandaloneInstallationBar.BuildLaunchArguments("gff", tempFile, null);

                Assert.That(args, Is.EqualTo(new[] { "--editor", "gff", "--theme", "light", "--open", tempFile }));
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Test]
        public void StandaloneEditorSwitchArguments_PreserveSelectedInstallation()
        {
            var installDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllBytes(Path.Combine(installDir, "chitin.key"), new byte[0]);
                File.WriteAllBytes(Path.Combine(installDir, "KOTOR2"), new byte[] { 0x7F, 0x45, 0x4C, 0x46 });
                var installation = new OdyInstallation(installDir, "Temp TSL", true);

                var args = StandaloneInstallationBar.BuildLaunchArguments("dlg", null, installation);

                Assert.That(args, Is.EqualTo(new[] { "--editor", "dlg", "--theme", "light", "--game-path", installDir, "--tsl" }));
            }
            finally
            {
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        [Test]
        public void StandaloneEditorSwitchArguments_HonorManualTslOverride()
        {
            var installDir = CreateFakeK1Install();
            try
            {
                var installation = new OdyInstallation(installDir, "Manual TSL", true);

                var args = StandaloneInstallationBar.BuildLaunchArguments("dlg", null, installation);

                Assert.That(args, Does.Contain("--tsl"));
                Assert.That(args, Does.Not.Contain("--k1"));
            }
            finally
            {
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        [Test]
        [AvaloniaTest]
        public void StandaloneInstallationBar_TslTogglePersistsSelectedInstallation()
        {
            var settings = new GlobalSettings();
            var original = CloneInstallations(settings.Installations());
            var installDir = CreateFakeK1Install();
            OdyToolPTH editor = null;
            try
            {
                settings.SetInstallations(new Dictionary<string, Dictionary<string, object>>
                {
                    ["Toggle Game"] = new Dictionary<string, object>
                    {
                        ["name"] = "Toggle Game",
                        ["path"] = Path.GetFullPath(installDir),
                        ["tsl"] = false
                    }
                });

                editor = new OdyToolPTH(null, null);
                StandaloneInstallationBar.Attach(editor);
                editor.Show();
                Dispatcher.UIThread.RunJobs();

                var installationCombo = FindNamedControl<ComboBox>(editor, "standaloneInstallationCombo");
                var tslCheck = FindNamedControl<CheckBox>(editor, "standaloneTslInstallationCheck");
                var pathText = FindNamedControl<TextBlock>(editor, "standaloneInstallationPathText");

                Assert.That(installationCombo, Is.Not.Null);
                Assert.That(tslCheck, Is.Not.Null);
                Assert.That(pathText, Is.Not.Null);

                installationCombo.SelectedIndex = 1;
                Dispatcher.UIThread.RunJobs();

                Assert.That(editor.Installation?.Name, Is.EqualTo("Toggle Game"));
                Assert.That(editor.Installation?.Tsl, Is.False);
                Assert.That(pathText.Text, Is.EqualTo(Path.GetFullPath(installDir)));
                Assert.That(tslCheck.IsChecked, Is.False);

                tslCheck.IsChecked = true;
                Dispatcher.UIThread.RunJobs();

                Assert.That(editor.Installation?.Tsl, Is.True);
                Assert.That(editor.Title, Does.Contain("Toggle Game"));
                Assert.That(new GlobalSettings().Installations()["Toggle Game"]["tsl"], Is.EqualTo(true));
            }
            finally
            {
                editor?.Close();
                settings.SetInstallations(original);
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        [Test]
        public void GlobalSettings_AddOrUpdateInstallation_PersistsBrowsableInstallation()
        {
            var settings = new GlobalSettings();
            var original = CloneInstallations(settings.Installations());
            var installDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(installDir);

                var savedName = settings.AddOrUpdateInstallation("Browsed Game", installDir, true);
                var installations = settings.Installations();

                Assert.That(savedName, Is.EqualTo("Browsed Game"));
                Assert.That(installations, Does.ContainKey("Browsed Game"));
                Assert.That(installations["Browsed Game"]["path"]?.ToString(), Is.EqualTo(Path.GetFullPath(installDir)));
                Assert.That(installations["Browsed Game"]["tsl"], Is.EqualTo(true));
            }
            finally
            {
                settings.SetInstallations(original);
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        [Test]
        public void GlobalSettings_AddOrUpdateInstallation_ReusesExistingPath()
        {
            var settings = new GlobalSettings();
            var original = CloneInstallations(settings.Installations());
            var installDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(installDir);
                settings.SetInstallations(new Dictionary<string, Dictionary<string, object>>
                {
                    ["Existing"] = new Dictionary<string, object>
                    {
                        ["name"] = "Existing",
                        ["path"] = Path.GetFullPath(installDir),
                        ["tsl"] = false
                    }
                });

                var savedName = settings.AddOrUpdateInstallation("Browsed Game", installDir, true);
                var installations = settings.Installations();

                Assert.That(savedName, Is.EqualTo("Existing"));
                Assert.That(installations.Keys, Is.EquivalentTo(new[] { "Existing" }));
                Assert.That(installations["Existing"]["tsl"], Is.EqualTo(true));
            }
            finally
            {
                settings.SetInstallations(original);
                if (Directory.Exists(installDir))
                {
                    Directory.Delete(installDir, recursive: true);
                }
            }
        }

        private static MenuItem FindFileMenu(Control root)
        {
            return FindControls<Menu>(root)
                .SelectMany(menu => menu.Items.OfType<MenuItem>())
                .FirstOrDefault(item => IsFileHeader(item.Header));
        }

        private static IEnumerable<MenuItem> FindHelpMenus(Control root)
        {
            return FindControls<Menu>(root)
                .SelectMany(menu => menu.Items.OfType<MenuItem>())
                .Where(item => NormalizeHeader(item.Header) == "help");
        }

        private static bool ContainsSettingsItem(MenuItem fileMenu)
        {
            return fileMenu.Items
                .OfType<MenuItem>()
                .Any(item => IsSettingsHeader(item.Header));
        }

        private static IEnumerable<T> FindControls<T>(Control root) where T : Control
        {
            if (root == null)
            {
                yield break;
            }

            if (root is T match)
            {
                yield return match;
            }

            foreach (var child in root.GetLogicalChildren().OfType<Control>())
            {
                foreach (var matchChild in FindControls<T>(child))
                {
                    yield return matchChild;
                }
            }
        }

        private static T FindNamedControl<T>(Control root, string name) where T : Control
        {
            return FindControls<T>(root).FirstOrDefault(control => control.Name == name);
        }

        private static bool IsFileHeader(object header)
        {
            return NormalizeHeader(header) == "file";
        }

        private static bool IsSettingsHeader(object header)
        {
            var normalized = NormalizeHeader(header);
            return normalized == "settings" || normalized.EndsWith(" settings", StringComparison.OrdinalIgnoreCase);
        }

        private static string FindRepoWikiDirectory()
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (directory != null)
            {
                string candidate = Path.Combine(directory.FullName, "wiki");
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            Assert.Fail("Could not locate repo wiki directory.");
            return null;
        }

        private static string NormalizeHeader(object header)
        {
            return (header?.ToString() ?? string.Empty).Replace("_", string.Empty).Replace(".", string.Empty).Trim().ToLowerInvariant();
        }

        private static string CreateFakeK1Install()
        {
            var installDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installDir);
            File.WriteAllBytes(Path.Combine(installDir, "chitin.key"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installDir, "SWKOTOR.EXE"), new byte[0]);
            return installDir;
        }

        private static Dictionary<string, Dictionary<string, object>> CloneInstallations(
            Dictionary<string, Dictionary<string, object>> source)
        {
            var clone = new Dictionary<string, Dictionary<string, object>>();
            if (source == null)
            {
                return clone;
            }

            foreach (var kvp in source)
            {
                clone[kvp.Key] = kvp.Value == null
                    ? new Dictionary<string, object>()
                    : new Dictionary<string, object>(kvp.Value);
            }

            return clone;
        }
    }
}
