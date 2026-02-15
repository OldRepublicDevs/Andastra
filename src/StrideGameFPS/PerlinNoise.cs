using System;

namespace StrideGameFPS
{
    /// <summary>
    /// Implementation of Perlin noise for procedural generation.
    /// Same as MonoGameFPS (no framework dependency).
    /// </summary>
    public class PerlinNoise
    {
        private readonly int[] _permutation;
        private readonly int _seed;

        private static readonly int[] PermutationBase = {
            151,160,137,91,90,15,131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,
            8,99,37,240,21,10,23,190,6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,
            35,11,32,57,177,33,88,237,149,56,87,174,20,125,136,171,168,68,175,74,165,71,
            134,139,48,27,166,77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,
            55,46,245,40,244,102,143,54,65,25,63,161,1,216,80,73,209,76,132,187,208,89,
            18,169,200,196,135,130,116,188,159,86,164,100,109,198,173,186,3,64,52,217,226,
            250,124,123,5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,
            189,28,42,223,183,170,213,119,248,152,2,44,154,163,70,221,153,101,155,167,43,
            172,9,129,22,39,253,19,98,108,110,79,113,224,232,178,185,112,104,218,246,97,
            228,251,34,242,193,238,210,144,12,191,179,162,241,81,51,145,235,249,14,239,
            107,49,192,214,31,181,199,106,157,184,84,204,176,115,121,50,45,127,4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        public PerlinNoise(int seed)
        {
            _seed = seed;
            _permutation = new int[512];
            var random = new Random(seed);
            int[] p = new int[256];
            for (int i = 0; i < 256; i++) p[i] = i;
            for (int i = 255; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = p[i]; p[i] = p[j]; p[j] = temp;
            }
            for (int i = 0; i < 512; i++) _permutation[i] = p[i & 255];
        }

        public float Noise(float x, float y)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            float u = Fade(x);
            float v = Fade(y);
            int A = _permutation[X] + Y;
            int AA = _permutation[A];
            int AB = _permutation[A + 1];
            int B = _permutation[X + 1] + Y;
            int BA = _permutation[B];
            int BB = _permutation[B + 1];
            return Lerp(v,
                Lerp(u, Grad(_permutation[AA], x, y), Grad(_permutation[BA], x - 1, y)),
                Lerp(u, Grad(_permutation[AB], x, y - 1), Grad(_permutation[BB], x - 1, y - 1)));
        }

        public float OctaveNoise(float x, float y, int octaves, float persistence = 0.5f, float lacunarity = 2.0f)
        {
            float total = 0, frequency = 1, amplitude = 1, maxValue = 0;
            for (int i = 0; i < octaves; i++)
            {
                total += Noise(x * frequency, y * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }
            return total / maxValue;
        }

        private static float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
        private static float Lerp(float t, float a, float b) => a + t * (b - a);
        private static float Grad(int hash, float x, float y)
        {
            int h = hash & 3;
            float u = h < 2 ? x : y, v = h < 2 ? y : x;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public float FractalBrownianMotion(float x, float y, int octaves, float persistence, float lacunarity, float scale)
            => OctaveNoise(x / scale, y / scale, octaves, persistence, lacunarity);
    }
}
