using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using KotorDiff.Gui;
using OdyTools.Data;

namespace OdyTools.Windows
{
    /// <summary>
    /// Hosts the shared KotorDiff GUI from src/Tools/KotorDiff (Holocron/PyKotor parity).
    /// </summary>
    public class KotorDiffWindow : KotorDiffApp
    {
        public KotorDiffWindowUi Ui { get; private set; }

        public KotorDiffWindow(
            Window parent = null,
            Dictionary<string, OdyInstallation> installations = null,
            OdyInstallation activeInstallation = null)
            : base(BuildInstallationPaths(installations), activeInstallation?.Path)
        {
            Title = "KotorDiff - OdyTools";
            Ui = new KotorDiffWindowUi();
        }

        private static IEnumerable<string> BuildInstallationPaths(Dictionary<string, OdyInstallation> installations)
        {
            if (installations == null || installations.Count == 0)
            {
                return null;
            }
            return installations.Values
                .Where(i => i != null && !string.IsNullOrEmpty(i.Path))
                .Select(i => i.Path)
                .Distinct()
                .ToList();
        }

        public void Compare()
        {
            Show();
        }
    }

    public class KotorDiffWindowUi
    {
    }
}
