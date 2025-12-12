// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class ASwitchCase : ScriptRootNode
    {
        protected AConst val;
        public ASwitchCase(int start, AConst val) : base(start, -1)
        {
            this.Val(val);
        }

        public ASwitchCase(int start) : base(start, -1)
        {
        }

        public virtual void End(int end)
        {
            this.end = end;
        }

        private void Val(AConst val)
        {
            val.Parent(this);
            this.val = val;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitchCase.java:32-42
        // Original: public List<AUnkLoopControl> getUnknowns() { ... }
        public virtual List<AUnkLoopControl> GetUnknowns()
        {
            List<AUnkLoopControl> unks = new List<AUnkLoopControl>();

            foreach (ScriptNode node in this.children)
            {
                if (node is AUnkLoopControl)
                {
                    unks.Add((AUnkLoopControl)node);
                }
            }

            return unks;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitchCase.java:44-48
        // Original: public void replaceUnknown(AUnkLoopControl unk, ScriptNode newnode) { newnode.parent(this); this.children.set(this.children.indexOf(unk), newnode); unk.parent(null); }
        public virtual void ReplaceUnknown(AUnkLoopControl unk, ScriptNode newnode)
        {
            newnode.Parent(this);
            this.children[this.children.IndexOf(unk)] = newnode;
            unk.Parent(null);
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitchCase.java:50-64
        // Original: @Override public String toString() { ... }
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            if (this.val == null)
            {
                buff.Append(this.tabs + "default:" + this.newline);
            }
            else
            {
                buff.Append(this.tabs + "case " + this.val.ToString() + ":" + this.newline);
            }

            for (int i = 0; i < this.children.Count; i++)
            {
                buff.Append(this.children[i].ToString());
            }

            return buff.ToString();
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitchCase.java
        // Original: @Override public void close()
        public override void Close()
        {
            base.Close();
            if (this.val != null)
            {
                this.val.Close();
            }

            this.val = null;
        }
    }
}




