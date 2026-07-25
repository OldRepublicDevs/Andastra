using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BWM;
using OdyTools.Data;
using OdyTools.Editors;
using NUnit.Framework;
using AColor = Avalonia.Media.Color;

namespace OdyTools.Tests
{
    /// <summary>
    /// BWM (walkmesh) Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolBWMTests
    {
        private static string VendorTestFile(string relativePath)
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (directory != null)
            {
                string candidate = Path.Combine(directory.FullName, "vendor", "tests", "test_files", relativePath);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            Assert.Fail("Could not locate vendor test file: " + relativePath);
            return null;
        }

        private static void AssertVector(Vector3 actual, Vector3 expected, string label, float tolerance = 0.0001f)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(tolerance), label + ".X");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(tolerance), label + ".Y");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(tolerance), label + ".Z");
        }

        private static string FaceSignature(BWMFace face)
        {
            return string.Join("|",
                VectorSignature(face.V1),
                VectorSignature(face.V2),
                VectorSignature(face.V3),
                ((int)face.Material).ToString(CultureInfo.InvariantCulture),
                face.Trans1.HasValue ? face.Trans1.Value.ToString(CultureInfo.InvariantCulture) : "null",
                face.Trans2.HasValue ? face.Trans2.Value.ToString(CultureInfo.InvariantCulture) : "null",
                face.Trans3.HasValue ? face.Trans3.Value.ToString(CultureInfo.InvariantCulture) : "null");
        }

        private static string VectorSignature(Vector3 vector)
        {
            return Math.Round(vector.X, 5).ToString(CultureInfo.InvariantCulture) + "," +
                   Math.Round(vector.Y, 5).ToString(CultureInfo.InvariantCulture) + "," +
                   Math.Round(vector.Z, 5).ToString(CultureInfo.InvariantCulture);
        }

        private static ModuleDesignerSettings SettingsWithValues(params (string Name, int Value)[] values)
        {
            var settings = new ModuleDesignerSettings();
            var field = typeof(Settings).GetField("_values", BindingFlags.Instance | BindingFlags.NonPublic);
            var dictionary = (Dictionary<string, object>)field.GetValue(settings);
            foreach (var value in values)
            {
                dictionary[value.Name] = value.Value;
            }

            return settings;
        }

        [Test]
        public async Task OdyToolBWM_New_BuildsValidBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolBWM(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.GreaterThanOrEqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_Load_RebuildsFaceAndTransitionState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    var face = new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(0, 1, 0))
                    {
                        Material = SurfaceMaterial.Stone,
                        Trans1 = 2
                    };
                    bwm.Faces.Add(face);
                    byte[] data = BWMAuto.BytesBwm(bwm, ResourceType.WOK);

                    var editor = new OdyToolBWM(null, null);
                    editor.Load("test.wok", "test", ResourceType.WOK, data);

                    Assert.That(editor.FaceCount, Is.EqualTo(1));
                    Assert.That(editor.TransitionCount, Is.EqualTo(1));
                    Assert.That(editor.Build().Item1.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_LoadVendorWok_BuildPreservesWalkmeshGeometryAndTransitions()
        {
            byte[] data = File.ReadAllBytes(VendorTestFile("zio006j.wok"));
            BWM original = BWMAuto.ReadBwm(data);

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolBWM(null, null);
                    editor.Load("zio006j.wok", "zio006j", ResourceType.WOK, data);

                    BWM rebuilt = BWMAuto.ReadBwm(editor.Build().Item1);

                    Assert.That(rebuilt.WalkmeshType, Is.EqualTo(original.WalkmeshType));
                    AssertVector(rebuilt.Position, original.Position, "Position");
                    AssertVector(rebuilt.RelativeHook1, original.RelativeHook1, "RelativeHook1");
                    AssertVector(rebuilt.RelativeHook2, original.RelativeHook2, "RelativeHook2");
                    AssertVector(rebuilt.AbsoluteHook1, original.AbsoluteHook1, "AbsoluteHook1");
                    AssertVector(rebuilt.AbsoluteHook2, original.AbsoluteHook2, "AbsoluteHook2");

                    Assert.That(rebuilt.Vertices().Count, Is.EqualTo(original.Vertices().Count));
                    Assert.That(rebuilt.Faces.Count, Is.EqualTo(original.Faces.Count));
                    Assert.That(rebuilt.Faces.Count, Is.GreaterThan(0));

                    var originalFaces = original.Faces.Select(FaceSignature).OrderBy(signature => signature).ToArray();
                    var rebuiltFaces = rebuilt.Faces.Select(FaceSignature).OrderBy(signature => signature).ToArray();
                    Assert.That(rebuiltFaces, Is.EqualTo(originalFaces));

                    int originalTransitionCount = original.Faces.Sum(face =>
                        (face.Trans1.HasValue ? 1 : 0) +
                        (face.Trans2.HasValue ? 1 : 0) +
                        (face.Trans3.HasValue ? 1 : 0));
                    int rebuiltTransitionCount = rebuilt.Faces.Sum(face =>
                        (face.Trans1.HasValue ? 1 : 0) +
                        (face.Trans2.HasValue ? 1 : 0) +
                        (face.Trans3.HasValue ? 1 : 0));
                    Assert.That(rebuiltTransitionCount, Is.EqualTo(originalTransitionCount));
                }, CancellationToken.None);
            }
        }

        [Test]
        public void BWM_Roundtrip_KeepsTransitionsOnWalkableFaces()
        {
            var bwm = new BWM();

            var v1 = new Vector3(0, 0, 0);
            var v2 = new Vector3(1, 0, 0);
            var v3 = new Vector3(0, 1, 0);
            var v4 = new Vector3(1, 1, 0);
            var v5 = new Vector3(2, 0, 0);
            var v6 = new Vector3(2, 1, 0);

            var walkableFace1 = new BWMFace(v1, v2, v3)
            {
                Material = SurfaceMaterial.Metal,
                Trans1 = 1,
                Trans2 = 1
            };
            var walkableFace2 = new BWMFace(v2, v4, v3)
            {
                Material = SurfaceMaterial.Metal
            };
            var unwalkableFace1 = new BWMFace(v2, v5, v4)
            {
                Material = SurfaceMaterial.NonWalk
            };
            var unwalkableFace2 = new BWMFace(v5, v6, v4)
            {
                Material = SurfaceMaterial.NonWalk
            };

            bwm.Faces.Add(walkableFace1);
            bwm.Faces.Add(walkableFace2);
            bwm.Faces.Add(unwalkableFace1);
            bwm.Faces.Add(unwalkableFace2);

            BWM loaded = BWMAuto.ReadBwm(BWMAuto.BytesBwm(bwm, ResourceType.WOK));
            var facesWithTransitions = loaded.Faces
                .Where(face => face.Trans1.HasValue || face.Trans2.HasValue || face.Trans3.HasValue)
                .ToList();

            Assert.That(facesWithTransitions, Has.Count.GreaterThan(0));
            Assert.That(facesWithTransitions.Sum(face =>
                (face.Trans1.HasValue ? 1 : 0) +
                (face.Trans2.HasValue ? 1 : 0) +
                (face.Trans3.HasValue ? 1 : 0)), Is.GreaterThanOrEqualTo(1));
            Assert.That(facesWithTransitions.All(face => face.Material.Walkable()), Is.True);
        }

        [Test]
        public async Task OdyToolBWM_RenderFaceIndex_UsesFaceIdentityForEqualFaces()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    var face1 = new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(0, 1, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    };
                    var face2 = new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(0, 1, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    };

                    bwm.Faces.Add(face1);
                    bwm.Faces.Add(face2);

                    var renderArea = new BWMRenderArea();
                    renderArea.SetWalkmesh(bwm);

                    Assert.That(face1.Equals(face2), Is.True);
                    Assert.That(renderArea.FaceIndexForTests(face1), Is.EqualTo("0"));
                    Assert.That(renderArea.FaceIndexForTests(face2), Is.EqualTo("1"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public void OdyToolBWM_DefaultMaterialPalette_CoversAllSurfaceMaterials()
        {
            var colors = OdyToolBWM.CreateDefaultMaterialColors();

            foreach (SurfaceMaterial material in Enum.GetValues(typeof(SurfaceMaterial)))
            {
                Assert.That(colors.ContainsKey(material), Is.True, "Missing color for " + material);
            }
        }

        [Test]
        public async Task OdyToolBWM_UsesCustomizedMaterialPaletteFromSettings()
        {
            var stoneColor = AColor.FromArgb(127, 25, 51, 76);
            var nonWalkGrassColor = AColor.FromArgb(127, 204, 153, 102);
            var settings = SettingsWithValues(
                ("stoneMaterialColour", new BioWare.Common.Color(25 / 255f, 51 / 255f, 76 / 255f, 127 / 255f).ToRgbaInteger()),
                ("nonWalkGrassMaterialColour", new BioWare.Common.Color(204 / 255f, 153 / 255f, 102 / 255f, 127 / 255f).ToRgbaInteger()));

            var colors = OdyToolBWM.CreateMaterialColors(settings);

            Assert.That(colors[SurfaceMaterial.Stone], Is.EqualTo(stoneColor));
            Assert.That(colors[SurfaceMaterial.Trigger], Is.EqualTo(nonWalkGrassColor));

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolBWM(null, null, settings);

                    Assert.That(editor.MaterialColorForTests(SurfaceMaterial.Stone), Is.EqualTo(stoneColor));
                    Assert.That(editor.RenderAreaForTests.MaterialColorForTests(SurfaceMaterial.Stone), Is.EqualTo(stoneColor));
                    Assert.That(editor.MaterialColorForTests(SurfaceMaterial.Trigger), Is.EqualTo(nonWalkGrassColor));
                    Assert.That(editor.RenderAreaForTests.MaterialColorForTests(SurfaceMaterial.Trigger), Is.EqualTo(nonWalkGrassColor));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_CameraNavigation_UpdatesRenderCamera()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    bwm.Faces.Add(new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(10, 0, 0),
                        new Vector3(0, 10, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    });

                    var editor = new OdyToolBWM(null, null);
                    editor.Load("test.wok", "test", ResourceType.WOK, BWMAuto.BytesBwm(bwm, ResourceType.WOK));

                    Assert.That(editor.RenderAreaForTests, Is.Not.Null);
                    Assert.That(editor.RenderAreaForTests.Focusable, Is.True);

                    Vector2 startPosition = editor.RenderAreaForTests.Camera.Position;
                    float startZoom = editor.RenderAreaForTests.Camera.Zoom;

                    editor.MoveCamera(2, -3);
                    editor.ZoomCamera(1.25f);

                    Assert.That(editor.RenderAreaForTests.Camera.Position.X, Is.EqualTo(startPosition.X + 2).Within(0.001));
                    Assert.That(editor.RenderAreaForTests.Camera.Position.Y, Is.EqualTo(startPosition.Y - 3).Within(0.001));
                    Assert.That(editor.RenderAreaForTests.Camera.Zoom, Is.GreaterThan(startZoom));

                    editor.FrameAll();

                    Assert.That(editor.RenderAreaForTests.Camera.Position.X, Is.EqualTo(5).Within(0.001));
                    Assert.That(editor.RenderAreaForTests.Camera.Position.Y, Is.EqualTo(5).Within(0.001));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_MaterialPainting_UsesPlainLeftDragLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    bwm.Faces.Add(new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(10, 0, 0),
                        new Vector3(0, 10, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    });

                    var editor = new OdyToolBWM(null, null);
                    editor.Load("paint.wok", "paint", ResourceType.WOK, BWMAuto.BytesBwm(bwm, ResourceType.WOK));
                    editor.RenderAreaForTests.SelectedMaterial = SurfaceMaterial.Metal;

                    Assert.That(editor.RenderAreaForTests.PaintFaceAtWorldForTests(1, 1, shiftPressed: false), Is.True);
                    Assert.That(BWMAuto.ReadBwm(editor.Build().Item1).Faces[0].Material, Is.EqualTo(SurfaceMaterial.Metal));
                    Assert.That(editor.RenderAreaForTests.PaintFaceAtWorldForTests(1, 1, shiftPressed: false), Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_MaterialListSelection_PaintsFaceAndRefreshesEditorState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    bwm.Faces.Add(new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(10, 0, 0),
                        new Vector3(0, 10, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    });
                    var editor = new OdyToolBWM(null, null);
                    editor.Load("paint-select.wok", "paint-select", ResourceType.WOK, BWMAuto.BytesBwm(bwm, ResourceType.WOK));

                    Assert.That(editor.IsDirty, Is.False);
                    Assert.That(editor.SummaryTextForTests, Does.Contain("Faces: 1"));
                    Assert.That(editor.SummaryTextForTests, Does.Contain("Walkable: 1"));

                    editor.SelectMaterialForTests(SurfaceMaterial.Grass);

                    Assert.That(editor.SelectedMaterialForTests, Is.EqualTo(SurfaceMaterial.Grass));
                    Assert.That(editor.RenderAreaForTests.PaintFaceAtWorldForTests(1, 1, shiftPressed: false), Is.True);
                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(editor.SummaryTextForTests, Does.Contain("Walkable: 1"));

                    BWM rebuilt = BWMAuto.ReadBwm(editor.Build().Item1);
                    Assert.That(rebuilt.Faces[0].Material, Is.EqualTo(SurfaceMaterial.Grass));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolBWM_CanLoadHolocronBwmExtensionAlias()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bwm = new BWM();
                    bwm.Faces.Add(new BWMFace(
                        new Vector3(0, 0, 0),
                        new Vector3(1, 0, 0),
                        new Vector3(0, 1, 0))
                    {
                        Material = SurfaceMaterial.Stone
                    });

                    byte[] data = BWMAuto.BytesBwm(bwm, ResourceType.WOK);
                    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".bwm");
                    File.WriteAllBytes(path, data);

                    try
                    {
                        var editor = new OdyToolBWM(null, null);

                        Assert.That(editor.CanLoadPath(path), Is.True);
                        Assert.That(editor.TryLoadStartupPath(path), Is.True);
                        Assert.That(editor.FaceCount, Is.EqualTo(1));
                        Assert.That(editor.Build().Item1.Length, Is.GreaterThan(0));
                    }
                    finally
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }, CancellationToken.None);
            }
        }
    }
}
