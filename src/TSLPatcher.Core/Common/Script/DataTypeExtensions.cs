using System;

namespace TSLPatcher.Core.Common.Script;

/// <summary>
/// Extension methods for DataType enum.
/// 1:1 port from Python DataType methods in pykotor/common/script.py
/// </summary>
public static class DataTypeExtensions
{
    /// <summary>
    /// Get the size in bytes for a data type.
    /// </summary>
    public static int Size(this DataType dataType)
    {
        if (dataType == DataType.Void)
        {
            return 0;
        }
        if (dataType == DataType.Vector)
        {
            return 12;
        }
        if (dataType == DataType.Struct)
        {
            throw new ArgumentException("Structs are variable size");
        }
        return 4;
    }

    /// <summary>
    /// Get the size in bytes for a data type (alias for Size).
    /// </summary>
    public static int GetSize(this DataType dataType)
    {
        return Size(dataType);
    }

    /// <summary>
    /// Convert DataType to its script string representation.
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
