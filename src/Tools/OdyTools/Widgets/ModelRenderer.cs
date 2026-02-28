using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using BioWare.Resource.Formats.TPC;
using JetBrains.Annotations;
using OdyTools.Data;
using ResourceType = BioWare.Common.ResourceType;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Renders MDL/MDX previews using Avalonia OpenGL.
    /// Keeps the same API used by existing editors.
    /// </summary>
    public class ModelRenderer : UserControl
    {
        public sealed class ConversionResult
        {
            public IReadOnlyList<object> Meshes { get; internal set; } =
                Array.Empty<object>();
        }

        private sealed class ModelViewport : OpenGlControlBase
        {
            private readonly ModelRenderer _owner;

            public ModelViewport(ModelRenderer owner)
            {
                _owner = owner;
            }

            protected override void OnOpenGlInit(GlInterface gl)
            {
                _owner.OnViewportInit();
            }

            protected override void OnOpenGlDeinit(GlInterface gl)
            {
                _owner.OnViewportDeinit();
            }

            protected override void OnOpenGlRender(GlInterface gl, int fb)
            {
                _owner.RenderOpenGl((int)Math.Max(1, Bounds.Width), (int)Math.Max(1, Bounds.Height));
            }
        }

        private OdyInstallation _installation;
        private byte[] _mdlData;
        private byte[] _mdxData;
        private MDL _parsedModel;
        private ConversionResult _convertedModel;
        private MdlToDrawableConverter.DrawableModel _drawableModel;
        private UTC _creatureToLoad;

        private readonly ModelViewport _viewport;
        private readonly TextBlock _overlayText;
        private DispatcherTimer _renderTimer;

        private Vector3 _cameraPosition;
        private Vector3 _cameraTarget;
        private Vector3 _cameraUp;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;
        private bool _glReady;

        private readonly Dictionary<string, uint> _textureCache = new Dictionary<string, uint>(StringComparer.OrdinalIgnoreCase);

        public OdyInstallation Installation
        {
            get => _installation;
            set => _installation = value;
        }

        public ModelRenderer()
        {
            _cameraPosition = new Vector3(0, 0, 10);
            _cameraTarget = Vector3.Zero;
            _cameraUp = Vector3.UnitY;
            UpdateViewMatrix();
            UpdateProjectionMatrix(600, 400);

            _viewport = new ModelViewport(this);
            _overlayText = new TextBlock
            {
                Margin = new Avalonia.Thickness(8),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                IsHitTestVisible = false,
                Text = "No model loaded",
            };

            var root = new Grid();
            root.Children.Add(_viewport);
            root.Children.Add(_overlayText);
            Content = root;

            _viewport.SizeChanged += (s, e) =>
            {
                UpdateProjectionMatrix((int)Math.Max(1, e.NewSize.Width), (int)Math.Max(1, e.NewSize.Height));
            };

            _renderTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(33),
            };
            _renderTimer.Tick += (s, e) =>
            {
                if (_viewport?.Parent != null && _drawableModel != null)
                {
                    _viewport.InvalidateVisual();
                }
            };
        }

        public void SetModel(byte[] mdlData, byte[] mdxData)
        {
            _mdlData = mdlData;
            _mdxData = mdxData;
            _parsedModel = null;
            _drawableModel = null;
            _convertedModel = null;

            if (_mdlData == null || _mdlData.Length <= 12)
            {
                UpdateOverlay(_installation == null ? "No installation – set game path in Settings" : "No model loaded");
                _viewport.InvalidateVisual();
                return;
            }

            try
            {
                _parsedModel = MDLAuto.ReadMdl(_mdlData, 12, 0, _mdxData, 0, 0);
                _drawableModel = MdlToDrawableConverter.Convert(_parsedModel);
                _convertedModel = new ConversionResult
                {
                    Meshes = _drawableModel != null
                        ? new List<object>(_drawableModel.Meshes)
                        : (IReadOnlyList<object>)Array.Empty<object>(),
                };

                string modelName = !string.IsNullOrWhiteSpace(_parsedModel?.Name) ? _parsedModel.Name : "model";
                int meshCount = _convertedModel.Meshes.Count;
                UpdateOverlay("Model: " + modelName + " | Meshes: " + meshCount);
                ResetCamera();
                _renderTimer?.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ModelRenderer] Failed to load model: " + ex.Message);
                _parsedModel = null;
                _drawableModel = null;
                _convertedModel = null;
                UpdateOverlay("Failed to parse model");
                _renderTimer?.Stop();
            }

            _viewport.InvalidateVisual();
        }

        public void ClearModel()
        {
            _mdlData = null;
            _mdxData = null;
            _parsedModel = null;
            _drawableModel = null;
            _convertedModel = null;
            _renderTimer?.Stop();
            UpdateOverlay(_installation == null ? "No installation – set game path in Settings" : "No model loaded");
            _viewport.InvalidateVisual();
        }

        public void InitializeGraphics(object graphicsDevice, Func<string, object> materialResolver = null)
        {
            if (_mdlData != null && _convertedModel == null)
            {
                SetModel(_mdlData, _mdxData);
            }
        }

        [CanBeNull]
        public MDL ParsedModel => _parsedModel;

        [CanBeNull]
        public ConversionResult ConvertedModel => _convertedModel;

        public void SetCreature(UTC utc)
        {
            _creatureToLoad = utc;
            _viewport.InvalidateVisual();
        }

        public void SetCamera(Vector3 position, Vector3 target)
        {
            _cameraPosition = position;
            _cameraTarget = target;
            UpdateViewMatrix();
            _viewport.InvalidateVisual();
        }

        private void OnViewportInit()
        {
            _glReady = true;
            NativeGl.glEnable(NativeGl.GL_DEPTH_TEST);
            NativeGl.glEnable(NativeGl.GL_CULL_FACE);
            NativeGl.glCullFace(NativeGl.GL_BACK);
            NativeGl.glFrontFace(NativeGl.GL_CCW);
        }

        private void OnViewportDeinit()
        {
            foreach (uint textureId in _textureCache.Values)
            {
                uint id = textureId;
                NativeGl.glDeleteTextures(1, ref id);
            }
            _textureCache.Clear();
            _glReady = false;
        }

        private void RenderOpenGl(int width, int height)
        {
            if (_creatureToLoad != null && _installation != null)
            {
                TryLoadCreatureModel();
            }

            NativeGl.glViewport(0, 0, width, height);
            NativeGl.glClearColor(0.11f, 0.12f, 0.13f, 1.0f);
            NativeGl.glClear(NativeGl.GL_COLOR_BUFFER_BIT | NativeGl.GL_DEPTH_BUFFER_BIT);

            if (!_glReady || _drawableModel == null || _drawableModel.Meshes.Count == 0)
            {
                return;
            }

            NativeGl.glMatrixMode(NativeGl.GL_PROJECTION);
            NativeGl.glLoadMatrixf(ToOpenGlArray(_projectionMatrix));

            NativeGl.glMatrixMode(NativeGl.GL_MODELVIEW);
            NativeGl.glLoadMatrixf(ToOpenGlArray(_viewMatrix));

            NativeGl.glEnable(NativeGl.GL_DEPTH_TEST);

            foreach (MdlToDrawableConverter.DrawableMesh mesh in _drawableModel.Meshes)
            {
                uint textureId = ResolveTexture(mesh.TextureName);
                if (textureId != 0)
                {
                    NativeGl.glEnable(NativeGl.GL_TEXTURE_2D);
                    NativeGl.glBindTexture(NativeGl.GL_TEXTURE_2D, textureId);
                }
                else
                {
                    NativeGl.glDisable(NativeGl.GL_TEXTURE_2D);
                }

                NativeGl.glPushMatrix();
                NativeGl.glMultMatrixf(ToOpenGlArray(mesh.WorldTransform));

                NativeGl.glBegin(NativeGl.GL_TRIANGLES);
                int[] indices = mesh.Indices;
                for (int i = 0; i < indices.Length; i++)
                {
                    int idx = indices[i];
                    if (idx < 0 || idx >= mesh.Positions.Length)
                    {
                        continue;
                    }

                    Vector3 n = idx < mesh.Normals.Length ? mesh.Normals[idx] : Vector3.UnitY;
                    Vector2 uv = idx < mesh.Uvs.Length ? mesh.Uvs[idx] : Vector2.Zero;
                    Vector3 p = mesh.Positions[idx];

                    NativeGl.glNormal3f(n.X, n.Y, n.Z);
                    NativeGl.glTexCoord2f(uv.X, uv.Y);
                    NativeGl.glVertex3f(p.X, p.Y, p.Z);
                }
                NativeGl.glEnd();
                NativeGl.glPopMatrix();
            }

            NativeGl.glDisable(NativeGl.GL_TEXTURE_2D);
        }

        private void TryLoadCreatureModel()
        {
            try
            {
                var modelTuple = BioWare.Tools.Creature.GetBodyModel(_creatureToLoad, _installation.Installation);
                string bodyModel = modelTuple.Item1;
                if (string.IsNullOrWhiteSpace(bodyModel))
                {
                    UpdateOverlay("Failed to resolve creature body model");
                    return;
                }

                var mdlResult = _installation.Resource(bodyModel, ResourceType.MDL, null);
                var mdxResult = _installation.Resource(bodyModel, ResourceType.MDX, null);
                if (mdlResult != null && mdlResult.Data != null && mdxResult != null && mdxResult.Data != null)
                {
                    SetModel(mdlResult.Data, mdxResult.Data);
                }
                else
                {
                    UpdateOverlay("Missing MDL/MDX for creature model: " + bodyModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ModelRenderer] Failed to load creature model: " + ex.Message);
                UpdateOverlay("Creature preview failed");
            }
            finally
            {
                _creatureToLoad = null;
            }
        }

        private uint ResolveTexture(string textureName)
        {
            if (string.IsNullOrWhiteSpace(textureName) || _installation == null)
            {
                return 0;
            }

            if (_textureCache.TryGetValue(textureName, out uint cached))
            {
                return cached;
            }

            try
            {
                var textureResult = _installation.Resource(textureName, ResourceType.TPC, null);
                if (textureResult?.Data == null || textureResult.Data.Length == 0)
                {
                    _textureCache[textureName] = 0;
                    return 0;
                }

                using (var parser = new TpcParser(textureResult.Data))
                {
                    var parsed = parser.Parse();
                    if (parsed == null || parsed.RgbaData == null || parsed.RgbaData.Length == 0)
                    {
                        _textureCache[textureName] = 0;
                        return 0;
                    }

                    uint textureId = 0;
                    NativeGl.glGenTextures(1, ref textureId);
                    if (textureId == 0)
                    {
                        _textureCache[textureName] = 0;
                        return 0;
                    }

                    NativeGl.glBindTexture(NativeGl.GL_TEXTURE_2D, textureId);
                    NativeGl.glTexParameteri(NativeGl.GL_TEXTURE_2D, NativeGl.GL_TEXTURE_MIN_FILTER, NativeGl.GL_LINEAR);
                    NativeGl.glTexParameteri(NativeGl.GL_TEXTURE_2D, NativeGl.GL_TEXTURE_MAG_FILTER, NativeGl.GL_LINEAR);
                    NativeGl.glTexParameteri(NativeGl.GL_TEXTURE_2D, NativeGl.GL_TEXTURE_WRAP_S, NativeGl.GL_REPEAT);
                    NativeGl.glTexParameteri(NativeGl.GL_TEXTURE_2D, NativeGl.GL_TEXTURE_WRAP_T, NativeGl.GL_REPEAT);
                    NativeGl.glPixelStorei(NativeGl.GL_UNPACK_ALIGNMENT, 1);

                    GCHandle handle = GCHandle.Alloc(parsed.RgbaData, GCHandleType.Pinned);
                    try
                    {
                        NativeGl.glTexImage2D(
                            NativeGl.GL_TEXTURE_2D,
                            0,
                            NativeGl.GL_RGBA,
                            parsed.Width,
                            parsed.Height,
                            0,
                            NativeGl.GL_RGBA,
                            NativeGl.GL_UNSIGNED_BYTE,
                            handle.AddrOfPinnedObject());
                    }
                    finally
                    {
                        handle.Free();
                    }

                    _textureCache[textureName] = textureId;
                    return textureId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ModelRenderer] Failed to load texture '" + textureName + "': " + ex.Message);
                _textureCache[textureName] = 0;
                return 0;
            }
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix = Matrix4x4.CreateLookAt(_cameraPosition, _cameraTarget, _cameraUp);
        }

        private void UpdateProjectionMatrix(int width, int height)
        {
            float aspect = width / (float)Math.Max(1, height);
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(
                45.0f * (float)Math.PI / 180.0f,
                aspect,
                0.1f,
                5000.0f);
        }

        private void ResetCamera()
        {
            _cameraPosition = new Vector3(0, 0, 10);
            _cameraTarget = Vector3.Zero;
            _cameraUp = Vector3.UnitY;
            UpdateViewMatrix();
        }

        private void UpdateOverlay(string text)
        {
            _overlayText.Text = text ?? string.Empty;
        }

        private static float[] ToOpenGlArray(Matrix4x4 matrix)
        {
            Matrix4x4 t = Matrix4x4.Transpose(matrix);
            return new[]
            {
                t.M11, t.M12, t.M13, t.M14,
                t.M21, t.M22, t.M23, t.M24,
                t.M31, t.M32, t.M33, t.M34,
                t.M41, t.M42, t.M43, t.M44,
            };
        }

        private static class NativeGl
        {
            public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
            public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;
            public const uint GL_DEPTH_TEST = 0x0B71;
            public const uint GL_CULL_FACE = 0x0B44;
            public const uint GL_BACK = 0x0405;
            public const uint GL_CCW = 0x0901;
            public const uint GL_MODELVIEW = 0x1700;
            public const uint GL_PROJECTION = 0x1701;
            public const uint GL_TRIANGLES = 0x0004;
            public const uint GL_TEXTURE_2D = 0x0DE1;
            public const uint GL_RGBA = 0x1908;
            public const uint GL_UNSIGNED_BYTE = 0x1401;
            public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
            public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
            public const uint GL_TEXTURE_WRAP_S = 0x2802;
            public const uint GL_TEXTURE_WRAP_T = 0x2803;
            public const uint GL_LINEAR = 0x2601;
            public const uint GL_REPEAT = 0x2901;
            public const uint GL_UNPACK_ALIGNMENT = 0x0CF5;

            [DllImport("opengl32.dll")] public static extern void glViewport(int x, int y, int width, int height);
            [DllImport("opengl32.dll")] public static extern void glClearColor(float red, float green, float blue, float alpha);
            [DllImport("opengl32.dll")] public static extern void glClear(uint mask);
            [DllImport("opengl32.dll")] public static extern void glEnable(uint cap);
            [DllImport("opengl32.dll")] public static extern void glDisable(uint cap);
            [DllImport("opengl32.dll")] public static extern void glCullFace(uint mode);
            [DllImport("opengl32.dll")] public static extern void glFrontFace(uint mode);
            [DllImport("opengl32.dll")] public static extern void glMatrixMode(uint mode);
            [DllImport("opengl32.dll")] public static extern void glLoadMatrixf(float[] m);
            [DllImport("opengl32.dll")] public static extern void glMultMatrixf(float[] m);
            [DllImport("opengl32.dll")] public static extern void glPushMatrix();
            [DllImport("opengl32.dll")] public static extern void glPopMatrix();
            [DllImport("opengl32.dll")] public static extern void glBegin(uint mode);
            [DllImport("opengl32.dll")] public static extern void glEnd();
            [DllImport("opengl32.dll")] public static extern void glVertex3f(float x, float y, float z);
            [DllImport("opengl32.dll")] public static extern void glNormal3f(float nx, float ny, float nz);
            [DllImport("opengl32.dll")] public static extern void glTexCoord2f(float s, float t);
            [DllImport("opengl32.dll")] public static extern void glBindTexture(uint target, uint texture);
            [DllImport("opengl32.dll")] public static extern void glGenTextures(int n, ref uint textures);
            [DllImport("opengl32.dll")] public static extern void glDeleteTextures(int n, ref uint textures);
            [DllImport("opengl32.dll")] public static extern void glTexParameteri(uint target, uint pname, uint param);
            [DllImport("opengl32.dll")] public static extern void glPixelStorei(uint pname, int param);
            [DllImport("opengl32.dll")]
            public static extern void glTexImage2D(
                uint target,
                int level,
                uint internalFormat,
                int width,
                int height,
                int border,
                uint format,
                uint type,
                IntPtr pixels);
        }
    }
}
