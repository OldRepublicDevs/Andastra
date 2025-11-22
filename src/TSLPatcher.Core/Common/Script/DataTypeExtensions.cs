using System;

namespace TSLPatcher.Core.Common.Script;

/// <summary>
/// Extension methods for DataType enum.
/// </summary>
public static class DataTypeExtensions
{
    /// <summary>
    /// Calculates the size of the data type in bytes.
    /// </summary>
    public static int GetSize(this DataType dataType)
    {
        switch (dataType)
        {
            case DataType.Void:
                return 0;
            case DataType.Vector:
                return 12; // 3 floats * 4 bytes/float
            case DataType.Struct:
                throw new InvalidOperationException("Structs have variable size and must be determined by context.");
            default:
                return 4; // Most other types are 4 bytes (int, float, object, string reference, etc.)
        }
    }

    /// <summary>
    /// Gets the script string representation of the data type.
    /// </summary>
    public static string ToScriptString(this DataType dataType)
    {
        return dataType switch
        {
            DataType.Void => "void",
            DataType.Int => "int",
            DataType.Float => "float",
            DataType.String => "string",
            DataType.Object => "object",
            DataType.Vector => "vector",
            DataType.Location => "location",
            DataType.Event => "event",
            DataType.Effect => "effect",
            DataType.ItemProperty => "itemproperty",
            DataType.Talent => "talent",
            DataType.Action => "action",
            DataType.Struct => "struct",
            _ => dataType.ToString().ToLowerInvariant()
        };
    }
}

