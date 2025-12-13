// Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:9-77
// Original: public class ABinaryExp extends ScriptNode implements AExpression
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;
using CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class ABinaryExp : ScriptNode, AExpression
    {
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:10
        // Original: private AExpression left;
        private AExpression left;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:11
        // Original: private AExpression right;
        private AExpression right;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:12
        // Original: private String op;
        private string op;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:13
        // Original: private StackEntry stackentry;
        private StackEntry stackentry;

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:15-19
        // Original: public ABinaryExp(AExpression left, AExpression right, String op) { this.left(left); this.right(right); this.op = op; }
        public ABinaryExp(AExpression left, AExpression right, string op)
        {
            this.Left(left);
            this.Right(right);
            this.op = op;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:21-24
        // Original: protected void left(AExpression left) { this.left = left; left.parent(this); }
        protected virtual void Left(AExpression left)
        {
            this.left = left;
            left.Parent(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:26-29
        // Original: protected void right(AExpression right) { this.right = right; right.parent(this); }
        protected virtual void Right(AExpression right)
        {
            this.right = right;
            right.Parent(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:31-33
        // Original: public AExpression left() { return this.left; }
        public virtual AExpression Left()
        {
            return this.left;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:35-37
        // Original: public AExpression right() { return this.right; }
        public virtual AExpression Right()
        {
            return this.right;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:39-41
        // Original: public String op() { return this.op; }
        public virtual string Op()
        {
            return this.op;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:43-46
        // Original: @Override public String toString() { return ExpressionFormatter.format(this); }
        public override string ToString()
        {
            return ExpressionFormatter.Format(this);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:48-51
        // Original: @Override public StackEntry stackentry() { return this.stackentry; }
        public virtual StackEntry Stackentry()
        {
            return this.stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:53-56
        // Original: @Override public void stackentry(StackEntry stackentry) { this.stackentry = stackentry; }
        public virtual void Stackentry(StackEntry stackentry)
        {
            this.stackentry = stackentry;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ABinaryExp.java:58-76
        // Original: @Override public void close() { super.close(); if (this.left != null) { ((ScriptNode)this.left).close(); this.left = null; } if (this.right != null) { ((ScriptNode)this.right).close(); this.right = null; } if (this.stackentry != null) { this.stackentry.close(); } this.stackentry = null; }
        public override void Close()
        {
            base.Close();
            if (this.left != null)
            {
                ((ScriptNode)this.left).Close();
                this.left = null;
            }

            if (this.right != null)
            {
                ((ScriptNode)this.right).Close();
                this.right = null;
            }

            if (this.stackentry != null)
            {
                this.stackentry.Close();
            }

            this.stackentry = null;
        }
    }
}




