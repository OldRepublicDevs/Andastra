using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
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
