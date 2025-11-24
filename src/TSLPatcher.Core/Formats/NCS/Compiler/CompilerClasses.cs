using System;
using System.Collections.Generic;
using System.Linq;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Common.Script;

namespace TSLPatcher.Core.Formats.NCS.Compiler;

/// <summary>
/// Base exception for NSS compilation errors.
/// Provides detailed error messages to help debug script issues.
/// 
/// References:
///     vendor/HoloLSP/server/src/nwscript-parser.ts (NSS parser error handling)
///     vendor/xoreos-tools/src/nwscript/compiler.cpp (NSS compiler error handling)
///     vendor/KotOR.js/src/nwscript/NWScriptCompiler.ts (TypeScript compiler errors)
/// </summary>
public class CompileError : Exception
{
    public int? LineNum { get; }
    public string? Context { get; }

    public CompileError(string message, int? lineNum = null, string? context = null)
        : base(FormatMessage(message, lineNum, context))
    {
        LineNum = lineNum;
        Context = context;
    }

    private static string FormatMessage(string message, int? lineNum, string? context)
    {
        string fullMessage = message;
        if (lineNum != null)
        {
            fullMessage = $"Line {lineNum}: {message}";
        }
        if (context != null)
        {
            fullMessage = $"{fullMessage}\n  Context: {context}";
        }
        return fullMessage;
    }
}

/// <summary>Raised when script has no valid entry point (main or StartingConditional).</summary>
public class EntryPointError : CompileError
{
    public EntryPointError(string message, int? lineNum = null, string? context = null)
        : base(message, lineNum, context)
    {
    }
}

/// <summary>Raised when a #include file cannot be found.</summary>
public class MissingIncludeError : CompileError
{
    public MissingIncludeError(string message, int? lineNum = null, string? context = null)
        : base(message, lineNum, context)
    {
    }
}

/// <summary>
/// Identifier in NSS source code (variable names, function names, etc.).
/// </summary>
public class Identifier
{
    public string Label { get; }

    public Identifier(string label)
    {
        Label = label;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Identifier other) return Label == other.Label;
        if (obj is string str) return Label == str;
        return false;
    }

    public override int GetHashCode() => Label.GetHashCode();
    public override string ToString() => Label;
}

/// <summary>
/// Control keywords in NSS (if, while, for, etc.).
/// </summary>
public enum ControlKeyword
{
    BREAK,
    CASE,
    DEFAULT,
    DO,
    ELSE,
    SWITCH,
    WHILE,
    FOR,
    IF,
    RETURN
}

/// <summary>
/// Operator enumeration for NSS expressions.
/// </summary>
public enum Operator
{
    ADDITION,
    SUBTRACT,
    MULTIPLY,
    DIVIDE,
    MODULUS,
    NOT,
    EQUAL,
    NOT_EQUAL,
    GREATER_THAN,
    LESS_THAN,
    GREATER_THAN_OR_EQUAL,
    LESS_THAN_OR_EQUAL,
    AND,
    OR,
    BITWISE_AND,
    BITWISE_OR,
    BITWISE_XOR,
    BITWISE_LEFT,
    BITWISE_RIGHT,
    ONES_COMPLEMENT
}

/// <summary>
/// Mapping of operator tokens to NCS instructions for unary operations.
/// </summary>
public class UnaryOperatorMapping
{
    public NCSInstructionType Instruction { get; }
    public DataType Rhs { get; }

    public UnaryOperatorMapping(NCSInstructionType instruction, DataType rhs)
    {
        Instruction = instruction;
        Rhs = rhs;
    }

    public override string ToString() =>
        $"UnaryOperatorMapping(instruction={Instruction}, rhs={Rhs})";
}

/// <summary>
/// Mapping of operator tokens to NCS instructions for binary operations.
/// </summary>
public class BinaryOperatorMapping
{
    public NCSInstructionType Instruction { get; }
    public DataType Result { get; }
    public DataType Lhs { get; }
    public DataType Rhs { get; }

    public BinaryOperatorMapping(NCSInstructionType instruction, DataType result, DataType lhs, DataType rhs)
    {
        Instruction = instruction;
        Result = result;
        Lhs = lhs;
        Rhs = rhs;
    }

    public override string ToString() =>
        $"BinaryOperatorMapping(instruction={Instruction}, result={Result}, lhs={Lhs}, rhs={Rhs})";
}

