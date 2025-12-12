// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class AVectorConstExp : ScriptNode, AExpression
    {
        private AExpression exp1;
        private AExpression exp2;
        private AExpression exp3;
        public AVectorConstExp(AExpression exp1, AExpression exp2, AExpression exp3)
        {
            this.Exp1(exp1);
            this.Exp2(exp2);
            this.Exp3(exp3);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVectorConstExp.java:21-24
        // Original: public void exp1(AExpression exp1) { this.exp1 = exp1; exp1.parent(this); }
        public virtual void Exp1(AExpression exp1)
        {
            this.exp1 = exp1;
            exp1.Parent(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVectorConstExp.java:26-29
        // Original: public void exp2(AExpression exp2) { this.exp2 = exp2; exp2.parent(this); }
        public virtual void Exp2(AExpression exp2)
        {
            this.exp2 = exp2;
            exp2.Parent(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVectorConstExp.java:31-34
        // Original: public void exp3(AExpression exp3) { this.exp3 = exp3; exp3.parent(this); }
        public virtual void Exp3(AExpression exp3)
        {
            this.exp3 = exp3;
            exp3.Parent(this);
        }

        public override string ToString()
        {
            return "[" + this.exp1 + "," + this.exp2 + "," + this.exp3 + "]";
        }

        public virtual StackEntry Stackentry()
        {
            return null;
        }

        public virtual void Stackentry(StackEntry stackentry)
        {
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVectorConstExp.java
        // Original: @Override public void close()
        public override void Close()
        {
            base.Close();
            if (this.exp1 != null)
            {
                ((ScriptNode)this.exp1).Close();
            }

            this.exp1 = null;
            if (this.exp2 != null)
            {
                ((ScriptNode)this.exp2).Close();
            }

            this.exp2 = null;
            if (this.exp3 != null)
            {
                ((ScriptNode)this.exp3).Close();
            }

            this.exp3 = null;
        }
    }
}




