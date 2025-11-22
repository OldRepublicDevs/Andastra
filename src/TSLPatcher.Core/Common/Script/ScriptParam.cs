using System;

namespace TSLPatcher.Core.Common.Script;

/// <summary>
/// Represents a parameter in a NWScript function signature.
/// 
/// References:
///     vendor/xoreos-tools/src/nwscript/ (NWScript parameter definitions)
///     vendor/KotOR.js/src/nwscript/NWScriptDefK1.ts (K1 function parameters)
/// </summary>
public class ScriptParam
{
    public DataType DataType { get; set; }
    public string Name { get; set; }
    public object? Default { get; set; }

    public ScriptParam(DataType dataType, string name, object? defaultValue = null)
    {
        DataType = dataType;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Default = defaultValue;
    }

    public override string ToString()
    {
        if (Default != null)
        {
            return $"{DataType.ToScriptString()} {Name} = {Default}";
        }
        return $"{DataType.ToScriptString()} {Name}";
    }
}

