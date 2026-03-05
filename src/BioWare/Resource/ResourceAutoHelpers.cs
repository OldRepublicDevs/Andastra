using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.ARE;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.GFF.Generics.UTI;

namespace BioWare.Resource
{
    /// <summary>
    /// Helper functions for auto-detection and resource reading.
    /// Includes Auto format detection/conversion functions and centralized source/target dispatching.
    /// </summary>
    // Helper functions for reading GFF-based resources
    // These will be used by ModuleResource<T>.Resource() to load resources
    public static class ResourceAutoHelpers
    {
        /// <summary>
        /// Centralized source/target dispatching for Auto format readers.
        /// Eliminates duplicated pattern: if (source is string) { } else if (source is byte[]) { } else if (source is Stream) { }
        /// Used by GFFAuto, TwoDAAuto, TLKAuto, MDLAuto, and other format-specific Auto classes.
        /// </summary>
        public static class SourceDispatcher
        {
            /// <summary>
            /// Converts any supported source (file path, byte array, or stream) to a raw byte array.
            /// Also returns the source filepath if source is a string path (null otherwise).
            /// </summary>
            /// <param name="source">Source object: string (file path), byte[], or Stream</param>
            /// <param name="filepath">Output: file path if source is string, null otherwise</param>
            /// <returns>Byte array representation of source</returns>
            /// <exception cref="ArgumentException">If source type is not supported</exception>
            public static byte[] ToBytes(object source, out string filepath)
            {
                if (source is string path)
                {
                    filepath = path;
                    return File.ReadAllBytes(path);
                }
                if (source is byte[] bytes)
                {
                    filepath = null;
                    return bytes;
                }
                if (source is Stream stream)
                {
                    filepath = null;
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
                throw new ArgumentException("Source must be a file path (string), byte array (byte[]), or stream (Stream).", nameof(source));
            }

            /// <summary>
            /// Converts any supported source (file path, byte array, or stream) to a raw byte array.
            /// Discards filepath information if present.
            /// </summary>
            /// <param name="source">Source object: string (file path), byte[], or Stream</param>
            /// <returns>Byte array representation of source</returns>
            public static byte[] ToBytes(object source)
            {
                return ToBytes(source, out _);
            }

            /// <summary>
            /// Writes bytes to any supported target (file path or stream).
            /// </summary>
            /// <param name="data">Byte array to write</param>
            /// <param name="target">Target object: string (file path) or Stream</param>
            /// <exception cref="ArgumentException">If target type is not supported</exception>
            public static void WriteBytes(byte[] data, object target)
            {
                if (target is string filepath)
                {
                    File.WriteAllBytes(filepath, data);
                }
                else if (target is Stream stream)
                {
                    stream.Write(data, 0, data.Length);
                }
                else
                {
                    throw new ArgumentException("Target must be a file path (string) or stream (Stream).", nameof(target));
                }
            }

            /// <summary>
            /// Writes text (UTF-8 encoded) to any supported target (file path or stream).
            /// </summary>
            /// <param name="text">Text to write</param>
            /// <param name="target">Target object: string (file path) or Stream</param>
            public static void WriteText(string text, object target)
            {
                if (target is string filepath)
                {
                    File.WriteAllText(filepath, text);
                }
                else if (target is Stream stream)
                {
                    var data = System.Text.Encoding.UTF8.GetBytes(text);
                    stream.Write(data, 0, data.Length);
                }
                else
                {
                    throw new ArgumentException("Target must be a file path (string) or stream (Stream).", nameof(target));
                }
            }

            /// <summary>
            /// Dispatches write operations to either a file path or stream using provided lambdas.
            /// Consolidates repeated pattern: if (target is string) { }  else if (target is Stream) { }
            /// </summary>
            /// <param name="target">Target object: string (file path) or Stream</param>
            /// <param name="writeToPath">Action to execute if target is a file path</param>
            /// <param name="writeToStream">Action to execute if target is a Stream</param>
            /// <param name="formatName">Optional format name for error messages</param>
            /// <exception cref="ArgumentException">If target type is not supported</exception>
            public static void DispatchWrite(object target, Action<string> writeToPath, Action<Stream> writeToStream, string formatName = "output")
            {
                if (target is string filepath)
                {
                    writeToPath(filepath);
                    return;
                }

                if (target is Stream stream)
                {
                    writeToStream(stream);
                    return;
                }

                throw new ArgumentException($"Target must be a file path (string) or stream (Stream) for {formatName}.", nameof(target));
            }
        }
    
        public static ARE ReadAre(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return AREHelpers.ConstructAre(gff);
        }

        public static GIT ReadGit(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return GITHelpers.ConstructGit(gff);
        }

        public static IFO ReadIfo(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return IFOHelpers.ConstructIfo(gff);
        }

        public static UTC ReadUtc(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return UTCHelpers.ConstructUtc(gff);
        }

        public static PTH ReadPth(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return PTHHelpers.ConstructPth(gff);
        }

        public static UTD ReadUtd(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return UTDHelpers.ConstructUtd(gff);
        }

        public static UTP ReadUtp(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return UTPHelpers.ConstructUtp(gff);
        }

        public static UTS ReadUts(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return UTSHelpers.ConstructUts(gff);
        }

        public static UTI ReadUti(byte[] data)
        {
            var reader = new GFFBinaryReader(data);
            GFF gff = reader.Load();
            return UTIHelpers.ConstructUti(gff);
        }
    }
}
