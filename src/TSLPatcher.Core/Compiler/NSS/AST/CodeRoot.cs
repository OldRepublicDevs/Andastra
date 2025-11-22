using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST;

/// <summary>
/// Root compilation context for NSS compilation.
///
/// Manages global scope, function definitions, constants, and compilation state.
/// Provides symbol resolution and type checking during NSS to NCS compilation.
///
/// References:
///     vendor/KotOR.js/src/nwscript/NWScriptCompiler.ts (TypeScript compiler architecture)
///     vendor/xoreos-tools/src/nwscript/decompiler.cpp (NCS decompiler, reverse reference for compilation)
///     vendor/HoloLSP/server/src/nwscript-parser.ts (NSS parser and AST generation)
/// </summary>
public class CodeRoot
{
    public List<TopLevelObject> Objects { get; set; }
    public Dictionary<string, byte[]> Library { get; set; }
    public List<ScriptFunction> Functions { get; set; }
    public List<ScriptConstant> Constants { get; set; }
    public List<string> LibraryLookup { get; set; }
    public Dictionary<string, Struct> StructMap { get; set; }
    public Dictionary<Identifier, (DynamicDataType DataType, int Offset)> GlobalScope { get; set; }
    public Dictionary<Identifier, FunctionDefinition> FunctionMap { get; set; }
    public int GlobalStackSize { get; set; }

    public CodeRoot(
        List<ScriptConstant> constants,
        List<ScriptFunction> functions,
        IEnumerable<string>? libraryLookup,
        Dictionary<string, byte[]> library)
    {
        Objects = new List<TopLevelObject>();
        Library = library ?? new Dictionary<string, byte[]>();
        Functions = functions ?? new List<ScriptFunction>();
        Constants = constants ?? new List<ScriptConstant>();
        LibraryLookup = libraryLookup?.ToList() ?? new List<string>();
        StructMap = new Dictionary<string, Struct>();
        GlobalScope = new Dictionary<Identifier, (DynamicDataType, int)>();
        FunctionMap = new Dictionary<Identifier, FunctionDefinition>();
        GlobalStackSize = 0;
    }

    public void AddScoped(Identifier identifier, DynamicDataType dataType)
    {
        int size = dataType.Size(this);
        GlobalScope[identifier] = (dataType, GlobalStackSize);
        GlobalStackSize += size;
    }

    public (bool IsGlobal, DynamicDataType DataType, int Offset) GetScoped(Identifier identifier, CodeBlock? block)
    {
        if (block != null && block.TryGetLocal(identifier, out (DynamicDataType DataType, int Offset) local))
        {
            return (false, local.DataType, local.Offset);
        }

        if (GlobalScope.TryGetValue(identifier, out (DynamicDataType DataType, int Offset) global))
        {
            return (true, global.DataType, global.Offset);
        }

        throw new CompileError($"Undefined variable: {identifier}");
    }

    public void Compile(NCS ncs)
    {
        foreach (TopLevelObject obj in Objects)
        {
            obj.Compile(ncs, this);
        }

        if (!FunctionMap.ContainsKey(new Identifier("main")) &&
            !FunctionMap.ContainsKey(new Identifier("StartingConditional")))
        {
            throw new EntryPointError(
                "Script must have either a 'main()' or 'StartingConditional()' function as entry point");
        }
    }

    public ScriptFunction? FindEngineFunction(string name)
    {
        return Functions.FirstOrDefault(f => f.Name == name);
    }

    public ScriptConstant? FindConstant(string name)
    {
        return Constants.FirstOrDefault(c => c.Name == name);
    }

    public byte[]? FindInclude(string includePath)
    {
        if (Library.TryGetValue(includePath, out byte[]? data))
        {
            return data;
        }

        foreach (string lookupPath in LibraryLookup)
        {
            string fullPath = Path.Combine(lookupPath, includePath);
            if (File.Exists(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }
        }

        return null;
    }
}

/// <summary>
/// Represents a user-defined struct type.
/// </summary>
public class Struct
{
    public Identifier Identifier { get; set; }
    public List<StructMember> Members { get; set; }
    private int? _cachedSize;

    public Struct(Identifier identifier, List<StructMember> members)
    {
        Identifier = identifier;
        Members = members;
    }

    public void Initialize(NCS ncs, CodeRoot root)
    {
        foreach (StructMember member in Members)
        {
            member.Initialize(ncs, root);
        }
    }

