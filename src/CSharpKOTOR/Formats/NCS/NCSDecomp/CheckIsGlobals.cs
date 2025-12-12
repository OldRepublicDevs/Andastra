//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CSharpKOTOR.Formats.NCS.NCSDecomp;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Analysis;
using AST = CSharpKOTOR.Formats.NCS.NCSDecomp.AST;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Utils
{
    public class CheckIsGlobals : PrunedReversedDepthFirstAdapter
    {
        private bool isGlobals;
        public CheckIsGlobals()
        {
            this.isGlobals = false;
        }

        public override void InABpCommand(ABpCommand node)
        {
            this.isGlobals = true;
        }

        public override void CaseABpCommand(ABpCommand node)
        {
            // Matching DeNCS implementation: explicitly call InABpCommand
            this.InABpCommand(node);
        }

        // Handle AST.ABpCommand as well (from NcsToAstConverter)
        public override void CaseABpCommand(AST.ABpCommand node)
        {
            // Matching DeNCS implementation: set isGlobals directly
            // This is called when AST.ABpCommand.Apply() routes to CaseABpCommand
            JavaSystem.@out.Println("DEBUG CheckIsGlobals.CaseABpCommand(AST): found ABpCommand, setting isGlobals = true");
            this.isGlobals = true;
        }

        public override void CaseABpCmd(ABpCmd node)
        {
            // Matching DeNCS implementation: traverse into ABpCmd to reach ABpCommand
            this.InABpCmd(node);
            if (node.GetBpCommand() != null)
            {
                node.GetBpCommand().Apply(this);
            }
            if (!this.isGlobals)
            {
                this.OutABpCmd(node);
            }
        }

        // Handle AST.ABpCmd as well (from NcsToAstConverter)
        public override void CaseABpCmd(AST.ABpCmd node)
        {
            // Traverse into AST.ABpCmd to reach ABpCommand
            JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseABpCmd(AST): called, GetBpCommand() is {(node.GetBpCommand() != null ? "non-null" : "null")}");
            if (node.GetBpCommand() != null)
            {
                JavaSystem.@out.Println("DEBUG CheckIsGlobals.CaseABpCmd(AST): calling GetBpCommand().Apply(this)");
                node.GetBpCommand().Apply(this);
            }
        }

        public override void CaseACommandBlock(ACommandBlock node)
        {
            JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseACommandBlock: called, command count = {node.GetCmd().Count}");
            this.InACommandBlock(node);
            Object[] temp = node.GetCmd().ToArray();
            for (int i = temp.Length - 1; i >= 0; --i)
            {
                var cmd = (PCmd)temp[i];
                JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseACommandBlock: command {i} type = {cmd.GetType().FullName}");
                cmd.Apply(this);
                if (this.isGlobals)
                {
                    JavaSystem.@out.Println("DEBUG CheckIsGlobals.CaseACommandBlock: isGlobals = true, returning");
                    return;
                }
            }

            this.OutACommandBlock(node);
        }

        // Handle AST.ACommandBlock as well (from NcsToAstConverter)
        public override void CaseACommandBlock(AST.ACommandBlock node)
        {
            // Traverse into AST.ACommandBlock to find ABpCommand
            Object[] temp = node.GetCmd().ToArray();
            JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseACommandBlock(AST): processing {temp.Length} commands");
            for (int i = temp.Length - 1; i >= 0; --i)
            {
                var cmd = (PCmd)temp[i];
                JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseACommandBlock(AST): command {i} type = {cmd.GetType().FullName}");
                cmd.Apply(this);
                if (this.isGlobals)
                {
                    JavaSystem.@out.Println("DEBUG CheckIsGlobals.CaseACommandBlock(AST): isGlobals = true, returning");
                    return;
                }
            }
        }

        // Override CaseASubroutine to ensure command block is traversed
        public override void CaseASubroutine(ASubroutine node)
        {
            // Don't call InASubroutine/OutASubroutine - just traverse command block
            if (node.GetCommandBlock() != null)
            {
                node.GetCommandBlock().Apply(this);
            }
        }

        // Handle AST.ASubroutine as well (from NcsToAstConverter)
        // Override to ensure command block is traversed (matching DeNCS pattern)
        public override void CaseASubroutine(AST.ASubroutine node)
        {
            // Traverse into AST.ASubroutine to reach command block
            JavaSystem.@out.Println($"DEBUG CheckIsGlobals.CaseASubroutine(AST): called, GetCommandBlock() is {(node.GetCommandBlock() != null ? "non-null" : "null")}");
            if (node.GetCommandBlock() != null)
            {
                JavaSystem.@out.Println("DEBUG CheckIsGlobals.CaseASubroutine(AST): calling GetCommandBlock().Apply(this)");
                node.GetCommandBlock().Apply(this);
            }
        }

        public virtual bool GetIsGlobals()
        {
            return this.isGlobals;
        }
    }
}




