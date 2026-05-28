using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Interactivity;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using NUnit.Framework;
using OdyTools.Data;
using OdyTools.Utils;

namespace OdyTools.Tests
{
    [TestFixture]
    public class ReferenceSearchHelperTests
    {
        [Test]
        public void FindAndShowTagReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ReferenceSearchHelper.FindAndShowTagReferences(null, "npc_tag", null));
        }

        [Test]
        public void FindAndShowTagReferences_WhitespaceNeedle_DoesNotThrow()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTagReferences(null, "   ", installation, showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void FindAndShowScriptReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ReferenceSearchHelper.FindAndShowScriptReferences(null, "k_test", null));
        }

        [Test]
        public void FindAndShowScriptReferences_WhitespaceNeedle_DoesNotThrow()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowScriptReferences(null, "", installation, showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTagFindReferencesMenu_WiresFindTagReferencesItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "npc_tag" };

                ReferenceSearchHelper.AttachTagFindReferencesMenu(textBox, null, installation);

                Assert.That(textBox.ContextMenu, Is.Not.Null);
                Assert.That(textBox.ContextMenu.Items.Count, Is.EqualTo(1));
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;
                Assert.That(menuItem, Is.Not.Null);
                Assert.That(menuItem.Header?.ToString(), Is.EqualTo("Find Tag References"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTemplateResRefFindReferencesMenu_WiresFindTemplateItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "p_npc001" };

                ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(textBox, null, installation);

                Assert.That(textBox.ContextMenu, Is.Not.Null);
                Assert.That(textBox.ContextMenu.Items.Count, Is.EqualTo(1));
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;
                Assert.That(menuItem, Is.Not.Null);
                Assert.That(menuItem.Header?.ToString(), Is.EqualTo("Find Template ResRef References"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTemplateResRefFindReferencesMenu_EmptyResRef_DisablesMenuItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "p_unique_tpl" };

                ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(textBox, null, installation);
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

                textBox.Text = string.Empty;
                textBox.ContextMenu.RaiseEvent(new RoutedEventArgs(ContextMenu.OpenedEvent));

                Assert.That(menuItem.IsEnabled, Is.False);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTemplateResRefFindReferencesMenu_WithResRefAndInstallation_EnablesMenuItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "p_unique_tpl" };

                ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(textBox, null, installation);
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

                Assert.That(menuItem.IsEnabled, Is.True);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTemplateResRefFindReferencesMenu_NullInstallation_DisablesMenuItem()
        {
            var textBox = new TextBox { Text = "p_unique_tpl" };

            ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(textBox, null, null);
            var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

            Assert.That(menuItem.IsEnabled, Is.False);
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowTemplateResRefReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithTemplateResRef();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTemplateResRefReferences(
                        null,
                        "p_unique_tpl",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTagFindReferencesMenu_EmptyTag_DisablesMenuItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "npc_tag" };

                ReferenceSearchHelper.AttachTagFindReferencesMenu(textBox, null, installation);
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

                textBox.Text = string.Empty;
                textBox.ContextMenu.RaiseEvent(new RoutedEventArgs(ContextMenu.OpenedEvent));

                Assert.That(menuItem.IsEnabled, Is.False);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTagFindReferencesMenu_WithTagAndInstallation_EnablesMenuItem()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "npc_tag" };

                ReferenceSearchHelper.AttachTagFindReferencesMenu(textBox, null, installation);
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

                Assert.That(menuItem.IsEnabled, Is.True);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void AttachTagFindReferencesMenu_NullInstallation_DisablesMenuItem()
        {
            var textBox = new TextBox();

            ReferenceSearchHelper.AttachTagFindReferencesMenu(textBox, null, null);
            var menuItem = textBox.ContextMenu.Items[0] as MenuItem;

            textBox.Text = "npc_tag";
            textBox.ContextMenu.RaiseEvent(new RoutedEventArgs(ContextMenu.OpenedEvent));

            Assert.That(menuItem.IsEnabled, Is.False);
        }

        [Test]
        public void FindAndShowTemplateResRefReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                ReferenceSearchHelper.FindAndShowTemplateResRefReferences(null, "p_npc001", null));
        }

        [Test]
        public void FindAndShowConversationReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                ReferenceSearchHelper.FindAndShowConversationReferences(null, "dlg_test", null));
        }

        [Test]
        public void FindAndShowConversationReferences_WhitespaceNeedle_DoesNotThrow()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowConversationReferences(
                        null,
                        "   ",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void FindAndShowTemplateResRefReferences_WhitespaceNeedle_DoesNotThrow()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTemplateResRefReferences(
                        null,
                        "",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowConversationReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithConversation();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowConversationReferences(
                        null,
                        "test_dlg_ref",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowScriptReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithScriptHeartbeat();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowScriptReferences(
                        null,
                        "k_test_hb",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowTagReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTagReferences(
                        null,
                        "npc_tag",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowTagReferences_NoMatch_CompletesWithoutException()
        {
            string installRoot = CreateMinimalInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTagReferences(
                        null,
                        "nonexistent_tag_xyz",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowTemplateResRefReferences_NoMatch_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithTemplateResRef();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    ReferenceSearchHelper.FindAndShowTemplateResRefReferences(
                        null,
                        "p_missing_tpl",
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void PromptSearchOptions_NullParent_NotAccepted_ReturnsNull()
        {
            ReferenceSearchOptions result = ReferenceSearchHelper.PromptSearchOptions(
                null,
                new ReferenceSearchOptions { SearchOverride = true });

            Assert.That(result, Is.Null);
        }

        private static string CreateMinimalInstallRoot()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-refhelper-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "npc_tag";
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithTemplateResRef()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-refhelper-tpl-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef("p_unique_tpl");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithConversation()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-refhelper-dlg-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef("test_dlg_ref");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithScriptHeartbeat()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-refhelper-script-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static void DeleteDirectorySafe(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                // Best-effort cleanup.
            }
        }
    }
}
