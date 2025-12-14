using System;
using System.Collections.Generic;
using Odyssey.Scripting.Interfaces;

namespace Odyssey.Scripting.EngineApi
{
    /// <summary>
    /// Base implementation of engine API with common functions.
    /// </summary>
    public abstract class BaseEngineApi : IEngineApi
    {
        protected readonly Random _random;
        protected readonly Dictionary<int, string> _functionNames;
        protected readonly HashSet<int> _implementedFunctions;
        
        public const uint ObjectInvalid = 0x7F000000;
        public const uint ObjectSelf = 0x7F000001;
        
        protected BaseEngineApi()
        {
            _random = new Random();
            _functionNames = new Dictionary<int, string>();
            _implementedFunctions = new HashSet<int>();
            RegisterFunctions();
        }
        
        protected abstract void RegisterFunctions();
        
        public abstract Variable CallEngineFunction(int routineId, IReadOnlyList<Variable> args, IExecutionContext ctx);
        
        public string GetFunctionName(int routineId)
        {
            if (_functionNames.TryGetValue(routineId, out string name))
            {
                return name;
            }
            return "Unknown_" + routineId;
        }
        
        public int GetArgumentCount(int routineId)
        {
            // This would be populated from ScriptDefs
            return -1;
        }
        
        public bool IsImplemented(int routineId)
        {
            return _implementedFunctions.Contains(routineId);
        }
        
        #region Common Functions
        
        protected Variable Func_Random(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            int max = args.Count > 0 ? args[0].AsInt() : 0;
            if (max <= 0)
            {
                return Variable.FromInt(0);
            }
            return Variable.FromInt(_random.Next(max));
        }
        
        protected Variable Func_PrintString(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string msg = args.Count > 0 ? args[0].AsString() : string.Empty;
            Console.WriteLine("[Script] " + msg);
            return Variable.Void();
        }
        
        protected Variable Func_PrintInteger(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            int value = args.Count > 0 ? args[0].AsInt() : 0;
            Console.WriteLine("[Script] " + value);
            return Variable.Void();
        }
        
        protected Variable Func_PrintFloat(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            float value = args.Count > 0 ? args[0].AsFloat() : 0f;
            Console.WriteLine("[Script] " + value);
            return Variable.Void();
        }
        
        protected Variable Func_IntToString(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            int value = args.Count > 0 ? args[0].AsInt() : 0;
            return Variable.FromString(value.ToString());
        }
        
        protected Variable Func_FloatToString(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            float value = args.Count > 0 ? args[0].AsFloat() : 0f;
            int width = args.Count > 1 ? args[1].AsInt() : 18;
            int decimals = args.Count > 2 ? args[2].AsInt() : 9;
            return Variable.FromString(value.ToString("F" + decimals));
        }
        
        protected Variable Func_StringToInt(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string s = args.Count > 0 ? args[0].AsString() : string.Empty;
            int.TryParse(s, out int result);
            return Variable.FromInt(result);
        }
        
        protected Variable Func_StringToFloat(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string s = args.Count > 0 ? args[0].AsString() : string.Empty;
            float.TryParse(s, out float result);
            return Variable.FromFloat(result);
        }
        
        protected Variable Func_GetTag(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                return Variable.FromString(entity.Tag ?? string.Empty);
            }
            return Variable.FromString(string.Empty);
        }
        
        protected Variable Func_GetObjectByTag(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string tag = args.Count > 0 ? args[0].AsString() : string.Empty;
            int nth = args.Count > 1 ? args[1].AsInt() : 0;
            
            var entity = ctx.World.GetEntityByTag(tag, nth);
            return Variable.FromObject(entity?.ObjectId ?? ObjectInvalid);
        }
        
        protected Variable Func_GetLocalInt(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                return Variable.FromInt(ctx.Globals.GetLocalInt(entity, name));
            }
            return Variable.FromInt(0);
        }
        
        protected Variable Func_SetLocalInt(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            int value = args.Count > 2 ? args[2].AsInt() : 0;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                ctx.Globals.SetLocalInt(entity, name, value);
            }
            return Variable.Void();
        }
        
        protected Variable Func_GetLocalFloat(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                return Variable.FromFloat(ctx.Globals.GetLocalFloat(entity, name));
            }
            return Variable.FromFloat(0f);
        }
        
        protected Variable Func_SetLocalFloat(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            float value = args.Count > 2 ? args[2].AsFloat() : 0f;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                ctx.Globals.SetLocalFloat(entity, name, value);
            }
            return Variable.Void();
        }
        
        protected Variable Func_GetLocalString(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                return Variable.FromString(ctx.Globals.GetLocalString(entity, name));
            }
            return Variable.FromString(string.Empty);
        }
        
        protected Variable Func_SetLocalString(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            string value = args.Count > 2 ? args[2].AsString() : string.Empty;
            
            var entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                ctx.Globals.SetLocalString(entity, name, value);
            }
            return Variable.Void();
        }
        
        protected Variable Func_GetIsObjectValid(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            if (objectId == ObjectInvalid)
            {
                return Variable.FromInt(0);
            }
            
            var entity = ctx.World.GetEntity(objectId);
            return Variable.FromInt(entity != null && entity.IsValid ? 1 : 0);
        }
        
        protected Variable Func_GetModule(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            // Module is a special object - return a placeholder ID
            return Variable.FromObject(0x7F000002);
        }
        
        protected Variable Func_GetArea(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            // Area is a special object - return a placeholder ID
            return Variable.FromObject(0x7F000003);
        }
        
        protected Core.Interfaces.IEntity ResolveObject(uint objectId, IExecutionContext ctx)
        {
            if (objectId == ObjectInvalid)
            {
                return null;
            }
            
            if (objectId == ObjectSelf)
            {
                return ctx.Caller;
            }
            
            return ctx.World.GetEntity(objectId);
        }
        
        #endregion
    }
}

