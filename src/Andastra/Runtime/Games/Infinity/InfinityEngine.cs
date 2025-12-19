using System;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Content.ResourceProviders;
using Andastra.Runtime.Engines.Common;

namespace Andastra.Runtime.Engines.Infinity
{
    /// <summary>
    /// Infinity Engine implementation for Baldur's Gate, Icewind Dale, and Planescape: Torment.
    /// </summary>
    /// <remarks>
    /// Infinity Engine:
    /// - Based on Infinity Engine architecture (Baldur's Gate, Icewind Dale, Planescape: Torment)
    /// - Resource provider: Uses Infinity-specific resource system (BIF files, KEY index files, override directory)
    /// - Game session: Coordinates module loading, entity management, script execution for Infinity games
    /// - Cross-engine: Similar engine initialization pattern to Odyssey/Aurora/Eclipse but different resource system
    ///   - Odyssey: RIM/ERF/BIF files with chitin.key index
    ///   - Aurora: HAK files with module files
    ///   - Eclipse: PCC/UPK packages with streaming resources
    ///   - Infinity: BIF files with KEY index files
    /// - Inheritance: BaseEngine (Runtime.Games.Common) implements common engine initialization
    ///   - Infinity: InfinityEngine : BaseEngine (Runtime.Games.Infinity) - Infinity-specific resource provider (InfinityResourceProvider)
    /// - Original implementation: Infinity Engine uses CResMan/CResManager for resource loading
    /// - Resource precedence: OVERRIDE > MODULE > BIF (via KEY) > HARDCODED
    /// - TODO: Reverse engineer specific function addresses from Infinity Engine executables using Ghidra MCP
    ///   - Baldur's Gate: BaldurGate.exe engine initialization functions
    ///   - Icewind Dale: IcewindDale.exe engine initialization functions
    ///   - Planescape: Torment: PlanescapeTorment.exe engine initialization functions
    /// </remarks>
    public class InfinityEngine : BaseEngine
    {
        private string _installationPath;

        public InfinityEngine(IEngineProfile profile)
            : base(profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (profile.EngineFamily != EngineFamily.Infinity)
            {
                throw new ArgumentException("Profile must be for Infinity engine family", nameof(profile));
            }
        }

        public override IEngineGame CreateGameSession()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Engine must be initialized before creating game session");
            }

            return new InfinityGameSession(this);
        }

        protected override IGameResourceProvider CreateResourceProvider(string installationPath)
        {
            if (string.IsNullOrEmpty(installationPath))
            {
                throw new ArgumentException("Installation path cannot be null or empty", nameof(installationPath));
            }

            _installationPath = installationPath;

            // Determine game type from installation path
            // Infinity Engine games: Baldur's Gate, Icewind Dale, Planescape: Torment
            GameType gameType = DetectInfinityGameType(installationPath);

            return new InfinityResourceProvider(installationPath, gameType);
        }

        /// <summary>
        /// Detects the specific Infinity Engine game type from the installation path.
        /// </summary>
        /// <param name="installationPath">The installation path to check.</param>
        /// <returns>The detected game type, or Unknown if detection fails.</returns>
        /// <remarks>
        /// Infinity Engine Game Detection:
        /// - Based on Infinity Engine game detection patterns (Baldur's Gate, Icewind Dale, Planescape: Torment)
        /// - Detection method: Checks for game-specific executable files in installation directory
        /// - Baldur's Gate: Checks for "BaldurGate.exe" or "BALDURGATE.EXE"
        /// - Icewind Dale: Checks for "IcewindDale.exe" or "ICEWINDDALE.EXE"
        /// - Planescape: Torment: Checks for "PlanescapeTorment.exe" or "PLANESCAPETORMENT.EXE"
        /// - Fallback: Checks for KEY file (chitin.key) and game-specific module files if executables not found
        /// - Similar to Odyssey Engine detection pattern (swkotor.exe/swkotor2.exe detection)
        /// - Original implementation: Infinity Engine executables identify themselves via executable name
        /// - Cross-engine: Similar detection pattern across all BioWare engines (executable name + fallback file checks)
        /// </remarks>
        private static GameType DetectInfinityGameType(string installationPath)
        {
            if (string.IsNullOrEmpty(installationPath) || !System.IO.Directory.Exists(installationPath))
            {
                return GameType.Unknown;
            }

            // Check for Baldur's Gate executable
            string baldurGateExe = System.IO.Path.Combine(installationPath, "BaldurGate.exe");
            string baldurGateExeUpper = System.IO.Path.Combine(installationPath, "BALDURGATE.EXE");
            if (System.IO.File.Exists(baldurGateExe) || System.IO.File.Exists(baldurGateExeUpper))
            {
                return GameType.BaldursGate;
            }

            // Check for Icewind Dale executable
            string icewindDaleExe = System.IO.Path.Combine(installationPath, "IcewindDale.exe");
            string icewindDaleExeUpper = System.IO.Path.Combine(installationPath, "ICEWINDDALE.EXE");
            if (System.IO.File.Exists(icewindDaleExe) || System.IO.File.Exists(icewindDaleExeUpper))
            {
                return GameType.IcewindDale;
            }

            // Check for Planescape: Torment executable
            string planescapeExe = System.IO.Path.Combine(installationPath, "PlanescapeTorment.exe");
            string planescapeExeUpper = System.IO.Path.Combine(installationPath, "PLANESCAPETORMENT.EXE");
            if (System.IO.File.Exists(planescapeExe) || System.IO.File.Exists(planescapeExeUpper))
            {
                return GameType.PlanescapeTorment;
            }

            // Fallback: Check for KEY file and game-specific module files
            string keyFilePath = System.IO.Path.Combine(installationPath, "chitin.key");
            if (System.IO.File.Exists(keyFilePath))
            {
                // Try to detect based on module files or other game-specific files
                string dataPath = System.IO.Path.Combine(installationPath, "data");
                if (System.IO.Directory.Exists(dataPath))
                {
                    // Check for game-specific GAM files or other indicators
                    string baldurGam = System.IO.Path.Combine(dataPath, "Baldur.gam");
                    if (System.IO.File.Exists(baldurGam))
                    {
                        return GameType.BaldursGate;
                    }

                    string icewindGam = System.IO.Path.Combine(dataPath, "Icewind.gam");
                    if (System.IO.File.Exists(icewindGam))
                    {
                        return GameType.IcewindDale;
                    }

                    string tormentGam = System.IO.Path.Combine(dataPath, "Torment.gam");
                    if (System.IO.File.Exists(tormentGam))
                    {
                        return GameType.PlanescapeTorment;
                    }
                }
            }

            return GameType.Unknown;
        }
    }
}


