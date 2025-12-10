using System;
using System.Collections.Generic;

namespace CSharpKOTOR.Formats.TPC
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/tpc/tpc_data.py:241-315
    // Original: class TPCLayer
    public class TPCLayer : IEquatable<TPCLayer>
    {
        public List<TPCMipmap> Mipmaps { get; set; }

        public TPCLayer()
        {
            Mipmaps = new List<TPCMipmap>();
        }

        public override bool Equals(object obj)
        {
            return obj is TPCLayer other && Equals(other);
        }

        public bool Equals(TPCLayer other)
        {
            if (other == null)
            {
                return false;
            }
            if (Mipmaps.Count != other.Mipmaps.Count)
            {
                return false;
            }
            for (int i = 0; i < Mipmaps.Count; i++)
            {
                if (!Mipmaps[i].Equals(other.Mipmaps[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            foreach (var mm in Mipmaps)
            {
                hash.Add(mm);
            }
            return hash.ToHashCode();
        }
    }
}

