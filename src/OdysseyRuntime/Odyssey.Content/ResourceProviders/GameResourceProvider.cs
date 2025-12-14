using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpKOTOR.Common;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using Odyssey.Content.Interfaces;
using KotorSearchLocation = CSharpKOTOR.Installation.SearchLocation;
using OdysseySearchLocation = Odyssey.Content.Interfaces.SearchLocation;

namespace Odyssey.Content.ResourceProviders
{
    /// <summary>
    /// Resource provider that wraps CSharpKOTOR.Installation for unified resource access.
    /// </summary>
    public class GameResourceProvider : IGameResourceProvider
    {
        private readonly Installation _installation;
        private readonly GameType _gameType;
        private string _currentModule;

        public GameResourceProvider(Installation installation)
        {
            _installation = installation ?? throw new ArgumentNullException("installation");
            _gameType = installation.Game == Game.K1 ? GameType.K1 : GameType.K2;
        }

        public GameType GameType { get { return _gameType; } }

        /// <summary>
        /// The installation this provider wraps.
        /// </summary>
        public Installation Installation { get { return _installation; } }

        /// <summary>
        /// Sets the current module context for resource lookups.
        /// </summary>
        public void SetCurrentModule(string moduleResRef)
        {
            _currentModule = moduleResRef;
        }

        public async Task<Stream> OpenResourceAsync(ResourceIdentifier id, CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                var result = _installation.Resources.LookupResource(
                    id.ResName,
                    id.ResType,
                    null,
                    _currentModule
                );

                if (result == null)
                {
                    return null;
                }

                byte[] data = result.Data;
                if (data == null || data.Length == 0)
                {
                    return null;
                }

                return new MemoryStream(data, writable: false);
            }, ct);
        }

        public async Task<bool> ExistsAsync(ResourceIdentifier id, CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                var result = _installation.Resources.LookupResource(
                    id.ResName,
                    id.ResType,
                    null,
                    _currentModule
                );

                return result != null;
            }, ct);
        }

        public async Task<IReadOnlyList<Odyssey.Content.Interfaces.LocationResult>> LocateAsync(
            ResourceIdentifier id,
            OdysseySearchLocation[] order,
            CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                KotorSearchLocation[] kotorOrder = order != null
                    ? order.Select(ConvertSearchLocation).Where(l => l.HasValue).Select(l => l.Value).ToArray()
                    : null;

                var results = _installation.Resources.LocateResource(
                    id.ResName,
                    id.ResType,
                    kotorOrder,
                    _currentModule
                );

                var converted = new List<Odyssey.Content.Interfaces.LocationResult>();
                foreach (var r in results)
                {
                    converted.Add(new Odyssey.Content.Interfaces.LocationResult
                    {
                        Location = ConvertBackSearchLocation(r.FilePath),
                        Path = r.FilePath,
                        Size = r.Size,
                        Offset = r.Offset
                    });
                }

                return (IReadOnlyList<Odyssey.Content.Interfaces.LocationResult>)converted;
            }, ct);
        }

        public IEnumerable<ResourceIdentifier> EnumerateResources(ResourceType type)
        {
            // This would enumerate resources from the installation
            // For now, return empty - full implementation would scan all archives
            yield break;
        }

        public async Task<byte[]> GetResourceBytesAsync(ResourceIdentifier id, CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                var result = _installation.Resources.LookupResource(
                    id.ResName,
                    id.ResType,
                    null,
                    _currentModule
                );

                return result?.Data;
            }, ct);
        }

        #region Type Conversion

        private static KotorSearchLocation? ConvertSearchLocation(OdysseySearchLocation location)
        {
            switch (location)
            {
                case OdysseySearchLocation.Override: return KotorSearchLocation.OVERRIDE;
                case OdysseySearchLocation.Module: return KotorSearchLocation.MODULES;
                case OdysseySearchLocation.Chitin: return KotorSearchLocation.CHITIN;
                case OdysseySearchLocation.TexturePacks: return KotorSearchLocation.TEXTURES_TPA;
                default: return null;
            }
        }

        private static OdysseySearchLocation ConvertBackSearchLocation(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return OdysseySearchLocation.Chitin;
            }

            string lower = path.ToLowerInvariant();
            if (lower.Contains("override"))
            {
                return OdysseySearchLocation.Override;
            }
            if (lower.Contains("modules"))
            {
                return OdysseySearchLocation.Module;
            }
            if (lower.Contains("texturepacks"))
            {
                return OdysseySearchLocation.TexturePacks;
            }

            return OdysseySearchLocation.Chitin;
        }

        #endregion
    }
}

