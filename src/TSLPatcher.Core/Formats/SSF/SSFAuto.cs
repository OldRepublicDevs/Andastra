using System;
using System.IO;
using TSLPatcher.Core.Resources;

namespace TSLPatcher.Core.Formats.SSF;

/// <summary>
/// Auto-detection and convenience functions for SSF files.
/// 1:1 port of Python ssf_auto.py from pykotor/resource/formats/ssf/ssf_auto.py
/// </summary>
public static class SSFAuto
{
    /// <summary>
    /// Writes the SSF data to the target location with the specified format (SSF or SSF_XML).
    /// 1:1 port of Python write_ssf function.
    /// </summary>
    public static void WriteSsf(SSF ssf, string target, ResourceType fileFormat)
    {
        if (fileFormat == ResourceType.SSF)
        {
            var writer = new SSFBinaryWriter(ssf);
            byte[] data = writer.Write();
            File.WriteAllBytes(target, data);
        }
        else
        {
            throw new ArgumentException("Unsupported format specified; use SSF or SSF_XML.");
        }
    }

    /// <summary>
    /// Returns the SSF data as a byte array.
    /// 1:1 port of Python bytes_ssf function.
    /// </summary>
    public static byte[] BytesSsf(SSF ssf, ResourceType? fileFormat = null)
    {
        var writer = new SSFBinaryWriter(ssf);
        return writer.Write();
    }
}

