using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
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
    public class FieldValueReferenceHelperTests
    {
        [Test]
        public void CollectFieldValueReferences_EmptyValue_ReturnsEmpty()
        {
            string installRoot = CreateInstallWithUtcTag("find_me_tag");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var fieldNames = new HashSet<string> { "Tag" };

                Assert.That(
                    FieldValueReferenceHelper.CollectFieldValueReferences("", installation, fieldNames),
                    Is.Empty);
                Assert.That(
                    FieldValueReferenceHelper.CollectFieldValueReferences("   ", installation, fieldNames),
                    Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectFieldValueReferences_NullInstallation_ReturnsEmpty()
        {
            var fieldNames = new HashSet<string> { "Tag" };

            Assert.That(
                FieldValueReferenceHelper.CollectFieldValueReferences("find_me_tag", null, fieldNames),
                Is.Empty);
        }

        [Test]
        public void FindAndShowFieldValueReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                FieldValueReferenceHelper.FindAndShowFieldValueReferences(null, "find_me_tag", null));
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowFieldValueReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithUtcTag("find_me_tag");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var fieldNames = new HashSet<string> { "Tag" };

                Assert.DoesNotThrow(() =>
                    FieldValueReferenceHelper.FindAndShowFieldValueReferences(
                        null,
                        "find_me_tag",
                        installation,
                        fieldNames,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void AttachFieldValueFindReferencesMenu_WiresMenuItem()
        {
            string installRoot = CreateInstallWithUtcTag("find_me_tag");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "find_me_tag" };

                FieldValueReferenceHelper.AttachFieldValueFindReferencesMenu(
                    textBox,
                    null,
                    installation,
                    () => "Tag");

                Assert.That(textBox.ContextMenu, Is.Not.Null);
                Assert.That(textBox.ContextMenu.Items.Count, Is.EqualTo(1));
                var menuItem = textBox.ContextMenu.Items[0] as MenuItem;
                Assert.That(menuItem, Is.Not.Null);
                Assert.That(menuItem.Header as string, Is.EqualTo("Find Field Value References"));
                Assert.That(menuItem.IsEnabled, Is.True);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void AppendFieldValueFindReferencesMenuItem_AddsToExistingMenu()
        {
            string installRoot = CreateInstallWithUtcTag("find_me_tag");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var textBox = new TextBox { Text = "find_me_tag" };
                var contextMenu = new ContextMenu();
                contextMenu.Items.Add(new MenuItem { Header = "Find Tag References" });
                textBox.ContextMenu = contextMenu;

                FieldValueReferenceHelper.AppendFieldValueFindReferencesMenuItem(
                    contextMenu,
                    textBox,
                    null,
                    installation,
                    () => "Tag");

                Assert.That(contextMenu.Items.Count, Is.EqualTo(2));
                var fieldValueItem = contextMenu.Items[1] as MenuItem;
                Assert.That(fieldValueItem, Is.Not.Null);
                Assert.That(fieldValueItem.Header as string, Is.EqualTo("Find Field Value References"));
                Assert.That(fieldValueItem.IsEnabled, Is.True);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void BuildFieldNameFilter_EmptyLabel_ReturnsNull()
        {
            Assert.That(FieldValueReferenceHelper.BuildFieldNameFilter(() => ""), Is.Null);
            Assert.That(FieldValueReferenceHelper.BuildFieldNameFilter(() => "   "), Is.Null);
            Assert.That(FieldValueReferenceHelper.BuildFieldNameFilter(null), Is.Null);
        }

        [Test]
        public void BuildFieldNameFilter_NonEmptyLabel_ReturnsSet()
        {
            HashSet<string> fieldNames = FieldValueReferenceHelper.BuildFieldNameFilter(() => "Tag");

            Assert.That(fieldNames, Is.Not.Null);
            Assert.That(fieldNames, Has.Count.EqualTo(1));
            Assert.That(fieldNames, Contains.Item("Tag"));
        }

        [Test]
        public void CollectFieldValueReferences_UtpTag_FindsInOverride()
        {
            string installRoot = CreateInstallWithUtpTag("plc_find_me");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var fieldNames = new HashSet<string> { "Tag" };

                List<ReferenceSearchResult> results = FieldValueReferenceHelper.CollectFieldValueReferences(
                    "plc_find_me",
                    installation,
                    fieldNames);

                Assert.That(results, Is.Not.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectFieldValueReferences_UtdTemplateResRef_FindsInOverride()
        {
            string installRoot = CreateInstallWithUtdTemplateResRef("dor_find_me");
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var fieldNames = new HashSet<string> { "TemplateResRef" };

                List<ReferenceSearchResult> results = FieldValueReferenceHelper.CollectFieldValueReferences(
                    "dor_find_me",
                    installation,
                    fieldNames);

                Assert.That(results, Is.Not.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithUtcTag(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-fldval-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = tag;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithUtpTag(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-fldval-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utp = new UTP();
            utp.Tag = tag;
            GFF gff = UTPHelpers.DismantleUtp(utp, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTP);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_plc.utp"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithUtdTemplateResRef(string templateResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-fldval-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utd = new UTD();
            utd.ResRef = new ResRef(templateResRef);
            GFF gff = UTDHelpers.DismantleUtd(utd, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTD);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_dor.utd"), bytes);

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