/// <summary>
/// Operator mapping for tokens that can be both unary and binary.
/// </summary>
public class OperatorMapping
{
    public List<UnaryOperatorMapping> Unary { get; }
    public List<BinaryOperatorMapping> Binary { get; }

    public OperatorMapping(List<UnaryOperatorMapping> unary, List<BinaryOperatorMapping> binary)
    {
        Unary = unary;
        Binary = binary;
    }
}

/// <summary>
/// Dynamic data type that can represent built-in types or structs.
/// </summary>
public class DynamicDataType
{
    public static readonly DynamicDataType INT = new(DataType.Int);
    public static readonly DynamicDataType STRING = new(DataType.String);
    public static readonly DynamicDataType FLOAT = new(DataType.Float);
    public static readonly DynamicDataType OBJECT = new(DataType.Object);
    public static readonly DynamicDataType VECTOR = new(DataType.Vector);
    public static readonly DynamicDataType VOID = new(DataType.Void);
    public static readonly DynamicDataType EVENT = new(DataType.Event);
    public static readonly DynamicDataType TALENT = new(DataType.Talent);
    public static readonly DynamicDataType LOCATION = new(DataType.Location);
    public static readonly DynamicDataType EFFECT = new(DataType.Effect);

    public DataType Builtin { get; }
    public string? Struct { get; }

    public DynamicDataType(DataType datatype, string? structName = null)
    {
        Builtin = datatype;
        Struct = structName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is DynamicDataType other)
        {
            if (Builtin == other.Builtin)
            {
                return Builtin != DataType.Struct || (Builtin == DataType.Struct && Struct == other.Struct);
            }
            return false;
        }
        if (obj is DataType dt)
        {
            return Builtin == dt && Builtin != DataType.Struct;
        }
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Builtin, Struct);

    public override string ToString() =>
        $"DynamicDataType(builtin={Builtin}({Builtin.ToScriptString()}), struct={Struct})";

    public int Size(CodeRoot root)
    {
        if (Builtin == DataType.Struct)
        {
            if (Struct == null)
                throw new CompileError("Struct type has no name");
            return root.StructMap[Struct].Size(root);
        }
        return Builtin.Size();
    }
}

/// <summary>
/// Result of scoped variable lookup.
/// </summary>
public class GetScopedResult
{
    public bool IsGlobal { get; }
    public DynamicDataType Datatype { get; }
    public int Offset { get; }

    public GetScopedResult(bool isGlobal, DynamicDataType datatype, int offset)
    {
        IsGlobal = isGlobal;
        Datatype = datatype;
        Offset = offset;
    }
}

/// <summary>
/// Scoped variable in a code block or global scope.
/// </summary>
public class ScopedValue
{
    public Identifier Identifier { get; }
    public DynamicDataType DataType { get; }

    public ScopedValue(Identifier identifier, DynamicDataType dataType)
    {
        Identifier = identifier;
        DataType = dataType;
    }
}

/// <summary>
/// Reference to a function definition or forward declaration.
/// </summary>
public class FunctionReference
{
    public NCSInstruction Instruction { get; }
    public object Definition { get; } // FunctionForwardDeclaration or FunctionDefinition

    public FunctionReference(NCSInstruction instruction, object definition)
    {
        Instruction = instruction;
        Definition = definition;
    }

    public bool IsPrototype() => Definition is FunctionForwardDeclaration;
}

/// <summary>
/// Root compilation context for NSS compilation.
/// Manages global scope, function definitions, constants, and compilation state.
/// 
/// References:
///     vendor/KotOR.js/src/nwscript/NWScriptCompiler.ts (TypeScript compiler architecture)
///     vendor/xoreos-tools/src/nwscript/decompiler.cpp (NCS decompiler, reverse reference)
///     vendor/HoloLSP/server/src/nwscript-parser.ts (NSS parser and AST generation)
/// </summary>
public class CodeRoot
{
    public List<ITopLevelObject> Objects { get; set; } = new();
    public Dictionary<string, byte[]> Library { get; }
    public List<ScriptFunction> Functions { get; }
    public List<ScriptConstant> Constants { get; }
    public List<string> LibraryLookup { get; }
    public Dictionary<string, FunctionReference> FunctionMap { get; } = new();
    public Dictionary<string, Struct> StructMap { get; } = new();

