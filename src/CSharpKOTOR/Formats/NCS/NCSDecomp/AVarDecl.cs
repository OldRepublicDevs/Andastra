// Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:11-79
// Original: public class AVarDecl extends ScriptNode
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Utils;
using CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode;
using UtilsType = CSharpKOTOR.Formats.NCS.NCSDecomp.Utils.Type;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class AVarDecl : ScriptNode
    {
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:12
        // Original: private Variable var;
        private Variable var;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:13
        // Original: private AExpression exp;
        private AExpression exp;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:14
        // Original: private boolean isFcnReturn;
        private bool isFcnReturn;

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:16-20
        // Original: public AVarDecl(Variable var) { this.var(var); this.exp = null; this.isFcnReturn = false; }
        public AVarDecl(Variable var)
        {
            this.Var(var);
            this.exp = null;
            this.isFcnReturn = false;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:22-24
        // Original: public void var(Variable var) { this.var = var; }
        public virtual void Var(Variable var)
        {
            this.var = var;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:26-28
        // Original: public Variable var() { return this.var; }
        public virtual Variable Var()
        {
            return this.var;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:30-32
        // Original: public void isFcnReturn(boolean is) { this.isFcnReturn = is; }
        public virtual void IsFcnReturn(bool @is)
        {
            this.isFcnReturn = @is;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:34-36
        // Original: public boolean isFcnReturn() { return this.isFcnReturn; }
        public virtual bool IsFcnReturn()
        {
            return this.isFcnReturn;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:38-40
        // Original: public Type type() { return this.var.type(); }
        public virtual UtilsType Type()
        {
            return this.var.Type();
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:42-45
        // Original: public void initializeExp(AExpression exp) { exp.parent(this); this.exp = exp; }
        public virtual void InitializeExp(AExpression exp)
        {
            exp.Parent(this);
            this.exp = exp;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:47-52
        // Original: public AExpression removeExp() { AExpression aexp = this.exp; this.exp.parent(null); this.exp = null; return aexp; }
        public virtual AExpression RemoveExp()
        {
            AExpression aexp = this.exp;
            this.exp.Parent(null);
            this.exp = null;
            return aexp;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:54-56
        // Original: public AExpression exp() { return this.exp; }
        public virtual AExpression Exp()
        {
            return this.exp;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:58-63
        // Original: @Override public String toString() { return this.exp == null ? this.tabs + this.var.toDeclString() + ";" + this.newline : this.tabs + this.var.toDeclString() + " = " + ExpressionFormatter.formatValue(this.exp) + ";" + this.newline; }
        public override string ToString()
        {
            return this.exp == null
                ? this.tabs + this.var.ToDeclString() + ";" + this.newline
                : this.tabs + this.var.ToDeclString() + " = " + ExpressionFormatter.FormatValue(this.exp) + ";" + this.newline;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AVarDecl.java:65-78
        // Original: @Override public void close() { super.close(); if (this.exp != null) { ((ScriptNode)this.exp).close(); } this.exp = null; if (this.var != null) { this.var.close(); } this.var = null; }
        public override void Close()
        {
            base.Close();
            if (this.exp != null)
            {
                ((ScriptNode)this.exp).Close();
            }

            this.exp = null;
            if (this.var != null)
            {
                this.var.Close();
            }

            this.var = null;
        }
    }
}




