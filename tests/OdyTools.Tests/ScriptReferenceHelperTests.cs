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
    public class ScriptReferenceHelperTests
    {
        [Test]
        public void FindAndShowScriptReferences_NullComboBox_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
                ScriptReferenceHelper.FindAndShowScriptReferences(null, null, null));
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowScriptReferences_EmptyComboAndNoSelection_DoesNotThrow()
        {
            var comboBox = new ComboBox();

            Assert.DoesNotThrow(() =>
                ScriptReferenceHelper.FindAndShowScriptReferences(null, comboBox, null));
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowScriptReferences_SelectedItemFallback_NullInstallation_DoesNotThrow()
        {
            var comboBox = new ComboBox
            {
                Text = string.Empty
            };
            comboBox.Items.Add("k_test_hb");
            comboBox.SelectedItem = "k_test_hb";

            Assert.DoesNotThrow(() =>
                ScriptReferenceHelper.FindAndShowScriptReferences(null, comboBox, null));
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowScriptReferences_ComboTextTrimmed_NullInstallation_DoesNotThrow()
        {
            var comboBox = new ComboBox
            {
                Text = "  k_test_hb  "
            };
            comboBox.Items.Add("k_other");
            comboBox.SelectedItem = "k_other";

            Assert.DoesNotThrow(() =>
                ScriptReferenceHelper.FindAndShowScriptReferences(null, comboBox, null));
        }

        [Test]
        [AvaloniaTest]
        public void FindAndShowScriptReferences_OverrideHit_CompletesWithoutException()
        {
            string installRoot = CreateInstallWithScriptHeartbeat();
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var comboBox = new ComboBox { Text = "k_test_hb" };

                Assert.DoesNotThrow(() =>
                    ScriptReferenceHelper.FindAndShowScriptReferences(null, comboBox, installation));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithScriptHeartbeat()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-scriptref-" + Guid.NewGuid().ToString("N"));
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