    private readonly List<ScopedValue> _globalScope = new();

    public CodeRoot(
        List<ScriptConstant> constants,
        List<ScriptFunction> functions,
        object? libraryLookup,
        Dictionary<string, byte[]> library)
    {
        Constants = constants;
        Functions = functions;
        Library = library;
        LibraryLookup = new List<string>();

        if (libraryLookup != null)
        {
            if (libraryLookup is List<string> list)
            {
                LibraryLookup = list;
            }
            else if (libraryLookup is string str)
            {
                LibraryLookup = new List<string> { str };
            }
        }
    }

    public void AddScoped(Identifier identifier, DynamicDataType datatype)
    {
        _globalScope.Insert(0, new ScopedValue(identifier, datatype));
    }

    public GetScopedResult GetScoped(Identifier identifier, CodeRoot root)
    {
        int offset = 0;
        ScopedValue? found = null;

        foreach (ScopedValue scoped in _globalScope)
        {
            offset -= scoped.DataType.Size(root);
            if (scoped.Identifier.Equals(identifier))
            {
                found = scoped;
                break;
            }
        }

        if (found == null)
        {
            // Provide helpful error with available globals
            List<string> available = _globalScope.Take(10).Select(s => s.Identifier.Label).ToList();
            int more = _globalScope.Count - 10;
            string moreText = more > 0 ? $" (and {more} more)" : "";
            string msg = $"Undefined variable '{identifier}'\n" +
                         $"  Available globals: {string.Join(", ", available)}{moreText}";
            throw new CompileError(msg);
        }

        return new GetScopedResult(true, found.DataType, offset);
    }

    public int ScopeSize()
    {
        return -_globalScope.Sum(scoped => scoped.DataType.Size(this));
    }

    // Compile method to be implemented
    public void Compile(NCS ncs)
    {
        // TODO: Implement compilation logic
        throw new NotImplementedException("CodeRoot.Compile not yet implemented");
    }
}

/// <summary>
/// Top-level object in NSS source (functions, globals, includes, etc.).
/// </summary>
public interface ITopLevelObject
{
    void Compile(NCS ncs, CodeRoot root);
}

/// <summary>
/// Function forward declaration (prototype).
/// </summary>
public class FunctionForwardDeclaration : ITopLevelObject
{
    public DynamicDataType ReturnType { get; }
    public Identifier Identifier { get; }
    public List<FunctionDefinitionParam> Parameters { get; }

    public FunctionForwardDeclaration(
        DynamicDataType returnType,
        Identifier identifier,
        List<FunctionDefinitionParam> parameters)
    {
        ReturnType = returnType;
        Identifier = identifier;
        Parameters = parameters;
    }

    public void Compile(NCS ncs, CodeRoot root)
    {
        string functionName = Identifier.Label;

        if (root.FunctionMap.ContainsKey(Identifier.Label))
        {
            throw new CompileError($"Function '{functionName}' already has a prototype or been defined.");
        }

        root.FunctionMap[Identifier.Label] = new FunctionReference(
            ncs.Add(NCSInstructionType.NOP, new List<object>()),
            this
        );
    }
}

/// <summary>
/// Function definition parameter.
/// </summary>
public class FunctionDefinitionParam
{
    public DynamicDataType DataType { get; }
    public Identifier Identifier { get; }
    public IExpression? Default { get; }

    public FunctionDefinitionParam(DynamicDataType dataType, Identifier identifier, IExpression? defaultExpr = null)
    {
        DataType = dataType;
        Identifier = identifier;
        Default = defaultExpr;
    }
}

/// <summary>
/// Code block containing statements.
/// </summary>
public class CodeBlock
{
    public List<ScopedValue> Scope { get; } = new();
    public int TempStack { get; set; }

    private CodeBlock? _parent;
    private readonly List<IStatement> _statements = new();
    private bool _breakScope;

    public void Add(IStatement statement)
    {
        _statements.Add(statement);
    }

    public void AddScoped(Identifier identifier, DynamicDataType dataType)
    {
        Scope.Insert(0, new ScopedValue(identifier, dataType));
    }

