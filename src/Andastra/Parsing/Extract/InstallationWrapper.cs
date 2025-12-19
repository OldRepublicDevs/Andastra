using System.Collections.Generic;
using Andastra.Parsing.Installation;
using ResourceType = Andastra.Parsing.Resource.ResourceType;
using LocationResult = Andastra.Parsing.Extract.LocationResult;
using Andastra.Parsing.Common;

namespace Andastra.Parsing.Extract
{
    // Thin wrapper to mirror PyKotor extract.installation.Installation semantics.
    public class InstallationWrapper
    {
        private readonly Installation.Installation _installation;

        public InstallationWrapper(string installPath)
        {
            _installation = new Installation.Installation(installPath);
        }

        public Andastra.Parsing.Installation.ResourceResult Resource(string resref, ResourceType restype)
        {
            return _installation.Resources.LookupResource(resref, restype);
        }

        public List<LocationResult> Locate(string resref, ResourceType restype)
        {
            return _installation.Resources.LocateResource(resref, restype);
        }

        public Installation.Installation Inner => _installation;
    }
}