    public int Size(CodeRoot root)
    {
        if (_cachedSize.HasValue)
        {
            return _cachedSize.Value;
        }

        _cachedSize = Members.Sum(m => m.Size(root));
        return _cachedSize.Value;
    }

    public int ChildOffset(CodeRoot root, Identifier identifier)
    {
        int size = 0;
        foreach (StructMember member in Members)
        {
            if (member.Identifier.Equals(identifier))
            {
                return size;
            }
            size += member.Size(root);
        }

        string available = string.Join(", ", Members.Select(m => m.Identifier.Label));
        throw new CompileError(
            $"Unknown member '{identifier}' in struct '{Identifier}'\n" +
            $"  Available members: {available}");
    }

    public DynamicDataType ChildType(CodeRoot root, Identifier identifier)
    {
        foreach (StructMember member in Members)
        {
            if (member.Identifier.Equals(identifier))
            {
                return member.DataType;
            }
        }

        string available = string.Join(", ", Members.Select(m => m.Identifier.Label));
        throw new CompileError(
            $"Member '{identifier}' not found in struct '{Identifier}'\n" +
            $"  Available members: {available}");
    }
}

/// <summary>
/// Represents a member of a user-defined struct.
/// </summary>
public class StructMember
{
    public DynamicDataType DataType { get; set; }
    public Identifier Identifier { get; set; }

    public StructMember(DynamicDataType dataType, Identifier identifier)
    {
        DataType = dataType;
        Identifier = identifier;
    }

    public void Initialize(NCS ncs, CodeRoot root)
    {
        switch (DataType.Builtin)
        {
            case Common.Script.DataType.Int:
                ncs.Add(NCSInstructionType.RSADDI, new List<object>());
                break;
            case Common.Script.DataType.Float:
                ncs.Add(NCSInstructionType.RSADDF, new List<object>());
                break;
            case Common.Script.DataType.String:
                ncs.Add(NCSInstructionType.RSADDS, new List<object>());
                break;
            case Common.Script.DataType.Object:
                ncs.Add(NCSInstructionType.RSADDO, new List<object>());
                break;
            case Common.Script.DataType.Event:
                ncs.Add(NCSInstructionType.RSADDEVT, new List<object>());
                break;
            case Common.Script.DataType.Effect:
                ncs.Add(NCSInstructionType.RSADDEFF, new List<object>());
                break;
            case Common.Script.DataType.Location:
                ncs.Add(NCSInstructionType.RSADDLOC, new List<object>());
                break;
            case Common.Script.DataType.Talent:
                ncs.Add(NCSInstructionType.RSADDTAL, new List<object>());
                break;
            case Common.Script.DataType.Vector:
                ncs.Add(NCSInstructionType.RSADDF, new List<object>());
                ncs.Add(NCSInstructionType.RSADDF, new List<object>());
                ncs.Add(NCSInstructionType.RSADDF, new List<object>());
                break;
            case Common.Script.DataType.Struct:
                if (DataType.Struct != null && root.StructMap.TryGetValue(DataType.Struct, out Struct? structDef))
                {
                    structDef.Initialize(ncs, root);
                }
                else
                {
                    throw new CompileError($"Unknown struct type for member '{Identifier}'");
                }
                break;
            default:
                throw new CompileError($"Unsupported struct member type: {DataType.Builtin}");
        }
    }

    public int Size(CodeRoot root)
    {
        return DataType.Size(root);
    }
}

/// <summary>
/// Placeholder for FunctionDefinition - will be fully implemented in next batch.
/// </summary>
public class FunctionDefinition : TopLevelObject
{
    public Identifier Name { get; set; }
    public DynamicDataType ReturnType { get; set; }
    public List<FunctionParameter> Parameters { get; set; }
    public CodeBlock Body { get; set; }

    public FunctionDefinition(Identifier name, DynamicDataType returnType, List<FunctionParameter> parameters, CodeBlock body)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }

    public override void Compile(NCS ncs, CodeRoot root)
    {
        throw new NotImplementedException("FunctionDefinition.Compile - to be implemented");
    }
}

/// <summary>
/// Placeholder for function parameter.
/// </summary>
public class FunctionParameter
{
    public Identifier Name { get; set; }
    public DynamicDataType DataType { get; set; }
    public Expression? DefaultValue { get; set; }

    public FunctionParameter(Identifier name, DynamicDataType dataType, Expression? defaultValue = null)
    {
        Name = name;
        DataType = dataType;
        DefaultValue = defaultValue;
    }
}

