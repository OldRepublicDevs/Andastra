// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode
{
    public class ASwitch : ScriptNode
    {
        protected AExpression switchexp;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:17
        // Original: protected List<ASwitchCase> cases;
        protected List<ASwitchCase> cases;
        protected ASwitchCase defaultcase;
        protected int start;
        protected int end;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:22-26
        // Original: public ASwitch(int start, AExpression switchexp) { this.start = start; this.cases = new ArrayList<>(); this.switchExp(switchexp); }
        public ASwitch(int start, AExpression switchexp)
        {
            this.start = start;
            this.cases = new List<ASwitchCase>();
            this.SwitchExp(switchexp);
        }

        public virtual void SwitchExp(AExpression switchexp)
        {
            switchexp.Parent(this);
            this.switchexp = switchexp;
        }

        public virtual AExpression SwitchExp()
        {
            return this.switchexp;
        }

        public virtual void End(int end)
        {
            this.end = end;
            if (this.defaultcase != null)
            {
                this.defaultcase.End(end);
            }
            else if (this.cases.Count > 0)
            {
                ((ASwitchCase)this.cases[this.cases.Count - 1]).End(end);
            }
        }

        public virtual int End()
        {
            return this.end;
        }

        public virtual void AddCase(ASwitchCase acase)
        {
            acase.Parent(this);
            this.cases.Add(acase);
        }

        public virtual void AddDefaultCase(ASwitchCase acase)
        {
            acase.Parent(this);
            this.defaultcase = acase;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:60-62
        // Original: public ASwitchCase getLastCase() { return this.cases.get(this.cases.size() - 1); }
        public virtual ASwitchCase GetLastCase()
        {
            return this.cases[this.cases.Count - 1];
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:64-77
        // Original: public ASwitchCase getNextCase(ASwitchCase lastcase) { ... }
        public virtual ASwitchCase GetNextCase(ASwitchCase lastcase)
        {
            if (lastcase == null)
            {
                return this.GetFirstCase();
            }
            else if (lastcase.Equals(this.defaultcase))
            {
                return null;
            }
            else
            {
                int index = this.cases.IndexOf(lastcase) + 1;
                if (index == 0)
                {
                    throw new Exception("invalid last case passed in");
                }
                else
                {
                    return this.cases.Count > index ? this.cases[index] : this.defaultcase;
                }
            }
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:79-81
        // Original: public ASwitchCase getFirstCase() { return this.cases.size() > 0 ? this.cases.get(0) : this.defaultcase; }
        public virtual ASwitchCase GetFirstCase()
        {
            return this.cases.Count > 0 ? this.cases[0] : this.defaultcase;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:83-89
        // Original: public int getFirstCaseStart() { ... }
        public virtual int GetFirstCaseStart()
        {
            if (this.cases.Count > 0)
            {
                return this.cases[0].GetStart();
            }
            else
            {
                return this.defaultcase != null ? this.defaultcase.GetStart() : -1;
            }
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java:91-106
        // Original: @Override public String toString() { ... }
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.tabs + "switch(" + this.switchexp + ") {" + this.newline);

            for (int i = 0; i < this.cases.Count; i++)
            {
                buff.Append(this.cases[i].ToString());
            }

            if (this.defaultcase != null)
            {
                buff.Append(this.defaultcase.ToString());
            }

            buff.Append(this.tabs + "}" + this.newline);
            return buff.ToString();
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/scriptnode/ASwitch.java
        // Original: @Override public void close()
        public override void Close()
        {
            base.Close();
            if (this.cases != null)
            {
                foreach (ScriptNode param in this.cases)
                {
                    param.Close();
                }

                this.cases = null;
            }

            if (this.switchexp != null)
            {
                ((ScriptNode)this.switchexp).Close();
            }

            this.switchexp = null;
            if (this.defaultcase != null)
            {
                this.defaultcase.Close();
            }

            this.defaultcase = null;
        }
    }
}




