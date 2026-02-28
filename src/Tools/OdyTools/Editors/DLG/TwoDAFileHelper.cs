using System;
using System.IO;
using BioWare.Resource.Formats.TwoDA;

namespace OdyTools.Editors.DLG
{
    /// <summary>
    /// Loads TwoDA from a file path. Used by the DLG editor when no installation is set (override paths).
    /// </summary>
    public static class TwoDAFileHelper
    {
        /// <summary>
        /// Loads a 2DA from the given file path. Returns null if the file does not exist or loading fails.
        /// </summary>
        public static TwoDA LoadFromPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;
            try
            {
                var reader = new TwoDABinaryReader(filePath);
                return reader.Load();
            }
            catch
            {
                return null;
            }
        }
    }
}
