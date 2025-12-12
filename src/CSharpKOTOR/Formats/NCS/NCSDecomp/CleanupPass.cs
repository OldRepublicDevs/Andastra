// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Stack;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Utils;
namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptutils
{
    public class CleanupPass
    {
        private ASub root;
        private NodeAnalysisData nodedata;
        private SubroutineAnalysisData subdata;
        private SubScriptState state;
        public CleanupPass(ASub root, NodeAnalysisData nodedata, SubroutineAnalysisData subdata, SubScriptState state)
        {
            this.root = root;
            this.nodedata = nodedata;
            this.subdata = subdata;
            this.state = state;
        }

        public virtual void Apply()
        {
            this.CheckSubCodeBlock();
            this.Apply(this.root);
        }

        public virtual void Done()
        {
            this.root = null;
            this.nodedata = null;
            this.subdata = null;
            this.state = null;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptutils/CleanupPass.java:62-73
        // Original: private void checkSubCodeBlock() { try { if (this.root.size() == 1 && ACodeBlock.class.isInstance(this.root.getLastChild())) { ACodeBlock block = (ACodeBlock)this.root.removeLastChild(); List<ScriptNode> children = block.removeChildren(); this.root.addChildren(children); } } finally { ACodeBlock block = null; List<ScriptNode> children = null; } }
        private void CheckSubCodeBlock()
        {
            try
            {
                if (this.root.Size() == 1 && typeof(ACodeBlock).IsInstanceOfType(this.root.GetLastChild()))
                {
                    ACodeBlock block = (ACodeBlock)this.root.RemoveLastChild();
                    List<ScriptNode> children = block.RemoveChildren();
                    this.root.AddChildren(children);
                }
            }
            finally
            {
                ACodeBlock block = null;
                List<ScriptNode> children = null;
            }
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptutils/CleanupPass.java:75-164
        // Original: private void apply(ScriptRootNode rootnode) { try { LinkedList children = rootnode.getChildren(); ListIterator it = children.listIterator(); ... } finally { ... } }
        private void Apply(ScriptRootNode rootnode)
        {
            try
            {
                List<ScriptNode> children = rootnode.GetChildren();
                ListIterator it = children.ListIterator();

                while (it.HasNext())
                {
                    ScriptNode node1 = (ScriptNode)it.Next();
                    if (typeof(AVarDecl).IsInstanceOfType(node1))
                    {
                        AVarDecl decl = (AVarDecl)node1;
                        if (decl.Exp() == null && it.HasNext())
                        {
                            ScriptNode maybeAssign = (ScriptNode)it.Next();
                            if (typeof(AExpressionStatement).IsInstanceOfType(maybeAssign)
                                && typeof(AModifyExp).IsInstanceOfType(((AExpressionStatement)maybeAssign).Exp()))
                            {
                                AModifyExp modexp = (AModifyExp)((AExpressionStatement)maybeAssign).Exp();
                                if (modexp.VarRef().Var() == decl.Var())
                                {
                                    decl.InitializeExp(modexp.Expression());
                                    it.Remove(); // drop the now-merged assignment statement
                                }
                                else
                                {
                                    it.Previous();
                                }
                            }
                            else
                            {
                                it.Previous();
                            }
                        }
                    }
                    if (typeof(AVarDecl).IsInstanceOfType(node1))
                    {
                        Variable var = ((AVarDecl)node1).Var();
                        if (var != null && var.IsStruct())
                        {
                            VarStruct structx = ((AVarDecl)node1).Var().Varstruct();
                            AVarDecl structdecx = new AVarDecl(structx);
                            if (it.HasNext())
                            {
                                node1 = (ScriptNode)it.Next();
                            }
                            else
                            {
                                node1 = null;
                            }

                            while (typeof(AVarDecl).IsInstanceOfType(node1) && structx.Equals(((AVarDecl)node1).Var().Varstruct()))
                            {
                                it.Remove();
                                node1.Parent(null);
                                if (it.HasNext())
                                {
                                    node1 = (ScriptNode)it.Next();
                                }
                                else
                                {
                                    node1 = null;
                                }
                            }

                            it.Previous();
                            if (node1 != null)
                            {
                                it.Previous();
                            }

                            node1 = (ScriptNode)it.Next();
                            structdecx.Parent(node1.Parent());
                            it.Set(structdecx);
                            node1 = structdecx;
                        }
                    }

                    if (this.IsDanglingExpression(node1))
                    {
                        AExpressionStatement expstm = new AExpressionStatement((AExpression)node1);
                        expstm.Parent(rootnode);
                        it.Set(expstm);
                    }

                    it.Previous();
                    it.Next();
                    if (typeof(ScriptRootNode).IsInstanceOfType(node1))
                    {
                        this.Apply((ScriptRootNode)node1);
                    }

                    ASwitchCase acase = null;
                    if (typeof(ASwitch).IsInstanceOfType(node1))
                    {
                        while ((acase = ((ASwitch)node1).GetNextCase(acase)) != null)
                        {
                            this.Apply(acase);
                        }
                    }
                }
            }
            finally
            {
                List<ScriptNode> children = null;
                ListIterator it = null;
                ScriptNode node1x = null;
                Variable var = null;
                VarStruct structx = null;
                AVarDecl structdecx = null;
                ScriptNode node2 = null;
                AModifyExp modexp = null;
                AExpressionStatement expstm = null;
                ASwitchCase acase = null;
            }
        }

        private bool IsDanglingExpression(Scriptnode.ScriptNode node)
        {
            return node is AExpression;
        }
    }
}




