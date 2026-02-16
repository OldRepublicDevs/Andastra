using System;
using OdyTools.Editors.Standalone.EditorStandaloneHost;

namespace OdyTools.Editors.Standalone.DLG
{
    /// <summary>Entry point for OdyToolDLG.Standalone.exe</summary>
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            EditorStandaloneProgram.Run(args);
        }
    }
}
