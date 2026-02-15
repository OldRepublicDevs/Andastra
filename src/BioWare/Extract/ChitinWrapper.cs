using System.Collections.Generic;
using System.Linq;
using BioWare.Common;
using BioWare.Extract.Chitin;
using BioWare.Resource;
using ChitinClass = BioWare.Extract.Chitin.Chitin;

namespace BioWare.Extract
{
    // Thin wrapper matching PyKotor extract.chitin.Chitin semantics (read-only).
    public class ChitinWrapper
    {
        private readonly ChitinClass _chitin;

        public ChitinWrapper(string keyPath, string basePath = null)
        {
            _chitin = new ChitinClass(keyPath, basePath);
        }

        public List<FileResource> Resources => _chitin.ToList();
    }
}
