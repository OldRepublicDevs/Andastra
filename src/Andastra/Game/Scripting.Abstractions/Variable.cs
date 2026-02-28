using System;
using System.Numerics;

namespace Andastra.Game.Scripting.Abstractions
{
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

    public struct Variable
    {
        public VariableType Type;
        public int IntValue;
        public float FloatValue;
        public string StringValue;
        public uint ObjectId;
        public Vector3 VectorValue;
        public object ComplexValue;

        public static Variable Void() => new Variable { Type = VariableType.Void };
        public static Variable FromInt(int value) => new Variable { Type = VariableType.Int, IntValue = value };
        public static Variable FromFloat(float value) => new Variable { Type = VariableType.Float, FloatValue = value };
        public static Variable FromString(string value) => new Variable { Type = VariableType.String, StringValue = value ?? string.Empty };
        public static Variable FromObject(uint objectId) => new Variable { Type = VariableType.Object, ObjectId = objectId };
        public static Variable FromVector(Vector3 value) => new Variable { Type = VariableType.Vector, VectorValue = value };
        public static Variable FromVector(float x, float y, float z) => new Variable { Type = VariableType.Vector, VectorValue = new Vector3(x, y, z) };
        public static Variable FromLocation(object location) => new Variable { Type = VariableType.Location, ComplexValue = location };
        public static Variable FromEffect(object effect) => new Variable { Type = VariableType.Effect, ComplexValue = effect };
        public static Variable FromEvent(object evt) => new Variable { Type = VariableType.Event, ComplexValue = evt };
        public static Variable FromTalent(object talent) => new Variable { Type = VariableType.Talent, ComplexValue = talent };
        public static Variable FromAction(object action) => new Variable { Type = VariableType.Action, ComplexValue = action };

        public int AsInt() => Type == VariableType.Int ? IntValue : Type == VariableType.Float ? (int)FloatValue : 0;
        public float AsFloat() => Type == VariableType.Float ? FloatValue : Type == VariableType.Int ? IntValue : 0f;
        public string AsString() => Type == VariableType.String ? (StringValue ?? string.Empty) : Type == VariableType.Int ? IntValue.ToString() : Type == VariableType.Float ? FloatValue.ToString() : string.Empty;
        public uint AsObjectId() => Type == VariableType.Object ? ObjectId : 0x7F000000;
        public Vector3 AsVector() => Type == VariableType.Vector ? VectorValue : Vector3.Zero;
        public object AsLocation() => Type == VariableType.Location ? ComplexValue : null;
        public object AsEffect() => Type == VariableType.Effect ? ComplexValue : null;

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
