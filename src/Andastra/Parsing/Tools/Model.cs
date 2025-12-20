using System;
using System.Collections.Generic;
using System.Linq;
using Andastra.Parsing;
using JetBrains.Annotations;
using BinaryReader = Andastra.Parsing.Common.BinaryReader;

namespace Andastra.Parsing.Tools
{
    /// <summary>
    /// Tuple class for returning MDL and MDX data pairs.
    /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:87-89
    /// </summary>
    public class MDLMDXTuple
    {
        public byte[] Mdl { get; set; }
        public byte[] Mdx { get; set; }

        public MDLMDXTuple(byte[] mdl, byte[] mdx)
        {
            Mdl = mdl;
            Mdx = mdx;
        }
    }

    /// <summary>
    /// Utility functions for working with 3D model data.
    /// </summary>
    [PublicAPI]
    public static class ModelTools
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:34-78
        // Mesh type constants for determining TSL vs K1
        private const uint _MESH_FP0_K1 = 4216656;
        private const uint _SKIN_FP0_K1 = 4216592;
        private const uint _DANGLY_FP0_K2 = 4216864;
        private const uint _AABB_FP0_K1 = 4216656;
        private const uint _SABER_FP0_K1 = 4216656;
        private const int _NODE_TYPE_MESH = 32;
        /// <summary>
        /// Extracts texture and lightmap names from MDL model data.
        /// </summary>
        /// <param name="data">The binary MDL data.</param>
        /// <returns>An enumerable of texture and lightmap names.</returns>
        public static IEnumerable<string> IterateTexturesAndLightmaps(byte[] data)
        {
            HashSet<string> seenNames = new HashSet<string>();

            using (BinaryReader reader = BinaryReader.FromBytes(data, 12))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                Queue<uint> nodes = new Queue<uint>();
                nodes.Enqueue(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Dequeue();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        nodes.Enqueue(reader.ReadUInt32());
                    }

                    if ((nodeId & 32) != 0)
                    {
                        // Extract texture name
                        reader.Seek((int)nodeOffset + 168);
                        string name = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim().ToLower();
                        if (!string.IsNullOrEmpty(name) && name != "null" && !seenNames.Contains(name) && name != "dirt")
                        {
                            seenNames.Add(name);
                            yield return name;
                        }

                        // Extract lightmap name
                        reader.Seek((int)nodeOffset + 200);
                        name = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim().ToLower();
                        if (!string.IsNullOrEmpty(name) && name != "null" && !seenNames.Contains(name))
                        {
                            seenNames.Add(name);
                            yield return name;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts texture names from MDL model data.
        /// </summary>
        /// <param name="data">The binary MDL data.</param>
        /// <returns>An enumerable of texture names.</returns>
        public static IEnumerable<string> IterateTextures(byte[] data)
        {
            HashSet<string> textureCaseset = new HashSet<string>();

            using (BinaryReader reader = BinaryReader.FromBytes(data, 12))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                Stack<uint> nodes = new Stack<uint>();
                nodes.Push(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Pop();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    Stack<uint> childOffsets = new Stack<uint>();
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        childOffsets.Push(reader.ReadUInt32());
                    }
                    while (childOffsets.Count > 0)
                    {
                        nodes.Push(childOffsets.Pop());
                    }

                    if ((nodeId & 32) != 0)
                    {
                        reader.Seek((int)nodeOffset + 168);
                        string texture = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim();
                        string lowerTexture = texture.ToLower();
                        if (!string.IsNullOrEmpty(texture)
                            && texture.ToUpper() != "NULL"
                            && !textureCaseset.Contains(lowerTexture)
                            && lowerTexture != "dirt")
                        {
                            textureCaseset.Add(lowerTexture);
                            yield return lowerTexture;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extracts lightmap names from MDL model data.
        /// </summary>
        /// <param name="data">The binary MDL data.</param>
        /// <returns>An enumerable of lightmap names.</returns>
        public static IEnumerable<string> IterateLightmaps(byte[] data)
        {
            HashSet<string> lightmapsCaseset = new HashSet<string>();

            using (BinaryReader reader = BinaryReader.FromBytes(data, 12))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                Stack<uint> nodes = new Stack<uint>();
                nodes.Push(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Pop();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    Stack<uint> childOffsets = new Stack<uint>();
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        childOffsets.Push(reader.ReadUInt32());
                    }
                    while (childOffsets.Count > 0)
                    {
                        nodes.Push(childOffsets.Pop());
                    }

                    if ((nodeId & 32) != 0)
                    {
                        reader.Seek((int)nodeOffset + 200);
                        string lightmap = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim().ToLower();
                        if (!string.IsNullOrEmpty(lightmap) && lightmap != "null" && !lightmapsCaseset.Contains(lightmap))
                        {
                            lightmapsCaseset.Add(lightmap);
                            yield return lightmap;
                        }
                    }
                }
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:92-96
        // Original: def rename(data: bytes, name: str) -> bytes:
        /// <summary>
        /// Renames an MDL model by replacing the name field at offset 20.
        /// </summary>
        public static byte[] Rename(byte[] data, string name)
        {
            if (data == null || data.Length < 52)
            {
                throw new ArgumentException("Invalid MDL data");
            }
            byte[] result = new byte[data.Length];
            Array.Copy(data, 0, result, 0, 20);
            byte[] nameBytes = new byte[32];
            System.Text.Encoding.ASCII.GetBytes(name.PadRight(32, '\0'), 0, Math.Min(name.Length, 32), nameBytes, 0);
            Array.Copy(nameBytes, 0, result, 20, 32);
            Array.Copy(data, 52, result, 52, data.Length - 52);
            return result;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:197-248
        // Original: def change_textures(data: bytes | bytearray, textures: dict[str, str]) -> bytes | bytearray:
        /// <summary>
        /// Changes texture names in MDL model data.
        /// </summary>
        public static byte[] ChangeTextures(byte[] data, Dictionary<string, string> textures)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (textures == null)
            {
                return data;
            }

            byte[] parsedData = new byte[data.Length];
            Array.Copy(data, parsedData, data.Length);
            Dictionary<string, List<int>> offsets = new Dictionary<string, List<int>>();

            // Normalize texture names to lowercase
            Dictionary<string, string> texturesLower = new Dictionary<string, string>();
            foreach (var kvp in textures)
            {
                texturesLower[kvp.Key.ToLowerInvariant()] = kvp.Value.ToLowerInvariant();
            }

            using (BinaryReader reader = BinaryReader.FromBytes(parsedData, 12))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                Stack<uint> nodes = new Stack<uint>();
                nodes.Push(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Pop();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    Stack<uint> childOffsets = new Stack<uint>();
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        childOffsets.Push(reader.ReadUInt32());
                    }
                    while (childOffsets.Count > 0)
                    {
                        nodes.Push(childOffsets.Pop());
                    }

                    if ((nodeId & 32) != 0)
                    {
                        reader.Seek((int)nodeOffset + 168);
                        string texture = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim().ToLowerInvariant();

                        if (texturesLower.ContainsKey(texture))
                        {
                            if (!offsets.ContainsKey(texture))
                            {
                                offsets[texture] = new List<int>();
                            }
                            offsets[texture].Add((int)nodeOffset + 168);
                        }
                    }
                }
            }

            // Replace texture names at found offsets
            foreach (var kvp in offsets)
            {
                string newTexture = texturesLower[kvp.Key];
                byte[] newTextureBytes = new byte[32];
                System.Text.Encoding.ASCII.GetBytes(newTexture.PadRight(32, '\0'), 0, Math.Min(newTexture.Length, 32), newTextureBytes, 0);
                foreach (int offset in kvp.Value)
                {
                    int actualOffset = offset + 12;
                    Array.Copy(newTextureBytes, 0, parsedData, actualOffset, 32);
                }
            }

            return parsedData;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:251-302
        // Original: def change_lightmaps(data: bytes | bytearray, textures: dict[str, str]) -> bytes | bytearray:
        /// <summary>
        /// Changes lightmap names in MDL model data.
        /// </summary>
        public static byte[] ChangeLightmaps(byte[] data, Dictionary<string, string> lightmaps)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (lightmaps == null)
            {
                return data;
            }

            byte[] parsedData = new byte[data.Length];
            Array.Copy(data, parsedData, data.Length);
            Dictionary<string, List<int>> offsets = new Dictionary<string, List<int>>();

            // Normalize lightmap names to lowercase
            Dictionary<string, string> lightmapsLower = new Dictionary<string, string>();
            foreach (var kvp in lightmaps)
            {
                lightmapsLower[kvp.Key.ToLowerInvariant()] = kvp.Value.ToLowerInvariant();
            }

            using (BinaryReader reader = BinaryReader.FromBytes(parsedData, 12))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                Stack<uint> nodes = new Stack<uint>();
                nodes.Push(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Pop();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    Stack<uint> childOffsets = new Stack<uint>();
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        childOffsets.Push(reader.ReadUInt32());
                    }
                    while (childOffsets.Count > 0)
                    {
                        nodes.Push(childOffsets.Pop());
                    }

                    if ((nodeId & 32) != 0)
                    {
                        reader.Seek((int)nodeOffset + 200);
                        string lightmap = reader.ReadString(32, encoding: "ascii", errors: "ignore").Trim().ToLowerInvariant();

                        if (lightmapsLower.ContainsKey(lightmap))
                        {
                            if (!offsets.ContainsKey(lightmap))
                            {
                                offsets[lightmap] = new List<int>();
                            }
                            offsets[lightmap].Add((int)nodeOffset + 200);
                        }
                    }
                }
            }

            // Replace lightmap names at found offsets
            foreach (var kvp in offsets)
            {
                string newLightmap = lightmapsLower[kvp.Key];
                byte[] newLightmapBytes = new byte[32];
                System.Text.Encoding.ASCII.GetBytes(newLightmap.PadRight(32, '\0'), 0, Math.Min(newLightmap.Length, 32), newLightmapBytes, 0);
                foreach (int offset in kvp.Value)
                {
                    int actualOffset = offset + 12;
                    Array.Copy(newLightmapBytes, 0, parsedData, actualOffset, 32);
                }
            }

            return parsedData;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:724-886
        // Original: def flip(mdl_data: bytes | bytearray, mdx_data: bytes | bytearray, *, flip_x: bool, flip_y: bool) -> MDLMDXTuple:
        /// <summary>
        /// Flips a model by negating X and/or Y coordinates in vertices and normals.
        /// </summary>
        /// <param name="mdlData">The MDL model data.</param>
        /// <param name="mdxData">The MDX material index data.</param>
        /// <param name="flipX">Whether to flip along the X axis.</param>
        /// <param name="flipY">Whether to flip along the Y axis.</param>
        /// <returns>A tuple containing the flipped MDL and MDX data.</returns>
        public static MDLMDXTuple Flip(byte[] mdlData, byte[] mdxData, bool flipX, bool flipY)
        {
            // If neither bools are set to True, no transformations need to be done and we can just return the original data
            if (!flipX && !flipY)
            {
                return new MDLMDXTuple(mdlData, mdxData);
            }

            // The data we need to change:
            //    1. The vertices stored in the MDL
            //    2. The vertex positions, normals, stored in the MDX

            // Trim the data to correct the offsets
            byte[] mdlStart = new byte[12];
            Array.Copy(mdlData, 0, mdlStart, 0, 12);
            byte[] parsedMdlData = new byte[mdlData.Length - 12];
            Array.Copy(mdlData, 12, parsedMdlData, 0, parsedMdlData.Length);
            byte[] parsedMdxData = new byte[mdxData.Length];
            Array.Copy(mdxData, 0, parsedMdxData, 0, mdxData.Length);

            // Lists to store offsets: (count, offset) for MDL vertices
            var mdlVertexOffsets = new List<Tuple<int, int>>();
            // Lists to store offsets: (count, offset, stride, position) for MDX vertices and normals
            var mdxVertexOffsets = new List<Tuple<int, int, int, int>>();
            var mdxNormalOffsets = new List<Tuple<int, int, int, int>>();
            var elementsOffsets = new List<Tuple<int, int>>();
            var facesOffsets = new List<Tuple<int, int>>();

            using (BinaryReader reader = BinaryReader.FromBytes(parsedMdlData, 0))
            {
                reader.Seek(168);
                uint rootOffset = reader.ReadUInt32();

                var nodes = new Stack<uint>();
                nodes.Push(rootOffset);

                while (nodes.Count > 0)
                {
                    uint nodeOffset = nodes.Pop();
                    reader.Seek((int)nodeOffset);
                    uint nodeId = reader.ReadUInt32();

                    mdlVertexOffsets.Add(new Tuple<int, int>(1, (int)nodeOffset + 16));

                    // Need to determine the location of the position controller
                    reader.Seek((int)nodeOffset + 56);
                    uint controllersOffset = reader.ReadUInt32();
                    uint controllersCount = reader.ReadUInt32();

                    reader.Seek((int)nodeOffset + 68);
                    uint controllerDatasOffset = reader.ReadUInt32();
                    reader.ReadUInt32(); // Skip next uint32

                    for (uint i = 0; i < controllersCount; i++)
                    {
                        reader.Seek((int)(controllersOffset + i * 16));
                        uint controllerType = reader.ReadUInt32();
                        if (controllerType == 8)
                        {
                            reader.Seek((int)(controllersOffset + i * 16 + 6));
                            ushort dataOffset = reader.ReadUInt16();
                            mdlVertexOffsets.Add(new Tuple<int, int>(1, (int)(controllerDatasOffset + dataOffset * 4)));
                        }
                    }

                    reader.Seek((int)nodeOffset + 44);
                    uint childOffsetsOffset = reader.ReadUInt32();
                    uint childOffsetsCount = reader.ReadUInt32();

                    reader.Seek((int)childOffsetsOffset);
                    for (uint i = 0; i < childOffsetsCount; i++)
                    {
                        nodes.Push(reader.ReadUInt32());
                    }

                    if ((nodeId & _NODE_TYPE_MESH) != 0)
                    {
                        reader.Seek((int)nodeOffset + 80);
                        uint fp = reader.ReadUInt32();
                        bool tsl = fp != _MESH_FP0_K1 && fp != _SKIN_FP0_K1 && fp != _DANGLY_FP0_K2 && fp != _AABB_FP0_K1 && fp != _SABER_FP0_K1;

                        reader.Seek((int)nodeOffset + 80 + 8);
                        uint facesOffset = reader.ReadUInt32();
                        uint facesCount = reader.ReadUInt32();
                        facesOffsets.Add(new Tuple<int, int>((int)facesCount, (int)facesOffset));

                        reader.Seek((int)nodeOffset + 80 + 188);
                        uint offsetToElementsOffset = reader.ReadUInt32();
                        reader.Seek((int)offsetToElementsOffset);
                        uint elementsOffset = reader.ReadUInt32();
                        elementsOffsets.Add(new Tuple<int, int>((int)facesCount, (int)elementsOffset));

                        reader.Seek((int)nodeOffset + 80 + 304);
                        ushort vertexCount = reader.ReadUInt16();
                        reader.Seek((int)(nodeOffset + 80 + (tsl ? 336 : 328)));
                        uint vertexOffset = reader.ReadUInt32();
                        mdlVertexOffsets.Add(new Tuple<int, int>(vertexCount, (int)vertexOffset));

                        reader.Seek((int)nodeOffset + 80 + 252);
                        uint mdxStride = reader.ReadUInt32();
                        reader.ReadUInt32(); // Skip next uint32
                        reader.Seek((int)nodeOffset + 80 + 260);
                        uint mdxOffsetPos = reader.ReadUInt32();
                        uint mdxOffsetNorm = reader.ReadUInt32();
                        reader.Seek((int)(nodeOffset + 80 + (tsl ? 332 : 324)));
                        uint mdxStart = reader.ReadUInt32();
                        mdxVertexOffsets.Add(new Tuple<int, int, int, int>((int)vertexCount, (int)mdxStart, (int)mdxStride, (int)mdxOffsetPos));
                        mdxNormalOffsets.Add(new Tuple<int, int, int, int>((int)vertexCount, (int)mdxStart, (int)mdxStride, (int)mdxOffsetNorm));
                    }
                }
            }

            // Fix vertex order
            if (flipX != flipY)
            {
                foreach (var tuple in elementsOffsets)
                {
                    int count = tuple.Item1;
                    int startOffset = tuple.Item2;
                    for (int i = 0; i < count; i++)
                    {
                        int offset = startOffset + i * 6;
                        ushort v1 = BitConverter.ToUInt16(parsedMdlData, offset);
                        ushort v2 = BitConverter.ToUInt16(parsedMdlData, offset + 2);
                        ushort v3 = BitConverter.ToUInt16(parsedMdlData, offset + 4);
                        byte[] v1Bytes = BitConverter.GetBytes(v1);
                        byte[] v3Bytes = BitConverter.GetBytes(v3);
                        byte[] v2Bytes = BitConverter.GetBytes(v2);
                        Array.Copy(v1Bytes, 0, parsedMdlData, offset, 2);
                        Array.Copy(v3Bytes, 0, parsedMdlData, offset + 2, 2);
                        Array.Copy(v2Bytes, 0, parsedMdlData, offset + 4, 2);
                    }
                }

                foreach (var tuple in facesOffsets)
                {
                    int count = tuple.Item1;
                    int startOffset = tuple.Item2;
                    for (int i = 0; i < count; i++)
                    {
                        int offset = startOffset + i * 32 + 26;
                        ushort v1 = BitConverter.ToUInt16(parsedMdlData, offset);
                        ushort v2 = BitConverter.ToUInt16(parsedMdlData, offset + 2);
                        ushort v3 = BitConverter.ToUInt16(parsedMdlData, offset + 4);
                        byte[] v1Bytes = BitConverter.GetBytes(v1);
                        byte[] v3Bytes = BitConverter.GetBytes(v3);
                        byte[] v2Bytes = BitConverter.GetBytes(v2);
                        Array.Copy(v1Bytes, 0, parsedMdlData, offset, 2);
                        Array.Copy(v3Bytes, 0, parsedMdlData, offset + 2, 2);
                        Array.Copy(v2Bytes, 0, parsedMdlData, offset + 4, 2);
                    }
                }
            }

            // Update the MDL vertices
            foreach (var tuple in mdlVertexOffsets)
            {
                int count = tuple.Item1;
                int startOffset = tuple.Item2;
                for (int i = 0; i < count; i++)
                {
                    int offset = startOffset + i * 12;
                    if (flipX)
                    {
                        float x = BitConverter.ToSingle(parsedMdlData, offset);
                        byte[] xBytes = BitConverter.GetBytes(-x);
                        Array.Copy(xBytes, 0, parsedMdlData, offset, 4);
                    }
                    if (flipY)
                    {
                        float y = BitConverter.ToSingle(parsedMdlData, offset + 4);
                        byte[] yBytes = BitConverter.GetBytes(-y);
                        Array.Copy(yBytes, 0, parsedMdlData, offset + 4, 4);
                    }
                }
            }

            // Update the MDX vertices
            foreach (var tuple in mdxVertexOffsets)
            {
                int count = tuple.Item1;
                int startOffset = tuple.Item2;
                int stride = tuple.Item3;
                int position = tuple.Item4;
                for (int i = 0; i < count; i++)
                {
                    int offset = startOffset + i * stride + position;
                    if (flipX)
                    {
                        float x = BitConverter.ToSingle(parsedMdxData, offset);
                        byte[] xBytes = BitConverter.GetBytes(-x);
                        Array.Copy(xBytes, 0, parsedMdxData, offset, 4);
                    }
                    if (flipY)
                    {
                        float y = BitConverter.ToSingle(parsedMdxData, offset + 4);
                        byte[] yBytes = BitConverter.GetBytes(-y);
                        Array.Copy(yBytes, 0, parsedMdxData, offset + 4, 4);
                    }
                }
            }

            // Update the MDX normals
            foreach (var tuple in mdxNormalOffsets)
            {
                int count = tuple.Item1;
                int startOffset = tuple.Item2;
                int stride = tuple.Item3;
                int position = tuple.Item4;
                for (int i = 0; i < count; i++)
                {
                    int offset = startOffset + i * stride + position;
                    if (flipX)
                    {
                        float x = BitConverter.ToSingle(parsedMdxData, offset);
                        byte[] xBytes = BitConverter.GetBytes(-x);
                        Array.Copy(xBytes, 0, parsedMdxData, offset, 4);
                    }
                    if (flipY)
                    {
                        float y = BitConverter.ToSingle(parsedMdxData, offset + 4);
                        byte[] yBytes = BitConverter.GetBytes(-y);
                        Array.Copy(yBytes, 0, parsedMdxData, offset + 4, 4);
                    }
                }
            }

            // Re-add the first 12 bytes
            byte[] resultMdl = new byte[mdlStart.Length + parsedMdlData.Length];
            Array.Copy(mdlStart, 0, resultMdl, 0, mdlStart.Length);
            Array.Copy(parsedMdlData, 0, resultMdl, mdlStart.Length, parsedMdlData.Length);

            return new MDLMDXTuple(resultMdl, parsedMdxData);
        }
    }
}
