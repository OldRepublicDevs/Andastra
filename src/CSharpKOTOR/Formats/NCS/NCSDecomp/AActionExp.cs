// Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:17-22
    // Original: public class AActionExp extends ScriptNode implements AExpression { private List<AExpression> params; private String action; private StackEntry stackentry; private int id; private ActionsData actionsData; }
    public class AActionExp : ScriptNode, AExpression
    {
        private List<AExpression> @params;
        private string action;
        private StackEntry stackentry;
        private int id;
        private ActionsData actionsData;

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:24-26
        // Original: public AActionExp(String action, int id, List<AExpression> params) { this(action, id, params, null); }
        public AActionExp(string action, int id, List<AExpression> @params)
        {
            this.Initialize(action, id, @params, null);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:28-39
        // Original: public AActionExp(String action, int id, List<AExpression> params, ActionsData actionsData) { ... }
        public AActionExp(string action, int id, List<AExpression> @params, ActionsData actionsData)
        {
            this.Initialize(action, id, @params, actionsData);
        }

        private void Initialize(string action, int id, List<AExpression> @params, ActionsData actionsData)
        {
            this.action = action;
            this.@params = new List<AExpression>();
            this.actionsData = actionsData;

            for (int i = 0; i < @params.Count; i++)
            {
                this.AddParam(@params[i]);
            }

            this.stackentry = null;
            this.id = id;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:41-44
        // Original: protected void addParam(AExpression param) { param.parent(this); this.params.add(param); }
        protected virtual void AddParam(AExpression param)
        {
            param.Parent(this);
            this.@params.Add(param);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:46-48
        // Original: public AExpression getParam(int pos) { return this.params.get(pos); }
        public virtual AExpression GetParam(int pos)
        {
            return this.@params[pos];
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:50-52
        // Original: public String action() { return this.action; }
        public virtual string Action()
        {
            return this.action;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:54-122
        // Original: @Override public String toString() { ... }
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.action + "(");
            string prefix = "";

            int paramCount = this.@params.Count;
            if (this.actionsData != null)
            {
                try
                {
                    List<string> defaults = this.actionsData.GetDefaultValues(this.id);
                    int requiredParams = this.actionsData.GetRequiredParamCount(this.id);
                    int optionalCount = Math.Max(0, defaults.Count - requiredParams);

                    // If any optional parameter differs from its default, keep the full argument list.
                    bool hasNonDefaultOptional = false;
                    for (int i = requiredParams; i < Math.Min(paramCount, defaults.Count); i++)
                    {
                        string defaultValue = defaults[i];
                        if (defaultValue == null)
                        {
                            continue;
                        }
                        string normalizedParam = NormalizeValue(this.@params[i].ToString());
                        string normalizedDefault = NormalizeValue(defaultValue);
                        if (!normalizedParam.Equals(normalizedDefault))
                        {
                            hasNonDefaultOptional = true;
                            break;
                        }
                    }

                    // Only trim when there are multiple optional parameters and all of them
                    // match defaults that look like compiler-inserted sentinels.
                    if (optionalCount > 1 && !hasNonDefaultOptional)
                    {
                        int lastNonDefault = paramCount;
                        for (int i = paramCount - 1; i >= 0 && i < defaults.Count; i--)
                        {
                            string defaultValue = defaults[i];
                            if (defaultValue == null)
                            {
                                break;
                            }

                            if (!IsLikelyCompilerInsertedDefault(defaultValue))
                            {
                                break;
                            }

                            string paramStr = this.@params[i].ToString();
                            string normalizedParam = NormalizeValue(paramStr);
                            string normalizedDefault = NormalizeValue(defaultValue);
                            if (normalizedParam.Equals(normalizedDefault))
                            {
                                lastNonDefault = i;
                            }
                            else
                            {
                                break;
                            }
                        }
                        paramCount = lastNonDefault;
                    }
                    else
                    {
                        paramCount = this.@params.Count;
                    }
                }
                catch (Exception)
                {
                    // If there's any error, output all parameters
                    paramCount = this.@params.Count;
                }
            }

            for (int i = 0; i < paramCount; i++)
            {
                buff.Append(prefix + this.@params[i].ToString());
                prefix = ", ";
            }

            buff.Append(")");
            return buff.ToString();
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:124-130
        // Original: private boolean isLikelyCompilerInsertedDefault(String defaultValue) { ... }
        private bool IsLikelyCompilerInsertedDefault(string defaultValue)
        {
            if (defaultValue == null)
            {
                return false;
            }
            string v = defaultValue.Trim();
            return v.Equals("-1") || v.Equals("0xFFFFFFFF") || v.Equals("0xFFFFFFFFFFFFFFFF");
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:142-168
        // Original: private String normalizeValue(String value) { ... }
        private string NormalizeValue(string value)
        {
            if (value == null)
            {
                return "";
            }
            value = value.Trim();
            // Handle TRUE/FALSE constants
            if (value.Equals("TRUE") || value.Equals("1"))
            {
                return "1";
            }
            if (value.Equals("FALSE") || value.Equals("0"))
            {
                return "0";
            }
            // Normalize float literals (1.0f -> 1.0, 0.0f -> 0.0)
            if (value.EndsWith("f") || value.EndsWith("F"))
            {
                value = value.Substring(0, value.Length - 1);
            }
            // Handle hex values (0xFFFFFFFF -> 4294967295, but we'll compare as hex)
            if (value.StartsWith("0x") || value.StartsWith("0X"))
            {
                try
                {
                    long hexVal = Convert.ToInt64(value.Substring(2), 16);
                    return hexVal.ToString();
                }
                catch (FormatException)
                {
                    // Not a valid hex number, return as-is
                }
            }
            return value;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:170-173
        // Original: @Override public StackEntry stackentry() { return this.stackentry; }
        public virtual StackEntry Stackentry()
        {
            return this.stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:175-178
        // Original: @Override public void stackentry(StackEntry stackentry) { this.stackentry = stackentry; }
        public virtual void Stackentry(StackEntry stackentry)
        {
            this.stackentry = stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:180-182
        // Original: public int getId() { return this.id; }
        public virtual int GetId()
        {
            return this.id;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AActionExp.java:184-202
        // Original: @Override public void close() { ... }
        public override void Close()
        {
            base.Close();
            if (this.@params != null)
            {
                foreach (AExpression param in this.@params)
                {
                    if (param is ScriptNode)
                    {
                        ((ScriptNode)param).Close();
                    }
                }

                this.@params = null;
            }

            if (this.stackentry != null)
            {
                this.stackentry.Close();
            }

            this.stackentry = null;
        }
    }
}