    public GetScopedResult GetScoped(Identifier identifier, CodeRoot root, int? offset = null)
    {
        int currentOffset = offset ?? -TempStack;
        currentOffset -= TempStack;

        foreach (ScopedValue scoped in Scope)
        {
            currentOffset -= scoped.DataType.Size(root);
            if (scoped.Identifier.Equals(identifier))
            {
                return new GetScopedResult(false, scoped.DataType, currentOffset);
            }
        }

        if (_parent != null)
        {
            return _parent.GetScoped(identifier, root, currentOffset);
        }

        return root.GetScoped(identifier, root);
    }

    public int ScopeSize(CodeRoot root)
    {
        return Scope.Sum(scoped => scoped.DataType.Size(root));
    }

    public int FullScopeSize(CodeRoot root)
    {
        int size = ScopeSize(root);
        if (_parent != null)
        {
            size += _parent.FullScopeSize(root);
        }
        return size;
    }

    public int BreakScopeSize(CodeRoot root)
    {
        int size = ScopeSize(root);
        if (_parent != null && !_parent._breakScope)
        {
            size += _parent.BreakScopeSize(root);
        }
        return size;
    }

    public void MarkBreakScope()
    {
        _breakScope = true;
    }

    public void Compile(
        NCS ncs,
        CodeRoot root,
        CodeBlock? block,
        NCSInstruction returnInstruction,
        NCSInstruction? breakInstruction,
        NCSInstruction? continueInstruction)
    {
        _parent = block;

        // TODO: Implement block compilation
        throw new NotImplementedException("CodeBlock.Compile not yet implemented");
    }
}

/// <summary>
/// Base interface for expressions in NSS.
/// </summary>
public interface IExpression
{
    DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block);
}

/// <summary>
/// Base interface for statements in NSS.
/// </summary>
public interface IStatement
{
    object Compile(
        NCS ncs,
        CodeRoot root,
        CodeBlock block,
        NCSInstruction returnInstruction,
        NCSInstruction? breakInstruction,
        NCSInstruction? continueInstruction);
}

/// <summary>
/// Struct definition.
/// </summary>
public class Struct
{
    public Identifier Identifier { get; }
    public List<StructMember> Members { get; }
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
        if (_cachedSize == null)
        {
            _cachedSize = Members.Sum(member => member.Size(root));
        }
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

        List<string> available = Members.Select(m => m.Identifier.Label).ToList();
        string msg = $"Unknown member '{identifier}' in struct '{Identifier}'\n" +
                     $"  Available members: {string.Join(", ", available)}";
        throw new CompileError(msg);
    }

    public DynamicDataType ChildType(CodeRoot root, Identifier identifier)
    {
        foreach (StructMember member in Members)
        {
            if (member.Identifier.Equals(identifier))
            {
                return member.Datatype;
            }
        }

        List<string> available = Members.Select(m => m.Identifier.Label).ToList();
        string msg = $"Member '{identifier}' not found in struct '{Identifier}'\n" +
                     $"  Available members: {string.Join(", ", available)}";
        throw new CompileError(msg);
    }
}

/// <summary>
/// Member of a struct definition.
/// </summary>
public class StructMember
{
    public DynamicDataType Datatype { get; }
    public Identifier Identifier { get; }

    public StructMember(DynamicDataType datatype, Identifier identifier)
    {
        Datatype = datatype;
        Identifier = identifier;
    }

    public void Initialize(NCS ncs, CodeRoot root)
    {
        if (Datatype.Builtin == DataType.Int)
        {
            ncs.Add(NCSInstructionType.RSADDI, null);
        }
        else if (Datatype.Builtin == DataType.Float)
        {
            ncs.Add(NCSInstructionType.RSADDF, null);
        }
        else if (Datatype.Builtin == DataType.String)
        {
            ncs.Add(NCSInstructionType.RSADDS, null);
        }
        else if (Datatype.Builtin == DataType.Object)
        {
            ncs.Add(NCSInstructionType.RSADDO, null);
        }
        else if (Datatype.Builtin == DataType.Struct)
        {
            root.StructMap[Identifier.Label].Initialize(ncs, root);
        }
        else
        {
            string msg = $"Unsupported struct member type: {Datatype.Builtin}\n" +
                         $"  Member: {Identifier}\n" +
                         $"  Supported types: int, float, string, object, event, effect, location, talent, struct";
            throw new CompileError(msg);
        }
    }

    public int Size(CodeRoot root)
    {
        return Datatype.Size(root);
    }
}

