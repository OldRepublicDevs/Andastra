// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;
namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class AFcnCallExp : ScriptNode, AExpression
    {
        private List<object> @params;
        private byte id;
        private StackEntry stackentry;
        public AFcnCallExp(byte id, List<object> @params)
        {
            this.id = id;
            this.@params = new List<object>();
            for (int i = 0; i < @params.Count; ++i)
            {
                this.AddParam((AExpression)@params[i]);
            }
        }

        protected virtual void AddParam(AExpression param)
        {
            param.Parent(this);
            this.@params.Add(param);
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append("sub" + this.id + "(");
            string prefix = "";
            for (int i = 0; i < this.@params.Count; ++i)
            {
                buff.Append(prefix.ToString() + this.@params[i].ToString());
                prefix = ", ";
            }

            buff.Append(")");
            return buff.ToString();
        }

        public virtual StackEntry Stackentry()
        {
            return this.stackentry;
        }

        public virtual void Stackentry(StackEntry stackentry)
        {
            this.stackentry = stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AFcnCallExp.java
        // Original: @Override public void close()
        public override void Close()
        {
            base.Close();
            if (this.@params != null)
            {
                foreach (ScriptNode param in this.@params)
                {
                    param.Close();
                }

                this.@params = null;
            }

            if (this.stackentry != null)
            {
                this.stackentry.Dispose();
            }

            this.stackentry = null;
        }
    }
}




