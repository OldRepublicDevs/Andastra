using System;
using System.Collections.Generic;
using System.Numerics;
using BioWare.Resource.Formats.MDLData;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Converts BioWare MDL node/mesh data into a render-friendly representation.
    /// This intentionally mirrors the KotOR.js and Odyssey renderer mesh traversal pattern.
    /// </summary>
    internal static class MdlToDrawableConverter
    {
        internal sealed class DrawableModel
        {
            public List<DrawableMesh> Meshes { get; } = new List<DrawableMesh>();
            public string Name { get; set; } = string.Empty;
        }

        internal sealed class DrawableMesh
        {
            public string TextureName { get; set; } = string.Empty;
            public Vector3[] Positions { get; set; } = Array.Empty<Vector3>();
            public Vector3[] Normals { get; set; } = Array.Empty<Vector3>();
            public Vector2[] Uvs { get; set; } = Array.Empty<Vector2>();
            public int[] Indices { get; set; } = Array.Empty<int>();
            public Matrix4x4 WorldTransform { get; set; } = Matrix4x4.Identity;
        }

        internal static DrawableModel Convert(MDL mdl)
        {
            var drawable = new DrawableModel
            {
                Name = mdl?.Name ?? string.Empty,
            };

            if (mdl?.Root == null)
            {
                return drawable;
            }

            TraverseNode(mdl.Root, Matrix4x4.Identity, drawable.Meshes);
            return drawable;
        }

        private static void TraverseNode(MDLNode node, Matrix4x4 parentTransform, List<DrawableMesh> output)
        {
            if (node == null)
            {
                return;
            }

            Matrix4x4 local = CalculateNodeTransform(node);
            Matrix4x4 world = Matrix4x4.Multiply(local, parentTransform);

            if (node.Mesh != null)
            {
                DrawableMesh converted = ConvertMesh(node.Mesh, world);
                if (converted != null)
                {
                    output.Add(converted);
                }
            }

            if (node.Children == null)
            {
                return;
            }

            foreach (MDLNode child in node.Children)
            {
                TraverseNode(child, world, output);
            }
        }

        private static Matrix4x4 CalculateNodeTransform(MDLNode node)
        {
            Vector3 translation = node.Position;
            Vector4 o = node.Orientation;

            Quaternion q = Quaternion.Identity;
            if (!(o.X == 0 && o.Y == 0 && o.Z == 0 && o.W == 0))
            {
                q = new Quaternion(o.X, o.Y, o.Z, o.W);
                if (q.LengthSquared() > 0.000001f)
                {
                    q = Quaternion.Normalize(q);
                }
                else
                {
                    q = Quaternion.Identity;
                }
            }

            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(q);
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(translation);

            // Keep the same order as the existing Odyssey renderer in this repository.
            return Matrix4x4.Multiply(translationMatrix, rotationMatrix);
        }

        private static DrawableMesh ConvertMesh(MDLMesh mesh, Matrix4x4 worldTransform)
        {
            if (mesh?.Vertices == null || mesh.Vertices.Count == 0)
            {
                return null;
            }

            if (mesh.Faces == null || mesh.Faces.Count == 0)
            {
                return null;
            }

            var positions = new Vector3[mesh.Vertices.Count];
            var normals = new Vector3[mesh.Vertices.Count];
            var uvs = new Vector2[mesh.Vertices.Count];

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                positions[i] = mesh.Vertices[i];
                normals[i] = (mesh.Normals != null && i < mesh.Normals.Count)
                    ? mesh.Normals[i]
                    : Vector3.UnitY;
                uvs[i] = (mesh.UV1 != null && i < mesh.UV1.Count)
                    ? mesh.UV1[i]
                    : Vector2.Zero;
            }

            var indices = new List<int>(mesh.Faces.Count * 3);
            foreach (MDLFace face in mesh.Faces)
            {
                if (face == null)
                {
                    continue;
                }

                if (!IsValidIndex(face.V1, positions.Length) ||
                    !IsValidIndex(face.V2, positions.Length) ||
                    !IsValidIndex(face.V3, positions.Length))
                {
                    continue;
                }

                indices.Add(face.V1);
                indices.Add(face.V2);
                indices.Add(face.V3);
            }

            if (indices.Count == 0)
            {
                return null;
            }

            string textureName = mesh.Texture1 ?? string.Empty;
            if (textureName.Equals("NULL", StringComparison.OrdinalIgnoreCase) ||
                textureName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
            {
                textureName = string.Empty;
            }

            return new DrawableMesh
            {
                TextureName = textureName,
                Positions = positions,
                Normals = normals,
                Uvs = uvs,
                Indices = indices.ToArray(),
                WorldTransform = worldTransform,
            };
        }

        private static bool IsValidIndex(int index, int length)
        {
            return index >= 0 && index < length;
        }
    }
}
