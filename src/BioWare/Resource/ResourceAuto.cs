using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.LTR;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.PCC;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.TLK;
using BioWare.Resource.Formats.TPC;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource.Formats.VIS;
using BioWare.Resource.Formats.LYT;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.ARE;
using BioWare.Resource.Formats.GFF.Generics.CNV;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using BioWare.Resource.Formats.GFF.Generics.UTM;
using JetBrains.Annotations;

namespace BioWare.Resource
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/resource_auto.py
    // Original: Automatic resource loading and saving utilities
    [PublicAPI]
    public static class ResourceAuto
    {
        private static byte[] ReadSourceBytes(object source)
        {
            if (source is string sourcePath)
            {
                return File.ReadAllBytes(sourcePath);
            }

            return source as byte[];
        }

        /// <summary>
        /// Reads a resource model from either file path or in-memory bytes.
        /// </summary>
        private static T ReadFromSource<T>(object source, Func<string, T> readFromPath, Func<byte[], T> readFromBytes)
        {
            if (source is string sourcePath)
            {
                return readFromPath(sourcePath);
            }

            return readFromBytes(source as byte[]);
        }

        private static TLK ReadTlkFromSource(object source)
        {
            return ReadFromSource(source, s => TLKAuto.ReadTlk(s), data => TLKAuto.ReadTlk(data));
        }

        private static SSF ReadSsfFromSource(object source)
        {
            return ReadFromSource(source, s => SSFAuto.ReadSsf(s), data => SSFAuto.ReadSsf(data));
        }

        private static ERF ReadErfFromSource(object source)
        {
            return ReadFromSource(source, s => ERFAuto.ReadErf(s), data => ERFAuto.ReadErf(data));
        }

        private static RIM ReadRimFromSource(object source)
        {
            return ReadFromSource(source, s => RIMAuto.ReadRim(s), data => RIMAuto.ReadRim(data));
        }

        private static PCC ReadPccFromSource(object source)
        {
            return ReadFromSource(source, s => PCCAuto.ReadPcc(s), data => PCCAuto.ReadPcc(data));
        }

        private static NCS ReadNcsFromSource(object source)
        {
            return ReadFromSource(source, s => NCSAuto.ReadNcs(s), data => NCSAuto.ReadNcs(data));
        }

        private static BWM ReadBwmFromSource(object source)
        {
            return ReadFromSource(source, s => BWMAuto.ReadBwm(s), data => BWMAuto.ReadBwm(data));
        }

        /// <summary>
        /// Converts a GFF generic resource to bytes through its dismantle function and binary type.
        /// </summary>
        private static byte[] BytesFromGffGeneric<TResource>(
            TResource resource,
            Func<TResource, BioWareGame, GFF> dismantle,
            ResourceType binaryType,
            BioWareGame game)
        {
            GFF gff = dismantle(resource, game);
            return GFFAuto.BytesGff(gff, binaryType);
        }

        private static byte[] BytesMdl(MDL mdl)
        {
            using (var ms = new MemoryStream())
            {
                MDLAuto.WriteMdl(mdl, ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Attempts a resource parse probe and suppresses exceptions.
        /// Returns null when the probe fails.
        /// </summary>
        [CanBeNull]
        private static byte[] TryReadProbe(Func<byte[]> probe)
        {
            try
            {
                return probe();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Runs parse probes in order and returns the first successful byte payload.
        /// Returns null when all probes fail.
        /// </summary>
        [CanBeNull]
        private static byte[] ReadFirstSuccessfulProbe(params Func<byte[]>[] probes)
        {
            if (probes == null)
            {
                return null;
            }

            foreach (Func<byte[]> probe in probes)
            {
                byte[] result = TryReadProbe(probe);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/resource_auto.py:58-123
        // Original: def read_resource(source: SOURCE_TYPES, resource_type: ResourceType | None = None) -> bytes:
        public static byte[] ReadResource(object source, ResourceType resourceType = null)
        {
            string sourcePath = null;
            try
            {
                if (source is string path)
                {
                    sourcePath = path;
                    if (resourceType == null)
                    {
                        var resId = ResourceIdentifier.FromPath(path);
                        resourceType = resId.ResType;
                    }
                }
            }
            catch
            {
                // Ignore errors in path detection
            }

            if (resourceType == null)
            {
                return ReadUnknownResource(source);
            }

            try
            {
                string ext = resourceType.Extension?.ToLowerInvariant() ?? "";
                string resourceExt = ext.StartsWith(".") ? ext.Substring(1) : ext;
                ResourceType extensionResourceType = ResourceType.FromExtension(resourceExt);

                if (resourceType.Category == "Talk Tables")
                {
                    TLK tlk = ReadTlkFromSource(source);
                    return TLKAuto.BytesTlk(tlk);
                }
                if (resourceType == ResourceType.TGA || resourceType == ResourceType.TPC)
                {
                    TPC tpc = TPCAuto.ReadTpc(source);
                    return TPCAuto.BytesTpc(tpc);
                }
                if (resourceExt == "ssf")
                {
                    SSF ssf = ReadSsfFromSource(source);
                    return SSFAuto.BytesSsf(ssf);
                }
                if (resourceExt == "2da")
                {
                    byte[] data2da = ReadSourceBytes(source);
                    var reader2da = new TwoDABinaryReader(data2da);
                    TwoDA twoda = reader2da.Load();
                    return TwoDAAuto.Bytes2DA(twoda);
                }
                if (resourceExt == "lip")
                {
                    LIP lip = LIPAuto.ReadLip(source);
                    return LIPAuto.BytesLip(lip);
                }
                if (extensionResourceType == ResourceType.ERF ||
                    extensionResourceType == ResourceType.MOD ||
                    extensionResourceType == ResourceType.SAV ||
                    extensionResourceType == ResourceType.HAK)
                {
                    ERF erf = ReadErfFromSource(source);
                    return ERFAuto.BytesErf(erf);
                }
                if (resourceExt == "rim")
                {
                    RIM rim = ReadRimFromSource(source);
                    return RIMAuto.BytesRim(rim);
                }
                if (resourceExt == "pcc" || resourceExt == "upk")
                {
                    PCC pcc = ReadPccFromSource(source);
                    return PCCAuto.BytesPcc(pcc);
                }
                if (resourceType.Extension?.ToUpperInvariant() == "GFF" || GFFContentExtensions.Contains(resourceType.Extension?.ToUpperInvariant() ?? ""))
                {
                    byte[] dataGff = ReadSourceBytes(source);
                    var readerGff = new GFFBinaryReader(dataGff);
                    GFF gff = readerGff.Load();
                    return GFFAuto.BytesGff(gff, ResourceType.GFF);
                }
                if (resourceExt == "ncs")
                {
                    NCS ncs = ReadNcsFromSource(source);
                    return NCSAuto.BytesNcs(ncs);
                }
                if (resourceExt == "mdl")
                {
                    MDL mdl = MDLAuto.ReadMdl(source);
                    return BytesMdl(mdl);
                }
                if (resourceExt == "vis")
                {
                    VIS vis = VISAuto.ReadVis(source);
                    return VISAuto.BytesVis(vis);
                }
                if (resourceExt == "lyt")
                {
                    BioWare.Resource.Formats.LYT.LYT lyt = LYTAuto.ReadLyt(source);
                    return LYTAuto.BytesLyt(lyt);
                }
                if (resourceExt == "ltr")
                {
                    LTR ltr = LTRAuto.ReadLtr(source);
                    return LTRAuto.BytesLtr(ltr);
                }
                if (resourceType.Category == "Walkmeshes")
                {
                    BWM bwm = ReadBwmFromSource(source);
                    return BWMAuto.BytesBwm(bwm);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Could not load resource '{sourcePath}' as resource type '{resourceType}': {e.Message}", e);
            }

            throw new ArgumentException($"Resource type {resourceType} is not supported by this library.");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/resource_auto.py:126-158
        // Original: def read_unknown_resource(source: SOURCE_TYPES) -> bytes:
        public static byte[] ReadUnknownResource(object source)
        {
            byte[] result = ReadFirstSuccessfulProbe(
                () =>
                {
                    TLK tlk = ReadTlkFromSource(source);
                    return TLKAuto.BytesTlk(tlk);
                },
                () =>
                {
                    SSF ssf = ReadSsfFromSource(source);
                    return SSFAuto.BytesSsf(ssf);
                },
                () =>
                {
                    byte[] data2da = ReadSourceBytes(source);
                    var reader2da = new TwoDABinaryReader(data2da);
                    TwoDA twoda = reader2da.Load();
                    return TwoDAAuto.Bytes2DA(twoda);
                },
                () =>
                {
                    LIP lip = LIPAuto.ReadLip(source);
                    return LIPAuto.BytesLip(lip);
                },
                () =>
                {
                    TPC tpc = TPCAuto.ReadTpc(source);
                    return TPCAuto.BytesTpc(tpc);
                },
                () =>
                {
                    ERF erf = ReadErfFromSource(source);
                    return ERFAuto.BytesErf(erf);
                },
                () =>
                {
                    RIM rim = ReadRimFromSource(source);
                    return RIMAuto.BytesRim(rim);
                },
                () =>
                {
                    NCS ncs = ReadNcsFromSource(source);
                    return NCSAuto.BytesNcs(ncs);
                },
                () =>
                {
                    byte[] dataGff = ReadSourceBytes(source);
                    var readerGff = new GFFBinaryReader(dataGff);
                    GFF gff = readerGff.Load();
                    return GFFAuto.BytesGff(gff, ResourceType.GFF);
                },
                () =>
                {
                    MDL mdl = MDLAuto.ReadMdl(source);
                    return BytesMdl(mdl);
                },
                () =>
                {
                    VIS vis = VISAuto.ReadVis(source);
                    return VISAuto.BytesVis(vis);
                },
                () =>
                {
                    BioWare.Resource.Formats.LYT.LYT lyt = LYTAuto.ReadLyt(source);
                    return LYTAuto.BytesLyt(lyt);
                },
                () =>
                {
                    LTR ltr = LTRAuto.ReadLtr(source);
                    return LTRAuto.BytesLtr(ltr);
                },
                () =>
                {
                    BWM bwm = ReadBwmFromSource(source);
                    return BWMAuto.BytesBwm(bwm);
                });
            if (result != null) return result;

            throw new ArgumentException("Source resource data not recognized as any kotor file formats.");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/resource_auto.py:210-243
        // Original: def resource_to_bytes(resource: ...) -> bytes:
        public static byte[] ResourceToBytes(object resource)
        {
            // Handle GFF generic types - convert to GFF and then to bytes
            // Matching PyKotor: uses dismantle methods to convert generics back to GFF, then bytes_gff
            if (resource is ARE are)
            {
                return AREHelpers.BytesAre(are, BioWareGame.K2);
            }
            if (resource is DLG dlg)
            {
                return DLGHelper.BytesDlg(dlg, BioWareGame.K2);
            }
            if (resource is CNV cnv)
            {
                // CNV format is only used by Eclipse Engine games
                // Default to Dragon Age Origins if game type is not specified
                return CNVHelper.BytesCnv(cnv, BioWareGame.DA);
            }
            if (resource is GIT git)
            {
                return GITHelpers.BytesGit(git, BioWareGame.K2);
            }
            if (resource is IFO ifo)
            {
                return BytesFromGffGeneric(ifo, IFOHelpers.DismantleIfo, IFO.BinaryType, BioWareGame.K2);
            }
            if (resource is JRL jrl)
            {
                return JRLHelpers.BytesJrl(jrl);
            }
            if (resource is PTH pth)
            {
                return PTHAuto.BytesPth(pth, BioWareGame.K2);
            }
            if (resource is UTC utc)
            {
                return UTCHelpers.BytesUtc(utc, BioWareGame.K2);
            }
            if (resource is UTD utd)
            {
                return BytesFromGffGeneric(utd, (value, game) => UTDHelpers.DismantleUtd(value, game), UTD.BinaryType, BioWareGame.K2);
            }
            if (resource is UTE ute)
            {
                return BytesFromGffGeneric(ute, (value, game) => UTEHelpers.DismantleUte(value, game), UTE.BinaryType, BioWareGame.K2);
            }
            if (resource is UTM utm)
            {
                return BytesFromGffGeneric(utm, (value, game) => UTMHelpers.DismantleUtm(value, game), UTM.BinaryType, BioWareGame.K2);
            }
            if (resource is UTP utp)
            {
                return BytesFromGffGeneric(utp, (value, game) => UTPHelpers.DismantleUtp(value, game), UTP.BinaryType, BioWareGame.K2);
            }
            if (resource is UTS uts)
            {
                return BytesFromGffGeneric(uts, (value, game) => UTSHelpers.DismantleUts(value, game), UTS.BinaryType, BioWareGame.K2);
            }
            if (resource is UTW utw)
            {
                return UTWAuto.BytesUtw(utw, BioWareGame.K2);
            }
            if (resource is UTT utt)
            {
                return UTTAuto.BytesUtt(utt, BioWareGame.K2);
            }
            if (resource is UTI uti)
            {
                return UTIHelpers.BytesUti(uti, BioWareGame.K2);
            }
            if (resource is BWM bwm)
            {
                return BWMAuto.BytesBwm(bwm);
            }
            if (resource is GFF gff)
            {
                return GFFAuto.BytesGff(gff, ResourceType.GFF);
            }
            if (resource is ERF erf)
            {
                return ERFAuto.BytesErf(erf);
            }
            if (resource is LIP lip)
            {
                return LIPAuto.BytesLip(lip);
            }
            if (resource is LTR ltr)
            {
                return LTRAuto.BytesLtr(ltr);
            }
            if (resource is BioWare.Resource.Formats.LYT.LYT lyt)
            {
                return LYTAuto.BytesLyt(lyt);
            }
            if (resource is MDL mdl)
            {
                return BytesMdl(mdl);
            }
            if (resource is NCS ncs)
            {
                return NCSAuto.BytesNcs(ncs);
            }
            if (resource is RIM rim)
            {
                return RIMAuto.BytesRim(rim);
            }
            if (resource is SSF ssf)
            {
                return SSFAuto.BytesSsf(ssf);
            }
            if (resource is TLK tlk)
            {
                return TLKAuto.BytesTlk(tlk);
            }
            if (resource is TPC tpc)
            {
                return TPCAuto.BytesTpc(tpc);
            }
            if (resource is TwoDA twoda)
            {
                return TwoDAAuto.Bytes2DA(twoda);
            }
            if (resource is VIS vis)
            {
                return VISAuto.BytesVis(vis);
            }

            throw new ArgumentException($"Invalid resource {resource} of type '{resource.GetType().Name}' passed to ResourceToBytes.");
        }

        private static readonly System.Collections.Generic.HashSet<string> GFFContentExtensions = new System.Collections.Generic.HashSet<string>
        {
            "ARE", "IFO", "GIT", "UTC", "UTI", "UTD", "UTE", "UTP", "UTS", "UTT", "UTW", "DLG", "CNV", "JRL", "PTH"
        };

        /// <summary>
        /// Loads a known binary resource by type id.
        /// Returns null for unsupported type ids.
        /// </summary>
        [CanBeNull]
        private static object LoadKnownBinaryResource(byte[] data, int typeId)
        {
            switch (typeId)
            {
                case 2002: // ERF
                    return new ERFBinaryReader(data).Load();

                case 2005: // GFF
                    return new GFFBinaryReader(data).Load();

                case 2008: // RIM
                    return new RIMBinaryReader(data).Load();

                case 10000: // PCC
                case 10001: // UPK
                    return new PCCBinaryReader(data).Load();

                case 2017: // SSF
                    return new SSFBinaryReader(data).Load();

                case 2018: // TLK
                    return new TLKBinaryReader(data).Load();

                case 2019: // 2DA
                    return new TwoDABinaryReader(data).Load();

                case 2034: // CNV
                    return CNVHelper.ReadCnv(data);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Automatically loads a resource from file data based on its type.
        /// </summary>
        /// <param name="data">The resource data.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>The loaded resource object, or null if loading failed.</returns>
        [CanBeNull]
        public static object LoadResource(byte[] data, ResourceType resourceType)
        {
            if (data == null || resourceType == null)
            {
                return null;
            }

            try
            {
                return LoadKnownBinaryResource(data, resourceType.TypeId);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically loads a resource from a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded resource object, or null if loading failed.</returns>
        [CanBeNull]
        public static object LoadResourceFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            try
            {
                byte[] data = File.ReadAllBytes(filePath);
                string extension = Path.GetExtension(filePath).ToLowerInvariant();

                ResourceType resourceType = ResourceType.FromExtension(extension);
                if (resourceType == null)
                {
                    return null;
                }

                return LoadResource(data, resourceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically saves a resource object to bytes based on its type.
        /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/resource_auto.py:resource_to_bytes
        /// Original: def resource_to_bytes(resource: ...) -> bytes:
        /// </summary>
        /// <param name="resource">The resource object.</param>
        /// <param name="resourceType">The resource type (used for validation and format-specific handling).</param>
        /// <returns>The resource data as bytes, or null if saving failed.</returns>
        [CanBeNull]
        public static byte[] SaveResource(object resource, ResourceType resourceType)
        {
            if (resource == null || resourceType == null)
            {
                return null;
            }

            try
            {
                // Delegate to ResourceToBytes which handles all resource types by checking the object type
                // This matches the PyKotor implementation where resource_to_bytes handles all formats
                return ResourceToBytes(resource);
            }
            catch (ArgumentException)
            {
                // ResourceToBytes throws ArgumentException for unsupported types
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically saves a resource object to a file.
        /// </summary>
        /// <param name="resource">The resource object.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="filePath">The file path to save to.</param>
        /// <returns>True if saving succeeded, false otherwise.</returns>
        public static bool SaveResourceToFile(object resource, ResourceType resourceType, string filePath)
        {
            if (resource == null || resourceType == null || string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            try
            {
                byte[] data = SaveResource(resource, resourceType);
                if (data == null)
                {
                    return false;
                }

                File.WriteAllBytes(filePath, data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
