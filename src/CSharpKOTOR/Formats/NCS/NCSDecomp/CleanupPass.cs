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
        // Original: private void checkSubCodeBlock()
        private void CheckSubCodeBlock()
        {
            ACodeBlock block = null;
            List<object> children = null;
            try
            {
                if (this.root.Size() == 1 && typeof(ACodeBlock).IsInstanceOfType(this.root.GetLastChild()))
                {
                    block = (ACodeBlock)this.root.RemoveLastChild();
                    children = block.RemoveChildren();
                    this.root.AddChildren(children);
                }
            }
            finally
            {
                block = null;
                children = null;
            }
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptutils/CleanupPass.java:75-164
        // Original: private void apply(ScriptRootNode rootnode)
        private void Apply(ScriptRootNode rootnode)
        {
            LinkedList children = null;
            ListIterator it = null;
            Scriptnode.ScriptNode node1 = null;
            Variable var = null;
            VarStruct structx = null;
            AVarDecl structdecx = null;
            Scriptnode.ScriptNode node2 = null;
            AModifyExp modexp = null;
            AExpressionStatement expstm = null;
            ASwitchCase acase = null;
            try
            {
                children = rootnode.GetChildren();
                it = children.ListIterator();

                while (it.HasNext())
                {
                    node1 = (Scriptnode.ScriptNode)it.Next();
                    if (typeof(AVarDecl).IsInstanceOfType(node1))
                    {
                        AVarDecl decl = (AVarDecl)node1;
                        if (decl.Exp() == null && it.HasNext())
                        {
                            node2 = (Scriptnode.ScriptNode)it.Next();
                            if (typeof(AExpressionStatement).IsInstanceOfType(node2)
                                && typeof(AModifyExp).IsInstanceOfType(((AExpressionStatement)node2).Exp()))
                            {
                                modexp = (AModifyExp)((AExpressionStatement)node2).Exp();
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
                        var = ((AVarDecl)node1).Var();
                        if (var != null && var.IsStruct())
                        {
                            structx = ((AVarDecl)node1).Var().Varstruct();
                            structdecx = new AVarDecl(structx);
                            if (it.HasNext())
                            {
                                node1 = (Scriptnode.ScriptNode)it.Next();
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
                                    node1 = (Scriptnode.ScriptNode)it.Next();
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

                            node1 = (Scriptnode.ScriptNode)it.Next();
                            structdecx.Parent(node1.Parent());
                            it.Set(structdecx);
                            node1 = structdecx;
                        }
                    }

                    if (this.IsDanglingExpression(node1))
                    {
                        expstm = new AExpressionStatement((AExpression)node1);
                        expstm.Parent(rootnode);
                        it.Set(expstm);
                    }

                    it.Previous();
                    it.Next();
                    if (typeof(ScriptRootNode).IsInstanceOfType(node1))
                    {
                        this.Apply((ScriptRootNode)node1);
                    }

                    acase = null;
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
                children = null;
                it = null;
                node1 = null;
                var = null;
                structx = null;
                structdecx = null;
                node2 = null;
                modexp = null;
                expstm = null;
                acase = null;
            }
        }

        private bool IsDanglingExpression(Scriptnode.ScriptNode node)
        {
            return node is AExpression;
        }
    }
}




