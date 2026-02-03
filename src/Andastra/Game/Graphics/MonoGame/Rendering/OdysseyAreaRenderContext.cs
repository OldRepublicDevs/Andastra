using System.Numerics;
using Andastra.Runtime.Graphics;

namespace Andastra.Game.Graphics.MonoGame.Rendering
{
    /// <summary>
    /// MonoGame implementation of IAreaRenderContext for Odyssey area rendering.
    /// Reva: Area rendering uses CExo3DInternal, room mesh rendering, camera matrices.
    /// </summary>
    public sealed class OdysseyAreaRenderContext : IAreaRenderContext
    {
        public IGraphicsDevice GraphicsDevice { get; set; }
        public IRoomMeshRenderer RoomMeshRenderer { get; set; }
        public IBasicEffect BasicEffect { get; set; }
        public Matrix4x4 ViewMatrix { get; set; }
        public Matrix4x4 ProjectionMatrix { get; set; }
        public Vector3 CameraPosition { get; set; }
    }
}
