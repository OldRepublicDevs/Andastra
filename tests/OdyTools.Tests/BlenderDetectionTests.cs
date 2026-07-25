using System;
using System.IO;
using NUnit.Framework;
using OdyTools.Blender;

namespace OdyTools.Tests
{
    public class BlenderDetectionTests
    {
        [TestCase("Blender 3.6.0", 3, 6, 0)]
        [TestCase("Blender 4.2.0 (sub 1)", 4, 2, 0)]
        [TestCase("Blender 5.0.0-alpha", 5, 0, 0)]
        public void ParseBlenderVersion_AcceptsHolocronSupportedFormats(string output, int major, int minor, int patch)
        {
            var version = BlenderDetection.ParseBlenderVersion(output);

            Assert.That(version.HasValue, Is.True);
            Assert.That(version.Value.Major, Is.EqualTo(major));
            Assert.That(version.Value.Minor, Is.EqualTo(minor));
            Assert.That(version.Value.Patch, Is.EqualTo(patch));
        }

        [TestCase(3, 5, 9, false)]
        [TestCase(3, 6, 0, true)]
        [TestCase(4, 1, 5, true)]
        [TestCase(4, 2, 0, true)]
        [TestCase(5, 0, 0, true)]
        public void IsSupportedVersion_RequiresBlender36OrNewer(int major, int minor, int patch, bool expected)
        {
            Assert.That(BlenderDetection.IsSupportedVersion((major, minor, patch)), Is.EqualTo(expected));
        }

        [Test]
        public void BlenderInfo_UsesExtensionInstallPathForBlender42AndNewer()
        {
            var info = new BlenderInfo
            {
                Executable = "/usr/bin/blender",
                Version = (4, 2, 0),
                AddonsPath = "/home/user/.config/blender/4.2/scripts/addons",
                ExtensionsPath = "/home/user/.config/blender/4.2/extensions",
                IsValid = true
            };
            info.UpdateVersionString();

            Assert.That(info.VersionString, Is.EqualTo("4.2.0"));
            Assert.That(info.SupportsExtensions, Is.True);
            Assert.That(info.KotorblenderPath, Is.EqualTo(Path.Combine("/home/user/.config/blender/4.2/extensions", "user_default", "io_scene_kotor")));
        }

        [Test]
        public void BlenderInfo_UsesLegacyAddonPathBeforeBlender42()
        {
            var info = new BlenderInfo
            {
                Executable = "/usr/bin/blender",
                Version = (3, 6, 0),
                AddonsPath = "/home/user/.config/blender/3.6/scripts/addons",
                ExtensionsPath = "/home/user/.config/blender/3.6/extensions",
                IsValid = true
            };
            info.UpdateVersionString();

            Assert.That(info.VersionString, Is.EqualTo("3.6.0"));
            Assert.That(info.SupportsExtensions, Is.False);
            Assert.That(info.KotorblenderPath, Is.EqualTo(Path.Combine("/home/user/.config/blender/3.6/scripts/addons", "io_scene_kotor")));
        }

        [Test]
        public void CheckKotorblenderInstalled_ReadsAddonVersion()
        {
            var root = Path.Combine(Path.GetTempPath(), "odytools-kotorblender-" + Guid.NewGuid().ToString("N"));
            var addon = Path.Combine(root, "io_scene_kotor");
            Directory.CreateDirectory(addon);

            try
            {
                File.WriteAllText(Path.Combine(addon, "__init__.py"), "bl_info = {\"version\": (4, 0, 4)}");

                var info = new BlenderInfo
                {
                    Version = (3, 6, 0),
                    AddonsPath = root,
                    IsValid = true
                };

                Assert.That(BlenderDetection.CheckKotorblenderInstalled(info), Is.True);
                Assert.That(info.KotorblenderVersion, Is.EqualTo("4.0.4"));
            }
            finally
            {
                if (Directory.Exists(root))
                {
                    Directory.Delete(root, true);
                }
            }
        }

        [Test]
        public void DetectBlender_ReturnsActionableStatusWhenAddonIsMissing()
        {
            var info = BlenderDetection.DetectBlender(
                _ =>
                {
                    var found = new BlenderInfo
                    {
                        Executable = "/usr/bin/blender",
                        Version = (4, 2, 0),
                        IsValid = true,
                        HasKotorblender = false
                    };
                    found.UpdateVersionString();
                    return found;
                });

            Assert.That(info.IsValid, Is.True);
            Assert.That(info.HasKotorblender, Is.False);
            Assert.That(info.Error, Does.Contain("kotorblender"));
            Assert.That(info.Error, Does.Contain("Install kotorblender"));
        }

        [Test]
        public void DetectBlender_ReturnsInvalidStatusWhenNoInstallationExists()
        {
            var info = BlenderDetection.DetectBlender(_ => null);

            Assert.That(info.IsValid, Is.False);
            Assert.That(info.Error, Does.Contain("No valid Blender installation"));
        }

        [Test]
        public void CreateBlenderIpcStartInfo_BuildsHolocronStylePythonLaunchCommand()
        {
            var info = new BlenderInfo
            {
                Executable = "/opt/Blender 4.2/blender",
                Version = (4, 2, 0),
                IsValid = true,
                HasKotorblender = true
            };

            var startInfo = BlenderDetection.CreateBlenderIpcStartInfo(
                info,
                8123,
                "/games/KOTOR 2",
                "/games/KOTOR 2/modules/101per.mod",
                "/tmp/test scene.blend",
                background: true);

            Assert.That(startInfo.FileName, Is.EqualTo("/opt/Blender 4.2/blender"));
            Assert.That(startInfo.UseShellExecute, Is.False);
            Assert.That(startInfo.Arguments, Does.Contain("--background"));
            Assert.That(startInfo.Arguments, Does.Contain("\"/tmp/test scene.blend\""));
            Assert.That(startInfo.Arguments, Does.Contain("--python-expr"));
            Assert.That(startInfo.Arguments, Does.Contain("start_ipc_server(port=8123"));
            Assert.That(startInfo.Arguments, Does.Contain("module_path = '/games/KOTOR 2/modules/101per.mod'"));
            Assert.That(startInfo.Arguments, Does.Contain("bl_ext.user_default.io_scene_kotor"));
            Assert.That(startInfo.Arguments, Does.Contain("io_scene_kotor"));
            Assert.That(startInfo.Arguments, Does.Contain("start_scene_monitor"));
        }

        [Test]
        public void GenerateIpcStartupScript_EscapesInstallationPathAndUsesPythonBoolean()
        {
            var script = BlenderDetection.GenerateIpcStartupScript(
                7531,
                "/tmp/Kotor's Path",
                null,
                background: true);

            Assert.That(script, Does.Contain("port=7531"));
            Assert.That(script, Does.Contain("installation_path='/tmp/Kotor\\'s Path'"));
            Assert.That(script, Does.Contain("if True:"));
            Assert.That(script, Does.Contain("[OdyTools.NET] IPC server started"));
        }

        [Test]
        public void IsBlenderAvailable_RequiresValidBlenderAndKotorblender()
        {
            Assert.That(BlenderDetection.IsBlenderAvailable(_ => new BlenderInfo { IsValid = true, HasKotorblender = true }), Is.True);
            Assert.That(BlenderDetection.IsBlenderAvailable(_ => new BlenderInfo { IsValid = true, HasKotorblender = false }), Is.False);
            Assert.That(BlenderDetection.IsBlenderAvailable(_ => new BlenderInfo { IsValid = false, HasKotorblender = true }), Is.False);
        }
    }
}
