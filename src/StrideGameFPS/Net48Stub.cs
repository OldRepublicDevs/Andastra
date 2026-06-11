using System;

namespace StrideGameFPS
{
    /// <summary>
    /// Stub entry point for non-Windows builds (net9.0). Stride 4.2 requires net9.0-windows.
    /// </summary>
    internal static class Net48Stub
    {
        public static void Main()
        {
            Console.WriteLine("StrideGameFPS requires Windows with Stride (net9.0-windows).");
            Environment.Exit(1);
        }
    }
}
