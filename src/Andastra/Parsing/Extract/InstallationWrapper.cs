using System.Collections.Generic;
using Andastra.Parsing.Common;
// TODO: STUB - InstallationWrapper removed to break circular dependency
// Installation → Extract (needs Capsule, FileResource)
// Extract → Installation (was using InstallationWrapper)
// Solution: InstallationWrapper moved to Installation project or removed
// LocationResult = Andastra.Parsing.Extract.LocationResult;

namespace Andastra.Parsing.Extract
{
    // Thin wrapper to mirror PyKotor extract.installation.Installation semantics.
    // TODO: STUB - This class was removed to break circular dependency between Extract and Installation
    // If needed, move this to Installation project or use a different approach
    public class InstallationWrapper
    {
        // TODO: STUB - Implementation removed to break circular dependency
        // Original implementation used Installation.Installation which created:
        // Extract → Installation → Extract circular dependency
        public InstallationWrapper(string installPath)
        {
            throw new System.NotImplementedException("InstallationWrapper removed to break circular dependency. Use Installation.Installation directly or move this class to Installation project.");
        }
    }
}
