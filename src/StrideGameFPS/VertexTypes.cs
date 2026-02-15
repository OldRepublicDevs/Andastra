using Stride.Core.Mathematics;

namespace StrideGameFPS
{
    /// <summary>
    /// Vertex format: position, normal, texture coordinates.
    /// Equivalent to MonoGameFPS.VertexPositionNormalTexture for Stride.
    /// </summary>
    public struct VertexPositionNormalTexture
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;

        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }
    }
}
