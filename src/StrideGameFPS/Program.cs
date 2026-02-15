using System;
using Stride.Engine;
using Stride.CommunityToolkit.Engine;

namespace StrideGameFPS
{
    /// <summary>
    /// The main entry point for the application.
    /// Uses Stride Community Toolkit (2025 code-only pattern): Run(start:) creates the window and start callback adds the graphics compositor so the game renders.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new FPSGame();
            // Toolkit Run(start:) creates Windows GameContext and window; AddGraphicsCompositor() adds default compositor so we get a visible window and our Draw() output is presented.
            game.Run(start: (Scene _) => game.AddGraphicsCompositor());
        }
    }
}
