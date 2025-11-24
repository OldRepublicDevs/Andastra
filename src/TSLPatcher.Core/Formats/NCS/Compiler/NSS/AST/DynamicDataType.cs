using System;
using JetBrains.Annotations;
using TSLPatcher.Core.Common.Script;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a data type that can be either a built-in type or a user-defined struct.
    /// </summary>
    public class DynamicDataType : IEquatable<DynamicDataType>
    {
        public static readonly DynamicDataType INT = new DynamicDataType(DataType.Int);
        public static readonly DynamicDataType STRING = new DynamicDataType(DataType.String);
        public static readonly DynamicDataType FLOAT = new DynamicDataType(DataType.Float);
        public static readonly DynamicDataType OBJECT = new DynamicDataType(DataType.Object);
        public static readonly DynamicDataType VECTOR = new DynamicDataType(DataType.Vector);
        public static readonly DynamicDataType VOID = new DynamicDataType(DataType.Void);
        public static readonly DynamicDataType EVENT = new DynamicDataType(DataType.Event);
        public static readonly DynamicDataType TALENT = new DynamicDataType(DataType.Talent);
        public static readonly DynamicDataType LOCATION = new DynamicDataType(DataType.Location);
        public static readonly DynamicDataType EFFECT = new DynamicDataType(DataType.Effect);

        public DataType Builtin { get; set; }
        [CanBeNull]
        public string Struct { get; set; }

        public DynamicDataType(DataType builtin, [CanBeNull] string structName = null)
        {
            Builtin = builtin;
            Struct = structName;
        }

        public int Size(CodeRoot root)
        {
            if (Builtin == DataType.Struct && Struct != null)
            {
                // Can be null if struct not found
                if (root.StructMap.TryGetValue(Struct, out Struct structDef))
                {
                    return structDef.Size(root);
                }
                throw new CompileError($"Unknown struct type: {Struct}");
            }
            return Builtin.GetSize();
        }

        public bool Equals([CanBeNull] DynamicDataType other)
        {
            if (other is null)
            {
                return false;
            }


            if (ReferenceEquals(this, other))
            {
                return true;
            }


            if (Builtin != other.Builtin)
            {
                return false;
            }


            if (Builtin == DataType.Struct)
            {
                return Struct == other.Struct;
            }
            return true;
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (obj is DynamicDataType dt)
            {
                return Equals(dt);
            }


            if (obj is DataType t)
            {
                return Builtin == t && Builtin != DataType.Struct;
            }


            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Builtin, Struct);
        }

        public override string ToString()
        {
            if (Builtin == DataType.Struct && Struct != null)
            {
                return $"struct {Struct}";
            }
            return Builtin.ToScriptString();
        }

        public static bool operator ==([CanBeNull] DynamicDataType left, [CanBeNull] DynamicDataType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=([CanBeNull] DynamicDataType left, [CanBeNull] DynamicDataType right)
        {
            return !Equals(left, right);
        }

        public static bool operator ==([CanBeNull] DynamicDataType left, DataType right)
        {
            return left?.Builtin == right && left?.Builtin != DataType.Struct;
        }

        public static bool operator !=([CanBeNull] DynamicDataType left, DataType right)
        {
            return !(left == right);
        }

        public static implicit operator DynamicDataType(DataType dataType)
        {
            return new DynamicDataType(dataType);
        }
    }
}

