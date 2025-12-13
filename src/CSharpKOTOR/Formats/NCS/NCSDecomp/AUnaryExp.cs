// Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:10-62
// Original: public class AUnaryExp extends ScriptNode implements AExpression
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;
using CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class AUnaryExp : ScriptNode, AExpression
    {
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:11
        // Original: private AExpression exp;
        private AExpression exp;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:12
        // Original: private String op;
        private string op;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:13
        // Original: private StackEntry stackentry;
        private StackEntry stackentry;

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:15-18
        // Original: public AUnaryExp(AExpression exp, String op) { this.exp(exp); this.op = op; }
        public AUnaryExp(AExpression exp, string op)
        {
            this.Exp(exp);
            this.op = op;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:20-23
        // Original: protected void exp(AExpression exp) { this.exp = exp; exp.parent(this); }
        protected virtual void Exp(AExpression exp)
        {
            this.exp = exp;
            exp.Parent(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:25-27
        // Original: public AExpression exp() { return this.exp; }
        public virtual AExpression Exp()
        {
            return this.exp;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:29-31
        // Original: public String op() { return this.op; }
        public virtual string Op()
        {
            return this.op;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:33-36
        // Original: @Override public String toString() { return ExpressionFormatter.format(this); }
        public override string ToString()
        {
            return ExpressionFormatter.Format(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:38-41
        // Original: @Override public StackEntry stackentry() { return this.stackentry; }
        public virtual StackEntry Stackentry()
        {
            return this.stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:43-46
        // Original: @Override public void stackentry(StackEntry stackentry) { this.stackentry = stackentry; }
        public virtual void Stackentry(StackEntry stackentry)
        {
            this.stackentry = stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/AUnaryExp.java:48-61
        // Original: @Override public void close() { super.close(); if (this.exp != null) { ((ScriptNode)this.exp).close(); } this.exp = null; if (this.stackentry != null) { this.stackentry.close(); } this.stackentry = null; }
        public override void Close()
        {
            base.Close();
            if (this.exp != null)
            {
                ((ScriptNode)this.exp).Close();
            }

            this.exp = null;
            if (this.stackentry != null)
            {
                this.stackentry.Close();
            }

            this.stackentry = null;
        }
    }
}




