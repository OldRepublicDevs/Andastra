using System;
using System.Numerics;

namespace Odyssey.Scripting.Interfaces
{
    /// <summary>
    /// Script variable types.
    /// </summary>
    public enum VariableType
    {
        Void = 0,
        Int = 1,
        Float = 2,
        String = 3,
        Object = 4,
        Vector = 5,
        Location = 6,
        Effect = 7,
        Event = 8,
        Talent = 9,
        Action = 10
    }
    
    /// <summary>
    /// NWScript variable - a tagged union of all possible script value types.
    /// </summary>
    public struct Variable
    {
        public VariableType Type;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public uint ObjectId;
        public Vector3 VectorValue;
        public object ComplexValue; // For location, effect, event, talent, action
        
        public static Variable Void()
        {
            return new Variable { Type = VariableType.Void };
        }
        
        public static Variable FromInt(int value)
        {
            return new Variable { Type = VariableType.Int, IntValue = value };
        }
        
        public static Variable FromFloat(float value)
        {
            return new Variable { Type = VariableType.Float, FloatValue = value };
        }
        
        public static Variable FromString(string value)
        {
            return new Variable { Type = VariableType.String, StringValue = value ?? string.Empty };
        }
        
        public static Variable FromObject(uint objectId)
        {
            return new Variable { Type = VariableType.Object, ObjectId = objectId };
        }
        
        public static Variable FromVector(Vector3 value)
        {
            return new Variable { Type = VariableType.Vector, VectorValue = value };
        }
        
        public static Variable FromVector(float x, float y, float z)
        {
            return new Variable { Type = VariableType.Vector, VectorValue = new Vector3(x, y, z) };
        }
        
        public static Variable FromLocation(object location)
        {
            return new Variable { Type = VariableType.Location, ComplexValue = location };
        }
        
        public static Variable FromEffect(object effect)
        {
            return new Variable { Type = VariableType.Effect, ComplexValue = effect };
        }
        
        public static Variable FromEvent(object evt)
        {
            return new Variable { Type = VariableType.Event, ComplexValue = evt };
        }
        
        public static Variable FromTalent(object talent)
        {
            return new Variable { Type = VariableType.Talent, ComplexValue = talent };
        }
        
        public static Variable FromAction(object action)
        {
            return new Variable { Type = VariableType.Action, ComplexValue = action };
        }
        
        public int AsInt()
        {
            if (Type == VariableType.Int)
            {
                return IntValue;
            }
            if (Type == VariableType.Float)
            {
                return (int)FloatValue;
            }
            return 0;
        }
        
        public float AsFloat()
        {
            if (Type == VariableType.Float)
            {
                return FloatValue;
            }
            if (Type == VariableType.Int)
            {
                return IntValue;
            }
            return 0f;
        }
        
        public string AsString()
        {
            if (Type == VariableType.String)
            {
                return StringValue ?? string.Empty;
            }
            if (Type == VariableType.Int)
            {
                return IntValue.ToString();
            }
            if (Type == VariableType.Float)
            {
                return FloatValue.ToString();
            }
            return string.Empty;
        }
        
        public uint AsObjectId()
        {
            if (Type == VariableType.Object)
            {
                return ObjectId;
            }
            return 0x7F000000; // OBJECT_INVALID
        }
        
        public Vector3 AsVector()
        {
            if (Type == VariableType.Vector)
            {
                return VectorValue;
            }
            return Vector3.Zero;
        }
        
        public override string ToString()
        {
            switch (Type)
            {
                case VariableType.Int: return "Int(" + IntValue + ")";
                case VariableType.Float: return "Float(" + FloatValue + ")";
                case VariableType.String: return "String(\"" + StringValue + "\")";
                case VariableType.Object: return "Object(" + ObjectId.ToString("X8") + ")";
                case VariableType.Vector: return "Vector(" + VectorValue.X + ", " + VectorValue.Y + ", " + VectorValue.Z + ")";
                case VariableType.Location: return "Location()";
                case VariableType.Effect: return "Effect()";
                case VariableType.Event: return "Event()";
                case VariableType.Talent: return "Talent()";
                case VariableType.Action: return "Action()";
                default: return "Void";
            }
        }
    }
}

