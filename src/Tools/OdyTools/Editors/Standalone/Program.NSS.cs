using System;
using OdyTools.Editors.Standalone.EditorStandaloneHost;

namespace OdyTools.Editors.Standalone.NSS
{
    /// <summary>Entry point for OdyToolNSS.Standalone.exe</summary>
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            EditorStandaloneProgram.Run(args);
        }
    }
}
