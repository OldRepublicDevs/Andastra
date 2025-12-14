using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpKOTOR.Common;
using CSharpKOTOR.Installation;
using Odyssey.Content.Interfaces;
using KotorResourceType = CSharpKOTOR.Resources.ResourceType;
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

                KotorResourceType kotorType = ConvertResourceType(id.Type);
                if (kotorType == null)
                {
                    return null;
                }

                var result = _installation.Resources.LookupResource(
                    id.ResRef,
                    kotorType,
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

                KotorResourceType kotorType = ConvertResourceType(id.Type);
                if (kotorType == null)
                {
                    return false;
                }

                var result = _installation.Resources.LookupResource(
                    id.ResRef,
                    kotorType,
                    null,
                    _currentModule
                );

                return result != null;
            }, ct);
        }

        public async Task<IReadOnlyList<LocationResult>> LocateAsync(
            ResourceIdentifier id,
            OdysseySearchLocation[] order,
            CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                KotorResourceType kotorType = ConvertResourceType(id.Type);
                if (kotorType == null)
                {
                    return new List<LocationResult>();
                }

                KotorSearchLocation[] kotorOrder = order != null
                    ? order.Select(ConvertSearchLocation).Where(l => l.HasValue).Select(l => l.Value).ToArray()
                    : null;

                var results = _installation.Resources.LocateResource(
                    id.ResRef,
                    kotorType,
                    kotorOrder,
                    _currentModule
                );

                var converted = new List<LocationResult>();
                foreach (var r in results)
                {
                    converted.Add(new LocationResult
                    {
                        Location = ConvertBackSearchLocation(r.FilePath),
                        Path = r.FilePath,
                        Size = r.Size,
                        Offset = r.Offset
                    });
                }

                return converted;
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

                KotorResourceType kotorType = ConvertResourceType(id.Type);
                if (kotorType == null)
                {
                    return null;
                }

                var result = _installation.Resources.LookupResource(
                    id.ResRef,
                    kotorType,
                    null,
                    _currentModule
                );

                return result?.Data;
            }, ct);
        }

        #region Type Conversion

        private static KotorResourceType ConvertResourceType(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.TGA: return KotorResourceType.TGA;
                case ResourceType.WAV: return KotorResourceType.WAV;
                case ResourceType.TXT: return KotorResourceType.TXT;
                case ResourceType.MDL: return KotorResourceType.MDL;
                case ResourceType.MDX: return KotorResourceType.MDX;
                case ResourceType.NSS: return KotorResourceType.NSS;
                case ResourceType.NCS: return KotorResourceType.NCS;
                case ResourceType.ARE: return KotorResourceType.ARE;
                case ResourceType.IFO: return KotorResourceType.IFO;
                case ResourceType.WOK: return KotorResourceType.WOK;
                case ResourceType.TwoDA: return KotorResourceType.TwoDA;
                case ResourceType.TLK: return KotorResourceType.TLK;
                case ResourceType.TXI: return KotorResourceType.TXI;
                case ResourceType.GIT: return KotorResourceType.GIT;
                case ResourceType.UTI: return KotorResourceType.UTI;
                case ResourceType.UTC: return KotorResourceType.UTC;
                case ResourceType.DLG: return KotorResourceType.DLG;
                case ResourceType.UTT: return KotorResourceType.UTT;
                case ResourceType.UTS: return KotorResourceType.UTS;
                case ResourceType.LTR: return KotorResourceType.LTR;
                case ResourceType.GFF: return KotorResourceType.GFF;
                case ResourceType.UTE: return KotorResourceType.UTE;
                case ResourceType.UTD: return KotorResourceType.UTD;
                case ResourceType.UTP: return KotorResourceType.UTP;
                case ResourceType.UTM: return KotorResourceType.UTM;
                case ResourceType.DWK: return KotorResourceType.DWK;
                case ResourceType.PWK: return KotorResourceType.PWK;
                case ResourceType.UTW: return KotorResourceType.UTW;
                case ResourceType.SSF: return KotorResourceType.SSF;
                case ResourceType.LYT: return KotorResourceType.LYT;
                case ResourceType.VIS: return KotorResourceType.VIS;
                case ResourceType.PTH: return KotorResourceType.PTH;
                case ResourceType.LIP: return KotorResourceType.LIP;
                // Note: BWM uses WOK type ID in CSharpKOTOR
                case ResourceType.BWM: return KotorResourceType.WOK;
                case ResourceType.TPC: return KotorResourceType.TPC;
                default: return null;
            }
        }

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

