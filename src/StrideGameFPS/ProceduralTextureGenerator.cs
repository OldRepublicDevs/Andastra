using Stride.Graphics;
using Stride.Core.Mathematics;
using System;

namespace StrideGameFPS
{
    /// <summary>
    /// Generates procedural textures for terrain. Equivalent to MonoGameFPS.ProceduralTextureGenerator.
    /// </summary>
    public class ProceduralTextureGenerator
    {
        private readonly PerlinNoise _noise;

        public ProceduralTextureGenerator(int seed)
        {
            _noise = new PerlinNoise(seed);
        }

        public Texture GenerateTexture(GraphicsDevice graphicsDevice, int width, int height, Func<int, int, Color4> colorFunction)
        {
            var data = new Color4[width * height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    data[y * width + x] = colorFunction(x, y);
            return Texture.New2D(graphicsDevice, width, height, PixelFormat.R8G8B8A8_UNorm, data);
        }

        public Texture GenerateGrassTexture(GraphicsDevice graphicsDevice, int size = 256)
        {
            return GenerateTexture(graphicsDevice, size, size, (x, y) =>
            {
                float noise = _noise.Noise(x * 0.1f, y * 0.1f);
                noise = (noise + 1f) * 0.5f;
                int r = (int)(40 + noise * 30);
                int g = (int)(100 + noise * 40);
                int b = (int)(30 + noise * 20);
                return new Color4(r, g, b, 255);
            });
        }
    }
}
