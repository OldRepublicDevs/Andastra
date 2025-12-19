using System.Collections.Generic;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Scripting.Interfaces;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Abstract base class for game profiles across all engines.
    /// </summary>
    /// <remarks>
    /// Base Engine Profile:
    /// - Common functionality shared across all BioWare engines (Odyssey, Aurora, Eclipse, Infinity)
    /// - Base classes MUST only contain functionality that is identical across ALL engines
    /// - Engine-specific details MUST be in subclasses (OdysseyEngineProfile, AuroraEngineProfile, EclipseEngineProfile)
    /// - TODO: Complete cross-engine reverse engineering to identify common profile patterns
    /// </remarks>
    public abstract class BaseEngineProfile : IEngineProfile
    {
        protected readonly IResourceConfig _resourceConfig;
        protected readonly ITableConfig _tableConfig;
        protected readonly HashSet<string> _supportedFeatures;

        protected BaseEngineProfile()
        {
            _resourceConfig = CreateResourceConfig();
            _tableConfig = CreateTableConfig();
            _supportedFeatures = new HashSet<string>();
            InitializeSupportedFeatures();
        }

        public abstract string GameType { get; }

        public abstract string Name { get; }

        public abstract EngineFamily EngineFamily { get; }

        public abstract IEngineApi CreateEngineApi();

        public IResourceConfig ResourceConfig
        {
            get { return _resourceConfig; }
        }

        public ITableConfig TableConfig
        {
            get { return _tableConfig; }
        }

        public bool SupportsFeature(string feature)
        {
            if (string.IsNullOrEmpty(feature))
            {
                return false;
            }

            return _supportedFeatures.Contains(feature);
        }

        protected abstract IResourceConfig CreateResourceConfig();

        protected abstract ITableConfig CreateTableConfig();

        protected abstract void InitializeSupportedFeatures();
    }
}


