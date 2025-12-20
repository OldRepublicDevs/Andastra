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

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/model.py:638-721
        // Original: def transform(data: bytes | bytearray, translation: Vector3, rotation: float) -> bytes | bytearray:
        /// <summary>
        /// Transforms a model by injecting a new transform node that applies translation and rotation.
        /// This creates a parent node that applies the transformation to the entire model hierarchy.
        /// </summary>
        /// <param name="data">The MDL model data (with 12-byte header: unused, mdl_size, mdx_size).</param>
        /// <param name="translation">The translation to apply (X, Y, Z).</param>
        /// <param name="rotation">The rotation angle in degrees (around Z-axis).</param>
        /// <returns>The transformed MDL data with the new transform node injected.</returns>
        public static byte[] Transform(byte[] data, System.Numerics.Vector3 translation, float rotation)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (data.Length < 12)
            {
                throw new ArgumentException("Invalid MDL data: must be at least 12 bytes", nameof(data));
            }

            // Create quaternion orientation from rotation (Z-axis rotation, roll=0, pitch=0, yaw=rotation)
            // Matching Python: orientation: Vector4 = Vector4.from_euler(0, 0, math.radians(rotation))
            float rotationRadians = (float)(rotation * Math.PI / 180.0);
            Andastra.Utility.Geometry.Quaternion orientation = QuaternionFromEuler(0.0, 0.0, rotationRadians);

            // Read MDX size from offset 8-12 (4 bytes)
            // Matching Python: mdx_size: int = struct.unpack("I", data[8:12])[0]
            uint mdxSize = BitConverter.ToUInt32(data, 8);

            // Extract parsed data starting at offset 12
            // Matching Python: parsed_data: bytearray = bytearray(data[12:])
            byte[] parsedData = new byte[data.Length - 12];
            Array.Copy(data, 12, parsedData, 0, parsedData.Length);

            if (parsedData.Length < 180)
            {
                // Not enough data to have a root node, return original
                return data;
            }

            uint nodeCount;
            uint rootOffset;
            uint childArrayOffset;
            uint childCount;

            using (BinaryReader reader = BinaryReader.FromBytes(parsedData, 0))
            {
                // Read node count at offset 44 (relative to parsed data)
                // Matching Python: reader.seek(44); node_count: int = reader.read_uint32()
                reader.Seek(44);
                nodeCount = reader.ReadUInt32();

                // Read root offset at offset 168 (relative to parsed data)
                // Matching Python: reader.seek(168); root_offset: int = reader.read_uint32()
                reader.Seek(168);
                rootOffset = reader.ReadUInt32();

                if (rootOffset >= parsedData.Length)
                {
                    // Invalid root offset, return original
                    return data;
                }

                // Read root node header (skip various fields)
                // Matching Python: reader.seek(root_offset); reader.read_uint16(); reader.read_uint16(); reader.read_uint32(); reader.skip(6); reader.skip(4); reader.skip(4); reader.skip(4 * 3); reader.skip(4 * 4);
                reader.Seek((int)rootOffset);
                reader.ReadUInt16(); // Skip first uint16
                reader.ReadUInt16(); // Skip second uint16
                reader.ReadUInt32(); // Skip uint32
                reader.Skip(6); // Skip 6 bytes
                reader.Skip(4); // Skip 4 bytes
                reader.Skip(4); // Skip 4 bytes
                reader.Skip(4 * 3); // Skip 3 floats (12 bytes)
                reader.Skip(4 * 4); // Skip 4 floats (16 bytes)

                // Read child array offset and count at root_offset + 44
                // Matching Python: reader.seek(root_offset + 44); child_array_offset: int = reader.read_uint32(); child_count: int = reader.ReadUInt32()
                reader.Seek((int)(rootOffset + 44));
                childArrayOffset = reader.ReadUInt32();
                childCount = reader.ReadUInt32();
            }

            // If no children, return original data (no transformation needed)
            // Matching Python: if child_count == 0: return parsed_data
            if (childCount == 0)
            {
                return data;
            }

            // Calculate offsets for injected data
            // Matching Python: root_child_array_offset: int = len(parsed_data)
            int rootChildArrayOffset = parsedData.Length;
            // Matching Python: insert_node_offset: int = len(parsed_data) + 4
            int insertNodeOffset = parsedData.Length + 4;
            // Matching Python: insert_controller_offset: int = insert_node_offset + 80
            int insertControllerOffset = insertNodeOffset + 80;
            // Matching Python: insert_controller_data_offset: int = insert_controller_offset + 32
            int insertControllerDataOffset = insertControllerOffset + 32;

            // Increase global node count by 1
            // Matching Python: parsed_data[44:48] = struct.pack("I", node_count + 1)
            byte[] newNodeCountBytes = BitConverter.GetBytes(nodeCount + 1);
            Array.Copy(newNodeCountBytes, 0, parsedData, 44, 4);

            // Update the offset the array of child offsets to our injected array
            // Matching Python: parsed_data[root_offset + 44 : root_offset + 48] = struct.pack("I", root_child_array_offset)
            byte[] newChildArrayOffsetBytes = BitConverter.GetBytes((uint)rootChildArrayOffset);
            Array.Copy(newChildArrayOffsetBytes, 0, parsedData, (int)(rootOffset + 44), 4);

            // Set the root node to have 1 child
            // Matching Python: parsed_data[root_offset + 48 : root_offset + 52] = struct.pack("I", 1)
            // Matching Python: parsed_data[root_offset + 52 : root_offset + 56] = struct.pack("I", 1)
            byte[] oneBytes = BitConverter.GetBytes(1u);
            Array.Copy(oneBytes, 0, parsedData, (int)(rootOffset + 48), 4);
            Array.Copy(oneBytes, 0, parsedData, (int)(rootOffset + 52), 4);

            // Create new byte array with injected data
            // Start with existing parsed data
            List<byte> newParsedData = new List<byte>(parsedData);

            // Populate the injected new root child offsets array
            // It will contain our new node
            // Matching Python: parsed_data += struct.pack("I", insert_node_offset)
            newParsedData.AddRange(BitConverter.GetBytes((uint)insertNodeOffset));

            // Create the new node
            // Matching Python: parsed_data += struct.pack("HHHH II fff ffff III III III", ...)
            // Node structure: 2+2+2+2 bytes (4 ushorts), 4+4 bytes (2 uints), 3 floats, 4 floats, 3 uints, 3 uints, 3 uints
            newParsedData.AddRange(BitConverter.GetBytes((ushort)1)); // Node Type
            newParsedData.AddRange(BitConverter.GetBytes((ushort)(nodeCount + 1))); // Node ID
            newParsedData.AddRange(BitConverter.GetBytes((ushort)1)); // Label ID (steal some existing node's label)
            newParsedData.AddRange(BitConverter.GetBytes((ushort)0)); // Padding
            newParsedData.AddRange(BitConverter.GetBytes(0u)); // Padding uint
            newParsedData.AddRange(BitConverter.GetBytes(rootOffset)); // Parent offset
            newParsedData.AddRange(BitConverter.GetBytes(translation.X)); // Node Position X
            newParsedData.AddRange(BitConverter.GetBytes(translation.Y)); // Node Position Y
            newParsedData.AddRange(BitConverter.GetBytes(translation.Z)); // Node Position Z
            newParsedData.AddRange(BitConverter.GetBytes(orientation.W)); // Node Orientation W
            newParsedData.AddRange(BitConverter.GetBytes(orientation.X)); // Node Orientation X
            newParsedData.AddRange(BitConverter.GetBytes(orientation.Y)); // Node Orientation Y
            newParsedData.AddRange(BitConverter.GetBytes(orientation.Z)); // Node Orientation Z
            newParsedData.AddRange(BitConverter.GetBytes((uint)childArrayOffset)); // Child Array Offset
            newParsedData.AddRange(BitConverter.GetBytes(childCount)); // Child Count
            newParsedData.AddRange(BitConverter.GetBytes(childCount)); // Child Count (duplicate)
            newParsedData.AddRange(BitConverter.GetBytes((uint)insertControllerOffset)); // Controller Array
            newParsedData.AddRange(BitConverter.GetBytes(2u)); // Controller Count
            newParsedData.AddRange(BitConverter.GetBytes(2u)); // Controller Count (duplicate)
            newParsedData.AddRange(BitConverter.GetBytes((uint)insertControllerDataOffset)); // Controller Data Array
            newParsedData.AddRange(BitConverter.GetBytes(9u)); // Controller Data Count
            newParsedData.AddRange(BitConverter.GetBytes(9u)); // Controller Data Count (duplicate)

            // Inject controller and controller data of new node to the end of the file
            // Matching Python: parsed_data += struct.pack("IHHHHBBBB", 8, 0xFFFF, 1, 0, 1, 3, 0, 0, 0)
            newParsedData.AddRange(BitConverter.GetBytes(8u)); // Controller type (position)
            newParsedData.AddRange(BitConverter.GetBytes((ushort)0xFFFF)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)1)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)0)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)1)); // Unknown
            newParsedData.Add((byte)3); // Unknown
            newParsedData.Add((byte)0); // Unknown
            newParsedData.Add((byte)0); // Unknown
            newParsedData.Add((byte)0); // Unknown

            // Matching Python: parsed_data += struct.pack("IHHHHBBBB", 20, 0xFFFF, 1, 4, 5, 4, 0, 0, 0)
            newParsedData.AddRange(BitConverter.GetBytes(20u)); // Controller type (orientation)
            newParsedData.AddRange(BitConverter.GetBytes((ushort)0xFFFF)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)1)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)4)); // Unknown
            newParsedData.AddRange(BitConverter.GetBytes((ushort)5)); // Unknown
            newParsedData.Add((byte)4); // Unknown
            newParsedData.Add((byte)0); // Unknown
            newParsedData.Add((byte)0); // Unknown
            newParsedData.Add((byte)0); // Unknown

            // Matching Python: parsed_data += struct.pack("ffff", 0.0, *translation)
            newParsedData.AddRange(BitConverter.GetBytes(0.0f)); // Time
            newParsedData.AddRange(BitConverter.GetBytes(translation.X)); // Translation X
            newParsedData.AddRange(BitConverter.GetBytes(translation.Y)); // Translation Y
            newParsedData.AddRange(BitConverter.GetBytes(translation.Z)); // Translation Z

            // Matching Python: parsed_data += struct.pack("fffff", 0.0, *orientation)
            newParsedData.AddRange(BitConverter.GetBytes(0.0f)); // Time
            newParsedData.AddRange(BitConverter.GetBytes(orientation.W)); // Orientation W
            newParsedData.AddRange(BitConverter.GetBytes(orientation.X)); // Orientation X
            newParsedData.AddRange(BitConverter.GetBytes(orientation.Y)); // Orientation Y
            newParsedData.AddRange(BitConverter.GetBytes(orientation.Z)); // Orientation Z

            byte[] finalParsedData = newParsedData.ToArray();

            // Return with header prepended
            // Matching Python: return struct.pack("III", 0, len(parsed_data), mdx_size) + parsed_data
            byte[] result = new byte[12 + finalParsedData.Length];
            Array.Copy(BitConverter.GetBytes(0u), 0, result, 0, 4); // Unused (always 0)
            Array.Copy(BitConverter.GetBytes((uint)finalParsedData.Length), 0, result, 4, 4); // MDL size
            Array.Copy(BitConverter.GetBytes(mdxSize), 0, result, 8, 4); // MDX size
            Array.Copy(finalParsedData, 0, result, 12, finalParsedData.Length); // Parsed data

            return result;
        }

        /// <summary>
        /// Creates a quaternion from Euler angles (roll, pitch, yaw).
        /// Matching PyKotor implementation at utility/common/geometry.py:887-914
        /// </summary>
        /// <param name="roll">Rotation around X axis in radians.</param>
        /// <param name="pitch">Rotation around Y axis in radians.</param>
        /// <param name="yaw">Rotation around Z axis in radians.</param>
        /// <returns>A quaternion representing the rotation.</returns>
        private static Andastra.Utility.Geometry.Quaternion QuaternionFromEuler(double roll, double pitch, double yaw)
        {
            // Matching Python implementation: Vector4.from_euler
            double qx = Math.Sin(roll / 2) * Math.Cos(pitch / 2) * Math.Cos(yaw / 2) - Math.Cos(roll / 2) * Math.Sin(pitch / 2) * Math.Sin(yaw / 2);
            double qy = Math.Cos(roll / 2) * Math.Sin(pitch / 2) * Math.Cos(yaw / 2) + Math.Sin(roll / 2) * Math.Cos(pitch / 2) * Math.Sin(yaw / 2);
            double qz = Math.Cos(roll / 2) * Math.Cos(pitch / 2) * Math.Sin(yaw / 2) - Math.Sin(roll / 2) * Math.Sin(pitch / 2) * Math.Cos(yaw / 2);
            double qw = Math.Cos(roll / 2) * Math.Cos(pitch / 2) * Math.Cos(yaw / 2) + Math.Sin(roll / 2) * Math.Sin(pitch / 2) * Math.Sin(yaw / 2);

            return new Andastra.Utility.Geometry.Quaternion((float)qx, (float)qy, (float)qz, (float)qw);
        }
    }
}
