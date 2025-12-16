using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Navigation;
using Odyssey.Content.Interfaces;

namespace Odyssey.Engines.Common
{
    /// <summary>
    /// Abstract base class for module management across all engines.
    /// </summary>
    public abstract class BaseEngineModule : IEngineModule
    {
        protected readonly IWorld _world;
        protected readonly IGameResourceProvider _resourceProvider;
        protected string _currentModuleName;
        protected IArea _currentArea;
        protected NavigationMesh _currentNavigationMesh;

        protected BaseEngineModule(IWorld world, IGameResourceProvider resourceProvider)
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            _world = world;
            _resourceProvider = resourceProvider;
        }

        [CanBeNull]
        public string CurrentModuleName
        {
            get { return _currentModuleName; }
            protected set { _currentModuleName = value; }
        }

        [CanBeNull]
        public IArea CurrentArea
        {
            get { return _currentArea; }
            protected set { _currentArea = value; }
        }

        [CanBeNull]
        public NavigationMesh CurrentNavigationMesh
        {
            get { return _currentNavigationMesh; }
            protected set { _currentNavigationMesh = value; }
        }

        public abstract Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null);

        public virtual void UnloadModule()
        {
            if (_currentModuleName != null)
            {
                OnUnloadModule();
                _currentModuleName = null;
                _currentArea = null;
                _currentNavigationMesh = null;
            }
        }

        public abstract bool HasModule(string moduleName);

        protected abstract void OnUnloadModule();
    }
}

