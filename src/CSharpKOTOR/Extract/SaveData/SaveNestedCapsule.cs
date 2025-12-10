using System.Collections.Generic;
using System.IO;
using CSharpKOTOR.Formats.ERF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Extract.SaveData
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/extract/savedata.py:1567-1848
    // Original: class SaveNestedCapsule
    public class SaveNestedCapsule
    {
        public List<ResourceIdentifier> ResourceOrder { get; } = new List<ResourceIdentifier>();
        public Dictionary<ResourceIdentifier, byte[]> ResourceData { get; } = new Dictionary<ResourceIdentifier, byte[]>();

        private readonly string _path;

        public SaveNestedCapsule(string folderPath)
        {
            _path = Path.Combine(folderPath, "savegame.sav");
        }

        public void Load()
        {
            ResourceOrder.Clear();
            ResourceData.Clear();

            if (!File.Exists(_path))
            {
                return;
            }

            byte[] bytes = File.ReadAllBytes(_path);
            ERF erf = ERFAuto.ReadErf(bytes);
            foreach (var res in erf)
            {
                var ident = new ResourceIdentifier(res.ResRef.ToString(), res.ResType);
                ResourceOrder.Add(ident);
                ResourceData[ident] = res.Data;
            }
        }

        public void Save()
        {
            var erf = new ERF(ERFType.ERF, isSave: true);

            // Insert resources in preserved order
            foreach (var ident in ResourceOrder)
            {
                if (ResourceData.TryGetValue(ident, out var data))
                {
                    erf.SetData(ident.ResName, ident.ResType, data);
                }
            }

            // Include any resources not in ResourceOrder
            foreach (var kvp in ResourceData)
            {
                if (!ResourceOrder.Contains(kvp.Key))
                {
                    erf.SetData(kvp.Key.ResName, kvp.Key.ResType, kvp.Value);
                }
            }

            byte[] bytes = ERFAuto.BytesErf(erf, ResourceType.SAV);
            File.WriteAllBytes(_path, bytes);
        }
    }
}

