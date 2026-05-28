using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.UTC;
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
