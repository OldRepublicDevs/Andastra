using Stride.Graphics;
using System;

namespace StrideGameFPS
{
    /// <summary>
    /// Renders procedurally generated terrain. Equivalent to MonoGameFPS.TerrainRenderer.
    /// Terrain drawing requires a Stride effect (e.g. BasicEffect-style); currently a no-op for minimal run.
    /// </summary>
    public class TerrainRenderer : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly TerrainGenerator _terrain;

        public TerrainRenderer(GraphicsDevice graphicsDevice, TerrainGenerator terrain)
        {
            _graphicsDevice = graphicsDevice;
            _terrain = terrain;
        }

        public void Draw(FPSCamera camera)
        {
            foreach (var chunk in _terrain.GetChunks())
            {
                if (!chunk.IsGenerated || chunk.VertexBuffer == null || chunk.IndexBuffer == null) continue;
                // Frustum culling: skip if chunk is outside view (Stride BoundingFrustum API may vary)
                // if (camera.Frustum.Contains(chunk.BoundingBox) == ContainmentType.Disjoint) continue;
                // TODO: Set pipeline and effect (World, View, Projection, texture), then:
                // commandList.SetVertexBuffer(0, chunk.VertexBuffer, ...);
                // commandList.SetIndexBuffer(chunk.IndexBuffer, ...);
                // commandList.DrawIndexed(chunk.IndexCount);
            }
        }

        public void Dispose() { }
    }
}
