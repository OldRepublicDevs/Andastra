using Stride.Core.Mathematics;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using StrideGraphics = Stride.Graphics;

namespace StrideGameFPS
{
    public class TerrainChunk : IDisposable
    {
        public Vector2 ChunkPosition { get; set; }
        public StrideGraphics.Buffer VertexBuffer { get; set; }
        public StrideGraphics.Buffer IndexBuffer { get; set; }
        public int VertexCount { get; set; }
        public int IndexCount { get; set; }
        public BoundingBox BoundingBox { get; set; }
        public bool IsGenerated { get; set; }

        public void Dispose()
        {
            VertexBuffer?.Dispose();
            IndexBuffer?.Dispose();
        }
    }

    /// <summary>
    /// Procedural terrain generator using Perlin noise. Equivalent to MonoGameFPS.TerrainGenerator.
    /// </summary>
    public class TerrainGenerator
    {
        private readonly PerlinNoise _noise;
        private readonly Dictionary<Vector2, TerrainChunk> _chunks;

        public const int ChunkSize = 64;
        public const float VertexSpacing = 2f;
        public const float HeightScale = 80f;
        public const float NoiseScale = 100f;
        public const int Octaves = 6;
        public const float Persistence = 0.5f;
        public const float Lacunarity = 2.0f;

        public TerrainGenerator(int seed)
        {
            _noise = new PerlinNoise(seed);
            _chunks = new Dictionary<Vector2, TerrainChunk>();
        }

        public float GetHeightAt(float x, float z)
        {
            float noise = _noise.FractalBrownianMotion(x, z, Octaves, Persistence, Lacunarity, NoiseScale);
            noise = (noise + 1f) * 0.5f;
            noise = (float)Math.Pow(noise, 1.5);
            return noise * HeightScale;
        }

        public void GenerateTerrainAroundPosition(Vector3 position, int chunkRadius)
        {
            int centerChunkX = (int)Math.Floor(position.X / (ChunkSize * VertexSpacing));
            int centerChunkZ = (int)Math.Floor(position.Z / (ChunkSize * VertexSpacing));
            for (int x = centerChunkX - chunkRadius; x <= centerChunkX + chunkRadius; x++)
                for (int z = centerChunkZ - chunkRadius; z <= centerChunkZ + chunkRadius; z++)
                {
                    var chunkPos = new Vector2(x, z);
                    if (!_chunks.ContainsKey(chunkPos))
                        _chunks[chunkPos] = new TerrainChunk { ChunkPosition = chunkPos, IsGenerated = false };
                }
            var toRemove = new List<Vector2>();
            foreach (var kvp in _chunks)
            {
                float distX = Math.Abs(kvp.Key.X - centerChunkX);
                float distZ = Math.Abs(kvp.Key.Y - centerChunkZ);
                if (distX > chunkRadius + 2 || distZ > chunkRadius + 2)
                    toRemove.Add(kvp.Key);
            }
            foreach (var pos in toRemove)
            {
                _chunks[pos].Dispose();
                _chunks.Remove(pos);
            }
        }

        public void GenerateChunkMesh(GraphicsDevice graphicsDevice, Vector2 chunkPosition)
        {
            if (!_chunks.TryGetValue(chunkPosition, out var chunk) || chunk.IsGenerated) return;
            float chunkWorldX = chunkPosition.X * ChunkSize * VertexSpacing;
            float chunkWorldZ = chunkPosition.Y * ChunkSize * VertexSpacing;
            var vertices = new VertexPositionNormalTexture[ChunkSize * ChunkSize];
            var minBounds = new Vector3(float.MaxValue);
            var maxBounds = new Vector3(float.MinValue);

            for (int z = 0; z < ChunkSize; z++)
                for (int x = 0; x < ChunkSize; x++)
                {
                    float worldX = chunkWorldX + x * VertexSpacing;
                    float worldZ = chunkWorldZ + z * VertexSpacing;
                    float height = GetHeightAt(worldX, worldZ);
                    var position = new Vector3(worldX, height, worldZ);
                    minBounds = Vector3.Min(minBounds, position);
                    maxBounds = Vector3.Max(maxBounds, position);
                    float textureScale = 0.1f;
                    var texCoord = new Vector2(worldX * textureScale, worldZ * textureScale);
                    vertices[z * ChunkSize + x] = new VertexPositionNormalTexture(position, Vector3.UnitY, texCoord);
                }

            for (int z = 0; z < ChunkSize; z++)
                for (int x = 0; x < ChunkSize; x++)
                {
                    int index = z * ChunkSize + x;
                    float heightL = (x > 0) ? vertices[z * ChunkSize + (x - 1)].Position.Y : vertices[index].Position.Y;
                    float heightR = (x < ChunkSize - 1) ? vertices[z * ChunkSize + (x + 1)].Position.Y : vertices[index].Position.Y;
                    float heightD = (z > 0) ? vertices[(z - 1) * ChunkSize + x].Position.Y : vertices[index].Position.Y;
                    float heightU = (z < ChunkSize - 1) ? vertices[(z + 1) * ChunkSize + x].Position.Y : vertices[index].Position.Y;
                    var normal = new Vector3(heightL - heightR, 2.0f * VertexSpacing, heightD - heightU);
                    normal.Normalize();
                    vertices[index] = new VertexPositionNormalTexture(vertices[index].Position, normal, vertices[index].TextureCoordinate);
                }

            var indices = new int[(ChunkSize - 1) * (ChunkSize - 1) * 6];
            int idx = 0;
            for (int z = 0; z < ChunkSize - 1; z++)
                for (int x = 0; x < ChunkSize - 1; x++)
                {
                    int topLeft = z * ChunkSize + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = (z + 1) * ChunkSize + x;
                    int bottomRight = bottomLeft + 1;
                    indices[idx++] = topLeft; indices[idx++] = bottomLeft; indices[idx++] = topRight;
                    indices[idx++] = topRight; indices[idx++] = bottomLeft; indices[idx++] = bottomRight;
                }

            chunk.VertexBuffer = StrideGraphics.Buffer.Vertex.New(graphicsDevice, vertices, GraphicsResourceUsage.Default);
            chunk.IndexBuffer = StrideGraphics.Buffer.Index.New(graphicsDevice, indices, GraphicsResourceUsage.Default);
            chunk.VertexCount = vertices.Length;
            chunk.IndexCount = indices.Length;
            chunk.BoundingBox = new BoundingBox(minBounds, maxBounds);
            chunk.IsGenerated = true;
        }

        public IEnumerable<TerrainChunk> GetChunks() => _chunks.Values;
        public TerrainChunk GetChunk(Vector2 position) => _chunks.TryGetValue(position, out var c) ? c : null;
    }
}
