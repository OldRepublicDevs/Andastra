using System;
using System.Collections.Generic;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Media;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using BioWare.Resource.Formats.TPC;
using ResourceType = BioWare.Common.ResourceType;
using OdyTools.Data;
using JetBrains.Annotations;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Renders MDL/MDX model preview using BioWare parsing only. Displays placeholder when no 3D backend is available.
    /// No dependency on Andastra or Stride.
    /// </summary>
    public class ModelRenderer : Control
    {
        /// <summary>Result of model "conversion" for API compatibility. OdyTools does not perform 3D conversion; this is a placeholder type.</summary>
        public sealed class ConversionResult
        {
            public IReadOnlyList<object> Meshes { get; } = Array.Empty<object>();
        }

        private OdyInstallation _installation;
        private byte[] _mdlData;
        private byte[] _mdxData;
        private MDL _parsedModel;
        private ConversionResult _convertedModel;
        private Vector3 _cameraPosition;
        private Vector3 _cameraTarget;
        private Vector3 _cameraUp;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;
        private UTC _creatureToLoad;

        public OdyInstallation Installation
        {
            get => _installation;
            set => _installation = value;
        }

        public void SetModel(byte[] mdlData, byte[] mdxData)
        {
            _mdlData = mdlData;
            _mdxData = mdxData;
            _convertedModel = null;

            if (_mdlData != null && _mdlData.Length > 12)
            {
                try
                {
                    _parsedModel = MDLAuto.ReadMdl(_mdlData, 12, 0, _mdxData, 0, 0);
                    _convertedModel = _parsedModel != null ? new ConversionResult() : null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ModelRenderer] Failed to load model: {ex.Message}");
                    _parsedModel = null;
                    _convertedModel = null;
                }
            }
            else
            {
                _parsedModel = null;
            }

            InvalidateVisual();
        }

        public void ClearModel()
        {
            _mdlData = null;
            _mdxData = null;
            _parsedModel = null;
            _convertedModel = null;
            InvalidateVisual();
        }

        /// <summary>Optional hook for a graphics backend. OdyTools does not use Andastra/Stride; this is a no-op for API compatibility.</summary>
        public void InitializeGraphics(object graphicsDevice, Func<string, object> materialResolver = null)
        {
            if (_mdlData != null && _convertedModel == null)
                SetModel(_mdlData, _mdxData);
        }

        [CanBeNull]
        public MDL ParsedModel => _parsedModel;

        [CanBeNull]
        public ConversionResult ConvertedModel => _convertedModel;

        public void SetCreature(UTC utc)
        {
            _creatureToLoad = utc;
        }

        public void SetCamera(Vector3 position, Vector3 target)
        {
            _cameraPosition = position;
            _cameraTarget = target;
            UpdateViewMatrix();
            InvalidateVisual();
        }

        public ModelRenderer()
        {
            _cameraPosition = new Vector3(0, 0, 10);
            _cameraTarget = new Vector3(0, 0, 0);
            _cameraUp = new Vector3(0, 1, 0);
            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix = Matrix4x4.CreateLookAt(_cameraPosition, _cameraTarget, _cameraUp);
        }

        private void UpdateProjectionMatrix()
        {
            float w = (float)Bounds.Width;
            float h = Math.Max(1, (float)Bounds.Height);
            float aspect = w / h;
            float fov = 45.0f * (float)Math.PI / 180.0f;
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspect, 0.1f, 1000.0f);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (_creatureToLoad != null && _installation != null)
            {
                try
                {
                    var (bodyModel, bodyTexture) = BioWare.Tools.Creature.GetBodyModel(_creatureToLoad, _installation.Installation);
                    if (!string.IsNullOrWhiteSpace(bodyModel))
                    {
                        var mdlResult = _installation.Resource(bodyModel, ResourceType.MDL, null);
                        var mdxResult = _installation.Resource(bodyModel, ResourceType.MDX, null);
                        if (mdlResult != null && mdlResult.Data != null && mdxResult != null && mdxResult.Data != null)
                        {
                            SetModel(mdlResult.Data, mdxResult.Data);
                            ResetCamera();
                        }
                        else
                            Console.WriteLine($"[ModelRenderer] Failed to load creature model '{bodyModel}' - MDL or MDX not found");
                    }
                    else
                        Console.WriteLine("[ModelRenderer] Failed to resolve body model from UTC appearance");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ModelRenderer] Failed to load creature model: {ex.Message}");
                }
                finally
                {
                    _creatureToLoad = null;
                }
            }

            DrawPlaceholderText(context);
        }

        private void ResetCamera()
        {
            _cameraPosition = new Vector3(0, 0, 10);
            _cameraTarget = new Vector3(0, 0, 0);
            _cameraUp = new Vector3(0, 1, 0);
            UpdateViewMatrix();
            InvalidateVisual();
        }

        private void DrawPlaceholderText(DrawingContext context)
        {
            string text = _parsedModel != null
                ? $"Model: {_parsedModel.Name}\n(Mesh count: {_convertedModel?.Meshes.Count ?? 0})"
                : "No model loaded";

            var brush = new SolidColorBrush(Colors.White);
            var formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface("Arial"),
                12,
                brush
            );
            context.DrawText(formattedText, new Avalonia.Point(10, 10));
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateProjectionMatrix();
        }
    }
}
