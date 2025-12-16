// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:772-779
// Original: @dataclass class ComparableResource: ...
namespace KotorDiff.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:772-779
    // Original: @dataclass class ComparableResource: ...
    public class ComparableResource
    {
        public string Identifier { get; set; } // e.g. folder/file.ext   or  resref.type inside capsule
        public string Ext { get; set; } // normalized lowercase extension  (for external files) or resource type extension
        public byte[] Data { get; set; }
        public int SourceIndex { get; set; } = 0; // Which path this resource came from (for n-way comparison)

        public ComparableResource(string identifier, string ext, byte[] data)
        {
            Identifier = identifier;
            Ext = ext;
            Data = data;
        }
    }
}

