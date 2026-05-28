using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Tools;
using NUnit.Framework;
using OdyTools.Data;
using OdyTools.Utils;

namespace OdyTools.Tests
{
    [TestFixture]
    public class TwoDAMemoryReferenceHelperTests
    {
        [Test]
        public void FindAndShowTwoDAMemoryReferences_NegativeRowIndex_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                TwoDAMemoryReferenceHelper.FindAndShowTwoDAMemoryReferences(null, "appearance", -1, null));
        }

        [Test]
        public void FindAndShowTwoDAMemoryReferences_NullInstallation_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                TwoDAMemoryReferenceHelper.FindAndShowTwoDAMemoryReferences(null, "appearance", 9, null));
        }

        [Test]
        public void FindAndShowTwoDAMemoryReferences_WhitespaceFilename_DoesNotThrow()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                Assert.DoesNotThrow(() =>
                    TwoDAMemoryReferenceHelper.FindAndShowTwoDAMemoryReferences(
                        null,
                        "   ",
                        9,
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_NegativeRowIndex_ReturnsEmpty()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "appearance",
                    -1,
                    null,
                    installation,
                    null);

                Assert.That(results, Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowTwoDAMemoryReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");

                Assert.DoesNotThrow(() =>
                    TwoDAMemoryReferenceHelper.FindAndShowTwoDAMemoryReferences(
                        null,
                        "appearance",
                        9,
                        installation,
                        showOptionsDialog: false));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_EmptyInstall_ReturnsEmpty()
        {
            string installRoot = CreateEmptyInstallRoot();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "appearance",
                    9,
                    null,
                    installation,
                    options);

                Assert.That(results, Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_OverrideOnly_FindsAppearanceMemoryRef()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "appearance",
                    9,
                    null,
                    installation,
                    options);

                Assert.That(results, Is.Not.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_NoOverride_SkipsOverrideAppearanceRef()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = false,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "appearance",
                    9,
                    null,
                    installation,
                    options);

                Assert.That(results, Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_WithTwoDA_FindsLabelFieldValueRef()
        {
            const int targetRow = 9;
            const string rowLabel = "row9label";
            string installRoot = CreateInstallWithAppearanceRowAndLabel(targetRow, rowLabel);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                var twoDA = new TwoDA(new List<string> { "label" });
                for (int i = 0; i <= targetRow; i++)
                {
                    twoDA.AddRow();
                }

                twoDA.SetLabel(targetRow, rowLabel);

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "appearance",
                    targetRow,
                    twoDA,
                    installation,
                    options);

                Assert.That(results.Count, Is.GreaterThanOrEqualTo(2));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectTwoDARowReferences_WithTwoDA_FindsRowStrRefColumnRef()
        {
            const int targetRow = 0;
            const int targetStrRef = 424242;
            string installRoot = CreateInstallWithStrRefReference(targetStrRef);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                var twoDA = new TwoDA(new List<string> { "description" });
                for (int i = 0; i <= targetRow; i++)
                {
                    twoDA.AddRow();
                }

                twoDA.SetCellString(targetRow, "description", targetStrRef.ToString());

                List<ReferenceSearchResult> results = TwoDAMemoryReferenceHelper.CollectTwoDARowReferences(
                    "test_table",
                    targetRow,
                    twoDA,
                    installation,
                    options);

                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.MatchedValue == targetStrRef.ToString()));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateEmptyInstallRoot()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-2da-empty-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
            return installRoot;
        }

        private static string CreateInstallWithAppearanceRow(int rowIndex)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-2da-ref-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.AppearanceId = rowIndex;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithAppearanceRowAndLabel(int rowIndex, string rowLabel)
        {
            string installRoot = CreateInstallWithAppearanceRow(rowIndex);
            string overrideDir = Path.Combine(installRoot, "Override");

            var utc = new UTC();
            utc.AppearanceId = rowIndex;
            utc.Tag = rowLabel;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithStrRefReference(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-strref-ref-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strref);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_set.ssf"), SSFAuto.BytesSsf(ssf));

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
