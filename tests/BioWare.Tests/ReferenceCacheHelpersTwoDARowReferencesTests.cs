using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class ReferenceCacheHelpersTwoDARowReferencesTests
    {
        [Test]
        public void CollectTwoDARowReferences_WithRowLabel_FindsFieldValueRef()
        {
            const int targetRow = 9;
            const string rowLabel = "row9label";
            string installRoot = CreateInstallWithAppearanceRowAndLabel(targetRow, rowLabel);
            try
            {
                var installation = new Installation(installRoot);
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

                List<ReferenceSearchResult> results = ReferenceCacheHelpers.CollectTwoDARowReferences(
                    installation,
                    "appearance",
                    targetRow,
                    twoDA,
                    null,
                    null,
                    options);

                Assert.That(results.Count, Is.GreaterThanOrEqualTo(2));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void TryLoadTwoDAFromInstallation_LoadsOverrideTable()
        {
            const int targetRow = 3;
            string installRoot = CreateInstallWithAppearanceRowAndLabel(targetRow, "row3");
            try
            {
                var installation = new Installation(installRoot);
                TwoDA loaded = ReferenceCacheHelpers.TryLoadTwoDAFromInstallation(installation, "appearance.2da");

                Assert.That(loaded, Is.Not.Null);
                Assert.That(loaded.GetHeight(), Is.GreaterThan(targetRow));
                Assert.That(loaded.GetLabel(targetRow), Is.EqualTo("row3"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithAppearanceRowAndLabel(int rowIndex, string rowLabel)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "bioware-2da-row-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var twoDA = new TwoDA(new List<string> { "label" });
            for (int i = 0; i <= rowIndex; i++)
            {
                twoDA.AddRow();
            }

            twoDA.SetLabel(rowIndex, rowLabel);
            File.WriteAllBytes(Path.Combine(overrideDir, "appearance.2da"), TwoDAAuto.BytesTwoDA(twoDA));

            var utc = new UTC();
            utc.AppearanceId = rowIndex;
            utc.Tag = rowLabel;
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
