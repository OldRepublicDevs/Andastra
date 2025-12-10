using System;
using JetBrains.Annotations;

namespace CSharpKOTOR.Common
{

    /// <summary>
    /// Represents a 3 dimensional vector.
    /// </summary>
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Zero => new Vector3(0f, 0f, 0f);
        public static Vector3 FromNull() => Zero;

        public static Vector3 FromVector2(Vector2 v)
        {
            return new Vector3(v.X, v.Y, 0f);
        }

        public void SetVectorCoords(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Magnitude()
        {
            return MathF.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Normalize()
        {
            float magnitude = Magnitude();
            if (magnitude == 0)
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            else
            {
                X /= magnitude;
                Y /= magnitude;
                Z /= magnitude;
            }
            return this;
        }

        public Vector3 Normal()
        {
            Vector3 result = new Vector3(X, Y, Z);
            return result.Normalize();
        }

        public float Dot(Vector3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public float Distance(Vector3 other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            float dz = Z - other.Z;
            return MathF.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        // Matching PyKotor implementation - cross product for ray-triangle intersection
        // Original: def cross(a: Vector3, b: Vector3) -> Vector3
        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public override string ToString() => $"{X} {Y} {Z}";

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public override bool Equals([CanBeNull] object obj)
        {
            return obj is Vector3 other && Equals(other);
        }

        public bool Equals(Vector3 other)
        {
            const float epsilon = 1e-9f;
            return Math.Abs(X - other.X) < epsilon &&
                   Math.Abs(Y - other.Y) < epsilon &&
                   Math.Abs(Z - other.Z) < epsilon;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
            => new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3 operator -(Vector3 a, Vector3 b)
            => new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3 operator *(Vector3 v, float scalar)
            => new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static Vector3 operator /(Vector3 v, float scalar)
            => new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);

        public static bool operator ==(Vector3 left, Vector3 right)
            => left.Equals(right);

        public static bool operator !=(Vector3 left, Vector3 right)
            => !left.Equals(right);

        // Matching PyKotor implementation - indexer for accessing components by index
        // Original: def __getitem__(self, item: int) -> float
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
    }

    // Matching PyKotor implementation - extension method for checking if vector is in list by identity
    // Original: def within(self, container: list) -> bool
    public static class Vector3Extensions
    {
        public static bool Within(this Vector3 vector, System.Collections.Generic.IList<Vector3> container)
        {
            // For structs, we check by value equality since identity doesn't apply
            // This matches the Python behavior where Vector3 objects with same coordinates are considered equal
            foreach (var item in container)
            {
                if (vector.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

