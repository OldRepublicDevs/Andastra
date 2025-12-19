using System;
using Microsoft.Xna.Framework.Graphics;

namespace Andastra.Runtime.MonoGame.Culling
{
    /// <summary>
    /// Custom effect for Hi-Z mipmap generation with proper max depth operations.
    /// 
    /// This effect performs hierarchical depth buffer downsampling by sampling
    /// 2x2 regions from the previous mip level and outputting the maximum depth value.
    /// 
    /// Based on Hi-Z occlusion culling algorithm:
    /// - Each mip level stores the maximum depth from 2x2 region of previous level
    /// - Enables efficient hierarchical depth testing for occlusion culling
    /// - Proper max depth operations ensure accurate occlusion queries
    /// </summary>
    /// <remarks>
    /// Hi-Z Mipmap Generation Effect:
    /// - Based on modern GPU-accelerated occlusion culling techniques
    /// - Original KOTOR engines (swkotor.exe, swkotor2.exe) used VIS file-based culling
    /// - This is a modernization feature for GPU-accelerated occlusion testing
    /// - Uses pixel shader to perform max depth operations on 2x2 texel regions
    /// </remarks>
    public class HiZMipmapEffect : Effect, IDisposable
    {
        private EffectParameter _sourceTextureParam;
        private EffectParameter _sourceSizeParam;
        private EffectParameter _texelSizeParam;

        /// <summary>
        /// Embedded HLSL shader source for Hi-Z mipmap generation.
        /// Performs max depth operation on 2x2 texel regions.
        /// </summary>
        private const string ShaderSource = @"
// Hi-Z Mipmap Generation Shader
// Performs maximum depth operation on 2x2 texel regions

texture SourceTexture;
sampler SourceSampler = sampler_state
{
    Texture = <SourceTexture>;
    MinFilter = Point;
    MagFilter = Point;
    MipFilter = None;
    AddressU = Clamp;
    AddressV = Clamp;
};

float2 SourceSize;
float2 TexelSize;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct PixelShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

// Vertex shader: Simple pass-through for full-screen quad
PixelShaderInput VertexShaderFunction(VertexShaderInput input)
{
    PixelShaderInput output;
    output.Position = input.Position;
    output.TexCoord = input.TexCoord;
    return output;
}

// Pixel shader: Sample 2x2 region and output maximum depth
float4 PixelShaderFunction(PixelShaderInput input) : COLOR0
{
    // Calculate texel coordinates for 2x2 sampling
    // We sample at the center of each 2x2 region
    float2 texCoord = input.TexCoord;
    
    // Offset by half texel to sample center of 2x2 region
    float2 offset = TexelSize * 0.5;
    
    // Sample 4 texels in 2x2 pattern
    float depth00 = tex2D(SourceSampler, texCoord + float2(-offset.x, -offset.y)).r;
    float depth10 = tex2D(SourceSampler, texCoord + float2(offset.x, -offset.y)).r;
    float depth01 = tex2D(SourceSampler, texCoord + float2(-offset.x, offset.y)).r;
    float depth11 = tex2D(SourceSampler, texCoord + float2(offset.x, offset.y)).r;
    
    // Compute maximum depth from 4 samples
    float maxDepth = max(max(depth00, depth10), max(depth01, depth11));
    
    // Output maximum depth (store in red channel, replicate to RGBA for Single format)
    return float4(maxDepth, maxDepth, maxDepth, maxDepth);
}

technique HiZMipmap
{
    pass Pass0
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
";

        /// <summary>
        /// Initializes a new Hi-Z mipmap generation effect.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device for effect compilation.</param>
        /// <exception cref="ArgumentNullException">Thrown if graphicsDevice is null.</exception>
        public HiZMipmapEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, ShaderSource)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException(nameof(graphicsDevice));
            }

            // Get effect parameters
            _sourceTextureParam = Parameters["SourceTexture"];
            _sourceSizeParam = Parameters["SourceSize"];
            _texelSizeParam = Parameters["TexelSize"];

            // Set default technique
            CurrentTechnique = Techniques["HiZMipmap"];
        }

        /// <summary>
        /// Sets the source texture for mipmap generation.
        /// </summary>
        /// <param name="texture">Source texture (previous mip level).</param>
        public void SetSourceTexture(Texture2D texture)
        {
            if (_sourceTextureParam != null && texture != null)
            {
                _sourceTextureParam.SetValue(texture);
            }
        }

        /// <summary>
        /// Sets the source texture size for texel size calculation.
        /// </summary>
        /// <param name="width">Source texture width.</param>
        /// <param name="height">Source texture height.</param>
        public void SetSourceSize(int width, int height)
        {
            if (_sourceSizeParam != null)
            {
                _sourceSizeParam.SetValue(new Microsoft.Xna.Framework.Vector2(width, height));
            }

            if (_texelSizeParam != null && width > 0 && height > 0)
            {
                // Calculate texel size (1.0 / texture dimensions)
                _texelSizeParam.SetValue(new Microsoft.Xna.Framework.Vector2(1.0f / width, 1.0f / height));
            }
        }

        /// <summary>
        /// Disposes the effect and releases resources.
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
        }
    }
}

