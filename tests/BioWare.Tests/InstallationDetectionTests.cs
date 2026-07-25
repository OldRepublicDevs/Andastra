using System;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class InstallationDetectionTests
    {
        [Test]
        public void DetermineGame_LinuxSteamTslWrapperPath_UsesSteamAssets()
        {
            string installRoot = CreateLinuxSteamTslInstall();

            try
            {
                BioWareGame? game = Installation.DetermineGame(installRoot);

                Assert.That(game, Is.EqualTo(BioWareGame.TSL));
            }
            finally
            {
                Directory.Delete(installRoot, recursive: true);
            }
        }

        [Test]
        public void Constructor_LinuxSteamTslWrapperPath_UsesSteamAssetsAsResourceRoot()
        {
            string installRoot = CreateLinuxSteamTslInstall();

            try
            {
                var installation = new Installation(installRoot);

                Assert.That(installation.Game, Is.EqualTo(BioWareGame.TSL));
                Assert.That(installation.Path, Is.EqualTo(Path.Combine(installRoot, "steamassets")));
            }
            finally
            {
                Directory.Delete(installRoot, recursive: true);
            }
        }

        private static string CreateLinuxSteamTslInstall()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "andastra-linux-steam-tsl-" + Guid.NewGuid().ToString("N"));
            string steamAssets = Path.Combine(installRoot, "steamassets");

            Directory.CreateDirectory(steamAssets);
            Directory.CreateDirectory(Path.Combine(steamAssets, "streamvoice"));
            Directory.CreateDirectory(Path.Combine(steamAssets, "data"));

            File.WriteAllBytes(Path.Combine(installRoot, "KOTOR2"), new byte[] { 0x7F, 0x45, 0x4C, 0x46 });
            File.WriteAllBytes(Path.Combine(steamAssets, "chitin.key"), new byte[0]);
            File.WriteAllText(Path.Combine(steamAssets, "swkotor2.ini"), "[Game Options]");
            File.WriteAllBytes(Path.Combine(steamAssets, "data", "dialogs.bif"), new byte[0]);

            return installRoot;
        }
    }
}
