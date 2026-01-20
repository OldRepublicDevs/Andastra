namespace Andastra.Runtime.Graphics
{
    /// <summary>
    /// Vertex declaration abstraction for defining vertex formats.
    /// </summary>
    /// <remarks>
    /// Vertex Declaration Interface:
    /// - GLRender::SetInterleavedBuffer @ (K1: 0x00425520, TSL: TODO: Find address) OpenGL vertex format/declaration system
    /// - VertexPrimitiveFlat::SetupArrays @ (K1: 0x00479020, TSL: TODO: Find address) Main vertex format setup orchestrator
    /// - Located via string references: "Disable Vertex Buffer Objects" @ 0x0073d6c0 (VBO option)
    /// - "glVertexAttrib4fvNV" @ 0x0073fcd0, "glVertexAttrib3fvNV" @ 0x0073fce4, "glVertexAttrib2fvNV" @ 0x0073fcf8
    /// - Original implementation: OpenGL vertex arrays using glVertexPointer, glNormalPointer, glTexCoordPointer, glColorPointer
    /// - Vertex formats: Define vertex structure (position, normal, texture coordinates, colors, etc.)
    /// - SetInterleavedBuffer: Sets up interleaved vertex buffer format based on bit flags:
    ///   - Bit 0 (0x01): Position (glVertexPointer, 3 floats)
    ///   - Bit 1 (0x02): Texture coordinate 0 (glTexCoordPointer, 2 floats)
    ///   - Bit 2 (0x04): Texture coordinate 1/lightmap (glTexCoordPointer, 2 floats)
    ///   - Bit 3 (0x08): Additional texture coordinates
    ///   - Bit 5 (0x20): Normal (glNormalPointer, 3 floats)
    ///   - Bit 6 (0x40): Color (glColorPointer, 4 unsigned bytes)
    /// - Supporting functions:
    ///   - GLRender::SetVertexBuffer @ (K1: 0x00425900) - Sets vertex position pointer
    ///   - GLRender::SetNormalBuffer @ (K1: 0x004259e0) - Sets normal pointer
    ///   - GLRender::SetTexCoordBuffer @ (K1: 0x00425a40) - Sets texture coordinate pointer
    ///   - GLRender::SetColorBuffer @ (K1: 0x00425970) - Sets color pointer
    /// - This interface: Abstraction layer for modern graphics APIs (DirectX 11/12, OpenGL, Vulkan)
    /// - Note: Modern APIs use vertex declarations/elements instead of legacy OpenGL client state
    /// </remarks>
    public interface IVertexDeclaration
    {
        /// <summary>
        /// Gets the vertex stride (size in bytes).
        /// </summary>
        int VertexStride { get; }

        /// <summary>
        /// Gets the vertex elements.
        /// </summary>
        VertexElement[] Elements { get; }
    }

    /// <summary>
    /// Vertex element definition.
    /// </summary>
    public struct VertexElement
    {
        public int Offset;
        public VertexElementFormat Format;
        public VertexElementUsage Usage;
        public int UsageIndex;

        public VertexElement(int offset, VertexElementFormat format, VertexElementUsage usage, int usageIndex = 0)
        {
            Offset = offset;
            Format = format;
            Usage = usage;
            UsageIndex = usageIndex;
        }
    }

    /// <summary>
    /// Vertex element format.
    /// </summary>
    public enum VertexElementFormat
    {
        Single,
        Vector2,
        Vector3,
        Vector4,
        Color,
        Byte4,
        Short2,
        Short4,
        NormalizedShort2,
        NormalizedShort4,
        HalfVector2,
        HalfVector4
    }

    /// <summary>
    /// Vertex element usage.
    /// </summary>
    public enum VertexElementUsage
    {
        Position,
        Color,
        TextureCoordinate,
        Normal,
        Binormal,
        Tangent,
        BlendIndices,
        BlendWeight,
        Depth,
        Fog,
        PointSize,
        Sample,
        TessellateFactor
    }
}

